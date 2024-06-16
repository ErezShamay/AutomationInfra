using Newtonsoft.Json;
using Splitit.Automation.NG.Backend.BaseActions;
using Splitit.Automation.NG.Backend.Services.AdminApi.InstallmentPlan.InstallmentPlanBaseObjects;
using Splitit.Automation.NG.Backend.Services.AdminApi.InstallmentPlan.InstallmentPlanResponses;
using Splitit.Automation.NG.Backend.Services.V3.DefaultValues;
using Splitit.Automation.NG.Utilities.EnvironmentsConfig;

namespace Splitit.Automation.NG.Backend.Services.AdminApi.InstallmentPlan.InstallmentsPlanFunctionality;

public class PostDisputeMarkFunctionality
{
    private const string EndPoint = "/api/installment-plan/dispute/mark";
    private readonly HttpSender _httpSender = new();
    private readonly EnvConfig _envConfig = new();
    private readonly PostDisputeMarkBaseObjects.Root _root = new();

    public async Task<PostDisputeMarkResponse.Root?> SendPostRequestPostDisputeMarkAsync(
        RequestHeader requestHeader, string[] transactionIdsToMark, string[] transactionIdsToUnmark,
        string ipn, bool partialResponseMapping)
    {
        try
        {
            Console.WriteLine("\nStarting SendPostRequestPostDisputeMarkAsync");
            _root.TransactionIdsToMark = new List<string>();
            _root.TransactionIdsToUnmark = new List<string>();
            _root.InstallmentPlanNumber = ipn;
            _root.PartialResponseMapping = partialResponseMapping;
            var response = await _httpSender.SendPostHttpsRequestAsync(
                _envConfig.AdminUrl + EndPoint, _root, requestHeader);
            var jResponse = JsonConvert.DeserializeObject<PostDisputeMarkResponse.Root>(response);
            Console.WriteLine("Done with SendPostRequestPostDisputeMarkAsync\n");
            return jResponse;
        }
        catch (Exception exception)
        {
            Console.WriteLine("Error in SendPostRequestPostDisputeMarkAsync\n" + exception + "\n");
            throw;
        }
    }
}