namespace Splitit.Automation.NG.Backend.Services.Processors.Processors.Response;

public class GetProcessorsResponse
{
     public class AuthenticationParameter
    {
        public int Id { get; set; }
        public string DisplayName { get; set; }
        public string Key { get; set; }
        public int Order { get; set; }
        public int ProcessorId { get; set; }
        public bool IsMandatory { get; set; }
    }

    public class Configuration
    {
        public int Id { get; set; }
        public int ProcessorId { get; set; }
        public string ProcessorIntegration { get; set; }
        public string CertificationLevel { get; set; }
        public string Status { get; set; }
        public bool IsDeletedFromSOB { get; set; }
        public string Fee { get; set; }
        public string SpecificRegistrationLink { get; set; }
        public bool ShowNewAccounts { get; set; }
        public bool ShowExistingAccounts { get; set; }
        public bool None { get; set; }
        public bool IsNoneSecure { get; set; }
        public bool SpecialSettingsRequired { get; set; }
        public bool RequiresSplititSpecificAccount { get; set; }
        public bool RequiresMerchantsToChange { get; set; }
        public string Note { get; set; }
        public string Logo { get; set; }
        public string DisplayName { get; set; }
        public bool IsSupportAllCountries { get; set; }
        public string InstructionsUrl { get; set; }
        public List<string> Countries { get; set; }
        public List<string> PaymentMethods { get; set; }
        public List<DynamicFee> DynamicFees { get; set; }
        public bool IsMerchantOfRecord { get; set; }
    }

    public class DisputesReason
    {
        public int Id { get; set; }
        public string ReasonCode { get; set; }
        public string DisputeLiability { get; set; }
    }

    public class DynamicFee
    {
        public int TransactionFlatFee { get; set; }
        public int TransactionPercentFee { get; set; }
        public string CurrencyIsoCode { get; set; }
        public string ProductCode { get; set; }
    }

    public class Error
    {
        public string Code { get; set; }
        public string Message { get; set; }
        public string AdditionalInfo { get; set; }
    }

    public class ErrorClassificaton
    {
        public int Id { get; set; }
        public string GatewayErrorCode { get; set; }
        public int PisException { get; set; }
        public bool AutoHandlingMode { get; set; }
        public bool CcNeedsUpdate { get; set; }
        public string GatewayErrorMessage { get; set; }
        public int ProcessorId { get; set; }
    }

    public class PagingResponseHeader
    {
        public int TotalNumber { get; set; }
    }

    public class Processor
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ApiUrl { get; set; }
        public int MaxNumberOfRetries { get; set; }
        public int CancelAfterSeconds { get; set; }
        public int MaxAuth { get; set; }
        public int GracePeriod { get; set; }
        public bool IsCheckedMastercard { get; set; }
        public bool IsCheckedVisa { get; set; }
        public bool IsCheckedAmex { get; set; }
        public bool IsCheckedMaestro { get; set; }
        public bool IsCheckedJcb { get; set; }
        public bool IsCheckedDiscover { get; set; }
        public string GatewayApiassembly { get; set; }
        public string DisplayName { get; set; }
        public string GatewayApiimplementorNs { get; set; }
        public bool IsTokenSupported { get; set; }
        public string ConsumerUpdateCcGracePeriod { get; set; }
        public bool IsVoidSupported { get; set; }
        public int MinAmountAllowedForAuth { get; set; }
        public int CaptureDelayInSec { get; set; }
        public int VoidDelayInSec { get; set; }
        public bool AllowPartialVoid { get; set; }
        public bool Is3ds2Supported { get; set; }
        public int AsyncRefundGracePeriodDays { get; set; }
        public bool IsCheckedUpi { get; set; }
        public bool IsVoidAfterFailRefund { get; set; }
        public bool IsVerifyCardSuppported { get; set; }
        public bool IsCaptureApprovalSupported { get; set; }
        public bool IsEnabledToProcessDisputesInformation { get; set; }
        public bool IsSupportAuthorizeWithCapture { get; set; }
        public bool IsAsyncRefund { get; set; }
        public bool IsDeleted { get; set; }
        public Configuration Configuration { get; set; }
        public List<ErrorClassificaton> ErrorClassificaton { get; set; }
        public List<AuthenticationParameter> AuthenticationParameters { get; set; }
        public List<DisputesReason> DisputesReasons { get; set; }
    }

    public class Root
    {
        public List<Error> Errors { get; set; }
        public int StatusCode { get; set; }
        public string TraceId { get; set; }
        public bool IsSuccess { get; set; }
        public List<Processor> Processors { get; set; }
        public PagingResponseHeader PagingResponseHeader { get; set; }
    }
}