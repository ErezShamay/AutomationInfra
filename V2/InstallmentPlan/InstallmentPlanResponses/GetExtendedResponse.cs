namespace Splitit.Automation.NG.Backend.Services.V2.InstallmentPlan.InstallmentPlanResponses;

public class GetExtendedResponse
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
        public int Value { get; set; }
        public Currency Currency { get; set; }
    }

    public class AVSResult
    {
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
        public string Token { get; set; }
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
        public string RoleName { get; set; }
        public bool IsLocked { get; set; }
        public bool IsDataRestricted { get; set; }
        public bool IsDataPrivateRestricted { get; set; }
    }

    public class Currency
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public string Symbol { get; set; }
    }

    public class CVCResult
    {
        public string Code { get; set; }
        public string Description { get; set; }
    }

    public class DelayResolution
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
    }

    public class Disputes
    {
        public bool InDispute { get; set; }
        public DateTime EvidenceProvidedOn { get; set; }
    }

    public class Error
    {
        public string ErrorCode { get; set; }
        public string Message { get; set; }
        public string AdditionalInfo { get; set; }
    }

    public class ExtendedParams
    {
        public string additionalProp1 { get; set; }
        public string additionalProp2 { get; set; }
        public string additionalProp3 { get; set; }
    }

    public class FraudCheck
    {
        public FraudCheckResult FraudCheckResult { get; set; }
        public string ProviderResultCode { get; set; }
        public string ProviderResultDesc { get; set; }
        public string ProviderReferenceId { get; set; }
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
        public bool Result { get; set; }
        public string PaymentMethod { get; set; }
    }

    public class InstallmentPlanStatus
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
    }

    public class Merchant
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
    }

    public class OperationType
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
    }

    public class OriginalAmount
    {
        public int Value { get; set; }
        public Currency Currency { get; set; }
    }

    public class OutstandingAmount
    {
        public int Value { get; set; }
        public Currency Currency { get; set; }
    }

    public class PagingResponseHeader
    {
        public int TotalNumber { get; set; }
    }

    public class PlansList
    {
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
        public Merchant Merchant { get; set; }
        public string RefOrderNumber { get; set; }
        public PurchaseMethod PurchaseMethod { get; set; }
        public Strategy Strategy { get; set; }
        public DelayResolution DelayResolution { get; set; }
        public ExtendedParams ExtendedParams { get; set; }
        public bool IsFullCaptured { get; set; }
        public bool IsChargedBack { get; set; }
        public bool ArePaymentsOnHold { get; set; }
        public int ScpFundingPercent { get; set; }
        public string FundingStatus { get; set; }
        public string TestMode { get; set; }
        public DateTime CreationDateTime { get; set; }
        public DateTime LifeTimeUrlExpirationTime { get; set; }
        public List<Installment> Installments { get; set; }
        public List<SecureAuthorization> SecureAuthorizations { get; set; }
        public string LogoUrl { get; set; }
        public bool IsInAutoRetry { get; set; }
        public string PaymentMethod { get; set; }
        public bool AllowCardUpdateOnSplititPortals { get; set; }
        public DateTime OnHoldLastOpenDate { get; set; }
        public string OnHoldLastOpenUserId { get; set; }
        public int InstallmentsScheduleInterval { get; set; }
    }

    public class PurchaseMethod
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
    }

    public class RefundAmount
    {
        public int Value { get; set; }
        public Currency Currency { get; set; }
    }

    public class RequiredCredit
    {
        public int Value { get; set; }
        public Currency Currency { get; set; }
    }

    public class ResponseHeader
    {
        public bool Succeeded { get; set; }
        public List<Error> Errors { get; set; }
        public string TraceId { get; set; }
    }

    public class Root
    {
        public List<PlansList> PlansList { get; set; }
        public ResponseHeader ResponseHeader { get; set; }
        public PagingResponseHeader PagingResponseHeader { get; set; }
    }

    public class SecureAuthorization
    {
        public DateTime ProcessingDate { get; set; }
        public Amount Amount { get; set; }
        public List<TransactionResult> TransactionResults { get; set; }
        public CardDetails CardDetails { get; set; }
        public bool Result { get; set; }
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
        public string DisputeStatus { get; set; }
    }
}