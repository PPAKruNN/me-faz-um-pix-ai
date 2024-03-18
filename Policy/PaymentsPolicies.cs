using FazUmPix.Models;

namespace FazUmPix.Policies;

public class PaymentsPolicies
{
    public static bool AccountCannotTransferToItself(PaymentProviderAccount origin, PaymentProviderAccount destiny)
    {
        bool isSame = origin.Agency == destiny.Agency
                    && origin.Number == destiny.Number
                    && origin.PaymentProviderId == destiny.PaymentProviderId;

        return !isSame;
    }

    public static DateTime GetDateCapOfPaymentDuplicationCheck()
    {
        return DateTime.UtcNow.AddSeconds(-30);
    }


}