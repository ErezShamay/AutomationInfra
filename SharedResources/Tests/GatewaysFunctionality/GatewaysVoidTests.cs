using Allure.NUnit;
using Allure.NUnit.Attributes;
using iText.StyledXmlParser.Jsoup.Select;
using NUnit.Framework;
using Splitit.Automation.NG.Backend.BaseActions;
using Splitit.Automation.NG.Backend.Services.AdminApi.InstallmentPlan.InstallmentsPlanFunctionality;
using Splitit.Automation.NG.Backend.Services.AdminApi.Merchant.MerchantFunctionality;
using Splitit.Automation.NG.Backend.Services.V3.DefaultValues;
using Splitit.Automation.NG.Backend.Services.V3.InstallmentPlan.InstallmentPlanApiEndPoint;
using Splitit.Automation.NG.Backend.Tests.TestsSetups;
using Splitit.Automation.NG.SharedResources.BaseActions;
using Splitit.Automation.NG.Utilities.EnvironmentsConfig;

namespace Splitit.Automation.NG.SharedResources.Tests.GatewaysFunctionality;

[TestFixture]
[AllureNUnit]
[AllureSuite("GatewaysVoidTestsSuite")]
[AllureDisplayIgnored]
[Parallelizable(scope: ParallelScope.All)]
public class GatewaysVoidTests
{
    private readonly InstallmentPlans _installmentPlans;
    private RequestHeader? _requestHeader;
    private readonly OpenAuthorizationsFunctionality _openAuthorizationsFunctionality;
    private readonly Settings _settings;
    private readonly SettingsSave _settingsSave;
    private readonly EnvConfig _envConfig;
    private readonly DoTheChallenge _doTheChallenge;

    public GatewaysVoidTests()
    {
        Console.WriteLine("Starting Setup");
        _installmentPlans = new InstallmentPlans();
        _settings = new Settings();
        _settingsSave = new SettingsSave();
        _openAuthorizationsFunctionality = new OpenAuthorizationsFunctionality();
        _envConfig = new EnvConfig();
        _doTheChallenge = new DoTheChallenge();
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
    
    [TestCase(Category = "GatewaysVoidTests")]
    [Test(Description = "CreatePlanWithTerminalWorldPayVoid")]
    public async Task CreatePlanWithTerminalWorldPayVoid()
    {
        try
        {
            Console.WriteLine("\nStarting CreatePlanWithTerminalWorldPayVoid");
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            var planCreateResponse = await _settings.SendGetSettingsRequestAsync( _requestHeader!, int.Parse(_envConfig.NgMockV2BU));
            planCreateResponse!.MerchantSettings.DefaultPlanStrategy = "SecuredPlan3";
            var planCreateResponseUpdated = await _settingsSave.SendUpdateSettingsRequestAsync( _requestHeader!, planCreateResponse);
            Assert.That(planCreateResponseUpdated.MerchantSettings.DefaultPlanStrategy, Is.EqualTo("SecuredPlan3"));
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            createPlanDefaultValues.paymentMethod.card.cardNumber = _envConfig.Card4111;
            createPlanDefaultValues.planData.totalAmount = AmountGenerator.GenerateAmountWithMinMaxValues(100, 4000);
            var json = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!,  _requestHeader!, "Active", 
                new Random().Next(4, 12), _envConfig.WorldPayTerminal, createPlanDefaultValues);
            var planCreateResponseVoid = await _openAuthorizationsFunctionality.SendPostRequestOpenAuthorizationsAsync( _requestHeader!, json.InstallmentPlanNumber, true);
            Assert.That(planCreateResponseVoid.ResponseHeader.Succeeded);
            Console.WriteLine("CreatePlanWithTerminalWorldPayVoid is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in CreatePlanWithTerminalWorldPayVoid \n" + exception + "\n");
        }
    }
    
    [TestCase(Category = "GatewaysVoidTests")]
    [Test(Description = "CreatePlanWithTerminalCyberSourceVoid")]
    public async Task CreatePlanWithTerminalCyberSourceVoid()
    {
        try
        {
            Console.WriteLine("\nStarting CreatePlanWithTerminalCyberSourceVoid");
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            createPlanDefaultValues.paymentMethod.card.cardNumber = _envConfig.Card4456;
            createPlanDefaultValues.paymentMethod.card.cardExpMonth = int.Parse(_envConfig.Card4456mth);
            createPlanDefaultValues.paymentMethod.card.cardExpYear = int.Parse(_envConfig.Card4456year);
            createPlanDefaultValues.planData.currency = "GBP";
            createPlanDefaultValues.planData.totalAmount = AmountGenerator.GenerateAmountWithMinMaxValues(10, 999);
            var json = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!,  _requestHeader!, "Active", 
                new Random().Next(4, 12), _envConfig.CyberSourceTerminal, createPlanDefaultValues);
            var planCreateResponseVoid = await _openAuthorizationsFunctionality.SendPostRequestOpenAuthorizationsAsync( _requestHeader!, json.InstallmentPlanNumber, true);
            Assert.That(planCreateResponseVoid.ResponseHeader.Succeeded);
            Console.WriteLine("CreatePlanWithTerminalCyberSourceVoid is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in CreatePlanWithTerminalCyberSourceVoid \n" + exception + "\n");
        }
    }
    
    [TestCase(Category = "GatewaysVoidTests")]
    [Test(Description = "CreatePlanWithTerminalCheckoutVoid")]
    public async Task CreatePlanWithTerminalCheckoutVoid()
    {
        try
        {
            Console.WriteLine("\nStarting CreatePlanWithTerminalCheckoutVoid");
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            var json = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!,  _requestHeader!, "Active", 
                new Random().Next(4, 12), _envConfig.CheckoutTerminal, createPlanDefaultValues);
            var planCreateResponseVoid = await _openAuthorizationsFunctionality.SendPostRequestOpenAuthorizationsAsync( _requestHeader!, json.InstallmentPlanNumber, true);
            Assert.That(planCreateResponseVoid.ResponseHeader.Succeeded);
            Console.WriteLine("CreatePlanWithTerminalCheckoutVoid is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in CreatePlanWithTerminalCheckoutVoid \n" + exception + "\n");
        }
    }
    
    [TestCase(Category = "GatewaysVoidTests")]
    [Test(Description = "CreatePlanWithTerminalBluesnapMorVoid")]
    public async Task CreatePlanWithTerminalBluesnapMorVoid()
    {
        try
        {
            Console.WriteLine("\nStarting CreatePlanWithTerminalBluesnapMorVoid");
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            var json = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!,  _requestHeader!, "Active", 
                new Random().Next(4, 12), _envConfig.BlueSnapMORTerminal, createPlanDefaultValues);
            var planCreateResponseVoid = await _openAuthorizationsFunctionality.SendPostRequestOpenAuthorizationsAsync( _requestHeader!, json.InstallmentPlanNumber, true);
            Assert.That(planCreateResponseVoid.ResponseHeader.Succeeded);
            Console.WriteLine("CreatePlanWithTerminalBluesnapMorVoid is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in CreatePlanWithTerminalBluesnapMorVoid \n" + exception + "\n");
        }
    }
    
    [TestCase(Category = "GatewaysVoidTests")]
    [Test(Description = "CreatePlanWithTerminalBluesnapDirectVoid")]
    public async Task CreatePlanWithTerminalBluesnapDirectVoid()
    {
        try
        {
            Console.WriteLine("\nStarting CreatePlanWithTerminalBluesnapDirectVoid");
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            var json = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!,  _requestHeader!, "Active", 
                new Random().Next(4, 12), _envConfig.BlueSnapDirectTerminal, createPlanDefaultValues);
            var planCreateResponseVoid = await _openAuthorizationsFunctionality.SendPostRequestOpenAuthorizationsAsync( _requestHeader!, json.InstallmentPlanNumber, true);
            Assert.That(planCreateResponseVoid.ResponseHeader.Succeeded);
            Console.WriteLine("CreatePlanWithTerminalBluesnapDirectVoid is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in CreatePlanWithTerminalBluesnapDirectVoid \n" + exception + "\n");
        }
    }
    
    // [TestCase(Category = "GatewaysVoidTests")]
    // [Test(Description = "CreatePlanWithTerminalBamboraVoid")]
    // public async Task CreatePlanWithTerminalBamboraVoid()
    // {
    //     try
    //     {
    //         Console.WriteLine("\nStarting CreatePlanWithTerminalBamboraVoid");
    //         var createPlanDefaultValues = new CreatePlanDefaultValues();
    //         var is3DsVarsCreation = new Is3DsVarsCreation();
    //         var planCreateResponse = await _settings.SendGetSettingsRequestAsync( _requestHeader!, int.Parse(_envConfig.NgMockV2BU));
    //         planCreateResponse!.MerchantSettings.DefaultPlanStrategy = "SecuredPlan4";
    //         var planCreateResponseUpdated = await _settingsSave.SendUpdateSettingsRequestAsync( _requestHeader!, planCreateResponse);
    //         Assert.That(planCreateResponseUpdated.MerchantSettings.DefaultPlanStrategy, Is.EqualTo("SecuredPlan4"));
    //         createPlanDefaultValues.planData.totalAmount = AmountGenerator.GenerateAmountWithMinMaxValues(10, 99);
    //         createPlanDefaultValues.paymentMethod.card.cardNumber = _envConfig.Card5100;
    //         createPlanDefaultValues.paymentMethod.card.cardCvv = "123";
    //         createPlanDefaultValues.planData.currency = "GBP";
    //         createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
    //         var json = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!,  _requestHeader!, "Active", 
    //             new Random().Next(4, 12), _envConfig.BamboraTerminal, createPlanDefaultValues);
    //         
    //         //var startInstallmentsResponse = await _startInstallmentsFunctionality.SendStartInstallmentsRequestAsync( _requestHeader!, json.InstallmentPlanNumber);
    //         //Assert.That(startInstallmentsResponse.ResponseHeader.Succeeded);
    //         var planCreateResponseVoid = await _openAuthorizationsFunctionality.SendPostRequestOpenAuthorizationsAsync( _requestHeader!, json.InstallmentPlanNumber, true);
    //         Assert.That(planCreateResponseVoid.ResponseHeader.Succeeded);
    //         Console.WriteLine("CreatePlanWithTerminalBamboraVoid is Done\n");
    //     }
    //     catch (Exception exception)
    //     {
    //         Assert.Fail("Error in CreatePlanWithTerminalBamboraVoid \n" + exception + "\n");
    //     }
    // }
    
    [TestCase(Category = "GatewaysVoidTests")]
    [Test(Description = "CreatePlanWithTerminalAuthNetV2Void")]
    public async Task CreatePlanWithTerminalAuthNetV2Void()
    {
        try
        {
            Console.WriteLine("\nStarting CreatePlanWithTerminalAuthNetV2Void");
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            var json = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!,  _requestHeader!, "Active", 
                new Random().Next(4, 12), _envConfig.AuthorizeNetV2, createPlanDefaultValues);
            var planCreateResponseVoid = await _openAuthorizationsFunctionality.SendPostRequestOpenAuthorizationsAsync( _requestHeader!, json.InstallmentPlanNumber, true);
            Assert.That(planCreateResponseVoid.ResponseHeader.Succeeded);
            Console.WriteLine("CreatePlanWithTerminalAuthNetV2Void is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in CreatePlanWithTerminalAuthNetV2Void \n" + exception + "\n");
        }
    }
    
    [TestCase(Category = "GatewaysVoidTests")]
    [Test(Description = "CreatePlanWithTerminalStripeDirectVoid")]
    public async Task CreatePlanWithTerminalStripeDirectVoid()
    {
        try
        {
            Console.WriteLine("\nStarting CreatePlanWithTerminalStripeDirectVoid");
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            var json = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!,  _requestHeader!, "Active", 
                new Random().Next(4, 12), _envConfig.StripeDirectTerminal, createPlanDefaultValues);
            var planCreateResponseVoid = await _openAuthorizationsFunctionality.SendPostRequestOpenAuthorizationsAsync( _requestHeader!, json.InstallmentPlanNumber, true);
            Assert.That(planCreateResponseVoid.ResponseHeader.Succeeded);
            Console.WriteLine("CreatePlanWithTerminalStripeDirectVoid is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in CreatePlanWithTerminalStripeDirectVoid \n" + exception + "\n");
        }
    }
    
    // [TestCase(Category = "GatewaysVoidTests")]
    // [Test(Description = "CreatePlanWithTerminalSagePayVoid")]
    // public async Task CreatePlanWithTerminalSagePayVoid()
    // {
    //     try
    //     {
    //         Console.WriteLine("\nStarting CreatePlanWithTerminalSagePayVoid");
    //         var createPlanDefaultValues = new CreatePlanDefaultValues();
    //         var is3DsVarsCreation = new Is3DsVarsCreation();
    //         var planCreateResponse = await _settings.SendGetSettingsRequestAsync( _requestHeader!, int.Parse(_envConfig.NgMockV2BU));
    //         planCreateResponse!.MerchantSettings.DefaultPlanStrategy = "SecuredPlan3";
    //         var planCreateResponseUpdated = await _settingsSave.SendUpdateSettingsRequestAsync( _requestHeader!, planCreateResponse);
    //         Assert.That(planCreateResponseUpdated.MerchantSettings.DefaultPlanStrategy, Is.EqualTo("SecuredPlan3"));
    //         createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "yes");
    //         createPlanDefaultValues.paymentMethod.card.cardNumber = _envConfig.Card4929;
    //         createPlanDefaultValues.paymentMethod.card.cardCvv = _envConfig.Card4929Cvv;
    //         createPlanDefaultValues.planData.currency = "GBP";
    //         createPlanDefaultValues.planData.totalAmount = AmountGenerator.GenerateAmountWithMinMaxValues(10, 99);
    //         var json = await _installmentPlans.CreatePlanInitiateAsync(Environment.GetEnvironmentVariable("ApiV3")!,  _requestHeader!, "Active", 
    //             new Random().Next(4, 12), _envConfig.SagePayTerminal, createPlanDefaultValues);
    //         Assert.AreEqual("Initialized", json.Status);
    //         Assert.That(await _doTheChallenge.DoTheChallengeV4Pf4Async(json, createPlanDefaultValues, 
    //             createPlanDefaultValues.planData.numberOfInstallments, "SagePay", null!, _requestHeader!));
    //         var startInstallmentsResponse = await _startInstallmentsFunctionality.SendStartInstallmentsRequestAsync( _requestHeader!, json.InstallmentPlanNumber);
    //         Assert.That(startInstallmentsResponse.ResponseHeader.Succeeded);
    //         var planCreateResponseVoid = await _openAuthorizationsFunctionality.SendPostRequestOpenAuthorizationsAsync( _requestHeader!, json.InstallmentPlanNumber, true);
    //         Assert.That(planCreateResponseVoid.ResponseHeader.Succeeded);
    //         Console.WriteLine("CreatePlanWithTerminalSagePayVoid is Done\n");
    //     }
    //     catch (Exception exception)
    //     {
    //         Assert.Fail("Error in CreatePlanWithTerminalSagePayVoid \n" + exception + "\n");
    //     }
    // }
    
    [TestCase(Category = "GatewaysVoidTests")]
    [Test(Description = "CreatePlanWithTerminalReachVoid")]
    public async Task CreatePlanWithTerminalReachVoid()
    {
        try
        {
            Console.WriteLine("\nStarting CreatePlanWithTerminalReachVoid");
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            var json = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!,  _requestHeader!, "Active", 
                new Random().Next(4, 12), _envConfig.ReachTerminal, createPlanDefaultValues);
            var planCreateResponseVoid = await _openAuthorizationsFunctionality.SendPostRequestOpenAuthorizationsAsync( _requestHeader!, json.InstallmentPlanNumber, true);
            Assert.That(planCreateResponseVoid.ResponseHeader.Succeeded);
            Console.WriteLine("CreatePlanWithTerminalReachVoid is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in CreatePlanWithTerminalReachVoid \n" + exception + "\n");
        }
    }
    
    [TestCase(Category = "GatewaysVoidTests")]
    [Test(Description = "CreatePlanWithTerminalPaySafeVoid")]
    public async Task CreatePlanWithTerminalPaySafeVoid()
    {
        try
        {
            Console.WriteLine("\nStarting CreatePlanWithTerminalPaySafeVoid");
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            var json = await _installmentPlans.CreatePlanInitiateAsync(Environment.GetEnvironmentVariable("ApiV3")!,  _requestHeader!,
                "Active", new Random().Next(4, 12), _envConfig.PaySafeTerminal, 
                createPlanDefaultValues);
            Assert.That(json.Status, Is.EqualTo("Initialized"));
            Assert.That(await _doTheChallenge.DoTheChallengeV4Pf4Async(json, createPlanDefaultValues,
                12, "PaySafe", null,  _requestHeader!), Is.True);
            var planCreateResponseVoid = await _openAuthorizationsFunctionality.SendPostRequestOpenAuthorizationsAsync(
                _requestHeader!, json.InstallmentPlanNumber, true);
            Assert.That(planCreateResponseVoid.ResponseHeader.Succeeded);
            Console.WriteLine("CreatePlanWithTerminalPaySafeVoid is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in CreatePlanWithTerminalPaySafeVoid \n" + exception + "\n");
        }
    }
    
    [TestCase(Category = "GatewaysVoidTests")]
    [Test(Description = "CreatePlanWithTerminalAdyenVoid")]
    public async Task CreatePlanWithTerminalAdyenVoid()
    {
        try
        {
            Console.WriteLine("\nStarting CreatePlanWithTerminalAdyenVoid");
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            var json = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!,  _requestHeader!, "Active", 
                new Random().Next(4, 12), _envConfig.AdyenTerminal, createPlanDefaultValues);
            var planCreateResponseVoid = await _openAuthorizationsFunctionality.SendPostRequestOpenAuthorizationsAsync( _requestHeader!, json.InstallmentPlanNumber, true);
            Assert.That(planCreateResponseVoid.ResponseHeader.Succeeded);
            Console.WriteLine("CreatePlanWithTerminalAdyenVoid is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in CreatePlanWithTerminalAdyenVoid \n" + exception + "\n");
        }
    }
    
    [TestCase(Category = "GatewaysVoidTests")]
    [Test(Description = "CreatePlanWithTerminalSpreedlyVoid")]
    public async Task CreatePlanWithTerminalSpreedlyVoid()
    {
        try
        {
            Console.WriteLine("\nStarting CreatePlanWithTerminalSpreedlyVoid");
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            var json = await _installmentPlans.CreatePlanAsync(
                Environment.GetEnvironmentVariable("ApiV3")!,  _requestHeader!, "Active", 
                new Random().Next(4, 12), _envConfig.SpreedlyTerminal, 
                createPlanDefaultValues);
            var planCreateResponseVoid = await _openAuthorizationsFunctionality.SendPostRequestOpenAuthorizationsAsync( _requestHeader!, json.InstallmentPlanNumber, true);
            Assert.That(planCreateResponseVoid.ResponseHeader.Succeeded);
            Console.WriteLine("CreatePlanWithTerminalSpreedlyVoid is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in CreatePlanWithTerminalSpreedlyVoid \n" + exception + "\n");
        }
    }
}