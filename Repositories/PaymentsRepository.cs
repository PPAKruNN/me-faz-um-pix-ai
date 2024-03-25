using FazUmPix.Data;
using FazUmPix.DTOs;
using FazUmPix.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace FazUmPix.Repositories;

public class PaymentsRepository(AppDbContext appDbContext)
{
    private readonly AppDbContext _context = appDbContext;

    public async Task<Payment> CreatePayment(CreatePaymentInputDTO payment, PaymentProviderAccount destinationAccount, PaymentProviderAccount originAccount, PixKey pixKey)
    {
        Payment newPayment = new()
        {
            Amount = payment.Amount,
            Description = payment.Description,
            Status = "PROCESSING",

            PixKey = pixKey,
            PixKeyId = pixKey.Id,

            OriginPaymentProviderAccountId = originAccount.Id,
            OriginPaymentProviderAccount = originAccount,

            DestinationPaymentProviderAccountId = destinationAccount.Id,
            DestinationPaymentProviderAccount = destinationAccount,
        };


        await _context.Payment.AddAsync(newPayment);
        await _context.SaveChangesAsync();

        return newPayment;
    }

    public async Task<Payment?> Read(uint id)
    {
        return await _context.Payment.FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task Update(Payment payment)
    {
        _context.Payment.Update(payment);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> CheckForDuplicate(PaymentIdempotenceKey paymentIdempotenceKey, DateTime dateCap)
    {
        Payment? payment =
        await _context.Payment
            .Where(p => p.Amount == paymentIdempotenceKey.Amount
                && p.PixKey.Value == paymentIdempotenceKey.PixKeyValue
                && p.OriginPaymentProviderAccount.Agency.ToString() == paymentIdempotenceKey.Account.Agency
                && p.OriginPaymentProviderAccount.Number.ToString() == paymentIdempotenceKey.Account.Number
                && p.CreatedAt > dateCap
            )
            .FirstOrDefaultAsync();

        return payment != null;
    }


}

