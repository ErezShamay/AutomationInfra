namespace Splitit.Automation.NG.Backend.Services.Ams.AccountManagerComponents.AccountManagerComponentsResponses;

public class GetPagesResponse
{
    public class Error
    {
        public string Code { get; set; }
        public string Message { get; set; }
        public string AdditionalInfo { get; set; }
    }

    public class Page
    {
        public string Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool HasStatus { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string PageName { get; set; }
        public List<SectionId> SectionIds { get; set; }
        public string Title { get; set; }
        public string Subtitle { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime ModifiedDate { get; set; }
    }

    public class Root
    {
        public List<Error> Errors { get; set; }
        public int StatusCode { get; set; }
        public string TraceId { get; set; }
        public bool IsSuccess { get; set; }
        public List<Page> Pages { get; set; }
        public int TotalResult { get; set; }
    }

    public class SectionId
    {
        public string Id { get; set; }
        public int Order { get; set; }
    }
}