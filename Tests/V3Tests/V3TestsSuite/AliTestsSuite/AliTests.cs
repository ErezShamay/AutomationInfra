using Allure.NUnit;
using Allure.NUnit.Attributes;
using NUnit.Framework;
using Splitit.Automation.NG.Backend.BaseActions;
using Splitit.Automation.NG.Backend.Services.AdminApi.InstallmentPlan.InstallmentPlanBaseObjects;
using Splitit.Automation.NG.Backend.Services.AdminApi.InstallmentPlan.InstallmentsPlanFunctionality;
using Splitit.Automation.NG.Backend.Services.V3.DefaultValues;
using Splitit.Automation.NG.Backend.Services.V3.InstallmentPlan.InstallmentPlanApiEndPoint;
using Splitit.Automation.NG.Backend.Services.V3.InstallmentPlan.InstallmentPlanBaseObjects;
using Splitit.Automation.NG.Backend.Tests.TestsSetups;
using Splitit.Automation.NG.SharedResources.BaseActions;

namespace Splitit.Automation.NG.Backend.Tests.V3Tests.V3TestsSuite.AliTestsSuite;

[TestFixture]
[AllureNUnit]
[AllureSuite("AliTests")]
[AllureDisplayIgnored]
[Parallelizable(ParallelScope.All)]
public class AliTests
{
    private RequestHeader? _requestHeader;
    private readonly InstallmentPlans _installmentPlans;
    private readonly InstallmentPlanNumberRefund _installmentPlanNumberRefund;
    private readonly InstallmentPlanNumberCancel _installmentPlanNumberCancel;
    private readonly InstallmentPlanNumberUpdateOrder _installmentPlanNumberUpdateOrder;
    private readonly PlanJobFutureInformation _planJobFutureInformation;
    private readonly DoTheChallenge _doTheChallenge;

    public AliTests()
    {
        Console.WriteLine("Staring AliTests Setup");
        _installmentPlans = new InstallmentPlans();
        _installmentPlanNumberUpdateOrder = new InstallmentPlanNumberUpdateOrder();
        _installmentPlanNumberRefund = new InstallmentPlanNumberRefund();
        _installmentPlanNumberCancel = new InstallmentPlanNumberCancel();
        _planJobFutureInformation = new PlanJobFutureInformation();
        _doTheChallenge = new DoTheChallenge();
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
            Environment.GetEnvironmentVariable("AliExpressPassword")!,
            Environment.GetEnvironmentVariable("AliTerminal")!,
            Environment.GetEnvironmentVariable("AliExpressClientId")!);
    }

    [Category("AliExpress")]
    [Test(Description = "TestValidateV3CreatePlan"), CancelAfter(80 * 1000)]
    public async Task TestValidateV3CreatePlan()
    {
        try
        {
            Console.WriteLine("\nStarting TestValidateV3CreatePlan");
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!, _requestHeader!,
                "Active", new Random().Next(2, 6), Environment.GetEnvironmentVariable("AliTerminal")!,
                createPlanDefaultValues);
            Console.WriteLine("TestValidateV3CreatePlan is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidateV3CreatePlan\n" + exception + "\n");
        }
    }

    [Category("AliExpress")]
    [Test(Description = "TestValidateV3CreatePlan"), CancelAfter(80 * 1000)]
    public async Task TestValidateV3CreatePlan3Ds()
    {
        try
        {
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "regular");
            Console.WriteLine("\nStarting TestValidateV3CreatePlan3Ds");
            var planCreateResponse = await _installmentPlans.CreatePlanAsync(
                Environment.GetEnvironmentVariable("ApiV3")!, _requestHeader!, "Active", 1,
                Environment.GetEnvironmentVariable("AliTerminal")!, createPlanDefaultValues);
            Assert.That(_installmentPlans.ValidateInstallmentPlanCreation(planCreateResponse, "Initialized"));
            await _doTheChallenge.DoTheChallengeCheckout3DsAsync(planCreateResponse.Authorization.ThreeDSRedirect.Url);
            var planCreateResponseSearch =
                await _installmentPlans.SendSearchInstallmentPlanByRefNumberAsync(_requestHeader!,
                    planCreateResponse.RefOrderNumber);
            Assert.That(_installmentPlans.ValidateRedirectionAfterChallenge(planCreateResponseSearch));
            var planCreateResponseGetByIpn =
                await _installmentPlans.GetInstallmentPlanByIpnAsync(_requestHeader!,
                    planCreateResponse.InstallmentPlanNumber);
            Assert.That(_installmentPlans.ValidateAuthorizationObjectAfter3DsChallenge(planCreateResponseGetByIpn));
            Console.WriteLine("TestValidateV3CreatePlan3Ds is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidateV3CreatePlan3Ds\n" + exception + "\n");
        }
    }

    [Category("AliExpress")]
    [Test(Description = "TestValidateV3FullRefundBeforeUpdateOrder"), CancelAfter(80 * 1000)]
    public async Task TestValidateV3FullRefundBeforeUpdateOrder()
    {
        try
        {
            var auditLogManager = new AuditLogController();
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            Console.WriteLine("\nStarting TestValidateV3FullRefundBeforeUpdateOrder");
            var planCreateResponse = await _installmentPlans.CreatePlanAsync(
                Environment.GetEnvironmentVariable("ApiV3")!,
                _requestHeader!, "PendingCapture", 1, Environment.GetEnvironmentVariable("AliTerminal")!,
                createPlanDefaultValues);
            var planCreateResponseRefund = await _installmentPlanNumberRefund.SendRefundRequestAsync(_requestHeader!,
                planCreateResponse.InstallmentPlanNumber, "FullRefund");
            Assert.That(
                _installmentPlanNumberRefund.ValidateRefund(planCreateResponseRefund,
                    planCreateResponse.InstallmentPlanNumber));
            var jAuditLogResponse =
                await auditLogManager.SendRetrieveAuditLogRequestAsync(_requestHeader!,
                    planCreateResponse.InstallmentPlanNumber);
            Assert.That(auditLogManager.ValidateAuditLogLogs(new[] { "Create", "RefundReceived" }, jAuditLogResponse!));
            Console.WriteLine("TestValidateV3FullRefundBeforeUpdateOrder is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidateV3FullRefundBeforeUpdateOrder\n" + exception + "\n");
        }
    }

    [Category("AliExpress")]
    [Test(Description = "TestValidateV3PartialRefundBeforeUpdateOrder"), CancelAfter(80 * 1000)]
    public async Task TestValidateV3PartialRefundBeforeUpdateOrder()
    {
        try
        {
            var auditLogManager = new AuditLogController();
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            Console.WriteLine("\nStarting TestValidateV3PartialRefundBeforeUpdateOrder");
            var planCreateResponse = await _installmentPlans.CreatePlanAsync(
                Environment.GetEnvironmentVariable("ApiV3")!,
                _requestHeader!, "PendingCapture", 1, Environment.GetEnvironmentVariable("AliTerminal")!,
                createPlanDefaultValues);
            var planCreateResponseRefund = await _installmentPlanNumberRefund.SendRefundRequestAsync(_requestHeader!,
                planCreateResponse.InstallmentPlanNumber, "PartialRefund");
            Assert.That(
                _installmentPlanNumberRefund.ValidateRefund(planCreateResponseRefund,
                    planCreateResponse.InstallmentPlanNumber));
            var jAuditLogResponse =
                await auditLogManager.SendRetrieveAuditLogRequestAsync(_requestHeader!,
                    planCreateResponse.InstallmentPlanNumber);
            Assert.That(auditLogManager.ValidateAuditLogLogs(new[] { "Create", "RefundReceived" }, jAuditLogResponse!));
            Console.WriteLine("TestValidateV3PartialRefundBeforeUpdateOrder is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidateV3PartialRefundBeforeUpdateOrder\n" + exception + "\n");
        }
    }

    [Category("AliExpress")]
    [Test(Description = "TestValidateV3PartialRefundAfterUpdateOrder"), CancelAfter(80 * 1000)]
    public async Task TestValidateV3PartialRefundAfterUpdateOrder()
    {
        try
        {
            var auditLogManager = new AuditLogController();
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            var identifier = new Identifier(null!);
            Console.WriteLine("\nStarting TestValidateV3PartialRefundAfterUpdateOrder");
            var planCreateResponse = await _installmentPlans.CreatePlanAsync(
                Environment.GetEnvironmentVariable("ApiV3")!, _requestHeader!, "PendingCapture", 1,
                Environment.GetEnvironmentVariable("AliTerminal")!, createPlanDefaultValues);
            var planCreateResponseUpdate =
                await _installmentPlanNumberUpdateOrder.SendUpdateRequestAsync(_requestHeader!,
                    planCreateResponse.InstallmentPlanNumber, "Delivered", false, identifier);
            Assert.That(
                _installmentPlanNumberUpdateOrder.ValidateUpdate(planCreateResponseUpdate!,
                    planCreateResponseUpdate!.InstallmentPlanNumber, "Delivered"));
            var planCreateResponseRefund = await _installmentPlanNumberRefund.SendRefundRequestAsync(_requestHeader!,
                planCreateResponse.InstallmentPlanNumber, "PartialRefund");
            Assert.That(
                _installmentPlanNumberRefund.ValidateRefund(planCreateResponseRefund,
                    planCreateResponse.InstallmentPlanNumber));
            var jAuditLogResponse =
                await auditLogManager.SendRetrieveAuditLogRequestAsync(_requestHeader!,
                    planCreateResponse.InstallmentPlanNumber);
            Assert.That(
                auditLogManager.ValidateAuditLogLogs(new[] { "Create", "Update", "RefundReceived" },
                    jAuditLogResponse!));
            Console.WriteLine("TestValidateV3PartialRefundAfterUpdateOrder is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidateV3PartialRefundAfterUpdateOrder\n" + exception + "\n");
        }
    }

    [Category("AliExpress")]
    [Test(Description = "TestValidateFailureOn3Ds 3DS Ali terminal"), CancelAfter(80 * 1000)]
    public async Task TestValidateFailureOn3Ds()
    {
        try
        {
            Console.WriteLine("\nStarting TestValidateFailureOn3Ds");
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "default");
            var planCreateResponse = await _installmentPlans.CreatePlanAsync(
                Environment.GetEnvironmentVariable("ApiV3")!, _requestHeader!, "Active", 1,
                Environment.GetEnvironmentVariable("AliTerminal")!, createPlanDefaultValues);
            Assert.That(_installmentPlans.ValidateInstallmentPlanCreation(planCreateResponse, "Initialized"));
            Assert.That(_installmentPlans.Validate3DsFailure(planCreateResponse));
            Console.WriteLine("TestValidateFailureOn3Ds is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidateFailureOn3Ds\n" + exception + "\n");
        }
    }

    [Category("AliExpress")]
    [Test(Description = "TestValidateNo3DSFailureOnAuthorization"), CancelAfter(80 * 1000)]
    public async Task TestValidateNo3DSFailureOnAuthorization()
    {
        try
        {
            Console.WriteLine("\nStarting TestValidateNo3DSFailureOnAuthorization");
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            createPlanDefaultValues.shopper.email = "";
            var planCreateResponse = await _installmentPlans.CreatePlanAsync(
                Environment.GetEnvironmentVariable("ApiV3")!, _requestHeader!, "PendingCapture",
                new Random().Next(2, 6), Environment.GetEnvironmentVariable("AliTerminal")!, createPlanDefaultValues,
                "yes");
            Assert.That(planCreateResponse.Error.Message.Equals("Email is missing"));
            Console.WriteLine("TestValidateNo3DSFailureOnAuthorization is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidateNo3DSFailureOnAuthorization\n" + exception + "\n");
        }
    }

    [Category("AliExpress")]
    [Test(Description = "TestValidateNo3DSFailureOnSplititValidation"), CancelAfter(80 * 1000)]
    public async Task TestValidateNo3DSFailureOnSplititValidation()
    {
        try
        {
            Console.WriteLine("\nStarting TestValidateNo3DSFailureOnSplititValidation");
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            createPlanDefaultValues.shopper.email = "";
            var planCreateResponse = await _installmentPlans.CreatePlanAsync(
                Environment.GetEnvironmentVariable("ApiV3")!, _requestHeader!, "PendingCapture",
                new Random().Next(2, 6), Environment.GetEnvironmentVariable("AliTerminal")!, createPlanDefaultValues,
                "yes");
            Assert.That(planCreateResponse.Error.Message.Equals("Email is missing"));
            var planCreateResponseVerify =
                await _installmentPlans.SendVerifyAuthorizationRequestAsync(_requestHeader!,
                    planCreateResponse.InstallmentPlanNumber);
            Assert.That(_installmentPlans.ValidateVerifyAuthorizationRequest(planCreateResponseVerify, false));
            Console.WriteLine("TestValidateNo3DSFailureOnSplititValidation is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidateNo3DSFailureOnSplititValidation\n" + exception + "\n");
        }
    }

    [Category("AliExpress")]
    [Test(Description = "TestValidateGetPlanWithOMsProcess"), CancelAfter(80 * 1000)]
    public async Task TestValidateGetPlanWithOMsProcess()
    {
        try
        {
            Console.WriteLine("\nStarting TestValidateGetPlanWithOMsProcess");
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            var identifier = new Identifier(createPlanDefaultValues.planData.extendedParams);
            var planCreateResponse = await _installmentPlans.CreatePlanAsync(
                Environment.GetEnvironmentVariable("ApiV3")!, _requestHeader!, "PendingCapture",
                new Random().Next(2, 6), Environment.GetEnvironmentVariable("AliTerminal")!, createPlanDefaultValues);
            var planCreateResponseUpdate =
                await _installmentPlanNumberUpdateOrder.SendUpdateRequestAsync(_requestHeader!,
                    planCreateResponse.InstallmentPlanNumber, "Delivered", false, identifier);
            Assert.That(
                _installmentPlanNumberUpdateOrder.ValidateUpdate(planCreateResponseUpdate!,
                    planCreateResponseUpdate!.InstallmentPlanNumber, "Delivered"));
            Console.WriteLine("TestValidateGetPlanWithOMsProcess is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidateGetPlanWithOMsProcess\n" + exception + "\n");
        }
    }

    [Category("AliExpress")]
    [Test(Description = "TestValidateGetPlanWithoutOMsProcess"), CancelAfter(80 * 1000)]
    public async Task TestValidateGetPlanWithoutOMsProcess()
    {
        try
        {
            Console.WriteLine("\nStarting TestValidateGetPlanWithoutOMsProcess");
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            createPlanDefaultValues.planData.refOrderNumber = "SIMULATE_DO_NOT_PROCESS";
            var planCreateResponse = await _installmentPlans.CreatePlanAsync(
                Environment.GetEnvironmentVariable("ApiV3")!, _requestHeader!, "PendingCapture", 1,
                Environment.GetEnvironmentVariable("AliTerminal")!, createPlanDefaultValues);
            var planCreateResponseExtendedParams =
                await _installmentPlans.SendExtendedParamsRequestAsync(_requestHeader!,
                    planCreateResponse.InstallmentPlanNumber);
            Assert.That(planCreateResponseExtendedParams.PlansList.Count == 0);
            Console.WriteLine("TestValidateGetPlanWithoutOMsProcess is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidateGetPlanWithoutOMsProcess\n" + exception + "\n");
        }
    }

    [Category("AliExpress")]
    [Test(Description = "TestValidateExemptionExist3DSTrue"), CancelAfter(80 * 1000)]
    public async Task TestValidateExemptionExist3DsTrue()
    {
        try
        {
            Console.WriteLine("\nStarting TestValidateExemptionExist3DSTrue");
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "yes");
            var identifier = new Identifier(createPlanDefaultValues.planData.extendedParams);
            var planCreateResponse = await _installmentPlans.CreatePlanAsync(
                Environment.GetEnvironmentVariable("ApiV3")!, _requestHeader!, "PendingCapture", 1,
                Environment.GetEnvironmentVariable("AliTerminal")!, createPlanDefaultValues);
            var planCreateResponseUpdate =
                await _installmentPlanNumberUpdateOrder.SendUpdateRequestAsync(_requestHeader!,
                    planCreateResponse.InstallmentPlanNumber, "Delivered", false, identifier);
            Assert.That(
                _installmentPlanNumberUpdateOrder.ValidateUpdate(planCreateResponseUpdate!,
                    planCreateResponseUpdate!.InstallmentPlanNumber, "Delivered"));
            var planCreateResponseGetByIpn =
                await _installmentPlans.GetInstallmentPlanByIpnAsync(_requestHeader!,
                    planCreateResponseUpdate.InstallmentPlanNumber);
            Assert.That(planCreateResponseGetByIpn.Status.Equals("Cleared") ||
                        planCreateResponseGetByIpn.Status.Equals("Active"));
            Console.WriteLine("TestValidateExemptionExist3DSTrue is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidateExemptionExist3DSTrue\n" + exception + "\n");
        }
    }

    [Category("AliExpress")]
    [Test(Description = "TestValidateExemptionExist3DSFalse"), CancelAfter(80 * 1000)]
    public async Task TestValidateExemptionExist3DsFalse()
    {
        try
        {
            Console.WriteLine("\nStarting TestValidateExemptionExist3DSFalse");
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!, _requestHeader!,
                "PendingCapture", 1, Environment.GetEnvironmentVariable("AliTerminal")!, createPlanDefaultValues);
            Console.WriteLine("TestValidateExemptionExist3DSFalse is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidateExemptionExist3DSFalse\n" + exception + "\n");
        }
    }

    [Category("AliExpress")]
    [Test(Description = "TestValidateExemptionDoesNotExist3DSTrue"), CancelAfter(80 * 1000)]
    public async Task TestValidateExemptionDoesNotExist3DsTrue()
    {
        try
        {
            Console.WriteLine("\nStarting TestValidateExemptionDoesNotExist3DSTrue");
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "yes");
            var identifier = new Identifier(createPlanDefaultValues.planData.extendedParams);
            var planCreateResponse = await _installmentPlans.CreatePlanAsync(
                Environment.GetEnvironmentVariable("ApiV3")!, _requestHeader!, "PendingCapture", 1,
                Environment.GetEnvironmentVariable("AliTerminal")!, createPlanDefaultValues);
            var planCreateResponseUpdate =
                await _installmentPlanNumberUpdateOrder.SendUpdateRequestAsync(_requestHeader!,
                    planCreateResponse.InstallmentPlanNumber, "Delivered", false, identifier);
            Assert.That(
                _installmentPlanNumberUpdateOrder.ValidateUpdate(planCreateResponseUpdate!,
                    planCreateResponseUpdate!.InstallmentPlanNumber, "Delivered"));
            var planCreateResponseGetPlanByIpn =
                await _installmentPlans.GetInstallmentPlanByIpnAsync(_requestHeader!,
                    planCreateResponse.InstallmentPlanNumber);
            Assert.That(
                planCreateResponseGetPlanByIpn.Status.Equals("Cleared") ||
                planCreateResponseGetPlanByIpn.Status.Equals("Active"));
            Console.WriteLine("TestValidateExemptionDoesNotExist3DSTrue is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidateExemptionDoesNotExist3DSTrue\n" + exception + "\n");
        }
    }

    [Category("AliExpress")]
    [Test(Description = "TestValidateCreatePlanWithEmailSpaceAtBeginning yes 3DS Ali terminal"), CancelAfter(80 * 1000)]
    public async Task TestValidateCreatePlanWithEmailSpaceAtBeginning()
    {
        try
        {
            Console.WriteLine("\nStarting TestValidateCreatePlanWithEmailSpaceAtBeginning");
            var createPlanDefaultValues = new CreatePlanDefaultValues
            {
                shopper =
                {
                    email = " splitit.automation@splitit.com"
                }
            };
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "default");
            var planCreateResponse = await _installmentPlans.CreatePlanAsync(
                Environment.GetEnvironmentVariable("ApiV3")!, _requestHeader!, "PendingCapture", 1,
                Environment.GetEnvironmentVariable("AliTerminal")!, createPlanDefaultValues);
            Assert.That(_installmentPlans.ValidateInstallmentPlanCreation(planCreateResponse, "Initialized"));
            await _doTheChallenge.DoTheChallengeCheckout3DsAsync(planCreateResponse.Authorization.ThreeDSRedirect.Url);
            var planCreateResponseSearchPlan =
                await _installmentPlans.SendSearchInstallmentPlanByRefNumberAsync(_requestHeader!,
                    planCreateResponse.RefOrderNumber);
            Assert.That(planCreateResponseSearchPlan.PlanList[0], Is.Not.Null);
            var planCreateResponseGetByIpn =
                await _installmentPlans.GetInstallmentPlanByIpnAsync(_requestHeader!,
                    planCreateResponse.InstallmentPlanNumber);
            Assert.That(_installmentPlans.ValidateAuthorizationObjectAfter3DsChallenge(planCreateResponseGetByIpn));
            Console.WriteLine("TestValidateCreatePlanWithEmailSpaceAtBeginning is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidateCreatePlanWithEmailSpaceAtBeginning\n" + exception + "\n");
        }
    }

    [Category("AliExpress")]
    [Test(Description = "TestValidateCreatePlanWithEmailSpaceAtTheEnd yes 3DS Ali terminal"), CancelAfter(80 * 1000)]
    public async Task TestValidateCreatePlanWithEmailSpaceAtTheEnd()
    {
        try
        {
            Console.WriteLine("\nStarting TestValidateCreatePlanWithEmailSpaceAtTheEnd");
            var createPlanDefaultValues = new CreatePlanDefaultValues
            {
                shopper =
                {
                    email = "splitit.automation@splitit.com "
                }
            };
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "default");
            var planCreateResponse = await _installmentPlans.CreatePlanAsync(
                Environment.GetEnvironmentVariable("ApiV3")!, _requestHeader!, "PendingCapture", 1,
                Environment.GetEnvironmentVariable("AliTerminal")!, createPlanDefaultValues);
            Assert.That(_installmentPlans.ValidateInstallmentPlanCreation(planCreateResponse, "Initialized"));
            await _doTheChallenge.DoTheChallengeCheckout3DsAsync(planCreateResponse.Authorization.ThreeDSRedirect.Url);
            var planCreateResponseSearchPlan =
                await _installmentPlans.SendSearchInstallmentPlanByRefNumberAsync(_requestHeader!,
                    planCreateResponse.RefOrderNumber);
            Assert.That(planCreateResponseSearchPlan.PlanList[0], Is.Not.Null);
            var planCreateResponseGetByIpn =
                await _installmentPlans.GetInstallmentPlanByIpnAsync(_requestHeader!,
                    planCreateResponse.InstallmentPlanNumber);
            Assert.That(_installmentPlans.ValidateAuthorizationObjectAfter3DsChallenge(planCreateResponseGetByIpn));
            Console.WriteLine("TestValidateCreatePlanWithEmailSpaceAtTheEnd is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidateCreatePlanWithEmailSpaceAtTheEnd\n" + exception + "\n");
        }
    }

    [Category("AliExpress")]
    [Test(Description =
         "TestValidateUpdatingPlanFromPendingCaptureToInProgress_PlanShouldBeUpdatedAndStatusShouldBeChanged yes 3DS Ali terminal"),
     CancelAfter(80 * 1000)]
    public async Task
        TestValidateUpdatingPlanFromPendingCaptureToInProgress_PlanShouldBeUpdatedAndStatusShouldBeChanged()
    {
        try
        {
            Console.WriteLine(
                "\nStarting TestValidateUpdatingPlanFromPendingCaptureToInProgress_PlanShouldBeUpdatedAndStatusShouldBeChanged");
            var createPlanDefaultValues = new CreatePlanDefaultValues
            {
                shopper =
                {
                    email = "splitit.automation@splitit.com "
                }
            };
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "default");
            var identifier = new Identifier(createPlanDefaultValues.planData.extendedParams);
            var planCreateResponse = await _installmentPlans.CreatePlanAsync(
                Environment.GetEnvironmentVariable("ApiV3")!, _requestHeader!, "PendingCapture", 1,
                Environment.GetEnvironmentVariable("AliTerminal")!, createPlanDefaultValues);
            Assert.That(_installmentPlans.ValidateInstallmentPlanCreation(planCreateResponse, "Initialized"));
            await _doTheChallenge.DoTheChallengeCheckout3DsAsync(planCreateResponse.Authorization.ThreeDSRedirect.Url);
            var planCreateResponseUpdate =
                await _installmentPlanNumberUpdateOrder.SendUpdateRequestAsync(_requestHeader!,
                    planCreateResponse.InstallmentPlanNumber, "Delivered", false, identifier);
            Assert.That(
                _installmentPlanNumberUpdateOrder.ValidateUpdate(planCreateResponseUpdate!,
                    planCreateResponseUpdate!.InstallmentPlanNumber, "Delivered"));
            var planCreateResponseSearchPlan =
                await _installmentPlans.SendSearchInstallmentPlanByRefNumberAsync(_requestHeader!,
                    planCreateResponseUpdate.RefOrderNumber);
            Assert.That(planCreateResponseSearchPlan.PlanList[0], Is.Not.Null);
            var planCreateResponseGetByIpn =
                await _installmentPlans.GetInstallmentPlanByIpnAsync(_requestHeader!,
                    planCreateResponse.InstallmentPlanNumber);
            if (!planCreateResponseGetByIpn.Status.Equals("Active") &&
                !planCreateResponseGetByIpn.Status.Equals("Cleared"))
            {
                Assert.Fail("Plan status is not in the right state");
            }

            Console.WriteLine(
                "TestValidateUpdatingPlanFromPendingCaptureToInProgress_PlanShouldBeUpdatedAndStatusShouldBeChanged is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail(
                "Error in TestValidateUpdatingPlanFromPendingCaptureToInProgress_PlanShouldBeUpdatedAndStatusShouldBeChanged\n" +
                exception + "\n");
        }
    }

    [Category("AliExpress")]
    [Test(Description = "TestValidateInitializedStatus"), CancelAfter(80 * 1000)]
    public async Task TestValidateInitializedStatus()
    {
        try
        {
            Console.WriteLine("\nStarting TestValidateInitializedStatus");
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "default");
            var planCreateResponse = await _installmentPlans.CreatePlanAsync(
                Environment.GetEnvironmentVariable("ApiV3")!, _requestHeader!, "PendingCapture", 1,
                Environment.GetEnvironmentVariable("AliTerminal")!, createPlanDefaultValues);
            Assert.That(_installmentPlans.ValidateInstallmentPlanCreation(planCreateResponse, "Initialized"));
            var planCreateResponseVerifyAuthorization =
                await _installmentPlans.SendVerifyAuthorizationRequestAsync(_requestHeader!,
                    planCreateResponse.InstallmentPlanNumber);
            Assert.That(planCreateResponseVerifyAuthorization.IsAuthorized.Equals(false));
            Console.WriteLine("TestValidateInitializedStatus is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidateInitializedStatus\n" + exception + "\n");
        }
    }

    [Category("AliExpress")]
    [Test(Description = "TestValidateCreatePlanWith3Installments"), CancelAfter(80 * 1000)]
    public async Task TestValidateCreatePlanWith3Installments()
    {
        try
        {
            Console.WriteLine("\nStarting TestValidateCreatePlanWith3Installments");
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "with_exemption");
            var planCreateResponse = await _installmentPlans.CreatePlanAsync(
                Environment.GetEnvironmentVariable("ApiV3")!, _requestHeader!, "PendingCapture",
                new Random().Next(2, 6), Environment.GetEnvironmentVariable("AliTerminal")!, createPlanDefaultValues);
            Assert.That(planCreateResponse.Authorization.Status.Equals("Succeeded"));
            var planCreateResponseVerifyAuthorization =
                await _installmentPlans.SendVerifyAuthorizationRequestAsync(_requestHeader!,
                    planCreateResponse.InstallmentPlanNumber);
            Assert.That(planCreateResponseVerifyAuthorization.IsAuthorized.Equals(true));
            Console.WriteLine("TestValidateCreatePlanWith3Installments is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidateCreatePlanWith3Installments\n" + exception + "\n");
        }
    }

    [Category("AliExpress")]
    [Test(Description = "TestValidateCreatePlanWith1Installments"), CancelAfter(80 * 1000)]
    public async Task TestValidateCreatePlanWith1Installments()
    {
        try
        {
            Console.WriteLine("\nStarting TestValidateCreatePlanWith1Installments");
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "with_exemption");
            var planCreateResponse = await _installmentPlans.CreatePlanAsync(
                Environment.GetEnvironmentVariable("ApiV3")!, _requestHeader!, "PendingCapture", 1,
                Environment.GetEnvironmentVariable("AliTerminal")!, createPlanDefaultValues);
            Assert.That(planCreateResponse.Authorization.Status.Equals("Succeeded"));
            var planCreateResponseVerifyAuthorization =
                await _installmentPlans.SendVerifyAuthorizationRequestAsync(_requestHeader!,
                    planCreateResponse.InstallmentPlanNumber);
            Assert.That(planCreateResponseVerifyAuthorization.IsAuthorized.Equals(true));
            Console.WriteLine("TestValidateCreatePlanWith1Installments is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidateCreatePlanWith1Installments\n" + exception + "\n");
        }
    }

    [Category("AliExpress")]
    [Test(Description = "TestValidateCreatePlanWithInsufficientFunds"), CancelAfter(80 * 1000)]
    public async Task TestValidateCreatePlanWithInsufficientFunds()
    {
        try
        {
            Console.WriteLine("\nStarting TestValidateCreatePlanWithInsufficientFunds");
            var createPlanDefaultValues = new CreatePlanDefaultValues
            {
                billingAddress =
                {
                    addressLine1 = "Create with Insufficient funds"
                }
            };
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "with_exemption");
            var planCreateResponse = await _installmentPlans.CreatePlanAsync(
                Environment.GetEnvironmentVariable("ApiV3")!,
                _requestHeader!, "PendingCapture", new Random().Next(2, 6),
                Environment.GetEnvironmentVariable("AliMockerTerminal")!, createPlanDefaultValues, "yes");
            Assert.That(planCreateResponse.Error.Message, Is.EqualTo("GtwyResultCCDataInsufficientFunds"));
            var planCreateResponseVerifyAuthorization =
                await _installmentPlans.SendVerifyAuthorizationRequestAsync(_requestHeader!,
                    planCreateResponse.InstallmentPlanNumber);
            Assert.That(planCreateResponseVerifyAuthorization.IsAuthorized, Is.False);
            Console.WriteLine("TestValidateCreatePlanWithInsufficientFunds is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidateCreatePlanWithInsufficientFunds\n" + exception + "\n");
        }
    }

    [Category("AliExpress")]
    [Test(Description = "TestValidateGetPlanByRefOrderNumber"), CancelAfter(80 * 1000)]
    public async Task TestValidateGetPlanByRefOrderNumber()
    {
        try
        {
            Console.WriteLine("\nStarting TestValidateGetPlanByRefOrderNumber");
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "with_exemption");
            var planCreateResponse = await _installmentPlans.CreatePlanAsync(
                Environment.GetEnvironmentVariable("ApiV3")!, _requestHeader!, "PendingCapture",
                new Random().Next(2, 6), Environment.GetEnvironmentVariable("AliTerminal")!, createPlanDefaultValues);
            var planCreateResponseSearchPlan =
                await _installmentPlans.SendSearchInstallmentPlanByRefNumberAsync(_requestHeader!,
                    planCreateResponse.RefOrderNumber);
            Assert.That(planCreateResponseSearchPlan.PlanList[0], Is.Not.Null);
            Assert.That(
                planCreateResponse.InstallmentPlanNumber.Equals(planCreateResponseSearchPlan.PlanList[0]
                    .InstallmentPlanNumber));
            Console.WriteLine("TestValidateGetPlanByRefOrderNumber is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidateGetPlanByRefOrderNumber\n" + exception + "\n");
        }
    }

    [Category("AliExpress")]
    [Test(Description = "TestValidateGetPlanByIPn"), CancelAfter(80 * 1000)]
    public async Task TestValidateGetPlanByIPn()
    {
        try
        {
            Console.WriteLine("\nStarting TestValidateGetPlanByIPn");
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "with_exemption");
            var planCreateResponse = await _installmentPlans.CreatePlanAsync(
                Environment.GetEnvironmentVariable("ApiV3")!, _requestHeader!, "PendingCapture",
                new Random().Next(2, 6), Environment.GetEnvironmentVariable("AliTerminal")!, createPlanDefaultValues);
            var planCreateResponseSearch =
                await _installmentPlans.GetInstallmentPlanByIpnAsync(_requestHeader!,
                    planCreateResponse.InstallmentPlanNumber);
            Assert.That(planCreateResponseSearch.InstallmentPlanNumber, Is.Not.Null);
            Console.WriteLine("TestValidateGetPlanByIPn is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidateGetPlanByIPn\n" + exception + "\n");
        }
    }

    [Category("AliExpress")]
    [Test(Description = "TestValidateUpdateOrderStatusShippingSetCaptureFalse"), CancelAfter(80 * 1000)]
    public async Task TestValidateUpdateOrderStatusShippingSetCaptureFalse()
    {
        try
        {
            Console.WriteLine("\nStarting TestValidateUpdateOrderStatusShippingSetCaptureFalse");
            var auditLogManager = new AuditLogController();
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "with_exemption");
            var identifier = new Identifier(null!);
            var planCreateResponse = await _installmentPlans.CreatePlanAsync(
                Environment.GetEnvironmentVariable("ApiV3")!, _requestHeader!, "PendingCapture",
                new Random().Next(2, 6), Environment.GetEnvironmentVariable("AliTerminal")!, createPlanDefaultValues);
            var planCreateResponseUpdate =
                await _installmentPlanNumberUpdateOrder.SendUpdateRequestAsync(_requestHeader!,
                    planCreateResponse.InstallmentPlanNumber, "Shipped", false, identifier);
            Assert.That(
                _installmentPlanNumberUpdateOrder.ValidateUpdate(planCreateResponseUpdate!,
                    planCreateResponseUpdate!.InstallmentPlanNumber, "Shipped"));
            var jAuditLogResponse =
                await auditLogManager.SendRetrieveAuditLogRequestAsync(_requestHeader!,
                    planCreateResponse.InstallmentPlanNumber);
            Assert.That(auditLogManager.ValidateAuditLogLogs(new[] { "Create", "Update" }, jAuditLogResponse!));
            Console.WriteLine("TestValidateUpdateOrderStatusShippingSetCaptureFalse is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidateUpdateOrderStatusShippingSetCaptureFalse\n" + exception + "\n");
        }
    }

    [Category("AliExpress")]
    [Test(Description = "TestValidateUpdateOrderStatusDeliveredSetCaptureFalse"),
     CancelAfter(80 * 1000)]
    public async Task TestValidateUpdateOrderStatusDeliveredSetCaptureFalse()
    {
        try
        {
            Console.WriteLine("\nStarting TestValidateUpdateOrderStatusDeliveredSetCaptureFalse");
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "with_exemption");
            var identifier = new Identifier(null!);
            var planCreateResponse = await _installmentPlans.CreatePlanAsync(
                Environment.GetEnvironmentVariable("ApiV3")!, _requestHeader!, "PendingCapture",
                new Random().Next(2, 6), Environment.GetEnvironmentVariable("AliTerminal")!, createPlanDefaultValues);
            var planCreateResponseUpdate =
                await _installmentPlanNumberUpdateOrder.SendUpdateRequestAsync(_requestHeader!,
                    planCreateResponse.InstallmentPlanNumber, "Delivered", false, identifier);
            Assert.That(
                _installmentPlanNumberUpdateOrder.ValidateUpdate(planCreateResponseUpdate!,
                    planCreateResponseUpdate!.InstallmentPlanNumber, "Delivered"));
            Console.WriteLine("TestValidateUpdateOrderStatusDeliveredSetCaptureFalse is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidateUpdateOrderStatusDeliveredSetCaptureFalse\n" + exception + "\n");
        }
    }

    [Category("AliExpress")]
    [Test(Description = "TestValidateUpdateOrderSetCaptureTrue"), CancelAfter(80 * 1000)]
    public async Task TestValidateUpdateOrderSetCaptureTrue()
    {
        try
        {
            Console.WriteLine("\nStarting TestValidateUpdateOrderSetCaptureTrue");
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "with_exemption");
            var identifier = new Identifier(null!);
            var planCreateResponse = await _installmentPlans.CreatePlanAsync(
                Environment.GetEnvironmentVariable("ApiV3")!, _requestHeader!, "PendingCapture",
                new Random().Next(2, 6), Environment.GetEnvironmentVariable("AliTerminal")!, createPlanDefaultValues);
            var planCreateResponseUpdate =
                await _installmentPlanNumberUpdateOrder.SendUpdateRequestAsync(_requestHeader!,
                    planCreateResponse.InstallmentPlanNumber, "Delivered", false, identifier);
            Assert.That(
                _installmentPlanNumberUpdateOrder.ValidateUpdate(planCreateResponseUpdate!,
                    planCreateResponseUpdate!.InstallmentPlanNumber, "Delivered"));
            var planCreateResponseSearch =
                await _installmentPlans.GetInstallmentPlanByIpnAsync(_requestHeader!,
                    planCreateResponse.InstallmentPlanNumber);
            Assert.That(planCreateResponseSearch.Status.Equals("Active"));
            Console.WriteLine("TestValidateUpdateOrderSetCaptureTrue is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidateUpdateOrderSetCaptureTrue\n" + exception + "\n");
        }
    }

    [Category("AliExpress")]
    [Test(Description = "TestValidateFullRefund"), CancelAfter(80 * 1000)]
    public async Task TestValidateFullRefund()
    {
        try
        {
            Console.WriteLine("\nStarting TestValidateFullRefund");
            var auditLogManager = new AuditLogController();
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "with_exemption");
            var identifier = new Identifier(null!);
            var planCreateResponse = await _installmentPlans.CreatePlanAsync(
                Environment.GetEnvironmentVariable("ApiV3")!, _requestHeader!, "PendingCapture",
                new Random().Next(2, 6), Environment.GetEnvironmentVariable("AliTerminal")!, createPlanDefaultValues);
            var planCreateResponseUpdate =
                await _installmentPlanNumberUpdateOrder.SendUpdateRequestAsync(_requestHeader!,
                    planCreateResponse.InstallmentPlanNumber, "Delivered", false, identifier);
            Assert.That(
                _installmentPlanNumberUpdateOrder.ValidateUpdate(planCreateResponseUpdate!,
                    planCreateResponseUpdate!.InstallmentPlanNumber, "Delivered"));
            var planCreateResponseRefund = await _installmentPlanNumberRefund.SendRefundRequestAsync(_requestHeader!,
                planCreateResponse.InstallmentPlanNumber, "FullRefund");
            Assert.That(
                _installmentPlanNumberRefund.ValidateRefund(planCreateResponseRefund,
                    planCreateResponse.InstallmentPlanNumber));
            var jAuditLogResponse =
                await auditLogManager.SendRetrieveAuditLogRequestAsync(_requestHeader!,
                    planCreateResponse.InstallmentPlanNumber);
            Assert.That(
                auditLogManager.ValidateAuditLogLogs(new[] { "Create", "Update", "RefundReceived" },
                    jAuditLogResponse!));
            Console.WriteLine("TestValidateFullRefund is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidateFullRefund\n" + exception + "\n");
        }
    }

    [Category("AliExpress")]
    [Test(Description = "TestValidatePartialRefund"), CancelAfter(80 * 1000)]
    public async Task TestValidatePartialRefund()
    {
        try
        {
            Console.WriteLine("\nStarting TestValidatePartialRefund");
            var auditLogManager = new AuditLogController();
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "with_exemption");
            var identifier = new Identifier(null!);
            var planCreateResponse = await _installmentPlans.CreatePlanAsync(
                Environment.GetEnvironmentVariable("ApiV3")!, _requestHeader!, "PendingCapture",
                new Random().Next(2, 6), Environment.GetEnvironmentVariable("AliTerminal")!, createPlanDefaultValues);
            var planCreateResponseUpdate =
                await _installmentPlanNumberUpdateOrder.SendUpdateRequestAsync(_requestHeader!,
                    planCreateResponse.InstallmentPlanNumber, "Delivered", false, identifier);
            Assert.That(
                _installmentPlanNumberUpdateOrder.ValidateUpdate(planCreateResponseUpdate!,
                    planCreateResponseUpdate!.InstallmentPlanNumber, "Delivered"));
            var planCreateResponseRefund = await _installmentPlanNumberRefund.SendRefundRequestAsync(_requestHeader!,
                planCreateResponse.InstallmentPlanNumber, "PartialRefund");
            Assert.That(
                _installmentPlanNumberRefund.ValidateRefund(planCreateResponseRefund,
                    planCreateResponse.InstallmentPlanNumber));
            var jAuditLogResponse =
                await auditLogManager.SendRetrieveAuditLogRequestAsync(_requestHeader!,
                    planCreateResponse.InstallmentPlanNumber);
            Assert.That(auditLogManager.ValidateAuditLogLogs(new[] { "Create", "Update", "RefundReceived" },
                jAuditLogResponse!));
            Console.WriteLine("TestValidatePartialRefund is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidatePartialRefund\n" + exception + "\n");
        }
    }

    [Category("AliExpress")]
    [Test(Description = "TestValidateExceededRefund"), CancelAfter(80 * 1000)]
    public async Task TestValidateExceededRefund()
    {
        try
        {
            Console.WriteLine("\nStarting TestValidateExceededRefund");
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "with_exemption");
            var identifier = new Identifier(null!);
            var planCreateResponse = await _installmentPlans.CreatePlanAsync(
                Environment.GetEnvironmentVariable("ApiV3")!, _requestHeader!, "PendingCapture",
                new Random().Next(2, 6), Environment.GetEnvironmentVariable("AliTerminal")!, createPlanDefaultValues);
            var planCreateResponseUpdate =
                await _installmentPlanNumberUpdateOrder.SendUpdateRequestAsync(_requestHeader!,
                    planCreateResponse.InstallmentPlanNumber, "Delivered", false, identifier);
            Assert.That(
                _installmentPlanNumberUpdateOrder.ValidateUpdate(planCreateResponseUpdate!,
                    planCreateResponseUpdate!.InstallmentPlanNumber, "Delivered"));
            var planCreateResponseRefund = await _installmentPlanNumberRefund.SendRefundRequestAsync(_requestHeader!,
                planCreateResponse.InstallmentPlanNumber,
                "ExceededAmount", null!, "yes");
            Assert.That(planCreateResponseRefund.Error.Code.Contains("400"));
            Console.WriteLine("TestValidateExceededRefund is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidateExceededRefund\n" + exception + "\n");
        }
    }

    [Category("AliExpress")]
    [Test(Description = "TestValidateCreatePlanWith3Ds"), CancelAfter(80 * 1000)]
    public async Task TestValidateCreatePlanWith3Ds()
    {
        try
        {
            Console.WriteLine("\nStarting TestValidateCreatePlanWith3Ds");
            var auditLogManager = new AuditLogController();
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "default");
            var planCreateResponse = await _installmentPlans.CreatePlanAsync(
                Environment.GetEnvironmentVariable("ApiV3")!, _requestHeader!, "PendingCapture",
                new Random().Next(2, 6), Environment.GetEnvironmentVariable("AliTerminal")!, createPlanDefaultValues);
            Assert.That(planCreateResponse.Status, Is.EqualTo("Initialized"));
            await _doTheChallenge.DoTheChallengeCheckout3DsAsync(planCreateResponse.Authorization.ThreeDSRedirect.Url);
            var planCreateResponseSearch =
                await _installmentPlans.GetInstallmentPlanByIpnAsync(_requestHeader!,
                    planCreateResponse.InstallmentPlanNumber);
            Assert.That(_installmentPlans.ValidateAuthorizationObjectAfter3DsChallenge(planCreateResponseSearch));
            var jAuditLogResponse =
                await auditLogManager.SendRetrieveAuditLogRequestAsync(_requestHeader!,
                    planCreateResponse.InstallmentPlanNumber);
            Assert.That(
                auditLogManager.ValidateAuditLogLogs(new[] { "Create", "Finalize 3D-Secure" }, jAuditLogResponse!));
            Console.WriteLine("TestValidateCreatePlanWith3Ds is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidateCreatePlanWith3Ds\n" + exception + "\n");
        }
    }

    [Category("AliExpress")]
    [Test(Description = "TestValidateCreatePlanWith3DsGetPlanShouldReturn3DsObject yes 3DS Ali terminal"),
     CancelAfter(80 * 1000)]
    public async Task TestValidateCreatePlanWith3DsGetPlanShouldReturn3DsObject()
    {
        try
        {
            Console.WriteLine("\nStarting TestValidateCreatePlanWith3DsGetPlanShouldReturn3DsObject");
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "default");
            var planCreateResponse = await _installmentPlans.CreatePlanAsync(
                Environment.GetEnvironmentVariable("ApiV3")!, _requestHeader!, "PendingCapture",
                new Random().Next(2, 6), Environment.GetEnvironmentVariable("AliTerminal")!, createPlanDefaultValues);
            Assert.That(planCreateResponse.Status, Is.EqualTo("Initialized"));
            Assert.That(_installmentPlans.Validate3DsFailure(planCreateResponse));
            Console.WriteLine("TestValidateCreatePlanWith3DsGetPlanShouldReturn3DsObject is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidateCreatePlanWith3DsGetPlanShouldReturn3DsObject\n" + exception + "\n");
        }
    }

    [Category("AliExpress")]
    [Test(Description =
         "TestValidateCreateActivePlanAndCheckTheAsyncCaptureFirstInstallmentShouldBeCaptured"),
     CancelAfter(80 * 1000)]
    public async Task TestValidateCreateActivePlanAndCheckTheAsyncCaptureFirstInstallmentShouldBeCaptured()
    {
        try
        {
            Console.WriteLine(
                "\nStarting TestValidateCreateActivePlanAndCheckTheAsyncCaptureFirstInstallmentShouldBeCaptured");
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "with_exemption");
            var planCreateResponse = await _installmentPlans.CreatePlanAsync(
                Environment.GetEnvironmentVariable("ApiV3")!, _requestHeader!, "Active", new Random().Next(2, 6),
                Environment.GetEnvironmentVariable("AliTerminal")!, createPlanDefaultValues);
            var planCreateResponsePlanJobFutureInfo =
                await _planJobFutureInformation.SendGetRequestForFutureJobsAsync(_requestHeader!,
                    planCreateResponse.InstallmentPlanNumber);
            Assert.That(
                _planJobFutureInformation.ValidateJobName(planCreateResponsePlanJobFutureInfo, "StartInstallments"));
            Console.WriteLine(
                "TestValidateCreateActivePlanAndCheckTheAsyncCaptureFirstInstallmentShouldBeCaptured is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail(
                "Error in TestValidateCreateActivePlanAndCheckTheAsyncCaptureFirstInstallmentShouldBeCaptured\n" +
                exception + "\n");
        }
    }

    [Category("AliExpress")]
    [Test(Description = "TestValidateActiveStatus"), CancelAfter(80 * 1000)]
    public async Task TestValidateActiveStatus()
    {
        try
        {
            Console.WriteLine("\nStarting TestValidateActiveStatus");
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "with_exemption");
            var planCreateResponse = await _installmentPlans.CreatePlanAsync(
                Environment.GetEnvironmentVariable("ApiV3")!, _requestHeader!, "Active",
                new Random().Next(2, 6), Environment.GetEnvironmentVariable("AliTerminal")!, createPlanDefaultValues);
            var planCreateResponsePlanJobFutureInfo =
                await _planJobFutureInformation.SendGetRequestForFutureJobsAsync(_requestHeader!,
                    planCreateResponse.InstallmentPlanNumber);
            Assert.That(planCreateResponsePlanJobFutureInfo, Is.Not.Null);
            Assert.That(_planJobFutureInformation.ValidateJobExecutionDate(planCreateResponsePlanJobFutureInfo,
                "ChargeInstallments"));
            var planCreateResponseVerify =
                await _installmentPlans.SendVerifyAuthorizationRequestAsync(_requestHeader!,
                    planCreateResponse.InstallmentPlanNumber);
            Assert.That(_installmentPlans.ValidateVerifyAuthorizationRequest(planCreateResponseVerify, true));
            Console.WriteLine("TestValidateActiveStatus is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidateActiveStatus\n" + exception + "\n");
        }
    }

    [Category("AliExpress")]
    [Test(Description = "TestValidatePrePaidCardOneInstallment"), CancelAfter(80 * 1000)]
    public async Task TestValidatePrePaidCardOneInstallment()
    {
        try
        {
            Console.WriteLine("\nStarting TestValidatePrePaidCardOneInstallment");
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "with_exemption");
            createPlanDefaultValues.paymentMethod.card.cardNumber = Environment.GetEnvironmentVariable("PrePaidCard")!;
            await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!, _requestHeader!,
                "Active", 1, Environment.GetEnvironmentVariable("AliTerminal")!, createPlanDefaultValues);
            Console.WriteLine("TestValidatePrePaidCardOneInstallment is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidatePrePaidCardOneInstallment\n" + exception + "\n");
        }
    }

    [Category("AliExpress")]
    [Test(Description = "TestValidatePrePaidCardMoreThenOneInstallment"), CancelAfter(80 * 1000)]
    public async Task TestValidatePrePaidCardMoreThenOneInstallment()
    {
        try
        {
            Console.WriteLine("\nStarting TestValidatePrePaidCardMoreThenOneInstallment");
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "with_exemption");
            createPlanDefaultValues.paymentMethod.card.cardNumber = Environment.GetEnvironmentVariable("PrePaidCard")!;
            var planCreateResponse = await _installmentPlans.CreatePlanAsync(
                Environment.GetEnvironmentVariable("ApiV3")!, _requestHeader!, "Active", new Random().Next(2, 6),
                Environment.GetEnvironmentVariable("AliTerminal")!, createPlanDefaultValues, "yes");
            Assert.That(planCreateResponse.Error.Code.Contains("400"));
            Console.WriteLine("TestValidatePrePaidCardMoreThenOneInstallment is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidatePrePaidCardMoreThenOneInstallment\n" + exception + "\n");
        }
    }

    [Category("AliExpress")]
    [Test(Description = "TestValidateDebitCardOneInstallment"), CancelAfter(80 * 1000)]
    public async Task TestValidateDebitCardOneInstallment()
    {
        try
        {
            Console.WriteLine("\nStarting TestValidateDebitCardOneInstallment");
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "with_exemption");
            createPlanDefaultValues.paymentMethod.card.cardNumber = Environment.GetEnvironmentVariable("DebitCard")!;
            await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!, _requestHeader!,
                "Active", 1, Environment.GetEnvironmentVariable("AliTerminal")!, createPlanDefaultValues);
            Console.WriteLine("TestValidateDebitCardOneInstallment is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidateDebitCardOneInstallment\n" + exception + "\n");
        }
    }

    [Category("AliExpress")]
    [Test(Description = "TestValidateDebitCardMoreThenOneInstallment"), CancelAfter(80 * 1000)]
    public async Task TestValidateDebitCardMoreThenOneInstallment()
    {
        try
        {
            Console.WriteLine("\nStarting TestValidateDebitCardMoreThenOneInstallment");
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "with_exemption");
            createPlanDefaultValues.paymentMethod.card.cardNumber = Environment.GetEnvironmentVariable("DebitCard")!;
            var planCreateResponse = await _installmentPlans.CreatePlanAsync(
                Environment.GetEnvironmentVariable("ApiV3")!, _requestHeader!, "Active", new Random().Next(2, 6),
                Environment.GetEnvironmentVariable("AliTerminal")!, createPlanDefaultValues, "yes");
            Assert.That(planCreateResponse.Error.Code.Contains("400"));
            Console.WriteLine("TestValidateDebitCardMoreThenOneInstallment is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidateDebitCardMoreThenOneInstallment\n" + exception + "\n");
        }
    }

    [Category("AliExpress")]
    [Test(Description = "TestValidateV3CancelPlan"), CancelAfter(80 * 1000)]
    public async Task TestValidateV3CancelPlan()
    {
        try
        {
            Console.WriteLine("\nStarting TestValidateV3CancelPlan");
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            var json = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!,
                _requestHeader!, "Active", new Random().Next(2, 6), Environment.GetEnvironmentVariable("AliTerminal")!,
                createPlanDefaultValues);
            var planCreateResponse =
                await _installmentPlanNumberCancel.SendCancelPlanRequestAsync(_requestHeader!,
                    json.InstallmentPlanNumber, 0, "NoRefunds");
            Assert.That(planCreateResponse.ResponseHeader.Succeeded);
            Assert.That(planCreateResponse.InstallmentPlan.InstallmentPlanStatus.Code, Is.EqualTo("Cleared"));
            Console.WriteLine("TestValidateV3CancelPlan is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidateV3CancelPlan\n" + exception + "\n");
        }
    }

    [Category("AliExpress")]
    [Test(Description = "TestValidateFirstInstallmentAmountFunctionality"), CancelAfter(80 * 1000)]
    public async Task TestValidateFirstInstallmentAmountFunctionality()
    {
        try
        {
            Console.WriteLine("\nStarting TestValidateFirstInstallmentAmountFunctionality");
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            createPlanDefaultValues.planData.FirstInstallmentAmount = 502;
            createPlanDefaultValues.planData.totalAmount = 1000;
            var json = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!,
                _requestHeader!, "Active", 2, Environment.GetEnvironmentVariable("AliTerminal")!,
                createPlanDefaultValues);
            Assert.That(_installmentPlans.ValidateFirstInstallmentAmount(json, 502));
            Console.WriteLine("TestValidateFirstInstallmentAmountFunctionality is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidateFirstInstallmentAmountFunctionality\n" + exception + "\n");
        }
    }

    [Category("AliExpress")]
    [Test(Description = "TestValidateFirstInstallmentAmountFunctionality"), CancelAfter(80 * 1000)]
    public async Task TestValidateFirstInstallmentAmountFunctionalityNegative()
    {
        try
        {
            Console.WriteLine("\nStarting TestValidateFirstInstallmentAmountFunctionality");
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            createPlanDefaultValues.planData.FirstInstallmentAmount = 490;
            createPlanDefaultValues.planData.totalAmount = 1000;
            var json = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!,
                _requestHeader!, "Active", 2, Environment.GetEnvironmentVariable("AliTerminal")!,
                createPlanDefaultValues);
            Assert.That(_installmentPlans.ValidateFirstInstallmentAmount(json, 490));
            Console.WriteLine("TestValidateFirstInstallmentAmountFunctionality is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidateFirstInstallmentAmountFunctionality\n" + exception + "\n");
        }
    }

    [Category("AliExpress")]
    [Test(Description = "TestValidateFirstInstallmentAmountFunctionalityNoValue"),
     CancelAfter(80 * 1000)]
    public async Task TestValidateFirstInstallmentAmountFunctionalityNoValue()
    {
        try
        {
            Console.WriteLine("\nStarting TestValidateFirstInstallmentAmountFunctionalityNoValue");
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            var json = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!,
                _requestHeader!, "Active", 2, Environment.GetEnvironmentVariable("AliTerminal")!,
                createPlanDefaultValues);
            Assert.That(_installmentPlans.ValidateAllInstallmentsAmountsEquals(json));
            Console.WriteLine("TestValidateFirstInstallmentAmountFunctionalityNoValue is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidateFirstInstallmentAmountFunctionalityNoValue\n" + exception + "\n");
        }
    }

    [Category("AliExpress")]
    [Test(Description = "TestValidateV3CancelPlan"), CancelAfter(80 * 1000)]
    public async Task TestValidateFirstInstallmentDateFunctionalityTomorrow()
    {
        try
        {
            Console.WriteLine("\nStarting TestValidateV3CancelPlan");
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            createPlanDefaultValues.planData.FirstInstallmentDate = DateTime.Today.AddDays(1).ToString("MM/dd/yyyy");
            var json = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!,
                _requestHeader!, "Active", new Random().Next(2, 6), Environment.GetEnvironmentVariable("AliTerminal")!,
                createPlanDefaultValues);
            Assert.That(
                json.Installments[0].ProcessDateTime.ToString("MM/dd/yyyy")
                    .Contains(DateTime.Today.AddDays(1).ToString("MM/dd/yyyy")));
            Console.WriteLine("TestValidateV3CancelPlan is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidateV3CancelPlan\n" + exception + "\n");
        }
    }

    [Category("AliExpress")]
    [Test(Description = "TestValidateV3CancelPlan"), CancelAfter(80 * 1000)]
    public async Task TestValidateFirstInstallmentDateFunctionalityNoValue()
    {
        try
        {
            Console.WriteLine("\nStarting TestValidateV3CancelPlan");
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            var json = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!,
                _requestHeader!, "Active", new Random().Next(2, 6), Environment.GetEnvironmentVariable("AliTerminal")!,
                createPlanDefaultValues);
            Assert.That(
                json.Installments[0].ProcessDateTime.ToString("MM/dd/yyyy").Equals(DateTime.Now.ToString("MM/dd/yyyy")));
            Console.WriteLine("TestValidateV3CancelPlan is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidateV3CancelPlan\n" + exception + "\n");
        }
    }

    [Category("AliExpress")]
    [Test(Description = "TestValidate3DsAndAuthFailureWhenComplete3DsIsDone Ali terminal"), CancelAfter(80 * 1000)]
    public async Task TestValidate3DsAndAuthFailureWhenComplete3DsIsDone()
    {
        try
        {
            Console.WriteLine("\nStarting TestValidate3DsAndAuthFailureWhenComplete3DsIsDone");
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "default");
            createPlanDefaultValues.billingAddress.addressLine1 = "Simulate failure MIT";
            var json = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!,
                _requestHeader!, "Active", new Random().Next(2, 6),
                Environment.GetEnvironmentVariable("AliTerminal")!, createPlanDefaultValues);
            Assert.That(_installmentPlans.ValidateInstallmentPlanCreation(json, "Initialized"));
            await _doTheChallenge.DoTheChallengeCheckout3DsAsync(json.Authorization.ThreeDSRedirect.Url);
            Console.WriteLine("TestValidate3DsAndAuthFailureWhenComplete3DsIsDone is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidate3DsAndAuthFailureWhenComplete3DsIsDone\n" + exception + "\n");
        }
    }
}