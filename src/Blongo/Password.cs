using System;
using System.Security.Cryptography;
using System.Text;

namespace Blongo
{
    public class Password
    {
        public Password(string password, string passwordSalt)
        {
            HashedPassword = GenerateHashedPassword(password, passwordSalt);
            PasswordSalt = PasswordSalt;
        }

        public static string GenerateHashedPassword(string password, string passwordSalt)
        {
            using (var sha256 = SHA256.Create())
            {
                var computedHash = sha256.ComputeHash(Encoding.Unicode.GetBytes(ConstantSalt + password + passwordSalt));

                return Convert.ToBase64String(computedHash);
            }
        }

        public static string ConstantSalt { get; set; }

        public string HashedPassword { get; }

        public string PasswordSalt { get; }
    }
}
