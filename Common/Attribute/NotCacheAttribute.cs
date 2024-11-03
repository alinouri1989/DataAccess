namespace System
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class NotCacheAttribute : FlagsAttribute
    {
    }
}
