using FazUmPix.Data;
using FazUmPix.DTOs;
using FazUmPix.Models;
using Microsoft.EntityFrameworkCore;

namespace FazUmPix.Repositories;

public class PaymentProviderAccountRepository(AppDbContext context)
{

    public async Task<PaymentProviderAccount?> ReadByAccountAndProvider(AccountDTO account, PaymentProvider psp)
    {
        return await context.PaymentProviderAccount
            .FirstOrDefaultAsync(
                a => a.PaymentProviderId == psp.Id
                  && a.Number.ToString() == account.Number
                  && a.Agency.ToString() == account.Agency
            );
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

        await context.PaymentProviderAccount.AddAsync(account);
        await context.SaveChangesAsync();

        return account;
    }


}