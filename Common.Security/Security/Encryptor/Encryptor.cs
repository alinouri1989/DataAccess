using San.CoreCommon.Attribute;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace San.CoreCommon.Security.Encryptor
{
  [SingletonService]
  public sealed class Encryptor : IEncryptor
  {
    private byte[] Encrypt(byte[] clearData, byte[] Key, byte[] IV)
    {
      using (MemoryStream memoryStream = new MemoryStream())
      {
        Rijndael rijndael = Rijndael.Create();
        rijndael.Key = Key;
        rijndael.IV = IV;
        using (CryptoStream cryptoStream = new CryptoStream((Stream) memoryStream, rijndael.CreateEncryptor(), CryptoStreamMode.Write))
        {
          cryptoStream.Write(clearData, 0, clearData.Length);
          cryptoStream.Close();
        }
        return memoryStream.ToArray();
      }
    }

    public string Encrypt(string clearText)
    {
      string strPassword = "SeyedAliNouriServices";
      using (PasswordDeriveBytes passwordDeriveBytes = new PasswordDeriveBytes(strPassword, new byte[13]
      {
        (byte) 73,
        (byte) 118,
        (byte) 97,
        (byte) 110,
        (byte) 32,
        (byte) 77,
        (byte) 101,
        (byte) 100,
        (byte) 118,
        (byte) 101,
        (byte) 100,
        (byte) 101,
        (byte) 118
      }))
        return Convert.ToBase64String(this.Encrypt(Encoding.Unicode.GetBytes(clearText), passwordDeriveBytes.GetBytes(32), passwordDeriveBytes.GetBytes(16)));
    }

    private byte[] Encrypt(byte[] clearData, string Password)
    {
      using (PasswordDeriveBytes passwordDeriveBytes = new PasswordDeriveBytes(Password, new byte[13]
      {
        (byte) 73,
        (byte) 118,
        (byte) 97,
        (byte) 110,
        (byte) 32,
        (byte) 77,
        (byte) 101,
        (byte) 100,
        (byte) 118,
        (byte) 101,
        (byte) 100,
        (byte) 101,
        (byte) 118
      }))
        return this.Encrypt(clearData, passwordDeriveBytes.GetBytes(32), passwordDeriveBytes.GetBytes(16));
    }

    private void Encrypt(string fileIn, string fileOut, string Password)
    {
      using (FileStream fileStream1 = new FileStream(fileIn, FileMode.Open, FileAccess.Read))
      {
        using (FileStream fileStream2 = new FileStream(fileOut, FileMode.OpenOrCreate, FileAccess.Write))
        {
          using (PasswordDeriveBytes passwordDeriveBytes = new PasswordDeriveBytes(Password, new byte[13]
          {
            (byte) 73,
            (byte) 118,
            (byte) 97,
            (byte) 110,
            (byte) 32,
            (byte) 77,
            (byte) 101,
            (byte) 100,
            (byte) 118,
            (byte) 101,
            (byte) 100,
            (byte) 101,
            (byte) 118
          }))
          {
            using (Rijndael rijndael = Rijndael.Create())
            {
              rijndael.Key = passwordDeriveBytes.GetBytes(32);
              rijndael.IV = passwordDeriveBytes.GetBytes(16);
              using (CryptoStream cryptoStream = new CryptoStream((Stream) fileStream2, rijndael.CreateEncryptor(), CryptoStreamMode.Write))
              {
                int count1 = 4096;
                byte[] buffer = new byte[count1];
                int count2;
                do
                {
                  count2 = fileStream1.Read(buffer, 0, count1);
                  cryptoStream.Write(buffer, 0, count2);
                }
                while ((uint) count2 > 0U);
                cryptoStream.Close();
                fileStream1.Close();
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
        using (Rijndael rijndael = Rijndael.Create())
        {
          rijndael.Key = Key;
          rijndael.IV = IV;
          using (CryptoStream cryptoStream = new CryptoStream((Stream) memoryStream, rijndael.CreateDecryptor(), CryptoStreamMode.Write))
          {
            cryptoStream.Write(cipherData, 0, cipherData.Length);
            cryptoStream.Close();
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
        (byte) 73,
        (byte) 118,
        (byte) 97,
        (byte) 110,
        (byte) 32,
        (byte) 77,
        (byte) 101,
        (byte) 100,
        (byte) 118,
        (byte) 101,
        (byte) 100,
        (byte) 101,
        (byte) 118
      }))
        return Encoding.Unicode.GetString(this.Decrypt(Convert.FromBase64String(cipherText), passwordDeriveBytes.GetBytes(32), passwordDeriveBytes.GetBytes(16)));
    }

    private byte[] Decrypt(byte[] cipherData, string Password)
    {
      using (PasswordDeriveBytes passwordDeriveBytes = new PasswordDeriveBytes(Password, new byte[13]
      {
        (byte) 73,
        (byte) 118,
        (byte) 97,
        (byte) 110,
        (byte) 32,
        (byte) 77,
        (byte) 101,
        (byte) 100,
        (byte) 118,
        (byte) 101,
        (byte) 100,
        (byte) 101,
        (byte) 118
      }))
        return this.Decrypt(cipherData, passwordDeriveBytes.GetBytes(32), passwordDeriveBytes.GetBytes(16));
    }

    private void Decrypt(string fileIn, string fileOut, string Password)
    {
      using (FileStream fileStream1 = new FileStream(fileIn, FileMode.Open, FileAccess.Read))
      {
        using (FileStream fileStream2 = new FileStream(fileOut, FileMode.OpenOrCreate, FileAccess.Write))
        {
          using (PasswordDeriveBytes passwordDeriveBytes = new PasswordDeriveBytes(Password, new byte[13]
          {
            (byte) 73,
            (byte) 118,
            (byte) 97,
            (byte) 110,
            (byte) 32,
            (byte) 77,
            (byte) 101,
            (byte) 100,
            (byte) 118,
            (byte) 101,
            (byte) 100,
            (byte) 101,
            (byte) 118
          }))
          {
            using (Rijndael rijndael = Rijndael.Create())
            {
              rijndael.Key = passwordDeriveBytes.GetBytes(32);
              rijndael.IV = passwordDeriveBytes.GetBytes(16);
              using (CryptoStream cryptoStream = new CryptoStream((Stream) fileStream2, rijndael.CreateDecryptor(), CryptoStreamMode.Write))
              {
                int count1 = 4096;
                byte[] buffer = new byte[count1];
                int count2;
                do
                {
                  count2 = fileStream1.Read(buffer, 0, count1);
                  cryptoStream.Write(buffer, 0, count2);
                }
                while ((uint) count2 > 0U);
                cryptoStream.Close();
                fileStream1.Close();
              }
            }
          }
        }
      }
    }
  }
}
