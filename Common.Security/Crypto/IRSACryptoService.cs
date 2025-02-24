namespace Common.Security.Crypto
{
    public interface IRSACryptoService
    {
        void FromXmlFile(string path);

        string Encrypt(string input);

        string Decrypt(string input);
    }
}
