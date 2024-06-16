using NUnit.Framework;
using Splitit.Automation.NG.Backend.BaseActions;
using Splitit.Automation.NG.Backend.Services.AdminApi.InstallmentPlan.InstallmentsPlanFunctionality;
using Splitit.Automation.NG.Backend.Services.V3.DefaultValues;
using Splitit.Automation.NG.Backend.Tests.TestsSetups;
using Splitit.Automation.NG.Utilities.EnvironmentsConfig;

namespace Splitit.Automation.NG.Backend.Tests.ToolsForCSM;

public class Program
{
    private readonly InstallmentPlanNumberCancel _installmentPlanNumberCancel = new();
    private RequestHeader? _requestHeader = new();
    private readonly OpenAuthorizationsFunctionality _openAuthorizationsFunctionality = new();

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

    // [Category("CleanUp")]
    // [Test(Description = "CleanUp")]
    // public async Task TestValidateCleanUp()
    // {
    //     var inputFile = "/Users/erez.shamay/Desktop/ipns3.txt";
    //     //var outputFile = "/Users/erez.shamay/Desktop/ipnsSplited";
    //
    //     await CreateFilesWithLimitedLines(inputFile, _requestHeader!);
    //     //CreateFilesWithLimitedLines(inputFile, outputFile, 400, _requestHeader!);
    //
    //     // var inputFile = "/Users/erez.shamay/Desktop/ipnsSplited_10.txt";
    //     // //var outputFile = "/Users/erez.shamay/Desktop/ipnsSplited";
    //     //
    //     // await CreateFilesWithLimitedLines(inputFile, _requestHeader!);
    //     // //CreateFilesWithLimitedLines(inputFile, outputFile, 400, _requestHeader!);
    // }

    async Task CreateFilesWithLimitedLines(string inputFilePath, RequestHeader requestHeader)
    {
        try
        {
            var allLines = File.ReadAllLines(inputFilePath);

            foreach (var line in allLines)
            {
                //await SendCancelRequestWithIpn(requestHeader, line);
                await SendVoidRequestWithIpn(requestHeader, line);
            }

            Console.WriteLine("Done TestValidateCleanUp successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }

    static void CreateFilesWithLimitedLines(string inputFilePath, string outputFilePrefix,
        int lineLimit, RequestHeader requestHeader)
    {
        try
        {
            // Read all lines from the input file
            string[] allLines = File.ReadAllLines(inputFilePath);
            int totalLines = allLines.Length;
            int currentLineIndex = 0;
            int fileCounter = 1;

            while (currentLineIndex < totalLines)
            {
                // Take the next block of lines up to 'lineLimit' or until the end of the file
                string[] selectedLines = allLines
                    .Skip(currentLineIndex)
                    .Take(lineLimit)
                    .ToArray();

                // Write the selected lines to the output file
                string outputFilePath = $"{outputFilePrefix}_{fileCounter}.txt";
                File.WriteAllLines(outputFilePath, selectedLines);

                Console.WriteLine($"Created file '{outputFilePath}' with {selectedLines.Length} lines.");

                // Update indexes and counters for the next iteration
                currentLineIndex += lineLimit;
                fileCounter++;
            }

            Console.WriteLine("All files created successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }

    private async Task SendCancelRequestWithIpn(RequestHeader requestHeader, string ipn)
    {
        await _installmentPlanNumberCancel.SendCancelPlanRequestAsync(requestHeader, ipn,
            0, "NoRefunds");
    }
    
    private async Task SendVoidRequestWithIpn(RequestHeader requestHeader, string ipn)
    {
        await _openAuthorizationsFunctionality.SendPostRequestOpenAuthorizationsAsync( requestHeader, ipn, true);
    }
}