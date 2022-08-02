using System;

namespace San.CoreCommon.Attribute
{
  [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
  public class NotCacheAttribute : FlagsAttribute
  {
  }
}
