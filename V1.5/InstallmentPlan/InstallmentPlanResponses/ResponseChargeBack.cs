namespace Splitit.Automation.NG.Backend.Services.V1._5.InstallmentPlan.InstallmentPlanResponses;

public class ResponseChargeBack
{
    public class ActiveCard
    {
        public string cardId { get; set; }
        public string cardNumber { get; set; }
        public string cardExpMonth { get; set; }
        public string cardExpYear { get; set; }
        public CardBrand cardBrand { get; set; }
        public CardType cardType { get; set; }
        public string bin { get; set; }
        public string cardHolderFullName { get; set; }
        public string cardCvv { get; set; }
        public Address address { get; set; }
        public string token { get; set; }
    }

    public class Address
    {
        public string addressLine { get; set; }
        public string addressLine2 { get; set; }
        public string city { get; set; }
        public string country { get; set; }
        public string state { get; set; }
        public string zip { get; set; }
        public string fullAddressLine { get; set; }
    }

    public class Amount
    {
        public int value { get; set; }
        public Currency currency { get; set; }
    }

    public class AvsResult
    {
        public string code { get; set; }
        public string description { get; set; }
    }

    public class CardBrand
    {
        public int id { get; set; }
        public string code { get; set; }
        public string description { get; set; }
    }

    public class CardDetails
    {
        public string cardId { get; set; }
        public string cardNumber { get; set; }
        public string cardExpMonth { get; set; }
        public string cardExpYear { get; set; }
        public CardBrand cardBrand { get; set; }
        public CardType cardType { get; set; }
        public string bin { get; set; }
        public string cardHolderFullName { get; set; }
        public string cardCvv { get; set; }
        public Address address { get; set; }
        public string token { get; set; }
    }

    public class CardType
    {
        public int id { get; set; }
        public string code { get; set; }
        public string description { get; set; }
    }

    public class Consumer
    {
        public string id { get; set; }
        public string uniqueId { get; set; }
        public string userName { get; set; }
        public string fullName { get; set; }
        public string email { get; set; }
        public string phoneNumber { get; set; }
        public string cultureName { get; set; }
        public bool isLocked { get; set; }
        public bool isDataRestricted { get; set; }
        public bool isDataPrivateRestricted { get; set; }
    }

    public class Currency
    {
        public int id { get; set; }
        public string code { get; set; }
        public string description { get; set; }
        public string symbol { get; set; }
    }

    public class CvcResult
    {
        public string code { get; set; }
        public string description { get; set; }
    }

    public class DelayResolution
    {
        public int id { get; set; }
        public string code { get; set; }
        public string description { get; set; }
    }

    public class Disputes
    {
        public bool inDispute { get; set; }
        public DateTime evidenceProvidedOn { get; set; }
    }

    public class Error
    {
        public string errorCode { get; set; }
        public string message { get; set; }
        public string additionalInfo { get; set; }
    }

    public class ExtendedParams
    {
        public string additionalProp1 { get; set; }
        public string additionalProp2 { get; set; }
        public string additionalProp3 { get; set; }
    }

    public class FraudCheck
    {
        public FraudCheckResult fraudCheckResult { get; set; }
        public string providerResultCode { get; set; }
        public string providerResultDesc { get; set; }
        public string providerReferenceId { get; set; }
    }

    public class FraudCheckResult
    {
        public int id { get; set; }
        public string code { get; set; }
        public string description { get; set; }
    }

    public class GatewayTransactionResult
    {
        public string gatewayTransactionId { get; set; }
        public string splititGatewayTransactionId { get; set; }
        public string gatewayResultCode { get; set; }
        public string gatewayResultMessage { get; set; }
        public OperationType operationType { get; set; }
        public bool gatewayResult { get; set; }
        public DateTime gatewayTransactionDate { get; set; }
        public bool isChargeback { get; set; }
        public AvsResult avsResult { get; set; }
        public CvcResult cvcResult { get; set; }
        public bool isInDispute { get; set; }
        public int disputeStatus { get; set; }
    }

    public class Installment
    {
        public string installmentId { get; set; }
        public int installmentNumber { get; set; }
        public Amount amount { get; set; }
        public OriginalAmount originalAmount { get; set; }
        public RefundAmount refundAmount { get; set; }
        public DateTime processDateTime { get; set; }
        public bool isRefund { get; set; }
        public RequiredCredit requiredCredit { get; set; }
        public DateTime createdDateTime { get; set; }
        public Status status { get; set; }
        public List<TransactionResult> transactionResults { get; set; }
        public CardDetails cardDetails { get; set; }
        public bool result { get; set; }
        public string paymentMethod { get; set; }
    }

    public class InstallmentPlan
    {
        public string installmentPlanNumber { get; set; }
        public InstallmentPlanStatus installmentPlanStatus { get; set; }
        public Amount amount { get; set; }
        public OutstandingAmount outstandingAmount { get; set; }
        public Disputes disputes { get; set; }
        public int numberOfInstallments { get; set; }
        public int numberOfProcessedInstallments { get; set; }
        public OriginalAmount originalAmount { get; set; }
        public RefundAmount refundAmount { get; set; }
        public Consumer consumer { get; set; }
        public ActiveCard activeCard { get; set; }
        public FraudCheck fraudCheck { get; set; }
        public Terminal terminal { get; set; }
        public Merchant merchant { get; set; }
        public string refOrderNumber { get; set; }
        public PurchaseMethod purchaseMethod { get; set; }
        public Strategy strategy { get; set; }
        public DelayResolution delayResolution { get; set; }
        public ExtendedParams extendedParams { get; set; }
        public bool isFullCaptured { get; set; }
        public bool isChargedBack { get; set; }
        public bool arePaymentsOnHold { get; set; }
        public int scpFundingPercent { get; set; }
        public int fundingStatus { get; set; }
        public int testMode { get; set; }
        public DateTime creationDateTime { get; set; }
        public DateTime lifeTimeUrlExpirationTime { get; set; }
        public List<Installment> installments { get; set; }
        public List<SecureAuthorization> secureAuthorizations { get; set; }
        public string logoUrl { get; set; }
        public bool isInAutoRetry { get; set; }
        public string paymentMethod { get; set; }
        public bool allowCardUpdateOnSplititPortals { get; set; }
        public DateTime onHoldLastOpenDate { get; set; }
        public string onHoldLastOpenUserId { get; set; }
        public int installmentsScheduleInterval { get; set; }
        public int externalPaymentProvider { get; set; }
    }

    public class InstallmentPlanStatus
    {
        public int id { get; set; }
        public string code { get; set; }
        public string description { get; set; }
    }

    public class Merchant
    {
        public int id { get; set; }
        public string code { get; set; }
        public string description { get; set; }
        public string name { get; set; }
    }

    public class OperationType
    {
        public int id { get; set; }
        public string code { get; set; }
        public string description { get; set; }
    }

    public class OriginalAmount
    {
        public int value { get; set; }
        public Currency currency { get; set; }
    }

    public class OutstandingAmount
    {
        public int value { get; set; }
        public Currency currency { get; set; }
    }

    public class PurchaseMethod
    {
        public int id { get; set; }
        public string code { get; set; }
        public string description { get; set; }
    }

    public class RefundAmount
    {
        public int value { get; set; }
        public Currency currency { get; set; }
    }

    public class RequiredCredit
    {
        public int value { get; set; }
        public Currency currency { get; set; }
    }

    public class ResponseHeader
    {
        public bool succeeded { get; set; }
        public List<Error> errors { get; set; }
        public string traceId { get; set; }
    }

    public class Root
    {
        public ResponseHeader responseHeader { get; set; }
        public InstallmentPlan installmentPlan { get; set; }
        public List<GatewayTransactionResult> gatewayTransactionResults { get; set; }
    }

    public class SecureAuthorization
    {
        public DateTime processingDate { get; set; }
        public Amount amount { get; set; }
        public List<TransactionResult> transactionResults { get; set; }
        public CardDetails cardDetails { get; set; }
        public bool result { get; set; }
    }

    public class Status
    {
        public int id { get; set; }
        public string code { get; set; }
        public string description { get; set; }
    }

    public class Strategy
    {
        public int id { get; set; }
        public string code { get; set; }
        public string description { get; set; }
    }

    public class Terminal
    {
        public int id { get; set; }
        public string code { get; set; }
        public string description { get; set; }
    }

    public class TransactionResult
    {
        public string gatewayTransactionId { get; set; }
        public string splititGatewayTransactionId { get; set; }
        public string gatewayResultCode { get; set; }
        public string gatewayResultMessage { get; set; }
        public OperationType operationType { get; set; }
        public bool gatewayResult { get; set; }
        public DateTime gatewayTransactionDate { get; set; }
        public bool isChargeback { get; set; }
        public AvsResult avsResult { get; set; }
        public CvcResult cvcResult { get; set; }
        public bool isInDispute { get; set; }
        public int disputeStatus { get; set; }
    }
}