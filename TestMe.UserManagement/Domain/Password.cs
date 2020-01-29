using System;
using System.Security.Cryptography;
using TestMe.BuildingBlocks.Domain;

namespace TestMe.UserManagement.Domain
{
    public sealed class Password
    {
        private const int NumberOfIterations = 7717;
        public const int MaxLength = 66;
        public const int MinLength = 8;

        public string Hash { get; private set; }
        public string Salt { get; private set; }

       
        private Password(string hash, string salt)
        {
            Hash = hash;
            Salt = salt;
        }


        public bool VerifyPassword(string password)
        {
            byte[] salt = Convert.FromBase64String(Salt);
            var rfc2898DeriveBytes = new Rfc2898DeriveBytes(password, salt: salt, iterations: NumberOfIterations);
            byte[] hash = rfc2898DeriveBytes.GetBytes(20);
            string hashBase64 = Convert.ToBase64String(hash);
            return Hash == hashBase64;
        }
        public static Password Create(string password)
        {
            if (IsPasswordValid(password))
            {
                var (hash, salt) = HashPassword(password);
                return new Password(hash, salt);
            }
            throw new DomainException(DomainExceptions.Password_is_invalid);
        }


        private static bool IsPasswordValid(string password)
        {
            return password.Length >= MinLength;
        }
        private static (string hash, string salt) HashPassword(string password)
        {
            var rfc2898DeriveBytes = new Rfc2898DeriveBytes(password, saltSize: 32, iterations: NumberOfIterations);           
            byte[] hash = rfc2898DeriveBytes.GetBytes(20);
            byte[] salt = rfc2898DeriveBytes.Salt;
            return (Convert.ToBase64String(hash), Convert.ToBase64String(salt));
        }
    }
}