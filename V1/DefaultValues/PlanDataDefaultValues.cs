using Splitit.Automation.NG.Backend.BaseActions;

namespace Splitit.Automation.NG.Backend.Services.V1.DefaultValues;

public class PlanDataDefaultValues
{
    public AmountDefaultValues amount;
    public bool isFunded;
    public bool Attempt3DSecure;
    public bool AutoCapture;
    public string strategy;
    public string refOrderNumber;
    public string TestMode;
    public string PurchaseMethod;
    public int numberOfInstallments;
    public Dictionary<string, string> extendedParams = new ();

    public PlanDataDefaultValues(){
        var rand = new Random();
        amount = new AmountDefaultValues();
        isFunded = false;
        Attempt3DSecure = false;
        AutoCapture = true;
        strategy = null!;
        amount.currencyCode = "USD";
        amount.value = AmountGenerator.GenerateAmount();
        numberOfInstallments = rand.Next(2, 12);
        refOrderNumber = GuidGenerator.GenerateNewGuid();;
        TestMode = "Regular";
        PurchaseMethod = "ECommerce";
    }
}