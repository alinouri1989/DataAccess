using System.ComponentModel;

namespace Common.Enums
{
    public enum SubscriptionStatus
    {

        [Description("فعال")]
        Valid = 1,

        [Description("رزرو شده")]
        Reserved = 2,

        [Description("در حال منقضی شدن")]
        Expiring = 3,

        [Description("منقضی شده")]
        Expired = 4,

        [Description("غیر فعال")]
        Deactivated = 5,

        [Description("وضعیت نا معلوم")]
        Unhandled = 6
    }
}