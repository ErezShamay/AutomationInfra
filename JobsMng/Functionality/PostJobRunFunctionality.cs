using Newtonsoft.Json;
using Splitit.Automation.NG.Backend.BaseActions;
using Splitit.Automation.NG.Backend.Services.AdminApi.JobsMng.BaseObjects;
using Splitit.Automation.NG.Backend.Services.AdminApi.JobsMng.Responses;
using Splitit.Automation.NG.Backend.Services.V3.DefaultValues;
using Splitit.Automation.NG.Utilities.EnvironmentsConfig;

namespace Splitit.Automation.NG.Backend.Services.AdminApi.JobsMng.Functionality;

public class PostJobRunFunctionality
{
    private const string EndPoint = "/api/v1/job/run";
    private readonly HttpSender _httpSender = new();
    private readonly PostJobRunBaseObjects.Root _root = new();
    private readonly EnvConfig _envConfig = new();

    public async Task<PostJobRunResponse.Root> SendPostRequestForPostJobRunAsync(
        RequestHeader requestHeader, string jobCode, string ipns)
    {
        try
        {
            Console.WriteLine("\nStarting SendPostRequestForPostJobRunAsync");
            _root.Code = jobCode;
            _root.Parameters = new PostJobRunBaseObjects.Parameters
            {
                InstallmentPlan = ipns
            };
            var response = await _httpSender.SendPostHttpsRequestAsync(
                _envConfig.JobsMngUrl + EndPoint,
                _root, requestHeader);
            var jResponse = JsonConvert.DeserializeObject<PostJobRunResponse.Root>(response);
            Console.WriteLine("Done with SendPostRequestForPostJobRunAsync\n");
            return jResponse!;
        }
        catch (Exception exception)
        {
            Console.WriteLine("Error in SendPostRequestForPostJobRunAsync\n" + exception);
            throw;
        }
    }
}