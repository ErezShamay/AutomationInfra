namespace Splitit.Automation.NG.Backend.Services.MerchantPortal.Report.Responses;

public class GetAmountResponse
{
    public class Amount
    {
        public int Value { get; set; }
        public Currency Currency { get; set; }
    }

    public class ConvertedAmount
    {
        public int Value { get; set; }
        public Currency Currency { get; set; }
    }

    public class Currency
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public string Symbol { get; set; }
    }

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
        public List<Amount> Amounts { get; set; }
        public int NewPlansCount { get; set; }
        public ConvertedAmount ConvertedAmount { get; set; }
    }
}