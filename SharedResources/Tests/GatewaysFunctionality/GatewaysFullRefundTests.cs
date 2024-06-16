using Allure.NUnit;
using Allure.NUnit.Attributes;
using NUnit.Framework;
using Splitit.Automation.NG.Backend.BaseActions;
using Splitit.Automation.NG.Backend.Services.AdminApi.Merchant.MerchantFunctionality;
using Splitit.Automation.NG.Backend.Services.V3.DefaultValues;
using Splitit.Automation.NG.Backend.Services.V3.InstallmentPlan.InstallmentPlanApiEndPoint;
using Splitit.Automation.NG.Backend.Tests.TestsSetups;
using Splitit.Automation.NG.Utilities.EnvironmentsConfig;

namespace Splitit.Automation.NG.SharedResources.Tests.GatewaysFunctionality;

[TestFixture]
[AllureNUnit]
[AllureSuite("GatewaysFullRefundTests")]
[AllureDisplayIgnored]
[Parallelizable(scope: ParallelScope.All)]
public class GatewaysFullRefundTests
{
    private readonly InstallmentPlans _installmentPlans;
    private RequestHeader? _requestHeader;
    private readonly InstallmentPlanNumberRefund _installmentPlanNumberRefund;
    private readonly Settings _settings;
    private readonly SettingsSave _settingsSave;
    private readonly EnvConfig _envConfig;

    public GatewaysFullRefundTests()
    {
        Console.WriteLine("Starting Setup");
        _installmentPlans = new InstallmentPlans();
        _settings = new Settings();
        _settingsSave = new SettingsSave();
        _installmentPlanNumberRefund = new InstallmentPlanNumberRefund();
        _envConfig = new EnvConfig();
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
    
    [TestCase(Category = "GatewaysFullRefundTests")]
    [Test(Description = "CreatePlanWithTerminalWorldPayFullRefund")]
    public async Task CreatePlanWithTerminalWorldPayFullRefund()
    {
        try
        {
            Console.WriteLine("\nStarting CreatePlanWithTerminalWorldPayFullRefund");
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            var jResponse = await _settings.SendGetSettingsRequestAsync( _requestHeader!, int.Parse(_envConfig.NgMockV2BU));
            jResponse!.MerchantSettings.DefaultPlanStrategy = "SecuredPlan3";
            var jResponseUpdated = await _settingsSave.SendUpdateSettingsRequestAsync( _requestHeader!, jResponse);
            Assert.That(jResponseUpdated.MerchantSettings.DefaultPlanStrategy, Is.EqualTo("SecuredPlan3"));
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            createPlanDefaultValues.paymentMethod.card.cardNumber = _envConfig.Card4111;
            createPlanDefaultValues.planData.totalAmount = AmountGenerator.GenerateAmountWithMinMaxValues(100, 4000);
            var planCreateResponse = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!,  _requestHeader!, "Active", 
                new Random().Next(4, 12), _envConfig.WorldPayTerminal, createPlanDefaultValues);
            var refundResponse = await _installmentPlanNumberRefund.SendRefundRequestAsync( _requestHeader!, planCreateResponse.InstallmentPlanNumber,
                "FullRefund");
            Assert.That(refundResponse.Error, Is.Not.Null);
            Console.WriteLine("CreatePlanWithTerminalWorldPayFullRefund is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in CreatePlanWithTerminalWorldPayFullRefund \n" + exception + "\n");
        }
    }
    
    [TestCase(Category = "GatewaysFullRefundTests")]
    [Test(Description = "CreatePlanWithTerminalAuthNetV2FullRefund")]
    public async Task CreatePlanWithTerminalAuthNetV2FullRefund()
    {
        try
        {
            Console.WriteLine("\nStarting CreatePlanWithTerminalAuthNetV2FullRefund");
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            var planCreateResponse = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!,  _requestHeader!, "Active", 
                new Random().Next(4, 12), _envConfig.AuthorizeNetV2, createPlanDefaultValues);
            var refundResponse = await _installmentPlanNumberRefund.SendRefundRequestAsync( _requestHeader!, planCreateResponse.InstallmentPlanNumber,
                "FullRefund");
            Assert.That(refundResponse.Error, Is.Not.Null);
            Console.WriteLine("CreatePlanWithTerminalAuthNetV2FullRefund is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in CreatePlanWithTerminalAuthNetV2FullRefund \n" + exception + "\n");
        }
    }
    
    [TestCase(Category = "GatewaysFullRefundTests")]
    [Test(Description = "CreatePlanWithTerminalBluesnapDirectFullRefund")]
    public async Task CreatePlanWithTerminalBluesnapDirectFullRefund()
    {
        try
        {
            Console.WriteLine("\nStarting CreatePlanWithTerminalBluesnapDirectFullRefund");
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            var planCreateResponse = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!,  _requestHeader!, "Active", 
                new Random().Next(4, 12), _envConfig.BlueSnapDirectTerminal, createPlanDefaultValues);
            var refundResponse = await _installmentPlanNumberRefund.SendRefundRequestAsync( _requestHeader!, planCreateResponse.InstallmentPlanNumber,
                "FullRefund");
            Assert.That(refundResponse.Error, Is.Not.Null);
            Console.WriteLine("CreatePlanWithTerminalBluesnapDirectFullRefund is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in CreatePlanWithTerminalBluesnapDirectFullRefund \n" + exception + "\n");
        }
    }
    
    [TestCase(Category = "GatewaysFullRefundTests")]
    [Test(Description = "CreatePlanWithTerminalBluesnapMorFullRefund")]
    public async Task CreatePlanWithTerminalBluesnapMorFullRefund()
    {
        try
        {
            Console.WriteLine("\nStarting CreatePlanWithTerminalBluesnapMorFullRefund");
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            var planCreateResponse = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!,  _requestHeader!, "Active", 
                new Random().Next(4, 12), _envConfig.BlueSnapMORTerminal, createPlanDefaultValues);
            var refundResponse = await _installmentPlanNumberRefund.SendRefundRequestAsync( _requestHeader!, planCreateResponse.InstallmentPlanNumber,
                "FullRefund");
            Assert.That(refundResponse.Error, Is.Not.Null);
            Console.WriteLine("CreatePlanWithTerminalBluesnapMorFullRefund is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in CreatePlanWithTerminalBluesnapMorFullRefund \n" + exception + "\n");
        }
    }
    
    [TestCase(Category = "GatewaysFullRefundTests")]
    [Test(Description = "CreatePlanWithTerminalCheckoutFullRefund")]
    public async Task CreatePlanWithTerminalCheckoutFullRefund()
    {
        try
        {
            Console.WriteLine("\nStarting CreatePlanWithTerminalCheckoutFullRefund");
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            var planCreateResponse = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!,  _requestHeader!, "Active", 
                new Random().Next(4, 12), _envConfig.CheckoutTerminal, createPlanDefaultValues);
            var refundResponse = await _installmentPlanNumberRefund.SendRefundRequestAsync( _requestHeader!, planCreateResponse.InstallmentPlanNumber,
                "FullRefund");
            Assert.That(refundResponse.Error, Is.Not.Null);
            Console.WriteLine("CreatePlanWithTerminalCheckoutFullRefund is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in CreatePlanWithTerminalCheckoutFullRefund \n" + exception + "\n");
        }
    }
    
    [TestCase(Category = "GatewaysFullRefundTests")]
    [Test(Description = "CreatePlanWithTerminalCyberSourceFullRefund")]
    public async Task CreatePlanWithTerminalCyberSourceFullRefund()
    {
        try
        {
            Console.WriteLine("\nStarting CreatePlanWithTerminalCyberSourceFullRefund");
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            createPlanDefaultValues.paymentMethod.card.cardNumber = _envConfig.Card4456;
            createPlanDefaultValues.paymentMethod.card.cardExpMonth = int.Parse(_envConfig.Card4456mth);
            createPlanDefaultValues.paymentMethod.card.cardExpYear = int.Parse(_envConfig.Card4456year);
            createPlanDefaultValues.planData.currency = "GBP";
            createPlanDefaultValues.planData.totalAmount = AmountGenerator.GenerateAmountWithMinMaxValues(10, 999);
            var planCreateResponse = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!,  _requestHeader!, "Active", 
                new Random().Next(4, 12), _envConfig.CyberSourceTerminal, createPlanDefaultValues);
            var refundResponse = await _installmentPlanNumberRefund.SendRefundRequestAsync( _requestHeader!, planCreateResponse.InstallmentPlanNumber,
                "FullRefund");
            Assert.That(refundResponse.Error, Is.Not.Null);
            Console.WriteLine("CreatePlanWithTerminalCyberSourceFullRefund is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in CreatePlanWithTerminalCyberSourceFullRefund \n" + exception + "\n");
        }
    }
    
    [TestCase(Category = "GatewaysFullRefundTests")]
    [Test(Description = "CreatePlanWithTerminalStripeDirectFullRefund")]
    public async Task CreatePlanWithTerminalStripeDirectFullRefund()
    {
        try
        {
            Console.WriteLine("\nStarting CreatePlanWithTerminalStripeDirectFullRefund");
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            var planCreateResponse = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!,  _requestHeader!, "Active", 
                new Random().Next(4, 12), _envConfig.StripeDirectTerminal, createPlanDefaultValues);
            var refundResponse = await _installmentPlanNumberRefund.SendRefundRequestAsync( _requestHeader!, planCreateResponse.InstallmentPlanNumber,
                "FullRefund");
            Assert.That(refundResponse.Error, Is.Not.Null);
            Console.WriteLine("CreatePlanWithTerminalStripeDirectFullRefund is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in CreatePlanWithTerminalStripeDirectFullRefund \n" + exception + "\n");
        }
    }
    
    [TestCase(Category = "GatewaysFullRefundTests")]
    [Test(Description = "CreatePlanWithTerminalReachFullRefund")]
    public async Task CreatePlanWithTerminalReachFullRefund()
    {
        try
        {
            Console.WriteLine("\nStarting CreatePlanWithTerminalReachFullRefund");
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            var planCreateResponse = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!,  _requestHeader!, "Active", 
                new Random().Next(4, 12), _envConfig.ReachTerminal, createPlanDefaultValues);
            await Task.Delay(300 * 1000);
            var refundResponse = await _installmentPlanNumberRefund.SendRefundRequestAsync( _requestHeader!, planCreateResponse.InstallmentPlanNumber,
                "FullRefund");
            Assert.That(refundResponse.Error, Is.Not.Null);
            Console.WriteLine("CreatePlanWithTerminalReachFullRefund is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in CreatePlanWithTerminalReachFullRefund \n" + exception + "\n");
        }
    }
    
    [TestCase(Category = "GatewaysFullRefundTests")]
    [Test(Description = "CreatePlanWithTerminalPaySafeFullRefund")]
    public async Task CreatePlanWithTerminalPaySafeFullRefund()
    {
        try
        {
            Console.WriteLine("\nStarting CreatePlanWithTerminalPaySafeFullRefund");
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            var planCreateResponse = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!,  _requestHeader!, "Active", 
                new Random().Next(4, 12), _envConfig.PaySafeTerminal, createPlanDefaultValues);
            var refundResponse = await _installmentPlanNumberRefund.SendRefundRequestAsync( _requestHeader!, planCreateResponse.InstallmentPlanNumber,
                "FullRefund");
            Assert.That(refundResponse.Error, Is.Not.Null);
            Console.WriteLine("CreatePlanWithTerminalPaySafeFullRefund is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in CreatePlanWithTerminalPaySafeFullRefund \n" + exception + "\n");
        }
    }
    
    [TestCase(Category = "GatewaysFullRefundTests")]
    [Test(Description = "CreatePlanWithTerminalAdyenFullRefund")]
    public async Task CreatePlanWithTerminalAdyenFullRefund()
    {
        try
        {
            Console.WriteLine("\nStarting CreatePlanWithTerminalAdyenFullRefund");
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            var planCreateResponse = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!,  _requestHeader!, "Active", 
                new Random().Next(4, 12), _envConfig.AdyenTerminal, createPlanDefaultValues);
            var refundResponse = await _installmentPlanNumberRefund.SendRefundRequestAsync( _requestHeader!, planCreateResponse.InstallmentPlanNumber,
                "FullRefund");
            Assert.That(refundResponse.Error, Is.Not.Null);
            Console.WriteLine("CreatePlanWithTerminalAdyenFullRefund is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in CreatePlanWithTerminalAdyenFullRefund \n" + exception + "\n");
        }
    }
    
    [TestCase(Category = "GatewaysFullRefundTests")]
    [Test(Description = "CreatePlanWithTerminalSpreedlyFullRefund")]
    public async Task CreatePlanWithTerminalSpreedlyFullRefund()
    {
        try
        {
            Console.WriteLine("\nStarting CreatePlanWithTerminalSpreedlyFullRefund");
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            createPlanDefaultValues.planData.totalAmount = new Random().Next(5, 50);
            var planCreateResponse = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!,  _requestHeader!, "Active", 
                new Random().Next(4, 12), _envConfig.SpreedlyTerminal, createPlanDefaultValues);
            var refundResponse = await _installmentPlanNumberRefund.SendRefundRequestAsync( _requestHeader!, planCreateResponse.InstallmentPlanNumber,
                "FullRefund");
            Assert.That(refundResponse.Error, Is.Not.Null);
            Console.WriteLine("CreatePlanWithTerminalSpreedlyFullRefund is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in CreatePlanWithTerminalSpreedlyFullRefund \n" + exception + "\n");
        }
    }
}