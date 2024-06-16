namespace Splitit.Automation.NG.Backend.Services.V3.DefaultValues;

public class CreatePlanDefaultValues
{
    public bool attemptAuthorize;
    public bool autoCapture;
    public bool attempt3DSecure;
    public bool termsAndConditionsAccepted;
    public PlanDataDefaultValues planData;
    public ShopperDefaultValues shopper;
    public BillingAddressDefaultValues billingAddress;
    public PaymentMethodDefaultValues paymentMethod;
    public RedirectUrlsDefaultValues redirectUrls;
    
	    
    public CreatePlanDefaultValues() 
    {
        attemptAuthorize = false;
        termsAndConditionsAccepted = true;
        autoCapture = true;
        attempt3DSecure = false;
        planData = new PlanDataDefaultValues();
        shopper = new ShopperDefaultValues();
        billingAddress = new BillingAddressDefaultValues();
        paymentMethod = new PaymentMethodDefaultValues();
        redirectUrls = new RedirectUrlsDefaultValues();
    }
}