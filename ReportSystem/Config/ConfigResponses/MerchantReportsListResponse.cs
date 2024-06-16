namespace Splitit.Automation.NG.Backend.Services.ReportSystem.Config.ConfigResponses;

public class MerchantReportsListResponse
{
     public class BaseConfig
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

    public class MerchantReport
    {
        public string Id { get; set; }
        public BaseConfig BaseConfig { get; set; }
        public string UserId { get; set; }
        public int Format { get; set; }
        public int DeliveryMethod { get; set; }
        public ScheduleSettings ScheduleSettings { get; set; }
        public int MerchantId { get; set; }
        public List<int> SelectedBusinessUnits { get; set; }
        public string RequestedReportName { get; set; }
        public bool EncryptReport { get; set; }
        public DateTime LastRunTime { get; set; }
        public bool NotifyOnEmptyReport { get; set; }
    }

    public class Root
    {
        public List<Error> Errors { get; set; }
        public int StatusCode { get; set; }
        public string TraceId { get; set; }
        public bool IsSuccess { get; set; }
        public List<MerchantReport> MerchantReports { get; set; }
    }

    public class ScheduleSettings
    {
        public string CronExpression { get; set; }
        public bool Enabled { get; set; }
        public string JobId { get; set; }
        public int DayOf { get; set; }
        public int HourOf { get; set; }
        public int MinuteOf { get; set; }
        public int Occurence { get; set; }
        public int TimeFrame { get; set; }
        public int TimeFrameInterval { get; set; }
        public int SubscriptionType { get; set; }
        public SubscriberInfo SubscriberInfo { get; set; }
    }

    public class SelectedColumn
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public int ValueType { get; set; }
        public int State { get; set; }
        public bool Selected { get; set; }
    }

    public class SftpSettings
    {
        public int SettingsType { get; set; }
        public string FolderName { get; set; }
    }

    public class SubscriberInfo
    {
        public List<string> Emails { get; set; }
        public SftpSettings SftpSettings { get; set; }
        public string TargetTimeZone { get; set; }
    }
}