namespace System
{
    [AttributeUsage(AttributeTargets.All)]
    public class HeightAttribute : Attribute
    {
        public HeightAttribute(int value)
        {
            Value = value;
        }
        public int Value { get; set; }
    }
}
