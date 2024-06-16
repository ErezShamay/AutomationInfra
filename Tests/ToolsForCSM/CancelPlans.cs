using Splitit.Automation.NG.Backend.BaseActions;
using Splitit.Automation.NG.Backend.Services.AdminApi.InstallmentPlan.InstallmentsPlanFunctionality;
using Splitit.Automation.NG.Backend.Services.V3.DefaultValues;

namespace Splitit.Automation.NG.Backend.Tests.ToolsForCSM;

public class CancelPlans
{
    private RequestHeader? _requestHeader = new();
    private readonly HttpSender _httpSender = new();
    private readonly InstallmentPlanNumberCancel _installmentPlanNumberCancel = new();
    
    // [Category("CancelPlans")]
    // [Test(Description = "CancelPlans")]
    // public async Task TestValidateCancelPlans()
    // {
    //     var inputFile = "/Users/erez.shamay/Desktop/retry.txt";
    //     _requestHeader!.sessionId = "e352186e091a442b906ab861dd7f3354-1";
    //     await ConvertFileToListAndSendCancel(inputFile, _requestHeader!);
    // }

    private async Task ConvertFileToListAndSendCancel(string inputFile, RequestHeader requestHeader)
    {
        try
        {
            var allLines = File.ReadAllLines(inputFile);
    
            foreach (var line in allLines)
            {
                await SendPostRequestWithIpn(requestHeader, line);
            }
    
            Console.WriteLine("Done ConvertFileToListAndSendCancel successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }

    private async Task SendPostRequestWithIpn(RequestHeader requestHeader, string line)
    {
        await _installmentPlanNumberCancel.SendCancelPlanRequestAsync(requestHeader, line,
            0, "NoRefunds");
    }
}