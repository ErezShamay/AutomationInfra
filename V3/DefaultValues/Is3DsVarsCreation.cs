using Faker;

namespace Splitit.Automation.NG.Backend.Services.V3.DefaultValues;

public class Is3DsVarsCreation
{
    public CreatePlanDefaultValues Create3DsVars(CreatePlanDefaultValues createPlanDefaultValues, string is3Ds)
    {
        Console.WriteLine();
        Console.WriteLine("\nStarting Create3DsVars");
        switch (is3Ds) {
            case "yes":
                createPlanDefaultValues.planData.extendedParams.Add("ThreeDSExemption", "no_preference");
                createPlanDefaultValues.attempt3DSecure = true;
                break;
            case "no":
                createPlanDefaultValues.planData.extendedParams.Add("ThreeDSExemption", "no_preference");
                createPlanDefaultValues.planData.extendedParams.Add("force-3ds", "true");
                createPlanDefaultValues.attempt3DSecure = false;
                break;
            case "regular":
                createPlanDefaultValues.planData.extendedParams.Add("IPayId", RandomNumber.Next(999999).ToString());
                createPlanDefaultValues.shopper.email = "Splitit.Automation@gmail.com";
                createPlanDefaultValues.attempt3DSecure = true;
                break;
            case "default":
                createPlanDefaultValues.planData.extendedParams.Add("IPayId", RandomNumber.Next(999999).ToString());
                createPlanDefaultValues.attempt3DSecure = true;
                break;
            case "with_exemption":
                createPlanDefaultValues.planData.extendedParams.Add("ThreeDSExemption", "no_preference");
                createPlanDefaultValues.attempt3DSecure = false;
                break;
            case "no_exemption":
                createPlanDefaultValues.attempt3DSecure = true;
                break;
            case "no_exemption_false":
                createPlanDefaultValues.attempt3DSecure = false;
                break;
            case "no_exemption_with_params":
                createPlanDefaultValues.planData.extendedParams.Add("Attempt3DSecure", "true");
                createPlanDefaultValues.attempt3DSecure = true;
                break;
        }

        Console.WriteLine("Done with Create3DsVars\n");
        Console.WriteLine();
        return createPlanDefaultValues;
    }
}