using System.Collections.Generic;
using System.Text.Json;

namespace Common.Export
{
    public class JsonService : IJsonService
    {
        public byte[] Write<T>(IList<T> registers)
        {
            return JsonSerializer.SerializeToUtf8Bytes(registers, new JsonSerializerOptions
            {
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            });
        }
    }
    public interface IJsonService
    {
        byte[] Write<T>(IList<T> registers);
    }
}
