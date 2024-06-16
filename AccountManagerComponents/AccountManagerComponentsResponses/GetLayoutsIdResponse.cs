namespace Splitit.Automation.NG.Backend.Services.Ams.AccountManagerComponents.AccountManagerComponentsResponses;

public class GetLayoutsIdResponse
{
    public class Error
    {
        public string Code { get; set; }
        public string Message { get; set; }
        public string AdditionalInfo { get; set; }
    }

    public class Layout
    {
        public string Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<PagesId> PagesIds { get; set; }
        public string Type { get; set; }
        public bool HasBackNavigation { get; set; }
        public bool AllowPageSkip { get; set; }
        public string NavigationView { get; set; }
        public bool HasStatusIndication { get; set; }
    }

    public class PagesId
    {
        public string Id { get; set; }
        public int Order { get; set; }
    }

    public class Root
    {
        public List<Error> Errors { get; set; }
        public int StatusCode { get; set; }
        public string TraceId { get; set; }
        public bool IsSuccess { get; set; }
        public Layout Layout { get; set; }
    }
}