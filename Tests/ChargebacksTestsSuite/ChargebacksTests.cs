using Allure.NUnit;
using Allure.NUnit.Attributes;
using NUnit.Framework;
using Splitit.Automation.NG.Backend.BaseActions;
using Splitit.Automation.NG.Backend.Services.AdminApi.InstallmentPlan.InstallmentsPlanFunctionality;
using Splitit.Automation.NG.Backend.Services.Chargebacks.Functionality;
using Splitit.Automation.NG.Backend.Services.DisputesV2.Dispute.Functionality;
using Splitit.Automation.NG.Backend.Services.Notifications.Bluesnap.BluesnapNotificationsFunctionality;
using Splitit.Automation.NG.Backend.Services.V1.InstallmentPlan.InstallmentPlanApiEndPoints;
using Splitit.Automation.NG.Backend.Services.V3.DefaultValues;
using Splitit.Automation.NG.Backend.Services.V3.InstallmentPlan.InstallmentPlanApiEndPoint;
using Splitit.Automation.NG.Backend.Services.V3.InstallmentPlan.InstallmentPlanResponses;
using Splitit.Automation.NG.Backend.Tests.DisputesV2Tests.TestsData;
using Splitit.Automation.NG.Backend.Tests.TestsSetups;

namespace Splitit.Automation.NG.Backend.Tests.ChargebacksTestsSuite;

[TestFixture]
[AllureNUnit]
[AllureSuite("ChargebacksTests")]
[AllureDisplayIgnored]
//[Parallelizable(ParallelScope.All)]
public class ChargebacksTests
{
    private RequestHeader? _requestHeader;
    private RequestHeader? _requestHeaderOther;
    private readonly FunctionalityUtil _functionalityUtil;
    private readonly InstallmentPlans _installmentPlans;
    private readonly PostDisputesFunctionality _postDisputesFunctionality;
    private readonly GetPgtl _getPgtl;
    private readonly BluesnapFunctionality _bluesnapFunctionality;
    private readonly FullPlanInfoIpn _fullPlanInfoIpn;
    private readonly TestsData _testsData;
    

    public ChargebacksTests()
    {
        Console.WriteLine("Starting ChargebacksTests setup");
        _installmentPlans = new InstallmentPlans();
        _postDisputesFunctionality = new PostDisputesFunctionality();
        _getPgtl = new GetPgtl();
        _bluesnapFunctionality = new BluesnapFunctionality();
        _testsData = new TestsData();
        _fullPlanInfoIpn = new FullPlanInfoIpn();
        _functionalityUtil = new FunctionalityUtil();
        Console.WriteLine("Done with ChargebacksTests setup");
    }
    
    [OneTimeSetUp]
    public async Task InitSetUp()
    {
        var testsSetup = new TestsSetup();
        testsSetup.Setup();
        var sendAdminLoginRequest = new SendAdminLoginRequest();
        _requestHeader = await sendAdminLoginRequest.DoClientLogin(
            Environment.GetEnvironmentVariable("AccessTokenURI")!,
            Environment.GetEnvironmentVariable("DummyMerchantForDlpTestPassword")!, 
            Environment.GetEnvironmentVariable("DummyMerchantForDlpTestTerminal")!,
            Environment.GetEnvironmentVariable("DummyMerchantForDlpTestClientId")!);
        _requestHeaderOther = await sendAdminLoginRequest.DoClientLogin(
            Environment.GetEnvironmentVariable("AccessTokenURI")!,
            Environment.GetEnvironmentVariable("UserAutomationDisputesTestPassword")!, 
            Environment.GetEnvironmentVariable("MerchantDisputesTestTerminal")!,
            Environment.GetEnvironmentVariable("UserAutomationDisputesTestUserName")!);
    }
    
    [Category("ChargebacksTests")]
    [Test(Description = "TestValidateOpenDispute with generated merchant"), CancelAfter(120*1000)]
    public async Task TestValidateOpenDispute()
    {
        try
        {
            Console.WriteLine("\nStarting TestValidateOpenDispute");
            var (installmentPlan, _) = await InitPlan();
            var disputesResponse =
                await _postDisputesFunctionality.SendPostRequestPostDisputesAsync(
                    _requestHeaderOther!, installmentPlan.InstallmentPlanNumber, 
                    int.Parse(Environment.GetEnvironmentVariable("MerchantDisputesClientId")!), "Open");
            var getDisputes = await _functionalityUtil.
                SendGetRequestGetChargebacksIdAsync(_requestHeaderOther!, disputesResponse.Disputes[0].Id);
            Assert.That(getDisputes.Chargeback.Status.Equals("Open"));
            var getPlanInfo =
                await _fullPlanInfoIpn.SendGetFullPlanInfoIpnAsync( _requestHeaderOther!, installmentPlan.InstallmentPlanNumber);
            if (getPlanInfo.InstallmentPlan.InstallmentPlanStatus.Code != "Delayed" && 
                getPlanInfo.InstallmentPlan.InstallmentPlanStatus.Code != "Canceled" && 
                getPlanInfo.InstallmentPlan.InstallmentPlanStatus.Code != "Cleared")
            {
                Assert.Fail("Plan status is not in the current state, should be -> Delayed " +
                            "OR -> Canceled but is: " + getPlanInfo.InstallmentPlan.InstallmentPlanStatus.Code);
            }
            Console.WriteLine("TestValidateOpenDispute is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidateOpenDispute\n" + exception + "\n");
        }
    }
    
    [Category("ChargebacksTests")]
    [Test(Description = "TestValidateGetAllDisputes with generated merchant"), CancelAfter(120*1000)]
    public async Task TestValidateGetAllDisputes()
    {
        try
        {
            Console.WriteLine("\nStarting TestValidateGetAllDisputes");
            var (installmentPlan, _) = await InitPlan();
            var disputesResponse =
                await _postDisputesFunctionality.SendPostRequestPostDisputesAsync(
                    _requestHeaderOther!, installmentPlan.InstallmentPlanNumber, 
                    int.Parse(Environment.GetEnvironmentVariable("MerchantDisputesClientId")!), "Open");
            Assert.That(disputesResponse.IsSuccess);
            var getDisputes = await _functionalityUtil.SendGetRequestGetChargebacksAsync(
                _requestHeaderOther!, "Open",DateTime.Now.AddDays(-3).ToString("MM/dd/yyyy"), DateTime.Now.ToString("MM/dd/yyyy"), 
                "0", "100");
            Assert.That(getDisputes.Chargebacks[0].Status, Is.EqualTo("Open"));
            var getPlanInfo =
                await _fullPlanInfoIpn.SendGetFullPlanInfoIpnAsync( _requestHeaderOther!, installmentPlan.InstallmentPlanNumber);
            if (getPlanInfo.InstallmentPlan.InstallmentPlanStatus.Code != "Delayed" && 
                getPlanInfo.InstallmentPlan.InstallmentPlanStatus.Code != "Canceled" && 
                getPlanInfo.InstallmentPlan.InstallmentPlanStatus.Code != "Cleared")
            {
                Assert.Fail("Plan status is not in the current state, should be -> Delayed " +
                            "OR -> Canceled but is: " + getPlanInfo.InstallmentPlan.InstallmentPlanStatus.Code);
            }
            Console.WriteLine("TestValidateGetAllDisputes is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidateGetAllDisputes\n" + exception + "\n");
        }
    }
    
    [Category("ChargebacksTests")]
    [Test(Description = "TestValidateAcceptChargebacks with generated merchant"), CancelAfter(120*1000)]
    public async Task TestValidateAcceptChargebacks()
    {
        try
        {
            Console.WriteLine("\nStarting TestValidateAcceptChargebacks");
            var (installmentPlan, _) = await InitPlan();
            var disputesResponse =
                await _postDisputesFunctionality.SendPostRequestPostDisputesAsync(
                    _requestHeaderOther!, installmentPlan.InstallmentPlanNumber, 
                    int.Parse(Environment.GetEnvironmentVariable("MerchantDisputesClientId")!), "Open");
            var getDisputes = await _functionalityUtil.
                SendGetRequestGetChargebacksIdAsync(_requestHeaderOther!, disputesResponse.Disputes[0].Id);
            Assert.That(getDisputes.Chargeback.Status, Is.EqualTo("Open"));
            var accept =
                await _functionalityUtil.
                    SendUpdateRequestPutChargebacksIdAcceptAsync(_requestHeaderOther!, 
                        disputesResponse.Disputes[0].Id, true);
            Assert.That(accept!.Chargeback.Status, Is.EqualTo("Open"));
            var disputesResponseAfter =
                await _postDisputesFunctionality.SendPostRequestPostDisputesAsync(
                    _requestHeaderOther!, installmentPlan.InstallmentPlanNumber, 
                    int.Parse(Environment.GetEnvironmentVariable("MerchantDisputesClientId")!), "Open");
            Assert.That(disputesResponseAfter.Disputes[0].Accept, Is.EqualTo("accepted"));
            Console.WriteLine("TestValidateAcceptChargebacks is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidateAcceptChargebacks\n" + exception + "\n");
        }
    }
    
    [Category("ChargebacksTests")]
    [Test(Description = "TestValidateChargebacksOpenDisputeAndUploadEvidenceFileInternalEvidenceFalse with generated merchant"), CancelAfter(720*1000)]
    public async Task TestValidateChargebacksOpenDisputeAndUploadEvidenceFileInternalEvidenceFalse()
    {
        try
        {
            
            Console.WriteLine("\nStarting TestValidateChargebacksOpenDisputeAndUploadEvidenceFileInternalEvidenceFalse");
            var (installmentPlan, _) = await InitPlan();
            var disputesResponse =
                await _postDisputesFunctionality.SendPostRequestPostDisputesAsync( _requestHeaderOther!, 
                    installmentPlan.InstallmentPlanNumber, int.Parse(Environment.GetEnvironmentVariable("MerchantDisputesClientId")!), "Open");
            var filePath = _testsData.ReturnEvidenceFileLocation("dummy.pdf");
            var responseUploadFile =
                await _functionalityUtil.SendPostRequestPostUploadAsync(
                    _requestHeaderOther!, disputesResponse.Disputes[0].Id, false, filePath,
                    "dummy", "dummy.pdf");
            Assert.That(responseUploadFile, Is.Not.Null);
            Console.WriteLine("TestValidateChargebacksOpenDisputeAndUploadEvidenceFileInternalEvidenceFalse is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidateChargebacksOpenDisputeAndUploadEvidenceFileInternalEvidenceFalse\n" + exception + "\n");
        }
    }
    
    [Category("ChargebacksTests")]
    [Test(Description = "TestValidateChargebacksOpenDisputeAndUploadEvidenceFileInternalEvidenceFalseAndDeleteEvidence with generated merchant"), CancelAfter(720*1000)]
    public async Task TestValidateChargebacksOpenDisputeAndUploadEvidenceFileInternalEvidenceFalseAndDeleteEvidence()
    {
        try
        {
            
            Console.WriteLine("\nStarting TestValidateChargebacksOpenDisputeAndUploadEvidenceFileInternalEvidenceFalseAndDeleteEvidence");
            var (installmentPlan, _) = await InitPlan();
            var disputesResponse =
                await _postDisputesFunctionality.SendPostRequestPostDisputesAsync( _requestHeaderOther!, 
                    installmentPlan.InstallmentPlanNumber, int.Parse(Environment.GetEnvironmentVariable("MerchantDisputesClientId")!), "Open");
            var filePath = _testsData.ReturnEvidenceFileLocation("dummy.pdf");
            var responseUploadFile =
                await _functionalityUtil.SendPostRequestPostUploadAsync( 
                    _requestHeaderOther!, disputesResponse.Disputes[0].Id, false,
                    filePath, "dummy", "dummy.pdf");
            Assert.That(responseUploadFile, Is.Not.Null);
            var evidences =
                await _functionalityUtil.SendGetRequestGetChargebacksIdAsync(
                     _requestHeaderOther!, disputesResponse.Disputes[0].Id);
            Assert.That(evidences.Chargeback.Evidences.Count, Is.EqualTo(1));
            var delete = await _functionalityUtil.SendDeleteRequestDeleteChargebacksIdDeleteEvidence(
                 _requestHeaderOther!, disputesResponse.Disputes[0].Id, evidences.Chargeback.Evidences[0].EvidenceId);
            Assert.That(delete.IsSuccess);
            Console.WriteLine("TestValidateChargebacksOpenDisputeAndUploadEvidenceFileInternalEvidenceFalseAndDeleteEvidence is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidateChargebacksOpenDisputeAndUploadEvidenceFileInternalEvidenceFalseAndDeleteEvidence\n" + exception + "\n");
        }
    }
    
    [Category("ChargebacksTests")]
    [Test(Description = "TestValidateChargebacksOpenDisputeAndAddCommentInternalCommentFalse with generated merchant"), CancelAfter(720*1000)]
    public async Task TestValidateChargebacksOpenDisputeAndAddCommentInternalCommentFalse()
    {
        try
        {
            
            Console.WriteLine("\nStarting TestValidateChargebacksOpenDisputeAndAddCommentInternalCommentFalse");
            var (installmentPlan, _) = await InitPlan();
            var disputesResponse =
                await _postDisputesFunctionality.SendPostRequestPostDisputesAsync( _requestHeaderOther!, 
                    installmentPlan.InstallmentPlanNumber, int.Parse(Environment.GetEnvironmentVariable("MerchantDisputesClientId")!), "Open");
            var postComments = await _functionalityUtil.SendPostRequestCommentsAsync(
                 _requestHeaderOther!, disputesResponse.Disputes[0].Id,
                "Automation testing", "splitit.automation@splitit.com", null!, false);
            Assert.That(postComments.Comment.CommentId, Is.Not.Null);
            await Task.Delay(10*1000);
            var chargeBack = await _functionalityUtil.SendGetRequestGetChargebacksIdAsync(
                     _requestHeaderOther!, disputesResponse.Disputes[0].Id);
            Assert.That(chargeBack.Chargeback.Comments.Count > 0);
            Console.WriteLine("TestValidateChargebacksOpenDisputeAndAddCommentInternalCommentFalse is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidateChargebacksOpenDisputeAndAddCommentInternalCommentFalse\n" + exception + "\n");
        }
    }
    
    [Category("ChargebacksTests")]
    [Test(Description = "TestValidateChargebacksOpenDisputeAndAddCommentInternalCommentFalseAddDeleteComment with generated merchant"), CancelAfter(720*1000)]
    public async Task TestValidateChargebacksOpenDisputeAndAddCommentInternalCommentFalseAddDeleteComment()
    {
        try
        {
            
            Console.WriteLine("\nStarting TestValidateChargebacksOpenDisputeAndAddCommentInternalCommentFalseAddDeleteComment");
            var (installmentPlan, _) = await InitPlan();
            var disputesResponse =
                await _postDisputesFunctionality.SendPostRequestPostDisputesAsync( _requestHeaderOther!, 
                    installmentPlan.InstallmentPlanNumber, int.Parse(Environment.GetEnvironmentVariable("MerchantDisputesClientId")!), "Open");
            var postComments = await _functionalityUtil.SendPostRequestCommentsAsync(
                 _requestHeaderOther!, disputesResponse.Disputes[0].Id,
                "Automation testing", "splitit.automation@splitit.com", null!, false);
            Assert.That(postComments.Comment.CommentId, Is.Not.Null);
            await Task.Delay(10*1000);
            var chargeBack = await _functionalityUtil.SendGetRequestGetChargebacksIdAsync(
                     _requestHeaderOther!, disputesResponse.Disputes[0].Id);
            Assert.That(chargeBack.Chargeback.Comments.Count > 0);
            var deleteComment =
                await _functionalityUtil.SendDeleteChargebacksIdDeleteCommentCommentIdAsync(
                     _requestHeaderOther!, disputesResponse.Disputes[0].Id,
                     postComments.Comment.CommentId);
            Assert.That(deleteComment.Equals(""));
            var chargeBackAfterDelete = await _functionalityUtil.SendGetRequestGetChargebacksIdAsync(
                 _requestHeaderOther!, disputesResponse.Disputes[0].Id);
            Assert.That(chargeBackAfterDelete.Chargeback.Comments.Count == 0);
            Console.WriteLine("TestValidateChargebacksOpenDisputeAndAddCommentInternalCommentFalseAddDeleteComment is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidateChargebacksOpenDisputeAndAddCommentInternalCommentFalseAddDeleteComment\n" + exception + "\n");
        }
    }
    
    [Category("ChargebacksTests")]
    [Test(Description = "TestValidateChargebacksOpenDisputeAndUploadEvidenceFileInternalEvidenceFalseTryToGetWithOtherMerchant with generated merchant"), CancelAfter(720*1000)]
    public async Task TestValidateChargebacksOpenDisputeAndUploadEvidenceFileInternalEvidenceFalseTryToGetWithOtherMerchant()
    {
        try
        {
            
            Console.WriteLine("\nStarting TestValidateChargebacksOpenDisputeAndUploadEvidenceFileInternalEvidenceFalseTryToGetWithOtherMerchant");
            var (installmentPlan, _) = await InitPlan();
            var disputesResponse =
                await _postDisputesFunctionality.SendPostRequestPostDisputesAsync( _requestHeaderOther!, 
                    installmentPlan.InstallmentPlanNumber, int.Parse(Environment.GetEnvironmentVariable("MerchantDisputesClientId")!), "Open");
            var filePath = _testsData.ReturnEvidenceFileLocation("dummy.pdf");
            var responseUploadFile =
                await _functionalityUtil.SendPostRequestPostUploadAsync(
                    _requestHeaderOther!, disputesResponse.Disputes[0].Id, false, filePath,
                    "dummy", "dummy.pdf");
            Assert.That(responseUploadFile, Is.Not.Null);
            var chargeBack = await _functionalityUtil.SendGetRequestGetChargebacksIdAsync(
                _requestHeaderOther!, disputesResponse.Disputes[0].Id);
            Assert.That(chargeBack.Chargeback.Evidences.Count > 0);
            var chargeBackWithOtherMerchant = await _functionalityUtil.SendGetRequestGetChargebacksIdAsync(
                _requestHeader!, disputesResponse.Disputes[0].Id);
            Assert.That(chargeBackWithOtherMerchant.Chargeback, Is.Null);
            Console.WriteLine("TestValidateChargebacksOpenDisputeAndUploadEvidenceFileInternalEvidenceFalseTryToGetWithOtherMerchant is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidateChargebacksOpenDisputeAndUploadEvidenceFileInternalEvidenceFalseTryToGetWithOtherMerchant\n" + exception + "\n");
        }
    }
    
    [Category("ChargebacksTests")]
    [Test(Description = "TestValidateChargebacksOpenDisputeAndAddCommentInternalCommentFalseToGetWithOtherMerchant with generated merchant"), CancelAfter(720*1000)]
    public async Task TestValidateChargebacksOpenDisputeAndAddCommentInternalCommentFalseToGetWithOtherMerchant()
    {
        try
        {
            
            Console.WriteLine("\nStarting TestValidateChargebacksOpenDisputeAndAddCommentInternalCommentFalseToGetWithOtherMerchant");
            var (installmentPlan, _) = await InitPlan();
            var disputesResponse =
                await _postDisputesFunctionality.SendPostRequestPostDisputesAsync( _requestHeaderOther!, 
                    installmentPlan.InstallmentPlanNumber, int.Parse(Environment.GetEnvironmentVariable("MerchantDisputesClientId")!), "Open");
            var postComments = await _functionalityUtil.SendPostRequestCommentsAsync(
                _requestHeaderOther!, disputesResponse.Disputes[0].Id,
                "Automation testing", "splitit.automation@splitit.com", null!, false);
            Assert.That(postComments.Comment.CommentId, Is.Not.Null);
            await Task.Delay(10*1000);
            var chargeBack = await _functionalityUtil.SendGetRequestGetChargebacksIdAsync(
                _requestHeaderOther!, disputesResponse.Disputes[0].Id);
            Assert.That(chargeBack.Chargeback.Comments.Count > 0);
            var chargeBackWithOtherMerchant = await _functionalityUtil.SendGetRequestGetChargebacksIdAsync(
                _requestHeader!, disputesResponse.Disputes[0].Id);
            Assert.That(chargeBackWithOtherMerchant.Chargeback, Is.Null);
            Console.WriteLine("TestValidateChargebacksOpenDisputeAndAddCommentInternalCommentFalseToGetWithOtherMerchant is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidateChargebacksOpenDisputeAndAddCommentInternalCommentFalseToGetWithOtherMerchant\n" + exception + "\n");
        }
    }
    
    [Category("ChargebacksTests")]
    [Test(Description = "TestValidateOpenDisputeWithChargebackApi with generated merchant"), CancelAfter(120*1000)]
    public async Task TestValidateOpenDisputeWithChargebackApi()
    {
        try
        {
            Console.WriteLine("\nStarting TestValidateOpenDisputeWithChargebackApi");
            var (installmentPlan, captureId) = await InitPlanNoOpenDispute();
            var openDisputeResponse = await _functionalityUtil.SendPostRequestPostChargebacksAsync(_requestHeaderOther!,
                installmentPlan.InstallmentPlanNumber, captureId!, "USD", "Invalid Data", 
                installmentPlan.Installments[0].Amount); 
            Assert.That(openDisputeResponse!.Chargeback.Id, Is.Not.Null);
            var getChargeBacks = await _functionalityUtil.SendGetRequestGetChargebacksIdAsync(
                _requestHeaderOther!, openDisputeResponse.Chargeback.Id);
            Assert.That(getChargeBacks.Chargeback.Id, Is.Not.Null);
            var getPlanInfo =
                await _fullPlanInfoIpn.SendGetFullPlanInfoIpnAsync( _requestHeaderOther!, installmentPlan.InstallmentPlanNumber);
            if (getPlanInfo.InstallmentPlan.InstallmentPlanStatus.Code != "Delayed" && 
                getPlanInfo.InstallmentPlan.InstallmentPlanStatus.Code != "Canceled" && 
                getPlanInfo.InstallmentPlan.InstallmentPlanStatus.Code != "Cleared")
            {
                Assert.Fail("Plan status is not in the current state, should be -> Delayed " +
                            "OR -> Canceled but is: " + getPlanInfo.InstallmentPlan.InstallmentPlanStatus.Code);
            }
            Console.WriteLine("TestValidateOpenDisputeWithChargebackApi is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidateOpenDisputeWithChargebackApi\n" + exception + "\n");
        }
    }
    
    [Category("ChargebacksTests")]
    [Test(Description = "TestValidateOpenDisputeAndMarkAsWonWithChargebackApi with generated merchant"), CancelAfter(120*1000)]
    public async Task TestValidateOpenDisputeAndMarkAsWonWithChargebackApi()
    {
        try
        {
            Console.WriteLine("\nStarting TestValidateOpenDisputeAndMarkAsWonWithChargebackApi");
            var (installmentPlan, captureId) = await InitPlanNoOpenDispute();
            var openDisputeResponse = await _functionalityUtil.SendPostRequestPostChargebacksAsync(_requestHeaderOther!,
                installmentPlan.InstallmentPlanNumber, captureId!, "USD", "Invalid Data", 
                installmentPlan.Installments[0].Amount); 
            var updateStatusResponse = await _functionalityUtil.SendUpdateRequestPutChargebacksIdStatusAsync(
                _requestHeaderOther!, openDisputeResponse!.Chargeback.Id, "Won");
            Assert.That(updateStatusResponse!.Chargeback.Status.Equals("Won"));
            var getPlanInfo =
                await _fullPlanInfoIpn.SendGetFullPlanInfoIpnAsync( _requestHeaderOther!, installmentPlan.InstallmentPlanNumber);
            if (getPlanInfo.InstallmentPlan.InstallmentPlanStatus.Code != "Delayed" && 
                getPlanInfo.InstallmentPlan.InstallmentPlanStatus.Code != "Canceled" && 
                getPlanInfo.InstallmentPlan.InstallmentPlanStatus.Code != "Cleared")
            {
                Assert.Fail("Plan status is not in the current state, should be -> Delayed " +
                            "OR -> Canceled but is: " + getPlanInfo.InstallmentPlan.InstallmentPlanStatus.Code);
            }
            Console.WriteLine("TestValidateOpenDisputeAndMarkAsWonWithChargebackApi is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidateOpenDisputeAndMarkAsWonWithChargebackApi\n" + exception + "\n");
        }
    }
    
    [Category("ChargebacksTests")]
    [Test(Description = "TestValidateOpenDisputeAndMarkAsLostWithChargebackApi with generated merchant"), CancelAfter(120*1000)]
    public async Task TestValidateOpenDisputeAndMarkAsLostWithChargebackApi()
    {
        try
        {
            Console.WriteLine("\nStarting TestValidateOpenDisputeAndMarkAsLostWithChargebackApi");
            var (installmentPlan, captureId) = await InitPlanNoOpenDispute();
            var openDisputeResponse = await _functionalityUtil.SendPostRequestPostChargebacksAsync(_requestHeaderOther!,
                installmentPlan.InstallmentPlanNumber, captureId!, "USD", "Invalid Data", 
                installmentPlan.Installments[0].Amount); 
            var updateStatusResponse = await _functionalityUtil.SendUpdateRequestPutChargebacksIdStatusAsync(
                _requestHeaderOther!, openDisputeResponse!.Chargeback.Id, "Lost");
            Assert.That(updateStatusResponse!.Chargeback.Status.Equals("Lost"));
            var getPlanInfo =
                await _fullPlanInfoIpn.SendGetFullPlanInfoIpnAsync( _requestHeaderOther!, installmentPlan.InstallmentPlanNumber);
            if (getPlanInfo.InstallmentPlan.InstallmentPlanStatus.Code != "Delayed" && 
                getPlanInfo.InstallmentPlan.InstallmentPlanStatus.Code != "Canceled" && 
                getPlanInfo.InstallmentPlan.InstallmentPlanStatus.Code != "Cleared")
            {
                Assert.Fail("Plan status is not in the current state, should be -> Delayed " +
                            "OR -> Canceled but is: " + getPlanInfo.InstallmentPlan.InstallmentPlanStatus.Code);
            }
            Console.WriteLine("TestValidateOpenDisputeAndMarkAsLostWithChargebackApi is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidateOpenDisputeAndMarkAsLostWithChargebackApi\n" + exception + "\n");
        }
    }

    private async Task<(ResponseV3.ResponseRoot installmentPlan, string? captureId)> InitPlan()
    {
        try
        {
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            var installmentPlan = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!,  _requestHeaderOther!,
                "Active", new Random().Next(2, 6), Environment.GetEnvironmentVariable("MerchantDisputesTestTerminal")!,
                createPlanDefaultValues);
            var captureId = await _getPgtl.ValidatePgtlKeyValueAsync(Environment.GetEnvironmentVariable("StoreProcedureUrl")!,
                _requestHeaderOther!, installmentPlan.InstallmentPlanNumber, "CaptureId");
            var notificationResponse = await _bluesnapFunctionality.SendPostRequestBluesnapNotifications(
                Environment.GetEnvironmentVariable("NotificationUrl")!, captureId!, "CHARGEBACK", "NEW", 
                "Set-Liabilty-Merchant");
            Assert.That(notificationResponse.Contains("accepted"));
            return (installmentPlan, captureId);
        }
        catch (Exception e)
        {
            Console.WriteLine("Error in InitPlan" + e);
            throw;
        }
    }
    
    private async Task<(ResponseV3.ResponseRoot installmentPlan, string? captureId)> InitPlanNoOpenDispute()
    {
        try
        {
            Console.WriteLine("Starting InitPlanNoOpenDispute");
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            var installmentPlan = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!,  _requestHeaderOther!,
                "Active", new Random().Next(2, 6), Environment.GetEnvironmentVariable("MerchantDisputesTestTerminal")!,
                createPlanDefaultValues);
            var captureId = await _getPgtl.ValidatePgtlKeyValueAsync(Environment.GetEnvironmentVariable("StoreProcedureUrl")!,
                _requestHeaderOther!, installmentPlan.InstallmentPlanNumber, "CaptureId");
            Console.WriteLine("Done with InitPlanNoOpenDispute");
            await Task.Delay(5 * 1000);
            return (installmentPlan, captureId);
        }
        catch (Exception e)
        {
            Console.WriteLine("Error in InitPlanNoOpenDispute" + e);
            throw;
        }
    }
}