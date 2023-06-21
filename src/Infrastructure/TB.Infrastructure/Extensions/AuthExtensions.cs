using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.Security.Cryptography;
using System.Text;
//using System.Xml;

namespace TB.Infrastructure.Extensions
{
    public static class AuthExtensions
    {
        public static IConfiguration? configuration { get; private set; }
        private static Random random = new Random();
        public static SymmetricSecurityKey? signKey { get; set; }
        public static double? clockSkew { get; set; }
        public static string? savedToken { get; set; }

        public static void AddOrUpdateAppSetting(byte[] value)
        {
            try
            {
                var filePath = Path.Combine(AppContext.BaseDirectory, "appsettings.json");
                string json = File.ReadAllText(filePath);
                var jsonObj = JsonConvert.DeserializeObject(json);
                string output = JsonConvert.SerializeObject(jsonObj, Formatting.Indented);

                SetValue(value);

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error writing app settings | {0}", ex.Message);
            }
        }

        private static void SetValue(byte[] value)
        {
            var securityKey = new SymmetricSecurityKey(value).KeyId;
            configuration!["Auth:Jwt:JwtSecurityKey"] = securityKey;

        }

        public static void Hash(string key, out byte[] keyHash, out byte[] keySalt)
        {
            using (var hmac = new HMACSHA512())
            {
                keySalt = hmac.Key;
                keyHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(key));
            }
        }

        public static void AesGenerateKey(out string keyBase64)
        {
            using (Aes aesAlgorithm = Aes.Create())
            {
                aesAlgorithm.KeySize = 256;
                aesAlgorithm.GenerateKey();
                keyBase64 = Convert.ToBase64String(aesAlgorithm.Key);

            }
        }

        public static string AesEncryptKey(string plainText, string keyBase64, out string vectorBase64)
        {
            using (Aes aesAlgorithm = Aes.Create())
            {
                aesAlgorithm.Key = Convert.FromBase64String(keyBase64);
                aesAlgorithm.GenerateIV();

                //set the parameters with out keyword
                vectorBase64 = Convert.ToBase64String(aesAlgorithm.IV);

                // Create encryptor object
                ICryptoTransform encryptor = aesAlgorithm.CreateEncryptor();

                byte[] encryptedData;

                //Encryption will be done in a memory stream through a CryptoStream object
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter sw = new StreamWriter(cs))
                        {
                            sw.Write(plainText);
                        }
                        encryptedData = ms.ToArray();
                    }
                }

                return Convert.ToBase64String(encryptedData);
            }
        }

        private static string AesDecryptKey(string cipherText, string keyBase64, string vectorBase64)
        {
            using (Aes aesAlgorithm = Aes.Create())
            {
                aesAlgorithm.Key = Convert.FromBase64String(keyBase64);
                aesAlgorithm.IV = Convert.FromBase64String(vectorBase64);

                // Create decryptor object
                ICryptoTransform decryptor = aesAlgorithm.CreateDecryptor();

                byte[] cipher = Convert.FromBase64String(cipherText);

                //Decryption will be done in a memory stream through a CryptoStream object
                using (MemoryStream ms = new MemoryStream(cipher))
                {
                    using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader sr = new StreamReader(cs))
                        {
                            return sr.ReadToEnd();
                        }
                    }
                }
            }
        }

        public static SecurityKey GetSymmetricKey()
        {
            var securityKey = Encoding.UTF8.GetBytes(SecurityKey(out string outString));
            return new SymmetricSecurityKey(securityKey);
        }

        public static SecurityKey GetSymmetricSecurityKey()
        {
            var hmac = new HMACSHA512(Encoding.UTF8.GetBytes(SecurityKey(out string outString)));
            byte[] key = Convert.FromBase64String(SecurityKey(out string outParam));
            return new SymmetricSecurityKey(hmac.Key);
        }

        public static byte[] GetSymmetricSecurityKeyAsBytes()
        {
            var Key = Encoding.UTF8.GetBytes(SecurityKey(out string outParam));
            var Secret = Convert.ToBase64String(Key);
            return Convert.FromBase64String(Secret);
        }

        public static TokenValidationParameters GetTokenValidationParameters()
        {
            var result = SecurityKey(out string outParam);
            var key = GetSymmetricSecurityKey();

            return new TokenValidationParameters
            {
                ValidIssuer = configuration!["Auth:Jwt:JwtIssuer"],
                ValidAudience = configuration["Auth:Jwt:JwtAudience"],
                IssuerSigningKey = key,
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,

            };
        }

        public static long RandomNumber(int min, int max)
        {
            long number = random.NextInt64(min, max);
            return number;
        }

        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public static string GenerateRandomString()
        {
            int length = 30;
            StringBuilder sb = new StringBuilder();
            //Random random = new Random();

            char letter;

            for (int i = 0; i < length; i++)
            {
                double flt = random.NextDouble();
                int shift = Convert.ToInt32(Math.Floor(25 * flt));
                letter = Convert.ToChar(shift + 65);
                sb.Append(letter);
            }

            return sb.ToString();
        }

        public static string GenerateRandomString(int length)
        {
            var random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var randomString = new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
            return randomString;
        }

        public static string GenerateSecureRandomString(out string secureRandomString)
        {
            byte[] bytes = new byte[64];
            RandomNumberGenerator.Fill(bytes);
            secureRandomString = Convert.ToBase64String(bytes);
            return secureRandomString;
        }

        public static string SecurityKey(out string hashString)
        {
            string secureRandomString = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
            using (var sha = SHA512.Create())
            {
                var bytes = Encoding.UTF8.GetBytes(secureRandomString);
                var hash = sha.ComputeHash(bytes);
                hashString = Convert.ToBase64String(hash);

                return hashString;
            }

        }

        public static string RandomKey()
        {
            var sb = new StringBuilder();
            sb.Append(RandomString(50));
            return sb.ToString();
        }

        public static string ToSHA512(this string value)
        {
            using var sha = SHA512.Create();
            var bytes = Encoding.UTF8.GetBytes(value);
            var hash = sha.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }


        public static string GeneratePasswordResetToken(string email, out string passwordResetToken)
        {
            try
            {
                GenerateSecureRandomString(out string secureRandomString);
                using (var sha256 = SHA256.Create())
                {
                    var salt = secureRandomString + email;
                    var hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(salt));
                    return passwordResetToken = Convert.ToBase64String(hash);
                }
            }
            catch (Exception )
            {

                throw;
            }

        }

        public static string GeneratePassResetToken(string email, out string token)
        {
            try
            {
                string salt = GenerateRandomString();
                using (var sha = SHA512.Create())
                {
                    var bytes = Encoding.UTF8.GetBytes(email + salt);
                    var hash = sha.ComputeHash(bytes);
                    token = Convert.ToBase64String(hash);

                    return token;
                }

            }
            catch (Exception )
            {

                throw ;
            }

        }

        public static string DecodePasswordResetToken(string encodedKey, out string decodedKey)
        {
            try
            {
                byte[] bytes = WebEncoders.Base64UrlDecode(encodedKey);
                return decodedKey = Encoding.UTF8.GetString(bytes);
            }
            catch (Exception )
            {

                throw ;
            }

        }



    }

}
