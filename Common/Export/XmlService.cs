using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace Common.Export
{
    public class XmlService : IXmlService
    {
        public byte[] Write<T>(IList<T> registers)
        {
            var serializer = new XmlSerializer(typeof(List<T>));

            using (MemoryStream memoryStream = new MemoryStream())
            {
                serializer.Serialize(memoryStream, registers);

                return memoryStream.ToArray();
            }
        }
    }
    public interface IXmlService
    {
        byte[] Write<T>(IList<T> registers);
    }
}
