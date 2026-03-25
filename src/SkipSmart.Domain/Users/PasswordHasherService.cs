using System.Security.Cryptography;
using System.Text;

namespace SkipSmart.Domain.Users;

public class PasswordHasherService {
    public string ComputeHash(string password, string salt, string pepper, int iteration) {
        if (iteration <= 0) return password;
        
        using var sha256 = SHA256.Create();
        var passwordSaltPepper = $"{salt}{password}{pepper}";
        var byteValue = Encoding.UTF8.GetBytes(passwordSaltPepper);
        var byteHash = sha256.ComputeHash(byteValue);
        var hash = Convert.ToBase64String(byteHash);
        
        return ComputeHash(hash, salt, pepper, iteration - 1);
    }
    
    public string GenerateSalt() {
        using var rng = RandomNumberGenerator.Create();
        var byteSalt = new byte[16];
        rng.GetBytes(byteSalt);
        var salt = Convert.ToBase64String(byteSalt);
        return salt;
    } 
}