using Allure.NUnit;
using Allure.NUnit.Attributes;
using NUnit.Framework;
using Splitit.Automation.NG.Backend.BaseActions;
using Splitit.Automation.NG.Backend.Services.V2.InstallmentPlan.InstallmentPlanApiEndPoints;
using Splitit.Automation.NG.Backend.Services.V2.InstallmentPlan.InstallmentPlanRequests;
using Splitit.Automation.NG.Backend.Services.V3.DefaultValues;
using Splitit.Automation.NG.Backend.Services.V3.InstallmentPlan.InstallmentPlanApiEndPoint;
using Splitit.Automation.NG.Backend.Tests.TestsSetups;
using Splitit.Automation.NG.SharedResources.BaseActions;
using Splitit.Automation.NG.Utilities.EnvironmentsConfig;

namespace Splitit.Automation.NG.SharedResources.Tests.GatewaysFunctionality;

[TestFixture]
[AllureNUnit]
[AllureSuite("GatewaysCreatePlanPaymentForm4Without3DsAndUpdateCardTests")]
[AllureDisplayIgnored]
[Parallelizable(ParallelScope.All)]
public class GatewaysCreatePlanPaymentForm4Without3DsAndUpdateCardTests
{
    private readonly InstallmentPlans _installmentPlans;
    private RequestHeader? _requestHeader;
    private readonly DoTheChallenge _doTheChallenge;
    private readonly DoUpdateCard _doUpdateCard;
    private readonly InitiateUpdatePaymentData _initiateUpdatePaymentData;
    private readonly InitiateUpdatePaymentDataRequest.Root _initiateUpdatePaymentDataRequest;
    private readonly EnvConfig _envConfig;


    public GatewaysCreatePlanPaymentForm4Without3DsAndUpdateCardTests()
    {
        Console.WriteLine("Starting Setup");
        _installmentPlans = new InstallmentPlans();
        _doTheChallenge = new DoTheChallenge();
        _doUpdateCard = new DoUpdateCard();
        _initiateUpdatePaymentData = new InitiateUpdatePaymentData();
        _initiateUpdatePaymentDataRequest = new InitiateUpdatePaymentDataRequest.Root();
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
    
    [TestCase(Category = "GatewaysCreatePlanPaymentFormWithout3DsAndUpdateCard")]
    [Test(Description = "CreatePlanWithTerminalCyberSourceWithout3DsAndUpdateCard")]
    public async Task CreatePlanWithTerminalCyberSourceWithout3DsAndUpdateCard()
    {
        try
        {
            Console.WriteLine("\nStarting CreatePlanWithTerminalCyberSourceWithout3DsAndUpdateCard");
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            createPlanDefaultValues.paymentMethod.card.cardNumber = "5555555555554444";
            createPlanDefaultValues.planData.currency = "GBP";
            var planCreateResponse = await _installmentPlans.CreatePlanInitiateAsync(Environment.GetEnvironmentVariable("ApiV3")!,  _requestHeader!, "Active", 
                new Random().Next(2, 6), _envConfig.CyberSourceTerminal, createPlanDefaultValues);
            Assert.That(planCreateResponse.Status, Is.EqualTo("Initialized"));
            Assert.That(await _doTheChallenge.DoTheChallengeV4Pf4Async(planCreateResponse, createPlanDefaultValues, 12, null!, null,  _requestHeader!), Is.True);
            _initiateUpdatePaymentDataRequest.InstallmentPlanNumber = planCreateResponse.InstallmentPlanNumber;
            await Task.Delay(30*1000);
            var jResponse = await _initiateUpdatePaymentData.SendInitiateUpdatePaymentDataRequestAsync( _requestHeader!, _initiateUpdatePaymentDataRequest);
            await _doUpdateCard.DoUpdateCreditCardAsync(jResponse.UpdateURL, Environment.GetEnvironmentVariable("Card5454")!, 
                createPlanDefaultValues.paymentMethod.card.cardHolderFullName, createPlanDefaultValues.paymentMethod.card.cardExpMonth, 
                createPlanDefaultValues.paymentMethod.card.cardExpYear, createPlanDefaultValues.paymentMethod.card.cardCvv, "CyberSource", createPlanDefaultValues,
                 _requestHeader!, planCreateResponse.InstallmentPlanNumber);
            Console.WriteLine("CreatePlanWithTerminalCyberSourceWithout3DsAndUpdateCard is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in CreatePlanWithTerminalCyberSourceWithout3DsAndUpdateCard \n" + exception + "\n");
        }
    }
    
    [TestCase(Category = "GatewaysCreatePlanPaymentFormWithout3DsAndUpdateCard")]
    [Test(Description = "CreatePlanWithTerminalCheckoutWithout3DsAndUpdateCard")]
    public async Task CreatePlanWithTerminalCheckoutWithout3DsAndUpdateCard()
    {
        try
        {
            Console.WriteLine("\nStarting CreatePlanWithTerminalCheckoutWithout3DsAndUpdateCard");
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            var planCreateResponse = await _installmentPlans.CreatePlanInitiateAsync(Environment.GetEnvironmentVariable("ApiV3")!,  _requestHeader!, "Active", 
                new Random().Next(2, 6), _envConfig.CheckoutTerminal, createPlanDefaultValues);
            Assert.That(planCreateResponse.Status, Is.EqualTo("Initialized"));
            Assert.That(await _doTheChallenge.DoTheChallengeV4Pf4Async(planCreateResponse, createPlanDefaultValues, 12, null!, null,  _requestHeader!), Is.True);
            _initiateUpdatePaymentDataRequest.InstallmentPlanNumber = planCreateResponse.InstallmentPlanNumber;
            await Task.Delay(30*1000);
            var jResponse = await _initiateUpdatePaymentData.SendInitiateUpdatePaymentDataRequestAsync( _requestHeader!, _initiateUpdatePaymentDataRequest);
            await _doUpdateCard.DoUpdateCreditCardAsync(jResponse.UpdateURL, Environment.GetEnvironmentVariable("Card5454")!, 
                createPlanDefaultValues.paymentMethod.card.cardHolderFullName, createPlanDefaultValues.paymentMethod.card.cardExpMonth, 
                createPlanDefaultValues.paymentMethod.card.cardExpYear, createPlanDefaultValues.paymentMethod.card.cardCvv, "Checkout", createPlanDefaultValues,
                 _requestHeader!, planCreateResponse.InstallmentPlanNumber);
            Console.WriteLine("CreatePlanWithTerminalCheckoutWithout3DsAndUpdateCard is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in CreatePlanWithTerminalCheckoutWithout3DsAndUpdateCard \n" + exception + "\n");
        }
    }
    
    [TestCase(Category = "GatewaysCreatePlanPaymentFormWithout3DsAndUpdateCard")]
    [Test(Description = "CreatePlanWithTerminalBlueSnapMorWithout3DsAndUpdateCard")]
    public async Task CreatePlanWithTerminalBlueSnapMorWithout3DsAndUpdateCard()
    {
        try
        {
            Console.WriteLine("\nStarting CreatePlanWithTerminalBlueSnapMorWithout3DsAndUpdateCard");
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            var planCreateResponse = await _installmentPlans.CreatePlanInitiateAsync(Environment.GetEnvironmentVariable("ApiV3")!,  _requestHeader!, "Active", 
                new Random().Next(2, 6), _envConfig.BlueSnapMORTerminal, createPlanDefaultValues);
            Assert.That(planCreateResponse.Status, Is.EqualTo("Initialized"));
            Assert.That(await _doTheChallenge.DoTheChallengeV4Pf4Async(planCreateResponse, createPlanDefaultValues, 12, null!, null,  _requestHeader!), Is.True);
            _initiateUpdatePaymentDataRequest.InstallmentPlanNumber = planCreateResponse.InstallmentPlanNumber;
            await Task.Delay(30*1000);
            var jResponse = await _initiateUpdatePaymentData.SendInitiateUpdatePaymentDataRequestAsync( _requestHeader!, _initiateUpdatePaymentDataRequest);
            await _doUpdateCard.DoUpdateCreditCardAsync(jResponse.UpdateURL, Environment.GetEnvironmentVariable("Card5454")!, 
                createPlanDefaultValues.paymentMethod.card.cardHolderFullName, createPlanDefaultValues.paymentMethod.card.cardExpMonth, 
                createPlanDefaultValues.paymentMethod.card.cardExpYear, createPlanDefaultValues.paymentMethod.card.cardCvv, "BlueSnapMor", createPlanDefaultValues,
                 _requestHeader!, planCreateResponse.InstallmentPlanNumber);
            Console.WriteLine("CreatePlanWithTerminalBlueSnapMorWithout3DsAndUpdateCard is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in CreatePlanWithTerminalBlueSnapMorWithout3DsAndUpdateCard \n" + exception + "\n");
        }
    }
    
    [TestCase(Category = "GatewaysCreatePlanPaymentFormWithout3DsAndUpdateCard")]
    [Test(Description = "CreatePlanWithTerminalBlueSnapDirectWithout3DsAndUpdateCard")]
    public async Task CreatePlanWithTerminalBlueSnapDirectWithout3DsAndUpdateCard()
    {
        try
        {
            Console.WriteLine("\nStarting CreatePlanWithTerminalBlueSnapDirectWithout3DsAndUpdateCard");
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            var planCreateResponse = await _installmentPlans.CreatePlanInitiateAsync(Environment.GetEnvironmentVariable("ApiV3")!,  _requestHeader!, "Active", 
                new Random().Next(2, 6), _envConfig.BlueSnapDirectTerminal, createPlanDefaultValues);
            Assert.That(planCreateResponse.Status, Is.EqualTo("Initialized"));
            Assert.That(await _doTheChallenge.DoTheChallengeV4Pf4Async(planCreateResponse, createPlanDefaultValues, 12, null!, null,  _requestHeader!), Is.True);
            _initiateUpdatePaymentDataRequest.InstallmentPlanNumber = planCreateResponse.InstallmentPlanNumber;
            await Task.Delay(30*1000);
            var jResponse = await _initiateUpdatePaymentData.SendInitiateUpdatePaymentDataRequestAsync( _requestHeader!, _initiateUpdatePaymentDataRequest);
            await _doUpdateCard.DoUpdateCreditCardAsync(jResponse.UpdateURL, _envConfig.Card4111, 
                createPlanDefaultValues.paymentMethod.card.cardHolderFullName, createPlanDefaultValues.paymentMethod.card.cardExpMonth, 
                createPlanDefaultValues.paymentMethod.card.cardExpYear, createPlanDefaultValues.paymentMethod.card.cardCvv, "BlueSnapDirect", createPlanDefaultValues,
                 _requestHeader!, planCreateResponse.InstallmentPlanNumber);
            Console.WriteLine("CreatePlanWithTerminalBlueSnapDirectWithout3DsAndUpdateCard is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in CreatePlanWithTerminalBlueSnapDirectWithout3DsAndUpdateCard \n" + exception + "\n");
        }
    }
    
    // [TestCase(Category = "GatewaysCreatePlanPaymentFormWithout3DsAndUpdateCard")]
    // [Test(Description = "CreatePlanWithTerminalBamboraWithout3DsAndUpdateCard")]
    // public async Task CreatePlanWithTerminalBamboraWithout3DsAndUpdateCard()
    // {
    //     try
    //     {
    //         Console.WriteLine("\nStarting CreatePlanWithTerminalBamboraWithout3DsAndUpdateCard");
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
    //         _initiateUpdatePaymentDataRequest.InstallmentPlanNumber = planCreateResponse.InstallmentPlanNumber;
    //         await Task.Delay(30*1000);
    //         var jResponseUpdate = await _initiateUpdatePaymentData.SendInitiateUpdatePaymentDataRequestAsync( _requestHeader!, _initiateUpdatePaymentDataRequest);
    //         await _doUpdateCard.DoUpdateCreditCardAsync(jResponseUpdate.UpdateURL, _envConfig.Card5100, 
    //             createPlanDefaultValues.paymentMethod.card.cardHolderFullName, createPlanDefaultValues.paymentMethod.card.cardExpMonth, 
    //             createPlanDefaultValues.paymentMethod.card.cardExpYear, createPlanDefaultValues.paymentMethod.card.cardCvv, "Bambora", createPlanDefaultValues,
    //              _requestHeader!, planCreateResponse.InstallmentPlanNumber);
    //         var jResponse1 = await _settings.SendGetSettingsRequestAsync( _requestHeader!, int.Parse(_envConfig.NgMockV2BU));
    //         jResponse1!.MerchantSettings.DefaultPlanStrategy = "SecuredPlan";
    //         var jResponseUpdated1 = await _settingsSave.SendUpdateSettingsRequestAsync( _requestHeader!, jResponse1);
    //         Assert.That(jResponseUpdated1.MerchantSettings.DefaultPlanStrategy, Is.EqualTo("SecuredPlan"));
    //         Console.WriteLine("CreatePlanWithTerminalBamboraWithout3DsAndUpdateCard is Done\n");
    //     }
    //     catch (Exception exception)
    //     { 
    //         var jResponse2 = await _settings.SendGetSettingsRequestAsync( _requestHeader!, int.Parse(_envConfig.NgMockV2BU)); 
    //         jResponse2!.MerchantSettings.DefaultPlanStrategy = "SecuredPlan"; 
    //         var jResponseUpdated2 = await _settingsSave.SendUpdateSettingsRequestAsync( _requestHeader!, jResponse2);
    //         Assert.That(jResponseUpdated2.MerchantSettings.DefaultPlanStrategy, Is.EqualTo("SecuredPlan"));
    //         Assert.Fail("Error in CreatePlanWithTerminalBamboraWithout3DsAndUpdateCard \n" + exception + "\n");
    //     }
    // }
    
    [TestCase(Category = "GatewaysCreatePlanPaymentFormWithout3DsAndUpdateCard")]
    [Test(Description = "CreatePlanWithTerminalAuthNetV2Without3DsAndUpdateCard")]
    public async Task CreatePlanWithTerminalAuthNetV2Without3DsAndUpdateCard()
    {
        try
        {
            Console.WriteLine("\nStarting CreatePlanWithTerminalAuthNetV2Without3DsAndUpdateCard");
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            var planCreateResponse = await _installmentPlans.CreatePlanInitiateAsync(Environment.GetEnvironmentVariable("ApiV3")!,  _requestHeader!, "Active", 
                3, _envConfig.AuthorizeNetV2, createPlanDefaultValues);
            Assert.That(planCreateResponse.Status, Is.EqualTo("Initialized"));
            Assert.That(await _doTheChallenge.DoTheChallengeV4Pf4Async(planCreateResponse, createPlanDefaultValues, 12, null!, "yes",  _requestHeader!), Is.True);
            _initiateUpdatePaymentDataRequest.InstallmentPlanNumber = planCreateResponse.InstallmentPlanNumber;
            await Task.Delay(30*1000);
            var jResponse = await _initiateUpdatePaymentData.SendInitiateUpdatePaymentDataRequestAsync( _requestHeader!, _initiateUpdatePaymentDataRequest);
            await _doUpdateCard.DoUpdateCreditCardAsync(jResponse.UpdateURL, Environment.GetEnvironmentVariable("Card5454")!, 
                createPlanDefaultValues.paymentMethod.card.cardHolderFullName, createPlanDefaultValues.paymentMethod.card.cardExpMonth, 
                createPlanDefaultValues.paymentMethod.card.cardExpYear, createPlanDefaultValues.paymentMethod.card.cardCvv, "AuthNetV2", createPlanDefaultValues,
                 _requestHeader!, planCreateResponse.InstallmentPlanNumber);
            Console.WriteLine("CreatePlanWithTerminalAuthNetV2Without3DsAndUpdateCard is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in CreatePlanWithTerminalAuthNetV2Without3DsAndUpdateCard \n" + exception + "\n");
        }
    }
    
    [TestCase(Category = "GatewaysCreatePlanPaymentFormWithout3DsAndUpdateCard")]
    [Test(Description = "CreatePlanWithTerminalWorldPayWithout3DsAndUpdateCard")]
    public async Task CreatePlanWithTerminalWorldPayWithout3DsAndUpdateCard()
    {
        try
        {
            Console.WriteLine("\nStarting CreatePlanWithTerminalWorldPayWithout3DsAndUpdateCard");
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            createPlanDefaultValues.paymentMethod.card.cardNumber = _envConfig.Card5555;
            createPlanDefaultValues.planData.totalAmount = AmountGenerator.GenerateAmountWithMinMaxValues(100, 4000);
            var planCreateResponse = await _installmentPlans.CreatePlanInitiateAsync(Environment.GetEnvironmentVariable("ApiV3")!,  _requestHeader!, "Active", 
                new Random().Next(3, 6), _envConfig.WorldPayTerminal, createPlanDefaultValues);
            Assert.That(planCreateResponse.Status, Is.EqualTo("Initialized"));
            Assert.That(await _doTheChallenge.DoTheChallengeV4Pf4Async(planCreateResponse, createPlanDefaultValues, 12, null!,
                null,  _requestHeader!), Is.True);
            _initiateUpdatePaymentDataRequest.InstallmentPlanNumber = planCreateResponse.InstallmentPlanNumber;
            await Task.Delay(30*1000);
            var jResponse = await _initiateUpdatePaymentData.SendInitiateUpdatePaymentDataRequestAsync( _requestHeader!, _initiateUpdatePaymentDataRequest);
            await _doUpdateCard.DoUpdateCreditCardAsync(jResponse.UpdateURL, _envConfig.Card4444, 
                createPlanDefaultValues.paymentMethod.card.cardHolderFullName, createPlanDefaultValues.paymentMethod.card.cardExpMonth, 
                createPlanDefaultValues.paymentMethod.card.cardExpYear, createPlanDefaultValues.paymentMethod.card.cardCvv,
                "Bambora", createPlanDefaultValues, _requestHeader!, planCreateResponse.InstallmentPlanNumber);
            Console.WriteLine("CreatePlanWithTerminalWorldPayWithout3DsAndUpdateCard is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in CreatePlanWithTerminalWorldPayWithout3DsAndUpdateCard \n" + exception + "\n");
        }
    }
    
    [TestCase(Category = "GatewaysCreatePlanPaymentFormWithout3DsAndUpdateCard")]
    [Test(Description = "CreatePlanWithTerminalStripeDirectWithout3DsAndUpdateCard")]
    public async Task CreatePlanWithTerminalStripeDirectWithout3DsAndUpdateCard()
    {
        try
        {
            Console.WriteLine("\nStarting CreatePlanWithTerminalStripeDirectWithout3DsAndUpdateCard");
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            var planCreateResponse = await _installmentPlans.CreatePlanInitiateAsync(Environment.GetEnvironmentVariable("ApiV3")!,  _requestHeader!, "Active", 
                new Random().Next(2, 6), _envConfig.StripeDirectTerminal, createPlanDefaultValues);
            Assert.That(planCreateResponse.Status, Is.EqualTo("Initialized"));
            Assert.That(await _doTheChallenge.DoTheChallengeV4Pf4Async(planCreateResponse, createPlanDefaultValues, 12, null!, null,  _requestHeader!), Is.True);
            _initiateUpdatePaymentDataRequest.InstallmentPlanNumber = planCreateResponse.InstallmentPlanNumber;
            await Task.Delay(30*1000);
            var jResponse = await _initiateUpdatePaymentData.SendInitiateUpdatePaymentDataRequestAsync( _requestHeader!, _initiateUpdatePaymentDataRequest);
            await _doUpdateCard.DoUpdateCreditCardAsync(jResponse.UpdateURL, Environment.GetEnvironmentVariable("Card5454")!, 
                createPlanDefaultValues.paymentMethod.card.cardHolderFullName, createPlanDefaultValues.paymentMethod.card.cardExpMonth, 
                createPlanDefaultValues.paymentMethod.card.cardExpYear, createPlanDefaultValues.paymentMethod.card.cardCvv, "StripeDirect", createPlanDefaultValues,
                 _requestHeader!, planCreateResponse.InstallmentPlanNumber);
            Console.WriteLine("CreatePlanWithTerminalStripeDirectWithout3DsAndUpdateCard is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in CreatePlanWithTerminalStripeDirectWithout3DsAndUpdateCard \n" + exception + "\n");
        }
    }
    
    [TestCase(Category = "GatewaysCreatePlanPaymentFormWithout3DsAndUpdateCard")]
    [Test(Description = "CreatePlanWithTerminalReachWithout3DsAndUpdateCard")]
    public async Task CreatePlanWithTerminalReachWithout3DsAndUpdateCard()
    {
        try
        {
            Console.WriteLine("\nStarting CreatePlanWithTerminalReachWithout3DsAndUpdateCard");
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            var planCreateResponse = await _installmentPlans.CreatePlanInitiateAsync(Environment.GetEnvironmentVariable("ApiV3")!,  _requestHeader!, "Active", 
                new Random().Next(2, 6), _envConfig.ReachTerminal, createPlanDefaultValues);
            Assert.That(planCreateResponse.Status, Is.EqualTo("Initialized"));
            Assert.That(await _doTheChallenge.DoTheChallengeV4Pf4Async(planCreateResponse, createPlanDefaultValues, 12, null!, null,  _requestHeader!), Is.True);
            _initiateUpdatePaymentDataRequest.InstallmentPlanNumber = planCreateResponse.InstallmentPlanNumber;
            await Task.Delay(30*1000);
            var jResponse = await _initiateUpdatePaymentData.SendInitiateUpdatePaymentDataRequestAsync( _requestHeader!, _initiateUpdatePaymentDataRequest);
            await _doUpdateCard.DoUpdateCreditCardAsync(jResponse.UpdateURL, Environment.GetEnvironmentVariable("Card5454")!, 
                createPlanDefaultValues.paymentMethod.card.cardHolderFullName, createPlanDefaultValues.paymentMethod.card.cardExpMonth, 
                createPlanDefaultValues.paymentMethod.card.cardExpYear, createPlanDefaultValues.paymentMethod.card.cardCvv, "Reach", createPlanDefaultValues,
                 _requestHeader!, planCreateResponse.InstallmentPlanNumber);
            Console.WriteLine("CreatePlanWithTerminalReachWithout3DsAndUpdateCard is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in CreatePlanWithTerminalReachWithout3DsAndUpdateCard \n" + exception + "\n");
        }
    }
    
    [TestCase(Category = "GatewaysCreatePlanPaymentFormWithout3DsAndUpdateCard")]
    [Test(Description = "CreatePlanWithTerminalPaySafeWithout3DsAndUpdateCard")]
    public async Task CreatePlanWithTerminalPaySafeWithout3DsAndUpdateCard()
    {
        try
        {
            Console.WriteLine("\nStarting CreatePlanWithTerminalPaySafeWithout3DsAndUpdateCard");
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            var planCreateResponse = await _installmentPlans.CreatePlanInitiateAsync(Environment.GetEnvironmentVariable("ApiV3")!,  _requestHeader!, "Active", 
                new Random().Next(2, 6), _envConfig.PaySafeTerminal, createPlanDefaultValues);
            Assert.That(planCreateResponse.Status, Is.EqualTo("Initialized"));
            Assert.That(await _doTheChallenge.DoTheChallengeV4Pf4Async(planCreateResponse, createPlanDefaultValues, 12, null!, null,  _requestHeader!), Is.True);
            _initiateUpdatePaymentDataRequest.InstallmentPlanNumber = planCreateResponse.InstallmentPlanNumber;
            await Task.Delay(30*1000);
            var jResponse = await _initiateUpdatePaymentData.SendInitiateUpdatePaymentDataRequestAsync( _requestHeader!, _initiateUpdatePaymentDataRequest);
            await _doUpdateCard.DoUpdateCreditCardAsync(jResponse.UpdateURL, Environment.GetEnvironmentVariable("Card5454")!, 
                createPlanDefaultValues.paymentMethod.card.cardHolderFullName, createPlanDefaultValues.paymentMethod.card.cardExpMonth, 
                createPlanDefaultValues.paymentMethod.card.cardExpYear, createPlanDefaultValues.paymentMethod.card.cardCvv, "PaySafe", createPlanDefaultValues,
                 _requestHeader!, planCreateResponse.InstallmentPlanNumber);
            Console.WriteLine("CreatePlanWithTerminalPaySafeWithout3DsAndUpdateCard is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in CreatePlanWithTerminalPaySafeWithout3DsAndUpdateCard \n" + exception + "\n");
        }
    }
    
    [TestCase(Category = "GatewaysCreatePlanPaymentFormWithout3DsAndUpdateCard")]
    [Test(Description = "CreatePlanWithTerminalAdyenWithout3DsAndUpdateCard")]
    public async Task CreatePlanWithTerminalAdyenWithout3DsAndUpdateCard()
    {
        try
        {
            Console.WriteLine("\nStarting CreatePlanWithTerminalAdyenWithout3DsAndUpdateCard");
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            var planCreateResponse = await _installmentPlans.CreatePlanInitiateAsync(Environment.GetEnvironmentVariable("ApiV3")!,  _requestHeader!, "Active", 
                new Random().Next(4, 12), _envConfig.AdyenTerminal, createPlanDefaultValues);
            Assert.That(planCreateResponse.Status, Is.EqualTo("Initialized"));
            Assert.That(await _doTheChallenge.DoTheChallengeV4Pf4Async(planCreateResponse, createPlanDefaultValues, 12, 
                null!, null,  _requestHeader!), Is.True);
            _initiateUpdatePaymentDataRequest.InstallmentPlanNumber = planCreateResponse.InstallmentPlanNumber;
            await Task.Delay(30*1000);
            var jResponse = await _initiateUpdatePaymentData.SendInitiateUpdatePaymentDataRequestAsync( _requestHeader!, _initiateUpdatePaymentDataRequest);
            await _doUpdateCard.DoUpdateCreditCardAsync(jResponse.UpdateURL, Environment.GetEnvironmentVariable("Card5454")!, 
                createPlanDefaultValues.paymentMethod.card.cardHolderFullName, createPlanDefaultValues.paymentMethod.card.cardExpMonth, 
                createPlanDefaultValues.paymentMethod.card.cardExpYear, createPlanDefaultValues.paymentMethod.card.cardCvv, "Adyen", createPlanDefaultValues,
                 _requestHeader!, planCreateResponse.InstallmentPlanNumber);
            Console.WriteLine("CreatePlanWithTerminalAdyenWithout3DsAndUpdateCard is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in CreatePlanWithTerminalAdyenWithout3DsAndUpdateCard \n" + exception + "\n");
        }
    }
}