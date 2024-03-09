using System.Text.RegularExpressions;
using fazumpix.Migrations;
using FazUmPix.DTOs;
using FazUmPix.Exceptions;
using FazUmPix.Models;
using FazUmPix.Policies;
using FazUmPix.Repositories;

namespace FazUmPix.Services;

public class KeysService(PaymentProviderAccountRepository paymentProviderAccountRepository, PaymentProviderRepository paymentProviderRepository, UserRepository userRepository, KeysRepository keysRepository)
{
    public async Task<CreateKeyOutputDTO> CreateKey(CreateKeyInputDTO dto, TokenDTO token)
    {
        PaymentProvider? PaymentProvider = await paymentProviderRepository.ReadByToken(token.Token);
        if (PaymentProvider == null) throw new InvalidPaymentProviderException("The payment provider token provided is invalid!");

        PixKey? duplicate = await keysRepository.Read(dto.Key.Type, dto.Key.Value);
        if (duplicate != null) throw new KeyAlreadyExistsException("Key already exists");

        User? user = await userRepository.ReadByCpf(dto.User.CPF);
        if (user == null) throw new UserNotFoundException("User not found");

        // Validate the key values based on Types.
        switch (dto.Key.Type) {
            case "CPF": 
                bool isValidCpf = Regex.IsMatch(dto.Key.Value, @"^\d{11}$");
                if(!isValidCpf) throw new InvalidKeyFormatException("CPF key value must be a valid CPF number (11 digits)");
                
                KeysPolicies.CpfTypeKeyShouldBeUserCpf(dto.Key, user);
                break;

            case "Random":
                bool isValidGuid = Guid.TryParse(dto.Key.Value, out _);
                if(!isValidGuid) throw new InvalidKeyFormatException("Random key value must be a valid GUID");

                break;

            case "Phone":
                bool isValidPhone = Regex.IsMatch(dto.Key.Value, @"^\d{13}$");
                if(!isValidPhone) throw new InvalidKeyFormatException("Phone key value must be a valid phone number (13 digits)");

                break;

            case "Email":
                bool isValidEmail = Regex.IsMatch(dto.Key.Value, @"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$");
                if(!isValidEmail) throw new InvalidKeyFormatException("Email key value must be a valid email");

                break;
        };

        // Mudar o nome desse método:
        // Adicionar validações para cada tipo de Chave.

        PaymentProviderAccount? account = 
            await paymentProviderAccountRepository.ReadByAccountAndProvider(dto.Account, PaymentProvider);
        
        AccountPolicies.CanModifyAccount(account, user);

        int pspKeyCount = await keysRepository.CountByUserAndPaymentProvider(PaymentProvider.Id, user.Id);
        KeysPolicies.MaxKeysPerPaymentProvider(pspKeyCount);

        int userKeyCount = await keysRepository.CountByUser(user.Id);
        KeysPolicies.MaxKeysPerUser(userKeyCount);

        if (account == null) account = await paymentProviderAccountRepository.Create(dto, PaymentProvider, user);

        CreateKeyOutputDTO newKey = await keysRepository.Create(dto, PaymentProvider, user, account);

        return newKey;
    }

        return key;
    }
}

