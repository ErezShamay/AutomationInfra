using Allure.NUnit;
using Allure.NUnit.Attributes;
using NUnit.Framework;
using Splitit.Automation.NG.Backend.BaseActions;
using Splitit.Automation.NG.Backend.Services.V3.DefaultValues;
using Splitit.Automation.NG.Backend.Services.V3.InstallmentPlan.InstallmentPlanApiEndPoint;
using Splitit.Automation.NG.Backend.Tests.TestsSetups;
using Splitit.Automation.NG.SharedResources.BaseActions;
using Splitit.Automation.NG.Utilities.EnvironmentsConfig;

namespace Splitit.Automation.NG.Backend.Tests.V3Tests.V3TestsSuite.VisTestsSuite;

[TestFixture]
[AllureNUnit]
[AllureSuite("VisTests")]
[AllureDisplayIgnored]
[Parallelizable(ParallelScope.All)]
public class VisTests
{
    private RequestHeader? _requestHeader;
    private readonly InstallmentPlans _installmentPlans;
    private readonly EnvConfig _envConfig;
    private readonly DoTheChallenge _doTheChallenge;
    
    public VisTests()
    {
        Console.WriteLine("Staring AliTests Setup");
        _installmentPlans = new InstallmentPlans();
        _doTheChallenge = new DoTheChallenge();
        _envConfig = new EnvConfig();
        Console.WriteLine("AliTests Setup Succeeded\n");
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
    
    [Category("VisTests")]
    [Test(Description = "TestValidateVisMerchantFeeOnlyCreatePlan"), CancelAfter(80*1000)]
    public async Task TestValidateVisMerchantFeeOnlyCreatePlan()
    {
        try
        {
            Console.WriteLine("\nStarting TestValidateVisMerchantFeeOnlyCreatePlan");
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            createPlanDefaultValues.paymentMethod.card = new CardDefaultValues
            {
                cardNumber = _envConfig.VisMerchantFeeOnlyCardNumber
            };
            var planInitiateResponse = await _installmentPlans.CreatePlanInitiateAsync(Environment.GetEnvironmentVariable("ApiV3")!,  _requestHeader!,
                "Active", new Random().Next(2, 6), 
                _envConfig.VisMockerTerminal, createPlanDefaultValues);
            Assert.That("Initialized", Is.EqualTo(planInitiateResponse.Status));
            Assert.That(await _doTheChallenge.DoTheChallengeVisAsync(planInitiateResponse));
            Console.WriteLine("TestValidateVisMerchantFeeOnlyCreatePlan is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidateVisMerchantFeeOnlyCreatePlan\n" + exception + "\n");
        }
    }
}