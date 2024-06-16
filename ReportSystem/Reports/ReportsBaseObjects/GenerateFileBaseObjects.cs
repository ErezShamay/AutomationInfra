namespace Splitit.Automation.NG.Backend.Services.ReportSystem.Reports.ReportsBaseObjects;

public class GenerateFileBaseObjects
{
    public class Root
    {
        public string ReportId { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public int Format { get; set; }
        public string TimeZoneId { get; set; }
        public List<int> BusinessUnits { get; set; }
        public int MerchantId { get; set; }
        public List<string> SelectedColumns { get; set; }
        public Identifier Identifier { get; set; }
    }
    
    public class Identifier
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }
}