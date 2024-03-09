
using FazUmPix.Data;
using FazUmPix.Models;
using Microsoft.EntityFrameworkCore;

namespace FazUmPix.Repositories;
public class UserRepository(AppDbContext context)
{
    public async Task<User?> ReadByCpf(string cpf)
    {
        var user =
            await context.User
                .Where(u => u.CPF == cpf)
                .FirstOrDefaultAsync();

        return user;
    }
}