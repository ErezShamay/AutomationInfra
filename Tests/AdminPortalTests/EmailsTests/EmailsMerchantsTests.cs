using Allure.NUnit;
using Allure.NUnit.Attributes;
using NUnit.Framework;
using Splitit.Automation.NG.Backend.BaseActions;
using Splitit.Automation.NG.Backend.Services.AdminApi.InstallmentPlan.InstallmentsPlanFunctionality;
using Splitit.Automation.NG.Backend.Services.AdminApi.Merchant.MerchantFunctionality;
using Splitit.Automation.NG.Backend.Services.AdminPortal.Logs.Functionality;
using Splitit.Automation.NG.Backend.Services.V1.DefaultValues;
using Splitit.Automation.NG.Backend.Services.V3.DefaultValues;
using Splitit.Automation.NG.Backend.Services.V3.InstallmentPlan.InstallmentPlanApiEndPoint;
using Splitit.Automation.NG.Backend.Tests.TestsSetups;
using Create = Splitit.Automation.NG.Backend.Services.V1.InstallmentPlan.InstallmentPlanApiEndPoints;

namespace Splitit.Automation.NG.Backend.Tests.AdminPortalTests.EmailsTests;

[TestFixture]
[AllureNUnit]
[AllureSuite("EmailsMerchantsTests")]
[AllureDisplayIgnored] 
//[Parallelizable(ParallelScope.All)]
public class EmailsMerchantsTests
{
    private RequestHeader? _requestHeader;
    private readonly UsersCreateFunctionality _usersCreateFunctionality;
    private readonly GetEmailsFunctionality _getEmailsFunctionality;
    private readonly EmailsController _emailsController;
    private readonly UsersDeleteFunctionality _usersDeleteFunctionality;
    private readonly Create.Create _create;
    private readonly Settings _settings;
    private readonly SettingsSave _settingsSave;
    private readonly FullCaptureFunctionality _fullCaptureFunctionality;
    private readonly InstallmentPlans _installmentPlans;
    
    public EmailsMerchantsTests()
    {
        Console.WriteLine("Staring EmailsMerchantsTests Setup");
        _usersCreateFunctionality = new UsersCreateFunctionality();
        _getEmailsFunctionality = new GetEmailsFunctionality();
        _emailsController = new EmailsController();
        _usersDeleteFunctionality = new UsersDeleteFunctionality();
        _create = new Create.Create();
        _settings = new Settings();
        _settingsSave = new SettingsSave();
        _fullCaptureFunctionality = new FullCaptureFunctionality();
        _installmentPlans = new InstallmentPlans();
        Console.WriteLine("EmailsMerchantsTests Setup Succeeded\n");
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
    [Test(Description = "TestValidatePaymentPlanCreatedEmailSent"), CancelAfter(360*1000)]
    public async Task TestValidatePaymentPlanCreatedEmailSent()
    {
        try
        {
            Console.WriteLine("\nStarting TestValidatePaymentPlanCreatedEmailSent");
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            var planCreateResponse = await _installmentPlans.CreatePlanAsync(
                Environment.GetEnvironmentVariable("ApiV3")!, _requestHeader!, "Active",
                new Random().Next(2, 6), 
                Environment.GetEnvironmentVariable("SplititMockTerminal")!, createPlanDefaultValues);
            var jResponse = await _getEmailsFunctionality.SendGetRequestGetEmailsAsync(
                _requestHeader!, "InstallmentPlanNumber", planCreateResponse.InstallmentPlanNumber);
            Assert.That(_emailsController.ValidateEmailSubject(jResponse, planCreateResponse.InstallmentPlanNumber,
                "Payment plan created"));
            Console.WriteLine("TestValidatePaymentPlanCreatedEmailSent is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidatePaymentPlanCreatedEmailSent\n" + exception + "\n");
        }
    }
    
    [Category("EmailsTests")]
    [Test(Description = "TestValidateMerchantCreationExplicitRecipientEmailSent"), CancelAfter(360*1000)]
    public async Task TestValidateMerchantCreationExplicitRecipientEmailSent()
    {
        try
        {
            Console.WriteLine("\nStarting TestValidateMerchantCreationExplicitRecipientEmailSent");
            var (jResponseUserCreate, userDetails) = await _usersCreateFunctionality.
                SendPostRequestForUsersCreateAsync(_requestHeader!);
            Assert.That(jResponseUserCreate.UserId, Is.Not.Null);
            var jResponse = await _getEmailsFunctionality.SendGetRequestGetEmailsAsync(
                _requestHeader!, "ExplicitRecipient",
                userDetails.User.Email, true);
            Assert.That(_emailsController.ValidateEmailSubject(jResponse, "",
                "Welcome to Splitit", "yes"));
            var jResponseDeleteUsers = await _usersDeleteFunctionality.
                SendPostRequestForUsersDeleteAsync(_requestHeader!, 
                    jResponseUserCreate.UserId);
            Assert.That(jResponseDeleteUsers.ResponseHeader.Succeeded);
            Console.WriteLine("TestValidateMerchantCreationExplicitRecipientEmailSent is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidateMerchantCreationExplicitRecipientEmailSent\n" + exception + "\n");
        }
    }
    
    [Category("EmailsTests")]
    [Test(Description = "TestValidateMerchantFullCaptureEmailSent"), CancelAfter(360*1000)]
    public async Task TestValidateMerchantFullCaptureEmailSent()
    {
        try
        {
            Console.WriteLine("\nStarting TestValidateMerchantFullCaptureEmailSent");
            var jResponseMerchantSettings = await _settings.SendGetSettingsRequestAsync(
                _requestHeader!, int.Parse(Environment.GetEnvironmentVariable("MerchantBusinessUnitId")!));
            jResponseMerchantSettings!.MerchantSettings.RunCaptureAsync = false;
            var jResponseUpdateSettings = await _settingsSave.SendUpdateSettingsRequestAsync(
                _requestHeader!, jResponseMerchantSettings);
            Assert.That(jResponseUpdateSettings.MerchantSettings.RunCaptureAsync, Is.False);
            var v1InitiateDefaultValues = new V1InitiateDefaultValues();
            var planCreateResponse = await _create.CreatePlanAsync(_requestHeader!, 
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
                "FullCapture_Succeeded_Merchant"));
            Console.WriteLine("TestValidateMerchantFullCaptureEmailSent is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidateMerchantFullCaptureEmailSent\n" + exception + "\n");
        }
    }
}