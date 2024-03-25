
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Text.Json;
using FazUmPix.Data;
using FazUmPix.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using FazUmPix.Exceptions;
using Microsoft.AspNetCore.Http.Extensions;

namespace FazUmPix.Authentication;

public class BearerAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    [Obsolete]
    public BearerAuthenticationHandler(
        IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        ISystemClock clock,
        AppDbContext dbContext
        )
        : base(options, logger, encoder, clock)
    {
        _dbContext = dbContext;
    }
    private readonly AppDbContext _dbContext;

    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {

        if (!Request.Headers.ContainsKey("Authorization"))
        {
            return AuthenticateResult.Fail(new InvalidPaymentProviderException("Authorization header not found"));
        }

        string? authorizationHeader = Request.Headers["Authorization"];

        if (string.IsNullOrEmpty(authorizationHeader))
        {
            return AuthenticateResult.Fail(new InvalidPaymentProviderException("Authorization header is empty"));
        }

        if (!authorizationHeader.StartsWith("Bearer"))
        {
            return AuthenticateResult.Fail(new InvalidPaymentProviderException("Authorization header does not start with 'Bearer'"));
        }

        if (authorizationHeader.Split(" ").Length != 2)
        {
            return AuthenticateResult.Fail(new InvalidPaymentProviderException("Authorization header is invalid"));
        }

        // Remove Bearer from string;
        var successParsing = Guid.TryParse(authorizationHeader.Substring(6), out Guid token);
        if (!successParsing) return AuthenticateResult.Fail(new InvalidPaymentProviderException("Authorization header is invalid"));

        PaymentProvider? paymentProvider = await _dbContext.PaymentProvider.FirstOrDefaultAsync(p => p.Token == token);

        if (paymentProvider == null)
        {
            return AuthenticateResult.Fail(new PaymentProviderNotFoundException("No payment provider found with the given token"));
        }

        var serializedPaymentProvider = JsonSerializer.Serialize(paymentProvider);


        var claims = new[]
        {
            new Claim(ClaimTypes.Authentication, token.ToString()),
            new Claim(ClaimTypes.UserData, serializedPaymentProvider)
        };
        var identity = new ClaimsIdentity(claims, "Bearer");
        var claimsPrincipal = new ClaimsPrincipal(identity);

        return AuthenticateResult.Success(new AuthenticationTicket(claimsPrincipal, "Bearer"));
    }
}
