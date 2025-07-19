using Common.Constants;
using CsvHelper.Configuration;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Common.Export
{
    public class CsvService : ICsvService
    {
        public byte[] Write<T>(IList<T> list, bool includeHeader = true)
        {
            var output = new MemoryStream();
            var writer = new StreamWriter(output, Encoding.UTF8);
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = true,
                TrimOptions = TrimOptions.Trim,
            };
            using (var csv = new CsvHelper.CsvWriter(writer, config, leaveOpen: true))
            {
                csv.WriteRecords(list);
            }
            output.Position = 0;
            return output.ToArray();
        }

        public string CreateCsvHeaderLine(PropertyInfo[] properties)
        {
            List<string> propertyValues = new List<string>();

            foreach (var prop in properties)
            {
                string value = prop.Name;

                var attribute = prop.GetCustomAttribute(typeof(DisplayAttribute));
                if (attribute != null)
                {
                    value = (attribute as DisplayAttribute).Name;
                }

                CreateCsvStringItem(propertyValues, value);
            }

            return CreateCsvLine(propertyValues);
        }

        public string CreateCsvLine<T>(T item, PropertyInfo[] properties)
        {
            List<string> propertyValues = new List<string>();

            foreach (var prop in properties)
            {
                string stringformatString = string.Empty;
                object value = prop.GetValue(item, null);

                if (prop.PropertyType == typeof(string))
                {
                    CreateCsvStringItem(propertyValues, value);
                }
                else if (prop.PropertyType == typeof(string[]))
                {
                    CreateCsvStringArrayItem(propertyValues, value);
                }
                else if (prop.PropertyType == typeof(List<string>))
                {
                    CreateCsvStringListItem(propertyValues, value);
                }
                else
                {
                    CreateCsvItem(propertyValues, value);
                }
            }

            return CreateCsvLine(propertyValues);
        }

        public string CreateCsvLine(IList<string> list)
        {
            return string.Join(ExportFormat.CsvDelimiter, list);
        }

        public void CreateCsvItem(List<string> propertyValues, object value)
        {
            if (value != null)
            {
                propertyValues.Add(value.ToString());
            }
            else
            {
                propertyValues.Add(string.Empty);
            }
        }

        public void CreateCsvStringListItem(List<string> propertyValues, object value)
        {
            string formatString = "\"{0}\"";
            if (value != null)
            {
                value = CreateCsvLine((List<string>)value);
                propertyValues.Add(string.Format(formatString, ProcessStringEscapeSequence(value)));
            }
            else
            {
                propertyValues.Add(string.Empty);
            }
        }

        public void CreateCsvStringArrayItem(List<string> propertyValues, object value)
        {
            string formatString = "\"{0}\"";
            if (value != null)
            {
                value = CreateCsvLine(((string[])value).ToList());
                propertyValues.Add(string.Format(formatString, ProcessStringEscapeSequence(value)));
            }
            else
            {
                propertyValues.Add(string.Empty);
            }
        }

        public void CreateCsvStringItem(List<string> propertyValues, object value)
        {
            string formatString = "\"{0}\"";
            if (value != null)
            {
                propertyValues.Add(string.Format(formatString, ProcessStringEscapeSequence(value)));
            }
            else
            {
                propertyValues.Add(string.Empty);
            }
        }

        public string ProcessStringEscapeSequence(object value)
        {
            return value.ToString().Replace("\"", "\"\"");
        }
    }
    public interface ICsvService
    {
        byte[] Write<T>(IList<T> list, bool includeHeader = true);

        string CreateCsvHeaderLine(PropertyInfo[] properties);

        string CreateCsvLine<T>(T item, PropertyInfo[] properties);

        string CreateCsvLine(IList<string> list);

        void CreateCsvItem(List<string> propertyValues, object value);

        void CreateCsvStringListItem(List<string> propertyValues, object value);

        void CreateCsvStringArrayItem(List<string> propertyValues, object value);

        void CreateCsvStringItem(List<string> propertyValues, object value);

        string ProcessStringEscapeSequence(object value);
    }
}
