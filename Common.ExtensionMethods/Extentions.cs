using Common.Constants;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Common.ExtensionMethods
{
    public static class Extentions
    {
        public static int GetRandomOtp()
        {
            return new Random().Next(11110, 99999);
        }
        public static long GenerateOrderCode()
        {
            return new Random().NextInt64(11001111000021200, 99999999999999999);
        }
        public static List<string> GetError(this ModelStateDictionary modelState)
        {
            var errors = modelState.Values.SelectMany(v => v.Errors)
                                  .Select(e => e.ErrorMessage)
                                  .ToList();
            return errors;
        }

        public static bool CheckNationalCode(string code)
        {
            try
            {
                var L = code.Length;
                if (L < 8 || long.Parse(code) == 0) return false;
                code = ("0000" + code).Substring(L + 4 - 10);
                if (int.Parse(code.Substring(3, 6)) == 0) return false;
                var c = int.Parse(code.Substring(9, 1));
                var s = 0;
                for (var i = 0; i < 9; i++)
                    s += int.Parse(code.Substring(i, 1)) * (10 - i);
                s = s % 11;
                return s < 2 && c == s || s >= 2 && c == 11 - s;
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Nationalcode is not correct format", ex);
            }

        }

        public static bool IsNumeric(this string text)
        {
            try
            {
                var valid = long.TryParse(text, out var value);
                return valid;
            }
            catch (Exception)
            {
                return false;
            }

        }
        public static int[] ToIntArray(this string value)
        {
            if (value.Contains("0"))
            {
                return [1, 2, 3, 4, 5, 6, 7];
            }
            else
            {
                string[] parts = value.Split(',');
                if (parts.Length > 0)
                {
                    int[] byteArray = new int[parts.Length];
                    for (int i = 0; i < parts.Length; i++)
                    {
                        byteArray[i] = Convert.ToInt32(parts[i]);
                    }
                    return byteArray;
                }
            }
            return default;
        }
        public static byte[] ToByteArray(this string value)
        {
            string[] parts = value.Split(',');
            if (parts.Length > 0)
            {
                byte[] byteArray = new byte[parts.Length];
                for (int i = 0; i < parts.Length; i++)
                {
                    byteArray[i] = Convert.ToByte(parts[i]);
                }
                return byteArray;
            }
            return default;
        }
        public static string GetExtension(this IFormFile formFile)
        {
            return Path.GetExtension(formFile.FileName).ToLower();
        }


        public static string GetEnumDescription(this Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());

            DescriptionAttribute[] attributes = fi.GetCustomAttributes(typeof(DescriptionAttribute), false) as DescriptionAttribute[];

            if (attributes != null && attributes.Any())
            {
                return attributes.First().Description;
            }

            return value.ToString();
        }

        public static string GetDescription(short value)
        {
            var props = typeof(StatusConstants).GetFields(BindingFlags.Public | BindingFlags.Static);
            var wantedProp = props.FirstOrDefault(prop => (short?)prop.GetValue(null) == value)?.Name;
            var desc = AttributeHelper.GetConstFieldAttributeValue<StatusConstants, string, DescriptionAttribute>(wantedProp, y => y.Description);
            return desc;
        }

        public static string GetDescription<T>(this Type exceptionType) where T : class
        {
            var props = typeof(T).GetFields(BindingFlags.Public | BindingFlags.Static);
            var wantedProp = props.FirstOrDefault(prop => prop.GetType().Name == exceptionType.Name)?.Name;
            var desc = AttributeHelper.GetConstFieldAttributeValue<T, string, DescriptionAttribute>(wantedProp, y => y.Description);
            return desc;
        }

        public static class AttributeHelper
        {
            public static TOut GetConstFieldAttributeValue<T, TOut, TAttribute>(
                string fieldName,
                Func<TAttribute, TOut> valueSelector)
                where TAttribute : Attribute
            {
                var fieldInfo = typeof(T).GetField(fieldName, BindingFlags.Public | BindingFlags.Static);
                if (fieldInfo == null)
                {
                    return default;
                }
                var att = fieldInfo.GetCustomAttributes(typeof(TAttribute), true).FirstOrDefault() as TAttribute;
                return att != null ? valueSelector(att) : default;
            }
        }
        public static string ToQueryString(this object request, string separator = ",")
        {
            if (request == null)
                throw new ArgumentNullException("Argument Null Exception");

            // Get all properties on the object
            var properties = request.GetType().GetProperties()
                .Where(x => x.CanRead)
                .Where(x => x.GetValue(request, null) != null)
                .ToDictionary(x => x.Name, x => x.GetValue(request, null));

            // Get names for all IEnumerable properties (excl. string)
            var propertyNames = properties
                .Where(x => !(x.Value is string) && x.Value is IEnumerable)
                .Select(x => x.Key)
                .ToList();

            // Concat all IEnumerable properties into a comma separated string
            foreach (var key in propertyNames)
            {
                var valueType = properties[key].GetType();
                var valueElemType = valueType.IsGenericType
                                        ? valueType.GetGenericArguments()[0]
                                        : valueType.GetElementType();
                if (valueElemType.IsPrimitive || valueElemType == typeof(string))
                {
                    var enumerable = properties[key] as IEnumerable;
                    properties[key] = string.Join(separator, enumerable.Cast<object>());
                }
            }

            // Concat all key/value pairs into a string separated by ampersand
            return "?" + string.Join("&", properties.Where(v => v.Value != null && !string.IsNullOrEmpty(v.Value.ToString()) && Convert.ToInt64(v.Value) != 0).Select(x => string.Concat(Uri.EscapeDataString(x.Key), "=", Uri.EscapeDataString(x.Value.ToString()))));
        }

        public static T ToEnum<T>(this int value) where T : Enum
        {
            return (T)Enum.ToObject(typeof(T), value);
        }

        public static T GetValue<T>(this Enum enumValue) where T : struct, IConvertible
        {
            if (!typeof(T).IsEnum)
            {
                throw new ArgumentException("T must be an enumerated type");
            }

            return (T)Enum.Parse(typeof(T), enumValue.ToString());
        }

        public static int GetValue<TEnum>(this TEnum enumvalue)
        {
            Enum test = Enum.Parse(typeof(TEnum), enumvalue.ToString()) as Enum;
            return Convert.ToInt32(test); // x is the integer value of enum
        }

        public static SelectList ToSelectList<TEnum>(this TEnum enumObj, string selectedValue = null) where TEnum : struct, IConvertible
        {

            var values = Enum.GetValues(typeof(TEnum))
                        .Cast<TEnum>()
                        .Select(e => new SelectListItem
                        {
                            Text = e.GetDescription(),
                            Value = e.GetValue().ToString(),
                            Selected = e.ToString() == selectedValue
                        })
                        .ToList();

            return new SelectList(values, "Value", "Text");
        }
        public static string GetDescription<T>(this T value)
        {
            Type type = typeof(T);
            if (!type.IsEnum)
                throw new ArgumentException("T must be an enumerated type");

            MemberInfo[] memberInfo = type.GetMember(value.ToString());
            if (memberInfo != null && memberInfo.Length > 0)
            {
                object[] desc = memberInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
                return ((DescriptionAttribute)desc[0]).Description;
            }

            return value.ToString();
        }
        public static Guid? LongToGuid(long value)
        {
            byte[] guidData = new byte[16];
            Array.Copy(BitConverter.GetBytes(value), guidData, 8);
            return new Guid(guidData);
        }

        public static long? GuidToLong(Guid? guid)
        {
            if (guid == null) return -1;
            if (BitConverter.ToInt64(guid.Value.ToByteArray(), 8) != 0)
                throw new OverflowException("Value was either too large or too small for an Int64.");
            return BitConverter.ToInt64(guid.Value.ToByteArray(), 0);
        }
        public static string GenerateReferalCode()
        {
            var guid = Guid.NewGuid();
            return guid.ToString().Substring(0, 8);
        }

        public static string GetChangeStatus(this bool IsActive)
        {
            switch (IsActive)
            {
                case true:
                    return "وضعیت با موفقیت فعال شد";
                case false:
                    return "وضعیت با موفقیت غیر فعال شد";
            }
        }

        public static PropertyInfo[] GetEditableProps<T>(this T edit)
            where T : class
        {
            var props = edit.GetType().GetProperties();
            var propertyInfos = new List<PropertyInfo>();

            foreach (var item in props)
            {
                var can = item.GetCustomAttribute(typeof(EditSpecificAttribute), false);
                if (can != null)
                {
                    propertyInfos.Add(item);
                }
            }

            return propertyInfos.ToArray();
        }

        public static PropertyInfo[] GetEditableProps(this Type type, string[] names)
        {
            var propertyInfos = new List<PropertyInfo>();

            foreach (var name in names)
            {
                foreach (var item in type.GetProperties())
                {
                    if (item.Name == name)
                    {
                        propertyInfos.Add(item);
                    }
                }
            }
            return propertyInfos.ToArray();
        }
    }
}


