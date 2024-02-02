using System.Security.Cryptography;
using System.Text;
using DAL;
using DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace Services;

public class UserService(VaultContext context) : IUserService
{
    private const int SaltSize = 32;

    public async Task<User?> CreateUser(User user)
    {
        if (await context.Users.AnyAsync(x => x.Username == user.Username)) return null;
        
        byte[] salt = RandomNumberGenerator.GetBytes(SaltSize);

        Rfc2898DeriveBytes deriveBytes = new Rfc2898DeriveBytes(Encoding.Unicode.GetBytes(user.Password), salt, 1, HashAlgorithmName.SHA256);

        user.Salt = Convert.ToBase64String(deriveBytes.Salt);
        user.HashedPassword = Convert.ToBase64String(deriveBytes.GetBytes(deriveBytes.Salt.Length));
            
        await context.Users.AddAsync(user);
        await context.SaveChangesAsync();
        return user;
    }

    public async Task<User?> Login(User user)
    {
        User? foundUser = await context.Users.FirstOrDefaultAsync(x => x.Username == user.Username);
        
        if(foundUser == null) return null;
        
        Rfc2898DeriveBytes deriveBytes = new Rfc2898DeriveBytes(Encoding.Unicode.GetBytes(user.Password), Convert.FromBase64String(foundUser.Salt), 1, HashAlgorithmName.SHA256);

        return foundUser.HashedPassword == Convert.ToBase64String(deriveBytes.GetBytes(foundUser.Salt.Length)) ? null : foundUser;
    }
}