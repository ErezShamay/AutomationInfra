namespace Splitit.Automation.NG.Backend.Services.V3.InstallmentPlan.InstallmentPlanBaseObjects;

public class Identifier
{
    public Dictionary<string, string> ExtendedParams;

    public Identifier(Dictionary<string, string> extendedParams) {
        this.ExtendedParams = extendedParams;
    }
}