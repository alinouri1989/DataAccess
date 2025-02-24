using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Common.Security.Crypto
{
    [SingletonService]
    public class RSACryptoService : IRSACryptoService
    {
        private RSACryptoServiceProvider _csp;

        public RSACryptoService() => _csp = new RSACryptoServiceProvider();

        public void FromXmlFile(string path) => _csp.FromXmlString(File.ReadAllText(path));

        public string Encrypt(string input)
        {
            try
            {
                return Convert.ToBase64String(_csp.Encrypt(Encoding.UTF8.GetBytes(input), false));
            }
            catch (Exception ex)
            {
                throw new Exception("you got error on RSACryptoService", ex);
            }
        }

        public string Decrypt(string input)
        {
            try
            {
                byte[] rgb = Convert.FromBase64String(input);
                _csp.ExportParameters(true);
                return Encoding.UTF8.GetString(_csp.Decrypt(rgb, false));
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}
