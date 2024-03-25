
using System.Security.Claims;
using System.Text.Json;
using FazUmPix.DTOs;
using FazUmPix.Exceptions;
using FazUmPix.Models;

public static class AuthReader
{
    public static TokenDTO GetToken(HttpContext httpContext)
    {
        Guid token = Guid.Parse(httpContext.User.Claims.First(c => c.Type == ClaimTypes.Authentication).Value);
        TokenDTO dto = new() { Token = token };

        return dto;
    }

    public static string GetPaymentProviderSerialized(HttpContext httpContext)
    {
        return httpContext.User.Claims.First(c => c.Type == ClaimTypes.UserData).Value;
    }

    public static PaymentProvider GetPaymentProvider(HttpContext httpContext)
    {
        PaymentProvider? paymentProvider = JsonSerializer.Deserialize<PaymentProvider>(GetPaymentProviderSerialized(httpContext));
        if (paymentProvider is null) throw new UnexpectedMissingPaymentProviderException();

        return paymentProvider;
    }
}
