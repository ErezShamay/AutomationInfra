namespace Splitit.Automation.NG.Backend.Services.AdminApi.Merchant.MerchantResponses;

public class ResponseSettingsSave
{
    public class Root
    {
        public ResponseHeader ResponseHeader { get; set; }
        public MerchantSettings MerchantSettings { get; set; }
        public List<object> TransactionLimits { get; set; }
    }
    
    public class MerchantSettings
    {
        public int MerchantSettingsId { get; set; }
        public int BusinessUnitId { get; set; }
        public string DefaultPlanStrategy { get; set; }
        public double SecurePlanMaxAllowedDebitAmount { get; set; }
        public double NonSecurePlanMaxAllowedDebitAmount { get; set; }
        public bool EnableDefaultAutoCapture { get; set; }
        public bool DefineDefaultNumberOfInstallments { get; set; }
        public int SelectedDefaultNumberOfInstallments { get; set; }
        public bool ShowCloseDialogBeforeAbandon { get; set; }
        public int MinInstallments { get; set; }
        public int MaxInstallments { get; set; }
        public double MinAmount { get; set; }
        public double MaxAmount { get; set; }
        public int SecurePlanMaxAllowedDebitInstallments { get; set; }
        public int NonSecurePlanMaxAllowedDebitInstallments { get; set; }
        public double SecurePlanMaxAllowedPrePaidAmount { get; set; }
        public int SecurePlanMaxAllowedPrePaidInstallments { get; set; }
        public double NonSecureMinAuthToAllowNonSecure { get; set; }
        public bool NonSecureFutureCaptureEnableAuthFotInitInstallment { get; set; }
        public object PaymentFormViewName { get; set; }
        public object PaymentFormTouchPointCode { get; set; }
        public object VposTouchPointCode { get; set; }
        public bool PaymentFotmForceDisplayImportantNotes { get; set; }
        public string KeepNonApprovedPlanLive { get; set; }
        public string ActivationLinkLifetime { get; set; }
        public string ExtendPlanLiveTimeOnPaymentRequest { get; set; }
        public string ExtendPlanLiveTimeOnApprovalRequest { get; set; }
        public bool EnableFraudTool { get; set; }
        public bool EnableFraudToolTransactionBlocking { get; set; }
        public string DefineDefaultCulture { get; set; }
        public bool AllowMultipleCapture { get; set; }
        public int DeferPlanStartByDays { get; set; }
        public bool DeclineBinsNotSupportingInstallment { get; set; }
        public bool SplititHoldsFinantialLiabilityForNonSecure { get; set; }
        public bool DelayConsumerPlanConfirmationEmailToInstallmentStart { get; set; }
        public bool MakeShopperEmailMandatory { get; set; }
        public bool MakeShopperPhoneNumberMandatory { get; set; }
        public string FraudDetectionProvider { get; set; }
        public bool ShowAddressForEcomm { get; set; }
        public bool ShowAddressForInStore { get; set; }
        public bool ShowAddressForPhoneOrder { get; set; }
        public bool IsPFAddressReadOnly { get; set; }
        public bool IsPFPhoneNumberIsReadOnly { get; set; }
        public bool IsPFEmailAddressIsReadOnly { get; set; }
        public bool ShowLearnMore { get; set; }
        public bool ShowMobilePhone { get; set; }
        public bool AllowSinglePaymentForLessThanMinAmount { get; set; }
        public object MainPFHexColorCode { get; set; }
        public bool Attempt3DsbyDefault { get; set; }
        public bool EnableDelayedChargeVPOS { get; set; }
        public object PaymentFormTPAbTestId { get; set; }
        public string NumberOfInstallmentsUiSelectionsOption { get; set; }
        public MoveToNonSecure MoveToNonSecure { get; set; }
        public string DefaultRefundStrategy { get; set; }
        public string FirstInstallmentAmountValueType { get; set; }
        public object FirstInstallmentAmountDefaultValue { get; set; }
        public string CustomNumberOfInstallments { get; set; }
        public bool RunCaptureAsync { get; set; }
        public bool IsPhoneOrderEnabledInVpos3 { get; set; }
        public bool IsCopyLinkEnabledInVpos3 { get; set; }
        public bool DoFullCaptureOnPendingShipmentInCCRejectionPhase { get; set; }
        public object LearnMoreTouchPointCode { get; set; }
        public string ScheduleInterval { get; set; }
        public object InstallmentsScheduleDaysInterval { get; set; }
        public bool IsWhiteLabel { get; set; }
        public bool IsRefOrderNumberMandatory { get; set; }
        public PaymentSettings PaymentSettings { get; set; }
    }

    public class MoveToNonSecure
    {
        public bool AllowInvalidCCCardBrandNotSupported { get; set; }
        public bool AllowInvalidCCCardTypeNotSupported { get; set; }
        public bool AllowInvalidCcPrepaidCardNotSupported { get; set; }
        public bool AllowGtwyResultGeneralError { get; set; }
        public bool AllowGtwyResultCCDataInsufficientFunds { get; set; }
        public bool AllowGtwyResultCCDataGeneralProblem { get; set; }
        public bool AllowGtwyResultCCWasDeclined { get; set; }
        public double AllowMoveToNonSecureForErrorsMaxAllowedAmount { get; set; }
        public int AllowMoveToNonSecureForErrorsMaxAllowedInstallments { get; set; }
    }

    public class PaymentSettings
    {
        public object CreditLine { get; set; }
        public object RiskRating { get; set; }
        public object ReservePool { get; set; }
        public object FundingTrigger { get; set; }
        public object DebitOnHold { get; set; }
        public object FundingOnHold { get; set; }
        public object FundingEndDate { get; set; }
        public object FundingStartDate { get; set; }
        public object MonetaryFlow { get; set; }
    }

    public class ResponseHeader
    {
        public bool Succeeded { get; set; }
        public object Errors { get; set; }
        public string TraceId { get; set; }
    }
}