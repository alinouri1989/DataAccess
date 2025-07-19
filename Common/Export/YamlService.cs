using System.Collections.Generic;
using System.Text;
using YamlDotNet.Serialization;

namespace Common.Export
{
    public class YamlService : IYamlService
    {
        public byte[] Write<T>(IList<T> registers)
        {
            var serializer = new SerializerBuilder().Build();

            string yamlString = serializer.Serialize(registers);

            return Encoding.UTF8.GetBytes(yamlString);
        }
    }
    public interface IYamlService
    {
        byte[] Write<T>(IList<T> registers);
    }
}
