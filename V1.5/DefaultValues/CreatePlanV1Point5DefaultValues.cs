using Splitit.Automation.NG.Backend.Services.V1.DefaultValues;
using Splitit.Automation.NG.Backend.Services.V3.DefaultValues;
using BillingAddressDefaultValues = Splitit.Automation.NG.Backend.Services.V1.DefaultValues.BillingAddressDefaultValues;
using PlanApprovalEvidenceDefaultValues = Splitit.Automation.NG.Backend.Services.V3.DefaultValues.PlanApprovalEvidenceDefaultValues;
using PlanDataDefaultValues = Splitit.Automation.NG.Backend.Services.V1.DefaultValues.PlanDataDefaultValues;
using RedirectUrlsDefaultValues = Splitit.Automation.NG.Backend.Services.V1.DefaultValues.RedirectUrlsDefaultValues;

namespace Splitit.Automation.NG.Backend.Services.V1._5.DefaultValues;

public class CreatePlanV1Point5DefaultValues
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
	    
    public CreatePlanV1Point5DefaultValues() 
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
    }
}