using System.ComponentModel;

namespace Common.Constants;

public class StatusConstants
{
    #region [- Positive -]
    [Description("موفق")]
    public const short success = 0;

    [Description("فعال سازی انجام شد")]
    public const short Enabled = 10;

    [Description("غیر فعال سازی انجام شد")]
    public const short Disabled = 11;
    #endregion

    #region [- Negetive -]
    [Description("خطای سیستمی")]
    public const short UnSuccess = -1;

    [Description("مجوز csp مجاز نمی باشد")]
    public const short invalidLicense = -2;

    [Description("داده ای یافت نشد")]
    public const short NotFound = -3;

    [Description("کد ملی وارد شده قبلا در سامانه ثبت نام نموده است")]
    public const short ExistNationalCode = -4;

    [Description("چنین شماره قراردادی یافت نشد")]
    public const short NotFoundContractId = -5;

    [Description("وضعیت اقساط نمی تواند صفر باشد")]
    public const short StateIdNotBeZero = -6;

    [Description("وضعیت اقساط پیدا نشد")]
    public const short StateIdNotFound = -7;

    [Description("شماره فاکتور پیدا نشد")]
    public const short FactorIdNotFound = -8;


    #endregion

    [Description("تاریخ منقضی شدن نمیتواند کوچکتر از زمان حال باشد")]
    public const short invalidExpireDate = -8;
}
