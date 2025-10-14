namespace Common.Security.Crypto
{
    public interface IEncryptor
    {
        string Encrypt(string clearText, string? key);
        string Decrypt(string cipherText, string? key);
    }
}
