namespace Splitit.Automation.NG.Backend.Services.Ams.AccountManager.AccountManagerResponses;

public class GetInfrastructureCountriesResponse
{
    public class Country
    {
        public string IsoA3 { get; set; }
        public string IsoA2 { get; set; }
        public string Name { get; set; }
        public List<Subdivision> Subdivisions { get; set; }
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
        public List<Country> Countries { get; set; }
    }

    public class Subdivision
    {
        public string Name { get; set; }
        public string IsoA2 { get; set; }
    }
}