namespace Splitit.Automation.NG.Backend.Services.AdminApi.JobsMng.BaseObjects;

public class PostJobRunBaseObjects
{
    public class Parameters
    {
        public string InstallmentPlan { get; set; }
    }

    public class Root
    {
        public string Code { get; set; }
        public Parameters Parameters { get; set; }
    }
}