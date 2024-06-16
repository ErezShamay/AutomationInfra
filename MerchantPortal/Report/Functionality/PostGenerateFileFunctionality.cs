using Newtonsoft.Json;
using Splitit.Automation.NG.Backend.BaseActions;
using Splitit.Automation.NG.Backend.Services.MerchantPortal.Report.BaseObjects;
using Splitit.Automation.NG.Backend.Services.MerchantPortal.Report.Responses;
using Splitit.Automation.NG.Backend.Services.V3.DefaultValues;
using Splitit.Automation.NG.Utilities.EnvironmentsConfig;

namespace Splitit.Automation.NG.Backend.Services.MerchantPortal.Report.Functionality;

public class PostGenerateFileFunctionality
{
    private const string EndPoint = "/api/v1/report/generate-file";
    private readonly HttpSender _httpSender = new();
    private readonly EnvConfig _envConfig = new();
    private readonly PostGenerateFileBaseObjects _postSendReminderEmailDisputeIdBaseObjects = new();

    public async Task<string> SendPostRequestPostGenerateFileAsync(
        RequestHeader requestHeader)
    {
        try
        {
            Console.WriteLine("\nStarting SendPostRequestPostGenerateFile");
            var response = await _httpSender.SendPostHttpsRequestAsync(
                _envConfig.MerchantApiUrl + EndPoint,
                _postSendReminderEmailDisputeIdBaseObjects, requestHeader);
            var jResponse = JsonConvert.DeserializeObject<PostGenerateFileResponse.Root>(response);
            Console.WriteLine("Done with SendPostRequestPostGenerateFile\n");
            return response!;
        }
        catch (Exception exception)
        {
            Console.WriteLine("Error in SendPostRequestPostSendReminderEmailDisputeId \n" + exception + "\n");
            throw;
        }
    }
}