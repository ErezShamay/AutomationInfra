namespace Splitit.Automation.NG.Backend.Services.ReportSystem.Config.ConfigResponses;

public class BaseReportsInsertResponse
{
    public class BaseReport
    {
        public string Id { get; set; }
        public List<SelectedColumn> SelectedColumns { get; set; }
        public string ReportName { get; set; }
        public string ReportDescription { get; set; }
        public string DataRetrievalConfigId { get; set; }
        public bool DateIntervalRequired { get; set; }
        public int ExposionLevel { get; set; }
        public string ReportDisplayName { get; set; }
        public bool Eventable { get; set; }
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
        public List<BaseReport> BaseReports { get; set; }
    }

    public class SelectedColumn
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public int ValueType { get; set; }
        public int State { get; set; }
        public bool Selected { get; set; }
    }
}