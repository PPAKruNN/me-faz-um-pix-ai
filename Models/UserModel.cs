using System.Security.Cryptography;

namespace FazUmPix.Models;

public class User {
    public required string CPF { get; set; }
    public required string Name { get; set; }
    public User(string CPF, string Name) {
        this.CPF = CPF;
        this.Name = Name;
    }
}