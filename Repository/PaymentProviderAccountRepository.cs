using FazUmPix.Data;
using FazUmPix.DTOs;
using FazUmPix.Models;
using Microsoft.EntityFrameworkCore;

namespace FazUmPix.Repositories;

public class PaymentProviderAccountRepository(AppDbContext context)
{
    private readonly AppDbContext _context = context;

    public async Task<PaymentProviderAccount?> ReadByAccountAndProvider(PaymentProviderAccountIdempotenceKey account, PaymentProvider psp)
    {
        var accountNumber = int.Parse(account.Number);
        var accountAgency = int.Parse(account.Agency);

        return await _context.PaymentProviderAccount
            .Where(
                a => a.Agency == accountAgency
                && a.Number == accountNumber
                && a.PaymentProviderId == psp.Id
            )
            .FirstOrDefaultAsync();
    }

    public async Task<PaymentProviderAccount> Create(CreateKeyInputDTO dto, PaymentProvider bank, User user)
    {
        PaymentProviderAccount account =
            new PaymentProviderAccount
            {
                Agency = int.Parse(dto.Account.Agency),
                Number = int.Parse(dto.Account.Number),
                UserId = user.Id,
                PaymentProviderId = bank.Id,
                PaymentProvider = bank,
                User = user
            };

        await _context.PaymentProviderAccount.AddAsync(account);
        await _context.SaveChangesAsync();

        return account;
    }

    public async Task<PaymentProviderAccount?> ReadByPixKey(string key)
    {
        PaymentProviderAccount? paymentProviderAccount = await _context.PaymentProviderAccount
            .Where(p => p.PixKeys != null
                    && p.PixKeys.Any(p => p.Value == key))
            .FirstOrDefaultAsync(a => a.PixKeys != null && a.PixKeys.Any(p => p.Value == key));

        return paymentProviderAccount;
    }


}