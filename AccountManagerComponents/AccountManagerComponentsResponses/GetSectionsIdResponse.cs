namespace Splitit.Automation.NG.Backend.Services.Ams.AccountManagerComponents.AccountManagerComponentsResponses;

public class GetSectionsIdResponse
{
    public class Error
    {
        public string Code { get; set; }
        public string Message { get; set; }
        public string AdditionalInfo { get; set; }
    }

    public class Field
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
        public Section Section { get; set; }
    }

    public class Section
    {
        public string Id { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public List<Field> Fields { get; set; }
        public string Title { get; set; }
        public string Subtitle { get; set; }
        public string Footer { get; set; }
        public string Arrangement { get; set; }
        public string AddItemText { get; set; }
        public string RemoveItemText { get; set; }
        public int TotalNumberOfForms { get; set; }
    }
}