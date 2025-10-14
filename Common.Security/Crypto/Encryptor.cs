using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Common.Security.Crypto
{
    [SingletonService]
    public sealed class Encryptor : IEncryptor
    {
        // Private method to encrypt input data
        private byte[] Encrypt(byte[] clearData, byte[] Key, byte[] IV)
        {
            using MemoryStream memoryStream = new MemoryStream(); // Properly initializing MemoryStream
            using Aes aes = Aes.Create(); // Creating instance of Aes

            // Set the key and IV (initialization vector)
            aes.Key = Key;
            aes.IV = IV;

            // Using CryptoStream to write data into the memoryStream
            using (CryptoStream cryptoStream = new CryptoStream(memoryStream, aes.CreateEncryptor(), CryptoStreamMode.Write))
            {
                // Writing input data to the CryptoStream
                cryptoStream.Write(clearData, 0, clearData.Length);
                cryptoStream.FlushFinalBlock(); // Finalize writing
            }

            // Returning the encrypted data
            return memoryStream.ToArray();
        }

        // Public method to encrypt a string
        public string Encrypt(string clearText, string? key = "")
        {
            // Using a default key if none is provided
            string strPassword = !string.IsNullOrEmpty(key) ? key : "Ca%%$@#spi!34an.com_8510sdt23456*&@mnbsd";

            // Generating a random salt
            byte[] salt = new byte[16];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt); // Generating random values for salt
            }

            // Using Rfc2898DeriveBytes to generate the key and IV
            using var rfc2898 = new Rfc2898DeriveBytes(strPassword, salt, 100000, HashAlgorithmName.SHA256);
            // Generating 256-bit key and 128-bit IV
            byte[] derivedKey = rfc2898.GetBytes(32);
            byte[] derivedIV = rfc2898.GetBytes(16);

            // Encrypting the data
            byte[] encrypted = Encrypt(Encoding.Unicode.GetBytes(clearText), derivedKey, derivedIV);

            // Combining salt and encrypted data for storage
            byte[] result = new byte[salt.Length + encrypted.Length];
            Buffer.BlockCopy(salt, 0, result, 0, salt.Length);
            Buffer.BlockCopy(encrypted, 0, result, salt.Length, encrypted.Length);

            // Returning the result as a Base64 string
            return Convert.ToBase64String(result);
        }

        // Private method to decrypt the encrypted data
        private byte[] Decrypt(byte[] cipherData, byte[] Key, byte[] IV)
        {
            using MemoryStream memoryStream = new MemoryStream(); // Properly initializing MemoryStream
            using Aes aes = Aes.Create(); // Creating instance of Aes

            // Set the key and IV
            aes.Key = Key;
            aes.IV = IV;

            // Using CryptoStream to read encrypted data from memoryStream
            using (CryptoStream cryptoStream = new CryptoStream(memoryStream, aes.CreateDecryptor(), CryptoStreamMode.Write))
            {
                // Writing the encrypted data to the CryptoStream
                cryptoStream.Write(cipherData, 0, cipherData.Length);
                cryptoStream.FlushFinalBlock(); // Finalize writing
            }

            // Returning the decrypted data
            return memoryStream.ToArray();
        }

        // Public method to decrypt an encrypted string
        public string Decrypt(string cipherText, string? key = "")
        {
            // Using a default key if none is provided
            string strPassword = !string.IsNullOrEmpty(key) ? key : "Ca%%$@#spi!34an.com_8510sdt23456*&@mnbsd";

            // Converting the encrypted string from Base64 to byte array
            byte[] fullCipher = Convert.FromBase64String(cipherText);

            // Extracting the salt from the encrypted data
            byte[] salt = new byte[16];
            Buffer.BlockCopy(fullCipher, 0, salt, 0, salt.Length);

            // Separating the encrypted data
            byte[] cipherData = new byte[fullCipher.Length - salt.Length];
            Buffer.BlockCopy(fullCipher, salt.Length, cipherData, 0, cipherData.Length);

            // Using Rfc2898DeriveBytes to generate the key and IV
            using var rfc2898 = new Rfc2898DeriveBytes(strPassword, salt, 100000, HashAlgorithmName.SHA256);
            // Generating 256-bit key and 128-bit IV
            byte[] derivedKey = rfc2898.GetBytes(32);
            byte[] derivedIV = rfc2898.GetBytes(16);

            // Decrypting the data and returning the original string
            return Encoding.Unicode.GetString(Decrypt(cipherData, derivedKey, derivedIV));
        }
    }
}