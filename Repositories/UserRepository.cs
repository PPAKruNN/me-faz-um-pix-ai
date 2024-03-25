
using FazUmPix.Data;
using FazUmPix.Exceptions;
using FazUmPix.Models;
using Microsoft.EntityFrameworkCore;

namespace FazUmPix.Repositories;
public class UserRepository(AppDbContext context)
{
    private readonly AppDbContext _context = context;
    public async Task<User?> ReadByCpf(string cpf)
    {
        var user =
            await _context.User
                .Where(u => u.CPF == cpf)
                .FirstOrDefaultAsync();

        return user;
    }
}