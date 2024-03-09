using FazUmPix.Data;
using FazUmPix.Models;
using Microsoft.EntityFrameworkCore;

namespace FazUmPix.Repositories;

public class PaymentProviderRepository(AppDbContext context)
{
    public async Task<PaymentProvider?> ReadByToken(Guid token)
    {
        return await context.PaymentProvider
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Token == token);
    }

}