using System.Runtime.CompilerServices;
using System.Security;
using System.Text.Json;
using FazUmPix.DTOs;
using FazUmPix.Exceptions;
using FazUmPix.Models;
using FazUmPix.Policies;
using FazUmPix.Repositories;

namespace FazUmPix.Services;

public class PaymentsService(PaymentsRepository paymentsRepository, QueueService queueService, PaymentProviderAccountRepository paymentProviderAccountRepository, KeysRepository keysRepository)
{
    private readonly PaymentsRepository _paymentsRepository = paymentsRepository;
    private readonly QueueService _queueService = queueService;
    private readonly PaymentProviderAccountRepository _paymentProviderAccountRepository = paymentProviderAccountRepository;
    private readonly KeysRepository _keysRepository = keysRepository;
    private async Task<PaymentProviderAccount> GetDestinyAccount(DestinyDTO destiny)
    {
        PaymentProviderAccount? paymentProviderAccount = await _paymentProviderAccountRepository.ReadByPixKey(destiny.Key);
        if (paymentProviderAccount is null) throw new PaymentProviderAccountNotFoundException("Destination account not found!");

        return paymentProviderAccount;
    }
    private async Task<PaymentProviderAccount> GetOriginAccount(PaymentProvider paymentProvider, OriginDTO origin)
    {
        PaymentProviderAccountIdempotenceKey accountKey = new() { Agency = origin.Account.Agency, Number = origin.Account.Number };
        PaymentProviderAccount? originAccount = await _paymentProviderAccountRepository.ReadByAccountAndProvider(accountKey, paymentProvider);

        if (originAccount is null) throw new PaymentProviderAccountNotFoundException("Origin account not found!");

        return originAccount;
    }
    private async Task<PixKey> GetDestinyPixKey(PixKeyDTO key)
    {
        PixKey? pixKey = await _keysRepository.Read(key.Type, key.Value);
        if (pixKey is null) throw new PixKeyNotFoundException("Destination pix key not found!");

        return pixKey;
    }
    private async Task VerifyIfPaymentIsDuplicate(CreatePaymentInputDTO dto)
    {
        PaymentIdempotenceKey paymentIdempotenceKey = new()
        {
            Amount = dto.Amount,
            PixKeyValue = dto.Destiny.Key.Value,
            Account = new PaymentProviderAccountIdempotenceKey()
            {
                Agency = dto.Origin.Account.Agency,
                Number = dto.Origin.Account.Number
            }
        };

        var dateCap = PaymentsPolicies.GetDateCapOfPaymentDuplicationCheck();

        bool isRepeatedPayment = await _paymentsRepository.CheckForDuplicate(paymentIdempotenceKey, dateCap);
        if (isRepeatedPayment) throw new PaymentRepeatedException("Payment already exists! Maybe you are trying to pay the same thing twice?");
    }

    public async Task<CreatePaymentOutputDTO> CreatePayment(CreatePaymentInputDTO dto, PaymentProvider paymentProvider)
    {
        var destinyAccount = await GetDestinyAccount(dto.Destiny);
        var pixKey = await GetDestinyPixKey(dto.Destiny.Key);
        var originAccount = await GetOriginAccount(paymentProvider, dto.Origin);

        // Check if user is transfering from x to x.
        var canTransfer = PaymentsPolicies.AccountCannotTransferToItself(originAccount, destinyAccount);
        if (!canTransfer) throw new AccountCannotTransferToItselfException("Account cannot transfer to itself.");

        await VerifyIfPaymentIsDuplicate(dto);

        // Now the payment.
        Payment payment = await _paymentsRepository.CreatePayment(dto, destinyAccount, originAccount, pixKey);
        _queueService.PublishPayment(new ProcessPaymentDTO
        {
            ProcessURL = paymentProvider.ProcessingWebhook,
            AcknowledgeURL = paymentProvider.AcknowledgeWebhook,
            PaymentId = payment.Id,
            Data = dto
        });

        return new CreatePaymentOutputDTO
        {
            Id = payment.Id,
            Status = payment.Status,
            Amount = payment.Amount,
            Description = payment.Description,
            Destination = new DestinationPaymentProviderAccountDTO
            {
                Agency = payment.DestinationPaymentProviderAccount.Agency,
                Number = payment.DestinationPaymentProviderAccount.Number,
                PaymentProviderToken = paymentProvider.Token
            },
            PixKey = new PixKeyDTO
            {
                Value = payment.PixKey.Value,
                Type = payment.PixKey.Type
            },
            CreatedAt = payment.CreatedAt,
            UpdatedAt = payment.UpdatedAt
        };
    }

    public async Task UpdatePayment(UpdatePaymentInputDTO dto, RouteIdDTO idDTO)
    {
        Payment? payment = await _paymentsRepository.Read(idDTO.Id);
        if (payment == null) throw new PaymentNotFoundException("Payment not found!");

        payment.Status = dto.Status;
        await _paymentsRepository.Update(payment);
    }

    public async Task CreatePaymentsConcilliation(ConcilliationInputDTO dto, PaymentProvider paymentProvider)
    {
        QueueConcilliationInputDTO queueDTO = new()
        {
            Date = dto.Date,
            File = dto.File,
            Postback = dto.Postback,
            PaymentProviderId = (int)paymentProvider.Id
        };

        // Process the file and update the payments
        _queueService.PublishConcilliation(queueDTO);
    }

}