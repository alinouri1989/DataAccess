namespace Common.Security.Crypto
{
  public interface IEncryptor
  {
    string Encrypt(string clearText);

    string Decrypt(string cipherText);
  }
}
