using Allure.NUnit;
using Allure.NUnit.Attributes;
using NUnit.Framework;
using Splitit.Automation.NG.Backend.BaseActions;
using Splitit.Automation.NG.Backend.Services.AdminApi.InstallmentPlan.InstallmentsPlanFunctionality;
using Splitit.Automation.NG.Backend.Services.V1.InstallmentPlan.InstallmentPlanApiEndPoints;
using Splitit.Automation.NG.Backend.Services.V3.DefaultValues;
using Splitit.Automation.NG.Backend.Services.V3.InstallmentPlan.InstallmentPlanApiEndPoint;
using Splitit.Automation.NG.Backend.Services.V3.InstallmentPlan.InstallmentPlanResponses;
using Splitit.Automation.NG.Backend.Tests.TestsSetups;
using PostDisputesFunctionality = Splitit.Automation.NG.Backend.Services.DisputesV2.Dispute.Functionality.PostDisputesFunctionality;

namespace Splitit.Automation.NG.Backend.Tests.DisputesV2Tests.DisputesV2TestsSuites;

[TestFixture]
[AllureNUnit]
[AllureSuite("CheckoutDisputesV2Tests")]
[AllureDisplayIgnored]
[Parallelizable(ParallelScope.All)]
public class DisputesV2CheckoutTests
{
    private RequestHeader? _requestHeader;
    private readonly InstallmentPlans _installmentPlans;
    private readonly ChargeFunctionality _chargeFunctionality;
    private readonly GetPgtl _getPgtl;
    private readonly PostDisputesFunctionality _postDisputesFunctionality;
    private readonly FullPlanInfoIpn _fullPlanInfoIpn;


    public DisputesV2CheckoutTests()
    {
        Console.WriteLine("\nStaring DisputesV2Tests Setup");
        _installmentPlans = new InstallmentPlans();
        _getPgtl = new GetPgtl();
        _chargeFunctionality = new ChargeFunctionality();
        _postDisputesFunctionality = new PostDisputesFunctionality();
        _fullPlanInfoIpn = new FullPlanInfoIpn();
        Console.WriteLine("DisputesV2Tests Setup Succeeded\n");
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

    [Category("CheckoutDisputesV2Tests")]
    [Test(Description = "TestValidateCheckoutDisputesV2Won with generated merchant"), CancelAfter(720 * 1000)]
    public async Task TestValidateCheckoutDisputesV2Won()
    {
        try
        {
            Console.WriteLine("\nStarting TestValidateCheckoutDisputesV2Won");
            var (installmentPlan, pgtlCaptureTransactionId) = await InitPlan(41.80);
            var fullPlanInfoAfter = await _fullPlanInfoIpn.SendGetFullPlanInfoIpnAsync(_requestHeader!,
                installmentPlan.InstallmentPlanNumber, null!, pgtlCaptureTransactionId!,
                new List<string> { "payment_captured", "dispute_received", "dispute_won" });
            Assert.That(fullPlanInfoAfter.ResponseHeader.Succeeded);
            Assert.That(fullPlanInfoAfter.InstallmentPlan.IsFullCaptured);
            Assert.That(fullPlanInfoAfter.InstallmentPlan.InstallmentPlanStatus.Code, Is.EqualTo("Cleared"));
            Console.WriteLine("TestValidateCheckoutDisputesV2Won is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidateCheckoutDisputesV2Won\n" + exception + "\n");
        }
    }

    [Category("CheckoutDisputesV2Tests")]
    [Test(Description = "TestValidateCheckoutDisputesV2Lost with generated merchant"), CancelAfter(720 * 1000)]
    public async Task TestValidateCheckoutDisputesV2Lost()
    {
        try
        {
            Console.WriteLine("\nStarting TestValidateCheckoutDisputesV2Lost");
            var (installmentPlan, pgtlCaptureTransactionId) = await InitPlan(41.84);
            var jResponseFullPlanInfo = await _fullPlanInfoIpn.SendGetFullPlanInfoIpnAsync(_requestHeader!,
                installmentPlan.InstallmentPlanNumber, null!, pgtlCaptureTransactionId!,
                new List<string> { "payment_captured", "dispute_received", "dispute_lost" });
            Assert.That(jResponseFullPlanInfo.ResponseHeader.Succeeded);
            var jResponseDisputesResponse = await _postDisputesFunctionality.SendPostRequestPostDisputesAsync(_requestHeader!,
                installmentPlan.InstallmentPlanNumber, jResponseFullPlanInfo.InstallmentPlan.Merchant.Id, "Lost");
            Assert.That(_postDisputesFunctionality.ValidateDisputeStatus(jResponseDisputesResponse, "Lost"));
            var fullPlanInfoAfter = await _fullPlanInfoIpn.SendGetFullPlanInfoIpnAsync(_requestHeader!,
                installmentPlan.InstallmentPlanNumber);
            Assert.That(fullPlanInfoAfter.ResponseHeader.Succeeded);
            Assert.That(fullPlanInfoAfter.InstallmentPlan.OutstandingAmount.Value, Is.EqualTo(0));
            Assert.That(fullPlanInfoAfter.InstallmentPlan.InstallmentPlanStatus.Code, Is.EqualTo("Canceled"));
            Console.WriteLine("TestValidateCheckoutDisputesV2Lost is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidateCheckoutDisputesV2Lost\n" + exception + "\n");
        }
    }

    private async Task<(ResponseV3.ResponseRoot installmentPlan, string? pgtlCaptureTransactionId)> InitPlan(double amount)
    {
        try
        {
            await Task.Delay(10 * 1000);
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            createPlanDefaultValues.planData.currency = "GBP";
            createPlanDefaultValues.planData.totalAmount = amount;
            createPlanDefaultValues.paymentMethod.card.cardHolderFullName = "John Smith";
            createPlanDefaultValues.paymentMethod.card.cardNumber = "4242424242424242";
            createPlanDefaultValues.paymentMethod.card.cardExpYear = 2099;
            createPlanDefaultValues.paymentMethod.card.cardExpMonth = 1;
            createPlanDefaultValues.paymentMethod.card.cardCvv = "100";
            var installmentPlan = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!, _requestHeader!,
                "Active", 4, Environment.GetEnvironmentVariable("CheckoutTerminal")!,
                createPlanDefaultValues, shouldCancel: false);
            var jResponseCharge =
                await _chargeFunctionality.SendPostRequestChargeFunctionalityAsync(_requestHeader!,
                    installmentPlan.InstallmentPlanNumber);
            Assert.That(jResponseCharge.ResponseHeader.Succeeded);
            var pgtlCaptureTransactionId = await _getPgtl.ValidatePgtlKeyValueInnerAsync(Environment.GetEnvironmentVariable("StoreProcedureUrl")!,
                _requestHeader!, installmentPlan.InstallmentPlanNumber, "Type", "Capture", "TransactionId");
            await Task.Delay(360 * 1000);
            return (installmentPlan, pgtlCaptureTransactionId);
        }
        catch (Exception e)
        {
            Console.WriteLine("Error in InitPlan" + e);
            throw;
        }
    }
}