using System.ComponentModel;

namespace Common.Enums
{
    public enum ContactTypeEnum
    {
        [Description("شماره تماس ثابت  ")]
        PhoneNumber = 1,

        [Description("شماره تماس همراه")]
        MobileNumber = 2,

        [Description("شماره تماس فنی")]
        ItManNumber = 3,
    }
}
