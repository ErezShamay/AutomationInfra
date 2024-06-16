namespace Splitit.Automation.NG.Backend.Services.ReportSystem.Config.ConfigResponses;

public class BaseReportsGetColumnsResponse
{
    public class Column
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public int ValueType { get; set; }
        public int State { get; set; }
        public bool Selected { get; set; }
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
        public List<Column> Columns { get; set; }
    }
}