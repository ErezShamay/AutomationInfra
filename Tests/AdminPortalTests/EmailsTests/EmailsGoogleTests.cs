using Allure.NUnit;
using Allure.NUnit.Attributes;
using NUnit.Framework;
using Splitit.Automation.NG.Backend.BaseActions;
using Splitit.Automation.NG.Backend.Services.AdminApi.InstallmentPlan.InstallmentsPlanFunctionality;
using Splitit.Automation.NG.Backend.Services.AdminPortal.Logs.Functionality;
using Splitit.Automation.NG.Backend.Services.MerchantPortal.InstallmentPlan.Functionality;
using Splitit.Automation.NG.Backend.Services.V1._5.DefaultValues;
using Splitit.Automation.NG.Backend.Services.V3.DefaultValues;
using Splitit.Automation.NG.Backend.Tests.TestsSetups;
using Splitit.Automation.NG.Backend.Services.V1._5.Login.LoginApiEndPoints;
using Splitit.Automation.NG.Backend.Services.AdminApi.Merchant.MerchantFunctionality;
using Splitit.Automation.NG.Backend.Services.V1.DefaultValues;
using Create = Splitit.Automation.NG.Backend.Services.V1._5.InstallmentPlan.InstallmentPlanApiEndPoints.Create;
using CreateV1 = Splitit.Automation.NG.Backend.Services.V1.InstallmentPlan.InstallmentPlanApiEndPoints.Create;

namespace Splitit.Automation.NG.Backend.Tests.AdminPortalTests.EmailsTests;

[TestFixture]
[AllureNUnit]
[AllureSuite("EmailsGoogleTests")]
[AllureDisplayIgnored]
//[Parallelizable(ParallelScope.All)]/
public class EmailsGoogleTests
{
    private RequestHeader? _requestHeader;
    private RequestHeader? _requestHeaderMerchant;
    private readonly GetEmailsFunctionality _getEmailsFunctionality;
    private readonly EmailsController _emailsController;
    private readonly CreatePlanV1Point5DefaultValues _createPlanV1Point5DefaultValues;
    private readonly Create _create;
    private readonly Login _login;
    private readonly Settings _settings;
    private readonly SettingsSave _settingsSave;
    private readonly CreateV1 _createV1;
    private readonly ReAuthFunctionality _reAuthFunctionality;
    private readonly RetryFunctionality _retryFunctionality;
    private readonly RequestUpdateCardFunctionality _requestUpdateCardFunctionality;
    
    public EmailsGoogleTests()
    {
        Console.WriteLine("Staring EmailsGoogleTests Setup");
        _getEmailsFunctionality = new GetEmailsFunctionality();
        _emailsController = new EmailsController();
        _createPlanV1Point5DefaultValues = new CreatePlanV1Point5DefaultValues();
        _create = new Create();
        _login = new Login();
        _settings = new Settings();
        _settingsSave = new SettingsSave();
        _createV1 = new CreateV1();
        _requestUpdateCardFunctionality = new RequestUpdateCardFunctionality();
        _reAuthFunctionality = new ReAuthFunctionality();
        _retryFunctionality = new RetryFunctionality();
        Console.WriteLine("EmailsGoogleTests Setup Succeeded\n");
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
        _requestHeaderMerchant = await _login.LoginAsMerchantAsync(
            Environment.GetEnvironmentVariable("GoogleBaseUri")!,
            new RequestHeader(),
            Environment.GetEnvironmentVariable("GoogleEmailsTerminal")!,
            Environment.GetEnvironmentVariable("GoogleEmailsUserName")!,
            Environment.GetEnvironmentVariable("GoogleEmailsPassword")!);
    }
    
    [Category("EmailsTests")]
    [Test(Description = "TestValidateGoogleEmailsRequestUpdateCcEmailSent"), CancelAfter(600*1000)]
    public async Task TestValidateGoogleEmailsRequestUpdateCcEmailSent()
    {
        try
        {
            Console.WriteLine("\nStarting TestValidateGoogleEmailsRequestUpdateCcEmailSent");
            _createPlanV1Point5DefaultValues.planData.amount.currencyCode = "JPY";
            _createPlanV1Point5DefaultValues.planData.amount.value = new Random().Next(10, 100);
            var planCreateResponse = await _create.CreatePlanV1Point5Async(_requestHeaderMerchant!,
                new Random().Next(2, 6), _createPlanV1Point5DefaultValues);
            Assert.That(planCreateResponse.InstallmentPlan.InstallmentPlanStatus.Code, Is.EqualTo("InProgress"));
            var jResponseRequestUpdateCard = await _requestUpdateCardFunctionality.
                SendPostRequestRequestUpdateCardAsync(_requestHeader!, planCreateResponse.InstallmentPlan.InstallmentPlanNumber,
                Environment.GetEnvironmentVariable("GoogleEmailsMerchantId")!);
            Assert.That(jResponseRequestUpdateCard.IsSuccess);
            var jResponse = await _getEmailsFunctionality.SendGetRequestGetEmailsAsync(
                _requestHeader!, "InstallmentPlanNumber",
                planCreateResponse.InstallmentPlan.InstallmentPlanNumber);
            Assert.That(_emailsController.ValidateEmailType(jResponse,
                planCreateResponse.InstallmentPlan.InstallmentPlanNumber, "RequestUpdateCreditCard"));
            Console.WriteLine("TestValidateGoogleEmailsRequestUpdateCcEmailSent is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidateGoogleEmailsRequestUpdateCcEmailSent\n" + exception + "\n");
        }
    }
    
    [Category("EmailsTests")]
    [Test(Description = "TestValidateGoogleEmailsReAuthFailureEmailSent"), CancelAfter(600*1000)]
    public async Task TestValidateGoogleEmailsReAuthFailureEmailSent()
    {
        try
        {
            Console.WriteLine("\nStarting TestValidateGoogleEmailsReAuthFailureEmailSent");
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
                },
                RequestHeader =
                {
                    apiKey = Environment.GetEnvironmentVariable("Splitit_Mock_Google_EmailsTerminal")!
                },
                planData =
                {
                    amount = new AmountDefaultValues
                    {
                        value = new Random().Next(10, 100)
                    },
                    numberOfInstallments = new Random().Next(4, 10)
                }
            };
            var planCreateResponse = await _createV1.CreatePlanAsync( _requestHeader!, 
                v1InitiateDefaultValues, Environment.GetEnvironmentVariable("Splitit_Mock_Google_EmailsTerminal")!);
            Assert.That(planCreateResponse.InstallmentPlan, Is.Not.Null);
            var jResponseReAuth = await _reAuthFunctionality.SendPostRequestReAuthAsync(
                _requestHeader!, planCreateResponse.InstallmentPlan.InstallmentPlanNumber, "yes");
            Assert.That(jResponseReAuth.Succeeded, Is.False);
            var jResponse = await _getEmailsFunctionality.SendGetRequestGetEmailsAsync(
                 _requestHeader!, "InstallmentPlanNumber", 
                planCreateResponse.InstallmentPlan.InstallmentPlanNumber);
            Assert.That(_emailsController.ValidateEmailType(
                jResponse, planCreateResponse.InstallmentPlan.InstallmentPlanNumber,
                "StartCCRejection"));
            Console.WriteLine("TestValidateGoogleEmailsReAuthFailureEmailSent is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidateGoogleEmailsReAuthFailureEmailSent\n" + exception + "\n");
        }
    }
    
    [Category("EmailsTests")]
    [Test(Description = "TestValidateGoogleEmailsRetryReAuthFailureEmailSent"), CancelAfter(600*1000)]
    public async Task TestValidateGoogleEmailsRetryReAuthFailureEmailSent()
    {
        try
        {
            Console.WriteLine("\nStarting TestValidateGoogleEmailsRetryReAuthFailureEmailSent");
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
                },
                RequestHeader =
                {
                    apiKey = Environment.GetEnvironmentVariable("Splitit_Mock_Google_EmailsTerminal")!
                },
                planData =
                {
                    amount = new AmountDefaultValues
                    {
                        value = new Random().Next(10, 100)
                    },
                    numberOfInstallments = new Random().Next(4, 10)
                }
            };
            var planCreateResponse = await _createV1.CreatePlanAsync( _requestHeader!, 
                v1InitiateDefaultValues, Environment.GetEnvironmentVariable("Splitit_Mock_Google_EmailsTerminal")!);
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
            Console.WriteLine("TestValidateGoogleEmailsRetryReAuthFailureEmailSent is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidateGoogleEmailsRetryReAuthFailureEmailSent\n" + exception + "\n");
        }
    }
}