using Splitit.Automation.NG.Backend.BaseActions;
using Splitit.Automation.NG.Backend.Services.V3.InstallmentPlan.InstallmentPlanBaseObjects;

namespace Splitit.Automation.NG.Backend.Services.V3.DefaultValues;

public class UpdateOrderDefaultValues
{
    public Identifier identifier;
    public string refOrderNumber;
    public string trackingNumber;
    public string shippingStatus;
    public bool capture;
    
    public UpdateOrderDefaultValues(string status, bool capture)
    {
        refOrderNumber = GuidGenerator.GenerateNewGuid();
        trackingNumber = GuidGenerator.GenerateNewGuid();
        shippingStatus = status;
        this.capture = capture;
    }
    
    public UpdateOrderDefaultValues(string status, bool capture, Identifier identifier)
    {
        refOrderNumber = GuidGenerator.GenerateNewGuid();
        trackingNumber = GuidGenerator.GenerateNewGuid();
        shippingStatus = status;
        this.capture = capture;
        this.identifier = identifier;
    }
}