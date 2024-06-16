using Allure.NUnit;
using Allure.NUnit.Attributes;
using NUnit.Framework;
using Splitit.Automation.NG.Backend.BaseActions;
using Splitit.Automation.NG.Backend.Services.AdminApi.Merchant.MerchantFunctionality;
using Splitit.Automation.NG.Backend.Services.V1.DefaultValues;
using Splitit.Automation.NG.Backend.Services.V2.InstallmentPlan.InstallmentPlanApiEndPoints;
using Splitit.Automation.NG.Backend.Services.V2.InstallmentPlan.InstallmentPlanRequests;
using Splitit.Automation.NG.Backend.Services.V3.DefaultValues;
using Splitit.Automation.NG.Backend.Services.V3.InstallmentPlan.InstallmentPlanApiEndPoint;
using Splitit.Automation.NG.Backend.Tests.TestsSetups;
using Splitit.Automation.NG.SharedResources.BaseActions;
using Splitit.Automation.NG.Utilities.EnvironmentsConfig;
using Create = Splitit.Automation.NG.Backend.Services.V1.InstallmentPlan.InstallmentPlanApiEndPoints.Create;

namespace Splitit.Automation.NG.SharedResources.Tests.GatewaysFunctionality;

[TestFixture]
[AllureNUnit]
[AllureSuite("GatewaysCreatePlanPaymentForm4With3DsAndUpdateCardTests")]
[AllureDisplayIgnored]
[Parallelizable(ParallelScope.All)]
public class GatewaysCreatePlanPaymentForm4With3DsAndUpdateCardTests
{
    private readonly InstallmentPlans _installmentPlans;
    private RequestHeader? _requestHeader;
    private readonly DoTheChallenge _doTheChallenge;
    private readonly Settings _settings;
    private readonly SettingsSave _settingsSave;
    private readonly DoUpdateCard _doUpdateCard;
    private readonly InitiateUpdatePaymentData _initiateUpdatePaymentData;
    private readonly InitiateUpdatePaymentDataRequest.Root _initiateUpdatePaymentDataRequest;
    private readonly EnvConfig _envConfig;
    private readonly Create _create;

    public GatewaysCreatePlanPaymentForm4With3DsAndUpdateCardTests()
    {
        Console.WriteLine("Starting Setup");
        _installmentPlans = new InstallmentPlans();
        _doTheChallenge = new DoTheChallenge();
        _settings = new Settings();
        _settingsSave = new SettingsSave();
        _doUpdateCard = new DoUpdateCard();
        _initiateUpdatePaymentData = new InitiateUpdatePaymentData();
        _initiateUpdatePaymentDataRequest = new InitiateUpdatePaymentDataRequest.Root();
        _envConfig = new EnvConfig();
        _create = new Create();
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
    
    [TestCase(Category = "GatewaysCreatePlanPaymentForm4With3DsAndUpdateCardTests")]
    [Test(Description = "CreatePlanWithTerminalCheckoutWith3DsAndUpdateCard")]
    public async Task CreatePlanWithTerminalCheckoutWith3DsAndUpdateCard()
    {
        try
        {
            Console.WriteLine("\nStarting CreatePlanWithTerminalCheckoutWith3DsAndUpdateCard");
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no_exemption_with_params");
            var planCreateResponse = await _installmentPlans.CreatePlanInitiateAsync(Environment.GetEnvironmentVariable("ApiV3")!,  _requestHeader!, "Active", 
                new Random().Next(2, 6), _envConfig.CheckoutTerminal, createPlanDefaultValues);
            Assert.That(planCreateResponse.Status, Is.EqualTo("Initialized"));
            Assert.That(await _doTheChallenge.DoTheChallengeV4Pf4Async(planCreateResponse, createPlanDefaultValues, 12, "Checkout", null,  _requestHeader!), Is.True);
            _initiateUpdatePaymentDataRequest.InstallmentPlanNumber = planCreateResponse.InstallmentPlanNumber;
            var jResponse = await _initiateUpdatePaymentData.SendInitiateUpdatePaymentDataRequestAsync( _requestHeader!, _initiateUpdatePaymentDataRequest);
            await _doUpdateCard.DoUpdateCreditCardAsync(jResponse.UpdateURL, createPlanDefaultValues.paymentMethod.card.cardNumber, 
                createPlanDefaultValues.paymentMethod.card.cardHolderFullName, createPlanDefaultValues.paymentMethod.card.cardExpMonth, 
                createPlanDefaultValues.paymentMethod.card.cardExpYear, "123", "Checkout", createPlanDefaultValues,
                 _requestHeader!, planCreateResponse.InstallmentPlanNumber);
            Console.WriteLine("CreatePlanWithTerminalCheckoutWith3DsAndUpdateCard is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in CreatePlanWithTerminalCheckoutWith3DsAndUpdateCard \n" + exception + "\n");
        }
    }
    
    [TestCase(Category = "GatewaysCreatePlanPaymentForm4With3DsAndUpdateCardTests")]
    [Test(Description = "CreatePlanWithTerminalBlueSnapDirectWith3DsAndUpdateCard")]
    public async Task CreatePlanWithTerminalBlueSnapDirectWith3DsAndUpdateCard()
    {
        try
        {
            Console.WriteLine("\nStarting CreatePlanWithTerminalBlueSnapDirectWith3DsAndUpdateCard");
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
            _initiateUpdatePaymentDataRequest.InstallmentPlanNumber = planCreateResponse.InstallmentPlanNumber;
            var jResponseUpdate = await _initiateUpdatePaymentData.SendInitiateUpdatePaymentDataRequestAsync( _requestHeader!, _initiateUpdatePaymentDataRequest);
            await _doUpdateCard.DoUpdateCreditCardAsync(jResponseUpdate.UpdateURL, _envConfig.Card4111, 
                createPlanDefaultValues.paymentMethod.card.cardHolderFullName, createPlanDefaultValues.paymentMethod.card.cardExpMonth, 
                createPlanDefaultValues.paymentMethod.card.cardExpYear, createPlanDefaultValues.paymentMethod.card.cardCvv, "BlueSnapDirect", createPlanDefaultValues,
                 _requestHeader!, planCreateResponse.InstallmentPlanNumber);
            var jResponse2 = await _settings.SendGetSettingsRequestAsync( _requestHeader!, int.Parse(_envConfig.NgMockV2BU)); 
            jResponse2!.MerchantSettings.DefaultPlanStrategy = "SecuredPlan"; 
            var jResponseUpdated2 = await _settingsSave.SendUpdateSettingsRequestAsync( _requestHeader!, jResponse2);
            Assert.That(jResponseUpdated2.MerchantSettings.DefaultPlanStrategy, Is.EqualTo("SecuredPlan"));
            Console.WriteLine("CreatePlanWithTerminalBlueSnapDirectWith3DsAndUpdateCard is Done\n");
        }
        catch (Exception exception)
        {
            var jResponse2 = await _settings.SendGetSettingsRequestAsync( _requestHeader!, int.Parse(_envConfig.NgMockV2BU)); 
            jResponse2!.MerchantSettings.DefaultPlanStrategy = "SecuredPlan"; 
            var jResponseUpdated2 = await _settingsSave.SendUpdateSettingsRequestAsync( _requestHeader!, jResponse2);
            Assert.That(jResponseUpdated2.MerchantSettings.DefaultPlanStrategy, Is.EqualTo("SecuredPlan"));
            Assert.Fail("Error in CreatePlanWithTerminalBlueSnapDirectWith3DsAndUpdateCard \n" + exception + "\n");
        }
    }
    
    [TestCase(Category = "GatewaysCreatePlanPaymentForm4With3DsAndUpdateCardTests")]
    [Test(Description = "CreatePlanWithTerminalWorldPayWith3DsAndUpdateCard")]
    public async Task CreatePlanWithTerminalWorldPayWith3DsAndUpdateCard()
    {
        try
        {
            Console.WriteLine("\nStarting CreatePlanWithTerminalWorldPayWith3DsAndUpdateCard");
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            var jResponse = await _settings.SendGetSettingsRequestAsync( _requestHeader!, int.Parse(_envConfig.NgMockV2BU));
            jResponse!.MerchantSettings.DefaultPlanStrategy = "SecuredPlan3";
            var jResponseUpdated = await _settingsSave.SendUpdateSettingsRequestAsync( _requestHeader!, jResponse);
            Assert.That(jResponseUpdated.MerchantSettings.DefaultPlanStrategy, Is.EqualTo("SecuredPlan3"));
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "yes");
            createPlanDefaultValues.shopper.fullName = "3DS_V2_CHALLENGE_IDENTIFIED";
            createPlanDefaultValues.paymentMethod.card.cardNumber = _envConfig.Card4111;
            createPlanDefaultValues.planData.totalAmount = AmountGenerator.GenerateAmountWithMinMaxValues(100, 4000);
            var planCreateResponse = await _installmentPlans.CreatePlanInitiateAsync(Environment.GetEnvironmentVariable("ApiV3")!,  _requestHeader!, "Active", 
                new Random().Next(3, 6), _envConfig.WorldPayTerminal, createPlanDefaultValues);
            Assert.That(planCreateResponse.Status, Is.EqualTo("Initialized"));
            await _doTheChallenge.DoTheChallengeV4Pf4Async(planCreateResponse, createPlanDefaultValues, 12,
                "WorldPay", null,  _requestHeader!);
            _initiateUpdatePaymentDataRequest.InstallmentPlanNumber = planCreateResponse.InstallmentPlanNumber;
            var jResponseUpdate = await _initiateUpdatePaymentData.SendInitiateUpdatePaymentDataRequestAsync( _requestHeader!, _initiateUpdatePaymentDataRequest);
            await _doUpdateCard.DoUpdateCreditCardAsync(jResponseUpdate.UpdateURL, Environment.GetEnvironmentVariable("Card5454")!, 
                "3DS_V2_CHALLENGE_IDENTIFIED", createPlanDefaultValues.paymentMethod.card.cardExpMonth, 
                createPlanDefaultValues.paymentMethod.card.cardExpYear, createPlanDefaultValues.paymentMethod.card.cardCvv,
                "WorldPay", createPlanDefaultValues, _requestHeader!, planCreateResponse.InstallmentPlanNumber);
            var jResponse1 = await _settings.SendGetSettingsRequestAsync( _requestHeader!, int.Parse(_envConfig.NgMockV2BU));
            jResponse1!.MerchantSettings.DefaultPlanStrategy = "SecuredPlan";
            var jResponseUpdated1 = await _settingsSave.SendUpdateSettingsRequestAsync( _requestHeader!, jResponse1);
            Assert.That(jResponseUpdated1.MerchantSettings.DefaultPlanStrategy, Is.EqualTo("SecuredPlan"));
            Console.WriteLine("CreatePlanWithTerminalWorldPayWith3DsAndUpdateCard is Done\n");
        }
        catch (Exception exception)
        {
            var jResponse1 = await _settings.SendGetSettingsRequestAsync( _requestHeader!, int.Parse(_envConfig.NgMockV2BU));
            jResponse1!.MerchantSettings.DefaultPlanStrategy = "SecuredPlan";
            var jResponseUpdated1 = await _settingsSave.SendUpdateSettingsRequestAsync( _requestHeader!, jResponse1);
            Assert.That(jResponseUpdated1.MerchantSettings.DefaultPlanStrategy, Is.EqualTo("SecuredPlan"));
            Assert.Fail("Error in CreatePlanWithTerminalWorldPayWith3DsAndUpdateCard \n" + exception + "\n");
        }
    }
    
    [TestCase(Category = "GatewaysCreatePlanPaymentForm4With3DsAndUpdateCardTests")]
    [Test(Description = "CreatePlanWithTerminalStripeDirectWith3DsAndUpdateCard")]
    public async Task CreatePlanWithTerminalStripeDirectWith3DsAndUpdateCard()
    {
        try
        {
            Console.WriteLine("\nStarting CreatePlanWithTerminalStripeDirectWith3DsAndUpdateCard");
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "yes");
            var planCreateResponse = await _installmentPlans.CreatePlanInitiateAsync(Environment.GetEnvironmentVariable("ApiV3")!,  _requestHeader!, "Active", 
                new Random().Next(2, 6), _envConfig.StripeDirectTerminal, createPlanDefaultValues);
            Assert.That(planCreateResponse.Status, Is.EqualTo("Initialized"));
            Assert.That(await _doTheChallenge.DoTheChallengeV4Pf4Async(planCreateResponse, createPlanDefaultValues, 12, "StripeDirect", null,  _requestHeader!), Is.True);
            _initiateUpdatePaymentDataRequest.InstallmentPlanNumber = planCreateResponse.InstallmentPlanNumber;
            var jResponse = await _initiateUpdatePaymentData.SendInitiateUpdatePaymentDataRequestAsync( _requestHeader!, _initiateUpdatePaymentDataRequest);
            await _doUpdateCard.DoUpdateCreditCardAsync(jResponse.UpdateURL, Environment.GetEnvironmentVariable("Card5454")!, 
                createPlanDefaultValues.paymentMethod.card.cardHolderFullName, createPlanDefaultValues.paymentMethod.card.cardExpMonth, 
                createPlanDefaultValues.paymentMethod.card.cardExpYear, createPlanDefaultValues.paymentMethod.card.cardCvv, "StripeDirect", createPlanDefaultValues,
                 _requestHeader!, planCreateResponse.InstallmentPlanNumber);
            Console.WriteLine("CreatePlanWithTerminalStripeDirectWith3DsAndUpdateCard is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in CreatePlanWithTerminalStripeDirectWith3DsAndUpdateCard \n" + exception + "\n");
        }
    }
    
    [TestCase(Category = "GatewaysCreatePlanPaymentForm4With3DsAndUpdateCardTests")]
    [Test(Description = "CreatePlanWithTerminalReachWith3DsAndUpdateCard")]
    public async Task CreatePlanWithTerminalReachWith3DsAndUpdateCard()
    {
        try
        {
            Console.WriteLine("\nStarting CreatePlanWithTerminalReachWith3DsAndUpdateCard");
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "yes");
            createPlanDefaultValues.paymentMethod.card.cardNumber = Environment.GetEnvironmentVariable("Card5454")!;
            var planCreateResponse = await _installmentPlans.CreatePlanInitiateAsync(Environment.GetEnvironmentVariable("ApiV3")!,  _requestHeader!, "Active", 
                new Random().Next(2, 6), _envConfig.ReachTerminal, createPlanDefaultValues);
            Assert.That(planCreateResponse.Status, Is.EqualTo("Initialized"));
            Assert.That(await _doTheChallenge.DoTheChallengeV4Pf4Async(planCreateResponse, createPlanDefaultValues, 12, "Reach", null,  _requestHeader!), Is.True);
            _initiateUpdatePaymentDataRequest.InstallmentPlanNumber = planCreateResponse.InstallmentPlanNumber;
            var jResponse = await _initiateUpdatePaymentData.SendInitiateUpdatePaymentDataRequestAsync( _requestHeader!, _initiateUpdatePaymentDataRequest);
            await _doUpdateCard.DoUpdateCreditCardAsync(jResponse.UpdateURL, Environment.GetEnvironmentVariable("Card5454")!, 
                createPlanDefaultValues.paymentMethod.card.cardHolderFullName, createPlanDefaultValues.paymentMethod.card.cardExpMonth, 
                createPlanDefaultValues.paymentMethod.card.cardExpYear, createPlanDefaultValues.paymentMethod.card.cardCvv, "Reach", createPlanDefaultValues,
                 _requestHeader!, planCreateResponse.InstallmentPlanNumber);
            Console.WriteLine("CreatePlanWithTerminalReachWith3DsAndUpdateCard is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in CreatePlanWithTerminalReachWith3DsAndUpdateCard \n" + exception + "\n");
        }
    }
    
    [TestCase(Category = "GatewaysCreatePlanPaymentForm4With3DsAndUpdateCardTests")]
    [Test(Description = "CreatePlanWithTerminalPaySafeWith3DsAndUpdateCard")]
    public async Task CreatePlanWithTerminalPaySafeWith3DsAndUpdateCard()
    {
        try
        {
            Console.WriteLine("\nStarting CreatePlanWithTerminalPaySafeWith3DsAndUpdateCard");
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "yes");
            var planCreateResponse = await _installmentPlans.CreatePlanInitiateAsync(Environment.GetEnvironmentVariable("ApiV3")!,  _requestHeader!, "Active", 
                new Random().Next(2, 6), _envConfig.PaySafeTerminal, createPlanDefaultValues);
            Assert.That(planCreateResponse.Status, Is.EqualTo("Initialized"));
            Assert.That(await _doTheChallenge.DoTheChallengeV4Pf4Async(planCreateResponse, createPlanDefaultValues, 
                12, "PaySafe", null,  _requestHeader!), Is.True);
            _initiateUpdatePaymentDataRequest.InstallmentPlanNumber = planCreateResponse.InstallmentPlanNumber;
            var jResponse = await _initiateUpdatePaymentData.SendInitiateUpdatePaymentDataRequestAsync( _requestHeader!, 
                _initiateUpdatePaymentDataRequest);
            await _doUpdateCard.DoUpdateCreditCardAsync(jResponse.UpdateURL, _envConfig.Card4111, 
                createPlanDefaultValues.paymentMethod.card.cardHolderFullName, 
                createPlanDefaultValues.paymentMethod.card.cardExpMonth, 
                createPlanDefaultValues.paymentMethod.card.cardExpYear, 
                createPlanDefaultValues.paymentMethod.card.cardCvv, "PaySafe", createPlanDefaultValues,
                 _requestHeader!, planCreateResponse.InstallmentPlanNumber);
            Console.WriteLine("CreatePlanWithTerminalPaySafeWith3DsAndUpdateCard is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in CreatePlanWithTerminalPaySafeWith3DsAndUpdateCard \n" + exception + "\n");
        }
    }
    
    [TestCase(Category = "GatewaysCreatePlanPaymentForm4With3DsAndUpdateCardTests")]
    [Test(Description = "CreatePlanWithTerminalAdyenWith3DsAndUpdateCard")]
    public async Task CreatePlanWithTerminalAdyenWith3DsAndUpdateCard()
    {
        try
        {
            Console.WriteLine("\nStarting CreatePlanWithTerminalAdyenWith3DsAndUpdateCard");
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "yes");
            createPlanDefaultValues.paymentMethod.card.cardNumber = Environment.GetEnvironmentVariable("Card5454")!;
            var planCreateResponse = await _installmentPlans.CreatePlanInitiateAsync(Environment.GetEnvironmentVariable("ApiV3")!,  _requestHeader!, "Active", 
                new Random().Next(2, 6), _envConfig.AdyenTerminal, createPlanDefaultValues);
            Assert.That(planCreateResponse.Status, Is.EqualTo("Initialized"));
            Assert.That(await _doTheChallenge.DoTheChallengeV4Pf4Async(planCreateResponse, createPlanDefaultValues, 12, "Adyen", null,  _requestHeader!), Is.True);
            _initiateUpdatePaymentDataRequest.InstallmentPlanNumber = planCreateResponse.InstallmentPlanNumber;
            var jResponse = await _initiateUpdatePaymentData.SendInitiateUpdatePaymentDataRequestAsync( _requestHeader!, _initiateUpdatePaymentDataRequest);
            await _doUpdateCard.DoUpdateCreditCardAsync(jResponse.UpdateURL, Environment.GetEnvironmentVariable("Card5454")!,
                createPlanDefaultValues.paymentMethod.card.cardHolderFullName,
                createPlanDefaultValues.paymentMethod.card.cardExpMonth,
                createPlanDefaultValues.paymentMethod.card.cardExpYear,
                createPlanDefaultValues.paymentMethod.card.cardCvv, "Adyen", createPlanDefaultValues,
                 _requestHeader!, planCreateResponse.InstallmentPlanNumber);
            Console.WriteLine("CreatePlanWithTerminalAdyenWith3DsAndUpdateCard is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in CreatePlanWithTerminalAdyenWith3DsAndUpdateCard \n" + exception + "\n");
        }
    }
}