using Splitit.Automation.NG.Backend.BaseActions;
using Splitit.Automation.NG.Backend.Services.V3.DefaultValues;

namespace Splitit.Automation.NG.Backend.Tests.ToolsForCSM;

public class XxxLutz
{
    private RequestHeader? _requestHeader = new();
    private readonly HttpSender _httpSender = new();
    private readonly string _endPoint = "https://api-admin.production.splitit.com/api/procedures/UpdateInstallmentPlan";
    
    // [Category("Retry")]
    // [Test(Description = "Retry")]
    // public async Task TestValidateXxxLutz()
    // {
    //     var inputFile = "/Users/erez.shamay/Desktop/retry.txt";
    //     _requestHeader!.sessionId = "e352186e091a442b906ab861dd7f3354-1";
    //     await SendPostForXxxLutz(inputFile, _requestHeader!);
    // }

    private async Task SendPostForXxxLutz(string inputFile, RequestHeader requestHeader)
    {
        try
        {
            var allLines = File.ReadAllLines(inputFile);
    
            foreach (var line in allLines)
            {
                var xxxLutzObj = new XxxLutzRequest.Root
                {
                    InstallmentPlanNumber = line,
                    Strategy = "SecuredPlan4"
                };
                await SendPostRequestWithIpn(requestHeader, xxxLutzObj);
            }
    
            Console.WriteLine("Done TestValidateCleanUp successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }

    private async Task SendPostRequestWithIpn(RequestHeader requestHeader, XxxLutzRequest.Root obj)
    {
        var response = await _httpSender.SendPostHttpsRequestAsync(_endPoint, obj, requestHeader);
    }
}