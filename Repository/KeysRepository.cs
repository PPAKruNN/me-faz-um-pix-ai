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
            .Where(k => k.Account.Bank.Id == paymentProviderId && k.Account.UserId == userId)
            .CountAsync();

        return count;
    }
            .Include(k => k.Account)
            .Include(k => k.Account.Bank)
            .Include(k => k.Account.User)
            .FirstOrDefaultAsync();

        if (Key == null) throw new PixKeyNotFoundException("Key not found");

        string maskedCpf = UserPolicies.MaskCpf(Key.Account.User.CPF);

        MaskedUserDTO User = new MaskedUserDTO { Name = Key.Account.User.Name, MaskedCpf = maskedCpf };
        PixKeyDTO PixKey = new PixKeyDTO { Type = Key.Type, Value = Key.Value };
        CompleteAccountDTO Account = new CompleteAccountDTO
        {
            BankName = Key.Account.Bank.Name,
            BankId = Key.Account.Bank.Id.ToString(),
            Number = Key.Account.Number.ToString(),
            Agency = Key.Account.Agency.ToString()
        };

        ReadKeyOutputDTO result = new ReadKeyOutputDTO
        {
            Key = PixKey,
            Account = Account,
            User = User
        };

        return result;
    }
}