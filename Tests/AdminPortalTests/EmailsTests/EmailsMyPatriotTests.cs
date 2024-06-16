using Allure.NUnit;
using Allure.NUnit.Attributes;
using NUnit.Framework;
using Splitit.Automation.NG.Backend.BaseActions;
using Splitit.Automation.NG.Backend.Services.AdminApi.InstallmentPlan.InstallmentsPlanFunctionality;
using Splitit.Automation.NG.Backend.Services.AdminApi.Merchant.MerchantFunctionality;
using Splitit.Automation.NG.Backend.Services.AdminPortal.Logs.Functionality;
using Splitit.Automation.NG.Backend.Services.V1.DefaultValues;
using Splitit.Automation.NG.Backend.Services.V1.InstallmentPlan.InstallmentPlanApiEndPoints;
using Splitit.Automation.NG.Backend.Services.V3.DefaultValues;
using Splitit.Automation.NG.Backend.Services.V3.InstallmentPlan.InstallmentPlanApiEndPoint;
using Splitit.Automation.NG.Backend.Tests.TestsSetups;
using Create = Splitit.Automation.NG.Backend.Services.V1.InstallmentPlan.InstallmentPlanApiEndPoints;

namespace Splitit.Automation.NG.Backend.Tests.AdminPortalTests.EmailsTests;

[TestFixture]
[AllureNUnit]
[AllureSuite("EmailsMyPatriotTests")]
[AllureDisplayIgnored]
//[Parallelizable(ParallelScope.All)]
public class EmailsMyPatriotTests
{
    private RequestHeader? _requestHeader;
    private readonly InstallmentPlans _installmentPlans;
    private readonly GetEmailsFunctionality _getEmailsFunctionality;
    private readonly EmailsController _emailsController;
    private readonly ChargeFunctionality _chargeFunctionality;
    private readonly Create.Create _create;
    private readonly Settings _settings;
    private readonly SettingsSave _settingsSave;
    private readonly ReAuthFunctionality _reAuthFunctionality;
    private readonly RetryFunctionality _retryFunctionality;
    private readonly FullCaptureFunctionality _fullCaptureFunctionality;
    private readonly InstallmentPlanNumberCancel _installmentPlanNumberCancel;
    private readonly RequestPayment _requestPayment;
    private readonly InstallmentPlanNumberRefund _installmentPlanNumberRefund;
    
    public EmailsMyPatriotTests()
    {
        Console.WriteLine("Staring EmailsShoppersTests Setup");
        _installmentPlans = new InstallmentPlans();
        _getEmailsFunctionality = new GetEmailsFunctionality();
        _emailsController = new EmailsController();
        _chargeFunctionality = new ChargeFunctionality();
        _create = new Create.Create();
        _settings = new Settings();
        _settingsSave = new SettingsSave();
        _reAuthFunctionality = new ReAuthFunctionality();
        _retryFunctionality = new RetryFunctionality();
        _fullCaptureFunctionality = new FullCaptureFunctionality();
        _installmentPlanNumberCancel = new InstallmentPlanNumberCancel();
        _requestPayment = new RequestPayment();
        _installmentPlanNumberRefund = new InstallmentPlanNumberRefund();
        Console.WriteLine("EmailsShoppersTests Setup Succeeded\n");
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
    
    [Category("EmailsTests")]
    [Test(Description = "TestValidateSubjectPaymentPlanCreatedEmailSent"), CancelAfter(240*1000)]
    public async Task TestValidateSubjectPaymentPlanCreatedEmailSent()
    {
        try
        {
            Console.WriteLine("\nStarting TestValidateSubjectPaymentPlanCreatedEmailSent");
            var v1InitiateDefaultValues = new V1InitiateDefaultValues();
            var planCreateResponse = await _create.CreatePlanAsync( _requestHeader!, 
                v1InitiateDefaultValues, Environment.GetEnvironmentVariable("My_Patriot_SupplyTerminal")!);
            Assert.That(planCreateResponse.InstallmentPlan, Is.Not.Null);
            var jResponse = await _getEmailsFunctionality.SendGetRequestGetEmailsAsync(
                 _requestHeader!, "InstallmentPlanNumber", 
                planCreateResponse.InstallmentPlan.InstallmentPlanNumber);
            Assert.That(_emailsController.ValidateEmailType(jResponse, 
                planCreateResponse.InstallmentPlan.InstallmentPlanNumber,
                "PlanApproval_Succeeded"));
            Console.WriteLine("TestValidateSubjectPaymentPlanCreatedEmailSent is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidateSubjectPaymentPlanCreatedEmailSent\n" + exception + "\n");
        }
    }
    
    [Category("EmailsTests")]
    [Test(Description = "TestValidateSubjectPlanCompletedEmailSent"), CancelAfter(240*1000)]
    public async Task TestValidateSubjectPlanCompletedEmailSent()
    {
        try
        {
            Console.WriteLine("\nStarting TestValidateSubjectPlanCompletedEmailSent");
            var v1InitiateDefaultValues = new V1InitiateDefaultValues
            {
                planData =
                {
                    numberOfInstallments = 2
                }
            };
            var planCreateResponse = await _create.CreatePlanAsync( _requestHeader!, 
                v1InitiateDefaultValues, Environment.GetEnvironmentVariable("My_Patriot_SupplyTerminal")!);
            Assert.That(planCreateResponse.InstallmentPlan, Is.Not.Null);
            var jResponseCharge = await _chargeFunctionality.SendPostRequestChargeFunctionalityAsync(
                 _requestHeader!, planCreateResponse.InstallmentPlan.InstallmentPlanNumber);
            Assert.That(jResponseCharge.Succeeded, Is.False);
            var jResponse = await _getEmailsFunctionality.SendGetRequestGetEmailsAsync(
                 _requestHeader!, "InstallmentPlanNumber", 
                planCreateResponse.InstallmentPlan.InstallmentPlanNumber);
            Assert.That(_emailsController.ValidateEmailType(jResponse, 
                planCreateResponse.InstallmentPlan.InstallmentPlanNumber,
                "Cleared"));
            Console.WriteLine("TestValidateSubjectPlanCompletedEmailSent is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidateSubjectPlanCompletedEmailSent\n" + exception + "\n");
        }
    }
    
    [Category("EmailsTests")]
    [Test(Description = "TestValidateSubjectFullCaptureEmailSent"), CancelAfter(240*1000)]
    public async Task TestValidateSubjectFullCaptureEmailSent()
    {
        try
        {
            Console.WriteLine("\nStarting TestValidateSubjectFullCaptureEmailSent");
            var v1InitiateDefaultValues = new V1InitiateDefaultValues();
            var planCreateResponse = await _create.CreatePlanAsync( _requestHeader!, 
                v1InitiateDefaultValues, Environment.GetEnvironmentVariable("My_Patriot_SupplyTerminal")!);
            Assert.That(planCreateResponse.InstallmentPlan, Is.Not.Null);
            var jResponseFullCapture = await _fullCaptureFunctionality.SendPostRequestFullCaptureAsync(
                 _requestHeader!, planCreateResponse.InstallmentPlan.InstallmentPlanNumber, 0);
            Assert.That(jResponseFullCapture.ResponseHeader.Succeeded);
            var jResponse = await _getEmailsFunctionality.SendGetRequestGetEmailsAsync(
                 _requestHeader!, "InstallmentPlanNumber", 
                planCreateResponse.InstallmentPlan.InstallmentPlanNumber);
            Assert.That(_emailsController.ValidateEmailType(jResponse, 
                planCreateResponse.InstallmentPlan.InstallmentPlanNumber,
                "FullCapture_Succeeded"));
            Console.WriteLine("TestValidateSubjectFullCaptureEmailSent is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidateSubjectFullCaptureEmailSent\n" + exception + "\n");
        }
    }
    
    [Category("EmailsTests")]
    [Test(Description = "TestValidateCreateValidationEmailSent"), CancelAfter(240*1000)]
    public async Task TestValidateCreateValidationEmailSent()
    {
        try
        {
            Console.WriteLine("\nStarting TestValidateCreateValidationEmailSent");
            var v1InitiateDefaultValues = new V1InitiateDefaultValues();
            var planCreateResponse = await _create.CreatePlanAsync( _requestHeader!, 
                v1InitiateDefaultValues, Environment.GetEnvironmentVariable("My_Patriot_SupplyTerminal")!);
            Assert.That(planCreateResponse.InstallmentPlan, Is.Not.Null);
            var jResponse = await _getEmailsFunctionality.SendGetRequestGetEmailsAsync(
                 _requestHeader!, "InstallmentPlanNumber", 
                planCreateResponse.InstallmentPlan.InstallmentPlanNumber);
            Assert.That(_emailsController.ValidateEmailType(jResponse, 
                planCreateResponse.InstallmentPlan.InstallmentPlanNumber,
                "PlanApproval_Succeeded"));
            Console.WriteLine("TestValidateCreateValidationEmailSent is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidateCreateValidationEmailSent\n" + exception + "\n");
        }
    }
    
    [Category("EmailsTests")]
    [Test(Description = "TestValidateCancelPlanEmailSent"), CancelAfter(240*1000)]
    public async Task TestValidateCancelPlanEmailSent()
    {
        try
        {
            Console.WriteLine("\nStarting TestValidateCancelPlanEmailSent");
            var v1InitiateDefaultValues = new V1InitiateDefaultValues();
            var planCreateResponse = await _create.CreatePlanAsync( _requestHeader!, 
                v1InitiateDefaultValues, Environment.GetEnvironmentVariable("My_Patriot_SupplyTerminal")!);
            Assert.That(planCreateResponse.InstallmentPlan, Is.Not.Null);
            await Task.Delay(10 * 1000);
            var jResponseCancel = await _installmentPlanNumberCancel.SendCancelPlanRequestAsync(
                 _requestHeader!, planCreateResponse.InstallmentPlan.InstallmentPlanNumber, 0,
                "NoRefunds");
            Assert.That(jResponseCancel.ResponseHeader.Succeeded);
            var jResponse = await _getEmailsFunctionality.SendGetRequestGetEmailsAsync(
                 _requestHeader!, "InstallmentPlanNumber", 
                planCreateResponse.InstallmentPlan.InstallmentPlanNumber);
            Assert.That(_emailsController.ValidateEmailType(jResponse, 
                planCreateResponse.InstallmentPlan.InstallmentPlanNumber,
                "InstallmentPlan_Cleared"));
            Console.WriteLine("TestValidateCancelPlanEmailSent is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidateCancelPlanEmailSent\n" + exception + "\n");
        }
    }
    
    [Category("EmailsTests")]
    [Test(Description = "TestValidatePartialRefundPastChargeEmailSent"), CancelAfter(240*1000)]
    public async Task TestValidatePartialRefundPastChargeEmailSent()
    {
        try
        {
            Console.WriteLine("\nStarting TestValidatePartialRefundPastChargeEmailSent");
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            var planCreateResponse = await _installmentPlans.CreatePlanAsync(
                Environment.GetEnvironmentVariable("ApiV3")!,  _requestHeader!,
                "Active", new Random().Next(4, 12), 
                Environment.GetEnvironmentVariable("My_Patriot_SupplyTerminal")!, createPlanDefaultValues);
            var jResponseCharge = await _chargeFunctionality.SendPostRequestChargeFunctionalityAsync(
                 _requestHeader!, planCreateResponse.InstallmentPlanNumber);
            Assert.That(jResponseCharge.ResponseHeader.Succeeded);
            var jResponseRefund = await _installmentPlanNumberRefund.SendRefundRequestAsync(
                 _requestHeader!, planCreateResponse.InstallmentPlanNumber,
                "PartialRefund", "FutureInstallmentsNotAllowed");
            Assert.That(jResponseRefund.RefundId, Is.Not.Null);
            var jResponse = await _getEmailsFunctionality.SendGetRequestGetEmailsAsync(
                 _requestHeader!, "InstallmentPlanNumber", 
                planCreateResponse.InstallmentPlanNumber);
            Assert.That(_emailsController.ValidateEmailType(jResponse, 
                planCreateResponse.InstallmentPlanNumber, "PlanApproval"));
            Console.WriteLine("TestValidatePartialRefundPastChargeEmailSent is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidatePartialRefundPastChargeEmailSent\n" + exception + "\n");
        }
    }
    
    [Category("EmailsTests")]
    [Test(Description = "TestValidatePartialRefundFutureChargeEmailSent"), CancelAfter(240*1000)]
    public async Task TestValidatePartialRefundFutureChargeEmailSent()
    {
        try
        {
            Console.WriteLine("\nStarting TestValidatePartialRefundFutureChargeEmailSent");
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            var planCreateResponse = await _installmentPlans.CreatePlanAsync(
                Environment.GetEnvironmentVariable("ApiV3")!,  _requestHeader!,
                "Active", new Random().Next(4, 12), 
                Environment.GetEnvironmentVariable("My_Patriot_SupplyTerminal")!, createPlanDefaultValues);
            var jResponseCharge = await _chargeFunctionality.SendPostRequestChargeFunctionalityAsync(
                 _requestHeader!, planCreateResponse.InstallmentPlanNumber);
            Assert.That(jResponseCharge.ResponseHeader.Succeeded);
            var jResponseRefund = await _installmentPlanNumberRefund.SendRefundRequestAsync(
                 _requestHeader!, planCreateResponse.InstallmentPlanNumber,
                "PartialRefund", "ReduceFromLastInstallment");
            Assert.That(jResponseRefund.RefundId, Is.Not.Null);
            var jResponse = await _getEmailsFunctionality.SendGetRequestGetEmailsAsync(
                 _requestHeader!, "InstallmentPlanNumber", 
                planCreateResponse.InstallmentPlanNumber);
            Assert.That(_emailsController.ValidateEmailType(jResponse, 
                planCreateResponse.InstallmentPlanNumber, "PlanApproval"));
            Console.WriteLine("TestValidatePartialRefundFutureChargeEmailSent is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidatePartialRefundFutureChargeEmailSent\n" + exception + "\n");
        }
    }
    
    [Category("EmailsTests")]
    [Test(Description = "TestValidatePartialRefundOutstandingAmountEmailSent"), CancelAfter(240*1000)]
    public async Task TestValidatePartialRefundOutstandingAmountEmailSent()
    {
        try
        {
            Console.WriteLine("\nStarting TestValidatePartialRefundOutstandingAmountEmailSent");
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            var planCreateResponse = await _installmentPlans.CreatePlanAsync(
                Environment.GetEnvironmentVariable("ApiV3")!,  _requestHeader!,
                "Active", new Random().Next(4, 12), 
                Environment.GetEnvironmentVariable("My_Patriot_SupplyTerminal")!, createPlanDefaultValues);
            var jResponseCharge = await _chargeFunctionality.SendPostRequestChargeFunctionalityAsync(
                 _requestHeader!, planCreateResponse.InstallmentPlanNumber);
            Assert.That(jResponseCharge.ResponseHeader.Succeeded);
            var jResponseRefund = await _installmentPlanNumberRefund.SendRefundRequestAsync(
                 _requestHeader!, planCreateResponse.InstallmentPlanNumber,
                "PartialRefund");
            Assert.That(jResponseRefund.RefundId, Is.Not.Null);
            var jResponse = await _getEmailsFunctionality.SendGetRequestGetEmailsAsync(
                 _requestHeader!, "InstallmentPlanNumber", 
                planCreateResponse.InstallmentPlanNumber);
            Assert.That(_emailsController.ValidateEmailType(jResponse, 
                planCreateResponse.InstallmentPlanNumber, "PlanApproval"));
            Console.WriteLine("TestValidatePartialRefundOutstandingAmountEmailSent is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidatePartialRefundOutstandingAmountEmailSent\n" + exception + "\n");
        }
    }
    
    [Category("EmailsTests")]
    [Test(Description = "TestValidateRequestPaymentEmailSent"), CancelAfter(240*1000)]
    public async Task TestValidateRequestPaymentEmailSent()
    {
        try
        {
            Console.WriteLine("\nStarting TestValidateRequestPaymentEmailSent");
            var v1InitiateDefaultValues = new V1InitiateDefaultValues
            {
                RequestHeader =
                {
                    apiKey = Environment.GetEnvironmentVariable("My_Patriot_SupplyTerminal")!
                }
            };
            var planCreateResponse = await _create.CreatePlanInitiateAsync( _requestHeader!, 
                v1InitiateDefaultValues);
            Assert.That(planCreateResponse.InstallmentPlan, Is.Not.Null);
            var jResponseRequestPayment = await _requestPayment.SendPostRequestRequestPaymentAsync(
                 _requestHeader!, planCreateResponse.InstallmentPlan.InstallmentPlanNumber, 
                "Splitit.Automation@splitit.com", "VposCheckOut",
                "2.0");
            Assert.That(jResponseRequestPayment.ResponseHeader.Succeeded);
            var jResponse = await _getEmailsFunctionality.SendGetRequestGetEmailsAsync(
                 _requestHeader!, "InstallmentPlanNumber",
                planCreateResponse.InstallmentPlan.InstallmentPlanNumber);
            Assert.That(_emailsController.ValidateEmailType(jResponse, planCreateResponse.InstallmentPlan.InstallmentPlanNumber,
                "RequestPayment"));
            Console.WriteLine("TestValidateRequestPaymentEmailSent is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidateRequestPaymentEmailSent\n" + exception + "\n");
        }
    }
    
    [Category("EmailsTests")]
    [Test(Description = "TestValidateRequestUpdateCcEmailSent"), CancelAfter(240*1000)]
    public async Task TestValidateRequestUpdateCcEmailSent()
    {
        try
        {
            Console.WriteLine("\nStarting TestValidateRequestUpdateCcEmailSent");
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            var planCreateResponse = await _installmentPlans.CreatePlanAsync(
                Environment.GetEnvironmentVariable("ApiV3")!,  _requestHeader!,
                "Active", new Random().Next(4, 12), 
                Environment.GetEnvironmentVariable("My_Patriot_SupplyTerminal")!, createPlanDefaultValues);
            var jResponseCharge = await _chargeFunctionality.SendPostRequestChargeFunctionalityAsync(
                 _requestHeader!, planCreateResponse.InstallmentPlanNumber);
            Assert.That(jResponseCharge.ResponseHeader.Succeeded);
            var jResponse = await _getEmailsFunctionality.SendGetRequestGetEmailsAsync(
                 _requestHeader!, "InstallmentPlanNumber",
                planCreateResponse.InstallmentPlanNumber);
            Assert.That(_emailsController.ValidateEmailType(jResponse,
                planCreateResponse.InstallmentPlanNumber, "PlanApproval"));
            Console.WriteLine("TestValidateRequestUpdateCcEmailSent is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidateRequestUpdateCcEmailSent\n" + exception + "\n");
        }
    }
    
    [Category("EmailsTests")]
    [Test(Description = "TestValidateReAuthFailureEmailSent"), CancelAfter(240*1000)]
    public async Task TestValidateReAuthFailureEmailSent()
    {
        try
        {
            Console.WriteLine("\nStarting TestValidateReAuthFailureEmailSent");
            var jResponseMerchantSettings = await _settings.SendGetSettingsRequestAsync(
                _requestHeader!, int.Parse(Environment.GetEnvironmentVariable("MY_PATRIOT_SUPPLY_MERCHANT_ID")!));
            jResponseMerchantSettings!.MerchantSettings.RunCaptureAsync = false;
            var jResponseUpdateSettings = await _settingsSave.SendUpdateSettingsRequestAsync(
                _requestHeader!, jResponseMerchantSettings);
            Assert.That(jResponseUpdateSettings.MerchantSettings.RunCaptureAsync, Is.False);
            var v1InitiateDefaultValues = new V1InitiateDefaultValues
            {
                BillingAddress =
                {
                    addressLine = "Automation-Auth2"
                }
            };
            var planCreateResponse = await _create.CreatePlanAsync( _requestHeader!, 
                v1InitiateDefaultValues, Environment.GetEnvironmentVariable("My_Patriot_SupplyTerminal")!);
            Assert.That(planCreateResponse.InstallmentPlan, Is.Not.Null);
            var jResponseReAuth = await _reAuthFunctionality.SendPostRequestReAuthAsync(
                _requestHeader!, planCreateResponse.InstallmentPlan.InstallmentPlanNumber, "yes");
            Assert.That(jResponseReAuth.Succeeded, Is.False);
            var jResponse = await _getEmailsFunctionality.SendGetRequestGetEmailsAsync(
                 _requestHeader!, "InstallmentPlanNumber", 
                planCreateResponse.InstallmentPlan.InstallmentPlanNumber);
            Assert.That(_emailsController.ValidateEmailType(
                jResponse, planCreateResponse.InstallmentPlan.InstallmentPlanNumber,
                "Rejection"));
            Console.WriteLine("TestValidateReAuthFailureEmailSent is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidateReAuthFailureEmailSent\n" + exception + "\n");
        }
    }
    
    [Category("EmailsTests")]
    [Test(Description = "TestValidateRetryReAuthFailureEmailSent"), CancelAfter(240*1000)]
    public async Task TestValidateRetryReAuthFailureEmailSent()
    {
        try
        {
            Console.WriteLine("\nStarting TestValidateRetryReAuthFailureEmailSent");
            var jResponseMerchantSettings = await _settings.SendGetSettingsRequestAsync(
                _requestHeader!, int.Parse(Environment.GetEnvironmentVariable("MY_PATRIOT_SUPPLY_MERCHANT_ID")!));
            jResponseMerchantSettings!.MerchantSettings.RunCaptureAsync = false;
            var jResponseUpdateSettings = await _settingsSave.SendUpdateSettingsRequestAsync(
                _requestHeader!, jResponseMerchantSettings);
            Assert.That(jResponseUpdateSettings.MerchantSettings.RunCaptureAsync, Is.False);
            var v1InitiateDefaultValues = new V1InitiateDefaultValues
            {
                BillingAddress =
                {
                    addressLine = "Automation-Auth2"
                }
            };
            var planCreateResponse = await _create.CreatePlanAsync( _requestHeader!, 
                v1InitiateDefaultValues, Environment.GetEnvironmentVariable("My_Patriot_SupplyTerminal")!);
            Assert.That(planCreateResponse.InstallmentPlan, Is.Not.Null);
            var jResponseReAuth = await _reAuthFunctionality.SendPostRequestReAuthAsync(
                _requestHeader!, planCreateResponse.InstallmentPlan.InstallmentPlanNumber, "yes");
            Assert.That(jResponseReAuth.Succeeded, Is.False);
            var jResponseRetry = await _retryFunctionality.SendPostRequestRetryFunctionalityAsync(
                 _requestHeader!, planCreateResponse.InstallmentPlan.InstallmentPlanNumber);
            Assert.That(jResponseRetry.Succeeded, Is.False);
            var jResponse = await _getEmailsFunctionality.SendGetRequestGetEmailsAsync(
                 _requestHeader!, "InstallmentPlanNumber", 
                planCreateResponse.InstallmentPlan.InstallmentPlanNumber);
            Assert.That(_emailsController.ValidateEmailType(
                jResponse, planCreateResponse.InstallmentPlan.InstallmentPlanNumber,
                "Retry_Failed"));
            Console.WriteLine("TestValidateRetryReAuthFailureEmailSent is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidateRetryReAuthFailureEmailSent\n" + exception + "\n");
        }
    }
}