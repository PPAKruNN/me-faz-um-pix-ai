namespace FazUmPix.Policies;

public class UserPolicies
{
    public static string MaskCpf (string cpf)
    {
        return cpf.Substring(0,3) + ".***.***-" + cpf.Substring(9,2);
    }
}
