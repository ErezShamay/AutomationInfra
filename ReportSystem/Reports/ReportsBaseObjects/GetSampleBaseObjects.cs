namespace Splitit.Automation.NG.Backend.Services.ReportSystem.Reports.ReportsBaseObjects;

public class GetSampleBaseObjects
{
    public class Root
    {
        public string ReportId { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public List<int> BusinessUnitlist { get; set; }
        public string TimeZoneCode { get; set; }
        public int MerchantId { get; set; }
        public List<string> SelectedColumns { get; set; }
    }
}