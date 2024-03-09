using FazUmPix.Exceptions;
using FazUmPix.Models;

namespace FazUmPix.Policies;

public class AccountPolicies
{
    public static void CanModifyAccount(PaymentProviderAccount? account, User user)
    {
        if (account != null && account.UserId != user.Id)
            throw new NoPermissionToModifyAnotherUserAccountException("Account already belongs to another user");
    }
}