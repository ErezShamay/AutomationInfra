namespace Splitit.Automation.NG.Backend.Services.V2.InstallmentPlan.InstallmentPlanResponses;

public class LearnMoreDetailsExtendedResponse
{
    public class Amount
    {
        public int Value { get; set; }
        public string CurrencyCode { get; set; }
    }

    public class Error
    {
        public string ErrorCode { get; set; }
        public string Message { get; set; }
        public string AdditionalInfo { get; set; }
    }

    public class Installment
    {
        public DateTime Date { get; set; }
        public Amount Amount { get; set; }
        public int HeldAmount { get; set; }
    }

    public class ResponseHeader
    {
        public bool Succeeded { get; set; }
        public List<Error> Errors { get; set; }
        public string TraceId { get; set; }
    }

    public class Root
    {
        public ResponseHeader ResponseHeader { get; set; }
        public bool SupportsCreditCard { get; set; }
        public bool SupportsDebitCard { get; set; }
        public bool ShowSecureStrategyInfo { get; set; }
        public bool ShowNonSecureStrategyInfo { get; set; }
        public bool Isfortnightly { get; set; }
        public bool IsWhiteLabel { get; set; }
        public Total Total { get; set; }
        public List<Installment> Installments { get; set; }
        public int NumberOfInstallments { get; set; }
        public string PlanStrategy { get; set; }
    }

    public class Total
    {
        public int Value { get; set; }
        public string CurrencyCode { get; set; }
    }
}