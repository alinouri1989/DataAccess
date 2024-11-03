namespace System
{
    [AttributeUsage(AttributeTargets.All)]
    public class WidthAttribute : Attribute
    {
        public WidthAttribute(int value)
        {
            Value = value;
        }
        public int Value { get; set; }
    }
}
