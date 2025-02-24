using System.Text.Json;

namespace DataAccess.EFCoreSecondLevelCacheInterceptor
{
    public static class Serialization
    {
        public static byte[] ToByteArray(this object obj)
        {
            if (obj == null)
                return null;

            // Serialize the object to a byte array using JSON  
            return JsonSerializer.SerializeToUtf8Bytes(obj);
        }

        public static T FromByteArray<T>(this byte[] byteArray) where T : class
        {
            if (byteArray == null)
                return default;

            // Deserialize the byte array back to an object using JSON  
            return JsonSerializer.Deserialize<T>(byteArray);
        }
    }
}