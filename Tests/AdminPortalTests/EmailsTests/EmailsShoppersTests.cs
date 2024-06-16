using Allure.NUnit;
using Allure.NUnit.Attributes;
using NUnit.Framework;
using Splitit.Automation.NG.Backend.BaseActions;
using Splitit.Automation.NG.Backend.Services.AdminApi.InstallmentPlan.InstallmentsPlanFunctionality;
using Splitit.Automation.NG.Backend.Services.AdminApi.Merchant.MerchantFunctionality;
using Splitit.Automation.NG.Backend.Services.AdminPortal.Logs.Functionality;
using Splitit.Automation.NG.Backend.Services.MerchantPortal.InstallmentPlan.Functionality;
using Splitit.Automation.NG.Backend.Services.V1.DefaultValues;
using Splitit.Automation.NG.Backend.Services.V1.InstallmentPlan.InstallmentPlanApiEndPoints;
using Splitit.Automation.NG.Backend.Services.V3.DefaultValues;
using Splitit.Automation.NG.Backend.Services.V3.InstallmentPlan.InstallmentPlanApiEndPoint;
using Splitit.Automation.NG.Backend.Services.WebApi.InstallmentPlan.Functionality;
using Splitit.Automation.NG.Backend.Tests.TestsSetups;
using Create = Splitit.Automation.NG.Backend.Services.V1.InstallmentPlan.InstallmentPlanApiEndPoints.Create;

namespace Splitit.Automation.NG.Backend.Tests.AdminPortalTests.EmailsTests;

[TestFixture]
[AllureNUnit]
[AllureSuite("EmailsShoppersTests")]
[AllureDisplayIgnored]
[Parallelizable(ParallelScope.All)]
public class EmailsShoppersTests
{
    private RequestHeader? _requestHeader;
    private readonly InstallmentPlans _installmentPlans;
    private readonly GetEmailsFunctionality _getEmailsFunctionality;
    private readonly EmailsController _emailsController;
    private readonly ChargeFunctionality _chargeFunctionality;
    private readonly Create _create;
    private readonly Settings _settings;
    private readonly SettingsSave _settingsSave;
    private readonly ReAuthFunctionality _reAuthFunctionality;
    private readonly RetryFunctionality _retryFunctionality;
    private readonly FullCaptureFunctionality _fullCaptureFunctionality;
    private readonly RefundFunctionality _refundFunctionality;
    private readonly InstallmentPlanNumberCancel _installmentPlanNumberCancel;
    private readonly PostTermsAndConditionsFunctionality _postTermsAndConditionsFunctionality;
    private readonly RequestUpdateCardFunctionality _requestUpdateCardFunctionality;
    private readonly RequestPayment _requestPayment;
    private readonly ReceiptFunctionality _receiptFunctionality;
    private readonly S3Controller _s3Controller;
    private readonly ConvertPdfToStringController _convertPdfToStringController;
    
    public EmailsShoppersTests()
    {
        Console.WriteLine("Staring EmailsShoppersTests Setup");
        _installmentPlans = new InstallmentPlans();
        _getEmailsFunctionality = new GetEmailsFunctionality();
        _emailsController = new EmailsController();
        _chargeFunctionality = new ChargeFunctionality();
        _create = new Create();
        _settings = new Settings();
        _settingsSave = new SettingsSave();
        _reAuthFunctionality = new ReAuthFunctionality();
        _retryFunctionality = new RetryFunctionality();
        _fullCaptureFunctionality = new FullCaptureFunctionality();
        _refundFunctionality = new RefundFunctionality();
        _installmentPlanNumberCancel = new InstallmentPlanNumberCancel();
        _postTermsAndConditionsFunctionality = new PostTermsAndConditionsFunctionality();
        _requestUpdateCardFunctionality = new RequestUpdateCardFunctionality();
        _requestPayment = new RequestPayment();
        _receiptFunctionality = new ReceiptFunctionality();
        _s3Controller = new S3Controller();
        _convertPdfToStringController = new ConvertPdfToStringController();
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
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            var planCreateResponse = await _installmentPlans.CreatePlanAsync(
                Environment.GetEnvironmentVariable("ApiV3")!, _requestHeader!, "Active",
                new Random().Next(2, 6), 
                Environment.GetEnvironmentVariable("SplititMockTerminal")!, createPlanDefaultValues);
            var jResponse = await _getEmailsFunctionality.SendGetRequestGetEmailsAsync(
                _requestHeader!, "InstallmentPlanNumber", planCreateResponse.InstallmentPlanNumber,
                true);
            Assert.That(_emailsController.ValidateEmailSubject(jResponse, planCreateResponse.InstallmentPlanNumber,
                "Payment plan created"));
            Console.WriteLine("TestValidateSubjectPaymentPlanCreatedEmailSent is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidateSubjectPaymentPlanCreatedEmailSent\n" + exception + "\n");
        }
    }
    
    [Category("EmailsTests")]
    [Test(Description = "TestValidateSubjectPaymentReceivedEmailSent"), CancelAfter(240*1000)]
    public async Task TestValidateSubjectPaymentReceivedEmailSent()
    {
        try
        {
            Console.WriteLine("\nStarting TestValidateSubjectPaymentReceivedEmailSent");
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            var planCreateResponse = await _installmentPlans.CreatePlanAsync(
                Environment.GetEnvironmentVariable("ApiV3")!, _requestHeader!, "Active",
                new Random().Next(4, 12), 
                Environment.GetEnvironmentVariable("SplititMockTerminal")!, createPlanDefaultValues);
            var jResponseCharge = await _chargeFunctionality.SendPostRequestChargeFunctionalityAsync(
                _requestHeader!, planCreateResponse.InstallmentPlanNumber);
            Assert.That(jResponseCharge.ResponseHeader.Succeeded);
            var jResponse = await _getEmailsFunctionality.SendGetRequestGetEmailsAsync(
                _requestHeader!, "InstallmentPlanNumber", planCreateResponse.InstallmentPlanNumber);
            Assert.That(_emailsController.ValidateEmailSubject(jResponse, planCreateResponse.InstallmentPlanNumber,
                "Payment received"));
            Console.WriteLine("TestValidateSubjectPaymentReceivedEmailSent is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidateSubjectPaymentReceivedEmailSent\n" + exception + "\n");
        }
    }
    
    [Category("EmailsTests")]
    [Test(Description = "TestValidateReAuthReminderSentEmailSent"), CancelAfter(300*1000)]
    public async Task TestValidateReAuthReminderSent()
    {
        try
        {
            Console.WriteLine("\nStarting TestValidateReAuthReminderSentEmailSent");
            var v1InitiateDefaultValues = new V1InitiateDefaultValues
            {
                planData =
                {
                    TestMode = "fast"
                },
                RequestHeader =
                {
                    apiKey = Environment.GetEnvironmentVariable("SplititMockTerminal")!
                }
            };
            var planCreateResponse = await _create.CreatePlanAsync( _requestHeader!, 
                v1InitiateDefaultValues);
            Assert.That(planCreateResponse.InstallmentPlan, Is.Not.Null);
            var jResponse = await _getEmailsFunctionality.SendGetRequestGetEmailsAsync(
                 _requestHeader!, "InstallmentPlanNumber", 
                planCreateResponse.InstallmentPlan.InstallmentPlanNumber);
            Assert.That(_emailsController.ValidateEmailType(jResponse, 
                planCreateResponse.InstallmentPlan.InstallmentPlanNumber, "PlanApproval"));
            Console.WriteLine("TestValidateReAuthReminderSentEmailSent is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidateReAuthReminderSentEmailSent\n" + exception + "\n");
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
                _requestHeader!, int.Parse(Environment.GetEnvironmentVariable("MerchantBusinessUnitId")!));
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
                v1InitiateDefaultValues, Environment.GetEnvironmentVariable("MockerV2Terminal")!);
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
    
    [Category("EmailsTests")]
    [Test(Description = "TestValidateFullCaptureEmailSent"), CancelAfter(240*1000)]
    public async Task TestValidateFullCaptureEmailSent()
    {
        try
        {
            Console.WriteLine("\nStarting TestValidateFullCaptureEmailSent");
            var jResponseMerchantSettings = await _settings.SendGetSettingsRequestAsync(
                _requestHeader!, int.Parse(Environment.GetEnvironmentVariable("MerchantBusinessUnitId")!));
            jResponseMerchantSettings!.MerchantSettings.RunCaptureAsync = false;
            var jResponseUpdateSettings = await _settingsSave.SendUpdateSettingsRequestAsync(
                _requestHeader!, jResponseMerchantSettings);
            Assert.That(jResponseUpdateSettings.MerchantSettings.RunCaptureAsync, Is.False);
            var v1InitiateDefaultValues = new V1InitiateDefaultValues();
            var planCreateResponse = await _create.CreatePlanAsync( _requestHeader!, 
                v1InitiateDefaultValues, Environment.GetEnvironmentVariable("MockerV2Terminal")!);
            Assert.That(planCreateResponse.InstallmentPlan, Is.Not.Null);
            var jResponseFullCapture = await _fullCaptureFunctionality.SendPostRequestFullCaptureAsync(
                _requestHeader!, planCreateResponse.InstallmentPlan.InstallmentPlanNumber, 0);
            Assert.That(jResponseFullCapture.ResponseHeader.Succeeded);
            var jResponse = await _getEmailsFunctionality.SendGetRequestGetEmailsAsync(
                 _requestHeader!, "InstallmentPlanNumber", 
                planCreateResponse.InstallmentPlan.InstallmentPlanNumber);
            Assert.That(_emailsController.ValidateEmailType(
                jResponse, planCreateResponse.InstallmentPlan.InstallmentPlanNumber,
                "FullCapture_Succeeded"));
            Console.WriteLine("TestValidateFullCaptureEmailSent is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidateFullCaptureEmailSent\n" + exception + "\n");
        }
    }
    
    [Category("EmailsTests")]
    [Test(Description = "TestValidateTestValidatePartialRefundFromPastInstallmentsEmailSent"), CancelAfter(240*1000)]
    public async Task TestValidatePartialRefundFromPastInstallmentsEmailSent()
    {
        try
        {
            Console.WriteLine("\nStarting TestValidatePartialRefundFromPastInstallmentsEmailSent");
            var jResponseMerchantSettings = await _settings.SendGetSettingsRequestAsync(
                _requestHeader!, int.Parse(Environment.GetEnvironmentVariable("MerchantBusinessUnitId")!));
            jResponseMerchantSettings!.MerchantSettings.RunCaptureAsync = false;
            var jResponseUpdateSettings = await _settingsSave.SendUpdateSettingsRequestAsync(
                _requestHeader!, jResponseMerchantSettings);
            Assert.That(jResponseUpdateSettings.MerchantSettings.RunCaptureAsync, Is.False);
            var v1InitiateDefaultValues = new V1InitiateDefaultValues
            {
                planData =
                {
                    numberOfInstallments = 10,
                    amount =
                    {
                        value = 1000
                    }
                }
            };
            var planCreateResponse = await _create.CreatePlanAsync( _requestHeader!, 
                v1InitiateDefaultValues, Environment.GetEnvironmentVariable("MockerV2Terminal")!);
            Assert.That(planCreateResponse.InstallmentPlan, Is.Not.Null);
            await Task.Delay(10 * 1000);
            var jResponseRefund = await _refundFunctionality.SendRefundRequestAsync(_requestHeader!,
                planCreateResponse.InstallmentPlan.InstallmentPlanNumber, "PartialRefund",
                "FutureInstallmentsNotAllowed");
            Assert.That(jResponseRefund.ResponseHeader.Succeeded, Is.True);
            var jResponse = await _getEmailsFunctionality.SendGetRequestGetEmailsAsync(
                 _requestHeader!, "InstallmentPlanNumber", 
                planCreateResponse.InstallmentPlan.InstallmentPlanNumber);
            Assert.That(_emailsController.ValidateEmailType(
                jResponse, planCreateResponse.InstallmentPlan.InstallmentPlanNumber,
                "Refund_Succeeded"));
            Console.WriteLine("TestValidatePartialRefundFromPastInstallmentsEmailSent is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidatePartialRefundFromPastInstallmentsEmailSent\n" + exception + "\n");
        }
    }
    
    [Category("EmailsTests")]
    [Test(Description = "TestValidatePartialRefundFromFutureInstallmentsEmailSent"), CancelAfter(240*1000)]
    public async Task TestValidatePartialRefundFromFutureInstallmentsEmailSent()
    {
        try
        {
            Console.WriteLine("\nStarting TestValidatePartialRefundFromFutureInstallmentsEmailSent");
            var jResponseMerchantSettings = await _settings.SendGetSettingsRequestAsync(
                _requestHeader!, int.Parse(Environment.GetEnvironmentVariable("MerchantBusinessUnitId")!));
            jResponseMerchantSettings!.MerchantSettings.RunCaptureAsync = false;
            var jResponseUpdateSettings = await _settingsSave.SendUpdateSettingsRequestAsync(
                _requestHeader!, jResponseMerchantSettings);
            Assert.That(jResponseUpdateSettings.MerchantSettings.RunCaptureAsync, Is.False);
            var v1InitiateDefaultValues = new V1InitiateDefaultValues
            {
                planData =
                {
                    numberOfInstallments = 10,
                    amount =
                    {
                        value = 1000
                    }
                }
            };
            var planCreateResponse = await _create.CreatePlanAsync( _requestHeader!, 
                v1InitiateDefaultValues, Environment.GetEnvironmentVariable("MockerV2Terminal")!);
            Assert.That(planCreateResponse.InstallmentPlan, Is.Not.Null);
            var jResponseRefund = await _refundFunctionality.SendRefundRequestAsync(_requestHeader!,
                planCreateResponse.InstallmentPlan.InstallmentPlanNumber, "PartialRefund");
            Assert.That(jResponseRefund.ResponseHeader.Succeeded, Is.True);
            var jResponse = await _getEmailsFunctionality.SendGetRequestGetEmailsAsync(
                 _requestHeader!, "InstallmentPlanNumber", 
                planCreateResponse.InstallmentPlan.InstallmentPlanNumber);
            Assert.That(_emailsController.ValidateEmailType(
                jResponse, planCreateResponse.InstallmentPlan.InstallmentPlanNumber,
                "Refund_Succeeded"));
            Console.WriteLine("TestValidatePartialRefundFromFutureInstallmentsEmailSent is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidatePartialRefundFromFutureInstallmentsEmailSent\n" + exception + "\n");
        }
    }
    
    [Category("EmailsTests")]
    [Test(Description = "TestValidatePartialRefundFromOutstandingAmountEmailSent"), CancelAfter(240*1000)]
    public async Task TestValidatePartialRefundFromOutstandingAmountEmailSent()
    {
        try
        {
            Console.WriteLine("\nStarting TestValidatePartialRefundFromOutstandingAmountEmailSent");
            var jResponseMerchantSettings = await _settings.SendGetSettingsRequestAsync(
                _requestHeader!, int.Parse(Environment.GetEnvironmentVariable("MerchantBusinessUnitId")!));
            jResponseMerchantSettings!.MerchantSettings.RunCaptureAsync = false;
            var jResponseUpdateSettings = await _settingsSave.SendUpdateSettingsRequestAsync(
                _requestHeader!, jResponseMerchantSettings);
            Assert.That(jResponseUpdateSettings.MerchantSettings.RunCaptureAsync, Is.True);
            var v1InitiateDefaultValues = new V1InitiateDefaultValues
            {
                planData =
                {
                    numberOfInstallments = 10,
                    amount =
                    {
                        value = 1000
                    }
                }
            };
            var planCreateResponse = await _create.CreatePlanAsync( _requestHeader!, 
                v1InitiateDefaultValues, Environment.GetEnvironmentVariable("MockerV2Terminal")!);
            Assert.That(planCreateResponse.InstallmentPlan, Is.Not.Null);
            await Task.Delay(10 * 1000);
            var jResponseRefund = await _refundFunctionality.SendRefundRequestAsync(_requestHeader!,
                planCreateResponse.InstallmentPlan.InstallmentPlanNumber, "PartialRefund",
                "FutureInstallmentsLast");
            Assert.That(jResponseRefund.ResponseHeader.Succeeded, Is.True);
            var jResponse = await _getEmailsFunctionality.SendGetRequestGetEmailsAsync(
                 _requestHeader!, "InstallmentPlanNumber", 
                planCreateResponse.InstallmentPlan.InstallmentPlanNumber);
            Assert.That(_emailsController.ValidateEmailType(
                jResponse, planCreateResponse.InstallmentPlan.InstallmentPlanNumber,
                "Refund_Succeeded"));
            Console.WriteLine("TestValidatePartialRefundFromOutstandingAmountEmailSent is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidatePartialRefundFromOutstandingAmountEmailSent\n" + exception + "\n");
        }
    }
    
    [Category("EmailsTests")]
    [Test(Description = "TestValidatePlanCanceledEmailSent"), CancelAfter(240*1000)]
    public async Task TestValidatePlanCanceledEmailSent()
    {
        try
        {
            Console.WriteLine("\nStarting TestValidatePlanCanceledEmailSent");
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            var planCreateResponse = await _installmentPlans.CreatePlanAsync(
                Environment.GetEnvironmentVariable("ApiV3")!, _requestHeader!, "Active",
                new Random().Next(2, 6), 
                Environment.GetEnvironmentVariable("SplititMockTerminal")!, createPlanDefaultValues);
            var jResponseCancel = await _installmentPlanNumberCancel.SendCancelPlanRequestAsync(
                _requestHeader!, planCreateResponse.InstallmentPlanNumber, 0, "NoRefunds");
            Assert.That(jResponseCancel.ResponseHeader.Succeeded);
            var jResponse = await _getEmailsFunctionality.SendGetRequestGetEmailsAsync(
                _requestHeader!, "InstallmentPlanNumber", 
                planCreateResponse.InstallmentPlanNumber);
            Assert.That(_emailsController.ValidateEmailSubject(jResponse, planCreateResponse.InstallmentPlanNumber,
                "We have canceled your payment plan"));
            Console.WriteLine("TestValidatePlanCanceledEmailSent is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidatePlanCanceledEmailSent\n" + exception + "\n");
        }
    }
    
    [Category("EmailsTests")]
    [Test(Description = "TestValidatePhoneOrderApprovalEmailSent"), CancelAfter(360*1000)]
    public async Task TestValidatePhoneOrderApprovalEmailSent()
    {
        try
        {
            Console.WriteLine("\nStarting TestValidatePhoneOrderApprovalEmailSent");
            var v1InitiateDefaultValues = new V1InitiateDefaultValues
            {
                PlanApprovalEvidence =
                {
                    AreTermsAndConditionsApproved = "false"
                }
            };
            var planCreateResponse = await _create.CreatePlanAsync( _requestHeader!, 
                v1InitiateDefaultValues, Environment.GetEnvironmentVariable("SplititMockTerminal")!);
            Assert.That(planCreateResponse.InstallmentPlan, Is.Not.Null);
            var jResponseTerms = await _postTermsAndConditionsFunctionality.SendPostRequestPostTermsAndConditionsAsync(
                _requestHeader!, planCreateResponse.InstallmentPlan.InstallmentPlanNumber);
            Assert.That(jResponseTerms.ResponseHeader.Succeeded);
            var jResponse = await _getEmailsFunctionality.SendGetRequestGetEmailsAsync(
                _requestHeader!, "InstallmentPlanNumber", 
                planCreateResponse.InstallmentPlan.InstallmentPlanNumber);
            Assert.That(_emailsController.ValidateEmailType(jResponse, 
                planCreateResponse.InstallmentPlan.InstallmentPlanNumber, "WaitingForApproval"));
            Console.WriteLine("TestValidatePhoneOrderApprovalEmailSent is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidatePhoneOrderApprovalEmailSent\n" + exception + "\n");
        }
    }
    
    [Category("EmailsTests")]
    [Test(Description = "TestValidatePlanCompletedEmailSent"), CancelAfter(240*1000)]
    public async Task TestValidatePlanCompletedEmailSent()
    {
        try
        {
            Console.WriteLine("\nStarting TestValidatePlanCompletedEmailSent");
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            var planCreateResponse = await _installmentPlans.CreatePlanAsync(
                Environment.GetEnvironmentVariable("ApiV3")!, _requestHeader!, "Active", 2, 
                Environment.GetEnvironmentVariable("SplititMockTerminal")!, createPlanDefaultValues);
            var jResponseChargeFunctionality = await _chargeFunctionality.SendPostRequestChargeFunctionalityAsync(
                _requestHeader!, planCreateResponse.InstallmentPlanNumber);
            Assert.That(jResponseChargeFunctionality.ResponseHeader.Succeeded);
            var jResponse = await _getEmailsFunctionality.SendGetRequestGetEmailsAsync(
                _requestHeader!, "InstallmentPlanNumber", planCreateResponse.InstallmentPlanNumber);
            Assert.That(_emailsController.ValidateEmailType(jResponse, planCreateResponse.InstallmentPlanNumber,
                "Cleared"));
            Console.WriteLine("TestValidatePlanCompletedEmailSent is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidatePlanCompletedEmailSent\n" + exception + "\n");
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
                _requestHeader!, int.Parse(Environment.GetEnvironmentVariable("MerchantBusinessUnitId")!));
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
                v1InitiateDefaultValues, Environment.GetEnvironmentVariable("MockerV2Terminal")!);
            Assert.That(planCreateResponse.InstallmentPlan, Is.Not.Null);
            var jResponseReAuth = await _reAuthFunctionality.SendPostRequestReAuthAsync(
                _requestHeader!, planCreateResponse.InstallmentPlan.InstallmentPlanNumber, "yes");
            Assert.That(jResponseReAuth.Succeeded, Is.False);
            await Task.Delay(10 * 1000);
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
                Environment.GetEnvironmentVariable("ApiV3")!, _requestHeader!, "Active",
                new Random().Next(2, 6), 
                Environment.GetEnvironmentVariable("SplititMockTerminal")!, createPlanDefaultValues);
            var jResponseUpdateCardRequest =
                await _requestUpdateCardFunctionality.SendPostRequestRequestUpdateCardAsync(
                    _requestHeader!, planCreateResponse.InstallmentPlanNumber,
                    Environment.GetEnvironmentVariable("MerchantId")!);
            Assert.That(jResponseUpdateCardRequest.IsSuccess);
            var jResponse = await _getEmailsFunctionality.SendGetRequestGetEmailsAsync(
                _requestHeader!, "InstallmentPlanNumber", planCreateResponse.InstallmentPlanNumber);
            Assert.That(_emailsController.ValidateEmailSubject(jResponse, planCreateResponse.InstallmentPlanNumber,
                "Update Your credit card"));
            Console.WriteLine("TestValidateRequestUpdateCcEmailSent is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidateRequestUpdateCcEmailSent\n" + exception + "\n");
        }
    }
    
    [Category("EmailsTests")]
    [Test(Description = "TestValidateRequestPaymentEmailSent"), CancelAfter(360*1000)]
    public async Task TestValidateRequestPaymentEmailSent()
    {
        try
        {
            Console.WriteLine("\nStarting TestValidateRequestPaymentEmailSent");
            var v1InitiateDefaultValues = new V1InitiateDefaultValues();
            var planCreateResponse = await _create.CreatePlanInitiateAsync( _requestHeader!, 
                v1InitiateDefaultValues);
            Assert.That(planCreateResponse.InstallmentPlan, Is.Not.Null);
            var jResponseRequestPayment = await _requestPayment.SendPostRequestRequestPaymentAsync(
                 _requestHeader!, planCreateResponse.InstallmentPlan.InstallmentPlanNumber, 
                "Splitit.Automation@splitit.com", "VposCheckOut", "2.0");
            Assert.That(jResponseRequestPayment.ResponseHeader.Succeeded);
            var jResponse = await _getEmailsFunctionality.SendGetRequestGetEmailsAsync(
                 _requestHeader!, "InstallmentPlanNumber",
                planCreateResponse.InstallmentPlan.InstallmentPlanNumber, true);
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
    [Test(Description = "TestValidateReceiptAttachmentEmailSent"), CancelAfter(240*1000)]
    public async Task TestValidateReceiptAttachmentEmailSent()
    {
        try
        {
            Console.WriteLine("\nStarting TestValidateReceiptAttachmentEmailSent");
            var v1InitiateDefaultValues = new V1InitiateDefaultValues();
            var planCreateResponse = await _create.CreatePlanAsync( _requestHeader!, 
                v1InitiateDefaultValues);
            Assert.That(planCreateResponse.InstallmentPlan, Is.Not.Null);
            var jResponseReceipt = await _receiptFunctionality.SendGetRequestGetReceiptAsync(
                 _requestHeader!, planCreateResponse.InstallmentPlan.InstallmentPlanNumber);
            Assert.That(jResponseReceipt.ResponseHeader.Succeeded);
            var filePathDownload = await _s3Controller.DownloadFileFromS3(
                jResponseReceipt.DocumentUrl, "DownloadedReceipt", ".pdf");
            Assert.That(filePathDownload, Is.Not.Null);
            var receiptConverted = _convertPdfToStringController.ConvertPdfToString(filePathDownload);
            Assert.That(receiptConverted, Is.Not.Null);
            File.Delete(filePathDownload);
            Console.WriteLine("TestValidateReceiptAttachmentEmailSent is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidateReceiptAttachmentEmailSent\n" + exception + "\n");
        }
    }
}