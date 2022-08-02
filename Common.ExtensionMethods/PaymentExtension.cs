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
