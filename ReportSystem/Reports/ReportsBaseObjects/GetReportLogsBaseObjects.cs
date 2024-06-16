namespace Splitit.Automation.NG.Backend.Services.ReportSystem.Reports.ReportsBaseObjects;

public class GetReportLogsBaseObjects
{
    public class Root
    {
        public int MerchantId { get; set; }
        public List<string> BaseReportIds { get; set; }
        public int Skip { get; set; }
        public int Take { get; set; }
    }
}