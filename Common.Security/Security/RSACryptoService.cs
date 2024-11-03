using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace San.CoreCommon.Security
{
    [SingletonService]
    public class RSACryptoService : IRSACryptoService
    {
        private RSACryptoServiceProvider _csp;

        public RSACryptoService() => this._csp = new RSACryptoServiceProvider();

        public void FromXmlFile(string path) => this._csp.FromXmlString(File.ReadAllText(path));

        public string Encrypt(string input)
        {
            try
            {
                return Convert.ToBase64String(this._csp.Encrypt(Encoding.UTF8.GetBytes(input), false));
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
                this._csp.ExportParameters(true);
                return Encoding.UTF8.GetString(this._csp.Decrypt(rgb, false));
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}
