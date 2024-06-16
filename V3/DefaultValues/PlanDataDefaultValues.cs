using Splitit.Automation.NG.Backend.BaseActions;

namespace Splitit.Automation.NG.Backend.Services.V3.DefaultValues;

public class PlanDataDefaultValues
{
    public double totalAmount;
    public string currency;
    public int numberOfInstallments;
    public string purchaseMethod;
    public string refOrderNumber;
    public string terminalId;
    public double FirstInstallmentAmount;
    public string FirstInstallmentDate;
    public Dictionary<string, string> extendedParams = new();

    public PlanDataDefaultValues()
    {
        totalAmount = AmountGenerator.GenerateAmount();
        numberOfInstallments = 3;
        currency = "USD";
        purchaseMethod = "ECommerce";
        refOrderNumber = GuidGenerator.GenerateNewGuid();
        FirstInstallmentAmount = 0;
        FirstInstallmentDate = DateTime.Now.ToString("MM/dd/yyyy");
    }
}