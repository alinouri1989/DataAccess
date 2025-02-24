using System;
using System.Linq;
using System.Reflection;

namespace Common.Constants
{
    public static class AttributeHelper
    {
        public static TOut GetConstFieldAttributeValue<T, TOut, TAttribute>(string fieldName, Func<TAttribute, TOut> valueSelector) where TAttribute : System.Attribute
        {
            FieldInfo field = typeof(T).GetField(fieldName, BindingFlags.Static | BindingFlags.Public);
            if (field == null)
            {
                return default(TOut);
            }

            TAttribute val = field.GetCustomAttributes(typeof(TAttribute), inherit: true).FirstOrDefault() as TAttribute;
            return (val != null) ? valueSelector(val) : default(TOut);
        }
    }
}
