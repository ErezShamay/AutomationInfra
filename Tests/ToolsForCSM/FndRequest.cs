using Newtonsoft.Json;
using NUnit.Framework;
using Splitit.Automation.NG.Backend.BaseActions;
using Splitit.Automation.NG.Backend.Services.AdminApi.InstallmentPlan.InstallmentsPlanFunctionality;
using Splitit.Automation.NG.Backend.Services.V3.DefaultValues;
using Splitit.Automation.NG.Backend.Tests.TestsSetups;
using Splitit.Automation.NG.Utilities.EnvironmentsConfig;

namespace Splitit.Automation.NG.Backend.Tests.ToolsForCSM;

public class FndRequest
{
    public readonly string auth = "sk_eiau3cdont7g5qpoafih53kgaip";
    
    private readonly InstallmentPlanNumberCancel _installmentPlanNumberCancel = new();
    private RequestHeader? _requestHeader = new();
    private readonly HttpSender _httpSender = new();
    
    [OneTimeSetUp]
    public async Task InitSetUp()
    {
        var testsSetup = new TestsSetup();
        testsSetup.Setup();
        var sendAdminLoginRequest = new SendAdminLoginRequest();
        _requestHeader = await sendAdminLoginRequest.DoAdminLogin(
            Environment.GetEnvironmentVariable("AccessTokenURI")!,
            Environment.GetEnvironmentVariable("ClientSecret")!,
            Environment.GetEnvironmentVariable("SplititMockTerminal")!,
            Environment.GetEnvironmentVariable("clientId")!);
    }
    
    // [Category("AutomationTools")]
    // [Test(Description = "AutomationTools")]
    // public async Task TestValidateAutomationTools()
    // {
    //     var inputFile = "/Users/erez.shamay/Desktop/missing collections.txt";
    //
    //     await CreateFileWithBalancesObj(inputFile, _requestHeader!);
    // }

    private async Task CreateFileWithBalancesObj(string inputFile, RequestHeader requestHeader)
    {
        try
        {
            var allLines = File.ReadAllLines(inputFile);
    
            foreach (var line in allLines)
            {
                await SendGetRequestWithPay(requestHeader, line);
            }
    
            Console.WriteLine("Done TestValidateAutomationTools successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }

    private async Task SendGetRequestWithPay(RequestHeader requestHeader, string line)
    {
        requestHeader.sessionId = auth;
        var response = await _httpSender.SendGetHttpsRequestAsync(line, requestHeader);
        var responseFndRequest = JsonConvert.DeserializeObject<ResponseFndRequest.Root>(response);
        var total_authorized = responseFndRequest.balances.total_authorized;
        var total_voided = responseFndRequest.balances.total_voided;
        var available_to_void = responseFndRequest.balances.available_to_void;
        var total_captured = responseFndRequest.balances.total_captured;
        var available_to_capture = responseFndRequest.balances.available_to_capture;
        var total_refunded = responseFndRequest.balances.total_refunded;
        var available_to_refund = responseFndRequest.balances.available_to_refund;

        var pathToFile = "/Users/erez.shamay/Desktop/FndRequest";
        if (!File.Exists(pathToFile))
        {
            // Create a file to write to.
            using (StreamWriter sw = File.CreateText(pathToFile))
            {
                sw.WriteLine("Hello Roiy");
            }	
        }
        using (var sw = File.AppendText(pathToFile))
        {
            sw.WriteLine(line);
            sw.WriteLine("{");
            sw.WriteLine("  total_authorized: " + total_authorized);
            sw.WriteLine("  total_voided: " + total_voided);
            sw.WriteLine("  available_to_void: " + available_to_void);
            sw.WriteLine("  total_captured: " + total_captured);
            sw.WriteLine("  available_to_capture: " + available_to_capture);
            sw.WriteLine("  total_refunded: " + total_refunded);
            sw.WriteLine("  available_to_refund: " + available_to_refund);
            sw.WriteLine("}");
            sw.WriteLine("\n\n");
        }	
    }
}