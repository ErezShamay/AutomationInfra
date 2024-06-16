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
[AllureSuite("GatewaysExceptionsHandlingTests")]
[AllureDisplayIgnored]
[Parallelizable(scope: ParallelScope.All)]
public class GatewaysExceptionsHandlingTests
{
    private readonly InstallmentPlans _installmentPlans;
    private RequestHeader? _requestHeader;
    private readonly Settings _settings;
    private readonly SettingsSave _settingsSave;
    private readonly EnvConfig _envConfig;
    
    public GatewaysExceptionsHandlingTests()
    {
        Console.WriteLine("Starting Setup");
        _installmentPlans = new InstallmentPlans();
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
        Console.WriteLine("Setup is Done");
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
            var jResponse = await _settings.SendGetSettingsRequestAsync( _requestHeader!, int.Parse(_envConfig.NgMockV2BU));
            jResponse!.MerchantSettings.DefaultPlanStrategy = "SecuredPlan3";
            var jResponseUpdated = await _settingsSave.SendUpdateSettingsRequestAsync( _requestHeader!, jResponse);
            Assert.That(jResponseUpdated.MerchantSettings.DefaultPlanStrategy, Is.EqualTo("SecuredPlan3"));
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            createPlanDefaultValues.paymentMethod.card.cardNumber = Environment.GetEnvironmentVariable("Card5454")!;
            createPlanDefaultValues.planData.totalAmount = AmountGenerator.GenerateAmountWithMinMaxValues(100, 4000);
            createPlanDefaultValues.shopper = new ShopperDefaultValues
            {
                fullName = "REFUSED41"
            };
            createPlanDefaultValues.paymentMethod = new PaymentMethodDefaultValues
            {
                card = new CardDefaultValues
                {
                    cardNumber = "4444333322221111",
                    cardHolderFullName = "REFUSED41"
                }
            };
            
            var planCreateResponse = await _installmentPlans.CreatePlanAsync(
                Environment.GetEnvironmentVariable("ApiV3")!,  _requestHeader!, "Active", 
                new Random().Next(4, 12), _envConfig.WorldPayTerminal, 
                createPlanDefaultValues, "yes");
            Assert.That(planCreateResponse.Error.ExtraData.GatewayErrorDescription.Equals("LOST CARD"));
            Console.WriteLine("CreatePlanWithTerminalWorldPayVoid is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in CreatePlanWithTerminalWorldPayVoid \n" + exception + "\n");
        }
    }
    
    [TestCase(Category = "GatewaysExceptionsHandlingTests")]
    [Test(Description = "ValidateCheckoutExceptionsInvalidTransaction")]
    public async Task ValidateCheckoutExceptionsInvalidTransaction()
    {
        try
        {
            Console.WriteLine("\nStarting ValidateCheckoutExceptionsInvalidTransaction");
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            createPlanDefaultValues.paymentMethod = new PaymentMethodDefaultValues
            {
                card = new CardDefaultValues
                {
                    cardNumber = "4024007103573027"
                }
            };
            var planCreateResponse = await _installmentPlans.CreatePlanAsync(
                Environment.GetEnvironmentVariable("ApiV3")!,  _requestHeader!, "Active", 
                new Random().Next(4, 12), _envConfig.CheckoutTerminal, 
                createPlanDefaultValues, "yes");
            Assert.That(planCreateResponse.Error.ExtraData.GatewayErrorDescription.Equals("Invalid Transaction"));
            Console.WriteLine("ValidateCheckoutExceptionsInvalidTransaction is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in ValidateCheckoutExceptionsInvalidTransaction \n" + exception + "\n");
        }
    }
    
    [TestCase(Category = "GatewaysExceptionsHandlingTests")]
    [Test(Description = "ValidateCheckoutExceptionsInsufficientFunds")]
    public async Task ValidateCheckoutExceptionsInsufficientFunds()
    {
        try
        {
            Console.WriteLine("\nStarting ValidateCheckoutExceptionsInsufficientFunds");
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            createPlanDefaultValues.paymentMethod = new PaymentMethodDefaultValues
            {
                card = new CardDefaultValues
                {
                    cardNumber = "4544249167673670"
                }
            };
            var planCreateResponse = await _installmentPlans.CreatePlanAsync(
                Environment.GetEnvironmentVariable("ApiV3")!,  _requestHeader!, "Active", 
                new Random().Next(4, 12), _envConfig.CheckoutTerminal, 
                createPlanDefaultValues, "yes");
            Assert.That(planCreateResponse.Error.ExtraData.GatewayErrorDescription.Equals("Insufficient Funds"));
            Console.WriteLine("ValidateCheckoutExceptionsInsufficientFunds is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in ValidateCheckoutExceptionsInsufficientFunds \n" + exception + "\n");
        }
    }
    
    [TestCase(Category = "GatewaysExceptionsHandlingTests")]
    [Test(Description = "ValidateCheckoutExceptionsActivityAmountLimitExceeded")]
    public async Task ValidateCheckoutExceptionsActivityAmountLimitExceeded()
    {
        try
        {
            Console.WriteLine("\nStarting ValidateCheckoutExceptionsActivityAmountLimitExceeded");
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            createPlanDefaultValues.paymentMethod = new PaymentMethodDefaultValues
            {
                card = new CardDefaultValues
                {
                    cardNumber = "4556294593757189"
                }
            };
            var planCreateResponse = await _installmentPlans.CreatePlanAsync(
                Environment.GetEnvironmentVariable("ApiV3")!,  _requestHeader!, "Active", 
                new Random().Next(4, 12), _envConfig.CheckoutTerminal, 
                createPlanDefaultValues, "yes");
            Assert.That(planCreateResponse.Error.ExtraData.GatewayErrorDescription.Equals("Exceeds Withdrawal Value/Amount Limits"));
            Console.WriteLine("ValidateCheckoutExceptionsActivityAmountLimitExceeded is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in ValidateCheckoutExceptionsActivityAmountLimitExceeded \n" + exception + "\n");
        }
    }
    
    [TestCase(Category = "GatewaysExceptionsHandlingTests")]
    [Test(Description = "ValidateCheckoutExceptionsRestrictedCard")]
    public async Task ValidateCheckoutExceptionsRestrictedCard()
    {
        try
        {
            Console.WriteLine("\nStarting ValidateCheckoutExceptionsRestrictedCard");
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            createPlanDefaultValues.paymentMethod = new PaymentMethodDefaultValues
            {
                card = new CardDefaultValues
                {
                    cardNumber = "4818924250131070"
                }
            };
            var planCreateResponse = await _installmentPlans.CreatePlanAsync(
                Environment.GetEnvironmentVariable("ApiV3")!,  _requestHeader!, "Active", 
                new Random().Next(4, 12), _envConfig.CheckoutTerminal, 
                createPlanDefaultValues, "yes");
            Assert.That(planCreateResponse.Error.ExtraData.GatewayErrorDescription.Equals("Restricted Card"));
            Console.WriteLine("ValidateCheckoutExceptionsRestrictedCard is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in ValidateCheckoutExceptionsRestrictedCard \n" + exception + "\n");
        }
    }
    
    [TestCase(Category = "GatewaysExceptionsHandlingTests")]
    [Test(Description = "ValidateCheckoutExceptionsSecurityViolation")]
    public async Task ValidateCheckoutExceptionsSecurityViolation()
    {
        try
        {
            Console.WriteLine("\nStarting ValidateCheckoutExceptionsSecurityViolation");
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            createPlanDefaultValues.paymentMethod = new PaymentMethodDefaultValues
            {
                card = new CardDefaultValues
                {
                    cardNumber = "4556253752712245"
                }
            };
            var planCreateResponse = await _installmentPlans.CreatePlanAsync(
                Environment.GetEnvironmentVariable("ApiV3")!,  _requestHeader!, "Active", 
                new Random().Next(4, 12), _envConfig.CheckoutTerminal, 
                createPlanDefaultValues, "yes");
            Assert.That(planCreateResponse.Error.ExtraData.GatewayErrorDescription.Equals("Security Violation"));
            Console.WriteLine("ValidateCheckoutExceptionsSecurityViolation is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in ValidateCheckoutExceptionsSecurityViolation \n" + exception + "\n");
        }
    }
    
    [TestCase(Category = "GatewaysExceptionsHandlingTests")]
    [Test(Description = "ValidateCheckoutExceptionsResponseReceivedTooLate_CancelAfterOrTransactionRejectedOrInternalError")]
    public async Task ValidateCheckoutExceptionsResponseReceivedTooLate_CancelAfterOrTransactionRejectedOrInternalError()
    {
        try
        {
            Console.WriteLine("\nStarting ValidateCheckoutExceptionsResponseReceivedTooLate_CancelAfterOrTransactionRejectedOrInternalError");
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            createPlanDefaultValues.paymentMethod = new PaymentMethodDefaultValues
            {
                card = new CardDefaultValues
                {
                    cardNumber = "4095254802642505"
                }
            };
            var planCreateResponse = await _installmentPlans.CreatePlanAsync(
                Environment.GetEnvironmentVariable("ApiV3")!,  _requestHeader!, "Active", 
                new Random().Next(4, 12), _envConfig.CheckoutTerminal, 
                createPlanDefaultValues, "yes");
            Assert.That(planCreateResponse.Error.ExtraData.GatewayErrorDescription.Equals("CancelAfter"));
            Console.WriteLine("ValidateCheckoutExceptionsResponseReceivedTooLate_CancelAfterOrTransactionRejectedOrInternalError is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in ValidateCheckoutExceptionsResponseReceivedTooLate_CancelAfterOrTransactionRejectedOrInternalError \n" + exception + "\n");
        }
    }
    
    [TestCase(Category = "GatewaysExceptionsHandlingTests")]
    [Test(Description = "ValidateCheckoutExceptionsLostCardPickUp")]
    public async Task ValidateCheckoutExceptionsLostCardPickUp()
    {
        try
        {
            Console.WriteLine("\nStarting ValidateCheckoutExceptionsLostCardPickUp");
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            createPlanDefaultValues.paymentMethod = new PaymentMethodDefaultValues
            {
                card = new CardDefaultValues
                {
                    cardNumber = "4941202060999329"
                }
            };
            var planCreateResponse = await _installmentPlans.CreatePlanAsync(
                Environment.GetEnvironmentVariable("ApiV3")!,  _requestHeader!, "Active", 
                new Random().Next(4, 12), _envConfig.CheckoutTerminal, 
                createPlanDefaultValues, "yes");
            Assert.That(planCreateResponse.Error.ExtraData.GatewayErrorDescription.Equals("Lost Card - Pick Up"));
            Console.WriteLine("ValidateCheckoutExceptionsLostCardPickUp is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in ValidateCheckoutExceptionsLostCardPickUp \n" + exception + "\n");
        }
    }
    
    [TestCase(Category = "GatewaysExceptionsHandlingTests")]
    [Test(Description = "ValidateCheckoutExceptions3dsAuthenticationRequired")]
    public async Task ValidateCheckoutExceptions3dsAuthenticationRequired()
    {
        try
        {
            Console.WriteLine("\nStarting ValidateCheckoutExceptions3dsAuthenticationRequired");
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            createPlanDefaultValues.paymentMethod = new PaymentMethodDefaultValues
            {
                card = new CardDefaultValues
                {
                    cardNumber = "4500622868341387"
                }
            };
            var planCreateResponse = await _installmentPlans.CreatePlanAsync(
                Environment.GetEnvironmentVariable("ApiV3")!,  _requestHeader!, "Active", 
                new Random().Next(4, 12), _envConfig.CheckoutTerminal, 
                createPlanDefaultValues, "yes");
            Assert.That(planCreateResponse.Error.ExtraData.GatewayErrorDescription.Equals("3D-Secure Authentication Required"));
            Console.WriteLine("ValidateCheckoutExceptions3dsAuthenticationRequired is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in ValidateCheckoutExceptions3dsAuthenticationRequired \n" + exception + "\n");
        }
    }
    
    [TestCase(Category = "GatewaysExceptionsHandlingTests")]
    [Test(Description = "ValidateCheckoutExceptionsLifecycle")]
    public async Task ValidateCheckoutExceptionsLifecycle()
    {
        try
        {
            Console.WriteLine("\nStarting ValidateCheckoutExceptionsLifecycle");
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            createPlanDefaultValues.paymentMethod = new PaymentMethodDefaultValues
            {
                card = new CardDefaultValues
                {
                    cardNumber = "5577483213391204"
                }
            };
            var planCreateResponse = await _installmentPlans.CreatePlanAsync(
                Environment.GetEnvironmentVariable("ApiV3")!,  _requestHeader!, "Active", 
                new Random().Next(4, 12), _envConfig.CheckoutTerminal, 
                createPlanDefaultValues, "yes");
            Assert.That(planCreateResponse.Error.ExtraData.GatewayErrorDescription.Equals("Lifecycle"));
            Console.WriteLine("ValidateCheckoutExceptionsLifecycle is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in ValidateCheckoutExceptionsLifecycle \n" + exception + "\n");
        }
    }
    
    [TestCase(Category = "GatewaysExceptionsHandlingTests")]
    [Test(Description = "ValidateCheckoutExceptionsSecurity")]
    public async Task ValidateCheckoutExceptionsSecurity()
    {
        try
        {
            Console.WriteLine("\nStarting ValidateCheckoutExceptionsSecurity");
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            createPlanDefaultValues.paymentMethod = new PaymentMethodDefaultValues
            {
                card = new CardDefaultValues
                {
                    cardNumber = "5357219827207436"
                }
            };
            var planCreateResponse = await _installmentPlans.CreatePlanAsync(
                Environment.GetEnvironmentVariable("ApiV3")!,  _requestHeader!, "Active", 
                new Random().Next(4, 12), _envConfig.CheckoutTerminal, 
                createPlanDefaultValues, "yes");
            Assert.That(planCreateResponse.Error.ExtraData.GatewayErrorDescription.Equals("Security"));
            Console.WriteLine("ValidateCheckoutExceptionsSecurity is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in ValidateCheckoutExceptionsSecurity \n" + exception + "\n");
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
            createPlanDefaultValues.paymentMethod = new PaymentMethodDefaultValues
            {
                card = new CardDefaultValues
                {
                    cardNumber = "378282246310005",
                    cardExpMonth = 5,
                    cardExpYear = 26
                }
            };
            var planCreateResponse = await _installmentPlans.CreatePlanAsync(
                Environment.GetEnvironmentVariable("ApiV3")!,  _requestHeader!, "Active", 
                new Random().Next(4, 12), _envConfig.BlueSnapDirectTerminal,
                createPlanDefaultValues, "yes");
            Assert.That(planCreateResponse.Error.ExtraData.GatewayErrorDescription.Equals("VALIDATION_GENERAL_FAILURE"));
            Console.WriteLine("CreatePlanWithTerminalBluesnapDirectVoid is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in CreatePlanWithTerminalBluesnapDirectVoid \n" + exception + "\n");
        }
    }
    
    [TestCase(Category = "GatewaysExceptionsHandlingTests")]
    [Test(Description = "ValidateStripeDirectExceptionsYourCardHasExpired")]
    public async Task ValidateStripeDirectExceptionsYourCardHasExpired()
    {
        try
        {
            Console.WriteLine("\nStarting ValidateStripeDirectExceptionsYourCardHasExpired");
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            createPlanDefaultValues.paymentMethod = new PaymentMethodDefaultValues
            {
                card = new CardDefaultValues
                {
                    cardNumber = "4000000000000069"
                }
            };
            var planCreateResponse = await _installmentPlans.CreatePlanAsync(
                Environment.GetEnvironmentVariable("ApiV3")!,  _requestHeader!, "Active", 
                new Random().Next(4, 12), _envConfig.StripeDirectTerminal, 
                createPlanDefaultValues, "yes");
            Assert.That(planCreateResponse.Error.ExtraData.GatewayErrorDescription.Equals("Your card has expired."));
            Console.WriteLine("ValidateStripeDirectExceptionsYourCardHasExpired is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in ValidateStripeDirectExceptionsYourCardHasExpired \n" + exception + "\n");
        }
    }
    
    [TestCase(Category = "GatewaysExceptionsHandlingTests")]
    [Test(Description = "ValidateStripeDirectExceptionsYourCardHasExpired")]
    public async Task ValidateStripeDirectExceptionsIncorrectNumber()
    {
        try
        {
            Console.WriteLine("\nStarting ValidateStripeDirectExceptionsYourCardHasExpired");
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            createPlanDefaultValues.paymentMethod = new PaymentMethodDefaultValues
            {
                card = new CardDefaultValues
                {
                    cardNumber = "4242424242424241"
                }
            };
            var planCreateResponse = await _installmentPlans.CreatePlanAsync(
                Environment.GetEnvironmentVariable("ApiV3")!,  _requestHeader!, "Active", 
                new Random().Next(4, 12), _envConfig.StripeDirectTerminal, 
                createPlanDefaultValues, "yes");
            Assert.That(planCreateResponse.Error.ExtraData.GatewayErrorDescription.Equals("Your card number is incorrect."));
            Console.WriteLine("ValidateStripeDirectExceptionsYourCardHasExpired is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in ValidateStripeDirectExceptionsYourCardHasExpired \n" + exception + "\n");
        }
    }
    
    [TestCase(Category = "GatewaysExceptionsHandlingTests")]
    [Test(Description = "ValidateStripeDirectExceptionsCardDeclined")]
    public async Task ValidateStripeDirectExceptionsCardDeclined()
    {
        try
        {
            Console.WriteLine("\nStarting ValidateStripeDirectExceptionsCardDeclined");
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            createPlanDefaultValues.paymentMethod = new PaymentMethodDefaultValues
            {
                card = new CardDefaultValues
                {
                    cardNumber = "4000000000000002"
                }
            };
            var planCreateResponse = await _installmentPlans.CreatePlanAsync(
                Environment.GetEnvironmentVariable("ApiV3")!,  _requestHeader!, "Active", 
                new Random().Next(4, 12), _envConfig.StripeDirectTerminal, 
                createPlanDefaultValues, "yes");
            Assert.That(planCreateResponse.Error.ExtraData.GatewayErrorDescription.Equals("Your card was declined."));
            Console.WriteLine("ValidateStripeDirectExceptionsCardDeclined is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in ValidateStripeDirectExceptionsCardDeclined \n" + exception + "\n");
        }
    }
    
    [TestCase(Category = "GatewaysExceptionsHandlingTests")]
    [Test(Description = "ValidateStripeDirectExceptionsIncorrectCvc")]
    public async Task ValidateStripeDirectExceptionsIncorrectCvc()
    {
        try
        {
            Console.WriteLine("\nStarting ValidateStripeDirectExceptionsIncorrectCvc");
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            createPlanDefaultValues.paymentMethod = new PaymentMethodDefaultValues
            {
                card = new CardDefaultValues
                {
                    cardNumber = "4000000000000127"
                }
            };
            var planCreateResponse = await _installmentPlans.CreatePlanAsync(
                Environment.GetEnvironmentVariable("ApiV3")!,  _requestHeader!, "Active", 
                new Random().Next(4, 12), _envConfig.StripeDirectTerminal, 
                createPlanDefaultValues, "yes");
            Assert.That(planCreateResponse.Error.ExtraData.GatewayErrorDescription.Equals("Your card's security code is incorrect."));
            Console.WriteLine("ValidateStripeDirectExceptionsIncorrectCvc is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in ValidateStripeDirectExceptionsIncorrectCvc \n" + exception + "\n");
        }
    }
    
    // [TestCase(Category = "GatewaysExceptionsHandlingTests")]
    // [Test(Description = "ValidateBamboraExceptionsVisaCardShouldBeDeclined")]
    // public async Task ValidateBamboraExceptionsVisaCardShouldBeDeclined()
    // {
    //     try
    //     {
    //         Console.WriteLine("\nStarting ValidateBamboraExceptionsVisaCardShouldBeDeclined");
    //         var createPlanDefaultValues = new CreatePlanDefaultValues();
    //         var is3DsVarsCreation = new Is3DsVarsCreation();
    //         var jResponse = await _settings.SendGetSettingsRequestAsync( _requestHeader!, int.Parse(_envConfig.NgMockV2BU));
    //         jResponse!.MerchantSettings.DefaultPlanStrategy = "SecuredPlan4";
    //         var jResponseUpdated = await _settingsSave.SendUpdateSettingsRequestAsync( _requestHeader!, jResponse);
    //         Assert.That(jResponseUpdated.MerchantSettings.DefaultPlanStrategy, Is.EqualTo("SecuredPlan4"));
    //         createPlanDefaultValues.planData.totalAmount = AmountGenerator.GenerateAmountWithMinMaxValues(10, 99);
    //         createPlanDefaultValues.paymentMethod.card.cardNumber = "4003050500040005";
    //         createPlanDefaultValues.paymentMethod.card.cardCvv = "123";
    //         createPlanDefaultValues.planData.currency = "GBP";
    //         createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
    //         var planCreateResponse = await _installmentPlans.CreatePlanAsync(
    //             Environment.GetEnvironmentVariable("ApiV3")!,  _requestHeader!, "Active", 
    //             new Random().Next(4, 12), _envConfig.BamboraTerminal,
    //             createPlanDefaultValues, "yes");
    //         Assert.That(planCreateResponse.Error.ExtraData.GatewayErrorDescription.Equals("DECLINE"));
    //         Console.WriteLine("ValidateBamboraExceptionsVisaCardShouldBeDeclined is Done\n");
    //     }
    //     catch (Exception exception)
    //     {
    //         Assert.Fail("Error in ValidateBamboraExceptionsVisaCardShouldBeDeclined \n" + exception + "\n");
    //     }
    // }
    //
    // [TestCase(Category = "GatewaysExceptionsHandlingTests")]
    // [Test(Description = "ValidateBamboraExceptionsMasterCardCardShouldBeDeclined")]
    // public async Task ValidateBamboraExceptionsMasterCardCardShouldBeDeclined()
    // {
    //     try
    //     {
    //         Console.WriteLine("\nStarting ValidateBamboraExceptionsMasterCardCardShouldBeDeclined");
    //         var createPlanDefaultValues = new CreatePlanDefaultValues();
    //         var is3DsVarsCreation = new Is3DsVarsCreation();
    //         var jResponse = await _settings.SendGetSettingsRequestAsync( _requestHeader!, int.Parse(_envConfig.NgMockV2BU));
    //         jResponse!.MerchantSettings.DefaultPlanStrategy = "SecuredPlan4";
    //         var jResponseUpdated = await _settingsSave.SendUpdateSettingsRequestAsync( _requestHeader!, jResponse);
    //         Assert.That(jResponseUpdated.MerchantSettings.DefaultPlanStrategy, Is.EqualTo("SecuredPlan4"));
    //         createPlanDefaultValues.planData.totalAmount = AmountGenerator.GenerateAmountWithMinMaxValues(10, 99);
    //         createPlanDefaultValues.paymentMethod.card.cardNumber = "5100000020002000";
    //         createPlanDefaultValues.paymentMethod.card.cardCvv = "123";
    //         createPlanDefaultValues.planData.currency = "GBP";
    //         createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
    //         var planCreateResponse = await _installmentPlans.CreatePlanAsync(
    //             Environment.GetEnvironmentVariable("ApiV3")!,  _requestHeader!, "Active", 
    //             new Random().Next(4, 12), _envConfig.BamboraTerminal,
    //             createPlanDefaultValues, "yes");
    //         Assert.That(planCreateResponse.Error.ExtraData.GatewayErrorDescription.Equals("DECLINE"));
    //         Console.WriteLine("ValidateBamboraExceptionsMasterCardCardShouldBeDeclined is Done\n");
    //     }
    //     catch (Exception exception)
    //     {
    //         Assert.Fail("Error in ValidateBamboraExceptionsMasterCardCardShouldBeDeclined \n" + exception + "\n");
    //     }
    // }
    //
    // [TestCase(Category = "GatewaysExceptionsHandlingTests")]
    // [Test(Description = "ValidateBamboraExceptionsPlanShouldGetDeclinedDueToUnAuthorizedAmount")]
    // public async Task ValidateBamboraExceptionsPlanShouldGetDeclinedDueToUnAuthorizedAmount()
    // {
    //     try
    //     {
    //         Console.WriteLine("\nStarting ValidateBamboraExceptionsPlanShouldGetDeclinedDueToUnAuthorizedAmount");
    //         var createPlanDefaultValues = new CreatePlanDefaultValues();
    //         var is3DsVarsCreation = new Is3DsVarsCreation();
    //         var jResponse = await _settings.SendGetSettingsRequestAsync( _requestHeader!, int.Parse(_envConfig.NgMockV2BU));
    //         jResponse!.MerchantSettings.DefaultPlanStrategy = "SecuredPlan4";
    //         var jResponseUpdated = await _settingsSave.SendUpdateSettingsRequestAsync( _requestHeader!, jResponse);
    //         Assert.That(jResponseUpdated.MerchantSettings.DefaultPlanStrategy, Is.EqualTo("SecuredPlan4"));
    //         createPlanDefaultValues.planData.totalAmount = AmountGenerator.GenerateAmountWithMinMaxValues(10, 100);
    //         createPlanDefaultValues.paymentMethod.card.cardNumber = "5100000020002000";
    //         createPlanDefaultValues.paymentMethod.card.cardCvv = "123";
    //         createPlanDefaultValues.planData.currency = "GBP";
    //         createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
    //         var planCreateResponse = await _installmentPlans.CreatePlanAsync(
    //             Environment.GetEnvironmentVariable("ApiV3")!,  _requestHeader!, "Active", 
    //             new Random().Next(4, 12), _envConfig.BamboraTerminal,
    //             createPlanDefaultValues, "yes");
    //         Assert.That(planCreateResponse.Error.ExtraData.GatewayErrorDescription.Equals("DECLINE"));
    //         Console.WriteLine("ValidateBamboraExceptionsPlanShouldGetDeclinedDueToUnAuthorizedAmount is Done\n");
    //     }
    //     catch (Exception exception)
    //     {
    //         Assert.Fail("Error in ValidateBamboraExceptionsMasterCardCardShouldBeDeclined \n" + exception + "\n");
    //     }
    // }
}