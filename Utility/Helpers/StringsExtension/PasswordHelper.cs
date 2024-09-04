using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helpers.StringsExtension
{
    using System;
    using System.Security.Cryptography;
    using System.Text;

    public static class PasswordHelper
    {
        
        public static (string Hash, string Salt) CreateHashAndSalt(this string password)
        {
            
            byte[] saltBytes = new byte[16];
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(saltBytes);
            }
            string salt = Convert.ToBase64String(saltBytes);

            
            string hash = HashPassword(password, salt);
            return (hash, salt);
        }

        private static string HashPassword(string password, string salt)
        {
            byte[] saltBytes = Convert.FromBase64String(salt);

            using (var pbkdf2 = new Rfc2898DeriveBytes(password, saltBytes, 10000))
            {
                byte[] hashBytes = pbkdf2.GetBytes(20);
                byte[] hashBytesWithSalt = new byte[36];
                Array.Copy(saltBytes, 0, hashBytesWithSalt, 0, 16);
                Array.Copy(hashBytes, 0, hashBytesWithSalt, 16, 20);
                return Convert.ToBase64String(hashBytesWithSalt);
            }
        }
        public static bool VerifyPassword(string password, string hash, string salt)
        {
           
            byte[] hashBytesWithSalt = Convert.FromBase64String(hash);
            byte[] saltBytes = new byte[16];
            Array.Copy(hashBytesWithSalt, 0, saltBytes, 0, 16);

            
            using (var pbkdf2 = new Rfc2898DeriveBytes(password, saltBytes, 10000))
            {
                byte[] hashBytes = pbkdf2.GetBytes(20);

               
                for (int i = 0; i < 20; i++)
                {
                    if (hashBytesWithSalt[i + 16] != hashBytes[i])
                    {
                        return false;
                    }
                }
                return true;
            }
        }
    }

}
