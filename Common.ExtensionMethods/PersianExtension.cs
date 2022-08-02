using System;
using System.Collections.Generic;
using System.Globalization;

namespace San.CoreCommon.ExtensionMethods
{
  public static class PersianExtension
  {
    public static List<string> PersianMonthNames = new List<string>()
    {
      "",
      "فروردین",
      "اردیبهشت",
      "خرداد",
      "تیر",
      "مرداد",
      "شهریور",
      "مهر",
      "آیان",
      "آذر",
      "دی",
      "بهمن",
      "اسفند"
    };
    public static List<string> IslamicMonthNames = new List<string>()
    {
      "",
      "المحرّم",
      "صفر",
      "ربيع الأوّل",
      "ربيع الثاني",
      "جمادى الأولى",
      "جمادى الثانية",
      "رجب",
      "شعبان",
      "رمضان",
      "شوّال",
      "ذو القعدة",
      "ذو الحجّة"
    };
    public static List<string> PersianDaysOfWeek = new List<string>()
    {
      "شنبه",
      "یکشنبه",
      "دوشنبه",
      "سه شنبه",
      "چهارشنبه",
      "پنج شنبه",
      "جمعه"
    };
    public static List<string> IslamicDaysOfWeek = new List<string>()
    {
      "السبت",
      "الأحد",
      "الاثنين",
      "الثلاثاء",
      " الأربعاء",
      "الخميس",
      "الجمعة"
    };

    public static string ToPersianNumbers(this string input)
    {
      if (string.IsNullOrEmpty(input))
        return string.Empty;
      input = input.Replace("0", "۰");
      input = input.Replace("1", "۱");
      input = input.Replace("2", "۲");
      input = input.Replace("3", "۳");
      input = input.Replace("4", "۴");
      input = input.Replace("5", "۵");
      input = input.Replace("6", "۶");
      input = input.Replace("7", "۷");
      input = input.Replace("8", "۸");
      input = input.Replace("9", "۹");
      return input;
    }

    public static string ToGeorianNumbers(this string input)
    {
      if (string.IsNullOrEmpty(input))
        return string.Empty;
      input = input.Replace("۰", "0");
      input = input.Replace("۱", "1");
      input = input.Replace("۲", "2");
      input = input.Replace("۳", "3");
      input = input.Replace("۴", "4");
      input = input.Replace("۵", "5");
      input = input.Replace("۶", "6");
      input = input.Replace("۷", "7");
      input = input.Replace("۸", "8");
      input = input.Replace("۹", "9");
      return input;
    }

    public static string ToNumbers(this string input)
    {
      input = input.Replace("۰", "0");
      input = input.Replace("۱", "1");
      input = input.Replace("۲", "2");
      input = input.Replace("۳", "3");
      input = input.Replace("۴", "4");
      input = input.Replace("۵", "5");
      input = input.Replace("۶", "6");
      input = input.Replace("۷", "7");
      input = input.Replace("۸", "8");
      input = input.Replace("۹", "9");
      return input;
    }

    public static string ToArabicNumbers(this string input)
    {
      input = input.Replace("0", "٠");
      input = input.Replace("1", "١");
      input = input.Replace("2", "٢");
      input = input.Replace("3", "٣");
      input = input.Replace("4", "٤");
      input = input.Replace("5", "٥");
      input = input.Replace("6", "٦");
      input = input.Replace("7", "٧");
      input = input.Replace("8", "٨");
      input = input.Replace("9", "٩");
      return input;
    }

    public static string ToPersianLetters(this string input) => input.Replace("ي", "ی").Replace("ك", "ک");

    public static string ToPersianLettersNew(this string input) => input.Replace("ي", "ی").Replace("ك", "ک").Replace("?", "ک");

    public static DateTimePersian ToPersianDateTime(
      this DateTime input)
    {
            DateTimePersian persianDateTime = new DateTimePersian();
      PersianCalendar persianCalendar = new PersianCalendar();
      persianDateTime.Day = persianCalendar.GetDayOfMonth(input);
      persianDateTime.Month = persianCalendar.GetMonth(input);
      persianDateTime.Year = persianCalendar.GetYear(input);
      persianDateTime.IsLeapYear = persianCalendar.IsLeapYear(input.Year);
      persianDateTime.MonthName = PersianMonthNames[persianDateTime.Month];
      persianDateTime.DayOfWeek = (int) (input.DayOfWeek + 1);
      if (persianDateTime.DayOfWeek > 6)
        persianDateTime.DayOfWeek = 0;
      persianDateTime.TimeString = string.Format("{0}:{1}", (object) input.Hour.ToString().PadLeft(2, '0'), (object) input.Minute.ToString().PadLeft(2, '0')).ToPersianNumbers();
      persianDateTime.DayOfWeekName = PersianDaysOfWeek[persianDateTime.DayOfWeek];
      persianDateTime.LongFormatString = string.Format("{0} {1} {2} {3}", (object) persianDateTime.DayOfWeekName, (object) persianDateTime.Day, (object) persianDateTime.MonthName, (object) persianDateTime.Year).ToPersianNumbers();
      persianDateTime.ShortFormatString = string.Format("{0}/{1}/{2}", (object) persianDateTime.Year, (object) persianDateTime.Month, (object) persianDateTime.Day).ToPersianNumbers();
      persianDateTime.DateTimeString = string.Format("{0} - {1}", (object) persianDateTime.ShortFormatString, (object) persianDateTime.TimeString);
      persianDateTime.LongFormatStringWithTime = string.Format("{0} {1} {2} {3} - {4}", (object) persianDateTime.DayOfWeekName, (object) persianDateTime.Day, (object) persianDateTime.MonthName, (object) persianDateTime.Year, (object) persianDateTime.TimeString).ToPersianNumbers();
      persianDateTime.Date = string.Format("{0}", (object) persianDateTime.ShortFormatString);
      return persianDateTime;
    }

    public static DateTimePersian ToPersianDateTimeENdigit(
      this DateTime input)
    {
      DateTimePersian persianDateTimeEndigit = new DateTimePersian();
      PersianCalendar persianCalendar = new PersianCalendar();
      persianDateTimeEndigit.Day = persianCalendar.GetDayOfMonth(input);
      persianDateTimeEndigit.Month = persianCalendar.GetMonth(input);
      persianDateTimeEndigit.Year = persianCalendar.GetYear(input);
      persianDateTimeEndigit.SimpleYear = Convert.ToInt32(persianCalendar.GetYear(input).ToString().Substring(2, 2));
      persianDateTimeEndigit.IsLeapYear = persianCalendar.IsLeapYear(input.Year);
      persianDateTimeEndigit.MonthName = PersianMonthNames[persianDateTimeEndigit.Month];
      persianDateTimeEndigit.DayOfWeek = (int) (input.DayOfWeek + 1);
      if (persianDateTimeEndigit.DayOfWeek > 6)
        persianDateTimeEndigit.DayOfWeek = 0;
      persianDateTimeEndigit.DayOfWeekName = PersianDaysOfWeek[persianDateTimeEndigit.DayOfWeek];
      persianDateTimeEndigit.LongFormatString = string.Format("{0} {1} {2} {3}", (object) persianDateTimeEndigit.DayOfWeekName, (object) persianDateTimeEndigit.Day, (object) persianDateTimeEndigit.MonthName, (object) persianDateTimeEndigit.Year);
      persianDateTimeEndigit.ShortFormatString = string.Format("{0}/{1}/{2}", (object) persianDateTimeEndigit.Year, (object) persianDateTimeEndigit.Month, (object) persianDateTimeEndigit.Day);
            DateTimePersian dateTimePersian = persianDateTimeEndigit;
      int num = input.Hour;
      string str1 = num.ToString().PadLeft(2, '0');
      num = input.Minute;
      string str2 = num.ToString().PadLeft(2, '0');
      string str3 = string.Format("{0}:{1}", (object) str1, (object) str2);
      dateTimePersian.TimeString = str3;
      persianDateTimeEndigit.DateTimeString = string.Format("{0} - {1}", (object) persianDateTimeEndigit.ShortFormatString, (object) persianDateTimeEndigit.TimeString);
      return persianDateTimeEndigit;
    }

    public static DateTime? ToDataTime(this string input)
    {
      string[] strArray = input.Split('/');
      return strArray.Length != 3 ? new DateTime?() : new DateTime?(new DateTime(int.Parse(strArray[0]), int.Parse(strArray[1]), int.Parse(strArray[2]), (Calendar) new PersianCalendar()));
    }

    public static DateTime? ToDataTime2(this string input)
    {
      string[] strArray = input.Split('/');
      return strArray.Length != 3 ? new DateTime?() : new DateTime?(new DateTime(int.Parse(strArray[2].Split(' ')[0]), int.Parse(strArray[0]), int.Parse(strArray[1]), (Calendar) new PersianCalendar()));
    }

    public static string ReverseString(this string input)
    {
      char[] charArray = input.ToCharArray();
      Array.Reverse<char>(charArray);
      return new string(charArray);
    }

    public static DateTimePersian ToHijriDateTime(
      this DateTime input)
    {
            DateTimePersian hijriDateTime = new DateTimePersian();
      HijriCalendar hijriCalendar = new HijriCalendar();
      hijriCalendar.HijriAdjustment = -1;
      hijriDateTime.Day = hijriCalendar.GetDayOfMonth(input);
      hijriDateTime.Month = hijriCalendar.GetMonth(input);
      hijriDateTime.Year = hijriCalendar.GetYear(input);
      hijriDateTime.IsLeapYear = hijriCalendar.IsLeapYear(input.Year);
      hijriDateTime.MonthName = IslamicMonthNames[hijriDateTime.Month];
      hijriDateTime.DayOfWeek = (int) (input.DayOfWeek + 1);
      if (hijriDateTime.DayOfWeek > 6)
        hijriDateTime.DayOfWeek = 0;
      hijriDateTime.DayOfWeekName = IslamicDaysOfWeek[hijriDateTime.DayOfWeek];
      hijriDateTime.LongFormatString = string.Format("{0} {1} {2} {3}", (object) hijriDateTime.DayOfWeekName, (object) hijriDateTime.Day, (object) hijriDateTime.MonthName, (object) hijriDateTime.Year).ToArabicNumbers();
      hijriDateTime.ShortFormatString = string.Format("{0}/{1}/{2}", (object) hijriDateTime.Year, (object) hijriDateTime.Month, (object) hijriDateTime.Day).ToArabicNumbers();
            DateTimePersian dateTimePersian = hijriDateTime;
      int num = input.Hour;
      string str1 = num.ToString().PadLeft(2, '0');
      num = input.Minute;
      string str2 = num.ToString().PadLeft(2, '0');
      string arabicNumbers = string.Format("{0}:{1}", (object) str1, (object) str2).ToArabicNumbers();
      dateTimePersian.TimeString = arabicNumbers;
      hijriDateTime.DateTimeString = string.Format("{0} - {1}", (object) hijriDateTime.ShortFormatString, (object) hijriDateTime.TimeString);
      return hijriDateTime;
    }

    public static string ReplaceFormatForXML(this string input)
    {
      input = input.Replace("\"", "&quot;");
      input = input.Replace("'", "&apos;");
      input = input.Replace("<", "&lt;");
      input = input.Replace(">", "&gt;");
      return input;
    }

    public class DateTimePersian
    {
      public int Year { get; set; }

      public int SimpleYear { get; set; }

      public int Month { get; set; }

      public int Day { get; set; }

      public int DayOfWeek { get; set; }

      public bool IsLeapYear { get; set; }

      public string MonthName { get; set; }

      public string DayOfWeekName { get; set; }

      public string LongFormatString { get; set; }

      public string LongFormatStringWithTime { get; set; }

      public string ShortFormatString { get; set; }

      public string ShortFormatStringEn { get; set; }

      public string TimeString { get; set; }

      public string DateTimeString { get; set; }

      public string Date { get; set; }
    }
  }
}
