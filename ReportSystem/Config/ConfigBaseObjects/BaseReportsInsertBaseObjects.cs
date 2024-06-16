namespace Splitit.Automation.NG.Backend.Services.ReportSystem.Config.ConfigBaseObjects;

public class BaseReportsInsertBaseObjects
{
    public class Root
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

    public class SelectedColumn
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public int ValueType { get; set; }
        public int State { get; set; }
        public bool Selected { get; set; }
    }
}