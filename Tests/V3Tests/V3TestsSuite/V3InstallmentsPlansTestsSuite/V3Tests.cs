using Allure.NUnit;
using Allure.NUnit.Attributes;
using NUnit.Framework;
using Splitit.Automation.NG.Backend.BaseActions;
using Splitit.Automation.NG.Backend.Services.AdminApi.InstallmentPlan.InstallmentPlanBaseObjects;
using Splitit.Automation.NG.Backend.Services.AdminApi.InstallmentPlan.InstallmentsPlanFunctionality;
using Splitit.Automation.NG.Backend.Services.AdminApi.Merchant.MerchantFunctionality;
using Splitit.Automation.NG.Backend.Services.V3.DefaultValues;
using Splitit.Automation.NG.Backend.Services.V3.InstallmentPlan.InstallmentPlanApiEndPoint;
using Splitit.Automation.NG.Backend.Services.V3.InstallmentPlan.InstallmentPlanBaseObjects;
using Splitit.Automation.NG.Backend.Tests.TestsSetups;
using Splitit.Automation.NG.SharedResources.BaseActions;

namespace Splitit.Automation.NG.Backend.Tests.V3Tests.V3TestsSuite.V3InstallmentsPlansTestsSuite;

[TestFixture]
[AllureNUnit]
[AllureSuite("V3Tests")]
[AllureDisplayIgnored]
[Parallelizable(ParallelScope.All)]
public class V3Tests
{
    private RequestHeader? _requestHeader;
    private readonly InstallmentPlans _installmentPlans;
    private readonly InstallmentPlanNumberRefund _installmentPlanNumberRefund;
    private readonly InstallmentPlanNumberCancel _installmentPlanNumberCancel;
    private readonly InstallmentPlanNumberUpdateOrder _installmentPlanNumberUpdateOrder;
    private readonly PlanJobFutureInformation _planJobFutureInformation;
    private readonly DoTheChallenge _doTheChallenge;
    private readonly CheckInstallmentsEligibility _checkInstallmentsEligibility;
    private readonly Settings _settings;
    private string? _terminalApiKey;
    private int _businessUnitId;

    public V3Tests()
    {
        Console.WriteLine("\nStaring V3 Setup");
        _installmentPlans = new InstallmentPlans();
        _installmentPlanNumberUpdateOrder = new InstallmentPlanNumberUpdateOrder();
        _installmentPlanNumberRefund = new InstallmentPlanNumberRefund();
        _installmentPlanNumberCancel = new InstallmentPlanNumberCancel();
        _planJobFutureInformation = new PlanJobFutureInformation();
        _doTheChallenge = new DoTheChallenge();
        _checkInstallmentsEligibility = new CheckInstallmentsEligibility();
        _settings = new Settings();
        Console.WriteLine("Done with V3 Setup");
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
        _terminalApiKey = Environment.GetEnvironmentVariable("V3TestsTerminal")!;
        _businessUnitId = int.Parse(Environment.GetEnvironmentVariable("V3TestsBU")!);
    }

    [Category("V3Tests")]
    [Test(Description = "TestValidateV3CreatePlan with generated merchant"), CancelAfter(120*1000)]
    public async Task TestValidateV3CreatePlan()
    {
        try
        {
            Assert.That(_terminalApiKey, Is.Not.EqualTo(""));
            Console.WriteLine("\nStarting TestValidateV3CreatePlan");
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            var planCreateResponse = await _installmentPlans.CreatePlanAsync(
                Environment.GetEnvironmentVariable("ApiV3")!, _requestHeader!,
                "Active", new Random().Next(4, 12), _terminalApiKey!,
                createPlanDefaultValues);
            var plan = await _installmentPlans.GetInstallmentPlanByIpnAsync(_requestHeader!, planCreateResponse.InstallmentPlanNumber);
            Assert.That(plan.Status.Equals("InProgress"));
            Console.WriteLine("TestValidateV3CreatePlan is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidateV3CreatePlan\n" + exception + "\n");
        }
    }

    [Category("V3Tests")]
    [Test(Description = "TestValidateV3CreatePlanWith3Ds with generated merchant"), CancelAfter(120*1000)]
    public async Task TestValidateV3CreatePlanWith3Ds()
    {
        try
        {
            Assert.That(_terminalApiKey, Is.Not.EqualTo(""));
            Console.WriteLine("\nStarting TestValidateV3CreatePlanWith3Ds");
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "yes");
            var json = await _installmentPlans.CreatePlanInitiateAsync(Environment.GetEnvironmentVariable("ApiV3")!, 
                _requestHeader!, "Active", new Random().Next(4, 12), _terminalApiKey!, createPlanDefaultValues);
            Assert.That(json.Status, Is.EqualTo("Initialized"));
            Assert.That(await _doTheChallenge.DoTheChallengeV4Async(json, createPlanDefaultValues,  _requestHeader!), Is.True);
            Console.WriteLine("TestValidateV3CreatePlanWith3Ds is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidateV3CreatePlanWith3Ds\n " + exception + "\n");
        }
    }

    [Category(" V3Tests")]
    [Test(Description = "TestValidateV3FullRefundBeforeUpdateOrder no 3DS with generated merchant"), CancelAfter(120*1000)]
    public async Task TestValidateV3FullRefundBeforeUpdateOrder()
    {
        try
        {
            Assert.That(_terminalApiKey, Is.Not.EqualTo(""));
            var auditLogManager = new AuditLogController();
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            Console.WriteLine("\nStarting TestValidateV3FullRefundBeforeUpdateOrder");
            var planCreateResponse = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!, 
                _requestHeader!, "PendingCapture", 1, _terminalApiKey!,
                createPlanDefaultValues);
            var jResponseRefund = await _installmentPlanNumberRefund.SendRefundRequestAsync( _requestHeader!,
                planCreateResponse.InstallmentPlanNumber, "FullRefund");
            Assert.That(_installmentPlanNumberRefund.ValidateRefund(jResponseRefund, planCreateResponse.InstallmentPlanNumber),
                Is.True);
            var jAuditLogResponse =
                await auditLogManager.SendRetrieveAuditLogRequestAsync( _requestHeader!, planCreateResponse.InstallmentPlanNumber);
            Assert.That(auditLogManager.ValidateAuditLogLogs(
                new[] { "Create", "RefundReceived" }, jAuditLogResponse!), Is.True);
            Console.WriteLine("TestValidateV3FullRefundBeforeUpdateOrder is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidateV3FullRefundBeforeUpdateOrder\n" + exception + "\n");
        }
    }

    [Category(" V3Tests")]
    [Test(Description = "TestValidateV3PartialRefundBeforeUpdateOrder no 3DS with generated merchant"), CancelAfter(120*1000)]
    public async Task TestValidateV3PartialRefundBeforeUpdateOrder()
    {
        try
        {
            Assert.That(_terminalApiKey, Is.Not.EqualTo(""));
            var auditLogManager = new AuditLogController();
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            Console.WriteLine("\nStarting TestValidateV3PartialRefundBeforeUpdateOrder");
            var planCreateResponse = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!, 
                _requestHeader!, "PendingCapture", 1, _terminalApiKey!,
                createPlanDefaultValues);
            var jResponseRefund = await _installmentPlanNumberRefund.SendRefundRequestAsync( _requestHeader!,
                planCreateResponse.InstallmentPlanNumber, "PartialRefund");
            Assert.That(_installmentPlanNumberRefund.ValidateRefund(jResponseRefund, planCreateResponse.InstallmentPlanNumber),
                Is.True);
            var jAuditLogResponse =
                await auditLogManager.SendRetrieveAuditLogRequestAsync( _requestHeader!, planCreateResponse.InstallmentPlanNumber);
            Assert.That(auditLogManager.ValidateAuditLogLogs(
                new[] { "Create", "RefundReceived" }, jAuditLogResponse!), Is.True);
            Console.WriteLine("TestValidateV3PartialRefundBeforeUpdateOrder is Done\n\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidateV3PartialRefundBeforeUpdateOrder\n" + exception + "\n");
        }
    }

    [Category(" V3Tests")]
    [Test(Description = "TestValidateV3PartialRefundAfterUpdateOrder no 3DS with generated merchant"), CancelAfter(120*1000)]
    public async Task TestValidateV3PartialRefundAfterUpdateOrder()
    {
        try
        {
            Assert.That(_terminalApiKey, Is.Not.EqualTo(""));
            var auditLogManager = new AuditLogController();
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            var identifier = new Identifier(null!);
            Console.WriteLine("\nStarting TestValidateV3PartialRefundAfterUpdateOrder");
            var planCreateResponse = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!, 
                _requestHeader!, "PendingCapture", 3, _terminalApiKey!,
                createPlanDefaultValues);
            var jResponseUpdate = await _installmentPlanNumberUpdateOrder.SendUpdateRequestAsync( _requestHeader!,
                planCreateResponse.InstallmentPlanNumber, "Delivered", false, identifier);
            Assert.That(_installmentPlanNumberUpdateOrder.ValidateUpdate(jResponseUpdate!,
                jResponseUpdate!.InstallmentPlanNumber, "Delivered"), Is.True);
            var jResponseRefund = await _installmentPlanNumberRefund.SendRefundRequestAsync( _requestHeader!,
                planCreateResponse.InstallmentPlanNumber, "PartialRefund");
            Assert.That(_installmentPlanNumberRefund.ValidateRefund(jResponseRefund, planCreateResponse.InstallmentPlanNumber),
                Is.True);
            var jAuditLogResponse =
                await auditLogManager.SendRetrieveAuditLogRequestAsync( _requestHeader!, planCreateResponse.InstallmentPlanNumber);
            Assert.That(auditLogManager.ValidateAuditLogLogs(
                new[] { "Create", "RefundReceived", "Update" }, jAuditLogResponse!), Is.True);
            Console.WriteLine("TestValidateV3PartialRefundAfterUpdateOrder is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidateV3PartialRefundAfterUpdateOrder\n" + exception + "\n");
        }
    }

    [Category(" V3Tests")]
    [Test(Description = "TestValidateFailureOn3Ds 3DS with generated merchant"), CancelAfter(120*1000)]
    public async Task TestValidateFailureOn3Ds()
    {
        try
        {
            Assert.That(_terminalApiKey, Is.Not.EqualTo(""));
            Console.WriteLine("\nStarting TestValidateFailureOn3Ds");
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "default");
            var planCreateResponse = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!, 
                _requestHeader!, "Active", 1, _terminalApiKey!,
                createPlanDefaultValues);
            Assert.That(_installmentPlans.ValidateInstallmentPlanCreation(planCreateResponse, "Initialized"), Is.True);
            Assert.That(_installmentPlans.Validate3DsFailure(planCreateResponse), Is.True);
            Console.WriteLine("TestValidateFailureOn3Ds is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidateFailureOn3Ds\n" + exception + "\n");
        }
    }

    [Category(" V3Tests")]
    [Test(Description = "TestValidateNo3DSFailureOnAuthorization no 3DS with generated merchant"), CancelAfter(120*1000)]
    public async Task TestValidateNo3DSFailureOnAuthorization()
    {
        try
        {
            Assert.That(_terminalApiKey, Is.Not.EqualTo(""));
            Console.WriteLine("\nStarting TestValidateNo3DSFailureOnAuthorization");
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            createPlanDefaultValues.shopper.email = "";
            var planCreateResponse = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!, 
                _requestHeader!, "PendingCapture", new Random().Next(4, 12), _terminalApiKey!,
                createPlanDefaultValues, "yes");
            Assert.That(planCreateResponse.Error.Message, Is.EqualTo("Email is missing"));
            Console.WriteLine("TestValidateNo3DSFailureOnAuthorization is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidateNo3DSFailureOnAuthorization\n" + exception + "\n");
        }
    }

    [Category(" V3Tests")]
    [Test(Description = "TestValidateNo3DSFailureOnSplititValidation no 3DS with generated merchant"), CancelAfter(120*1000)]
    public async Task TestValidateNo3DSFailureOnSplititValidation()
    {
        try
        {
            Assert.That(_terminalApiKey, Is.Not.EqualTo(""));
            Console.WriteLine("\nStarting TestValidateNo3DSFailureOnSplititValidation");
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            createPlanDefaultValues.shopper.email = "";
            var planCreateResponse = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!, 
                _requestHeader!, "PendingCapture", new Random().Next(4, 12), _terminalApiKey!,
                createPlanDefaultValues, "yes");
            Assert.That(planCreateResponse.Error.Message, Is.EqualTo("Email is missing"));
            var jResponseVerify =
                await _installmentPlans.SendVerifyAuthorizationRequestAsync( _requestHeader!,
                    planCreateResponse.InstallmentPlanNumber);
            Assert.That(_installmentPlans.ValidateVerifyAuthorizationRequest(jResponseVerify, false), Is.True);
            Console.WriteLine("TestValidateNo3DSFailureOnSplititValidation is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidateNo3DSFailureOnSplititValidation\n" + exception + "\n");
        }
    }

    [Category(" V3Tests")]
    [Test(Description = "TestValidateGetPlanWithOMsProcess no 3DS with generated merchant"), CancelAfter(120*1000)]
    public async Task TestValidateGetPlanWithOMsProcess()
    {
        try
        {
            Assert.That(_terminalApiKey, Is.Not.EqualTo(""));
            Console.WriteLine("\nStarting TestValidateGetPlanWithOMsProcess");
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            var identifier = new Identifier(createPlanDefaultValues.planData.extendedParams);
            var planCreateResponse = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!, 
                _requestHeader!, "PendingCapture", new Random().Next(4, 12), _terminalApiKey!,
                createPlanDefaultValues);
            var jResponseUpdate = await _installmentPlanNumberUpdateOrder.SendUpdateRequestAsync( _requestHeader!,
                planCreateResponse.InstallmentPlanNumber, "Delivered", false, identifier);
            Assert.That(_installmentPlanNumberUpdateOrder.ValidateUpdate(jResponseUpdate!,
                jResponseUpdate!.InstallmentPlanNumber, "Delivered"), Is.True);
            Console.WriteLine("TestValidateGetPlanWithOMsProcess is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidateGetPlanWithOMsProcess\n" + exception + "\n");
        }
    }

    [Category(" V3Tests")]
    [Test(Description = "TestValidateGetPlanWithoutOMsProcess no 3DS with generated merchant"), CancelAfter(120*1000)]
    public async Task TestValidateGetPlanWithoutOMsProcess()
    {
        try
        {
            Assert.That(_terminalApiKey, Is.Not.EqualTo(""));
            Console.WriteLine("\nStarting TestValidateGetPlanWithoutOMsProcess");
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            createPlanDefaultValues.planData.refOrderNumber = "SIMULATE_DO_NOT_PROCESS";
            var planCreateResponse = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!, 
                _requestHeader!, "PendingCapture", 1, _terminalApiKey!,
                createPlanDefaultValues);
            var jResponseExtendedParams =
                await _installmentPlans.SendExtendedParamsRequestAsync( _requestHeader!, planCreateResponse.InstallmentPlanNumber);
            Assert.That(jResponseExtendedParams.PlansList.Count == 0, Is.True);
            Console.WriteLine("TestValidateGetPlanWithoutOMsProcess is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidateGetPlanWithoutOMsProcess\n" + exception + "\n");
        }
    }

    [Category(" V3Tests")]
    [Test(Description = "TestValidateExemptionExist3DSFalse no 3DS with generated merchant"), CancelAfter(120*1000)]
    public async Task TestValidateExemptionExist3DsFalse()
    {
        try
        {
            Assert.That(_terminalApiKey, Is.Not.EqualTo(""));
            Console.WriteLine("\nStarting TestValidateExemptionExist3DSFalse");
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            var planCreateResponse = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!, 
                _requestHeader!, "PendingCapture", 1, _terminalApiKey!,
                createPlanDefaultValues);
            Assert.That(planCreateResponse.InstallmentPlanNumber, Is.Not.Null);
            Console.WriteLine("TestValidateExemptionExist3DSFalse is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidateExemptionExist3DSFalse\n" + exception + "\n");
        }
    }

    [Category(" V3Tests")]
    [Test(Description = "TestValidateInitializedStatus no 3DS with generated merchant"), CancelAfter(120*1000)]
    public async Task TestValidateInitializedStatus()
    {
        try
        {
            Assert.That(_terminalApiKey, Is.Not.EqualTo(""));
            Console.WriteLine("\nStarting TestValidateInitializedStatus");
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "default");
            var planCreateResponse = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!, 
                _requestHeader!, "PendingCapture", 1, _terminalApiKey!,
                createPlanDefaultValues);
            Assert.That(_installmentPlans.ValidateInstallmentPlanCreation(planCreateResponse, "Initialized"), Is.True);
            var jResponseVerifyAuthorization =
                await _installmentPlans.SendVerifyAuthorizationRequestAsync( _requestHeader!,
                    planCreateResponse.InstallmentPlanNumber);
            Assert.That(jResponseVerifyAuthorization.IsAuthorized, Is.EqualTo(false));
            Console.WriteLine("TestValidateInitializedStatus is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidateInitializedStatus\n" + exception + "\n");
        }
    }

    [Category(" V3Tests")]
    [Test(Description = "TestValidateCreatePlanWith3Installments no 3DS with generated merchant"), CancelAfter(120*1000)]
    public async Task TestValidateCreatePlanWith3Installments()
    {
        try
        {
            Assert.That(_terminalApiKey, Is.Not.EqualTo(""));
            Console.WriteLine("\nStarting TestValidateCreatePlanWith3Installments");
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "with_exemption");
            var planCreateResponse = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!, 
                _requestHeader!, "PendingCapture", new Random().Next(4, 12), _terminalApiKey!,
                createPlanDefaultValues);
            Assert.That(planCreateResponse.Authorization.Status, Is.EqualTo("Succeeded"));
            var jResponseVerifyAuthorization =
                await _installmentPlans.SendVerifyAuthorizationRequestAsync( _requestHeader!,
                    planCreateResponse.InstallmentPlanNumber);
            Assert.That(jResponseVerifyAuthorization.IsAuthorized.Equals(true), Is.True);
            Console.WriteLine("TestValidateCreatePlanWith3Installments is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidateCreatePlanWith3Installments\n" + exception + "\n");
        }
    }

    [Category(" V3Tests")]
    [Test(Description = "TestValidateCreatePlanWith1Installments no 3DS with generated merchant"), CancelAfter(120*1000)]
    public async Task TestValidateCreatePlanWith1Installments()
    {
        try
        {
            Assert.That(_terminalApiKey, Is.Not.EqualTo(""));
            Console.WriteLine("\nStarting TestValidateCreatePlanWith1Installments");
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "with_exemption");
            var planCreateResponse = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!, 
                _requestHeader!, "PendingCapture", 1, _terminalApiKey!,
                createPlanDefaultValues);
            Assert.That(planCreateResponse.Authorization.Status.Equals("Succeeded"), Is.True);
            var jResponseVerifyAuthorization =
                await _installmentPlans.SendVerifyAuthorizationRequestAsync( _requestHeader!,
                    planCreateResponse.InstallmentPlanNumber);
            Assert.That(jResponseVerifyAuthorization.IsAuthorized.Equals(true), Is.True);
            Console.WriteLine("TestValidateCreatePlanWith1Installments is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidateCreatePlanWith1Installments\n" + exception + "\n");
        }
    }

    [Category(" V3Tests")]
    [Test(Description = "TestValidateCreatePlanWithInsufficientFunds no 3DS with generated merchant"), CancelAfter(120*1000)]
    public async Task TestValidateCreatePlanWithInsufficientFunds()
    {
        try
        {
            Assert.That(_terminalApiKey, Is.Not.EqualTo(""));
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
            var planCreateResponse = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!, 
                _requestHeader!,
                "PendingCapture", new Random().Next(4, 12), _terminalApiKey!, createPlanDefaultValues, "yes");
            Assert.That(planCreateResponse.Error.Message.Equals("GtwyResultCCDataInsufficientFunds"), Is.True);
            var jResponseVerifyAuthorization =
                await _installmentPlans.SendVerifyAuthorizationRequestAsync( _requestHeader!,
                    planCreateResponse.InstallmentPlanNumber);
            Assert.That(jResponseVerifyAuthorization.IsAuthorized, Is.EqualTo(false));
            Console.WriteLine("TestValidateCreatePlanWithInsufficientFunds is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidateCreatePlanWithInsufficientFunds\n" + exception + "\n");
        }
    }

    [Category(" V3Tests")]
    [Test(Description = "TestValidateGetPlanByRefOrderNumber no 3DS with generated merchant"), CancelAfter(120*1000)]
    public async Task TestValidateGetPlanByRefOrderNumber()
    {
        try
        {
            Assert.That(_terminalApiKey, Is.Not.EqualTo(""));
            Console.WriteLine("\nStarting TestValidateGetPlanByRefOrderNumber");
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "with_exemption");
            var planCreateResponse = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!, 
                _requestHeader!, "PendingCapture", new Random().Next(4, 12), _terminalApiKey!,
                createPlanDefaultValues);
            var jResponseSearchPlan =
                await _installmentPlans.SendSearchInstallmentPlanByRefNumberAsync( _requestHeader!,
                    planCreateResponse.RefOrderNumber);
            Assert.That(jResponseSearchPlan.PlanList[0], Is.Not.Null);
            Assert.That(planCreateResponse.InstallmentPlanNumber,
                Is.EqualTo(jResponseSearchPlan.PlanList[0].InstallmentPlanNumber));
            Console.WriteLine("TestValidateGetPlanByRefOrderNumber is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidateGetPlanByRefOrderNumber\n" + exception + "\n");
        }
    }

    [Category(" V3Tests")]
    [Test(Description = "TestValidateGetPlanByIPn no 3DS with generated merchant"), CancelAfter(120*1000)]
    public async Task TestValidateGetPlanByIPn()
    {
        try
        {
            Assert.That(_terminalApiKey, Is.Not.EqualTo(""));
            Console.WriteLine("\nStarting TestValidateGetPlanByIPn");
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "with_exemption");
            var planCreateResponse = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!, 
                _requestHeader!, "PendingCapture", new Random().Next(4, 12), _terminalApiKey!,
                createPlanDefaultValues);
            var jResponseSearch =
                await _installmentPlans.GetInstallmentPlanByIpnAsync( _requestHeader!, planCreateResponse.InstallmentPlanNumber);
            Assert.That(jResponseSearch.InstallmentPlanNumber, Is.Not.Null);
            Console.WriteLine("TestValidateGetPlanByIPn is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidateGetPlanByIPn\n" + exception + "\n");
        }
    }

    [Category(" V3Tests")]
    [Test(Description = "TestValidateUpdateOrderStatusShippingSetCaptureFalse no 3DS with generated merchant"), CancelAfter(120*1000)]
    public async Task TestValidateUpdateOrderStatusShippingSetCaptureFalse()
    {
        try
        {
            Assert.That(_terminalApiKey, Is.Not.EqualTo(""));
            Console.WriteLine("\nStarting TestValidateUpdateOrderStatusShippingSetCaptureFalse");
            var auditLogManager = new AuditLogController();
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "with_exemption");
            var identifier = new Identifier(null!);
            var planCreateResponse = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!, 
                _requestHeader!, "PendingCapture", new Random().Next(4, 12), _terminalApiKey!,
                createPlanDefaultValues);
            var jResponseUpdate = await _installmentPlanNumberUpdateOrder.SendUpdateRequestAsync( _requestHeader!,
                planCreateResponse.InstallmentPlanNumber, "Shipped", false, identifier);
            Assert.That(_installmentPlanNumberUpdateOrder.ValidateUpdate(jResponseUpdate!,
                jResponseUpdate!.InstallmentPlanNumber, "Shipped"), Is.True);
            var jAuditLogResponse =
                await auditLogManager.SendRetrieveAuditLogRequestAsync( _requestHeader!, planCreateResponse.InstallmentPlanNumber);
            Assert.That(auditLogManager.ValidateAuditLogLogs(
                new[] { "Create", "Update" }, jAuditLogResponse!), Is.True);
            Console.WriteLine("TestValidateUpdateOrderStatusShippingSetCaptureFalse is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidateUpdateOrderStatusShippingSetCaptureFalse\n" + exception + "\n");
        }
    }

    [Category(" V3Tests")]
    [Test(Description = "TestValidateUpdateOrderStatusDeliveredSetCaptureFalse no 3DS with generated merchant"), CancelAfter(120*1000)]
    public async Task TestValidateUpdateOrderStatusDeliveredSetCaptureFalse()
    {
        try
        {
            Assert.That(_terminalApiKey, Is.Not.EqualTo(""));
            Console.WriteLine("\nStarting TestValidateUpdateOrderStatusDeliveredSetCaptureFalse");
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "with_exemption");
            var identifier = new Identifier(null!);
            var planCreateResponse = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!, 
                _requestHeader!, "PendingCapture", new Random().Next(4, 12), _terminalApiKey!,
                createPlanDefaultValues);
            var jResponseUpdate = await _installmentPlanNumberUpdateOrder.SendUpdateRequestAsync( _requestHeader!,
                planCreateResponse.InstallmentPlanNumber, "Delivered", false, identifier);
            Assert.That(_installmentPlanNumberUpdateOrder.ValidateUpdate(jResponseUpdate!,
                jResponseUpdate!.InstallmentPlanNumber, "Delivered"), Is.True);
            Console.WriteLine("TestValidateUpdateOrderStatusDeliveredSetCaptureFalse is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidateUpdateOrderStatusDeliveredSetCaptureFalse\n" + exception + "\n");
        }
    }

    [Category(" V3Tests")]
    [Test(Description = "TestValidateUpdateOrderSetCaptureTrue no 3DS with generated merchant"), CancelAfter(120*1000)]
    public async Task TestValidateUpdateOrderSetCaptureTrue()
    {
        try
        {
            Assert.That(_terminalApiKey, Is.Not.EqualTo(""));
            Console.WriteLine("\nStarting TestValidateUpdateOrderSetCaptureTrue");
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "with_exemption");
            var identifier = new Identifier(null!);
            var planCreateResponse = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!, 
                _requestHeader!, "PendingCapture", new Random().Next(4, 12), _terminalApiKey!,
                createPlanDefaultValues);
            var jResponseUpdate = await _installmentPlanNumberUpdateOrder.SendUpdateRequestAsync( _requestHeader!,
                planCreateResponse.InstallmentPlanNumber, "Delivered", false, identifier);
            Assert.That(_installmentPlanNumberUpdateOrder.ValidateUpdate(jResponseUpdate!,
                jResponseUpdate!.InstallmentPlanNumber, "Delivered"), Is.True);
            var jResponseSearch = await _installmentPlans.GetInstallmentPlanByIpnAsync( _requestHeader!, planCreateResponse.InstallmentPlanNumber);
            Assert.That(jResponseSearch.Status, Is.EqualTo("Active"));
            Console.WriteLine("TestValidateUpdateOrderSetCaptureTrue is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidateUpdateOrderSetCaptureTrue\n" + exception + "\n");
        }
    }

    [Category(" V3Tests")]
    [Test(Description = "TestValidateFullRefund no 3DS with generated merchant"), CancelAfter(120*1000)]
    public async Task TestValidateFullRefund()
    {
        try
        {
            Assert.That(_terminalApiKey, Is.Not.EqualTo(""));
            Console.WriteLine("\nStarting TestValidateFullRefund");
            var auditLogManager = new AuditLogController();
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "with_exemption");
            var identifier = new Identifier(null!);
            var planCreateResponse = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!, 
                _requestHeader!, "PendingCapture", new Random().Next(4, 12), _terminalApiKey!,
                createPlanDefaultValues);
            var jResponseUpdate = await _installmentPlanNumberUpdateOrder.SendUpdateRequestAsync( _requestHeader!,
                planCreateResponse.InstallmentPlanNumber, "Delivered", false, identifier);
            Assert.That(_installmentPlanNumberUpdateOrder.ValidateUpdate(jResponseUpdate!,
                jResponseUpdate!.InstallmentPlanNumber, "Delivered"), Is.True);
            var jResponseRefund = await _installmentPlanNumberRefund.SendRefundRequestAsync( _requestHeader!,
                planCreateResponse.InstallmentPlanNumber, "FullRefund");
            Assert.That(_installmentPlanNumberRefund.ValidateRefund(jResponseRefund, planCreateResponse.InstallmentPlanNumber),
                Is.True);
            var jAuditLogResponse =
                await auditLogManager.SendRetrieveAuditLogRequestAsync( _requestHeader!, planCreateResponse.InstallmentPlanNumber);
            Assert.That(auditLogManager.ValidateAuditLogLogs(
                new[] { "Create", "Update", "RefundReceived" }, jAuditLogResponse!), Is.True);
            Console.WriteLine("TestValidateFullRefund is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidateFullRefund\n" + exception + "\n");
        }
    }

    [Category(" V3Tests")]
    [Test(Description = "TestValidatePartialRefund no 3DS with generated merchant"), CancelAfter(120*1000)]
    public async Task TestValidatePartialRefund()
    {
        try
        {
            Assert.That(_terminalApiKey, Is.Not.EqualTo(""));
            Console.WriteLine("\nStarting TestValidatePartialRefund");
            var auditLogManager = new AuditLogController();
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "with_exemption");
            var identifier = new Identifier(null!);
            var planCreateResponse = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!, 
                _requestHeader!, "PendingCapture", new Random().Next(4, 12), _terminalApiKey!,
                createPlanDefaultValues);
            var jResponseUpdate = await _installmentPlanNumberUpdateOrder.SendUpdateRequestAsync( _requestHeader!,
                planCreateResponse.InstallmentPlanNumber, "Delivered", false, identifier);
            Assert.That(_installmentPlanNumberUpdateOrder.ValidateUpdate(jResponseUpdate!,
                jResponseUpdate!.InstallmentPlanNumber, "Delivered"), Is.True);
            var jResponseRefund = await _installmentPlanNumberRefund.SendRefundRequestAsync( _requestHeader!,
                planCreateResponse.InstallmentPlanNumber, "PartialRefund");
            Assert.That(_installmentPlanNumberRefund.ValidateRefund(jResponseRefund, planCreateResponse.InstallmentPlanNumber),
                Is.True);
            var jAuditLogResponse =
                await auditLogManager.SendRetrieveAuditLogRequestAsync( _requestHeader!, planCreateResponse.InstallmentPlanNumber);
            Assert.That(auditLogManager.ValidateAuditLogLogs(
                new[] { "Create", "Update", "RefundReceived" }, jAuditLogResponse!), Is.True);
            Console.WriteLine("TestValidatePartialRefund is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidatePartialRefund\n" + exception + "\n");
        }
    }

    [Category(" V3Tests")]
    [Test(Description = "TestValidateExceededRefund no 3DS with generated merchant"), CancelAfter(120*1000)]
    public async Task TestValidateExceededRefund()
    {
        try
        {
            Assert.That(_terminalApiKey, Is.Not.EqualTo(""));
            Console.WriteLine("\nStarting TestValidateExceededRefund");
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "with_exemption");
            var identifier = new Identifier(null!);
            var planCreateResponse = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!, 
                _requestHeader!, "PendingCapture", new Random().Next(4, 12), _terminalApiKey!,
                createPlanDefaultValues);
            var jResponseUpdate = await _installmentPlanNumberUpdateOrder.SendUpdateRequestAsync( _requestHeader!,
                planCreateResponse.InstallmentPlanNumber, "Delivered", false, identifier);
            Assert.That(_installmentPlanNumberUpdateOrder.ValidateUpdate(jResponseUpdate!,
                jResponseUpdate!.InstallmentPlanNumber, "Delivered"), Is.True);
            var jResponseRefund = await _installmentPlanNumberRefund.SendRefundRequestAsync( _requestHeader!,
                planCreateResponse.InstallmentPlanNumber, "ExceededAmount", null!, "yes");
            Assert.That(jResponseRefund.Error.Code.Contains("400"));
            Console.WriteLine("TestValidateExceededRefund is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidateExceededRefund\n" + exception + "\n");
        }
    }

    [Category(" V3Tests")]
    [Test(Description = "TestValidateCreatePlanWith3DsGetPlanShouldReturn3DsObject yes 3DS with generated merchant"), CancelAfter(120*1000)]
    public async Task TestValidateCreatePlanWith3DsGetPlanShouldReturn3DsObject()
    {
        try
        {
            Assert.That(_terminalApiKey, Is.Not.EqualTo(""));
            Console.WriteLine("\nStarting TestValidateCreatePlanWith3DsGetPlanShouldReturn3DsObject");
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "default");
            var planCreateResponse = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!, 
                _requestHeader!, "PendingCapture", new Random().Next(4, 12), _terminalApiKey!,
                createPlanDefaultValues);
            Assert.That(planCreateResponse.Status, Is.EqualTo("Initialized"));
            Assert.That(_installmentPlans.Validate3DsFailure(planCreateResponse), Is.True);
            Console.WriteLine("TestValidateCreatePlanWith3DsGetPlanShouldReturn3DsObject is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidateCreatePlanWith3DsGetPlanShouldReturn3DsObject\n" + exception + "\n");
        }
    }

    [Category(" V3Tests")]
    [Test(Description =
        "TestValidateCreateActivePlanAndCheckTheAsyncCaptureFirstInstallmentShouldBeCaptured no 3DS with generated merchant"), CancelAfter(120*1000)]
    public async Task TestValidateCreateActivePlanAndCheckTheAsyncCaptureFirstInstallmentShouldBeCaptured()
    {
        try
        {
            Assert.That(_terminalApiKey, Is.Not.EqualTo(""));
            Console.WriteLine("\nStarting TestValidateCreateActivePlanAndCheckTheAsyncCaptureFirstInstallmentShouldBeCaptured");
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "with_exemption");
            var planCreateResponse = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!, 
                _requestHeader!, "Active", new Random().Next(4, 12), _terminalApiKey!,
                createPlanDefaultValues);
            var jResponsePlanJobFutureInfo =
                await _planJobFutureInformation.SendGetRequestForFutureJobsAsync( _requestHeader!,
                    planCreateResponse.InstallmentPlanNumber);
            Assert.That(_planJobFutureInformation.ValidateJobName(jResponsePlanJobFutureInfo, "StartInstallments"),
                Is.True);
            Console.WriteLine(
                "TestValidateCreateActivePlanAndCheckTheAsyncCaptureFirstInstallmentShouldBeCaptured is Done\n");
        }
        catch (Exception exception)
        {
            Console.WriteLine(
                "Error in TestValidateCreateActivePlanAndCheckTheAsyncCaptureFirstInstallmentShouldBeCaptured\n" +
                exception + "\n");
        }
    }

    [Category(" V3Tests")]
    [Test(Description = "TestValidateActiveStatus no 3DS with generated merchant"), CancelAfter(120*1000)]
    public async Task TestValidateActiveStatus()
    {
        try
        {
            Assert.That(_terminalApiKey, Is.Not.EqualTo(""));
            Console.WriteLine("\nStarting TestValidateActiveStatus");
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "with_exemption");
            var planCreateResponse = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!, 
                _requestHeader!, "Active", new Random().Next(4, 12), _terminalApiKey!,
                createPlanDefaultValues);
            var jResponsePlanJobFutureInfo =
                await _planJobFutureInformation.SendGetRequestForFutureJobsAsync( _requestHeader!,
                    planCreateResponse.InstallmentPlanNumber);
            Assert.That(_planJobFutureInformation.ValidateJobName(jResponsePlanJobFutureInfo, "StartInstallments"),
                Is.True);
            var jResponseVerify =
                await _installmentPlans.SendVerifyAuthorizationRequestAsync( _requestHeader!,
                    planCreateResponse.InstallmentPlanNumber);
            Assert.That(_installmentPlans.ValidateVerifyAuthorizationRequest(jResponseVerify, true), Is.True);
            Console.WriteLine("TestValidateActiveStatus is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidateActiveStatus\n" + exception + "\n");
        }
    }

    [Category(" V3Tests")]
    [Test(Description = "TestValidatePrePaidCardOneInstallment no 3DS with generated merchant"), CancelAfter(120*1000)]
    public async Task TestValidatePrePaidCardOneInstallment()
    {
        try
        {
            Assert.That(_terminalApiKey, Is.Not.EqualTo(""));
            Console.WriteLine("\nStarting TestValidatePrePaidCardOneInstallment");
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "with_exemption");
            createPlanDefaultValues.paymentMethod.card.cardNumber = Environment.GetEnvironmentVariable("PrePaidCard")!;
            var planCreateResponse = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!, 
                _requestHeader!, "Active", 1, _terminalApiKey!,
                createPlanDefaultValues);
            Assert.That(planCreateResponse.InstallmentPlanNumber, Is.Not.Null);
            Console.WriteLine("TestValidatePrePaidCardOneInstallment is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidatePrePaidCardOneInstallment\n" + exception + "\n");
        }
    }

    [Category(" V3Tests")]
    [Test(Description = "TestValidatePrePaidCardMoreThenOneInstallment no 3DS with generated merchant"), CancelAfter(120*1000)]
    public async Task TestValidatePrePaidCardMoreThenOneInstallment()
    {
        try
        {
            Assert.That(_terminalApiKey, Is.Not.EqualTo(""));
            Console.WriteLine("\nStarting TestValidatePrePaidCardMoreThenOneInstallment");
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "with_exemption");
            createPlanDefaultValues.paymentMethod.card.cardNumber = Environment.GetEnvironmentVariable("PrePaidCard")!;
            var planCreateResponse = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!, 
                _requestHeader!, "Active", new Random().Next(4, 12), _terminalApiKey!,
                createPlanDefaultValues, "yes");
            Assert.That(planCreateResponse.Error.Code, Does.Contain("400"));
            Console.WriteLine("TestValidatePrePaidCardMoreThenOneInstallment is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidatePrePaidCardMoreThenOneInstallment\n" + exception + "\n");
        }
    }

    [Category(" V3Tests")]
    [Test(Description = "TestValidateDebitCardOneInstallment no 3DS with generated merchant"), CancelAfter(120*1000)]
    public async Task TestValidateDebitCardOneInstallment()
    {
        try
        {
            Assert.That(_terminalApiKey, Is.Not.EqualTo(""));
            Console.WriteLine("\nStarting TestValidateDebitCardOneInstallment");
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "with_exemption");
            createPlanDefaultValues.paymentMethod.card.cardNumber = Environment.GetEnvironmentVariable("DebitCard")!;
            var planCreateResponse = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!, 
                _requestHeader!, "Active", 1, _terminalApiKey!,
                createPlanDefaultValues);
            Assert.That(planCreateResponse.InstallmentPlanNumber, Is.Not.Null);
            Console.WriteLine("TestValidateDebitCardOneInstallment is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidateDebitCardOneInstallment\n" + exception + "\n");
        }
    }

    [Category(" V3Tests")]
    [Test(Description = "TestValidateV3CancelPlan without 3DS with generated merchant"), CancelAfter(120*1000)]
    public async Task TestValidateV3CancelPlan()
    {
        try
        {
            Assert.That(_terminalApiKey, Is.Not.EqualTo(""));
            Console.WriteLine("\nStarting TestValidateV3CancelPlan");
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            var planCreateResponse = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!,  _requestHeader!,
                "Active", new Random().Next(4, 12), _terminalApiKey!,
                createPlanDefaultValues);
            var cancelResponse = await _installmentPlanNumberCancel.SendCancelPlanRequestAsync( _requestHeader!,
                planCreateResponse.InstallmentPlanNumber, 0, "NoRefunds");
            Assert.That(cancelResponse.ResponseHeader.Succeeded);
            Console.WriteLine("TestValidateV3CancelPlan is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidateV3CancelPlan\n" + exception + "\n");
        }
    }

    [Category(" V3Tests")]
    [Test(Description = "TestValidateV3CheckEligibility without 3DS with generated merchant"), CancelAfter(120*1000)]
    public async Task TestValidateV3CheckEligibility()
    {
        try
        {
            Assert.That(_terminalApiKey, Is.Not.EqualTo(""));
            Console.WriteLine("\nStarting TestValidateV3CheckEligibility");
            var checkEligibility = new CheckEligibilityDefaultValues();
            var jResponse =
                await _checkInstallmentsEligibility.SendCheckEligibilityRequestAsync(_requestHeader!, _terminalApiKey!,
                    checkEligibility);
            Assert.That(_checkInstallmentsEligibility.ValidateCheckEligibility(jResponse), Is.True);
            Console.WriteLine("TestValidateV3CheckEligibility is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidateV3CheckEligibility\n" + exception + "\n");
        }
    }

    [Category(" V3Tests")]
    [Test(Description = "TestValidateMerchantSettings without 3DS with generated merchant"), CancelAfter(120*1000)]
    public async Task TestValidateMerchantSettings()
    {
        try
        {
            Assert.That(_terminalApiKey, Is.Not.EqualTo(""));
            Console.WriteLine("\nStarting TestValidateMerchantSettings");
            var planCreateResponse = await _settings.SendGetSettingsRequestAsync( _requestHeader!, _businessUnitId);
            Assert.That(_settings.ValidateMerchantPaymentSettings(planCreateResponse!, false), Is.True);
            Console.WriteLine("TestValidateMerchantSettings is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidateMerchantSettings\n" + exception + "\n");
        }
    }

    [Category("V3Tests")]
    [Test(Description = "TestValidateFirstInstallmentAmountFunctionality without 3DS generated merchant")]
    public async Task TestValidateFirstInstallmentAmountFunctionality()
    {
        try
        {
            Assert.That(_terminalApiKey, Is.Not.EqualTo(""));
            Console.WriteLine("\nStarting TestValidateFirstInstallmentAmountFunctionality");
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            createPlanDefaultValues.planData.FirstInstallmentAmount = 502;
            createPlanDefaultValues.planData.totalAmount = 1000;
            var json = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!,  _requestHeader!,
                "Active", 2, _terminalApiKey!, createPlanDefaultValues);
            Assert.That(_installmentPlans.ValidateFirstInstallmentAmount(json, 502.0), Is.True);
            Console.WriteLine("TestValidateFirstInstallmentAmountFunctionality is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidateFirstInstallmentAmountFunctionality\n" + exception + "\n");
        }
    }
    
    [Category("V3Tests")]
    [Test(Description = "TestValidateFirstInstallmentAmountFunctionalityNegative without 3DS generated merchant")]
    public async Task TestValidateFirstInstallmentAmountFunctionalityNegative()
    {
        try
        {
            Assert.That(_terminalApiKey, Is.Not.EqualTo(""));
            Console.WriteLine("\nStarting TestValidateFirstInstallmentAmountFunctionalityNegative");
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            createPlanDefaultValues.planData.FirstInstallmentAmount = 400;
            createPlanDefaultValues.planData.totalAmount = 1000;
            var json = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!,  _requestHeader!,
                "Active", 2, _terminalApiKey!, createPlanDefaultValues);
            Assert.That(_installmentPlans.ValidateFirstInstallmentAmount(json, 400.0), Is.True);
            Console.WriteLine("TestValidateFirstInstallmentAmountFunctionalityNegative is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidateFirstInstallmentAmountFunctionalityNegative\n" + exception + "\n");
        }
    }

    [Category("V3Tests")]
    [Test(Description = "TestValidateFirstInstallmentAmountFunctionalityNoValue without 3DS generated merchant")]
    public async Task TestValidateFirstInstallmentAmountFunctionalityNoValue()
    {
        try
        {
            Assert.That(_terminalApiKey, Is.Not.EqualTo(""));
            Console.WriteLine("\nStarting TestValidateFirstInstallmentAmountFunctionalityNoValue");
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            var json = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!,  _requestHeader!,
                "Active", 2, _terminalApiKey!, createPlanDefaultValues);
            Assert.That(_installmentPlans.ValidateAllInstallmentsAmountsEquals(json), Is.True);
            Console.WriteLine("TestValidateFirstInstallmentAmountFunctionalityNoValue is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidateFirstInstallmentAmountFunctionalityNoValue\n" + exception + "\n");
        }
    }

    [Category("V3Tests")]
    [Test(Description = "TestValidateFirstInstallmentDateFunctionalityTomorrow without 3DS generated merchant")]
    public async Task TestValidateFirstInstallmentDateFunctionalityTomorrow()
    {
        try
        {
            Assert.That(_terminalApiKey, Is.Not.EqualTo(""));
            Console.WriteLine("\nStarting TestValidateFirstInstallmentDateFunctionalityTomorrow");
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            createPlanDefaultValues.planData.FirstInstallmentDate = DateTime.Today.AddDays(1).ToString("MM/dd/yyyy");
            var json = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!,  _requestHeader!,
                "Active", new Random().Next(4, 12), _terminalApiKey!, createPlanDefaultValues);
            Assert.That(
                json.Installments[0].ProcessDateTime.ToString("MM/dd/yyyy")
                    .Contains(DateTime.Today.AddDays(1).ToString("MM/dd/yyyy")), Is.True);
            Console.WriteLine("TestValidateFirstInstallmentDateFunctionalityTomorrow is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidateFirstInstallmentDateFunctionalityTomorrow\n" + exception + "\n");
        }
    }

    [Category("V3Tests")]
    [Test(Description = "TestValidateFirstInstallmentDateFunctionalityNoValue without 3DS generated merchant")]
    public async Task TestValidateFirstInstallmentDateFunctionalityNoValue()
    {
        try
        {
            Assert.That(_terminalApiKey, Is.Not.EqualTo(""));
            Console.WriteLine("\nStarting TestValidateFirstInstallmentDateFunctionalityNoValue");
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            var json = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!,  _requestHeader!,
                "Active", new Random().Next(4, 12), _terminalApiKey!, createPlanDefaultValues);
            Assert.That(
                json.Installments[0].ProcessDateTime.ToString("MM/dd/yyyy").Equals(DateTime.Now.ToString("MM/dd/yyyy")),
                Is.True);
            Console.WriteLine("TestValidateFirstInstallmentDateFunctionalityNoValue is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidateFirstInstallmentDateFunctionalityNoValue\n" + exception + "\n");
        }
    }
    
    [Category("V3Tests")]
    [Test(Description = "TestValidatePlanAmountRounding with generated merchant"), CancelAfter(120*1000)]
    public async Task TestValidatePlanAmountRounding()
    {
        try
        {
            Assert.That(_terminalApiKey, Is.Not.EqualTo(""));
            Console.WriteLine("\nStarting TestValidatePlanAmountRounding");
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues.planData.currency = "JPY";
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            var json = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!,  _requestHeader!,
                "Active", new Random().Next(4, 12), _terminalApiKey!,
                createPlanDefaultValues, "yes");
            Assert.That(json.Error.Message.Contains("decimal"));
            Console.WriteLine("TestValidatePlanAmountRounding is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidatePlanAmountRounding\n" + exception + "\n");
        }
    }
}