namespace Splitit.Automation.NG.Backend.Services.Processors.Processors.BaseObjects;

public class PostProcessorsIdBaseObjects
{
    public class Root
    {
        public int ProcessorId { get; set; }
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
    }
}