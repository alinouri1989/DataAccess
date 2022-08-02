using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Repository.Cache
{
  public static class Serialization
  {
    public static byte[] ToByteArray(this object obj)
    {
      if (obj == null)
        return (byte[]) null;
      using (MemoryStream serializationStream = new MemoryStream())
      {
        new BinaryFormatter().Serialize((Stream) serializationStream, obj);
        return serializationStream.ToArray();
      }
    }

    public static T FromByteArray<T>(this byte[] byteArray) where T : class
    {
      if (byteArray == null)
        return default (T);
      using (MemoryStream serializationStream = new MemoryStream(byteArray))
        return new BinaryFormatter().Deserialize((Stream) serializationStream) as T;
    }
  }
}
