using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace Helpers
{
    public class PasswordHasher
    {

        /// <summary>
        /// //steps for creating hash for passwords
        /// 1- generate random salt
        /// 2- add the random salt to fixed salt 
        /// 3- generate hash with HMACSHA512
        /// </summary>
        public static readonly string _fixedSalt = ")!@#^%$";

        public static string GenerateHash(string password, string randomSalt = "")
        {
            if (string.IsNullOrEmpty(randomSalt))
                randomSalt = GenerateRandomSalt();
            var salt = Encoding.UTF8.GetBytes(_fixedSalt + randomSalt);
            var valueBytes = KeyDerivation.Pbkdf2(
                password,
                salt,
                    KeyDerivationPrf.HMACSHA512,
                    1000,
                    32
                );
            return Convert.ToBase64String(valueBytes);
        }
        public static string GenerateRandomSalt()
        {
            byte[] bytes = new byte[32];
            var generator = RandomNumberGenerator.Create();
            generator.GetBytes(bytes);
            return Convert.ToBase64String(bytes);
        }
        public static bool IsHashMatched(string password,string randomSalt,string hash)
        {
            string hashedPassword = GenerateHash(password, randomSalt);
            return hash == hashedPassword;
        }
        public string GenerateHashForTesting(string password, string randomSalt = "")
        {
            if (string.IsNullOrEmpty(randomSalt))
                randomSalt = GenerateRandomSalt();
            var salt = Encoding.UTF8.GetBytes(_fixedSalt + randomSalt);
            var valueBytes = KeyDerivation.Pbkdf2(
                password,
                salt,
                    KeyDerivationPrf.HMACSHA512,
                    1000,
                    32
                );
            return Convert.ToBase64String(valueBytes);
        }
        public bool IsHashMatchedForTesting(string password, string randomSalt, string hash)
        {
            string hashedPassword = GenerateHash(password, randomSalt);
            return hash == hashedPassword;
        }
    }
}
