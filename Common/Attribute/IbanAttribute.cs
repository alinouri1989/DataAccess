using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace System
{
    public class IbanAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var iban = string.Concat("IR", value);
            var IbanValid = CheckSheba(iban);

            switch (IbanValid)
            {
                case 0:
                    return ValidationResult.Success;

                case 1:
                    return new ValidationResult("طول شماره شبا نمیتواند خالی باشد");

                case 2:
                    return new ValidationResult("قالب شماره شبا صحیح نمی باشد");

                case 3:
                    return new ValidationResult("طول شماره شبا نمیتواند بیشتر از 26 باشد");

                case 4:
                    return new ValidationResult("شماره شبا صحیح نمی باشد");

                default:
                    return new ValidationResult("شماره شبا صحیح نمی باشد");
            }
        }


        public static byte CheckSheba(string iban)
        {
            if (string.IsNullOrEmpty(iban))
            {
                return 1;
                //return new ValidationResult("طول شماره شبا نمیتواند خالی باشد");
            }
            iban = iban.Replace(" ", "").ToLower();
            //بررسی رشته وارد شده برای اینکه در فرمت شبا باشد
            var isSheba = Regex.IsMatch(iban, "^[a-zA-Z]{2}\\d{2} ?\\d{4} ?\\d{4} ?\\d{4} ?\\d{4} ?[\\d]{0,2}",
                RegexOptions.Compiled);

            if (!isSheba)
                return 2;
            //return new ValidationResult("قالب شماره شبا صحیح نمی باشد");
            //طول شماره شبا را چک میکند کمتر نباشد
            if (iban.Length < 26)
                return 3;
            //                return new ValidationResult("طول شماره شبا نمیتواند بیشتر از 26 باشد");
            iban = iban.ToLower();
            //بررسی اعتبار سنجی اصلی شبا
            ////ابتدا گرفتن چهار رقم اول شبا
            var get4FirstDigit = iban.Substring(0, 4);
            ////جایگزین کردن عدد 18 و 27 به جای آی و آر
            var replacedGet4FirstDigit = get4FirstDigit.ToLower().Replace("i", "18").Replace("r", "27");
            ////حذف چهار رقم اول از رشته شبا
            var removedShebaFirst4Digit = iban.Replace(get4FirstDigit, "");
            ////کانکت کردن شبای باقیمانده با جایگزین شده چهار رقم اول
            var newSheba = removedShebaFirst4Digit + replacedGet4FirstDigit;
            ////تبدیل کردن شبا به عدد  - دسیمال تا 28 رقم را نگه میدارد
            var finalLongData = Convert.ToDecimal(newSheba);
            ////تقسیم عدد نهایی به مقدار 97 - اگر باقیمانده برابر با عدد یک شود این رشته شبا صحیح خواهد بود
            var finalReminder = finalLongData % 97;
            if (finalReminder == 1)
            {
                return 0;
                //return new ValidationResult("طول شماره شبا نمیتواند خالی باشد");
            }
            return 4;
            //return false;


        }
    }
}
