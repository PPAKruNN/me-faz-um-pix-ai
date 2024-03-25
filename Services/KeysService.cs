using System.Text.RegularExpressions;
using FazUmPix.DTOs;
using FazUmPix.Exceptions;
using FazUmPix.Models;
using FazUmPix.Policies;
using FazUmPix.Repositories;

namespace FazUmPix.Services;

public class KeysService(PaymentProviderAccountRepository paymentProviderAccountRepository, UserRepository userRepository, KeysRepository keysRepository)
{
    private readonly PaymentProviderAccountRepository _paymentProviderAccountRepository = paymentProviderAccountRepository;
    private readonly UserRepository _userRepository = userRepository;
    private readonly KeysRepository _keysRepository = keysRepository;

    private static void ValidateKeyFormat(PixKeyDTO key)
    {
        switch (key.Type)
        {
            case "CPF":
                bool isValidCpf = Regex.IsMatch(key.Value, @"^\d{11}$");
                if (!isValidCpf) throw new InvalidKeyFormatException("CPF key value must be a valid CPF number (11 digits)");

                break;

            case "Random":
                bool isValidGuid = Guid.TryParse(key.Value, out _);
                if (!isValidGuid) throw new InvalidKeyFormatException("Random key value must be a valid GUID");

                break;

            case "Phone":
                bool isValidPhone = Regex.IsMatch(key.Value, @"^\d{13}$");
                if (!isValidPhone) throw new InvalidKeyFormatException("Phone key value must be a valid phone number (13 digits)");

                break;

            case "Email":
                bool isValidEmail = Regex.IsMatch(key.Value, @"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$");
                if (!isValidEmail) throw new InvalidKeyFormatException("Email key value must be a valid email");

                break;
        }
    }

    public async Task<CreateKeyOutputDTO> CreateKey(CreateKeyInputDTO dto, PaymentProvider paymentProvider)
    {
        // Starting User and PixKey checking.
        var user = await _userRepository.ReadByCpf(dto.User.CPF);
        if (user is null) throw new UserNotFoundException("User not found");

        var duplicatePixKey = await _keysRepository.Read(dto.Key.Type, dto.Key.Value);
        if (duplicatePixKey is not null) throw new KeyAlreadyExistsException("Key already exists");

        // Keys and Account validations
        ValidateKeyFormat(dto.Key);
        if (dto.Key.Type == "CPF") KeysPolicies.CpfTypeKeyShouldBeUserCpf(dto.Key, user);

        PaymentProviderAccountIdempotenceKey accountKey = new() { Agency = dto.Account.Agency, Number = dto.Account.Number };
        PaymentProviderAccount? account =
            await _paymentProviderAccountRepository.ReadByAccountAndProvider(accountKey, paymentProvider);

        AccountPolicies.CanUserModifyThisAccount(account, user);

        // Key generation limit policies.
        var pspKeyCount = await _keysRepository.CountByUserAndPaymentProvider(paymentProvider.Id, user.Id);
        KeysPolicies.MaxKeysPerPaymentProvider(pspKeyCount);

        var userKeyCount = await _keysRepository.CountByUser(user.Id);
        KeysPolicies.MaxKeysPerUser(userKeyCount);

        // Create key (and account if needed)
        if (account is null) account = await _paymentProviderAccountRepository.Create(dto, paymentProvider, user);
        CreateKeyOutputDTO newKey = await _keysRepository.Create(dto, paymentProvider, user, account);
        return newKey;
    }

    public async Task<ReadKeyOutputDTO> ReadKey(ReadKeyInputDTO dto, TokenDTO token)
    {
        PixKey? key = await _keysRepository.ReadByKeyAndProviderIncludingBankAndUser(token.Token, dto.Type, dto.Value);
        if (key is null) throw new PixKeyNotFoundException("Pix key not found");

        var maskedCpf = UserPolicies.MaskCpf(key.Account.User.CPF);

        MaskedUserDTO user = new MaskedUserDTO { Name = key.Account.User.Name, MaskedCpf = maskedCpf };
        PixKeyDTO pixKey = new PixKeyDTO { Type = key.Type, Value = key.Value };
        CompleteAccountDTO account = new CompleteAccountDTO
        {
            BankName = key.Account.PaymentProvider.Name,
            BankId = key.Account.PaymentProvider.Token.ToString(),
            Number = key.Account.Number.ToString(),
            Agency = key.Account.Agency.ToString()
        };

        ReadKeyOutputDTO resultOutput = new ReadKeyOutputDTO
        {
            Key = pixKey,
            Account = account,
            User = user
        };

        return resultOutput;
    }
}

