using Allure.NUnit;
using Allure.NUnit.Attributes;
using NUnit.Framework;
using Splitit.Automation.NG.Backend.BaseActions;
using Splitit.Automation.NG.SharedResources.BaseActions;
using Splitit.Automation.NG.Backend.Services.V3.DefaultValues;
using Splitit.Automation.NG.Backend.Tests.TestsSetups;
using Splitit.Automation.NG.Backend.Services.V2.InstallmentPlan.InstallmentPlanApiEndPoints;
using Splitit.Automation.NG.Backend.Services.V2.InstallmentPlan.InstallmentPlanRequests;
using Splitit.Automation.NG.Backend.Services.V3.InstallmentPlan.InstallmentPlanApiEndPoint;

namespace Splitit.Automation.NG.Backend.Tests.V2Tests.V2TestsSuite;

[TestFixture]
[AllureNUnit]
[AllureSuite("V2Tests")]
[AllureDisplayIgnored]
public class V2Tests
{
    private RequestHeader? _requestHeader;
    private readonly DoUpdateCard _doUpdateCard;
    private readonly InstallmentPlans _installmentPlans;
    private readonly InitiateUpdatePaymentData _initiateUpdatePaymentData;
    private readonly InitiateUpdatePaymentDataRequest.Root _initiateUpdatePaymentDataRequest;

    public V2Tests()
    {
        Console.WriteLine("Starting V2Tests");
        _doUpdateCard = new DoUpdateCard();
        _installmentPlans = new InstallmentPlans();
        _initiateUpdatePaymentData = new InitiateUpdatePaymentData();
        _initiateUpdatePaymentDataRequest = new InitiateUpdatePaymentDataRequest.Root();
        Console.WriteLine("Done with V2Tests");
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
    
    [Category("V2Tests")]
    [Test(Description = "UpdateCardAfterPlanCreation"), CancelAfter(80*1000)]
    public async Task UpdateCardAfterPlanCreation()
    {
        try
        {
            Console.WriteLine("\nStarting UpdateCardAfterPlanCreation");
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            var planCreateResponse = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!, _requestHeader!, "Active", 
                new Random().Next(2, 6), Environment.GetEnvironmentVariable("NgMockV2Terminal")!, createPlanDefaultValues);
            _initiateUpdatePaymentDataRequest.InstallmentPlanNumber = planCreateResponse.InstallmentPlanNumber;
            var jResponse = await _initiateUpdatePaymentData.SendInitiateUpdatePaymentDataRequestAsync(_requestHeader!, _initiateUpdatePaymentDataRequest);
            await _doUpdateCard.DoUpdateCreditCardAsync(jResponse.UpdateURL, Environment.GetEnvironmentVariable("Card5454")!, 
                createPlanDefaultValues.paymentMethod.card.cardHolderFullName, createPlanDefaultValues.paymentMethod.card.cardExpMonth, 
                createPlanDefaultValues.paymentMethod.card.cardExpYear, createPlanDefaultValues.paymentMethod.card.cardCvv, null!, 
                createPlanDefaultValues, _requestHeader!, planCreateResponse.InstallmentPlanNumber);
            Console.WriteLine("UpdateCardAfterPlanCreation is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in UpdateCardAfterPlanCreation \n" + exception + "\n");
        }
    }
}