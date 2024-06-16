using Allure.NUnit;
using Allure.NUnit.Attributes;
using NUnit.Framework;
using Splitit.Automation.NG.Backend.BaseActions;
using Splitit.Automation.NG.Backend.Services.V1.DefaultValues;
using Splitit.Automation.NG.Backend.Services.V1.InstallmentPlan.InstallmentPlanApiEndPoints;
using Splitit.Automation.NG.Backend.Services.V3.DefaultValues;
using Splitit.Automation.NG.Backend.Tests.TestsSetups;
using Splitit.Automation.NG.SharedResources.BaseActions;

namespace Splitit.Automation.NG.Backend.Tests.V1Tests.V1TestsSuite;

[TestFixture]
[AllureNUnit]
[AllureSuite("V1Tests")]
[AllureDisplayIgnored]
public class V1Tests
{
    private RequestHeader? _requestHeader;
    private readonly DoTheChallenge _doTheChallenge;
    private readonly Create _create;
    
    public V1Tests()
    {
        Console.WriteLine("\nStaring V1 Setup");
        _doTheChallenge = new DoTheChallenge();
        _create = new Create();
        Console.WriteLine("V1 Setup Succeeded\n");
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
    
    [Category("V1Tests")]
    [Test(Description = "TestValidateCreatePlanV1"), CancelAfter(80*1000)]
    public async Task TestValidateCreatePlanV1()
    {
        try
        {
            Console.WriteLine("\nStarting TestValidateCreatePlanV1");
            var v1InitiateDefaultValues = new V1InitiateDefaultValues();
            var planCreateResponse = await _create.CreatePlanAsync( _requestHeader!, v1InitiateDefaultValues);
            Assert.That(planCreateResponse.InstallmentPlan, Is.Not.Null);
            Console.WriteLine("Done with TestValidateCreatePlanV1\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidateCreatePlanV1\n" +exception+ "\n");
        }
    }
    
    [Category("V1Tests")]
    [Test(Description = "TestValidateCreatePlanV1With3Ds")]
    public async Task TestValidateCreatePlanV1With3Ds()
    {
        try
        {
            Console.WriteLine("\nStarting TestValidateCreatePlanV1With3Ds");
            var v1InitiateDefaultValues = new V1InitiateDefaultValues
            {
                planData =
                {
                    Attempt3DSecure = true
                }
            };
            var planCreateResponse = await _create.CreatePlanInitiateAsync( _requestHeader!, v1InitiateDefaultValues);
            Assert.That(planCreateResponse.InstallmentPlan.InstallmentPlanStatus.Code.Equals("Initializing"));
            var cardExp = v1InitiateDefaultValues.CreditCardDetails.cardExpMonth + "/" + v1InitiateDefaultValues.CreditCardDetails.cardExpYear;
            Assert.That(await _doTheChallenge.DoTheChallengeV35Async(planCreateResponse.CheckoutUrl, v1InitiateDefaultValues.CreditCardDetails.cardNumber, 
                cardExp, v1InitiateDefaultValues.CreditCardDetails.cardCvv,
                "yes",  _requestHeader!, planCreateResponse.InstallmentPlan.InstallmentPlanNumber), Is.True);
            Console.WriteLine("Done with TestValidateCreatePlanV1With3Ds\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidateCreatePlanV1With3Ds\n" +exception+ "\n");
        }
    }
}