using Allure.NUnit;
using Allure.NUnit.Attributes;
using NUnit.Framework;
using Splitit.Automation.NG.Backend.BaseActions;
using Splitit.Automation.NG.Backend.Services.AdminApi.InstallmentPlan.InstallmentsPlanFunctionality;
using Splitit.Automation.NG.Backend.Services.DisputesV2.AuditLogs.Functionality;
using Splitit.Automation.NG.Backend.Services.DisputesV2.Comments.Functionality;
using Splitit.Automation.NG.Backend.Services.DisputesV2.Evidence.Functionality;
using Splitit.Automation.NG.Backend.Services.MerchantPortal.ChargeBack.Functionality;
using Splitit.Automation.NG.Backend.Services.Notifications.Bluesnap.BluesnapNotificationsFunctionality;
using Splitit.Automation.NG.Backend.Services.V1.InstallmentPlan.InstallmentPlanApiEndPoints;
using Splitit.Automation.NG.Backend.Services.V3.DefaultValues;
using Splitit.Automation.NG.Backend.Services.V3.InstallmentPlan.InstallmentPlanApiEndPoint;
using Splitit.Automation.NG.Backend.Services.V3.InstallmentPlan.InstallmentPlanResponses;
using Splitit.Automation.NG.Backend.Tests.TestsSetups;
using PostDisputesFunctionality = Splitit.Automation.NG.Backend.Services.DisputesV2.Dispute.Functionality.PostDisputesFunctionality;

namespace Splitit.Automation.NG.Backend.Tests.DisputesV2Tests.DisputesV2TestsSuites;

[TestFixture]
[AllureNUnit]
[AllureSuite("BlueSnapMorDisputesV2Tests")]
[AllureDisplayIgnored]
[Parallelizable(ParallelScope.All)]
public class DisputesV2BlueSnapMorTests
{
    private RequestHeader? _requestHeader;
    private readonly InstallmentPlans _installmentPlans;
    private string? _terminalApiKey;
    private int _merchantId;
    private readonly BluesnapFunctionality _bluesnapFunctionality;
    private readonly GetPgtl _getPgtl;
    private readonly PostDisputesFunctionality _postDisputesFunctionality;
    private readonly PostUploadFunctionality _postUploadFunctionality;
    private readonly TestsData.TestsData _testsData;
    private readonly PostCommentsDisputeIdFunctionality _postCommentsDisputeIdFunctionality;
    private readonly GetDisputesDisputesIdFunctionality _getDisputesDisputesIdFunctionality;
    private readonly DeleteCommentsDisputeIdCommentsIdFunctionality _deleteCommentsDisputeIdCommentsIdFunctionality;
    private readonly PostDisputesFunctionality _postDisputesFunctionalityMerchantPortal;
    private readonly GetDisputesDisputesIdFunctionality _getDisputesDisputesIdFunctionalityMerchantPortal;
    private readonly DeleteDisputesIdDeleteCommentCommentIdFunctionality _deleteDisputesIdDeleteCommentCommentIdFunctionality;
    private readonly PostDownloadDisputeIdEvidenceIdFunctionality _postDownloadDisputeIdEvidenceIdFunctionality;
    private readonly GetEvidencesDisputeIdFunctionality _getEvidencesDisputeIdFunctionality;
    private readonly Compare2Files _compare2Files;
    private readonly DeleteEvidencesFunctionality _deleteEvidencesFunctionality;
    private readonly PostUploadEvidenceFunctionality _postUploadEvidenceFunctionality;
    private readonly PostDisputesIdDownloadEvidenceEvidenceIdFunctionality _postDisputesIdDownloadEvidenceEvidenceIdFunctionality;
    private readonly PostDisputesIdDeleteEvidenceFunctionality _postDisputesIdDeleteEvidenceFunctionality;
    private readonly FullPlanInfoIpn _fullPlanInfoIpn;
    private readonly GetAuditLogsFunctionality _getAuditLogsFunctionality;


    public DisputesV2BlueSnapMorTests()
    {
        Console.WriteLine("\nStaring DisputesV2BlueSnapMorTests Setup");
        _installmentPlans = new InstallmentPlans();
        _bluesnapFunctionality = new BluesnapFunctionality();
        _getPgtl = new GetPgtl();
        _postDisputesFunctionality = new PostDisputesFunctionality();
        _postUploadFunctionality = new PostUploadFunctionality();
        _testsData = new TestsData.TestsData();
        _postCommentsDisputeIdFunctionality = new PostCommentsDisputeIdFunctionality();
        _getDisputesDisputesIdFunctionality = new GetDisputesDisputesIdFunctionality();
        _deleteCommentsDisputeIdCommentsIdFunctionality = new DeleteCommentsDisputeIdCommentsIdFunctionality();
        _postDisputesFunctionalityMerchantPortal = new PostDisputesFunctionality();
        _getDisputesDisputesIdFunctionalityMerchantPortal = new GetDisputesDisputesIdFunctionality();
        _deleteDisputesIdDeleteCommentCommentIdFunctionality = new DeleteDisputesIdDeleteCommentCommentIdFunctionality();
        _postDownloadDisputeIdEvidenceIdFunctionality = new PostDownloadDisputeIdEvidenceIdFunctionality();
        _getEvidencesDisputeIdFunctionality = new GetEvidencesDisputeIdFunctionality();
        _compare2Files = new Compare2Files();
        _deleteEvidencesFunctionality = new DeleteEvidencesFunctionality();
        _postUploadEvidenceFunctionality = new PostUploadEvidenceFunctionality();
        _postDisputesIdDownloadEvidenceEvidenceIdFunctionality = new PostDisputesIdDownloadEvidenceEvidenceIdFunctionality();
        _postDisputesIdDeleteEvidenceFunctionality = new PostDisputesIdDeleteEvidenceFunctionality();
        _fullPlanInfoIpn = new FullPlanInfoIpn();
        _getAuditLogsFunctionality = new GetAuditLogsFunctionality();
        Console.WriteLine("DisputesV2BlueSnapMorTests Setup Succeeded\n");
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
        _terminalApiKey = Environment.GetEnvironmentVariable("BlueSnapMorDisputesV2Terminal")!;
        _merchantId = int.Parse(Environment.GetEnvironmentVariable("BlueSnapMorDisputesV2MerchantId")!);
    }

    [Category("BlueSnapMorDisputesV2Tests")]
    [Test(Description = "TestValidateBlueSnapMorDisputesV2Open with generated merchant"), CancelAfter(720 * 1000)]
    public async Task TestValidateBlueSnapMorDisputesV2Open()
    {
        try
        {
            Console.WriteLine("\nStarting TestValidateBlueSnapMorDisputesV2Open");
            var (installmentPlan, captureId) = await InitPlan();

            await _bluesnapFunctionality
                .SendPostRequestBluesnapNotifications(Environment.GetEnvironmentVariable("NotificationUrl")!,
                    captureId!, "CHARGEBACK", "NEW", "Set-Liabilty-Merchant");

            var jResponseDisputesResponse =
                await _postDisputesFunctionality.SendPostRequestPostDisputesAsync(_requestHeader!,
                    installmentPlan.InstallmentPlanNumber, _merchantId, "Open");
            var jResponseDisputesAuditLog =
                await _getAuditLogsFunctionality.SendGetRequestGetAuditLogsAsync(_requestHeader!,
                    jResponseDisputesResponse.Disputes[0].Id, "0", "1000");

            Assert.That(_getAuditLogsFunctionality.ValidateAuditLogStatus(jResponseDisputesAuditLog,
                new List<string> { "Status=NEW" }));

            var fullPlanInfoAfter =
                await _fullPlanInfoIpn.SendGetFullPlanInfoIpnAsync(_requestHeader!,
                    installmentPlan.InstallmentPlanNumber);
            Assert.That(fullPlanInfoAfter.InstallmentPlan.InstallmentPlanStatus.Code, Is.EqualTo("Delayed"));
            Console.WriteLine("TestValidateBlueSnapMorDisputesV2Open is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidateBlueSnapMorDisputesV2Open\n" + exception + "\n");
        }
    }

    [Category("BlueSnapMorDisputesV2Tests")]
    [Test(Description = "TestValidateBlueSnapMorDisputesV2Won with generated merchant"), CancelAfter(720 * 1000)]
    public async Task TestValidateBlueSnapMorDisputesV2Won()
    {
        try
        {
            Console.WriteLine("\nStarting TestValidateBlueSnapMorDisputesV2Won");
            var (installmentPlan, captureId) = await InitPlan();

            await _bluesnapFunctionality.SendPostRequestBluesnapNotifications(
                Environment.GetEnvironmentVariable("NotificationUrl")!, captureId!, "CHARGEBACK", "Completed_Won",
                "Set-Liabilty-Merchant");

            var jResponseDisputesResponse =
                await _postDisputesFunctionality.SendPostRequestPostDisputesAsync(_requestHeader!,
                    installmentPlan.InstallmentPlanNumber, _merchantId, "Won");
            Assert.That(jResponseDisputesResponse.Disputes[0].Status.Contains("Won"));

            var jResponseDisputesAuditLog =
                await _getAuditLogsFunctionality.SendGetRequestGetAuditLogsAsync(_requestHeader!,
                    jResponseDisputesResponse.Disputes[0].Id, "0", "1000");
            Assert.That(_getAuditLogsFunctionality.ValidateAuditLogStatus(jResponseDisputesAuditLog,
                new List<string> { "Status=Completed_Won" }));
            var fullPlanInfoAfter =
                await _fullPlanInfoIpn.SendGetFullPlanInfoIpnAsync(_requestHeader!,
                    installmentPlan.InstallmentPlanNumber);

            Assert.That(fullPlanInfoAfter.ResponseHeader.Succeeded);
            Assert.That(fullPlanInfoAfter.InstallmentPlan.IsFullCaptured);
            Console.WriteLine("TestValidateBlueSnapMorDisputesV2Won is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidateBlueSnapMorDisputesV2Won\n" + exception + "\n");
        }
    }

    [Category("BlueSnapMorDisputesV2Tests")]
    [Test(Description = "TestValidateBlueSnapMorDisputesV2Lost with generated merchant"), CancelAfter(720 * 1000)]
    public async Task TestValidateBlueSnapMorDisputesV2Lost()
    {
        try
        {
            Console.WriteLine("\nStarting TestValidateBlueSnapMorDisputesV2Lost");
            var (installmentPlan, captureId) = await InitPlan();
            await _bluesnapFunctionality.SendPostRequestBluesnapNotifications(
                Environment.GetEnvironmentVariable("NotificationUrl")!, captureId!, "CHARGEBACK", "Completed_Lost",
                "Set-Liabilty-Merchant");

            var jResponseDisputesResponse =
                await _postDisputesFunctionality.SendPostRequestPostDisputesAsync(_requestHeader!,
                    installmentPlan.InstallmentPlanNumber, _merchantId, "Lost");
            Assert.That(jResponseDisputesResponse.Disputes[0].Status.Contains("Lost"));

            var jResponseDisputesAuditLog =
                await _getAuditLogsFunctionality.SendGetRequestGetAuditLogsAsync(_requestHeader!,
                    jResponseDisputesResponse.Disputes[0].Id, "0", "1000");
            Assert.That(_getAuditLogsFunctionality.ValidateAuditLogStatus(jResponseDisputesAuditLog,
                new List<string> { "Status=Completed_Lost" }));

            var fullPlanInfoAfter =
                await _fullPlanInfoIpn.SendGetFullPlanInfoIpnAsync(_requestHeader!,
                    installmentPlan.InstallmentPlanNumber);
            Assert.That(fullPlanInfoAfter.InstallmentPlan.InstallmentPlanStatus.Code, Is.EqualTo("Canceled"));
            Assert.That(0, Is.EqualTo(fullPlanInfoAfter.InstallmentPlan.OutstandingAmount.Value));
            Console.WriteLine("TestValidateBlueSnapMorDisputesV2Lost is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidateBlueSnapMorDisputesV2Lost\n" + exception + "\n");
        }
    }

    [Category("BlueSnapMorDisputesV2Tests")]
    [Test(Description =
         "TestValidateBlueSnapMorDisputesV2OpenDisputeAndUploadEvidenceFileInternalEvidenceFalse with generated merchant"),
     CancelAfter(720 * 1000)]
    public async Task TestValidateBlueSnapMorDisputesV2OpenDisputeAndUploadEvidenceFileInternalEvidenceFalse()
    {
        try
        {
            Console.WriteLine(
                "\nStarting TestValidateBlueSnapMorDisputesV2OpenDisputeAndUploadEvidenceFileInternalEvidenceFalse");
            var (installmentPlan, captureId) = await InitPlan();

            await _bluesnapFunctionality
                .SendPostRequestBluesnapNotifications(Environment.GetEnvironmentVariable("NotificationUrl")!,
                    captureId!, "CHARGEBACK", "NEW", "Set-Liabilty-Merchant");

            var jResponseDisputesResponse =
                await _postDisputesFunctionality.SendPostRequestPostDisputesAsync(_requestHeader!,
                    installmentPlan.InstallmentPlanNumber, _merchantId, "Open");
            var filePath = _testsData.ReturnEvidenceFileLocation("dummy.pdf");
            var responseUploadFile =
                await _postUploadFunctionality.SendPostRequestPostUploadAsync(_requestHeader!,
                    jResponseDisputesResponse.Disputes[0].Id, false, filePath, "dummy", "dummy.pdf");
            Assert.That(responseUploadFile, Is.Not.Null);
            var jResponseDisputesAuditLog =
                await _getAuditLogsFunctionality.SendGetRequestGetAuditLogsAsync(_requestHeader!,
                    jResponseDisputesResponse.Disputes[0].Id, "0", "1000");
            Assert.That(_getAuditLogsFunctionality.ValidateAuditLogStatus(jResponseDisputesAuditLog,
                new List<string> { "Status=NEW" }));
            Assert.That(_getAuditLogsFunctionality.ValidateAuditLogsActions(jResponseDisputesAuditLog,
                new List<string> { "Evidence:Upload", "Webhook:BlueSnapMOR" }));
            Console.WriteLine(
                "TestValidateBlueSnapMorDisputesV2OpenDisputeAndUploadEvidenceFileInternalEvidenceFalse is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail(
                "Error in TestValidateBlueSnapMorDisputesV2OpenDisputeAndUploadEvidenceFileInternalEvidenceFalse\n" +
                exception + "\n");
        }
    }

    [Category("BlueSnapMorDisputesV2Tests")]
    [Test(Description =
         "TestValidateBlueSnapMorDisputesV2OpenDisputeAndAddCommentInternalCommentTrue with generated merchant"),
     CancelAfter(720 * 1000)]
    public async Task TestValidateBlueSnapMorDisputesV2OpenDisputeAndAddCommentInternalCommentTrue()
    {
        try
        {
            Console.WriteLine(
                "\nStarting TestValidateBlueSnapMorDisputesV2OpenDisputeAndAddCommentInternalCommentTrue");
            var (installmentPlan, captureId) = await InitPlan();

            await _bluesnapFunctionality
                .SendPostRequestBluesnapNotifications(Environment.GetEnvironmentVariable("NotificationUrl")!,
                    captureId!, "CHARGEBACK", "NEW", "Set-Liabilty-Merchant");

            var jResponseDisputesResponse =
                await _postDisputesFunctionality.SendPostRequestPostDisputesAsync(_requestHeader!,
                    installmentPlan.InstallmentPlanNumber, _merchantId, "Open");

            var jResponsePostComments = await _postCommentsDisputeIdFunctionality.SendPostCommentsDisputeIdRequestAsync(
                _requestHeader!, jResponseDisputesResponse.Disputes[0].Id,
                "Automation testing", "splitit.automation@splitit.com", null!, true);

            Assert.That(jResponsePostComments.IsSuccess);
            Assert.That(jResponsePostComments.Comment.Internal);
            var jResponseChargeBack =
                await _getDisputesDisputesIdFunctionality.SendGetRequestGetDisputesDisputesIdAsync(
                    _requestHeader!, jResponseDisputesResponse.Disputes[0].Id, _merchantId.ToString());
            Assert.That(jResponseChargeBack.Dispute.Comments.Count == 0);
            var jResponseDisputesAuditLog =
                await _getAuditLogsFunctionality.SendGetRequestGetAuditLogsAsync(_requestHeader!,
                    jResponseDisputesResponse.Disputes[0].Id, "0", "1000");
            Assert.That(_getAuditLogsFunctionality.ValidateAuditLogStatus(jResponseDisputesAuditLog,
                new List<string> { "Status=NEW" }));
            Assert.That(_getAuditLogsFunctionality.ValidateAuditLogsActions(jResponseDisputesAuditLog,
                new List<string> { "Comments:UpsertComment", "Webhook:BlueSnapMOR" }));
            Console.WriteLine("TestValidateBlueSnapMorDisputesV2OpenDisputeAndAddCommentInternalCommentTrue is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidateBlueSnapMorDisputesV2OpenDisputeAndAddCommentInternalCommentTrue\n" +
                        exception + "\n");
        }
    }

    [Category("BlueSnapMorDisputesV2Tests")]
    [Test(Description =
         "TestValidateBlueSnapMorDisputesV2OpenDisputeAndAddCommentInternalCommentFalse with generated merchant"),
     CancelAfter(720 * 1000)]
    public async Task TestValidateBlueSnapMorDisputesV2OpenDisputeAndAddCommentInternalCommentFalse()
    {
        try
        {
            Console.WriteLine(
                "\nStarting TestValidateBlueSnapMorDisputesV2OpenDisputeAndAddCommentInternalCommentFalse");
            var (installmentPlan, captureId) = await InitPlan();


            await _bluesnapFunctionality
                .SendPostRequestBluesnapNotifications(Environment.GetEnvironmentVariable("NotificationUrl")!,
                    captureId!, "CHARGEBACK", "NEW", "Set-Liabilty-Merchant");

            var jResponseDisputesResponse =
                await _postDisputesFunctionality.SendPostRequestPostDisputesAsync(_requestHeader!,
                    installmentPlan.InstallmentPlanNumber, _merchantId, "Open");

            var jResponsePostComments = await _postCommentsDisputeIdFunctionality.SendPostCommentsDisputeIdRequestAsync(
                _requestHeader!, jResponseDisputesResponse.Disputes[0].Id,
                "Automation testing", "splitit.automation@splitit.com", null!, false);

            Assert.That(jResponsePostComments.IsSuccess);
            Assert.That(!jResponsePostComments.Comment.Internal);

            var jResponseChargeBack =
                await _getDisputesDisputesIdFunctionality.SendGetRequestGetDisputesDisputesIdAsync(
                    _requestHeader!, jResponseDisputesResponse.Disputes[0].Id, _merchantId.ToString());

            Assert.That(jResponseChargeBack.Dispute.Comments.Count > 0);

            var jResponseDisputesAuditLog =
                await _getAuditLogsFunctionality.SendGetRequestGetAuditLogsAsync(_requestHeader!,
                    jResponseDisputesResponse.Disputes[0].Id, "0", "1000");

            Assert.That(_getAuditLogsFunctionality.ValidateAuditLogStatus(jResponseDisputesAuditLog,
                new List<string> { "Status=NEW" }));
            Assert.That(_getAuditLogsFunctionality.ValidateAuditLogsActions(jResponseDisputesAuditLog,
                new List<string> { "Comments:UpsertComment", "Webhook:BlueSnapMOR" }));
            Console.WriteLine(
                "TestValidateBlueSnapMorDisputesV2OpenDisputeAndAddCommentInternalCommentFalse is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidateBlueSnapMorDisputesV2OpenDisputeAndAddCommentInternalCommentFalse\n" +
                        exception + "\n");
        }
    }

    [Category("BlueSnapMorDisputesV2Tests")]
    [Test(Description =
         "TestValidateBlueSnapMorDisputesV2OpenDisputeAndAddCommentInternalCommentFalseAddDeleteComment with generated merchant"),
     CancelAfter(720 * 1000)]
    public async Task TestValidateBlueSnapMorDisputesV2OpenDisputeAndAddCommentInternalCommentFalseAddDeleteComment()
    {
        try
        {
            Console.WriteLine(
                "\nStarting TestValidateBlueSnapMorDisputesV2OpenDisputeAndAddCommentInternalCommentFalseAddDeleteComment");
            var (installmentPlan, captureId) = await InitPlan();


            await _bluesnapFunctionality
                .SendPostRequestBluesnapNotifications(Environment.GetEnvironmentVariable("NotificationUrl")!,
                    captureId!, "CHARGEBACK", "NEW", "Set-Liabilty-Merchant");

            var jResponseDisputesResponse =
                await _postDisputesFunctionality.SendPostRequestPostDisputesAsync(_requestHeader!,
                    installmentPlan.InstallmentPlanNumber, _merchantId, "Open");

            var jResponsePostComments = await _postCommentsDisputeIdFunctionality.SendPostCommentsDisputeIdRequestAsync(
                _requestHeader!, jResponseDisputesResponse.Disputes[0].Id,
                "Automation testing", "splitit.automation@splitit.com", null!, false);

            Assert.That(jResponsePostComments.IsSuccess);
            Assert.That(!jResponsePostComments.Comment.Internal);

            var jResponseChargeBack =
                await _getDisputesDisputesIdFunctionality.SendGetRequestGetDisputesDisputesIdAsync(
                    _requestHeader!, jResponseDisputesResponse.Disputes[0].Id, _merchantId.ToString());
            Assert.That(jResponseChargeBack.Dispute.Comments.Count > 0);

            var jResponseDeleteComment =
                await _deleteCommentsDisputeIdCommentsIdFunctionality
                    .SendDeleteRequestDeleteCommentsDisputeIdCommentsIdAsync(
                        _requestHeader!, jResponseDisputesResponse.Disputes[0].Id,
                        jResponseChargeBack.Dispute.Comments[0].CommentId);
            Assert.That(jResponseDeleteComment.Equals(""));

            var jResponseChargeBackAfterDelete =
                await _getDisputesDisputesIdFunctionality.SendGetRequestGetDisputesDisputesIdAsync(
                    _requestHeader!, jResponseDisputesResponse.Disputes[0].Id, _merchantId.ToString());
            Assert.That(jResponseChargeBackAfterDelete.Dispute.Comments.Count == 0);

            var jResponseDisputesAuditLog =
                await _getAuditLogsFunctionality.SendGetRequestGetAuditLogsAsync(_requestHeader!,
                    jResponseDisputesResponse.Disputes[0].Id, "0", "1000");
            Assert.That(_getAuditLogsFunctionality.ValidateAuditLogStatus(jResponseDisputesAuditLog,
                new List<string> { "Status=NEW" }));
            Assert.That(_getAuditLogsFunctionality.ValidateAuditLogsActions(jResponseDisputesAuditLog,
                new List<string> { "Comments:DeleteComment", "Comments:UpsertComment", "Webhook:BlueSnapMOR" }));

            Console.WriteLine(
                "TestValidateBlueSnapMorDisputesV2OpenDisputeAndAddCommentInternalCommentFalseAddDeleteComment is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail(
                "Error in TestValidateBlueSnapMorDisputesV2OpenDisputeAndAddCommentInternalCommentFalseAddDeleteComment\n" +
                exception + "\n");
        }
    }

    [Category("BlueSnapMorDisputesV2Tests")]
    [Test(Description =
         "TestValidateBlueSnapMorMerchantPortalOpenDisputeAndAddCommentInternalCommentFalseSeeCommentFromDisputeV2 with generated merchant"),
     CancelAfter(720 * 1000)]
    public async Task
        TestValidateBlueSnapMorMerchantPortalOpenDisputeAndAddCommentInternalCommentFalseSeeCommentFromDisputeV2()
    {
        try
        {
            Console.WriteLine(
                "\nStarting TestValidateBlueSnapMorMerchantPortalOpenDisputeAndAddCommentInternalCommentFalseSeeCommentFromDisputeV2");
            var (installmentPlan, captureId) = await InitPlan();

            await _bluesnapFunctionality
                .SendPostRequestBluesnapNotifications(Environment.GetEnvironmentVariable("NotificationUrl")!,
                    captureId!, "CHARGEBACK", "NEW", "Set-Liabilty-Merchant");

            var jResponseDisputesResponse =
                await _postDisputesFunctionalityMerchantPortal.SendPostRequestPostDisputesAsync(_requestHeader!,
                    installmentPlan.InstallmentPlanNumber, _merchantId, "Open");

            var jResponsePostComments = await _postCommentsDisputeIdFunctionality.SendPostCommentsDisputeIdRequestAsync(
                _requestHeader!, jResponseDisputesResponse.Disputes[0].Id,
                "Automation testing", "splitit.automation@splitit.com", null!, false);
            Assert.That(jResponsePostComments.IsSuccess);
            Assert.That(!jResponsePostComments.Comment.Internal);

            var jResponseChargeBackMerchantPortal = await _getDisputesDisputesIdFunctionalityMerchantPortal
                .SendGetRequestGetDisputesDisputesIdAsync(
                    _requestHeader!, jResponseDisputesResponse.Disputes[0].Id, _merchantId.ToString());
            Assert.That(jResponseChargeBackMerchantPortal.Dispute.Comments.Count > 0);

            var jResponseChargeBackDisputesV2 =
                await _getDisputesDisputesIdFunctionality.SendGetRequestGetDisputesDisputesIdAsync(
                    _requestHeader!, jResponseDisputesResponse.Disputes[0].Id, _merchantId.ToString());
            Assert.That(jResponseChargeBackDisputesV2.Dispute.Comments.Count > 0);

            var jResponseDisputesAuditLog =
                await _getAuditLogsFunctionality.SendGetRequestGetAuditLogsAsync(_requestHeader!,
                    jResponseDisputesResponse.Disputes[0].Id, "0", "1000");
            Assert.That(_getAuditLogsFunctionality.ValidateAuditLogStatus(jResponseDisputesAuditLog,
                new List<string> { "Status=NEW" }));
            Assert.That(_getAuditLogsFunctionality.ValidateAuditLogsActions(jResponseDisputesAuditLog,
                new List<string> { "Comments:UpsertComment", "Webhook:BlueSnapMOR" }));

            Console.WriteLine(
                "TestValidateBlueSnapMorMerchantPortalOpenDisputeAndAddCommentInternalCommentFalseSeeCommentFromDisputeV2 is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail(
                "Error in TestValidateBlueSnapMorMerchantPortalOpenDisputeAndAddCommentInternalCommentFalseSeeCommentFromDisputeV2\n" +
                exception + "\n");
        }
    }

    [Category("BlueSnapMorDisputesV2Tests")]
    [Test(Description =
         "TestValidateBlueSnapMorMerchantPortalOpenDisputeAndAddCommentInternalCommentFalseSeeCommentFromDisputeV2DeleteCommentMerchantPortalSeeDeletedInDisputesV2 with generated merchant"),
     CancelAfter(720 * 1000)]
    public async Task
        TestValidateBlueSnapMorMerchantPortalOpenDisputeAndAddCommentInternalCommentFalseSeeCommentFromDisputeV2DeleteCommentMerchantPortalSeeDeletedInDisputesV2()
    {
        try
        {
            Console.WriteLine(
                "\nStarting TestValidateBlueSnapMorMerchantPortalOpenDisputeAndAddCommentInternalCommentFalseSeeCommentFromDisputeV2DeleteCommentMerchantPortalSeeDeletedInDisputesV2");
            var (installmentPlan, captureId) = await InitPlan();

            await _bluesnapFunctionality
                .SendPostRequestBluesnapNotifications(Environment.GetEnvironmentVariable("NotificationUrl")!,
                    captureId!, "CHARGEBACK", "NEW", "Set-Liabilty-Merchant");

            var jResponseDisputesResponse =
                await _postDisputesFunctionalityMerchantPortal.SendPostRequestPostDisputesAsync(_requestHeader!,
                    installmentPlan.InstallmentPlanNumber, _merchantId, "Open");

            var jResponsePostComments = await _postCommentsDisputeIdFunctionality.SendPostCommentsDisputeIdRequestAsync(
                _requestHeader!, jResponseDisputesResponse.Disputes[0].Id,
                "Automation testing", "splitit.automation@splitit.com", null!, false);
            Assert.That(jResponsePostComments.IsSuccess);
            Assert.That(!jResponsePostComments.Comment.Internal);

            var jResponseChargeBackMerchantPortal = await _getDisputesDisputesIdFunctionalityMerchantPortal
                .SendGetRequestGetDisputesDisputesIdAsync(
                    _requestHeader!, jResponseDisputesResponse.Disputes[0].Id, _merchantId.ToString());
            Assert.That(jResponseChargeBackMerchantPortal.Dispute.Comments.Count > 0);

            var jResponseChargeBackDisputesV2 =
                await _getDisputesDisputesIdFunctionality.SendGetRequestGetDisputesDisputesIdAsync(
                    _requestHeader!, jResponseDisputesResponse.Disputes[0].Id, _merchantId.ToString());
            Assert.That(jResponseChargeBackDisputesV2.Dispute.Comments.Count > 0);

            var jResponseDeleteComment =
                await _deleteDisputesIdDeleteCommentCommentIdFunctionality
                    .SendDeleteRequestDeleteDisputesIdDeleteCommentCommentIdAsync(
                        _requestHeader!, jResponseDisputesResponse.Disputes[0].Id,
                        jResponseChargeBackMerchantPortal.Dispute.Comments[0].CommentId, _merchantId);
            Assert.That(jResponseDeleteComment.Equals(""));

            var jResponseChargeBackAfterDelete = await _getDisputesDisputesIdFunctionalityMerchantPortal
                .SendGetRequestGetDisputesDisputesIdAsync(
                    _requestHeader!, jResponseDisputesResponse.Disputes[0].Id, _merchantId.ToString());
            Assert.That(jResponseChargeBackAfterDelete.Dispute.Comments.Count == 0);

            var jResponseDisputesV2AfterDelete =
                await _getDisputesDisputesIdFunctionality.SendGetRequestGetDisputesDisputesIdAsync(
                    _requestHeader!, jResponseDisputesResponse.Disputes[0].Id, _merchantId.ToString());
            Assert.That(jResponseDisputesV2AfterDelete.Dispute.Comments.Count == 0);

            var jResponseDisputesAuditLog =
                await _getAuditLogsFunctionality.SendGetRequestGetAuditLogsAsync(_requestHeader!,
                    jResponseDisputesResponse.Disputes[0].Id, "0", "1000");
            Assert.That(_getAuditLogsFunctionality.ValidateAuditLogStatus(jResponseDisputesAuditLog,
                new List<string> { "Status=NEW" }));
            Assert.That(_getAuditLogsFunctionality.ValidateAuditLogsActions(jResponseDisputesAuditLog,
                new List<string> { "Comments:DeleteComment", "Comments:UpsertComment", "Webhook:BlueSnapMOR" }));

            Console.WriteLine(
                "TestValidateBlueSnapMorMerchantPortalOpenDisputeAndAddCommentInternalCommentFalseSeeCommentFromDisputeV2DeleteCommentMerchantPortalSeeDeletedInDisputesV2 is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail(
                "Error in TestValidateBlueSnapMorMerchantPortalOpenDisputeAndAddCommentInternalCommentFalseSeeCommentFromDisputeV2DeleteCommentMerchantPortalSeeDeletedInDisputesV2\n" +
                exception + "\n");
        }
    }

    [Category("BlueSnapMorDisputesV2Tests")]
    [Test(Description =
         "TestValidateBlueSnapMorDisputesV2OpenDisputeAndUploadEvidenceFileInternalEvidenceFalseAndDownloadEvidenceAndValidateEvidenceFile with generated merchant"),
     CancelAfter(720 * 1000)]
    public async Task
        TestValidateBlueSnapMorDisputesV2OpenDisputeAndUploadEvidenceFileInternalEvidenceFalseAndDownloadEvidenceAndValidateEvidenceFile()
    {
        try
        {
            Console.WriteLine(
                "\nStarting TestValidateBlueSnapMorDisputesV2OpenDisputeAndUploadEvidenceFileInternalEvidenceFalseAndDownloadEvidenceAndValidateEvidenceFile");
            var (installmentPlan, captureId) = await InitPlan();

            await _bluesnapFunctionality
                .SendPostRequestBluesnapNotifications(Environment.GetEnvironmentVariable("NotificationUrl")!,
                    captureId!, "CHARGEBACK", "NEW", "Set-Liabilty-Merchant");
            var jResponseDisputesResponse =
                await _postDisputesFunctionality.SendPostRequestPostDisputesAsync(_requestHeader!,
                    installmentPlan.InstallmentPlanNumber, _merchantId, "Open");

            var filePath = _testsData.ReturnEvidenceFileLocation("dummy.pdf");
            var responseUploadFile =
                await _postUploadFunctionality.SendPostRequestPostUploadAsync(_requestHeader!,
                    jResponseDisputesResponse.Disputes[0].Id, false, filePath, "dummy", "dummy.pdf");
            Assert.That(responseUploadFile, Is.Not.Null);

            var jResponseEvidences =
                await _getEvidencesDisputeIdFunctionality.SendGetRequestGetEvidencesDisputeIdAsync(
                    _requestHeader!, jResponseDisputesResponse.Disputes[0].Id);
            var filePathDownload = _testsData.ReturnEvidenceFileLocation("dummyDownloaded.pdf");
            Assert.That(await _postDownloadDisputeIdEvidenceIdFunctionality
                .SendPostRequestPostDownloadDisputeIdEvidenceIdAsync(
                    _requestHeader!, jResponseDisputesResponse.Disputes[0].Id,
                    jResponseEvidences.Evidences[0].EvidenceId, filePathDownload));

            Assert.That(await _compare2Files.Compering2Files(filePath, filePathDownload));

            var jResponseDisputesAuditLog =
                await _getAuditLogsFunctionality.SendGetRequestGetAuditLogsAsync(_requestHeader!,
                    jResponseDisputesResponse.Disputes[0].Id, "0", "1000");

            Assert.That(_getAuditLogsFunctionality.ValidateAuditLogStatus(jResponseDisputesAuditLog,
                new List<string> { "Status=NEW" }));
            Assert.That(_getAuditLogsFunctionality.ValidateAuditLogsActions(jResponseDisputesAuditLog,
                new List<string> { "Evidence:Upload", "Webhook:BlueSnapMOR" }));
            File.Delete(filePathDownload);

            Console.WriteLine(
                "TestValidateBlueSnapMorDisputesV2OpenDisputeAndUploadEvidenceFileInternalEvidenceFalseAndDownloadEvidenceAndValidateEvidenceFile is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail(
                "Error in TestValidateBlueSnapMorDisputesV2OpenDisputeAndUploadEvidenceFileInternalEvidenceFalseAndDownloadEvidenceAndValidateEvidenceFile\n" +
                exception + "\n");
        }
    }

    [Category("BlueSnapMorDisputesV2Tests")]
    [Test(Description =
         "TestValidateBlueSnapMorDisputesV2OpenDisputeAndUploadEvidenceFileInternalEvidenceFalseAndDeleteEvidence with generated merchant"),
     CancelAfter(720 * 1000)]
    public async Task
        TestValidateBlueSnapMorDisputesV2OpenDisputeAndUploadEvidenceFileInternalEvidenceFalseAndDeleteEvidence()
    {
        try
        {
            Console.WriteLine(
                "\nStarting TestValidateBlueSnapMorDisputesV2OpenDisputeAndUploadEvidenceFileInternalEvidenceFalseAndDeleteEvidence");
            var (installmentPlan, captureId) = await InitPlan();

            await _bluesnapFunctionality
                .SendPostRequestBluesnapNotifications(Environment.GetEnvironmentVariable("NotificationUrl")!,
                    captureId!, "CHARGEBACK", "NEW", "Set-Liabilty-Merchant");
            var jResponseDisputesResponse =
                await _postDisputesFunctionality.SendPostRequestPostDisputesAsync(_requestHeader!,
                    installmentPlan.InstallmentPlanNumber, _merchantId, "Open");
            var filePath = _testsData.ReturnEvidenceFileLocation("dummy.pdf");
            var responseUploadFile =
                await _postUploadFunctionality.SendPostRequestPostUploadAsync(_requestHeader!,
                    jResponseDisputesResponse.Disputes[0].Id, false, filePath, "dummy", "dummy.pdf");
            Assert.That(responseUploadFile, Is.Not.Null);

            var jResponseEvidences =
                await _getEvidencesDisputeIdFunctionality.SendGetRequestGetEvidencesDisputeIdAsync(
                    _requestHeader!, jResponseDisputesResponse.Disputes[0].Id);

            Assert.That(1, Is.EqualTo(jResponseEvidences.Evidences.Count));
            var jResponseDelete = await _deleteEvidencesFunctionality.SendDeleteRequestDeleteEvidencesAsync(
                _requestHeader!, jResponseDisputesResponse.Disputes[0].Id, jResponseEvidences.Evidences[0].EvidenceId);
            Assert.That(0, Is.EqualTo(jResponseDelete.Dispute.Evidences.Count));

            var jResponseDisputesAuditLog =
                await _getAuditLogsFunctionality.SendGetRequestGetAuditLogsAsync(_requestHeader!,
                    jResponseDisputesResponse.Disputes[0].Id, "0", "1000");
            Assert.That(_getAuditLogsFunctionality.ValidateAuditLogStatus(jResponseDisputesAuditLog,
                new List<string> { "Status=NEW" }));
            Assert.That(_getAuditLogsFunctionality.ValidateAuditLogsActions(jResponseDisputesAuditLog,
                new List<string> { "Evidence:DeleteBulk", "Evidence:Upload", "Webhook:BlueSnapMOR" }));

            Console.WriteLine(
                "TestValidateBlueSnapMorDisputesV2OpenDisputeAndUploadEvidenceFileInternalEvidenceFalseAndDeleteEvidence is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail(
                "Error in TestValidateBlueSnapMorDisputesV2OpenDisputeAndUploadEvidenceFileInternalEvidenceFalseAndDeleteEvidence\n" +
                exception + "\n");
        }
    }

    [Category("BlueSnapMorDisputesV2Tests")]
    [Test(Description =
         "TestValidateBlueSnapMorDisputesV2OpenDisputeAndUploadEvidenceFileInternalEvidenceTrueEvidenceDoesNotShownInMerchantPortalApi with generated merchant"),
     CancelAfter(720 * 1000)]
    public async Task
        TestValidateBlueSnapMorDisputesV2OpenDisputeAndUploadEvidenceFileInternalEvidenceTrueEvidenceDoesNotShownInMerchantPortalApi()
    {
        try
        {
            Console.WriteLine(
                "\nStarting TestValidateBlueSnapMorDisputesV2OpenDisputeAndUploadEvidenceFileInternalEvidenceTrueEvidenceDoesNotShownInMerchantPortalApi");
            var (installmentPlan, captureId) = await InitPlan();

            await _bluesnapFunctionality
                .SendPostRequestBluesnapNotifications(Environment.GetEnvironmentVariable("NotificationUrl")!,
                    captureId!, "CHARGEBACK", "NEW", "Set-Liabilty-Merchant");

            var jResponseDisputesResponse =
                await _postDisputesFunctionality.SendPostRequestPostDisputesAsync(_requestHeader!,
                    installmentPlan.InstallmentPlanNumber, _merchantId, "Open");
            var filePath = _testsData.ReturnEvidenceFileLocation("dummy.pdf");
            var responseUploadFile =
                await _postUploadFunctionality.SendPostRequestPostUploadAsync(_requestHeader!,
                    jResponseDisputesResponse.Disputes[0].Id, true, filePath, "dummy", "dummy.pdf");
            Assert.That(responseUploadFile, Is.Not.Null);

            var jResponseEvidences =
                await _getEvidencesDisputeIdFunctionality.SendGetRequestGetEvidencesDisputeIdAsync(
                    _requestHeader!, jResponseDisputesResponse.Disputes[0].Id);
            Assert.That(0, Is.EqualTo(jResponseEvidences.Evidences.Count));

            var jResponseChargeBackMerchantPortal = await _getDisputesDisputesIdFunctionalityMerchantPortal
                .SendGetRequestGetDisputesDisputesIdAsync(
                    _requestHeader!, jResponseDisputesResponse.Disputes[0].Id, _merchantId.ToString());
            Assert.That(jResponseChargeBackMerchantPortal.Dispute.Evidences.Count, Is.EqualTo(0));
            var jResponseDisputesAuditLog =
                await _getAuditLogsFunctionality.SendGetRequestGetAuditLogsAsync(_requestHeader!,
                    jResponseDisputesResponse.Disputes[0].Id, "0", "1000");
            Assert.That(_getAuditLogsFunctionality.ValidateAuditLogStatus(jResponseDisputesAuditLog,
                new List<string> { "Status=NEW" }));
            Assert.That(_getAuditLogsFunctionality.ValidateAuditLogsActions(jResponseDisputesAuditLog,
                new List<string> { "Evidence:Upload", "Webhook:BlueSnapMOR" }));

            Console.WriteLine(
                "TestValidateBlueSnapMorDisputesV2OpenDisputeAndUploadEvidenceFileInternalEvidenceTrueEvidenceDoesNotShownInMerchantPortalApi is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail(
                "Error in TestValidateBlueSnapMorDisputesV2OpenDisputeAndUploadEvidenceFileInternalEvidenceTrueEvidenceDoesNotShownInMerchantPortalApi\n" +
                exception + "\n");
        }
    }

    [Category("BlueSnapMorDisputesV2Tests")]
    [Test(Description =
         "TestValidateBlueSnapMorMerchantPortalApiOpenDisputeAndUploadEvidenceFileInternalEvidenceFalse with generated merchant"),
     CancelAfter(720 * 1000)]
    public async Task TestValidateBlueSnapMorMerchantPortalApiOpenDisputeAndUploadEvidenceFileInternalEvidenceFalse()
    {
        try
        {
            Console.WriteLine(
                "\nStarting TestValidateBlueSnapMorMerchantPortalApiOpenDisputeAndUploadEvidenceFileInternalEvidenceFalse");
            var (installmentPlan, captureId) = await InitPlan();

            await _bluesnapFunctionality
                .SendPostRequestBluesnapNotifications(Environment.GetEnvironmentVariable("NotificationUrl")!,
                    captureId!, "CHARGEBACK", "NEW", "Set-Liabilty-Merchant");
            var jResponseDisputesResponse =
                await _postDisputesFunctionality.SendPostRequestPostDisputesAsync(_requestHeader!,
                    installmentPlan.InstallmentPlanNumber, _merchantId, "Open");
            var filePath = _testsData.ReturnEvidenceFileLocation("dummy.pdf");
            var responseUploadFile =
                await _postUploadEvidenceFunctionality.SendPostRequestPostUploadAsync(_requestHeader!,
                    jResponseDisputesResponse.Disputes[0].Id,
                    false, filePath, "dummy", "dummy.pdf", _merchantId);
            Assert.That(responseUploadFile, Is.Not.Null);
            var jResponseDisputesAuditLog =
                await _getAuditLogsFunctionality.SendGetRequestGetAuditLogsAsync(_requestHeader!,
                    jResponseDisputesResponse.Disputes[0].Id, "0", "1000");
            Assert.That(_getAuditLogsFunctionality.ValidateAuditLogStatus(jResponseDisputesAuditLog,
                new List<string> { "Status=NEW" }));
            Assert.That(_getAuditLogsFunctionality.ValidateAuditLogsActions(jResponseDisputesAuditLog,
                new List<string> { "Evidence:Upload", "Webhook:BlueSnapMOR" }));
            Console.WriteLine(
                "TestValidateBlueSnapMorMerchantPortalApiOpenDisputeAndUploadEvidenceFileInternalEvidenceFalse is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail(
                "Error in TestValidateBlueSnapMorMerchantPortalApiOpenDisputeAndUploadEvidenceFileInternalEvidenceFalse\n" +
                exception + "\n");
        }
    }

    [Category("BlueSnapMorDisputesV2Tests")]
    [Test(Description =
         "TestValidateBlueSnapMorMerchantPortalApiOpenDisputeAndUploadEvidenceFileInternalEvidenceFalseDownloadFileAndCompareIt with generated merchant"),
     CancelAfter(720 * 1000)]
    public async Task
        TestValidateBlueSnapMorMerchantPortalApiOpenDisputeAndUploadEvidenceFileInternalEvidenceFalseDownloadFileAndCompareIt()
    {
        try
        {
            Console.WriteLine(
                "\nStarting TestValidateBlueSnapMorMerchantPortalApiOpenDisputeAndUploadEvidenceFileInternalEvidenceFalseDownloadFileAndCompareIt");
            var (installmentPlan, captureId) = await InitPlan();

            await _bluesnapFunctionality
                .SendPostRequestBluesnapNotifications(Environment.GetEnvironmentVariable("NotificationUrl")!,
                    captureId!, "CHARGEBACK", "NEW", "Set-Liabilty-Merchant");

            var jResponseDisputesResponse =
                await _postDisputesFunctionality.SendPostRequestPostDisputesAsync(_requestHeader!,
                    installmentPlan.InstallmentPlanNumber, _merchantId, "Open");
            var filePath = _testsData.ReturnEvidenceFileLocation("dummy.pdf");
            var responseUploadFile =
                await _postUploadEvidenceFunctionality.SendPostRequestPostUploadAsync(_requestHeader!,
                    jResponseDisputesResponse.Disputes[0].Id,
                    false, filePath, "dummy", "dummy.pdf", _merchantId);
            Assert.That(responseUploadFile, Is.Not.Null);
            var jResponseChargeBackMerchantPortal = await _getDisputesDisputesIdFunctionalityMerchantPortal
                .SendGetRequestGetDisputesDisputesIdAsync(
                    _requestHeader!, jResponseDisputesResponse.Disputes[0].Id, _merchantId.ToString());
            var filePathDownload = _testsData.ReturnEvidenceFileLocation("dummyDownloaded.pdf");
            Assert.That(await _postDisputesIdDownloadEvidenceEvidenceIdFunctionality
                .SendPostRequestPostDisputesIdDownloadEvidenceEvidenceIdAsync(
                    _requestHeader!, jResponseDisputesResponse.Disputes[0].Id,
                    jResponseChargeBackMerchantPortal.Dispute.Evidences[0].EvidenceId, filePathDownload, _merchantId));
            Assert.That(await _compare2Files.Compering2Files(filePath, filePathDownload));
            File.Delete(filePathDownload);
            var jResponseDisputesAuditLog =
                await _getAuditLogsFunctionality.SendGetRequestGetAuditLogsAsync(_requestHeader!,
                    jResponseDisputesResponse.Disputes[0].Id, "0", "1000");
            Assert.That(_getAuditLogsFunctionality.ValidateAuditLogStatus(jResponseDisputesAuditLog,
                new List<string> { "Status=NEW" }));
            Assert.That(_getAuditLogsFunctionality.ValidateAuditLogsActions(jResponseDisputesAuditLog,
                new List<string> { "Evidence:Upload", "Webhook:BlueSnapMOR" }));
            Console.WriteLine(
                "TestValidateBlueSnapMorMerchantPortalApiOpenDisputeAndUploadEvidenceFileInternalEvidenceFalseDownloadFileAndCompareIt is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail(
                "Error in TestValidateBlueSnapMorMerchantPortalApiOpenDisputeAndUploadEvidenceFileInternalEvidenceFalseDownloadFileAndCompareIt\n" +
                exception + "\n");
        }
    }

    [Category("BlueSnapMorDisputesV2Tests")]
    [Test(Description =
         "TestValidateBlueSnapMorMerchantPortalApiOpenDisputeAndUploadEvidenceFileInternalEvidenceFalseAndDeleteIt with generated merchant"),
     CancelAfter(720 * 1000)]
    public async Task
        TestValidateBlueSnapMorMerchantPortalApiOpenDisputeAndUploadEvidenceFileInternalEvidenceFalseAndDeleteIt()
    {
        try
        {
            Console.WriteLine(
                "\nStarting TestValidateBlueSnapMorMerchantPortalApiOpenDisputeAndUploadEvidenceFileInternalEvidenceFalseAndDeleteIt");
            var (installmentPlan, captureId) = await InitPlan();

            await _bluesnapFunctionality
                .SendPostRequestBluesnapNotifications(Environment.GetEnvironmentVariable("NotificationUrl")!,
                    captureId!, "CHARGEBACK", "NEW", "Set-Liabilty-Merchant");

            var jResponseDisputesResponse =
                await _postDisputesFunctionality.SendPostRequestPostDisputesAsync(_requestHeader!,
                    installmentPlan.InstallmentPlanNumber, _merchantId, "Open");
            var filePath = _testsData.ReturnEvidenceFileLocation("dummy.pdf");
            var responseUploadFile =
                await _postUploadEvidenceFunctionality.SendPostRequestPostUploadAsync(_requestHeader!,
                    jResponseDisputesResponse.Disputes[0].Id,
                    false, filePath, "dummy", "dummy.pdf", _merchantId);
            Assert.That(responseUploadFile, Is.Not.Null);
            var jResponseChargeBackMerchantPortal = await _getDisputesDisputesIdFunctionalityMerchantPortal
                .SendGetRequestGetDisputesDisputesIdAsync(
                    _requestHeader!, jResponseDisputesResponse.Disputes[0].Id, _merchantId.ToString());
            var jResponseDeleteEvidence =
                await _postDisputesIdDeleteEvidenceFunctionality.SendPostRequestPostDisputesIdDeleteEvidenceAsync(
                    _requestHeader!, jResponseDisputesResponse.Disputes[0].Id,
                    jResponseChargeBackMerchantPortal.Dispute.Evidences[0].EvidenceId, _merchantId.ToString());
            Assert.That(0, Is.EqualTo(jResponseDeleteEvidence.Dispute.Evidences.Count));
            var jResponseDisputesAuditLog =
                await _getAuditLogsFunctionality.SendGetRequestGetAuditLogsAsync(_requestHeader!,
                    jResponseDisputesResponse.Disputes[0].Id, "0", "1000");
            Assert.That(_getAuditLogsFunctionality.ValidateAuditLogStatus(jResponseDisputesAuditLog,
                new List<string> { "Status=NEW" }));
            Assert.That(_getAuditLogsFunctionality.ValidateAuditLogsActions(jResponseDisputesAuditLog,
                new List<string> { "Evidence:DeleteBulk", "Evidence:Upload", "Webhook:BlueSnapMOR" }));
            Console.WriteLine(
                "TestValidateBlueSnapMorMerchantPortalApiOpenDisputeAndUploadEvidenceFileInternalEvidenceFalseAndDeleteIt is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail(
                "Error in TestValidateBlueSnapMorMerchantPortalApiOpenDisputeAndUploadEvidenceFileInternalEvidenceFalseAndDeleteIt\n" +
                exception + "\n");
        }
    }

    [Category("BlueSnapMorDisputesV2Tests")]
    [Test(Description =
         "TestValidateBlueSnapMorMerchantPortalApiOpenDisputeAndUploadEvidenceFileInternalEvidenceFalseAndCanSeeEvidenceInDisputesV2 with generated merchant"),
     CancelAfter(720 * 1000)]
    public async Task
        TestValidateBlueSnapMorMerchantPortalApiOpenDisputeAndUploadEvidenceFileInternalEvidenceFalseAndCanSeeEvidenceInDisputesV2()
    {
        try
        {
            Console.WriteLine(
                "\nStarting TestValidateBlueSnapMorMerchantPortalApiOpenDisputeAndUploadEvidenceFileInternalEvidenceFalseAndCanSeeEvidenceInDisputesV2");
            var (installmentPlan, captureId) = await InitPlan();

            await _bluesnapFunctionality
                .SendPostRequestBluesnapNotifications(Environment.GetEnvironmentVariable("NotificationUrl")!,
                    captureId!, "CHARGEBACK", "NEW", "Set-Liabilty-Merchant");

            var jResponseDisputesResponse =
                await _postDisputesFunctionality.SendPostRequestPostDisputesAsync(_requestHeader!,
                    installmentPlan.InstallmentPlanNumber, _merchantId, "Open");

            var filePath = _testsData.ReturnEvidenceFileLocation("dummy.pdf");
            var responseUploadFile =
                await _postUploadEvidenceFunctionality.SendPostRequestPostUploadAsync(_requestHeader!,
                    jResponseDisputesResponse.Disputes[0].Id,
                    false, filePath, "dummy", "dummy.pdf", _merchantId);
            Assert.That(responseUploadFile, Is.Not.Null);

            var jResponseChargeBackMerchantPortal = await _getDisputesDisputesIdFunctionalityMerchantPortal
                .SendGetRequestGetDisputesDisputesIdAsync(
                    _requestHeader!, jResponseDisputesResponse.Disputes[0].Id, _merchantId.ToString());
            var jResponseEvidences =
                await _getEvidencesDisputeIdFunctionality.SendGetRequestGetEvidencesDisputeIdAsync(
                    _requestHeader!, jResponseChargeBackMerchantPortal.Dispute.Id);
            Assert.That(1, Is.EqualTo(jResponseEvidences.Evidences.Count));

            var jResponseDisputesAuditLog =
                await _getAuditLogsFunctionality.SendGetRequestGetAuditLogsAsync(_requestHeader!,
                    jResponseDisputesResponse.Disputes[0].Id, "0", "1000");
            Assert.That(_getAuditLogsFunctionality.ValidateAuditLogStatus(jResponseDisputesAuditLog,
                new List<string> { "Status=NEW" }));
            Assert.That(_getAuditLogsFunctionality.ValidateAuditLogsActions(jResponseDisputesAuditLog,
                new List<string> { "Evidence:Upload", "Webhook:BlueSnapMOR" }));

            Console.WriteLine(
                "TestValidateBlueSnapMorMerchantPortalApiOpenDisputeAndUploadEvidenceFileInternalEvidenceFalseAndCanSeeEvidenceInDisputesV2 is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail(
                "Error in TestValidateBlueSnapMorMerchantPortalApiOpenDisputeAndUploadEvidenceFileInternalEvidenceFalseAndCanSeeEvidenceInDisputesV2\n" +
                exception + "\n");
        }
    }

    private async Task<(ResponseV3.ResponseRoot installmentPlan, string? captureId)> InitPlan()
    {
        try
        {
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            var installmentPlan = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!,
                _requestHeader!,
                "Active", new Random().Next(2, 6), _terminalApiKey!,
                createPlanDefaultValues, shouldCancel: false);


            var captureId = await _getPgtl.ValidatePgtlKeyValueAsync(
                Environment.GetEnvironmentVariable("StoreProcedureUrl")!, _requestHeader!,
                installmentPlan.InstallmentPlanNumber, "CaptureId");
            Console.WriteLine($"CaptureID - {captureId}");

            return (installmentPlan, captureId);
        }
        catch (Exception e)
        {
            Console.WriteLine("Error in InitPlan" + e);
            throw;
        }
    }
}