namespace Splitit.Automation.NG.Backend.Services.MerchantPortal.Report.Responses;

public class GetCountryResponse
{
    public class Countries
    {
        public int additionalProp1 { get; set; }
        public int additionalProp2 { get; set; }
        public int additionalProp3 { get; set; }
    }

    public class CountriesISO
    {
        public int additionalProp1 { get; set; }
        public int additionalProp2 { get; set; }
        public int additionalProp3 { get; set; }
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
        public Countries Countries { get; set; }
        public CountriesISO CountriesISO { get; set; }
    }
}