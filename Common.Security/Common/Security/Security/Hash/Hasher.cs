using System.Security.Cryptography;
using System.Text;

namespace Common.Security.Security.Hash
{
  public static class Hasher
  {
    private static byte[] GetHash(string input, eHashType hash)
    {
      byte[] bytes = Encoding.ASCII.GetBytes(input);
      switch (hash)
      {
        case eHashType.HMAC:
                    return HMAC.Create().ComputeHash(bytes);
        case eHashType.HMACMD5:
          return HMAC.Create().ComputeHash(bytes);
        case eHashType.HMACSHA1:
          return HMAC.Create().ComputeHash(bytes);
        case eHashType.HMACSHA256:
          return HMAC.Create().ComputeHash(bytes);
        case eHashType.HMACSHA384:
          return HMAC.Create().ComputeHash(bytes);
        case eHashType.HMACSHA512:
          return HMAC.Create().ComputeHash(bytes);
        case eHashType.MD5:
          return MD5.Create().ComputeHash(bytes);
        case eHashType.SHA1:
          return SHA1.Create().ComputeHash(bytes);
        case eHashType.SHA256:
          return SHA256.Create().ComputeHash(bytes);
        case eHashType.SHA384:
          return SHA384.Create().ComputeHash(bytes);
        case eHashType.SHA512:
          return SHA512.Create().ComputeHash(bytes);
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
      HMAC,
      HMACMD5,
      HMACSHA1,
      HMACSHA256,
      HMACSHA384,
      HMACSHA512,
      MD5,
      SHA1,
      SHA256,
      SHA384,
      SHA512,
    }
  }
}
