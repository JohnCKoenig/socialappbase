using Konscious.Security.Cryptography;
using System.Security.Cryptography;
using System.Text;

namespace MobileAppAPI.Helpers
{
    public class PasswordHelper
    {
        static public string HashPassword(string password)
        {

            byte[] salt = new byte[16];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            var argon2 = new Argon2id(Encoding.UTF8.GetBytes(password))
            {
                Salt = salt,
                Iterations = 2,
                MemorySize = 4096,
                DegreeOfParallelism = 1
            };

            byte[] hash = argon2.GetBytes(32);


            byte[] result = new byte[16 + 32];
            Array.Copy(salt, 0, result, 0, 16);
            Array.Copy(hash, 0, result, 16, 32);

            return Convert.ToBase64String(result);
        }
        static public bool VerifyPassword(string enteredPassword, string hashedPassword)
        {
            byte[] hashBytes = Convert.FromBase64String(hashedPassword);
            byte[] salt = new byte[16];
            Array.Copy(hashBytes, 0, salt, 0, 16);

            var argon2 = new Argon2id(Encoding.UTF8.GetBytes(enteredPassword))
            {
                Salt = salt,
                Iterations = 2,
                MemorySize = 4096,
                DegreeOfParallelism = 1
            };

            byte[] newHash = argon2.GetBytes(32);

            for (int i = 0; i < 32; i++)
            {
                if (hashBytes[i + 16] != newHash[i])
                {
                    return false;
                }
            }

            return true;
        }

    }
}
