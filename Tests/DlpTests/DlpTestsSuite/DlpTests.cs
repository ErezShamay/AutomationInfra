using Allure.NUnit;
using Allure.NUnit.Attributes;
using NUnit.Framework;
using Splitit.Automation.NG.Backend.BaseActions;
using Splitit.Automation.NG.Backend.Services.MerchantPortal.InstallmentPlan.Functionality;
using Splitit.Automation.NG.Backend.Services.V3.DefaultValues;
using Splitit.Automation.NG.Backend.Services.V3.InstallmentPlan.InstallmentPlanApiEndPoint;
using Splitit.Automation.NG.Backend.Tests.TestsSetups;

namespace Splitit.Automation.NG.Backend.Tests.DlpTests.DlpTestsSuite;

[TestFixture]
[AllureNUnit]
[AllureSuite("DlpTests")]
[AllureDisplayIgnored]
public class DlpTests
{
    private RequestHeader? _requestHeader;
    private readonly InstallmentPlans _installmentPlans;
    private readonly IpnDetailsFunctionality _ipnDetailsFunctionality;

    public DlpTests()
    {
        Console.WriteLine("Starting DlpTests Setup");
        _installmentPlans = new InstallmentPlans();
        _ipnDetailsFunctionality = new IpnDetailsFunctionality();
        Console.WriteLine("DlpTests Setup is done");
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
        Console.WriteLine("Setup is done");
    }
    
    [Category("DlpTests")]
    [Test(Description = "TestValidateDlpOnMerchants"), CancelAfter(80*1000)]
    public async Task TestValidateDlpOnMerchants()
    {
        try
        {
            Console.WriteLine("\nStarting TestValidateDlpOnMerchants");
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            var planCreateResponse = await _installmentPlans.CreatePlanAsync(
                Environment.GetEnvironmentVariable("ApiV3")!, _requestHeader!, "Active", 
                new Random().Next(2, 6), Environment.GetEnvironmentVariable("AliTerminal")!, 
                createPlanDefaultValues);
            var jResponseIpnDetails = await _ipnDetailsFunctionality.SendPostRequestIpnDetailsAsync(
                _requestHeader!, planCreateResponse.InstallmentPlanNumber, 
                Environment.GetEnvironmentVariable("GOOGLE_EMAILS_MERCHANT_ID")!);
            Assert.That(jResponseIpnDetails.Errors, Is.Not.Null);
            Assert.That(jResponseIpnDetails.StatusCode, Is.EqualTo(401));
            Console.WriteLine("TestValidateDlpOnMerchants is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidateDlpOnMerchants\n" + exception + "\n");
        }
    }
}