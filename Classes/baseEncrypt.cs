using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BCrypt.Net;

namespace PlatnedMahara.Classes
{
    public static class Encrypt
    {
        static Encrypt() { }

        public static string EncryptPassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Password cannot be null or whitespace.", nameof(password));

            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public static bool VerifyPassword(string password, string hashedPassword)
        {
            if (string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(hashedPassword))
                throw new ArgumentException("Arguments cannot be null or whitespace.");

            return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
        }
    }
}
