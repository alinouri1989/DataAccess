namespace San.CoreCommon.Security.Encryptor
{
  public interface IEncryptor
  {
    string Encrypt(string clearText);

    string Decrypt(string cipherText);
  }
}
