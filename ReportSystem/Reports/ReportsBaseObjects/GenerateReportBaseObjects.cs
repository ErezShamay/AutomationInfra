namespace Splitit.Automation.NG.Backend.Services.ReportSystem.Reports.ReportsBaseObjects;

public class GenerateReportBaseObjects
{
    public class Root
    {
        public string ReportCode { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public string Format { get; set; }
        public List<string> SelectedColumns { get; set; }
    }
}