namespace Splitit.Automation.NG.Backend.Services.ReportSystem.Config.ConfigBaseObjects;

public class MerchantReportsInsertBaseObjects
{
    public class Root
    {
        public string BaseConfigId { get; set; }
        public string UserId { get; set; }
        public int Format { get; set; }
        public int DeliveryMethod { get; set; }
        public ScheduleSettings ScheduleSettings { get; set; }
        public List<SelectedColumn> SelectedColumns { get; set; }
        public int MerchantId { get; set; }
        public List<int> SelectedBusinessUnits { get; set; }
        public string RequestedReportName { get; set; }
        public bool EncryptReport { get; set; }
        public bool NotifyOnEmptyReport { get; set; }
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