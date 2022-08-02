using System.Text.RegularExpressions;

namespace Common.ValidationRole.ValidationRule
{
  public static class Validate
  {
    public static bool IsValidEmailAddress(this string s) => new Regex("^[\\w-\\.]+@([\\w-]+\\.)+[\\w-]{2,4}$").IsMatch(s);

    public static bool IsValidUrl(this string text) => new Regex("http(s)?://([\\w-]+\\.)+[\\w-]+(/[\\w- ./?%&=]*)?").IsMatch(text);

    public static bool IsValidIPAddress(this string s) => Regex.IsMatch(s, "\\b(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\\b");

    public static bool IsStrongPassword(this string s)
    {
      bool flag = Regex.IsMatch(s, "[\\d]");
      if (flag)
        flag = Regex.IsMatch(s, "[a-z]");
      if (flag)
        flag = Regex.IsMatch(s, "[A-Z]");
      if (flag)
        flag = Regex.IsMatch(s, "[\\s~!@#\\$%\\^&\\*\\(\\)\\{\\}\\|\\[\\]\\\\:;'?,.`+=<>\\/]");
      if (flag)
        flag = s.Length > 7;
      return flag;
    }

    public static bool IsInRange(this int target, int start, int end) => target >= start && target <= end;
  }
}
