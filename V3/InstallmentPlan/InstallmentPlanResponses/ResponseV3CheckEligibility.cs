namespace Splitit.Automation.NG.Backend.Services.V3.InstallmentPlan.InstallmentPlanResponses;

public class ResponseV3CheckEligibility
{
    public class Root
    {
        public List<PaymentPlanOption> PaymentPlanOptions { get; set; }
    }
    
    public class Links
    {
        public string PrivacyPolicyUrl { get; set; }
        public string TermsAndConditionsUrl { get; set; }
        public string LearnMoreUrl { get; set; }
    }

    public class PaymentPlanOption
    {
        public int NumberOfInstallments { get; set; }
        public double FirstInstallmentAmount { get; set; }
        public double InstallmentAmount { get; set; }
        public double LastInstallmentAmount { get; set; }
        public Links Links { get; set; }
    }
}