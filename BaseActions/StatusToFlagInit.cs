using Splitit.Automation.NG.Backend.Services.V3.DefaultValues;

namespace Splitit.Automation.NG.Backend.BaseActions;

public class StatusToFlagInit
{
    public CreatePlanDefaultValues StatusToFlagsInit(string status, CreatePlanDefaultValues createPlanDefaultValues)
    {
        Console.WriteLine("\nStarting StatusToFlagsInit");
        switch (status) {
            case "Active":
                createPlanDefaultValues.attemptAuthorize = true;
                createPlanDefaultValues.autoCapture = true;
                break;
            case "Initialized":
                createPlanDefaultValues.attemptAuthorize = false;
                createPlanDefaultValues.autoCapture = false;
                break;
            case "PendingCapture" :
                createPlanDefaultValues.attemptAuthorize = true;
                createPlanDefaultValues.autoCapture = false;
                break;
        }

        Console.WriteLine("Done with StatusToFlagsInit\n");
        return createPlanDefaultValues;
    }
}