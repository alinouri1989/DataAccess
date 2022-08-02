// Decompiled with JetBrains decompiler
// Type: PEC.CoreCommon.ExtensionMethods.PaymentExtension
// Assembly: Common.ExtensionMethods, Version=1.0.4.0, Culture=neutral, PublicKeyToken=null
// MVID: FD6FAEF5-3851-460D-92AD-987C8995180F
// Assembly location: C:\Users\sa.nori\AppData\Local\Temp\Riqygac\0d10b3a396\lib\net5.0\Common.ExtensionMethods.dll

namespace San.CoreCommon.ExtensionMethods
{
  public static class PaymentExtension
  {
    public static string Mask64(this string value)
    {
      if (string.IsNullOrEmpty(value))
        return (string) null;
      return value.Length != 16 ? value : value.Substring(0, 6) + "******" + value.Substring(12, 4);
    }
  }
}
