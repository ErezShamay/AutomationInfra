using Newtonsoft.Json;
using Splitit.Automation.NG.Backend.BaseActions;
using Splitit.Automation.NG.Backend.Services.AdminApi.InstallmentPlan.InstallmentPlanBaseObjects;
using Splitit.Automation.NG.Backend.Services.AdminApi.InstallmentPlan.InstallmentPlanResponses;
using Splitit.Automation.NG.Backend.Services.V3.DefaultValues;
using Splitit.Automation.NG.Utilities.EnvironmentsConfig;

namespace Splitit.Automation.NG.Backend.Services.AdminApi.InstallmentPlan.InstallmentsPlanFunctionality;

public class DisputeResolveFunctionality
{
    private const string EndPoint = "/api/installment-plan/dispute/resolve";
    private readonly HttpSender _httpSender = new();
    private readonly DisputeResolveBaseObjects.Root _disputeResolveBaseObjects = new();
    private readonly EnvConfig _envConfig = new();
    
    public async Task<ResponseDisputeResolve.Root> SendPutRequestDisputeResolveAsync(
        RequestHeader requestHeader, string ipn, string captureId, string? wonFlag = default)
    {
        try
        {
            Console.WriteLine("\nStarting SendPutRequestDisputeResolve");
            _disputeResolveBaseObjects.InstallmentPlanNumber = ipn;
            if (wonFlag != null)
            {
                _disputeResolveBaseObjects.TransactionIdToMarkAsWon = new List<string> { captureId };
                _disputeResolveBaseObjects.TransactionIdToMarkAsLost = new List<string>();
            }
            else
            {
                _disputeResolveBaseObjects.TransactionIdToMarkAsWon = new List<string>();
                _disputeResolveBaseObjects.TransactionIdToMarkAsLost = new List<string> { captureId };
            }
            var response = await _httpSender.SendPutHttpsRequestAsync(_envConfig.AdminUrl + EndPoint,
                _disputeResolveBaseObjects, requestHeader);
            var jResponse = JsonConvert.DeserializeObject<ResponseDisputeResolve.Root>(response);
            Console.WriteLine("Done with SendPutRequestDisputeResolve\n");
            return jResponse!;
        }
        catch (Exception exception)
        {
            Console.WriteLine("Error in SendPutRequestDisputeResolve \n" + exception + "\n");
            throw;
        }
    }
}