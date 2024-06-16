using MongoDB.Bson.IO;

namespace Splitit.Automation.NG.Backend.Services.AdminApi.InstallmentPlan.InstallmentPlanResponses;

public class ResponseFullPlanInfoIpn
{
    public class ActiveCard
    {
        public string CardId { get; set; }
        public string CardNumber { get; set; }
        public string CardExpMonth { get; set; }
        public string CardExpYear { get; set; }
        public CardBrand CardBrand { get; set; }
        public CardType CardType { get; set; }
        public string Bin { get; set; }
        public string CardHolderFullName { get; set; }
        public string CardCvv { get; set; }
        public Address Address { get; set; }
        public string Token { get; set; }
    }

    public class Address
    {
        public string AddressLine { get; set; }
        public string AddressLine2 { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string FullAddressLine { get; set; }
    }

    public class Amount
    {
        public double Value { get; set; }
        public Currency Currency { get; set; }
    }

    public class AVSResult
    {
        public object Code { get; set; }
        public object Description { get; set; }
    }

    public class BusinessUnit
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
    }

    public class CardBrand
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
    }

    public class CardDetails
    {
        public string CardId { get; set; }
        public string CardNumber { get; set; }
        public string CardExpMonth { get; set; }
        public string CardExpYear { get; set; }
        public CardBrand CardBrand { get; set; }
        public CardType CardType { get; set; }
        public string Bin { get; set; }
        public string CardHolderFullName { get; set; }
        public string CardCvv { get; set; }
        public Address Address { get; set; }
        public object Token { get; set; }
    }

    public class CardType
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
    }

    public class Consumer
    {
        public string Id { get; set; }
        public string UniqueId { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string CultureName { get; set; }
        public object RoleName { get; set; }
        public bool IsLocked { get; set; }
        public bool IsDataRestricted { get; set; }
        public bool IsDataPrivateRestricted { get; set; }
    }

    public class Currency
    {
        public string Symbol { get; set; }
        public int Id { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
    }

    public class CVCResult
    {
        public object Code { get; set; }
        public object Description { get; set; }
    }

    public class Disputes
    {
        public bool InDispute { get; set; }
    }

    public class ExtendedParams
    {
        public string acceptHeader { get; set; }
        public string colorDepth { get; set; }
        public string javaEnabled { get; set; }
        public string javascriptEnabled { get; set; }
        public string language { get; set; }
        public string screenHeight { get; set; }
        public string screenWidth { get; set; }
        public string timeZoneOffset { get; set; }
        public string userAgent { get; set; }
        public string challengeWindowSize { get; set; }
        public string browser_size { get; set; }
        public string forterToken { get; set; }
    }

    public class FraudCheck
    {
        public FraudCheckResult FraudCheckResult { get; set; }
        public object ProviderResultCode { get; set; }
        public object ProviderResultDesc { get; set; }
        public object ProviderReferenceId { get; set; }
    }

    public class FraudCheckResult
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
    }

    public class Installment
    {
        public int InstallmentNumber { get; set; }
        public Amount Amount { get; set; }
        public OriginalAmount OriginalAmount { get; set; }
        public RefundAmount RefundAmount { get; set; }
        public DateTime ProcessDateTime { get; set; }
        public bool IsRefund { get; set; }
        public RequiredCredit RequiredCredit { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public Status Status { get; set; }
        public List<TransactionResult> TransactionResults { get; set; }
        public CardDetails CardDetails { get; set; }
        public bool? Result { get; set; }
        public string PaymentMethod { get; set; }
    }

    public class InstallmentPlan
    {
        public BusinessUnit BusinessUnit { get; set; }
        public object ShopperPaymentRequestTime { get; set; }
        public DateTime? NextAuthorization { get; set; }
        public string InstallmentPlanNumber { get; set; }
        public InstallmentPlanStatus InstallmentPlanStatus { get; set; }
        public Amount Amount { get; set; }
        public OutstandingAmount OutstandingAmount { get; set; }
        public Disputes Disputes { get; set; }
        public int NumberOfInstallments { get; set; }
        public int NumberOfProcessedInstallments { get; set; }
        public OriginalAmount OriginalAmount { get; set; }
        public RefundAmount RefundAmount { get; set; }
        public Consumer Consumer { get; set; }
        public ActiveCard ActiveCard { get; set; }
        public FraudCheck FraudCheck { get; set; }
        public Terminal Terminal { get; set; }
        public Merchant Merchant { get; set; }
        public string RefOrderNumber { get; set; }
        public PurchaseMethod PurchaseMethod { get; set; }
        public Strategy Strategy { get; set; }
        public object DelayResolution { get; set; }
        public ExtendedParams ExtendedParams { get; set; }
        public bool IsFullCaptured { get; set; }
        public bool IsChargedBack { get; set; }
        public bool ArePaymentsOnHold { get; set; }
        public double ScpFundingPercent { get; set; }
        public string FundingStatus { get; set; }
        public string TestMode { get; set; }
        public DateTime CreationDateTime { get; set; }
        public DateTime LifeTimeUrlExpirationTime { get; set; }
        public List<Installment> Installments { get; set; }
        public List<SecureAuthorization> SecureAuthorizations { get; set; }
        public object LogoUrl { get; set; }
        public bool IsInAutoRetry { get; set; }
        public string PaymentMethod { get; set; }
        public bool AllowCardUpdateOnSplititPortals { get; set; }
        public object OnHoldLastOpenDate { get; set; }
        public object OnHoldLastOpenUserId { get; set; }
        public int InstallmentsScheduleInterval { get; set; }
    }

    public class InstallmentPlanStatus
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
    }

    public class Merchant
    {
        public string Name { get; set; }
        public int Id { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
    }

    public class OperationType
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
    }

    public class OriginalAmount
    {
        public double Value { get; set; }
        public Currency Currency { get; set; }
    }

    public class OutstandingAmount
    {
        public double Value { get; set; }
        public Currency Currency { get; set; }
    }

    public class PurchaseMethod
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
    }

    public class RefundAmount
    {
        public double Value { get; set; }
        public Currency Currency { get; set; }
    }

    public class RequiredCredit
    {
        public double Value { get; set; }
        public Currency Currency { get; set; }
    }

    public class ResponseHeader
    {
        public bool Succeeded { get; set; }
        public object Errors { get; set; }
        public string TraceId { get; set; }
    }

    public class Root
    {
        public ResponseHeader ResponseHeader { get; set; }
        public InstallmentPlan InstallmentPlan { get; set; }
        public override string ToString()
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(this, Newtonsoft.Json.Formatting.Indented);
        }
    }

    public class SecureAuthorization
    {
        public DateTime ProcessingDate { get; set; }
        public Amount Amount { get; set; }
        public List<TransactionResult> TransactionResults { get; set; }
        public CardDetails CardDetails { get; set; }
        public object Result { get; set; }
    }

    public class Status
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
    }

    public class Strategy
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
    }

    public class Terminal
    {
        public int Id { get; set; }
        public object Code { get; set; }
        public string Description { get; set; }
    }

    public class TransactionResult
    {
        public string GatewayTransactionId { get; set; }
        public int SplititTransactionId { get; set; }
        public string SplititGatewayTransactionId { get; set; }
        public string GatewayResultCode { get; set; }
        public string GatewayResultMessage { get; set; }
        public OperationType OperationType { get; set; }
        public bool GatewayResult { get; set; }
        public DateTime GatewayTransactionDate { get; set; }
        public bool IsChargeback { get; set; }
        public AVSResult AVSResult { get; set; }
        public CVCResult CVCResult { get; set; }
        public bool IsInDispute { get; set; }
    }
}