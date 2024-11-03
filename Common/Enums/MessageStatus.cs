using System.ComponentModel;

namespace Common.Enums
{
    public enum MessageStatusEnum
    {
        [Description("عملیات موفق")]
        Success = 0,

        [Description("درخواست شما با خطا مواجه شد")]
        Error = -1,

        [Description("احراز هوست شما نا معتبر می باشد")]
        Unauthorized = 401,

        [Description("درخواست شما نا معتبر می باشد")]
        BadRequest = 400,

        [Description("یافت نشد")]
        NotFound = 404,

        [Description("عدم دسترسی به سرویس مورد نظر")]
        Forbidden = 403
    }
}
