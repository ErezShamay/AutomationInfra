using Splitit.Automation.NG.Backend.Services.V3.DefaultValues;

namespace Splitit.Automation.NG.Backend.Services.V1.DefaultValues;

public class V1InitiateDefaultValues
{
    public RequestHeader RequestHeader;
    public PlanDataDefaultValues planData;
    public string InstallmentPlanNumber;
    public CreditCardDetailsDefaultValues CreditCardDetails;
    public BillingAddressDefaultValues BillingAddress;
    public ConsumerDataDefaultValues ConsumerData;
    public PlanApprovalEvidenceDefaultValues PlanApprovalEvidence;
    public PaymentWizardDataDefaultValues PaymentWizardData;
    public RedirectUrlsDefaultValues RedirectUrls;
    public PaymentToken paymentToken;
	
    public V1InitiateDefaultValues()
    {
        RequestHeader = new RequestHeader();
        InstallmentPlanNumber = null!;
        planData = new PlanDataDefaultValues();
        CreditCardDetails = new CreditCardDetailsDefaultValues();
        BillingAddress = new BillingAddressDefaultValues();
        ConsumerData = new ConsumerDataDefaultValues();
        PlanApprovalEvidence = new PlanApprovalEvidenceDefaultValues();
        PaymentWizardData = new PaymentWizardDataDefaultValues();
        RedirectUrls = new RedirectUrlsDefaultValues();
        paymentToken = new PaymentToken();
    }
}