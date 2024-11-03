using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Common.Enums
{
    public enum BankStatusEnum
    {
        [Display(Name = "پرداخت موفق")]
        PaymentDone = 0,

        [Display(Name = "تایید اولیه")]
        PriliminaryConfirmed = 1,

        [Display(Name = "در انتظار پرداخت")]
        PaymentPending = 2,

        [Display(Name = "پرداخت ابطال شده")]
        [Description("پرداخت سفارش شما کنسل شده است.")]
        PaymentCanceled = 3,

        [Display(Name = "خطا در پرداخت.")]
        [Description("پرداخت سفارش شما با مشکل مواجه شده است.")]
        PaymentError = 4,

        [Display(Name = "پرداخت تایید شده")]
        [Description("سفارش شما با موفقیت پرداخت شده است.")]
        PaymentConfirmed = 5,

        [Display(Name = "در انتظار تایید")]
        [Description("سفارش شما در حال بررسی است و در حال حاظر توسط مدیریت تایید نشده است.")]
        FinalConfirmPending = 6,

        [Display(Name = "سفارش تایید شده")]
        [Description("سفارش شما توسط مدیریت تایید شده است و تا لحظاتی دیگر برای شما لحاظ خواهد شد.")]
        Confirmed = 7,

        [Display(Name = "سفارش ابطال شده")]
        [Description("سفارش شما ابطال گردیده است")]
        Canceled = 8,

        [Display(Name = "مبلغ عودت داده شده")]
        [Description("مبلغ سفارش برای شما عودت داده شده است.")]
        RefundDone = 9,

        [Display(Name = "باطل شده توسط سیستم")]
        SystemCanceled = 10,

        [Display(Name = "باطل شده توسط کاربر")]
        [Description("سفارش توسط شما باطل شده است.")]
        UserCanceled = 11,
    }
}
