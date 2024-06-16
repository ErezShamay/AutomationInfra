using Allure.NUnit;
using Allure.NUnit.Attributes;
using NUnit.Framework;
using Splitit.Automation.NG.Backend.BaseActions;
using Splitit.Automation.NG.Backend.Services.AdminApi.InstallmentPlan.InstallmentsPlanFunctionality;
using Splitit.Automation.NG.Backend.Services.V1.InstallmentPlan.InstallmentPlanApiEndPoints;
using Splitit.Automation.NG.Backend.Services.V3.DefaultValues;
using Splitit.Automation.NG.Backend.Services.V3.InstallmentPlan.InstallmentPlanApiEndPoint;
using Splitit.Automation.NG.Backend.Tests.TestsSetups;
using Splitit.Automation.NG.Backend.Services.Notifications.Notification.NotificationsFunctionality;
using Splitit.Automation.NG.Backend.Services.V1.DefaultValues;
using Splitit.Automation.NG.Utilities.EnvironmentsConfig;
using BillingAddressDefaultValues = Splitit.Automation.NG.Backend.Services.V3.DefaultValues.BillingAddressDefaultValues;
using Create = Splitit.Automation.NG.Backend.Services.V1.InstallmentPlan.InstallmentPlanApiEndPoints.Create;

namespace Splitit.Automation.NG.SharedResources.Tests.GatewaysFunctionality;

[TestFixture]
[AllureNUnit]
[AllureSuite("GatewaysNotificationsTests")]
[AllureDisplayIgnored]
[Parallelizable(scope: ParallelScope.All)]
public class GatewaysNotificationsTests
{
    private readonly InstallmentPlans _installmentPlans;
    private RequestHeader? _requestHeader;
    private readonly GetPgtl _getPgtl;
    private readonly GetList _getList;
    private readonly InstallmentPlanNumberRefund _installmentPlanNumberRefund;
    private readonly EnvConfig _envConfig;
    private readonly ChargeFunctionality _chargeFunctionality;
    private readonly Create _create;
    private readonly GetAuditLog _getAuditLog;
    
    public GatewaysNotificationsTests()
    {
        Console.WriteLine("Starting Setup");
        _installmentPlans = new InstallmentPlans();
        _getPgtl = new GetPgtl();
        _getList = new GetList();
        _installmentPlanNumberRefund = new InstallmentPlanNumberRefund();
        _envConfig = new EnvConfig();
        _chargeFunctionality = new ChargeFunctionality();
        _create = new Create();
        _getAuditLog = new GetAuditLog();
        Console.WriteLine("Done with Setup\n");
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
    
    [TestCase(Category = "GatewaysNotificationsTests")]
    [Test(Description = "AdyenCaptureWebhookNotifications")]
    public async Task TestValidateAdyenCaptureWebhookNotifications()
    {
        try
        {
            Console.WriteLine("\nStarting TestValidateAdyenCaptureWebhookNotifications");
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            var planCreateResponse = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!,  _requestHeader!, "Active", 
                3, _envConfig.AdyenTerminal, createPlanDefaultValues);
            var idempotecnyKey = await _getPgtl.ValidatePgtlKeyValueInnerAsync(_envConfig.StoreProcedureUrl, 
                _requestHeader!, planCreateResponse.InstallmentPlanNumber, "Type", "Capture", "IdempotencyKey");
            var getNotificationsListResponse = await _getList.SendGetRequestGetNotificationsListAsync(_requestHeader!,
                    "IdempotecnyKey", idempotecnyKey!);
            Assert.That(_getList.ValidateGatewayEventType(getNotificationsListResponse, "CAPTURE"));
            Assert.That(getNotificationsListResponse.isSuccess);
            Console.WriteLine("TestValidateAdyenCaptureWebhookNotifications is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidateAdyenCaptureWebhookNotifications \n" + exception + "\n");
        }
    }
    
    [TestCase(Category = "GatewaysNotificationsTests")]
    [Test(Description = "AdyenFailedCaptureWebhookNotifications")]
    public async Task TestValidateAdyenFailedCaptureWebhookNotifications()
    {
        try
        {
            Console.WriteLine("\nStarting TestValidateAdyenFailedCaptureWebhookNotifications");
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            var planCreateResponse = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!,  _requestHeader!, "Active", 
                1, _envConfig.AdyenTerminal, createPlanDefaultValues);
            await Task.Delay(2 * 1000);
            var idempotecnyKey = await _getPgtl.ValidatePgtlKeyValueInnerAsync(_envConfig.StoreProcedureUrl, 
                _requestHeader!, planCreateResponse.InstallmentPlanNumber, "Type", "Capture", "IdempotencyKey");
            var getNotificationsListResponse = await _getList.SendGetRequestGetNotificationsListAsync(_requestHeader!,
                "IdempotecnyKey", idempotecnyKey!);
            Assert.That(_getList.ValidateGatewayEventType(getNotificationsListResponse, "CAPTURE"));
            var chargeResponse = await _chargeFunctionality.SendPostRequestChargeFunctionalityAsync(_requestHeader!,
                planCreateResponse.InstallmentPlanNumber, true);
            Assert.That(chargeResponse.Errors, Is.Not.Null);
            Console.WriteLine("TestValidateAdyenFailedCaptureWebhookNotifications is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidateAdyenFailedCaptureWebhookNotifications \n" + exception + "\n");
        }
    }
    
    [TestCase(Category = "GatewaysNotificationsTests")]
    [Test(Description = "AdyenRefundWebhookNotifications")]
    public async Task TestValidateAdyenRefundWebhookNotifications()
    {
        try
        {
            Console.WriteLine("\nStarting TestValidateAdyenRefundWebhookNotifications");
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            var planCreateResponse = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!,  _requestHeader!, "Active", 
                2, _envConfig.AdyenTerminal, createPlanDefaultValues);
            await _installmentPlanNumberRefund.SendRefundRequestAsync( _requestHeader!, 
                planCreateResponse.InstallmentPlanNumber, "FullRefund");
            var idempotecnyKey = await _getPgtl.ValidatePgtlKeyValueInnerAsync(_envConfig.StoreProcedureUrl, 
                _requestHeader!, planCreateResponse.InstallmentPlanNumber, "Type", "Refund", "IdempotencyKey");
            var getNotificationsListResponse = await _getList.SendGetRequestGetNotificationsListAsync(_requestHeader!,
                "IdempotecnyKey", idempotecnyKey!);
            Assert.That(_getList.ValidateGatewayEventType(getNotificationsListResponse, "REFUND"));
            Console.WriteLine("TestValidateAdyenRefundWebhookNotifications is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidateAdyenRefundWebhookNotifications \n" + exception + "\n");
        }
    }
    
    [TestCase(Category = "GatewaysNotificationsTests")]
    [Test(Description = "AuthorizeNetV2CaptureWebhookNotifications")]
    public async Task TestValidateAuthorizeNetV2CaptureWebhookNotifications()
    {
        try
        {
            Console.WriteLine("\nStarting TestValidateAuthorizeNetV2CaptureWebhookNotifications");
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            var planCreateResponse = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!,  _requestHeader!, "Active", 
                new Random().Next(4, 6), _envConfig.AuthorizeNetV2, createPlanDefaultValues);
            var auditLogResponse = await _getAuditLog.SendGetRequestForGetAuditLogAsync(_requestHeader!, 
                planCreateResponse.InstallmentPlanNumber);
            Assert.That(await _getAuditLog.ValidateAuditLogExpectedActivityWithPolling(auditLogResponse, 
                "InstallmentPlan.Capture.Succeeded.Gateway.Notification", _requestHeader!,
                planCreateResponse.InstallmentPlanNumber));
            Console.WriteLine("TestValidateAuthorizeNetV2CaptureWebhookNotifications is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidateAuthorizeNetV2CaptureWebhookNotifications \n" + exception + "\n");
        }
    }
    
    [TestCase(Category = "GatewaysNotificationsTests")]
    [Test(Description = "AuthorizeNetV2RefundWebhookNotifications")]
    public async Task TestValidateAuthorizeNetV2RefundWebhookNotifications()
    {
        try
        {
            Console.WriteLine("\nStarting TestValidateAuthorizeNetV2CaptureWebhookNotifications");
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            var planCreateResponse = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!,  _requestHeader!, "Active", 
                new Random().Next(4, 6), _envConfig.AuthorizeNetV2, createPlanDefaultValues);
            await _installmentPlanNumberRefund.SendRefundRequestAsync( _requestHeader!, 
                planCreateResponse.InstallmentPlanNumber, "FullRefund");
            var auditLogResponse = await _getAuditLog.SendGetRequestForGetAuditLogAsync(_requestHeader!, 
                planCreateResponse.InstallmentPlanNumber);
            Assert.That(await _getAuditLog.ValidateAuditLogExpectedActivityWithPolling(auditLogResponse, 
                "RefundUpdated", _requestHeader!,
                planCreateResponse.InstallmentPlanNumber));
            Console.WriteLine("TestValidateAuthorizeNetV2CaptureWebhookNotifications is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidateAuthorizeNetV2CaptureWebhookNotifications \n" + exception + "\n");
        }
    }
    
    // [TestCase(Category = "GatewaysNotificationsTests")]
    // [Test(Description = "BamboraCaptureWebhookNotifications")]
    // public async Task TestValidateBamboraCaptureWebhookNotifications()
    // {
    //     try
    //     {
    //         Console.WriteLine("\nStarting TestValidateBamboraCaptureWebhookNotifications");
    //         var createPlanDefaultValues = new CreatePlanDefaultValues();
    //         var is3DsVarsCreation = new Is3DsVarsCreation();
    //         var jResponseSettings = await _settings.SendGetSettingsRequestAsync( _requestHeader!, int.Parse(_envConfig.NgMockV2BU));
    //         jResponseSettings!.MerchantSettings.DefaultPlanStrategy = "SecuredPlan4";
    //         var jResponseUpdated = await _settingsSave.SendUpdateSettingsRequestAsync( _requestHeader!, jResponseSettings);
    //         Assert.That(jResponseUpdated.MerchantSettings.DefaultPlanStrategy, Is.EqualTo("SecuredPlan4"));
    //         createPlanDefaultValues.planData.totalAmount = AmountGenerator.GenerateAmountWithMinMaxValues(10, 99);
    //         createPlanDefaultValues.paymentMethod.card.cardNumber = _envConfig.Card5100;
    //         createPlanDefaultValues.paymentMethod.card.cardCvv = "123";
    //         createPlanDefaultValues.planData.currency = "GBP";
    //         createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
    //         var planCreateResponse = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!,  _requestHeader!, "Active", 
    //             new Random().Next(4, 6), _envConfig.BamboraTerminal, createPlanDefaultValues);
    //         var idempotecnyKey = await _getPgtl.ValidatePgtlKeyValueInnerAsync(_envConfig.StoreProcedureUrl, 
    //             _requestHeader!, planCreateResponse.InstallmentPlanNumber, "Type", "Capture", "IdempotencyKey");
    //         var getNotificationsListResponse = await _getList.SendGetRequestGetNotificationsListAsync(_requestHeader!,
    //             "IdempotecnyKey", idempotecnyKey!);
    //         Assert.That(_getList.ValidateGatewayEventType(getNotificationsListResponse, "PAC_Approved"));
    //         Console.WriteLine("TestValidateBamboraCaptureWebhookNotifications is Done\n");
    //     }
    //     catch (Exception exception)
    //     {
    //         Assert.Fail("Error in TestValidateBamboraCaptureWebhookNotifications \n" + exception + "\n");
    //     }
    // }
    
    // [TestCase(Category = "GatewaysNotificationsTests")]
    // [Test(Description = "BamboraRefundWebhookNotifications")]
    // public async Task TestValidateBamboraRefundWebhookNotifications()
    // {
    //     try
    //     {
    //         Console.WriteLine("\nStarting TestValidateBamboraRefundWebhookNotifications");
    //         var createPlanDefaultValues = new CreatePlanDefaultValues();
    //         var is3DsVarsCreation = new Is3DsVarsCreation();
    //         var jResponseSettings = await _settings.SendGetSettingsRequestAsync( _requestHeader!, int.Parse(_envConfig.NgMockV2BU));
    //         jResponseSettings!.MerchantSettings.DefaultPlanStrategy = "SecuredPlan4";
    //         var jResponseUpdated = await _settingsSave.SendUpdateSettingsRequestAsync( _requestHeader!, jResponseSettings);
    //         Assert.That(jResponseUpdated.MerchantSettings.DefaultPlanStrategy, Is.EqualTo("SecuredPlan4"));
    //         createPlanDefaultValues.planData.totalAmount = AmountGenerator.GenerateAmountWithMinMaxValues(10, 99);
    //         createPlanDefaultValues.paymentMethod.card.cardNumber = _envConfig.Card5100;
    //         createPlanDefaultValues.paymentMethod.card.cardCvv = "123";
    //         createPlanDefaultValues.planData.currency = "GBP";
    //         createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
    //         var planCreateResponse = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!,  _requestHeader!, "Active", 
    //             new Random().Next(4, 6), _envConfig.BamboraTerminal, createPlanDefaultValues);
    //         await _installmentPlanNumberRefund.SendRefundRequestAsync( _requestHeader!, 
    //             planCreateResponse.InstallmentPlanNumber, "FullRefund");
    //         var idempotecnyKey = await _getPgtl.ValidatePgtlKeyValueInnerAsync(_envConfig.StoreProcedureUrl, 
    //             _requestHeader!, planCreateResponse.InstallmentPlanNumber, "Type", "Refund", "IdempotencyKey");
    //         var getNotificationsListResponse = await _getList.SendGetRequestGetNotificationsListAsync(_requestHeader!,
    //             "IdempotecnyKey", idempotecnyKey!);
    //         Assert.That(_getList.ValidateGatewayEventType(getNotificationsListResponse, "R_Approved"));
    //         Console.WriteLine("TestValidateBamboraRefundWebhookNotifications is Done\n");
    //     }
    //     catch (Exception exception)
    //     {
    //         Assert.Fail("Error in TestValidateBamboraRefundWebhookNotifications \n" + exception + "\n");
    //     }
    // }
    
    [TestCase(Category = "GatewaysNotificationsTests")]
    [Test(Description = "BluesnapMorCaptureWebhookNotifications")]
    public async Task TestValidateBluesnapMorCaptureWebhookNotifications()
    {
        try
        {
            Console.WriteLine("\nStarting TestValidateBluesnapMorCaptureWebhookNotifications");
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            var planCreateResponse = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!,  _requestHeader!, "Active", 
                new Random().Next(4, 6), _envConfig.BlueSnapMORTerminal, createPlanDefaultValues);
            var auditLogResponse = await _getAuditLog.SendGetRequestForGetAuditLogAsync(_requestHeader!, 
                planCreateResponse.InstallmentPlanNumber);
            Assert.That(await _getAuditLog.ValidateAuditLogExpectedActivityWithPolling(auditLogResponse, 
                "InstallmentPlan.Capture.Succeeded.Gateway.Notification", _requestHeader!,
                planCreateResponse.InstallmentPlanNumber));
            Console.WriteLine("TestValidateBluesnapMorCaptureWebhookNotifications is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidateBluesnapMorCaptureWebhookNotifications \n" + exception + "\n");
        }
    }
    
    [TestCase(Category = "GatewaysNotificationsTests")]
    [Test(Description = "BluesnapDirectCaptureWebhookNotifications")]
    public async Task TestValidateBluesnapDirectCaptureWebhookNotifications()
    {
        try
        {
            Console.WriteLine("\nStarting TestValidateBluesnapDirectCaptureWebhookNotifications");
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            var planCreateResponse = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!,  _requestHeader!, "Active", 
                new Random().Next(4, 6), _envConfig.BlueSnapDirectTerminal, createPlanDefaultValues);
            var auditLogResponse = await _getAuditLog.SendGetRequestForGetAuditLogAsync(_requestHeader!, 
                planCreateResponse.InstallmentPlanNumber);
            Assert.That(await _getAuditLog.ValidateAuditLogExpectedActivityWithPolling(auditLogResponse, 
                "InstallmentPlan.Capture.Succeeded.Gateway.Notification", _requestHeader!,
                planCreateResponse.InstallmentPlanNumber));
            Console.WriteLine("TestValidateBluesnapDirectCaptureWebhookNotifications is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidateBluesnapDirectCaptureWebhookNotifications \n" + exception + "\n");
        }
    }
    

    
    [TestCase(Category = "GatewaysNotificationsTests")]
    [Test(Description = "CheckoutCaptureWebhookNotifications")]
    public async Task TestValidateCheckoutCaptureWebhookNotifications()
    {
        try
        {
            Console.WriteLine("\nStarting TestValidateCheckoutCaptureWebhookNotifications");
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            var planCreateResponse = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!,  _requestHeader!, "Active", 
                new Random().Next(4, 6), _envConfig.CheckoutTerminal, createPlanDefaultValues);
            var idempotecnyKey = await _getPgtl.ValidatePgtlKeyValueInnerAsync(_envConfig.StoreProcedureUrl, 
                _requestHeader!, planCreateResponse.InstallmentPlanNumber, "Type", "Capture", "IdempotencyKey");
            var getNotificationsListResponse = await _getList.SendGetRequestGetNotificationsListAsync(_requestHeader!,
                "IdempotecnyKey", idempotecnyKey!);
            Assert.That(_getList.ValidateGatewayEventType(getNotificationsListResponse, "payment_captured"));
            Console.WriteLine("TestValidateCheckoutCaptureWebhookNotifications is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidateCheckoutCaptureWebhookNotifications \n" + exception + "\n");
        }
    }
    
    [TestCase(Category = "GatewaysNotificationsTests")]
    [Test(Description = "CheckoutRefundWebhookNotifications")]
    public async Task TestValidateCheckoutRefundWebhookNotifications()
    {
        try
        {
            Console.WriteLine("\nStarting TestValidateCheckoutRefundWebhookNotifications");
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            var planCreateResponse = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!,  _requestHeader!, "Active", 
                new Random().Next(4, 6), _envConfig.CheckoutTerminal, createPlanDefaultValues);
            await _installmentPlanNumberRefund.SendRefundRequestAsync( _requestHeader!, 
                planCreateResponse.InstallmentPlanNumber, "FullRefund");
            var idempotecnyKey = await _getPgtl.ValidatePgtlKeyValueInnerAsync(_envConfig.StoreProcedureUrl, 
                _requestHeader!, planCreateResponse.InstallmentPlanNumber, "Type", "Refund", "IdempotencyKey");
            var getNotificationsListResponse = await _getList.SendGetRequestGetNotificationsListAsync(_requestHeader!,
                "IdempotecnyKey", idempotecnyKey!);
            Assert.That(_getList.ValidateGatewayEventType(getNotificationsListResponse, "payment_refunded"));
            Console.WriteLine("TestValidateCheckoutRefundWebhookNotifications is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidateCheckoutRefundWebhookNotifications \n" + exception + "\n");
        }
    }
    
    [TestCase(Category = "GatewaysNotificationsTests")]
    [Test(Description = "CheckoutMorCaptureWebhookNotifications")]
    public async Task TestValidateCheckoutMorCaptureWebhookNotifications()
    {
        try
        {
            Console.WriteLine("\nStarting TestValidateCheckoutMorCaptureWebhookNotifications");
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            var planCreateResponse = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!,  _requestHeader!, "Active", 
                new Random().Next(4, 6), _envConfig.CheckoutTerminal, createPlanDefaultValues);
            var idempotecnyKey = await _getPgtl.ValidatePgtlKeyValueInnerAsync(_envConfig.StoreProcedureUrl, 
                _requestHeader!, planCreateResponse.InstallmentPlanNumber, "Type", "Capture", "IdempotencyKey");
            var getNotificationsListResponse = await _getList.SendGetRequestGetNotificationsListAsync(_requestHeader!,
                "IdempotecnyKey", idempotecnyKey!);
            Assert.That(_getList.ValidateGatewayEventType(getNotificationsListResponse, "payment_captured"));
            Console.WriteLine("TestValidateCheckoutMorCaptureWebhookNotifications is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidateCheckoutMorCaptureWebhookNotifications \n" + exception + "\n");
        }
    }
    
    [TestCase(Category = "GatewaysNotificationsTests")]
    [Test(Description = "CheckoutMorRefundWebhookNotifications")]
    public async Task TestValidateCheckoutMorRefundWebhookNotifications()
    {
        try
        {
            Console.WriteLine("\nStarting TestValidateCheckoutMorRefundWebhookNotifications");
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            var planCreateResponse = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!,  _requestHeader!, "Active", 
                new Random().Next(4, 6), _envConfig.CheckoutTerminal, createPlanDefaultValues);
            await _installmentPlanNumberRefund.SendRefundRequestAsync( _requestHeader!, 
                planCreateResponse.InstallmentPlanNumber, "FullRefund");
            var idempotecnyKey = await _getPgtl.ValidatePgtlKeyValueInnerAsync(_envConfig.StoreProcedureUrl, 
                _requestHeader!, planCreateResponse.InstallmentPlanNumber, "Type", "Refund", "IdempotencyKey");
            var getNotificationsListResponse = await _getList.SendGetRequestGetNotificationsListAsync(_requestHeader!,
                "IdempotecnyKey", idempotecnyKey!);
            Assert.That(_getList.ValidateGatewayEventType(getNotificationsListResponse, "payment_refunded"));
            Console.WriteLine("TestValidateCheckoutMorRefundWebhookNotifications is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidateCheckoutMorRefundWebhookNotifications \n" + exception + "\n");
        }
    }
    
    [TestCase(Category = "GatewaysNotificationsTests")]
    [Test(Description = "PaySafeDirectCaptureWebhookNotifications")]
    public async Task TestValidatePaySafeDirectCaptureWebhookNotifications()
    {
        try
        {
            Console.WriteLine("\nStarting TestValidatePaySafeDirectCaptureWebhookNotifications");
            var v1InitiateDefaultValues = new V1InitiateDefaultValues();
            var planCreateResponse = await _create.CreatePlanAsync(_requestHeader!, v1InitiateDefaultValues, 
                _envConfig.PaySafeDirectTerminal);
            var chargeResponse = await _chargeFunctionality.SendPostRequestChargeFunctionalityAsync(_requestHeader!,
                planCreateResponse.InstallmentPlan.InstallmentPlanNumber);
            Assert.That(chargeResponse.ResponseHeader.Succeeded);
            var auditLogResponse = await _getAuditLog.SendGetRequestForGetAuditLogAsync(_requestHeader!, 
                planCreateResponse.InstallmentPlan.InstallmentPlanNumber);
            Assert.That(await _getAuditLog.ValidateAuditLogExpectedActivityWithPolling(auditLogResponse, 
                "Charge", _requestHeader!,
                planCreateResponse.InstallmentPlan.InstallmentPlanNumber));
            Console.WriteLine("TestValidatePaySafeDirectCaptureWebhookNotifications is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidatePaySafeDirectCaptureWebhookNotifications \n" + exception + "\n");
        }
    }
    
    [TestCase(Category = "GatewaysNotificationsTests")]
    [Test(Description = "ReachCaptureWebhookNotifications")]
    public async Task TestValidateReachCaptureWebhookNotifications()
    {
        try
        {
            Console.WriteLine("\nStarting TestValidateReachCaptureWebhookNotifications");
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            var planCreateResponse = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!,  _requestHeader!, "Active", 
                new Random().Next(4, 6), _envConfig.ReachTerminal, createPlanDefaultValues);
            var idempotecnyKey = await _getPgtl.ValidatePgtlKeyValueInnerAsync(_envConfig.StoreProcedureUrl, 
                _requestHeader!, planCreateResponse.InstallmentPlanNumber, "Type", "Capture", "IdempotencyKey");
            var getNotificationsListResponse = await _getList.SendGetRequestGetNotificationsListAsync(_requestHeader!,
                    "IdempotecnyKey", idempotecnyKey!);
            Assert.That(_getList.ValidateGatewayEventType(getNotificationsListResponse, "ORDER_PROCESSED"));
            Console.WriteLine("TestValidateReachCaptureWebhookNotifications is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidateReachCaptureWebhookNotifications \n" + exception + "\n");
        }
    }
    
    [TestCase(Category = "GatewaysNotificationsTests")]
    [Test(Description = "ReachRefundWebhookNotifications")]
    public async Task TestValidateReachRefundWebhookNotifications()
    {
        try
        {
            Console.WriteLine("\nStarting TestValidateReachRefundWebhookNotifications");
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            var planCreateResponse = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!,  _requestHeader!, "Active", 
                new Random().Next(4, 6), _envConfig.ReachTerminal, createPlanDefaultValues);
            await Task.Delay(300 * 1000);
            await _installmentPlanNumberRefund.SendRefundRequestAsync( _requestHeader!, 
                planCreateResponse.InstallmentPlanNumber, "FullRefund");
            var auditLogResponse = await _getAuditLog.SendGetRequestForGetAuditLogAsync(_requestHeader!, 
                planCreateResponse.InstallmentPlanNumber);
            Assert.That(await _getAuditLog.ValidateAuditLogExpectedActivityWithPolling(auditLogResponse, 
                "RefundReceived", _requestHeader!,
                planCreateResponse.InstallmentPlanNumber));
            Console.WriteLine("TestValidateReachRefundWebhookNotifications is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidateReachRefundWebhookNotifications \n" + exception + "\n");
        }
    }
    
    [TestCase(Category = "GatewaysNotificationsTests")]
    [Test(Description = "StripeDirectCaptureWebhookNotifications")]
    public async Task TestValidateStripeDirectCaptureWebhookNotifications()
    {
        try
        {
            Console.WriteLine("\nStarting TestValidateStripeDirectCaptureWebhookNotifications");
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            var planCreateResponse = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!,  _requestHeader!, "Active", 
                new Random().Next(4, 6), _envConfig.StripeDirectTerminal, createPlanDefaultValues);
            var idempotecnyKey = await _getPgtl.ValidatePgtlKeyValueInnerAsync(_envConfig.StoreProcedureUrl, 
                _requestHeader!, planCreateResponse.InstallmentPlanNumber, "Type", "Capture", "IdempotencyKey");
            var getNotificationsListResponse = await _getList.SendGetRequestGetNotificationsListAsync(_requestHeader!,
                    "IdempotecnyKey", idempotecnyKey!);
            Assert.That(_getList.ValidateGatewayEventType(getNotificationsListResponse, "charge.captured"));
            Console.WriteLine("TestValidateStripeDirectCaptureWebhookNotifications is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidateStripeDirectCaptureWebhookNotifications \n" + exception + "\n");
        }
    }
    
    [TestCase(Category = "GatewaysNotificationsTests")]
    [Test(Description = "StripeDirectRefundWebhookNotifications")]
    public async Task TestValidateStripeDirectRefundWebhookNotifications()
    {
        try
        {
            Console.WriteLine("\nStarting TestValidateStripeDirectRefundWebhookNotifications");
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            var planCreateResponse = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!,  _requestHeader!, "Active", 
                new Random().Next(4, 6), _envConfig.StripeDirectTerminal, createPlanDefaultValues);
            await Task.Delay(10 * 1000);
            await _installmentPlanNumberRefund.SendRefundRequestAsync( _requestHeader!, 
                planCreateResponse.InstallmentPlanNumber, "FullRefund");
            var idempotecnyKey = await _getPgtl.ValidatePgtlKeyValueInnerAsync(_envConfig.StoreProcedureUrl, 
                _requestHeader!, planCreateResponse.InstallmentPlanNumber, "Type", "Refund", "IdempotencyKey");
            var getNotificationsListResponse = await _getList.SendGetRequestGetNotificationsListAsync(_requestHeader!,
                "IdempotecnyKey", idempotecnyKey!);
            Assert.That(_getList.ValidateGatewayEventType(getNotificationsListResponse, "charge.refunded"));
            Console.WriteLine("TestValidateStripeDirectRefundWebhookNotifications is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidateStripeDirectRefundWebhookNotifications \n" + exception + "\n");
        }
    }
    
    [TestCase(Category = "GatewaysNotificationsTests")]
    [Test(Description = "MockerV2CaptureWebhookNotifications")]
    public async Task TestValidateMockerV2CaptureWebhookNotifications()
    {
        try
        {
            Console.WriteLine("\nStarting TestValidateMockerV2CaptureWebhookNotifications");
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            var planCreateResponse = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!,  _requestHeader!, "Active", 
                new Random().Next(4, 6), _envConfig.MockerV2Terminal, createPlanDefaultValues);
            var auditLogResponse = await _getAuditLog.SendGetRequestForGetAuditLogAsync(_requestHeader!, 
                planCreateResponse.InstallmentPlanNumber);
            var captureResponse = await _chargeFunctionality.SendPostRequestChargeFunctionalityAsync(_requestHeader!,
                    planCreateResponse.InstallmentPlanNumber);
            Assert.That(captureResponse.ResponseHeader.Succeeded);
            Assert.That(await _getAuditLog.ValidateAuditLogExpectedActivityWithPolling(auditLogResponse, 
                "Charge", _requestHeader!,
                planCreateResponse.InstallmentPlanNumber));
            Console.WriteLine("TestValidateMockerV2CaptureWebhookNotifications is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidateMockerV2CaptureWebhookNotifications \n" + exception + "\n");
        }
    }
    
    [TestCase(Category = "GatewaysNotificationsTests")]
    [Test(Description = "MockerV2RefundWebhookNotifications")]
    public async Task TestValidateMockerV2RefundWebhookNotifications()
    {
        try
        {
            Console.WriteLine("\nStarting TestValidateMockerV2RefundWebhookNotifications");
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            var planCreateResponse = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!,  _requestHeader!, "Active", 
                new Random().Next(4, 6), _envConfig.MockerV2Terminal, createPlanDefaultValues);
            await Task.Delay(10 * 1000);
            await _installmentPlanNumberRefund.SendRefundRequestAsync( _requestHeader!, 
                planCreateResponse.InstallmentPlanNumber, "FullRefund");
            var idempotecnyKey = await _getPgtl.ValidatePgtlKeyValueInnerAsync(_envConfig.StoreProcedureUrl, 
                _requestHeader!, planCreateResponse.InstallmentPlanNumber, "Type", "Refund", "IdempotencyKey");
            var getNotificationsListResponse = await _getList.SendGetRequestGetNotificationsListAsync(_requestHeader!,
                "IdempotecnyKey", idempotecnyKey!);
            Assert.That(_getList.ValidateGatewayEventType(getNotificationsListResponse, "Refund_Succeeded"));
            Console.WriteLine("TestValidateMockerV2RefundWebhookNotifications is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidateMockerV2RefundWebhookNotifications \n" + exception + "\n");
        }
    }
    
    [TestCase(Category = "GatewaysNotificationsTests")]
    [Test(Description = "MockerV2FailedCaptureWebhookNotifications")]
    public async Task TestValidateMockerV2FailedCaptureWebhookNotifications()
    {
        try
        {
            Console.WriteLine("\nStarting TestValidateMockerV2FailedCaptureWebhookNotifications");
            var v1InitiateDefaultValues = new V1InitiateDefaultValues
            {
                BillingAddress =
                {
                    addressLine = "Automation-Charge1"
                },
                planData =
                {
                    numberOfInstallments = 4
                }
            };
            var planCreateResponse = await _create.CreatePlanAsync(_requestHeader!, v1InitiateDefaultValues, 
                _envConfig.SplititMockTerminal);
            await Task.Delay(2 * 1000);
            var chargeResponse = await _chargeFunctionality.SendPostRequestChargeFunctionalityAsync(_requestHeader!,
                planCreateResponse.InstallmentPlan.InstallmentPlanNumber, true);
            Console.WriteLine("Validating capture failed");
            Assert.That(chargeResponse.Errors.Count > 0);
            Console.WriteLine("Validating capture failed is done");
            var idempotecnyKey = await _getPgtl.ValidatePgtlKeyValueInnerAsync(_envConfig.StoreProcedureUrl, 
                _requestHeader!, planCreateResponse.InstallmentPlan.InstallmentPlanNumber, "ResultMessageMessageCode", 
                "Automation-Charge1", "IdempotencyKey");
            Assert.That(idempotecnyKey, Is.Not.Null);
            Console.WriteLine("TestValidateMockerV2FailedCaptureWebhookNotifications is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidateMockerV2FailedCaptureWebhookNotifications \n" + exception + "\n");
        }
    }
    
    [TestCase(Category = "GatewaysNotificationsTests")]
    [Test(Description = "MockerV2FailedRefundWebhookNotifications")]
    public async Task TestValidateMockerV2FailedRefundWebhookNotifications()
    {
        try
        {
            Console.WriteLine("\nStarting TestValidateMockerV2FailedRefundWebhookNotifications");
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            createPlanDefaultValues.billingAddress = new BillingAddressDefaultValues
            {
                addressLine1 = "Configuration-2"
            };
            var planCreateResponse = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!,  _requestHeader!, "Active", 
                new Random().Next(4, 6), _envConfig.MockerV2Terminal, createPlanDefaultValues);
            await _installmentPlanNumberRefund.SendRefundRequestAsync( _requestHeader!, 
                planCreateResponse.InstallmentPlanNumber, "FullRefund");
            var idempotecnyKey = await _getPgtl.ValidatePgtlKeyValueInnerAsync(_envConfig.StoreProcedureUrl, 
                _requestHeader!, planCreateResponse.InstallmentPlanNumber, "ResultMessageMessageCode", 
                "SomeRefundErrorCode", "IdempotencyKey");
            Assert.That(idempotecnyKey, Is.Not.Null);
            Console.WriteLine("TestValidateMockerV2FailedRefundWebhookNotifications is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidateMockerV2FailedRefundWebhookNotifications \n" + exception + "\n");
        }
    }
}