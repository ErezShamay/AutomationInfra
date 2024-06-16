using Allure.NUnit;
using Allure.NUnit.Attributes;
using NUnit.Framework;
using Splitit.Automation.NG.Backend.BaseActions;
using Splitit.Automation.NG.Backend.Services.AdminApi.InstallmentPlan.InstallmentsPlanFunctionality;
using Splitit.Automation.NG.Backend.Services.DisputesV2.AuditLogs.Functionality;
using Splitit.Automation.NG.Backend.Services.DisputesV2.Dispute.Functionality;
using Splitit.Automation.NG.Backend.Services.V1.InstallmentPlan.InstallmentPlanApiEndPoints;
using Splitit.Automation.NG.Backend.Services.V3.DefaultValues;
using Splitit.Automation.NG.Backend.Services.V3.InstallmentPlan.InstallmentPlanApiEndPoint;
using Splitit.Automation.NG.Backend.Services.V3.InstallmentPlan.InstallmentPlanResponses;
using Splitit.Automation.NG.Backend.Tests.TestsSetups;
using Splitit.Automation.NG.Utilities.EnvironmentsConfig;

namespace Splitit.Automation.NG.Backend.Tests.DisputesV2Tests.DisputesV2TestsSuites;

[TestFixture]
[AllureNUnit]
[AllureSuite("StripeDirectDisputesV2Tests")]
[AllureDisplayIgnored]
//[Parallelizable(ParallelScope.All)]
public class DisputesV2StripeDirectTests
{
    private RequestHeader? _requestHeader;
    private readonly InstallmentPlans _installmentPlans;
    private readonly GetPgtl _getPgtl;
    private readonly DisputeResolveFunctionality _disputeResolveFunctionality;
    private readonly FullPlanInfoIpn _fullPlanInfoIpn;
    private readonly PostDisputesFunctionality _postDisputesFunctionality;
    private readonly GetAuditLogsFunctionality _getAuditLogsFunctionality;


    public DisputesV2StripeDirectTests()
    {
        Console.WriteLine("\nStaring DisputesV2Tests Setup");
        _installmentPlans = new InstallmentPlans();
        _getPgtl = new GetPgtl();
        _postDisputesFunctionality = new PostDisputesFunctionality();
        _fullPlanInfoIpn = new FullPlanInfoIpn();
        _disputeResolveFunctionality = new DisputeResolveFunctionality();
        _getAuditLogsFunctionality = new GetAuditLogsFunctionality();
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
    
    [Category("StripeDirectDisputesV2Tests")]
    [Test(Description = "TestValidateStripeDirectDisputesV2Open with generated merchant"), CancelAfter(720*1000)]
    public async Task TestValidateStripeDirectDisputesV2Open()
    {
        try
        {
            Console.WriteLine("\nStarting TestValidateStripeDirectDisputesV2Open");
            var (installmentPlan, _) = await InitPlan();
            var jResponseFullPlanInfo = await _fullPlanInfoIpn.SendGetFullPlanInfoIpnAsync( _requestHeader!, installmentPlan.InstallmentPlanNumber);
            Assert.That(jResponseFullPlanInfo.ResponseHeader.Succeeded);
            var jResponseDisputesResponse =
                await _postDisputesFunctionality.SendPostRequestPostDisputesAsync( _requestHeader!, 
                    installmentPlan.InstallmentPlanNumber, jResponseFullPlanInfo.InstallmentPlan.Merchant.Id, "Open");
            var jResponseDisputesAuditLog =
                await _getAuditLogsFunctionality.SendGetRequestGetAuditLogsAsync( _requestHeader!,
                    jResponseDisputesResponse.Disputes[0].Id, "0", "1000");
            Assert.That(_getAuditLogsFunctionality.ValidateAuditLogStatus(jResponseDisputesAuditLog, 
                new List<string> { "charge.dispute.created" }));
            var fullPlanInfoAfter = await _fullPlanInfoIpn.SendGetFullPlanInfoIpnAsync( _requestHeader!, installmentPlan.InstallmentPlanNumber);
            Assert.That(fullPlanInfoAfter.ResponseHeader.Succeeded);
            Assert.That(fullPlanInfoAfter.InstallmentPlan.InstallmentPlanStatus.Code, Is.EqualTo("Delayed"));
            Console.WriteLine("TestValidateStripeDirectDisputesV2Open is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidateStripeDirectDisputesV2Open\n" + exception + "\n");
        }
    }
    
    [Category("StripeDirectDisputesV2Tests")]
    [Test(Description = "TestValidateStripeDirectDisputesV2Won with generated merchant"), CancelAfter(720*1000)]
    public async Task TestValidateStripeDirectDisputesV2Won()
    {
        try
        {
            Console.WriteLine("\nStarting TestValidateStripeDirectDisputesV2Won");
            var (installmentPlan, pgtlCaptureId) = await InitPlan();
            var jResponseFullPlanInfo = await _fullPlanInfoIpn.SendGetFullPlanInfoIpnAsync( _requestHeader!, installmentPlan.InstallmentPlanNumber);
            Assert.That(jResponseFullPlanInfo.ResponseHeader.Succeeded);
            var jResponse = await _disputeResolveFunctionality.SendPutRequestDisputeResolveAsync( _requestHeader!, 
                installmentPlan.InstallmentPlanNumber, pgtlCaptureId!, "yes");
            Assert.That(jResponse.ResponseHeader.Succeeded);
            var jResponseDisputesResponse = await _postDisputesFunctionality.SendPostRequestPostDisputesAsync( _requestHeader!,
                    installmentPlan.InstallmentPlanNumber, jResponseFullPlanInfo.InstallmentPlan.Merchant.Id, "Won");
            Assert.That(_postDisputesFunctionality.ValidateDisputeStatus(jResponseDisputesResponse, "Won"));
            var jResponseDisputesAuditLog =
                await _getAuditLogsFunctionality.SendGetRequestGetAuditLogsAsync( _requestHeader!,
                    jResponseDisputesResponse.Disputes[0].Id, "0", "1000");
            Assert.That(_getAuditLogsFunctionality.ValidateAuditLogStatus(jResponseDisputesAuditLog, new List<string> { "charge.dispute.created" }));
            var fullPlanInfoAfter = await _fullPlanInfoIpn.SendGetFullPlanInfoIpnAsync( _requestHeader!, installmentPlan.InstallmentPlanNumber);
            Assert.That(fullPlanInfoAfter.ResponseHeader.Succeeded);
            Assert.That(fullPlanInfoAfter.InstallmentPlan.IsFullCaptured);
            Assert.That(fullPlanInfoAfter.InstallmentPlan.InstallmentPlanStatus.Code, Is.EqualTo("Cleared"));
            Console.WriteLine("TestValidateStripeDirectDisputesV2Won is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidateStripeDirectDisputesV2Won\n" + exception + "\n");
        }
    }
    
    [Category("StripeDirectDisputesV2Tests")]
    [Test(Description = "TestValidateStripeDirectDisputesV2Lost with generated merchant"), CancelAfter(720*1000)]
    public async Task TestValidateStripeDirectDisputesV2Lost()
    {
        try
        {
            Console.WriteLine("\nStarting TestValidateStripeDirectDisputesV2Lost");
            var (installmentPlan, pgtlCaptureId) = await InitPlan();
            var jResponseFullPlanInfo = await _fullPlanInfoIpn.SendGetFullPlanInfoIpnAsync( _requestHeader!, installmentPlan.InstallmentPlanNumber);
            Assert.That(jResponseFullPlanInfo.ResponseHeader.Succeeded);
            var jResponse = await _disputeResolveFunctionality.SendPutRequestDisputeResolveAsync( _requestHeader!, 
                installmentPlan.InstallmentPlanNumber, pgtlCaptureId!);
            Assert.That(jResponse.ResponseHeader.Succeeded);
            var jResponseDisputesResponse = await _postDisputesFunctionality.SendPostRequestPostDisputesAsync( _requestHeader!,
                installmentPlan.InstallmentPlanNumber, jResponseFullPlanInfo.InstallmentPlan.Merchant.Id, "Lost");
            var jResponseDisputesAuditLog =
                await _getAuditLogsFunctionality.SendGetRequestGetAuditLogsAsync( _requestHeader!,
                    jResponseDisputesResponse.Disputes[0].Id, "0", "1000");
            Assert.That(_getAuditLogsFunctionality.ValidateAuditLogsActions(jResponseDisputesAuditLog, new List<string> { "Dispute:Resolve", "Webhook:StripeDirect" }));
            var fullPlanInfoAfter = await _fullPlanInfoIpn.SendGetFullPlanInfoIpnAsync( _requestHeader!, installmentPlan.InstallmentPlanNumber);
            Assert.That(fullPlanInfoAfter.InstallmentPlan.OutstandingAmount.Value, Is.EqualTo(0));
            Assert.That(fullPlanInfoAfter.InstallmentPlan.InstallmentPlanStatus.Code, Is.EqualTo("Canceled"));
            Console.WriteLine("TestValidateStripeDirectDisputesV2Lost is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidateStripeDirectDisputesV2Lost\n" + exception + "\n");
        }
    }

    private async Task<(ResponseV3.ResponseRoot installmentPlan, string? pgtlCaptureId)> InitPlan()
    {
        try
        {
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            createPlanDefaultValues.paymentMethod.card.cardHolderFullName = "John Smith";
            createPlanDefaultValues.paymentMethod.card.cardNumber = "4000000000000259";
            createPlanDefaultValues.paymentMethod.card.cardExpYear = 2030;
            createPlanDefaultValues.paymentMethod.card.cardExpMonth = 1;
            createPlanDefaultValues.paymentMethod.card.cardCvv = "737";
            var installmentPlan = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!,  _requestHeader!,
                "Active", new Random().Next(2, 6), Environment.GetEnvironmentVariable("StripeDirectTerminal")!,
                createPlanDefaultValues, shouldCancel: false);
            
            var pgtlCaptureId = await _getPgtl.ValidatePgtlKeyValueInnerAsync(Environment.GetEnvironmentVariable("StoreProcedureUrl")!,
                _requestHeader!, installmentPlan.InstallmentPlanNumber, "Type", "Capture","Id");
            return (installmentPlan, pgtlCaptureId);
        }
        catch (Exception e)
        {
            Console.WriteLine("Error in InitPlan" + e);
            throw;
        }
    }
}