using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace San.CoreCommon.Constants
{
  public sealed class PaymentStatus
  {
    [Description("عملیات موفق")]
    public const short Successful = 0;
    [Description("Unauthorized")]
    public const short Unauthorized = -5;
    [Description("  پذیرنده غیر فعال می باشد  ")]
    public const short MerchantIsNotActive = -100;
    [Description(" پذیرنده اهراز هویت نشد ")]
    public const short MerchantAuthenticationFailed = -101;
    [Description(" تراکنش با موفقیت برگشت داده شد ")]
    public const short ReverseSuccessful = -102;
    [Description("قابلیت خرید برای پذیرنده غیر فعال می باشد ")]
    public const short SaleIsNotEnabled = -103;
    [Description("قابلیت پرداخت قبض برای پذیرنده غیر فعال می باشد")]
    public const short BillIsNotEnabled = -104;
    [Description("قابلیت تاپ آپ برای پذیرنده غیر فعال می باشد")]
    public const short TopupIsNotEnabled = -105;
    [Description("قابلیت شارژ برای پذیرنده غیر فعال می باشد")]
    public const short ChargeIsNotEnabled = -106;
    [Description("قابلیت ارسال تاییده تراکنش برای پذیرنده غیر فعال می باشد ")]
    public const short AdviceIsNotEnabled = -107;
    [Description("قابلیت برگشت تراکنش برای پذیرنده غیر فعال می باشد")]
    public const short ReverseIsNotEnabled = -108;
    [Description("  قابلیت پرداخت خرید کالای ایرانی برای پذیرنده غیر فعال می باشد  ")]
    public const short IranianGoodIsNotEnabled = -109;
    [Description(" قابلیت OCP برای پذیرنده غیر فعال می باشد ")]
    public const short OcpIsNotEnabled = -110;
    [Description(" مبلغ تراکنش بیش از حد مجاز پذیرنده می باشد ")]
    public const short InvalidMerchantMaxTransAmount = -111;
    [Description(" شناسه سفارش تکراری است ")]
    public const short OrderIdDuplicated = -112;
    [Description("پارامتر ورودی و یا برخی از خصوصیات آن خالی است و یا مقداردهی نشده است")]
    public const short ValueIsNull = -113;
    [Description("شناسه قبض نامعتبر می باشد")]
    public const short InvalidBillId = -114;
    [Description("شناسه پرداخت نامعتبر می باشد")]
    public const short InvalidPayId = -115;
    [Description("طول رشته بیش از حد مجاز می باشد")]
    public const short LenghtIsMoreOfMaximum = -116;
    [Description("طول رشته کم تر از حد مجاز می باشد")]
    public const short LenghtIsLessOfMinimum = -117;
    [Description("مقدار ارسال شده عدد نمی باشد")]
    public const short ValueIsNotNumeric = -118;
    [Description("سازمان نامعتبر می باشد")]
    public const short InvalidOrganizationId = -119;
    [Description("طول داده ورودی معتبر نمی باشد")]
    public const short InvalidLength = -120;
    [Description("رشته داده شده بطور کامل عددی نمی باشد")]
    public const short InvalidStringIsNumeric = -121;
    [Description("شماره کارت معتبر نمی باشد")]
    public const short InvalidCardNumber = -122;
    [Description("تاریخ انقضای کارت معتبر نمی باشد")]
    public const short InvalidExpireDate = -123;
    [Description("لطفا فيلد رمز اينترنتي کارت را کامل کنيد")]
    public const short InvalidPinCode = -124;
    [Description("کد CVV2 صحیح نمی باشد")]
    public const short InvalidCvv2 = -125;
    [Description("کد شناسایی پذیرنده معتبر نمی باشد")]
    public const short InvalidMerchantPin = -126;
    [Description("آدرس IP معتبر نمی باشد")]
    public const short InvalidMerchantIp = -127;
    [Description("قالب آدرس IP معتبر نمی باشد")]
    public const short InvalidIpAddressFormat = -128;
    [Description("قالب داده ورودی صحیح نمی باشد")]
    public const short InvalidInputDataFormat = -129;
    [Description("Token زمان منقضی شده است")]
    public const short TokenIsExpired = -130;
    [Description("Token نامعتبر می باشد")]
    public const short InvalidToken = -131;
    [Description("حداقل مبلغ پرداخت رعایت نشده است")]
    public const short InvalidMinimumPaymentAmount = -132;
    [Description("DelegateCode معتبر نمی باشد")]
    public const short InvalidIsacoDelegateCode = -133;
    [Description("DelegatePass معتبر نمی باشد")]
    public const short InvalidIsacoDelegatePass = -134;
    [Description("CheckDigit معتبر نمی باشد")]
    public const short InvalidIsacoCheckDigit = -135;
    [Description("DelegateCode length is less than or equal to CheckDigit.")]
    public const short InvalidDelegateCodeLengthComparedToCheckDigit = -136;
    [Description("DelegateCode does not end with CheckDigit.")]
    public const short InvalidDelegateCodeEndsWithCheckDigit = -137;
    [Description("تراکنش توسط کاربر کنسل شده است")]
    public const short CanceledByUser = -138;
    [Description("کد امنیتی وارد شده صحیح نمی باشد")]
    public const short InvalidCaptcha = -139;
    [Description("رمز دوم معتبر نمی باشد")]
    public const short InvalidPin2Format = -140;
    [Description("طول رمز دوم معتبر نمی باشد")]
    public const short InvalidPin2Length = -141;
    [Description("شناسه قبض یا پرداخت معتبر نمی باشد")]
    public const short InvalidBillIdOrPayId = -142;
    [Description("جمع مبالغ تسهیم با مبلغ تراکنش مغایرت دارد")]
    public const short OnlineMultiplexedSaleSumAmountNotEqualsRequestAmount = -143;
    [Description("مبلغ در برخی از اقلام معتبر نمی باشد")]
    public const short InvalidAmountInSomeOfItems = -144;
    [Description("اقلام تسهیم تعیین نشده است")]
    public const short InvalidMultiplexItems = -145;
    [Description("شماره موبایل برای شارژ معتبر نمی باشد")]
    public const short InvalidTopupChargeMobileNumber = -146;
    [Description("نوع شارژ معتبر نمی باشد")]
    public const short InvalidTopupType = -147;
    [Description("پارامتر ورودی معتبر نمی باشد")]
    public const short InvalidInputParameter = -148;
    [Description("برگشت تراکنش شارژ کارت امکانپذیر نمی باشد")]
    public const short CardChargeReversalIsNotSupported = -149;
    [Description("قابلیت شارژ کارت پیش پرداخت برای پذیرنده غیرفعال است")]
    public const short PrepaiedCardChargeIsNotEnabled = -150;
    [Description("قابلیت شارژ کارت هدیه برای پذیرنده غیرفعال است")]
    public const short GiftCardChargeIsNotEnabled = -151;
    [Description("عدم وجود شناسه حساب در داده اضافی")]
    public const short GovermentIdSale_NoIdFoundInAdditionalData = -152;
    [Description("قالب شناسه حساب صحیح نمی باشد")]
    public const short GovermentIdSale_InvalidIdFormat = -153;
    [Description("Either Card Number or Card Index parameters must be sSanified.")]
    public const short NonOfCardNumberOrCardIndexIsSSanified = -154;
    [Description("مجوز پرداخت تک فاز اعطا نشده است")]
    public const short SinglePhasedServicesNotEnabled = -155;
    [Description("Invalid CallBack Url.")]
    public const short InvalidCallBackUrlFormat = -156;
    [Description("درخواست حاوی شماره شبای نامعتبر است")]
    public const short WithIBANMultiplexedSalePaymentRequestHasInvalidIbans = -157;
    [Description("Invalid length of PayId in one or more Account item(s).")]
    public const short WithIBANMultiplexedSalePaymentRequestHasInvalidPayIds = -158;
    [Description("Invalid number of Accounts. can not exceed 9 items.")]
    public const short WithIBANMultiplexedSalePaymentRequestHasInvalidAccountsCount = -159;
    [Description("قابلیت مانده گیری برای پذیرنده غیرفعال است")]
    public const short balanceIsNotEnabled = -160;
    [Description(" خطا در درج اطلاعات ")]
    public const short InsertPaymentRequestException = -500;
    [Description(" خطا در برقراری ارتباط شبکه ای با دیتابیس ")]
    public const short DbLanTimeOut = -501;
    [Description("POST ISO Update")]
    public const short PreUpdateIsoResponseStepStatus = -502;
    [Description("خطا در دریافت اطلاعات از سوئیچ")]
    public const short ReceiveError = -1000;
    [Description("خطا در اتصال و یا دریافت اطلاعات از  سوئیچ Network Timeout")]
    public const short LanTimeout = -1001;
    [Description("خطا در ارسال اطلاعات به سوئیچ")]
    public const short SendError = -1002;
    [Description("خطا در انجام تراکنش روی سوئیچ")]
    public const short IsoDoTransactionException = -1003;
    [Description("طول داده دریافتی از سوئیچ نامعتبر است")]
    public const short IsoTransactionReadStreamLengthIsZeroException = -1004;
    [Description("Iso Response Parse Error")]
    public const short IsoResponseParseException = -1005;
    [Description("Can not get ISO Response Code from Message")]
    public const short IsoResponseCodeParseException = -1006;
    [Description("ISO Send Receive Error")]
    public const short PerformIsoSendReceiveException = -1007;
    [Description(" ثبت اولیه درخواست ")]
    public const short PaymentRequestIsInitialized = -1500;
    [Description(" کاربر وارد صفحه درگاه پرداخت اینترتی شد ")]
    public const short IpgPageLoaded = -1502;
    [Description(" تراکنش پرداخت به سوئیچ ارسال شد ")]
    public const short PaymentIsSentToSwitch = -1503;
    [Description(" درگاه پرداخت اینترنتی صفحه پرداخت را به پذیرنده ارجاع داد ")]
    public const short IpgCalledbackMerchant = -1504;
    [Description(" تایید تراکنش توسط پذیرنده انجام شد ")]
    public const short PaymentConfirmRequested = -1505;
    [Description(" تراکنش تاییدیه به سوئیچ ارسال شد ")]
    public const short AdviceCompleted = -1506;
    [Description(" تراکنش برگشت به سوئیچ ارسال شد ")]
    public const short ReversalCompleted = -1507;
    [Description(" درج مرحله ثبت درخواست اولیه پرداخت ناموفق می باشد ")]
    public const short InsertPaymentRequestAuditRequestIniializedFailed = -1508;
    [Description(" درج مرحله ارسال تراکنش به سویچ ناموفق می باشد ")]
    public const short InsertPaymentRequestAuditPaymentIsSentToSwitchFailed = -1509;
    [Description(" درج مرحله ارجاع به سایت پذیرنده ناموفق می باشد ")]
    public const short InsertPaymentRequestAuditIpgCalledbackMerchantFailed = -1510;
    [Description(" درج مرحله درخواست تایید تراکنش  ناموفق می باشد ")]
    public const short InsertPaymentRequestAuditPaymentConfirmRequestedFailed = -1511;
    [Description(" درج مرحله ارسال تراکنش تاییده  به سویج  ناموفق می باشد ")]
    public const short InsertPaymentRequestAuditAdviceCompletedFailed = -1512;
    [Description(" درج مرحله ارسال تراکنش برگشت  به سویچ ناموفق می باشد ")]
    public const short InsertPaymentRequestAuditReversalCompletedFailed = -1513;
    [Description(" درج مرحله بارگذاری صفحه درگاه پرداخت ناموفق می باشد ")]
    public const short InsertPaymentRequestAuditIpgPageLoadedFailed = -1514;
    [Description(" درج اطلاعات درخواست پرداخت ناموفق می باشد ")]
    public const short InsertPaymentRequestFailed = -1515;
    [Description(" درج اطلاعات تراکنش پرداخت ناموفق می باشد ")]
    public const short InsertTransactionFailed = -1516;
    [Description(" کاربر دکمه پرداخت را فشرده است ")]
    public const short StartPayment = -1517;
    [Description(" درج مرحله شروع پرداخت ناموفق می باشد ")]
    public const short InsertStartPaymentAuditIpgPageLoadedFailed = -1518;
    [Description(" تراکنش آماده ارسال به سوئیچ می باشد ")]
    public const short PaymentIsAboutToBeSentToSwitch = -1519;
    [Description(" درج مرحله آماده بودن تراکنش برای ارسال به سویچ ناموفق می باشد ")]
    public const short InsertPaymentRequestAuditPaymentIsAboutToBeSentToSwitchFailed = -1520;
    [Description(" Iso Creation failed. ")]
    public const short IsoCreationFailed = -1521;
    [Description(" Unable to find PaymentSwitchConfig record for current transaction request. ")]
    public const short UnableToFindPaymentSwitchConfigRecord = -1522;
    [Description("پردازش فرآیند انجام تراکنش ناموفق می باشد")]
    public const short TransactionRequestProcessRequestCoreException = -1523;
    [Description(" درج اطلاعات پرداخت بیمه پارسیان ناموفق بود ")]
    public const short InsertParsianInsurancePaymentInfoFailed = -1524;
    [Description("عملیات پرداخت به اتمام رسیده است ")]
    public const short EndPayment = -1525;
    [Description(" درج مرحله پایان پرداخت ناموفق می باشد ")]
    public const short InsertEndPaymentAuditIpgPageLoadedFailed = -1526;
    [Description(" انجام عملیات درخواست پرداخت تراکنش خرید ناموفق بود ")]
    public const short CallSalePaymentRequestServiceFailed = -1527;
    [Description(" اطلاعات درخواست یافت نشد ")]
    public const short ConfirmPaymentRequestInfoNotFound = -1528;
    [Description(" بازیابی اطلاعات درخواست برای تایید تراکنش ناموفق بود ")]
    public const short GetConfirmPaymentRequestInfoFailed = -1529;
    [Description(" پذیرنده مجاز به تایید این تراکنش نمی باشد ")]
    public const short MerchantConfirmPaymentRequestAccessVaiolated = -1530;
    [Description(" تایید تراکنش ناموفق امکان پذیر نمی باشد ")]
    public const short CannotConfirmNonSuccessfulPayment = -1531;
    [Description(" تراکنش از سوی پذیرنده تایید شد ")]
    public const short MerchantHasConfirmedPaymentRequest = -1532;
    [Description(" تراکنش قبلاً تایید شده است ")]
    public const short PaymentIsAlreadyConfirmed = -1533;
    [Description(" Successful Audit Time was not found. ")]
    public const short SuccessfulAuditTimeNotFound = -1534;
    [Description(" Insert MerchantHasConfirmedPaymentRequest failed. ")]
    public const short InsertMerchantHasConfirmedPaymentRequestFailed = -1535;
    [Description(" فراخوانی سرویس درخواست شارژ تاپ آپ ناموفق بود ")]
    public const short TopupChargeServiceTopupChargeRequestFailed = -1536;
    [Description(" An error occured in BeforeSaveRequest ")]
    public const short BeforeSaveRequestFailed = -1537;
    [Description(" اعتبارسنجی اطلاعات درخواست برای تایید ناموفق است")]
    public const short ValidateBeforeConfirmFailed = -1538;
    [Description(" Payment is not successful to advice. ")]
    public const short InvalidPaymentStatusToAdvice = -1539;
    [Description(" تایید تراکنش ناموفق می باشد ")]
    public const short InvalidConfirmRequestService = -1540;
    [Description(" درج وضعیت انجام تراکنش در سوئیچ ناموفق می باشد ")]
    public const short InsertPaymentRequestAuditTransactionSwitchResponseFailed = -1541;
    [Description(" تراکنش تاییدیه در انتظار برای ارسال به سوئیچ است ")]
    public const short AdviceTransactionIsAboutToBeSentToSwitch = -1542;
    [Description(" درج وضعیت تراکنش پیش از ارسال تراکنش تاییدیه ناموفق بود ")]
    public const short InsertAdviceTransactionIsAboutToBeSentToSwitchFailed = -1543;
    [Description(" تراکنش تاییدیه با موفقیت به سوئیچ ارسال شد ")]
    public const short AdviceTransactionIsSentToSwitch = -1544;
    [Description(" درج وضعیت تراکنش پیش از ارسال تراکنش تاییدیه ناموفق بود ")]
    public const short InsertAdviceTransactionIsSentToBeSentToSwitchFailed = -1545;
    [Description(" تراکنش برگشت در انتظار برای ارسال به سوئیچ است ")]
    public const short ReversalTransactionIsAboutToBeSentToSwitch = -1546;
    [Description(" تراکنش برگشت به سوئیچ ارسال شد ")]
    public const short ReversalTransactionIsSentToSwitch = -1547;
    [Description(" فراخوانی سرویس درخواست پرداخت قبض ناموفق بود ")]
    public const short BillPaymentRequestServiceFailed = -1548;
    [Description(" زمان مجاز برای درخواست برگشت تراکنش به اتمام رسیده است ")]
    public const short MaxAllowedTimeToReversalHasExceeded = -1549;
    [Description(" برگشت تراکنش در وضعیت جاری امکان پذیر نمی باشد ")]
    public const short PaymentRequestStatusIsNotReversalable = -1550;
    [Description(" برگشت تراکنش قبلاً انجام شده است ")]
    public const short PaymentRequestIsAlreadyReversed = -1551;
    [Description(" برگشت تراکنش مجاز نمی باشد ")]
    public const short PaymentRequestIsNotEligibleToReversal = -1552;
    [Description(" خطا در درج درخواست قبض گروهی ")]
    public const short InsertBatchBillPaymentRequestFailed = -1553;
    [Description(" تعدادی از قبوض داده شده پذیرش شد. وضعیت قبوض را بررسی نمایید ")]
    public const short BatchBillPaymentRequestWasValidForSomeOfItems = -1554;
    [Description(" هیچ یک از قبوض داده شده معتبر نبود. وضعیت قبوض را بررسی نمایید ")]
    public const short BatchBillPaymentRequestWasNotValidForAllItems = -1555;
    [Description("صفحه درگاه پرداخت پارسیان بارگزاری شد ")]
    public const short IpgPageLoadEnd = -1560;
    [Description(" تاییده تراکنش ناموفق می باشد -مشکلی در سیستم رخ داده لطفامجددا تلاش نمایید ")]
    public const short ConfirmTransactionRequestFailed = -1561;
    [Description("بخشی از قبوض پرداخت شده و بخشی دچار مشکل شده است  ")]
    public const short ThePartOfBillPaymentIsDone = -1562;
    [Description(" تمامی پرداخت ها ناموفق می باشد ")]
    public const short PaymentFailed = -1563;
    [Description(" فراخوانی سرویس درخواست   خرید UD ناموفق بود ")]
    public const short UDSaleServiceRequestFailed = -1578;
    [Description("عملیات برگشت تراکنش در سرویس برگشت تراکنش ناموفق بود")]
    public const short ReversalServiceReversalPaymentException = -1585;
    [Description("اشکال در عملیات پرداخت تک فاز")]
    public const short SinglePhasedPayment_Exception = -1564;
    [Description("اشکال در عملیات پرداخت قبض بصورت تک فاز")]
    public const short SinglePhasedBillPayment_Exception = -1565;
    [Description("نتیجه تراکنش معتبر نمی باشد")]
    public const short SinglePhasedPayment_TransactionResponseIsNotValid = -1566;
    [Description("پرداخت قبض داده شده به دلیل محدودیت امکانپذیر نمی باشد")]
    public const short BillPaymentForMerchantIsLimited = -1567;
    [Description("مدل صفحه پرداخت معتبر نبود")]
    public const short IPGPagePostedModelWasNotValid = -1568;
    [Description("کاپچا معتبر نبود")]
    public const short IPGPagePostedModelCaptchaWasNotValid = -1569;
    [Description("فرآیند پرداخت در صفحه درگاه پرداخت با اشکالی مواجه شد")]
    public const short IPGPaymentProcessExceptionOccuredInIPGPage = -1570;
    [Description("اشکال در بارگذاری صفحه پرداخت")]
    public const short IPGPageLoadException = -1571;
    [Description("پیش از اجرای متد ایندکس صفحه پرداخت")]
    public const short IPGIndexActionIsExecuting = -1572;
    [Description("پس از اجرای متد ایندکس صفحه پرداخت")]
    public const short IPGIndexActionIsExecuted = -1573;
    [Description("خطا در اجرای متد ایندکس صفحه پرداخت")]
    public const short IPGIndexActionExecuteException = -1574;
    [Description("صفحه درگاه در کلاینت بارگزاری شد")]
    public const short IPGPageClientDocumentReady = -1575;
    [Description("اشکال در عملیات خرید معمولی بصورت تک فاز")]
    public const short SinglePhasedNormalSalePayment_Exception = -1576;
    [Description("اشکال در عملیات خرید شارژ تاپ آپ بصورت تک فاز")]
    public const short SinglePhasedTopupCharge_Exception = -1577;
    [Description("اشکال در عملیات تراکنش اینترنتی خرید MPL")]
    public const short MplInternetSaleTransactionRequestWebServiceException = -1579;
    [Description("اشکال در عملیات تراکنش اینترنتی قبض MPL")]
    public const short MplInternetBillTransactionRequestWebServiceException = -1580;
    [Description("اشکال در عملیات تراکنش شارژ تاپ آپ MPL")]
    public const short MplTopupChargeTransactionRequestWebServiceException = -1581;
    [Description("اشکال در عملیات تراکنش موبایل خرید MPL")]
    public const short MplMobileSaleTransactionRequestWebServiceException = -1582;
    [Description("اشکال در عملیات تراکنش موبایل قبض MPL")]
    public const short MplMobileBillTransactionRequestWebServiceException = -1583;
    [Description("اشکال در عملیات تراکنش موبایل خرید MPL")]
    public const short MplMobileUDTransactionRequestWebServiceException = -1584;
    [Description("خطا در تبدیل پاسخ ایزو به خروجی BusinessRule")]
    public const short ConvertIsoResponseToPgwResponseDataFailure = -1586;
    [Description("Invalid PaymentRequestData in TransactionRule")]
    public const short TransactionRuleBase_InvalidPaymentRequestData = -1587;
    [Description("IsoWrapper.DoTransaction failure in TransactionRule")]
    public const short TransactionRuleBase_IsoWrapperDoTransactionException = -1588;
    [Description("نقض قالب مقدار عددی")]
    public const short InvalidNumericValue = -1589;
    [Description("شماره موبایل شارژ شونده معتبر نمی باشد")]
    public const short InvalidSimChargeRequestChargeMobileNumber = -1590;
    [Description("توکن کارت معتبر نمی باشد")]
    public const short InvalidCardTspToken = -1591;
    [Description("اشکال در عملیات بازیابی اطلاعات تراکنش خرید ام پی ال")]
    public const short PaymentInfoRules_GetMPLSaleTransactionInfoByTokenException = -1592;
    [Description("اشکال در عملیات بازیابی اطلاعات تراکنش قبض ام پی ال")]
    public const short GetBillInfoRules_GetMPLBillTransactionInfoByTokenException = -1593;
    [Description("اشکال در عملیات بازیابی اطلاعات تراکنش تاپ آپ ام پی ال")]
    public const short TopupChargeInfoRules_GetMPLTopupChargeTransactionInfoByTokenException = -1594;
    [Description("اشکال در عملیات بازیابی اطلاعات تراکنش خرید سرویس پرداخت")]
    public const short PaymentInfoRules_GetPaymentServiceSaleTransactionInfoByTokenException = -1595;
    [Description("اشکال در عملیات بازیابی اطلاعات تراکنش قبض سرویس پرداخت")]
    public const short GetBillInfoRules_GetPaymentServiceBillTransactionInfoByTokenException = -1596;
    [Description("اشکال در عملیات بازیابی اطلاعات تراکنش تاپ آپ سرویس پرداخت")]
    public const short TopupChargeInfoRules_GetPaymentServiceTopupChargeTransactionInfoByTokenException = -1597;
    [Description("اشکال در عملیات تراکنش اینترنتی خرید سرویس پرداخت")]
    public const short PaymentServiceInternetSaleTransactionRequestWebServiceException = -1598;
    [Description("اشکال در عملیات تراکنش اینترنتی قبض سرویس پرداخت")]
    public const short PaymentServiceInternetBillTransactionRequestWebServiceException = -1599;
    [Description("اشکال در عملیات تراکنش شارژ تاپ آپ سرویس پرداخت")]
    public const short PaymentServiceTopupChargeTransactionRequestWebServiceException = -1600;
    [Description("اشکال در عملیات تراکنش موبایل خرید سرویس پرداخت")]
    public const short PaymentServiceMobileSaleTransactionRequestWebServiceException = -1601;
    [Description("اشکال در عملیات تراکنش موبایل قبض سرویس پرداخت")]
    public const short PaymentServiceMobileBillTransactionRequestWebServiceException = -1602;
    [Description("اشکال در عملیات تراکنش موبایل خرید سرویس پرداخت")]
    public const short PaymentServiceMobileUDTransactionRequestWebServiceException = -1603;
    [Description("Update transaction sw response failed.")]
    public const short UpdateTransactionIsoResponseFailed = -1604;
    [Description("Iso Advice failed.")]
    public const short IsoAdviceFailedAudit = -1605;
    [Description("Iso Reversal failed.")]
    public const short IsoReversalFailedAudit = -1606;
    [Description("اشکال در عملیات پرداخت قبض بصورت تک فاز")]
    public const short ShetabSinglePhasedBillPayment_Exception = -1607;
    [Description("خطای بانک در پرداخت قبض.به کد وضعیت بانک مراجعه شود")]
    public const short ShetabSinglePhasedBillPaymentParsianException = -1608;
    [Description("خطای بانک در پرداخت. به کد وضعیت بانک مراجعه شود")]
    public const short ShetabSinglePhasedUDSalePayment_Exception = -1609;
    [Description("خطای بانک در پرداخت.به کد وضعیت بانک مراجعه شود")]
    public const short ShetabSinglePhasedUDPaymentParsianException = -1610;
    [Description("اشکال در بازیابی شماره سفارش جدید")]
    public const short IVRHamrahAvalShetabUDPaymentGetNewOrderIdException = -1611;
    [Description("Exception:MPLServices.ValidationRequest")]
    public const short MplServices_ValidationRequest_Exception = -1612;
    [Description("Exception:MerchantInfoRules.GetMplMerchantInfo")]
    public const short MerchantInfoRules_GetMplMerchantInfo_Exception = -1613;
    [Description("Exception:PaymentRequestQR Save")]
    public const short PaymentRequestQR_Insert_Exception = -1614;
    [Description("استثنا در بازیابی اطلاعات پرداخت QR")]
    public const short QRPayment_GetTokenInfo_Exception = -1615;
    [Description("استثنای ناشناخته در پرداخت")]
    public const short QRPayment_Pay_Exception = -1616;
    [Description("خطای بانک در شارژ تاپآپ. به کد وضعیت بانک مراجعه شود")]
    public const short ShetabSinglePhasedUDTopupChargePayment_Exception = -1617;
    [Description("تاپ آپ توسط تامین کننده ناموفق بود. جهت رفع مغایرت با مرکز تماس به شماره 021-2318 تماس بگیرید")]
    public const short SinglePhasedShetabUDRayanmehrTopupPayment_TopupOnRayanmehrFailed = -1618;
    [Description("تاپ آپ در واسط سرویس ناموفق بود. جهت رفع مغایرت با مرکز تماس به شماره 021-2318 تماس بگیرید")]
    public const short SinglePhasedShetabUDRayanmehrTopupPayment_TopupOnProxyFailed = -1619;
    [Description("تاپ آپ در درگاه ناموفق بود. جهت رفع مغایرت با مرکز تماس به شماره 021-2318 تماس بگیرید")]
    public const short SinglePhasedShetabUDRayanmehrTopupPayment_TopupOnPgwSideFailed = -1620;
    [Description("انجام تراکنش محدود به کارت های پذیرنده می باشد")]
    public const short MerchantCardRestrictionControlRejectedCard = -1621;
    [Description("انجام تراکنش محدود به کارت های مشتری می باشد")]
    public const short MerchantCardRestrictionControlWithNahabRejectedCard = -1689;
    [Description("استثنا در کنترل محدودیت پذیرنده برای شماره کارت")]
    public const short MerchantCardRestrictionValidationException = -1622;
    [Description("استثنای فراخوانی سرویس صادرکننده")]
    public const short IssuerCardChargeServiceProxyCallException = -1623;
    [Description("شارژ کارت ناموفق می باشد")]
    public const short IssuerCardChargeResultWasNotSuccessful = -1624;
    [Description("تراکنشی برای درخواست انجام نشده است که قابل برگشت باشد و یا برگشت تراکنش برای درخواست، غیرمجاز می باشد")]
    public const short InvalidReversalRequestForNonTransactionedPaymentRequest = -1625;
    [Description("انجام تراکنش روی این درگاه مجاز نمی باشد")]
    public const short InvalidTransactionRequestBasedOnCurrentMerchantsSwitchId = -1626;
    [Description("اندیس کارت معتبر نمی باشد")]
    public const short InvalidCardTspId = -1627;
    [Description("اندیس پیوند کارت معتبر نمی باشد")]
    public const short InvalidPayvandCardIndexId = -1628;
    [Description("Batch Transaction Exception.")]
    public const short BatchBillTransactionRuleProcessRequestException = -1629;
    [Description("Failed to continue batch bill transactions, because of a not continuable status occured.")]
    public const short BatchBillTransactionRuleProcessRequestNotContinuable = -1630;
    [Description("Some of bill items transaction was successful.")]
    public const short BatchBillTransactionRuleProcessRequestSomeWasSuccessful = -1631;
    [Description("No batch bill request items found to process.")]
    public const short NoBatchBillRequestItemsFoundToProcessForTransaction = -1632;
    [Description("Non of batch bill request items transaction was successful.")]
    public const short NonOfBatchBillRequestItemsTransactionWasSuccessful = -1633;
    [Description("Single-Phased USSD Bill Payment Exception")]
    public const short SinglePhasedUSSDBillPaymentException = -1634;
    [Description("Single-Phased USSD Sale Payment Exception")]
    public const short SinglePhasedUSSDSalePaymentException = -1635;
    [Description("Single-Phased USSD Topup Charge Payment Exception")]
    public const short SinglePhasedUSSDTopupChargePaymentException = -1636;
    [Description("Single-Phased UD Payment SW2 exception")]
    public const short SinglePhasedUSSDSalePaymentSW2Exception = -1637;
    [Description(" فراخوانی سرویس درخواست شارژ تاپ آپ ناموفق بود ")]
    public const short TopupChargeServiceSW2TopupChargeRequestFailed = -1638;
    [Description("Audit save repository exception")]
    public const short InsertAuditRepositoryException = -1639;
    [Description("قابلیت تراکنش برگشت برای این نوع تراکنش ایجاد نشده است")]
    public const short ReversalIsNotDefinedOrImplementedForThisTypeOfTrans = -1640;
    [Description("بازیابی اطلاعات درخواست با اشکال مواجه شد")]
    public const short IPGPaymentInfoRule_GetPaymentRequestInfoException = -1641;
    [Description("ذخیره اطلاعات درخواست موفق نبود")]
    public const short OfflineMultiplexedSalePaymentRequestRules_SaveRequestForGetTokenException = -1642;
    [Description("ذخیره اطلاعات درخواست پرداخت قبض موفق نبود")]
    public const short BillPaymentRequestRule_SaveSSanificRequestDataInvalidResult = -1643;
    [Description("ذخیره اطلاعات درخواست پرداخت قبض با اشکال مواجه شد")]
    public const short BillPaymentRequestRule_SaveSSanificRequestDataInvalidException = -1644;
    [Description("اعتبارسنجی ویژه درخواست پرداخت قبض با اشکال مواجه شد")]
    public const short BillPaymentValidationRule_PerformSSanificValidationException = -1645;
    [Description("ذخیره اطلاعات درخواست خرید شارژ تاپ آپ سیم کارت موفق نبود")]
    public const short TopupChargePaymentRequestRule_SaveSSanificRequestDataInvalidResult = -1646;
    [Description("ذخیره اطلاعات درخواست خرید شارژ تاپ آپ سیم کارت با اشکال مواجه شد")]
    public const short TopupChargePaymentRequestRule_SaveSSanificRequestDataException = -1647;
    [Description("ذخیره اطلاعات اختصاصی درخواست موفق نبود")]
    public const short PaymentRequestRuleBase_SaveSSanificRequestDataInvalidResult = -1648;
    [Description("ذخیره اطلاعات درخواست پرداخت قبض بصورت گروهی با اشکال مواجه شد")]
    public const short BatchBillPaymentRequestRule_ProcessBillRequestItemsException = -1649;
    [Description("داده الگوریتم خالی است")]
    public const short AlgorithmIsNull = -1650;
    [Description("قالب ورودی الگوریتم صحیح نیست")]
    public const short AlgorithmFormatIsNotValid = -1651;
    [Description("داده ی ورودی با الگوریتم مورد نظر مطابقت ندارد")]
    public const short AlgorithmIsNotSatisfied = -1652;
    [Description("شناسه الگوریتم صحیح نمی باشد")]
    public const short AlgorithmIdIsNotValid = -1653;
    [Description("کد مرجع تراکنش تکراری است")]
    public const short ExRRNDuplicate = -1654;
    [Description("قبض با مشخصات وارد شده پیدا نشد")]
    public const short ExRecordNotFound = -1655;
    [Description("کد مرجع تراکنش برای این قبض، قبلا ثبت شده است")]
    public const short ExRecordUpdatedInThePast = -1656;
    [Description("تراکنش پرداخت قبض ناموفق است")]
    public const short BillPaymentTransactionWasNotSucceed = -1657;
    [Description("خطای سرور")]
    public const short ServerError = -1;
    [Description(" صادرکننده ی کارت از انجام تراکنش صرف نظر کرد ")]
    public const short ReferToCardIssuerDecline = 1;
    [Description(" عملیات تاییدیه این تراکنش قبلا باموفقیت صورت پذیرفته است ")]
    public const short ReferToCardIssuerSSanialConditions = 2;
    [Description(" پذیرنده ی فروشگاهی نامعتبر می باشد ")]
    public const short InvalidMerchant = 3;
    [Description(" کارت توسط دستگاه ضبط شود ")]
    public const short PickUp = 4;
    [Description(" از انجام تراکنش صرف نظر شد ")]
    public const short DoNotHonour = 5;
    [Description(" بروز خطايي ناشناخته ")]
    public const short Error = 6;
    [Description(" به دلیل شرایط خاص کارت توسط دستگاه ضبط شود ")]
    public const short PickUpCardSSanialCondition = 7;
    [Description(" باتشخیص هویت دارنده ی کارت، تراکنش موفق می باشد ")]
    public const short HonourWithIdentification = 8;
    [Description(" درخواست رسيده در حال پي گيري و انجام است  ")]
    public const short RequestInprogress = 9;
    [Description(" تراکنش با مبلغي پايين تر از مبلغ درخواستي ( کمبود پول ATM يا حساب مشتري ) پذيرفته شده است  ")]
    public const short ApprovedForPartialAmount = 10;
    [Description(" تراکنش با وجود احتمالي برخي مشکلات پذيرفته شده است ( به علت چايگاه مشتري VIP) ")]
    public const short ApprovedVip = 11;
    [Description(" تراکنش نامعتبر است ")]
    public const short InvalidTransaction = 12;
    [Description(" مبلغ تراکنش اصلاحیه نادرست است ")]
    public const short InvalidAmount = 13;
    [Description("شماره کارت ارسالی نامعتبر است (وجود ندارد)")]
    public const short SwInvalidCardNumber = 14;
    [Description(" صادرکننده ی کارت نامعتبراست (وجود ندارد) ")]
    public const short NoSuchIssuer = 15;
    [Description(" تراکنش مورد تایید است و اطلاعات شیار سوم کارت به روز رسانی شود ")]
    public const short ApprovedUpdateTrack3 = 16;
    [Description("مشتري درخواست کننده حذف شده است   ")]
    public const short CustomerCancellation = 17;
    [Description(" در مواقعي که يک تراکنش به هر دليل پذيرفته نشده است و يا با شرايط خاصي پذيرفته شده است در صورت تاييد و يا سماجت مشتري اين پيغام را خواهيم داشتپ ")]
    public const short CustomerDispute = 18;
    [Description(" تراکنش مجددا ارسال شود ")]
    public const short ReenterTransaction = 19;
    [Description(" در موقعيتي که سوئيچ جهت پذيرش تراکنش نيازمند پرس و جو از کارت است ممکن است درخواست از کارت ( ترمينال) بنمايد اين پيام مبين نامعتبر بودن جواب است ")]
    public const short InvalidResponse = 20;
    [Description(" در صورتي که پاسخ به در خواست ترمينا ل نيازمند هيچ پاسخ خاص يا عملکردي نباشيم اين پيام را خواهيم داشت  ")]
    public const short NoActionTacken = 21;
    [Description(" تراکنش مشکوک به بد عمل کردن ( کارت ، ترمينال ، دارنده کارت ) بوده است لذا پذيرفته نشده است ")]
    public const short SusSantedMalfunction = 22;
    [Description(" کارمزد ارسالی پذیرنده غیر قابل قبول است ")]
    public const short UnAcceptableTransactionFee = 23;
    [Description("زماني که يک تراکنش نيازمند عمل کرد يا فراخواني فايلي خاص باشد و فايل مذکرو در مبدا درخواست موجود نباشد  اين پيام را خواهيم داشت   ")]
    public const short FileActionNotSupportedByReceiver = 24;
    [Description("تراکنش اصلی یافت نشد  ")]
    public const short UnableToLocateRecordOnFile = 25;
    [Description(" عمليات فايل تکراري ")]
    public const short DuplicateFileActionRecord = 26;
    [Description(" خطا در اصلاح فيلد اطلاعاتي ")]
    public const short FileActionFieldEditError = 27;
    [Description(" فايل مورد نظر lock  شده است ")]
    public const short FileActionFileLockedOut = 28;
    [Description(" عمليات فايل ناموفق ")]
    public const short FileActionNotSuccessful = 29;
    [Description("قالب پیام دارای اشکال است  ")]
    public const short FormatError = 30;
    [Description(" پذیرنده توسط سوئی پشتیبانی نمی شود ")]
    public const short BankNotSupportedBySwitch = 31;
    [Description(" تراکنش به صورت غير قطعي کامل شده است ( به عنوان مثال تراکنش سپرده گزاري که از ديد مشتري کامل شده است ولي مي بايست تکميل گردد ")]
    public const short CompletedPartially = 32;
    [Description(" تاریخ انقضای کارت سپری شده است. ")]
    public const short ExpiredCardPickUp = 33;
    [Description(" تراکنش اصلی باموفقیت انجام نپذیرفته است ")]
    public const short SusSantedFraudPickUp = 34;
    [Description(" بنابر توصيه موسسه يا بانک مدير کارت به پذيرنده کارت ضبط  شده است  ")]
    public const short CardAcceptorContactAqcuirerPickUp = 35;
    [Description(" کارت محدود شده است.  ")]
    public const short RestrictedCardPickUp = 36;
    [Description("پذيرنده در نتيجه چنين درخواستي با بخش امنيتي موسسه يا بانک مدير کارت تماس گرفته است ( يا ميگيرد )  ")]
    public const short CardAcceptorCallAqcuirerSecurity = 37;
    [Description(" تعداد دفعات ورود رمزغلط بیش از حدمجاز است. ")]
    public const short AllowablePinTriesExceededPickUp = 38;
    [Description(" کارت حساب اعتباری ندارد ")]
    public const short NoCreditAcount = 39;
    [Description(" عملیات درخواستی پشتیبانی نمی گردد ")]
    public const short RequestedFunction = 40;
    [Description(" کارت مفقودی می باشد.  ")]
    public const short LostCard = 41;
    [Description(" کارت حساب عمومی ندارد ")]
    public const short NoUniversalAccount = 42;
    [Description(" کارت مسروقه می باشد.  ")]
    public const short StolenCard = 43;
    [Description(" کارت حساب سرمایه گذاری ندارد ")]
    public const short NoInvestmentAcount = 44;
    [Description(" قبض قابل پرداخت نمی باشد ")]
    public const short BillCannotBePayed = 45;
    [Description("موجودی کافی نمی باشد  ")]
    public const short NoSufficientFunds = 51;
    [Description(" کارت حساب جاری ندارد ")]
    public const short NoChequeAccount = 52;
    [Description(" کارت حساب مرض الحسنه ندارد ")]
    public const short NoSavingAccount = 53;
    [Description(" تاریخ انقضای کارت سپری شده است ")]
    public const short ExpiredAccount = 54;
    [Description(" رمز کارت نا معتبر است ")]
    public const short IncorrectPin = 55;
    [Description("کارت نا معتبر است  ")]
    public const short NoCardRecord = 56;
    [Description(" انجام تراکنش مربوطه توسط دارنده ی کارت مجاز نمی باشد ")]
    public const short TransactionNotPermittedToCardHolder = 57;
    [Description(" انجام تراکنش مربوطه توسط پایانه ی انجام دهنده مجاز نمی باشد ")]
    public const short TransactionNotPermittedToTerminal = 58;
    [Description(" کارت مظنون به تقلب است ")]
    public const short SusSantedFraudDecline = 59;
    [Description("بنابر توصيه موسسه يا بانک مدير کارت به پذيرنده کارت ، تراکنش درخواستي پذيرفته نمي شود  ")]
    public const short CardAcceptorContactAqcuirerDecline = 60;
    [Description("مبلغ تراکنش کمتر از حد تعیین شده توسط صادرکننده کارت و یا بیشتر از حد مجاز می باشد")]
    public const short ExceedsWithdrawalAmountLimit = 61;
    [Description(" کارت محدود شده است ")]
    public const short RestrictedCardDecline = 62;
    [Description(" تمهیدات امنیتی نقض گردیده است ")]
    public const short SecurityViolation = 63;
    [Description(" مبلغ تراکنش اصلی ن امعتبر است. (تراکنش مالی اصلی با این مبلغ نمی باشد) ")]
    public const short OriginalAmountIncorrect = 64;
    [Description(" تعداد درخواست تراکنش بیش از حد مجاز می باشد ")]
    public const short ExceedsWithdrawalFrequencyLimit = 65;
    [Description(" در پي تراکنش درخواستي پذيرنده با بخش امنيتي موسسه يا بانک تماس گرفته است ( و يا ميگيرد ) ")]
    public const short CardAcceptorCallAqcuirersSecurityDepartment = 66;
    [Description("کارت توسط دستگاه ضبط شود  ")]
    public const short HardCapture = 67;
    [Description(" پاسخ لازم براي تکميل يا انجام تراکنش خيلي دير رسيده است ")]
    public const short ResponseReceivedTooLate = 68;
    [Description(" تعداد دفعات تکراررمز از حد مجاز گذشته است  ")]
    public const short AllowabeNumberOfPinTriesExceeded = 69;
    [Description("تعداد دفعات ورود رمزغلط بیش از حدمجاز است  ")]
    public const short PinRetiesExceedsSlm = 75;
    [Description("مبلغ انتقال داده شده معتبر نيست   ")]
    public const short InvaidIntergangeAmountSlm = 76;
    [Description(" روز مالی تراکنش نا معتبر است یا مهلت زمان ارسال اصلاحیه به پایان رسیده است ")]
    public const short InvalidBusinessDateSlm = 77;
    [Description(" کارت فعال نیست ")]
    public const short DeactivatedCardSlm = 78;
    [Description(" حساب متصل به کارت نا معتبر است یا دارای اشکال است ")]
    public const short InvalidAmountSlm = 79;
    [Description(" درخواست تراکنش رد شده است ")]
    public const short TransactionDeniedSlm = 80;
    [Description(" کارت پذيرفته نشد ( اختصاصي SLM) ")]
    public const short CancelledCardSlm = 81;
    [Description(" پيام تاييد از دستگاه خود پرداز دريافت نشده است ( اختصاصي SLm) ")]
    public const short NoAckFromAtmSlm = 82;
    [Description(" سرويس گر سوئيچ کارت تراکنش را نپذيرفته است ")]
    public const short HostRefuseSlm = 83;
    [Description(" در تراکنشهايي که انجام آن مستلزم ارتباط با صادر کننده است در صورت فعال نبودن صادر کننده اين پيام در پاسخ ارسال خواهد شد  ")]
    public const short IssuerDownSlm = 84;
    [Description(" پردازش گر و يا مبدا انجام تراکنش معتبر نيست ")]
    public const short InvalidOriginatorOrProcessorSlm = 85;
    [Description(" تراکنش درخواستي براي بخش سخت افزاري درخواست شده از آن قابل قبول نيست  ")]
    public const short NotAllowedForDeviceSlm = 86;
    [Description(" سيستم در تبادل کليد رمز دچار مشکل شده است ( کد پاسخ اختصاصي SLM)  ")]
    public const short PinKeySyncErrorSlm = 87;
    [Description(" سيستم در تبادل کليد MAC  دچار مشکل شده است (کد پاسخ اختصاصي SLM ) ")]
    public const short MacKeySyncErrorSlm = 88;
    [Description("عدم تاييد تراکنش توسط سوئيچ خارجي ( کد پاسخ اختصاصي SLM)   ")]
    public const short ExternalSwitchDeclineSlm = 89;
    [Description(" سامانه مقصد تراکنش درحال انجام عملیات پایان روز می باشد ")]
    public const short CutOffIsInprogress = 90;
    [Description("سيستم صدور مجوز انجام تراکنش موقتا غير فعال است و يا زمان تعيين شده براي صدور مجوز به پايان رسيده است  ")]
    public const short IssuerOrSwitchIsInoperative = 91;
    [Description(" مقصد تراکنش پيدا نشد ")]
    public const short FinancialInstOrIntermediateNetFacilityNotFoundforRouting = 92;
    [Description(" امکان تکميل تراکنش وجود ندارد ")]
    public const short TranactionCannotBeCompleted = 93;
    [Description(" ارسال تکراري تراکنش بوجود آمده است  ")]
    public const short DuplicateTransmission = 94;
    [Description(" در عمليات مغايرت گيري ترمينال اشکال رخ داده است ")]
    public const short ReconcileError = 95;
    [Description(" اشکال در عملکرد سيستم ")]
    public const short SystemMulfunction = 96;
    [Description("تراکنش از سوی صادرکننده کارت مردود شده است")]
    public const short TransactionRejectedByIssuer = 97;
    [Description("سقف استفاده از رمز ایستا به پايان رسيده است ")]
    public const short TheNumberOfStaticPasswordUsageIsOver = 98;
    [Description(" خطای صادرکننده ")]
    public const short IssuerError = 99;
    [Description("سایر خطاهای سامانه های بانکی")]
    public const short OtherBankingSwitchUnmapableExceptions = 200;
    [Description("خطای ناشناخته")]
    public const short UnkownError = -32768;

    public static bool IsAboutToSentToSwitchStatus(short status) => status == (short) -1519;

    public static bool IsRetryAdviceOrReversalStatus(short status) => status == (short) -1002 || status == (short) -1000 || status == (short) 91;

    public static bool IsCapableToShowDescriptionToUser(short status) => status > (short) 0 || status >= (short) -200 && status <= (short) -100 || status == (short) -1626;

    public static bool IsCapableToShowCodeToUser(short status) => status > (short) 0;

    public static bool IsSwitchOrDbConnectionError(short status) => status == (short) -500 || status == (short) -501 || status == (short) -1000 || status == (short) -1001 || status == (short) -1002 || status > (short) 0 || status == (short) -1 || status == (short) -1521 || status == (short) -1522 || status == (short) -1523 || status == (short) -1516;

    public static string GetDescription(short value) => AttributeHelper.GetConstFieldAttributeValue<PaymentStatus, string, DescriptionAttribute>(((IEnumerable<FieldInfo>) typeof (PaymentStatus).GetFields(BindingFlags.Static | BindingFlags.Public)).FirstOrDefault<FieldInfo>((Func<FieldInfo, bool>) (prop => (int) (ushort) prop.GetValue((object) null) == (int) value)).Name, (Func<DescriptionAttribute, string>) (y => y.Description));

    public sealed class OfflineMultiplexed
    {
      [Description("خطا در کنترل شبا ها")]
      public const short PGWSupervisorServiceProxy_CheckIbans_Failure = -32000;
      [Description("هیچ شماره شبایی داده نشده است")]
      public const short IbansValidation_EmptyList = -32001;
      [Description("مبلغ در برخی آیتم ها معتبر نمی باشد")]
      public const short InvalidOneOrMoreAmounts = -32002;
      [Description("جمع کل مبالغ آیتم ها با مبلغ کل داده شده برابر نمی باشد")]
      public const short InvalidTotalAmount = -32003;
      [Description("در پردازش درخواست خرید با تسهیم آفلاین خطایی به وجود آمده است")]
      public const short CallOfflineMultiplexedSalePaymentRequestFailed = -32004;
      [Description("برخی از شماره شباهای داده شده معتبر نمی باشد")]
      public const short PGWSupervisorServiceProxy_CheckIbans_InvalidIbans = -20000;
    }

    public sealed class TSPConst
    {
      public const short PGWAddRequest_Failure = -2001;
      public const short GetTspByIdFailed = -2002;
      public const short GetBlackListByMobileNoFailed = -2003;
      public const short GetBlackListByMobileNoNullResult = -2004;
      public const short GetTspDataWithBlackListCardsFailed = -2005;
      public const short AddCardToBlackListFailed = -2006;
    }

    public sealed class PayvandConst
    {
      public const short PayvandGetCardInfo_Failure = -2002;
    }

    public sealed class PGWSupervisorApi
    {
      [Description("استثنای ناشناخته در استعلام وضعیت نهایی تراکنش قبض پرداختی")]
      public const short PGWSupervisorProxyGetBillStatusException = -2100;
    }
  }
}
