using Allure.NUnit;
using Allure.NUnit.Attributes;
using NUnit.Framework;
using Splitit.Automation.NG.Backend.BaseActions;
using Splitit.Automation.NG.Backend.Services.V3.DefaultValues;
using Splitit.Automation.NG.Backend.Services.V3.InstallmentPlan.InstallmentPlanApiEndPoint;
using Splitit.Automation.NG.Backend.Tests.TestsSetups;
using Splitit.Automation.NG.SharedResources.BaseActions;
using Splitit.Automation.NG.Utilities.EnvironmentsConfig;

namespace Splitit.Automation.NG.SharedResources.Tests.GatewaysFunctionality;

[TestFixture]
[AllureNUnit]
[AllureSuite("GatewaysCreatePlanPaymentForm4Without3DsTests")]
[AllureDisplayIgnored]
[Parallelizable(ParallelScope.All)]
public class GatewaysCreatePlanPaymentForm4Without3DsTests
{
    private readonly InstallmentPlans _installmentPlans;
    private RequestHeader? _requestHeader;
    private readonly DoTheChallenge _doTheChallenge;
    private readonly EnvConfig _envConfig;

    public GatewaysCreatePlanPaymentForm4Without3DsTests()
    {
        Console.WriteLine("Starting Setup");
        _installmentPlans = new InstallmentPlans();
        _doTheChallenge = new DoTheChallenge();
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
    
    [TestCase(Category = "GatewaysCreatePlanPaymentForm4Without3DsTests")]
    [Test(Description = "CreatePlanWithTerminalCyberSourceWithout3Ds")]
    public async Task CreatePlanWithTerminalCyberSourceWithout3Ds()
    {
        try
        {
            Console.WriteLine("\nStarting CreatePlanWithTerminalCyberSourceWithout3Ds");
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            createPlanDefaultValues.paymentMethod.card.cardNumber = "5555555555554444";
            createPlanDefaultValues.planData.currency = "GBP";
            var planCreateResponse = await _installmentPlans.CreatePlanInitiateAsync(Environment.GetEnvironmentVariable("ApiV3")!,  _requestHeader!, "Active", 
                new Random().Next(2, 6), _envConfig.CyberSourceTerminal, createPlanDefaultValues);
            Assert.That(planCreateResponse.Status, Is.EqualTo("Initialized"));
            Assert.That(await _doTheChallenge.DoTheChallengeV4Pf4Async(planCreateResponse, createPlanDefaultValues, 12, null!, null,  _requestHeader!), Is.True);
            Console.WriteLine("CreatePlanWithTerminalCyberSourceWithout3Ds is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in CreatePlanWithTerminalCyberSourceWithout3Ds \n" + exception + "\n");
        }
    }
    
    [TestCase(Category = "GatewaysCreatePlanPaymentForm4Without3DsTests")]
    [Test(Description = "CreatePlanWithTerminalCheckoutWithout3Ds")]
    public async Task CreatePlanWithTerminalCheckoutWithout3Ds()
    {
        try
        {
            Console.WriteLine("\nStarting CreatePlanWithTerminalCheckoutWithout3Ds");
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            var planCreateResponse = await _installmentPlans.CreatePlanInitiateAsync(Environment.GetEnvironmentVariable("ApiV3")!,  _requestHeader!, "Active", 
                new Random().Next(2, 6), _envConfig.CheckoutTerminal, createPlanDefaultValues);
            Assert.That(planCreateResponse.Status, Is.EqualTo("Initialized"));
            Assert.That(await _doTheChallenge.DoTheChallengeV4Pf4Async(planCreateResponse, createPlanDefaultValues, 12, null!, null,  _requestHeader!), Is.True);
            Console.WriteLine("CreatePlanWithTerminalCheckoutWithout3Ds is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in CreatePlanWithTerminalCheckoutWithout3Ds \n" + exception + "\n");
        }
    }
    
    [TestCase(Category = "GatewaysCreatePlanPaymentForm4Without3DsTests")]
    [Test(Description = "CreatePlanWithTerminalBlueSnapMorWithout3Ds")]
    public async Task CreatePlanWithTerminalBlueSnapMorWithout3Ds()
    {
        try
        {
            Console.WriteLine("\nStarting CreatePlanWithTerminalBlueSnapMorWithout3Ds");
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            var planCreateResponse = await _installmentPlans.CreatePlanInitiateAsync(Environment.GetEnvironmentVariable("ApiV3")!,  _requestHeader!, "Active", 
                new Random().Next(2, 6), _envConfig.BlueSnapMORTerminal, createPlanDefaultValues);
            Assert.That(planCreateResponse.Status, Is.EqualTo("Initialized"));
            Assert.That(await _doTheChallenge.DoTheChallengeV4Pf4Async(planCreateResponse, createPlanDefaultValues, 12, null!, null,  _requestHeader!), Is.True);
            Console.WriteLine("CreatePlanWithTerminalBlueSnapMorWithout3Ds is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in CreatePlanWithTerminalBlueSnapMorWithout3Ds \n" + exception + "\n");
        }
    }
    
    [TestCase(Category = "GatewaysCreatePlanPaymentForm4Without3DsTests")]
    [Test(Description = "CreatePlanWithTerminalBlueSnapDirectWithout3Ds")]
    public async Task CreatePlanWithTerminalBlueSnapDirectWithout3Ds()
    {
        try
        {
            Console.WriteLine("\nStarting CreatePlanWithTerminalBlueSnapDirectWithout3Ds");
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            var planCreateResponse = await _installmentPlans.CreatePlanInitiateAsync(Environment.GetEnvironmentVariable("ApiV3")!,  _requestHeader!, "Active", 
                new Random().Next(2, 6), _envConfig.BlueSnapDirectTerminal, createPlanDefaultValues);
            Assert.That(planCreateResponse.Status, Is.EqualTo("Initialized"));
            Assert.That(await _doTheChallenge.DoTheChallengeV4Pf4Async(planCreateResponse, createPlanDefaultValues, 12, null!, null,  _requestHeader!), Is.True);
            Console.WriteLine("CreatePlanWithTerminalBlueSnapDirectWithout3Ds is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in CreatePlanWithTerminalBlueSnapDirectWithout3Ds \n" + exception + "\n");
        }
    }
    
    // [TestCase(Category = "GatewaysCreatePlanPaymentForm4Without3DsTests")]
    // [Test(Description = "CreatePlanWithTerminalBamboraWithout3Ds")]
    // public async Task CreatePlanWithTerminalBamboraWithout3Ds()
    // {
    //     try
    //     {
    //         Console.WriteLine("\nStarting CreatePlanWithTerminalBamboraWithout3Ds");
    //         var createPlanDefaultValues = new CreatePlanDefaultValues();
    //         var is3DsVarsCreation = new Is3DsVarsCreation();
    //         createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
    //         var jResponse = await _settings.SendGetSettingsRequestAsync( _requestHeader!, int.Parse(_envConfig.NgMockV2BU));
    //         jResponse!.MerchantSettings.DefaultPlanStrategy = "SecuredPlan4";
    //         var jResponseUpdated = await _settingsSave.SendUpdateSettingsRequestAsync( _requestHeader!, jResponse);
    //         Assert.That(jResponseUpdated.MerchantSettings.DefaultPlanStrategy, Is.EqualTo("SecuredPlan4"));
    //         createPlanDefaultValues.planData.totalAmount = AmountGenerator.GenerateAmountWithMinMaxValues(10, 99);
    //         createPlanDefaultValues.paymentMethod.card.cardNumber = _envConfig.Card5100;
    //         createPlanDefaultValues.paymentMethod.card.cardCvv = "123";
    //         createPlanDefaultValues.planData.currency = "GBP";
    //         var planCreateResponse = await _installmentPlans.CreatePlanInitiateAsync(Environment.GetEnvironmentVariable("ApiV3")!,  _requestHeader!, "Active", 
    //             new Random().Next(2, 6), _envConfig.BamboraTerminal, createPlanDefaultValues);
    //         Assert.That(planCreateResponse.Status, Is.EqualTo("Initialized"));
    //         Assert.That(await _doTheChallenge.DoTheChallengeV4Pf4Async(planCreateResponse, createPlanDefaultValues, 12, "Bambora", null,  _requestHeader!), Is.True);
    //         var jResponse1 = await _settings.SendGetSettingsRequestAsync( _requestHeader!, int.Parse(_envConfig.NgMockV2BU));
    //         jResponse1!.MerchantSettings.DefaultPlanStrategy = "SecuredPlan";
    //         var jResponseUpdated1 = await _settingsSave.SendUpdateSettingsRequestAsync( _requestHeader!, jResponse1);
    //         Assert.That(jResponseUpdated1.MerchantSettings.DefaultPlanStrategy, Is.EqualTo("SecuredPlan"));
    //         Console.WriteLine("CreatePlanWithTerminalBamboraWithout3Ds is Done\n");
    //     }
    //     catch (Exception exception)
    //     { 
    //         var jResponse2 = await _settings.SendGetSettingsRequestAsync( _requestHeader!, int.Parse(_envConfig.NgMockV2BU)); 
    //         jResponse2!.MerchantSettings.DefaultPlanStrategy = "SecuredPlan"; 
    //         var jResponseUpdated2 = await _settingsSave.SendUpdateSettingsRequestAsync( _requestHeader!, jResponse2);
    //         Assert.That(jResponseUpdated2.MerchantSettings.DefaultPlanStrategy, Is.EqualTo("SecuredPlan"));
    //         Assert.Fail("Error in CreatePlanWithTerminalBamboraWithout3Ds \n" + exception + "\n");
    //     }
    // }
    
    [TestCase(Category = "GatewaysCreatePlanPaymentForm4Without3DsTests")]
    [Test(Description = "CreatePlanWithTerminalAuthNetV2Without3Ds")]
    public async Task CreatePlanWithTerminalAuthNetV2Without3Ds()
    {
        try
        {
            Console.WriteLine("\nStarting CreatePlanWithTerminalAuthNetV2Without3Ds");
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            var planCreateResponse = await _installmentPlans.CreatePlanInitiateAsync(Environment.GetEnvironmentVariable("ApiV3")!,  _requestHeader!, "Active", 
                3, _envConfig.AuthorizeNetV2, createPlanDefaultValues);
            Assert.That(planCreateResponse.Status, Is.EqualTo("Initialized"));
            Assert.That(await _doTheChallenge.DoTheChallengeV4Pf4Async(planCreateResponse, createPlanDefaultValues, 12, null!,null,  _requestHeader!), Is.True);
            Console.WriteLine("CreatePlanWithTerminalAuthNetV2Without3Ds is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in CreatePlanWithTerminalAuthNetV2Without3Ds \n" + exception + "\n");
        }
    }
    
    [TestCase(Category = "GatewaysCreatePlanPaymentForm4Without3DsTests")]
    [Test(Description = "CreatePlanWithTerminalWorldPayWithout3Ds")]
    public async Task CreatePlanWithTerminalWorldPayWithout3Ds()
    {
        try
        {
            Console.WriteLine("\nStarting CreatePlanWithTerminalWorldPayWithout3Ds");
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            createPlanDefaultValues.paymentMethod.card.cardNumber = _envConfig.Card5555;
            createPlanDefaultValues.planData.totalAmount = AmountGenerator.GenerateAmountWithMinMaxValues(100, 4000);
            var planCreateResponse = await _installmentPlans.CreatePlanInitiateAsync(Environment.GetEnvironmentVariable("ApiV3")!,  _requestHeader!, "Active", 
                new Random().Next(3, 6), _envConfig.WorldPayTerminal, createPlanDefaultValues);
            Assert.That(planCreateResponse.Status, Is.EqualTo("Initialized"));
            Assert.That(await _doTheChallenge.DoTheChallengeV4Pf4Async(planCreateResponse, createPlanDefaultValues, 12,
                null!,null,  _requestHeader!), Is.True);
            Console.WriteLine("CreatePlanWithTerminalWorldPayWithout3Ds is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in CreatePlanWithTerminalWorldPayWithout3Ds \n" + exception + "\n");
        }
    }
    
    [TestCase(Category = "GatewaysCreatePlanPaymentForm4Without3DsTests")]
    [Test(Description = "CreatePlanWithTerminalStripeDirectWithout3Ds")]
    public async Task CreatePlanWithTerminalStripeDirectWithout3Ds()
    {
        try
        {
            Console.WriteLine("\nStarting CreatePlanWithTerminalStripeDirectWithout3Ds");
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            var planCreateResponse = await _installmentPlans.CreatePlanInitiateAsync(Environment.GetEnvironmentVariable("ApiV3")!,  _requestHeader!, "Active", 
                new Random().Next(2, 6), _envConfig.StripeDirectTerminal, createPlanDefaultValues);
            Assert.That(planCreateResponse.Status, Is.EqualTo("Initialized"));
            Assert.That(await _doTheChallenge.DoTheChallengeV4Pf4Async(planCreateResponse, createPlanDefaultValues, 12, null!, null,  _requestHeader!), Is.True);
            Console.WriteLine("CreatePlanWithTerminalStripeDirectWithout3Ds is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in CreatePlanWithTerminalStripeDirectWithout3Ds \n" + exception + "\n");
        }
    }
    
    [TestCase(Category = "GatewaysCreatePlanPaymentForm4Without3DsTests")]
    [Test(Description = "CreatePlanWithTerminalReachWithout3Ds")]
    public async Task CreatePlanWithTerminalReachWithout3Ds()
    {
        try
        {
            Console.WriteLine("\nStarting CreatePlanWithTerminalReachWithout3Ds");
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            var planCreateResponse = await _installmentPlans.CreatePlanInitiateAsync(Environment.GetEnvironmentVariable("ApiV3")!,  _requestHeader!, "Active", 
                new Random().Next(2, 6), _envConfig.ReachTerminal, createPlanDefaultValues);
            Assert.That(planCreateResponse.Status, Is.EqualTo("Initialized"));
            Assert.That(await _doTheChallenge.DoTheChallengeV4Pf4Async(planCreateResponse, createPlanDefaultValues, 12, null!, null,  _requestHeader!), Is.True);
            Console.WriteLine("CreatePlanWithTerminalReachWithout3Ds is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in CreatePlanWithTerminalReachWithout3Ds \n" + exception + "\n");
        }
    }
    
    [TestCase(Category = "GatewaysCreatePlanPaymentForm4Without3DsTests")]
    [Test(Description = "CreatePlanWithTerminalPaySafeWithout3Ds")]
    public async Task CreatePlanWithTerminalPaySafeWithout3Ds()
    {
        try
        {
            Console.WriteLine("\nStarting CreatePlanWithTerminalPaySafeWithout3Ds");
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            var planCreateResponse = await _installmentPlans.CreatePlanInitiateAsync(Environment.GetEnvironmentVariable("ApiV3")!,  _requestHeader!, "Active", 
                new Random().Next(2, 6), _envConfig.PaySafeTerminal, createPlanDefaultValues);
            Assert.That(planCreateResponse.Status, Is.EqualTo("Initialized"));
            Assert.That(await _doTheChallenge.DoTheChallengeV4Pf4Async(planCreateResponse, createPlanDefaultValues, 12, null!, null,  _requestHeader!), Is.True);
            Console.WriteLine("CreatePlanWithTerminalPaySafeWithout3Ds is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in CreatePlanWithTerminalPaySafeWithout3Ds \n" + exception + "\n");
        }
    }
    
    [TestCase(Category = "GatewaysCreatePlanPaymentForm4Without3DsTests")]
    [Test(Description = "CreatePlanWithTerminalAdyenWithout3Ds")]
    public async Task CreatePlanWithTerminalAdyenWithout3Ds()
    {
        try
        {
            Console.WriteLine("\nStarting CreatePlanWithTerminalAdyenWithout3Ds");
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            var planCreateResponse = await _installmentPlans.CreatePlanInitiateAsync(Environment.GetEnvironmentVariable("ApiV3")!,  _requestHeader!, "Active", 
                new Random().Next(2, 6), _envConfig.AdyenTerminal, createPlanDefaultValues);
            Assert.That(planCreateResponse.Status, Is.EqualTo("Initialized"));
            Assert.That(await _doTheChallenge.DoTheChallengeV4Pf4Async(planCreateResponse, createPlanDefaultValues, 12, null!, null,  _requestHeader!), Is.True);
            Console.WriteLine("CreatePlanWithTerminalAdyenWithout3Ds is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in CreatePlanWithTerminalAdyenWithout3Ds \n" + exception + "\n");
        }
    }
}