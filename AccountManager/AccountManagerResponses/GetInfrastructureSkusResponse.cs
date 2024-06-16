namespace Splitit.Automation.NG.Backend.Services.Ams.AccountManager.AccountManagerResponses;

public class GetInfrastructureSkusResponse
{
    public class Error
    {
        public string Code { get; set; }
        public string Message { get; set; }
        public string AdditionalInfo { get; set; }
    }

    public class Root
    {
        public List<Error> Errors { get; set; }
        public int StatusCode { get; set; }
        public string TraceId { get; set; }
        public bool IsSuccess { get; set; }
        public List<SKU> SKU { get; set; }
        public int TotalResult { get; set; }
    }

    public class SKU
    {
        public string Sku { get; set; }
        public int FixedFee { get; set; }
        public int VariableFee { get; set; }
        public int ChargebackFee { get; set; }
        public string Description { get; set; }
        public int BankRejectFee { get; set; }
    }
}