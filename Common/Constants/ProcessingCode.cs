namespace Common.Constants
{
    public sealed class ProcessingCode
    {
        public const byte NormalSale = 0;
        public const byte ParsianInsurance = 14;
        public const byte IranKhodroMega = 13;
        public const byte Balance = 31;
        public const byte BillPayment = 17;
        public const byte PinCharge = 15;
        public const byte UDSale = 16;
        public const byte TopUpCharge = 23;
        public const byte IsacoSale = 18;
        public const byte IsacoWithTashim = 5;
        public const byte MultiplexAccountSale = 21;
        public const byte BatchBillPayment = 177;
        public const byte OfflineMultiplexedSale = 100;
        public const byte MultiplexedSaleWithIBAN = 101;
        public const byte PrepaiedCardCharge = 150;
        public const byte GiftCardCharge = 151;
        public const byte GovermentIdSale = 25;
        public const byte GovermentIdMehranaSale = 26;
        public const byte Advice = 24;
        public const byte Reversal = 254;
        public const byte ClubAcceptable = 253;
        public const byte NormalUDSale = 29;

        public static bool IsAutoConfirmImportantForTransSuccess(byte processingCode) => processingCode != (byte)254 && processingCode != (byte)24 && processingCode != (byte)17 && processingCode != (byte)177 && processingCode != (byte)23 && processingCode != (byte)31 && processingCode != (byte)15;

        public static bool TspEnabled(byte processingCode) => processingCode != (byte)254 && processingCode != (byte)24 && processingCode != (byte)177 && processingCode != (byte)31;

        public static bool ReversalEnabled(byte processingCode) => processingCode != (byte)254 && processingCode != (byte)24 && processingCode != (byte)17 && processingCode != (byte)177 && processingCode != (byte)23 && processingCode != (byte)151 && processingCode != (byte)150 && processingCode != (byte)31;
    }
}
