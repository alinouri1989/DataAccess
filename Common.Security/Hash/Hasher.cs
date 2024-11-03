using System.Security.Cryptography;
using System.Text;

namespace Common.Security.Hash
{
    public static class Hasher
    {
        private static byte[] GetHash(string input, eHashType hash)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(input);

            switch (hash)
            {
                case eHashType.HMACSHA256:
                    using (var hmac = new HMACSHA256())
                    {
                        return hmac.ComputeHash(bytes);
                    }
                case eHashType.MD5:
                    return MD5.HashData(bytes);
                case eHashType.SHA1:
                    return SHA1.HashData(bytes);
                case eHashType.SHA256:
                    return SHA256.HashData(bytes);
                case eHashType.SHA384:
                    return SHA384.HashData(bytes);
                case eHashType.SHA512:
                    return SHA512.HashData(bytes);
                default:
                    return bytes;
            }
        }

        public static string ComputeHash(this string input, eHashType hashType)
        {
            try
            {
                byte[] hash = GetHash(input, hashType);
                StringBuilder stringBuilder = new StringBuilder();
                for (int index = 0; index < hash.Length; ++index)
                    stringBuilder.Append(hash[index].ToString("x2"));
                return stringBuilder.ToString();
            }
            catch
            {
                return string.Empty;
            }
        }

        public enum eHashType
        {
            HMACSHA256,
            MD5,
            SHA1,
            SHA256,
            SHA384,
            SHA512,
        }
    }
}