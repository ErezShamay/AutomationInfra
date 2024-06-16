using Allure.NUnit;
using Allure.NUnit.Attributes;
using NUnit.Framework;
using Splitit.Automation.NG.Backend.BaseActions;
using Splitit.Automation.NG.Backend.Services.V1.DefaultValues;
using Splitit.Automation.NG.Backend.Services.V3.DefaultValues;
using Splitit.Automation.NG.Backend.Tests.TestsSetups;
using Splitit.Automation.NG.Backend.Services.V1.InstallmentPlan.InstallmentPlanApiEndPoints;
using Splitit.Automation.NG.Utilities.EnvironmentsConfig;

namespace Splitit.Automation.NG.SharedResources.Tests.GatewaysFunctionality;

[TestFixture]
[AllureNUnit]
[AllureSuite("GatewaysCreatePlanWithTokenTests")]
[AllureDisplayIgnored]
[Parallelizable(scope: ParallelScope.All)]
public class GatewaysCreatePlanWithToken
{
    private readonly Create _create;
    private RequestHeader? _requestHeader;
    private readonly EnvConfig _envConfig;
    
    public GatewaysCreatePlanWithToken()
    {
        Console.WriteLine("Starting Setup");
        _create = new Create();
        _envConfig = new EnvConfig();
        Console.WriteLine("Done with Setup\n");
    }
    
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
    
    [TestCase(Category = "GatewaysCreatePlanWithTokenTests")]
    [Test(Description = "ValidateCreatePlanWithTokenWithTerminalAuthNetV2")]
    public async Task ValidateCreatePlanWithTokenWithTerminalAuthNetV2()
    {
        try
        {
            Console.WriteLine("\nStarting ValidateCreatePlanWithTokenWithTerminalAuthNetV2");
            var v1InitiateDefaultValues = new V1InitiateDefaultValues
            {
                CreditCardDetails = null!
            };
            var planCreateResponse = await _create.CreatePlanAsync( _requestHeader!,
                v1InitiateDefaultValues, _envConfig.AuthorizeNetV2,
                "AuthNetV2",
                _envConfig.AuthNetToken, 
                _envConfig.AuthNetV2Type, 
                _envConfig.Last4Digits);
            Assert.That(planCreateResponse.InstallmentPlan.InstallmentPlanStatus.Code.Equals("InProgress"));
            Assert.That(planCreateResponse.ResponseHeader.Succeeded);
            Console.WriteLine("ValidateCreatePlanWithTokenWithTerminalAuthNetV2 is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in ValidateCreatePlanWithTokenWithTerminalAuthNetV2 \n" + exception + "\n");
        }
    }
}