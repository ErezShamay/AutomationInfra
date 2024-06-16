using Splitit.Automation.NG.Backend.Services.AdminApi.BaseObjects.Merchant.response;

namespace Splitit.Automation.NG.Backend.Services.V1.InstallmentPlan.InstallmentPlanResponses;

public class GetPgtlResponse
{
   public class Amount
{
    public decimal DecimalAmount { get; set; }
    public int DecimalPlaces { get; set; }
    public string CurrencyCode { get; set; }
    public string CurrencySymbol { get; set; }
    public string CurrencyName { get; set; }
}

public class InstallmentPlan
{
    public int Id { get; set; }
    public string PublicToken { get; set; }
    public string InstallmentPlanNumber { get; set; }
    public Dictionary<string, string> ExtendedParams { get; set; }
    public string StateIso2Code { get; set; }
    public string BillingAddressCity { get; set; }
    public string BillingAddressAddressLine { get; set; }
    public string BillingAddressZip { get; set; }
    public string RefOrderNumber { get; set; }
    public int NumberOfInstallments { get; set; }
    public int PurchaseMethod { get; set; }
    public decimal Amount { get; set; }
    public string CountryIsoA2 { get; set; }
    public bool UseTestGateway { get; set; }
    public int MerchantId { get; set; }
    public int TerminalId { get; set; }
    public string MerchantName { get; set; }
    public string MerchantCountryIsoA3 { get; set; }
    public int FundingTypesId { get; set; }
    public string ClientIpAddress { get; set; }
    public ActiveCard ActiveCard { get; set; }
    public Shopper Shopper { get; set; }
    public Amount Currency { get; set; }
    public List<Installment> Installments { get; set; }
}

public class ActiveCard
{
    // Define properties for ActiveCard
}

public class Shopper
{
    // Define properties for Shopper
}

public class Installment
{
    // Define properties for Installment
}

public class CreditCardDetails
{
    // Define properties for CreditCardDetails
}

public class Idempotency
{
    // Define properties for Idempotency
}

public class ActiveTerminal
{
    // Define properties for ActiveTerminal
}

public class Payfac
{
    // Define properties for Payfac
}

public class GatewayResult
{
    // Define properties for GatewayResult
}

public class PaymentGatewayTransactionResponse
{
    public string Id { get; set; }
    public bool Result { get; set; }
    public string TraceId { get; set; }
    public bool IsChargeback { get; set; }
    public DateTime CreatedDate { get; set; }
    public string TransactionId { get; set; }
    public int InstallmentPlanId { get; set; }
    public string CompleteResponseXml { get; set; }
    public int TerminalGatewayDataId { get; set; }
    public string AvsMessageMessageCode { get; set; }
    public string AvsMessageMessageText { get; set; }
    public string CvvMessageMessageCode { get; set; }
    public string CvvMessageMessageText { get; set; }
    public string RequestedCurrencyCode { get; set; }
    public decimal ProcessedAmountAmount { get; set; }
    public decimal RequestedAmountAmount { get; set; }
    public string ResultMessageMessageCode { get; set; }
    public string ResultMessageMessageText { get; set; }
    public string Type { get; set; }
    public string ReferencePaymentGatewayTransactionLogId { get; set; }
    public string IdempotencyKey { get; set; }
    public string ProcessorName { get; set; }
}

public class Root
{
    public CreateMerchantResponse.ResponseHeader ResponseHeader { get; set; }
    public Dictionary<string, List<PaymentGatewayTransactionResponse>> paymentGatewaytransactionResponses { get; set; }
}
}