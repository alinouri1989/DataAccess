using System.ComponentModel;

namespace Common.Enums
{
    public enum AttachmentTypeEnum
    {
        [Description("کارت ملی")]
        NationalCard = 1,

        [Description("شناسنامه")]
        BirthCertificate = 2,

        [Description("قرارداد")]
        Contract = 3,

    }
}