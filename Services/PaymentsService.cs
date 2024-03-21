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
    public async Task<Payment> CreatePayment(CreatePaymentInputDTO dto, TokenDTO token, string paymentProviderSerialized)
    {
        // Test by searching for the account (Using ENUMERABLE on the query. Maybe slow?)
        PaymentProviderAccount? destinyAccount = await _paymentProviderAccountRepository.ReadByPixKey(dto.Destiny.Key.Value);
        if (destinyAccount == null) throw new PaymentProviderAccountNotFoundException("Destination account not found!");

        PixKey? pixKey = await _keysRepository.Read(dto.Destiny.Key.Type, dto.Destiny.Key.Value);
        if (pixKey == null) throw new PixKeyNotFoundException("Destination pix key not found!");

        PaymentProvider? paymentProvider = JsonSerializer.Deserialize<PaymentProvider>(paymentProviderSerialized);
        if (paymentProvider == null) throw new UnexpectedMissingPaymentProviderException();

        PaymentProviderAccountIdempotenceKey accountKey = new() { Agency = dto.Origin.Account.Agency, Number = dto.Origin.Account.Number };
        PaymentProviderAccount? originAccount = await _paymentProviderAccountRepository.ReadByAccountAndProvider(accountKey, paymentProvider);
        if (originAccount == null) throw new PaymentProviderAccountNotFoundException("Origin account not found!");

        // Check if user is transfering from x to x.
        var canTransfer = PaymentsPolicies.AccountCannotTransferToItself(originAccount, destinyAccount);
        if (!canTransfer) throw new AccountCannotTransferToItselfException("Account cannot transfer to itself.");

        // Verify if payment is not a duplicate

        // Formas de verificacao de tempo:
        // - Pesquisar pela chave e criar uma politica para verificar a data dentro da API.
        // - Filtrar diretamente no banco pela data (Escolhi essa!).
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

        // Test by searching for the account (Using TWO QUERIES. Maybe faster? It does not use ENUMERABLE 
        // (Should be faster with a large amount of keys on a single PaymentProviderAccount which is a common scenario))
        // PixKey pixKey = await _appDbContext.PixKey.FirstOrDefaultAsync(p => p.Value == dto.Destiny.Key.Value);
        // PaymentProviderAccount paymentProviderAccount = await _appDbContext.PaymentProviderAccount.FirstOrDefaultAsync(p => p.Id == pixKey.PaymentProviderAccountId && p.PaymentProvider.Token == token.Token);

        // Now Create the payment on DB as started.
        Payment payment = await _paymentsRepository.CreatePayment(dto, destinyAccount, originAccount, pixKey);

        // Now process the payment
        ProcessPaymentDTO processPaymentDTO = new()
        {
            ProcessURL = paymentProvider.ProcessingWebhook,
            AcknowledgeURL = paymentProvider.AcknowledgeWebhook,
            PaymentId = payment.Id,
            Data = dto
        };
        _queueService.PublishPayment(processPaymentDTO);

        // Return the new payment data to the controller
        return payment;

    }
    public async Task UpdatePayment(UpdatePaymentInputDTO dto, RouteIdDTO idDTO)
    {
        Payment? payment = await _paymentsRepository.Read(idDTO.Id);
        if (payment == null) throw new PaymentNotFoundException("Payment not found!");

        payment.Status = dto.Status;
        await _paymentsRepository.Update(payment);
    }

    public async Task Concilliation(ConcilliationInputDTO dto)
    {
        // Process the file and update the payments
        _queueService.PublishConcilliation(dto);

    }

}