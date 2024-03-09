
using FazUmPix.Data;
using FazUmPix.DTOs;
using FazUmPix.Exceptions;
using FazUmPix.Models;

namespace FazUmPix.Policies;

public class KeysPolicies
{
    public static void CpfTypeKeyShouldBeUserCpf(PixKeyDTO keyDTO, User user)
    {
        if (keyDTO.Type == "CPF")
        {
            if (keyDTO.Value != user.CPF)
                throw new InvalidCpfException("CPF key must be equal to user's CPF");
        }
    }

    public static void MaxKeysPerPaymentProvider(int pspKeyCount)
    {
        if (pspKeyCount >= 5) throw new ReachedKeyLimitException("User already has 5 keys in this psp");
    }

    public static void MaxKeysPerUser(int userKeyCount)
    {
        if (userKeyCount >= 20) throw new ReachedKeyLimitException("User have reached the limit of 20 keys per User");
    }
    
}