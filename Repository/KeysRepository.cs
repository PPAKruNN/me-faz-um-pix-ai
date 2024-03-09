using FazUmPix.Data;
using FazUmPix.DTOs;
using FazUmPix.Exceptions;
using FazUmPix.Policies;
using Microsoft.EntityFrameworkCore;

namespace FazUmPix.Repositories;

public class KeysRepository(AppDbContext context)
{
    public async Task<ReadKeyOutputDTO> Read(ReadKeyInputDTO dto)
    {
        var Key =
        await context.PixKey
            .Where(k => k.Type == dto.Type && k.Value == dto.Value)
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