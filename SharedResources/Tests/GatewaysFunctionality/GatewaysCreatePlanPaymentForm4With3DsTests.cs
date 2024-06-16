using Allure.NUnit;
using Allure.NUnit.Attributes;
using NUnit.Framework;
using Splitit.Automation.NG.Backend.BaseActions;
using Splitit.Automation.NG.Backend.Services.AdminApi.Merchant.MerchantFunctionality;
using Splitit.Automation.NG.Backend.Services.V3.DefaultValues;
using Splitit.Automation.NG.Backend.Services.V3.InstallmentPlan.InstallmentPlanApiEndPoint;
using Splitit.Automation.NG.Backend.Tests.TestsSetups;
using Splitit.Automation.NG.SharedResources.BaseActions;
using Splitit.Automation.NG.Utilities.EnvironmentsConfig;

namespace Splitit.Automation.NG.SharedResources.Tests.GatewaysFunctionality;

[TestFixture]
[AllureNUnit]
[AllureSuite("GatewaysCreatePlanPaymentForm4With3DsTests")]
[AllureDisplayIgnored]
//[Parallelizable(ParallelScope.All)]
public class GatewaysCreatePlanPaymentForm4With3DsTests
{
    private readonly InstallmentPlans _installmentPlans; 
    private RequestHeader? _requestHeader;
    private readonly DoTheChallenge _doTheChallenge;
    private readonly Settings _settings;
    private readonly SettingsSave _settingsSave;
    private readonly EnvConfig _envConfig;

    public GatewaysCreatePlanPaymentForm4With3DsTests()
    {
        Console.WriteLine("Starting Setup");
        _installmentPlans = new InstallmentPlans();
        _doTheChallenge = new DoTheChallenge();
        _settings = new Settings();
        _settingsSave = new SettingsSave();
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
    
    [TestCase(Category = "GatewaysCreatePlanPaymentForm4With3DsTests")]
    [Test(Description = "CreatePlanWithTerminalCheckoutWith3Ds")]
    public async Task CreatePlanWithTerminalCheckoutWith3Ds()
    {
        try
        {
            Console.WriteLine("\nStarting CreatePlanWithTerminalCheckoutWith3Ds");
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no_exemption_with_params");
            var planCreateResponse = await _installmentPlans.CreatePlanInitiateAsync(Environment.GetEnvironmentVariable("ApiV3")!,  _requestHeader!, "Active", 
                new Random().Next(2, 6), _envConfig.CheckoutTerminal, createPlanDefaultValues);
            Assert.That(planCreateResponse.Status, Is.EqualTo("Initialized"));
            Assert.That(await _doTheChallenge.DoTheChallengeV4Pf4Async(planCreateResponse, createPlanDefaultValues, 12, "Checkout", null,  _requestHeader!), Is.True);
            Console.WriteLine("CreatePlanWithTerminalCheckoutWith3Ds is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in CreatePlanWithTerminalCheckoutWith3Ds \n" + exception + "\n");
        }
    }
    
    [TestCase(Category = "GatewaysCreatePlanPaymentForm4With3DsTests")]
    [Test(Description = "CreatePlanWithTerminalBlueSnapMorWith3Ds")]
    public async Task CreatePlanWithTerminalBlueSnapMorWith3Ds()
    {
        try
        {
            Console.WriteLine("\nStarting CreatePlanWithTerminalBlueSnapMorWith3Ds");
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "yes");
            var planCreateResponse = await _installmentPlans.CreatePlanInitiateAsync(Environment.GetEnvironmentVariable("ApiV3")!,  _requestHeader!, "Active", 
                new Random().Next(2, 6), _envConfig.BlueSnapMORTerminal, createPlanDefaultValues);
            Assert.That(planCreateResponse.Status, Is.EqualTo("Initialized"));
            Assert.That(await _doTheChallenge.DoTheChallengeV4Pf4Async(planCreateResponse, createPlanDefaultValues, 12, "BlueSnapMor", null,  _requestHeader!), Is.True);
            Console.WriteLine("CreatePlanWithTerminalBlueSnapMorWith3Ds is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in CreatePlanWithTerminalBlueSnapMorWith3Ds \n" + exception + "\n");
        }
    }
    
     [TestCase(Category = "GatewaysCreatePlanPaymentForm4With3DsTests")]
    [Test(Description = "CreatePlanWithTerminalBlueSnapDirectWith3Ds")]
    public async Task CreatePlanWithTerminalBlueSnapDirectWith3Ds()
    {
        try
        {
            Console.WriteLine("\nStarting CreatePlanWithTerminalBlueSnapDirectWith3Ds");
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "yes");
            var jResponse = await _settings.SendGetSettingsRequestAsync( _requestHeader!, int.Parse(_envConfig.NgMockV2BU));
            jResponse!.MerchantSettings.DefaultPlanStrategy = "SecuredPlan4";
            var jResponseUpdated = await _settingsSave.SendUpdateSettingsRequestAsync( _requestHeader!, jResponse);
            Assert.That(jResponseUpdated.MerchantSettings.DefaultPlanStrategy, Is.EqualTo("SecuredPlan4"));
            var planCreateResponse = await _installmentPlans.CreatePlanInitiateAsync(Environment.GetEnvironmentVariable("ApiV3")!,  _requestHeader!, "Active", 
                new Random().Next(2, 6), _envConfig.BlueSnapDirectTerminal, createPlanDefaultValues);
            Assert.That(planCreateResponse.Status, Is.EqualTo("Initialized"));
            Assert.That(await _doTheChallenge.DoTheChallengeV4Pf4Async(planCreateResponse, createPlanDefaultValues, 12, "BlueSnapDirect", null,  _requestHeader!), Is.True);
            var jResponse2 = await _settings.SendGetSettingsRequestAsync( _requestHeader!, int.Parse(_envConfig.NgMockV2BU)); 
            jResponse2!.MerchantSettings.DefaultPlanStrategy = "SecuredPlan"; 
            var jResponseUpdated2 = await _settingsSave.SendUpdateSettingsRequestAsync( _requestHeader!, jResponse2);
            Assert.That(jResponseUpdated2.MerchantSettings.DefaultPlanStrategy, Is.EqualTo("SecuredPlan"));
            Console.WriteLine("CreatePlanWithTerminalBlueSnapDirectWith3Ds is Done\n");
        }
        catch (Exception exception)
        {
            var jResponse2 = await _settings.SendGetSettingsRequestAsync( _requestHeader!, int.Parse(_envConfig.NgMockV2BU)); 
            jResponse2!.MerchantSettings.DefaultPlanStrategy = "SecuredPlan"; 
            var jResponseUpdated2 = await _settingsSave.SendUpdateSettingsRequestAsync( _requestHeader!, jResponse2);
            Assert.That(jResponseUpdated2.MerchantSettings.DefaultPlanStrategy, Is.EqualTo("SecuredPlan"));
            Assert.Fail("Error in CreatePlanWithTerminalBlueSnapDirectWith3Ds \n" + exception + "\n");
        }
    }
    
    [TestCase(Category = "GatewaysCreatePlanPaymentForm4With3DsTests")]
    [Test(Description = "CreatePlanWithTerminalWorldPayWith3Ds")]
    public async Task CreatePlanWithTerminalWorldPayWith3Ds()
    {
        try
        {
            Console.WriteLine("\nStarting CreatePlanWithTerminalWorldPayWith3Ds");
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            var jResponse = await _settings.SendGetSettingsRequestAsync( _requestHeader!, int.Parse(_envConfig.NgMockV2BU));
            jResponse!.MerchantSettings.DefaultPlanStrategy = "SecuredPlan3";
            var jResponseUpdated = await _settingsSave.SendUpdateSettingsRequestAsync( _requestHeader!, jResponse);
            Assert.That(jResponseUpdated.MerchantSettings.DefaultPlanStrategy, Is.EqualTo("SecuredPlan3"));
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "yes");
            createPlanDefaultValues.shopper.fullName = "3DS_V2_CHALLENGE_VALID_ERROR";
            createPlanDefaultValues.paymentMethod.card.cardNumber = Environment.GetEnvironmentVariable("Card5454")!;
            createPlanDefaultValues.planData.totalAmount = AmountGenerator.GenerateAmountWithMinMaxValues(100, 4000);
            var planCreateResponse = await _installmentPlans.CreatePlanInitiateAsync(Environment.GetEnvironmentVariable("ApiV3")!,  _requestHeader!, "Active", 
                new Random().Next(3, 6), _envConfig.WorldPayTerminal, createPlanDefaultValues);
            Assert.That(planCreateResponse.Status, Is.EqualTo("Initialized"));
            Assert.That(await _doTheChallenge.DoTheChallengeV4Pf4Async(planCreateResponse, createPlanDefaultValues, 12, "WorldPay", null,  _requestHeader!), Is.True);
            var jResponse1 = await _settings.SendGetSettingsRequestAsync( _requestHeader!, int.Parse(_envConfig.NgMockV2BU));
            jResponse1!.MerchantSettings.DefaultPlanStrategy = "SecuredPlan";
            var jResponseUpdated1 = await _settingsSave.SendUpdateSettingsRequestAsync( _requestHeader!, jResponse1);
            Assert.That(jResponseUpdated1.MerchantSettings.DefaultPlanStrategy, Is.EqualTo("SecuredPlan"));
            Console.WriteLine("CreatePlanWithTerminalWorldPayWith3Ds is Done\n");
        }
        catch (Exception exception)
        {
            var jResponse1 = await _settings.SendGetSettingsRequestAsync( _requestHeader!, int.Parse(_envConfig.NgMockV2BU));
            jResponse1!.MerchantSettings.DefaultPlanStrategy = "SecuredPlan";
            var jResponseUpdated1 = await _settingsSave.SendUpdateSettingsRequestAsync( _requestHeader!, jResponse1);
            Assert.That(jResponseUpdated1.MerchantSettings.DefaultPlanStrategy, Is.EqualTo("SecuredPlan"));
            Assert.Fail("Error in CreatePlanWithTerminalWorldPayWith3Ds \n" + exception + "\n");
        }
    }
    
    [TestCase(Category = "GatewaysCreatePlanPaymentForm4With3DsTests")]
    [Test(Description = "CreatePlanWithTerminalStripeDirectWith3Ds")]
    public async Task CreatePlanWithTerminalStripeDirectWith3Ds()
    {
        try
        {
            Console.WriteLine("\nStarting CreatePlanWithTerminalStripeDirectWith3Ds");
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "yes");
            var planCreateResponse = await _installmentPlans.CreatePlanInitiateAsync(Environment.GetEnvironmentVariable("ApiV3")!,  _requestHeader!, "Active", 
                new Random().Next(2, 6), _envConfig.StripeDirectTerminal, createPlanDefaultValues);
            Assert.That(planCreateResponse.Status, Is.EqualTo("Initialized"));
            Assert.That(await _doTheChallenge.DoTheChallengeV4Pf4Async(planCreateResponse, createPlanDefaultValues, 12, "StripeDirect", null,  _requestHeader!), Is.True);
            Console.WriteLine("CreatePlanWithTerminalStripeDirectWith3Ds is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in CreatePlanWithTerminalStripeDirectWith3Ds \n" + exception + "\n");
        }
    }
    
    // [TestCase(Category = "GatewaysCreatePlanPaymentForm4With3DsTests")]
    // [Test(Description = "CreatePlanWithTerminalSagePayWith3Ds")]
    // public async Task CreatePlanWithTerminalSagePayWith3Ds()
    // {
    //     try
    //     {
    //         Console.WriteLine("\nStarting CreatePlanWithTerminalSagePayWith3Ds");
    //         var createPlanDefaultValues = new CreatePlanDefaultValues();
    //         var is3DsVarsCreation = new Is3DsVarsCreation();
    //         var jResponse = await _settings.SendGetSettingsRequestAsync( _requestHeader!, int.Parse(_envConfig.NgMockV2BU));
    //         jResponse!.MerchantSettings.DefaultPlanStrategy = "SecuredPlan3";
    //         var jResponseUpdated = await _settingsSave.SendUpdateSettingsRequestAsync( _requestHeader!, jResponse);
    //         Assert.That(jResponseUpdated.MerchantSettings.DefaultPlanStrategy, Is.EqualTo("SecuredPlan3"));
    //         createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "yes");
    //         createPlanDefaultValues.paymentMethod.card.cardNumber = _envConfig.Card4929;
    //         createPlanDefaultValues.paymentMethod.card.cardCvv = _envConfig.Card4929Cvv;
    //         createPlanDefaultValues.planData.currency = "GBP";
    //         createPlanDefaultValues.planData.totalAmount = AmountGenerator.GenerateAmountWithMinMaxValues(10, 99);
    //         createPlanDefaultValues.planData.refOrderNumber = "AutomationRun";
    //         var planCreateResponse = await _installmentPlans.CreatePlanInitiateAsync(Environment.GetEnvironmentVariable("ApiV3")!,  _requestHeader!, "Active", 
    //             new Random().Next(2, 6), _envConfig.SagePayTerminal, createPlanDefaultValues);
    //         Assert.That(planCreateResponse.Status, Is.EqualTo("Initialized"));
    //         Assert.That(await _doTheChallenge.DoTheChallengeV4Pf4Async(planCreateResponse, createPlanDefaultValues, 12, 
    //             "SagePay", null,  _requestHeader!), Is.True);
    //         var jResponse1 = await _settings.SendGetSettingsRequestAsync( _requestHeader!, int.Parse(_envConfig.NgMockV2BU));
    //         jResponse1!.MerchantSettings.DefaultPlanStrategy = "SecuredPlan";
    //         var jResponseUpdated1 = await _settingsSave.SendUpdateSettingsRequestAsync( _requestHeader!, jResponse1);
    //         Assert.That(jResponseUpdated1.MerchantSettings.DefaultPlanStrategy, Is.EqualTo("SecuredPlan"));
    //         Console.WriteLine("CreatePlanWithTerminalSagePayWith3Ds is Done\n");
    //     }
    //     catch (Exception exception)
    //     {
    //         var jResponse1 = await _settings.SendGetSettingsRequestAsync( _requestHeader!, int.Parse(_envConfig.NgMockV2BU));
    //         jResponse1!.MerchantSettings.DefaultPlanStrategy = "SecuredPlan";
    //         var jResponseUpdated1 = await _settingsSave.SendUpdateSettingsRequestAsync( _requestHeader!, jResponse1);
    //         Assert.That(jResponseUpdated1.MerchantSettings.DefaultPlanStrategy, Is.EqualTo("SecuredPlan"));
    //         Assert.Fail("Error in CreatePlanWithTerminalSagePayWith3Ds \n" + exception + "\n");
    //     }
    // }
    
    [TestCase(Category = "GatewaysCreatePlanPaymentForm4With3DsTests")]
    [Test(Description = "CreatePlanWithTerminalReachWith3Ds")]
    public async Task CreatePlanWithTerminalReachWith3Ds()
    {
        try
        {
            Console.WriteLine("\nStarting CreatePlanWithTerminalReachWith3Ds");
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "yes");
            createPlanDefaultValues.paymentMethod.card.cardNumber = Environment.GetEnvironmentVariable("Card5454")!;
            var planCreateResponse = await _installmentPlans.CreatePlanInitiateAsync(Environment.GetEnvironmentVariable("ApiV3")!,  _requestHeader!, "Active", 
                new Random().Next(2, 6), _envConfig.ReachTerminal, createPlanDefaultValues);
            Assert.That(planCreateResponse.Status, Is.EqualTo("Initialized"));
            Assert.That(await _doTheChallenge.DoTheChallengeV4Pf4Async(planCreateResponse, createPlanDefaultValues, 12, "Reach", null,  _requestHeader!), Is.True);
            Console.WriteLine("CreatePlanWithTerminalReachWith3Ds is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in CreatePlanWithTerminalReachWith3Ds \n" + exception + "\n");
        }
    }
    
    [TestCase(Category = "GatewaysCreatePlanPaymentForm4With3DsTests")]
    [Test(Description = "CreatePlanWithTerminalPaySafeWith3Ds")]
    public async Task CreatePlanWithTerminalPaySafeWith3Ds()
    {
        try
        {
            Console.WriteLine("\nStarting CreatePlanWithTerminalPaySafeWith3Ds");
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "yes");
            var planCreateResponse = await _installmentPlans.CreatePlanInitiateAsync(Environment.GetEnvironmentVariable("ApiV3")!,  _requestHeader!, "Active", 
                new Random().Next(2, 6), _envConfig.PaySafeTerminal, createPlanDefaultValues);
            Assert.That(planCreateResponse.Status, Is.EqualTo("Initialized"));
            Assert.That(await _doTheChallenge.DoTheChallengeV4Pf4Async(planCreateResponse, createPlanDefaultValues, 12, "PaySafe", null,  _requestHeader!), Is.True);
            Console.WriteLine("CreatePlanWithTerminalPaySafeWith3Ds is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in CreatePlanWithTerminalPaySafeWith3Ds \n" + exception + "\n");
        }
    }
    
    [TestCase(Category = "GatewaysCreatePlanPaymentForm4With3DsTests")]
    [Test(Description = "CreatePlanWithTerminalAdyenWith3Ds")]
    public async Task CreatePlanWithTerminalAdyenWith3Ds()
    {
        try
        {
            Console.WriteLine("\nStarting CreatePlanWithTerminalAdyenWith3Ds");
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "yes");
            createPlanDefaultValues.paymentMethod.card.cardNumber = Environment.GetEnvironmentVariable("Card5454")!;
            var planCreateResponse = await _installmentPlans.CreatePlanInitiateAsync(Environment.GetEnvironmentVariable("ApiV3")!,  _requestHeader!, "Active", 
                new Random().Next(2, 6), _envConfig.AdyenTerminal, createPlanDefaultValues);
            Assert.That(planCreateResponse.Status, Is.EqualTo("Initialized"));
            Assert.That(await _doTheChallenge.DoTheChallengeV4Pf4Async(planCreateResponse, createPlanDefaultValues, 12, "Adyen", null,  _requestHeader!), Is.True);
            Console.WriteLine("CreatePlanWithTerminalAdyenWith3Ds is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in CreatePlanWithTerminalAdyenWith3Ds \n" + exception + "\n");
        }
    }
}