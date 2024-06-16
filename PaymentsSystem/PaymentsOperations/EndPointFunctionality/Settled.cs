using Newtonsoft.Json;
using Splitit.Automation.NG.Backend.BaseActions;
using Splitit.Automation.NG.Backend.Services.V3.DefaultValues;
using Splitit.Automation.NG.PaymentsSystem.PaymentsOperations.BaseObjects;
using Splitit.Automation.NG.Utilities.EnvironmentsConfig;

namespace Splitit.Automation.NG.PaymentsSystem.PaymentsOperations.EndPointFunctionality;

public class Settled
{
    private const string SettledEndPoint = "/api/payments/settled";
    private readonly HttpSender _httpSender = new();
    private readonly SettledBaseObjects.Root _settled = new();
    private readonly SettledBaseObjects.Result _result = new();
    private readonly EnvConfig _envConfig = new();

    public async Task<string> SendPutRequestSettledAsync(
        RequestHeader requestHeader, string batchedId, string internalId, bool settledSuccessFlag)
    {
        try
        {
            Console.WriteLine("\nStarting SendPutRequestSettled");
            _settled.batch_id = batchedId;
            _settled.results = new List<SettledBaseObjects.Result>();
            _result.internal_id = internalId;
            _result.success = settledSuccessFlag;
            _settled.results.Add(_result);
            var response = await _httpSender.SendPutHttpsRequestAsync(_envConfig.PaymentsUrl + SettledEndPoint,
                _settled, requestHeader);
            //var jResponse = JsonConvert.DeserializeObject<ResponseV3CheckEligibility.Root>(response);
            Console.WriteLine("Done with SendPutRequestSettled\n");
            return response;
        }
        catch (Exception exception)
        {
            Console.WriteLine("Error in SendPutRequestSettled\n" + exception + "\n");
            throw;
        }
    }
}