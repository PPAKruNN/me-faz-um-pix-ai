using FazUmPix.Data;
using FazUmPix.DTOs;
using FazUmPix.Models;
using Microsoft.EntityFrameworkCore;

namespace FazUmPix.Repositories;

public class KeysRepository(AppDbContext context)
{
    public async Task<PixKey?> Read(string type, string value)
    {
        var Key =
        await context.PixKey
            .Where(k => k.Type == type && k.Value == value)
            .FirstOrDefaultAsync();

        return Key;
    }

    public async Task<int> CountByUser(uint userId)
    {
        var count = await context.PixKey
            .Where(k => k.Account.UserId == userId)
            .CountAsync();

        return count;
    }

    public async Task<int> CountByUserAndPaymentProvider(uint paymentProviderId, uint userId)
    {
        var count = await context.PixKey
            .Where(k => k.Account.PaymentProvider.Id == paymentProviderId && k.Account.UserId == userId)
            .CountAsync();

        return count;
    }


    public async Task<PixKey?> ReadByKeyAndProviderIncludingBankAndUser(Guid paymentProviderToken, string type, string value)
    {
        var Key =
        await context.PixKey
            .Where(k => k.Type == type
            && k.Value == value
            && k.Account.PaymentProvider.Token == paymentProviderToken)
            .Include(k => k.Account)
            .Include(k => k.Account.PaymentProvider)
            .Include(k => k.Account.User)
            .FirstOrDefaultAsync();

        return Key;
    }

    public async Task<CreateKeyOutputDTO> Create(CreateKeyInputDTO dto, PaymentProvider psp, User user, PaymentProviderAccount account)
    {

        PixKey newKey = new PixKey
        {
            Account = account,
            PaymentProviderAccountId = account.Id,
            Type = dto.Key.Type,
            Value = dto.Key.Value
        };

        await context.PixKey.AddAsync(newKey);
        await context.SaveChangesAsync();

        CreateKeyOutputDTO output = new CreateKeyOutputDTO
        {
            Type = newKey.Type,
            Value = newKey.Value
        };


        return output;
    }
}