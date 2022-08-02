using System;

namespace San.CoreCommon.Attribute
{
  [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
  public class SingletonServiceAttribute : FlagsAttribute
  {
  }
}
