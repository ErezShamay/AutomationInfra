using Splitit.Automation.NG.Backend.BaseActions;
using Splitit.Automation.NG.Backend.Services.V3.DefaultValues;
using Splitit.Automation.NG.PaymentsSystem.SplititOperations.BaseObjects;
using Splitit.Automation.NG.Utilities.EnvironmentsConfig;

namespace Splitit.Automation.NG.PaymentsSystem.SplititOperations.EndPointsFunctionality;

public class MovePlanToStd
{
    private const string MovePlanToStdEndPoint = "/api/splitit-operations/move-plan-to-std";
    private readonly MovePlanToStdBaseObjects.Root _movePlanToStdBaseObjects = new();
    private readonly HttpSender _httpSender = new  HttpSender();
    private readonly EnvConfig _envConfig = new();
    
    public async Task<string> SendPostRequestMovePlanToStdAsync(RequestHeader requestHeader, string ipn)
    {
        try
        {
            Console.WriteLine("\nStarting SendPostRequestMovePlanToStd");
            _movePlanToStdBaseObjects.installmentPlanNumbers = new List<string> { ipn };
            var response = await _httpSender.SendPostHttpsRequestAsync(_envConfig.PaymentsUrl + MovePlanToStdEndPoint,
                _movePlanToStdBaseObjects, requestHeader);
            Console.WriteLine("Done with SendPostRequestMovePlanToStd\n");
            return response;
        }
        catch (Exception exception)
        {
            Console.WriteLine("Error in SendPostRequestMovePlanToStd\n" + exception + "\n");
            throw;
        }
    }
}