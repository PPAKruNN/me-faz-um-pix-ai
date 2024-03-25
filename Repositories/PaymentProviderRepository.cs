using FazUmPix.Data;
using FazUmPix.Models;
using Microsoft.EntityFrameworkCore;

namespace FazUmPix.Repositories;

public class PaymentProviderRepository(AppDbContext context)
{
    private readonly AppDbContext _context = context;

    public async Task<PaymentProvider?> ReadByToken(Guid token)
    {
        return await _context.PaymentProvider
            .FirstOrDefaultAsync(p => p.Token == token);
    }

}