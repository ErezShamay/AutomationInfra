namespace Splitit.Automation.NG.Backend.Services.AdminApi.InstallmentPlan.InstallmentPlanResponses;

public class ResponseVerifyAuthorization
{
    public class Root
    {
        public bool IsAuthorized { get; set; }
        public double AuthorizationAmount { get; set; }
        public Authorization Authorization{ get; set; }
    }

    public class Authorization
    {
        public string Status { get; set; }
    }
}