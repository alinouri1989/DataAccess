// Decompiled with JetBrains decompiler
// Type: Repository.Cache.Serialization
// Assembly: DataAccess, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6804894C-F989-4432-B8EA-6F3F70ACE424
// Assembly location: C:\Users\sa.nori\AppData\Local\Temp\Riqygac\68676b643f\lib\net5.0\DataAccess.dll

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
