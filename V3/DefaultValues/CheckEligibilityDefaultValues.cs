namespace Splitit.Automation.NG.Backend.Services.V3.DefaultValues;

public class CheckEligibilityDefaultValues
{
    public PlanDataDefaultValues planData;
    public BillingAddressDefaultValues billingAddress;
    public CardDefaultValues CardDetails;

    public CheckEligibilityDefaultValues()
    {
        planData = new PlanDataDefaultValues();
        billingAddress = new BillingAddressDefaultValues();
        CardDetails = new CardDefaultValues();
    }
}