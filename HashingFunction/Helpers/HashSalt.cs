using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace HashingFunction
{
    class HashSalt
    {
        public static string[] CreateHash(string password)
        {
            SHA256 sha256 = SHA256.Create();
            
            string salt = Salt();
            byte[] hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(password + salt));
            
            StringBuilder stringBuilder = new StringBuilder();
            for(int i = 0; i < hash.Length; i++)
            {
                stringBuilder.Append(hash[i].ToString("x2"));
            }

            return new string[] { stringBuilder.ToString(), salt };
        }

        public static string GetHash(string password, string salt)
        {
            SHA256 sha256 = SHA256.Create();

            byte[] hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(password + salt));

            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                stringBuilder.Append(hash[i].ToString("x2"));
            }

            return stringBuilder.ToString();
        }

        private static string Salt()
        {
            RandomNumberGenerator rng = new RNGCryptoServiceProvider();
            byte[] salt = new byte[32];

            rng.GetBytes(salt);

            return Convert.ToBase64String(salt);
        }
    }
}
