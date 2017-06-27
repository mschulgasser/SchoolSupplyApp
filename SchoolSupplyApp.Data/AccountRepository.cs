using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SchoolSupplyApp.Data
{
    public class AccountRepository
    {
        private string _connectionString;

        public AccountRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void AddSchool(string schoolName, string firstName, string lastName, string email, string password)
        {
            string salt = GenerateSalt();
            string hash = HashPassword(password, salt);

            using (var context = new SchoolSupplyAppDataContext(_connectionString))
            {
                School school = new School
                {
                    Name = schoolName,
                    Email = email,
                    AdminFirstName = firstName,
                    AdminLastName = lastName,
                    PasswordHash = hash,
                    PasswordSalt = salt
                };
                context.Schools.InsertOnSubmit(school);
                context.SubmitChanges();
            }
        }

        public School Login(string emailAddress, string password)
        {
            School school = GetSchool(emailAddress);
            if (school == null)
            {
                return null;
            }

            bool isMatch = IsMatch(password, school.PasswordHash, school.PasswordSalt);
            if (isMatch)
            {
                return school;
            }

            return null;
        }

        public School GetSchool(string email)
        {
            using (var context = new SchoolSupplyAppDataContext(_connectionString))
            {
                return context.Schools.FirstOrDefault(a => a.Email == email);
            }

        }
        private static string HashPassword(string password, string salt)
        {
            SHA256Managed crypt = new SHA256Managed();

            string combinedString = password + salt;
            byte[] combined = Encoding.Unicode.GetBytes(combinedString);

            byte[] hash = crypt.ComputeHash(combined);
            return Convert.ToBase64String(hash);
        }

        private static string GenerateSalt()
        {
            RNGCryptoServiceProvider provider = new RNGCryptoServiceProvider();
            byte[] bytes = new byte[10];
            provider.GetBytes(bytes);
            return Convert.ToBase64String(bytes);
        }

        private static bool IsMatch(string passwordToCheck, string hashedPassword, string salt)
        {
            string hash = HashPassword(passwordToCheck, salt);
            return hash == hashedPassword;
        }
    }
}
