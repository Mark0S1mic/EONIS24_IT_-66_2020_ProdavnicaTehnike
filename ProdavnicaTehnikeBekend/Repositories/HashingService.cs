using System.Security.Cryptography;

namespace ProdavnicaTehnikeBekend.Repositories
{
   
        public class HashingService
        {
            private const int SaltSize = 16; // 128 bit
            private const int KeySize = 32; // 256 bit

            public string HashPassword(string password)
            {
                using (var algorithm = new Rfc2898DeriveBytes(
                    password,
                    SaltSize,
                    10000,
                    HashAlgorithmName.SHA256))
                {
                    var key = Convert.ToBase64String(algorithm.GetBytes(KeySize));
                    var salt = Convert.ToBase64String(algorithm.Salt);

                    return $"{salt}.{key}";
                }
            }

            public bool VerifyPassword(string hashedPassword, string password)
            {
                var parts = hashedPassword.Split('.', 2);
                if (parts.Length != 2)
                {
                    throw new FormatException("Unexpected hash format. Should be formatted as `{salt}.{passwordHash}`");
                }

                var salt = Convert.FromBase64String(parts[0]);
                var key = Convert.FromBase64String(parts[1]);

                using (var algorithm = new Rfc2898DeriveBytes(
                    password,
                    salt,
                    10000,
                    HashAlgorithmName.SHA256))
                {
                    var keyToCheck = algorithm.GetBytes(KeySize);
                    return keyToCheck.SequenceEqual(key);
                }
            }
        }


    }

