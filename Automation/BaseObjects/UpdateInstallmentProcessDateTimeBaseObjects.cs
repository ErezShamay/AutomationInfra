namespace Splitit.Automation.NG.Backend.Services.AdminApi.Automation.BaseObjects;

public class UpdateInstallmentProcessDateTimeBaseObjects
{
    public class Root
    {
        public int InstallmentId { get; set; }
        public DateTime newProcessedDateTime { get; set; }
    }
}