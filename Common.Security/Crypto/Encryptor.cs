using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Common.Security.Crypto
{
    [SingletonService]
    public sealed class Encryptor : IEncryptor
    {
        private byte[] Encrypt(byte[] clearData, byte[] Key, byte[] IV)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (Aes aes = Aes.Create())
                {
                    aes.Key = Key;
                    aes.IV = IV;
                    using (CryptoStream cryptoStream = new CryptoStream(memoryStream, aes.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cryptoStream.Write(clearData, 0, clearData.Length);
                        cryptoStream.FlushFinalBlock();
                    }
                    return memoryStream.ToArray();
                }
            }
        }

        public string Encrypt(string clearText)
        {
            string strPassword = "SeyedAliNouriServices";
            using (PasswordDeriveBytes passwordDeriveBytes = new PasswordDeriveBytes(strPassword, new byte[13]
            {
                73, 118, 97, 110,32, 77, 101, 100,118, 101, 100, 101,118
            }))
            {
                return Convert.ToBase64String(Encrypt(
                    Encoding.Unicode.GetBytes(clearText),
                    passwordDeriveBytes.GetBytes(32),
                    passwordDeriveBytes.GetBytes(16)));
            }
        }

        private byte[] Encrypt(byte[] clearData, string Password)
        {
            using PasswordDeriveBytes passwordDeriveBytes = new PasswordDeriveBytes(Password, new byte[13]
            {
                73, 118, 97, 110,
                32, 77, 101, 100,
                118, 101, 100, 101,
                118
            });
            return Encrypt(clearData, passwordDeriveBytes.GetBytes(32), passwordDeriveBytes.GetBytes(16));
        }

        private void Encrypt(string fileIn, string fileOut, string Password)
        {
            using (FileStream fileStream1 = new FileStream(fileIn, FileMode.Open, FileAccess.Read))
            {
                using (FileStream fileStream2 = new FileStream(fileOut, FileMode.OpenOrCreate, FileAccess.Write))
                {
                    using (PasswordDeriveBytes passwordDeriveBytes = new PasswordDeriveBytes(Password, new byte[13]
                    {
                        73, 118, 97, 110,
                        32, 77, 101, 100,
                        118, 101, 100, 101,
                        118
                    }))
                    {
                        using (Aes aes = Aes.Create())
                        {
                            aes.Key = passwordDeriveBytes.GetBytes(32);
                            aes.IV = passwordDeriveBytes.GetBytes(16);
                            using (CryptoStream cryptoStream = new CryptoStream(fileStream2, aes.CreateEncryptor(), CryptoStreamMode.Write))
                            {
                                int count1 = 4096;
                                byte[] buffer = new byte[count1];
                                int count2;
                                do
                                {
                                    count2 = fileStream1.Read(buffer, 0, count1);
                                    cryptoStream.Write(buffer, 0, count2);
                                }
                                while (count2 > 0);
                            }
                        }
                    }
                }
            }
        }

        private byte[] Decrypt(byte[] cipherData, byte[] Key, byte[] IV)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (Aes aes = Aes.Create())
                {
                    aes.Key = Key;
                    aes.IV = IV;
                    using (CryptoStream cryptoStream = new CryptoStream(memoryStream, aes.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cryptoStream.Write(cipherData, 0, cipherData.Length);
                        cryptoStream.FlushFinalBlock();
                        return memoryStream.ToArray();
                    }
                }
            }
        }

        public string Decrypt(string cipherText)
        {
            string strPassword = "SeyedAliNouriServices";
            using (PasswordDeriveBytes passwordDeriveBytes = new PasswordDeriveBytes(strPassword, new byte[13]
            {
                73, 118, 97, 110,
                32, 77, 101, 100,
                118, 101, 100, 101,
                118
            }))
            {
                return Encoding.Unicode.GetString(Decrypt(
                    Convert.FromBase64String(cipherText),
                    passwordDeriveBytes.GetBytes(32),
                    passwordDeriveBytes.GetBytes(16)));
            }
        }

        private byte[] Decrypt(byte[] cipherData, string Password)
        {
            using (PasswordDeriveBytes passwordDeriveBytes = new PasswordDeriveBytes(Password, new byte[13]
            {
                73, 118, 97, 110,
                32, 77, 101, 100,
                118, 101, 100, 101,
                118
            }))
            {
                return Decrypt(cipherData, passwordDeriveBytes.GetBytes(32), passwordDeriveBytes.GetBytes(16));
            }
        }

        private void Decrypt(string fileIn, string fileOut, string Password)
        {
            using (FileStream fileStream1 = new FileStream(fileIn, FileMode.Open, FileAccess.Read))
            {
                using (FileStream fileStream2 = new FileStream(fileOut, FileMode.OpenOrCreate, FileAccess.Write))
                {
                    using (PasswordDeriveBytes passwordDeriveBytes = new PasswordDeriveBytes(Password, new byte[13]
                    {
                        73, 118, 97, 110,
                        32, 77, 101, 100,
                        118, 101, 100, 101,
                        118
                    }))
                    {
                        using (Aes aes = Aes.Create())
                        {
                            aes.Key = passwordDeriveBytes.GetBytes(32);
                            aes.IV = passwordDeriveBytes.GetBytes(16);
                            using (CryptoStream cryptoStream = new CryptoStream(fileStream2, aes.CreateDecryptor(), CryptoStreamMode.Write))
                            {
                                int count1 = 4096;
                                byte[] buffer = new byte[count1];
                                int count2;
                                do
                                {
                                    count2 = fileStream1.Read(buffer, 0, count1);
                                    cryptoStream.Write(buffer, 0, count2);
                                }
                                while (count2 > 0);
                            }
                        }
                    }
                }
            }
        }
    }
}