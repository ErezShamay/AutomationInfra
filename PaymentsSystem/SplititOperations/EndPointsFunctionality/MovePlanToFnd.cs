using Splitit.Automation.NG.Backend.BaseActions;
using Splitit.Automation.NG.Backend.Services.V3.DefaultValues;
using Splitit.Automation.NG.PaymentsSystem.SplititOperations.BaseObjects;
using Splitit.Automation.NG.Utilities.EnvironmentsConfig;

namespace Splitit.Automation.NG.PaymentsSystem.SplititOperations.EndPointsFunctionality;

public class MovePlanToFnd
{
    private const string MovePlanToFndEndPoint = "/api/splitit-operations/move-plan-to-fnd";
    private readonly MovePlanToFndBaseObjects.Root _movePlanToFndBaseObjects = new();
    private readonly HttpSender _httpSender = new();
    private readonly EnvConfig _envConfig = new();
    
    public async Task<string> SendPostRequestMovePlanToFndAsync(RequestHeader requestHeader, string ipn)
    {
        try
        {
            Console.WriteLine("\nStarting SendPostRequestMovePlanToFnd");
            _movePlanToFndBaseObjects.installmentPlanNumbers = new List<string> { ipn };
            var response = await _httpSender.SendPostHttpsRequestAsync(_envConfig.PaymentsUrl + MovePlanToFndEndPoint,
                _movePlanToFndBaseObjects, requestHeader);
            Console.WriteLine("Done with SendPostRequestMovePlanToFnd\n");
            return response;
        }
        catch (Exception exception)
        {
            Console.WriteLine("Error in SendPostRequestMovePlanToFnd\n" + exception + "\n");
            throw;
        }
    }
}