namespace Splitit.Automation.NG.Backend.Services.Ams.AccountManager.AccountManagerResponses;

public class GetPartnerProfilesIdResponse
{
    public class Error
    {
        public string Code { get; set; }
        public string Message { get; set; }
        public string AdditionalInfo { get; set; }
    }

    public class Partner
    {
        public string Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string Name { get; set; }
        public string LayoutName { get; set; }
        public string LayoutId { get; set; }
        public string PresetName { get; set; }
        public string PresetId { get; set; }
        public string AccountName { get; set; }
        public string AccountId { get; set; }
    }

    public class Root
    {
        public List<Error> Errors { get; set; }
        public int StatusCode { get; set; }
        public string TraceId { get; set; }
        public bool IsSuccess { get; set; }
        public Partner Partner { get; set; }
    }
}