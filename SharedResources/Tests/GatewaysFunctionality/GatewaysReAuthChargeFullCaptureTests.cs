using Allure.NUnit;
using Allure.NUnit.Attributes;
using NUnit.Framework;
using Splitit.Automation.NG.Backend.BaseActions;
using Splitit.Automation.NG.Backend.Services.AdminApi.InstallmentPlan.InstallmentsPlanFunctionality;
using Splitit.Automation.NG.Backend.Services.AdminApi.Merchant.MerchantFunctionality;
using Splitit.Automation.NG.Backend.Services.V3.DefaultValues;
using Splitit.Automation.NG.Backend.Services.V3.InstallmentPlan.InstallmentPlanApiEndPoint;
using Splitit.Automation.NG.Backend.Tests.TestsSetups;
using Splitit.Automation.NG.Utilities.EnvironmentsConfig;

namespace Splitit.Automation.NG.SharedResources.Tests.GatewaysFunctionality;

[TestFixture]
[AllureNUnit]
[AllureSuite("GatewaysReAuthChargeFullCaptureTests")]
[AllureDisplayIgnored]
[Parallelizable(scope: ParallelScope.All)]
public class GatewaysReAuthChargeFullCaptureTests
{
    private readonly InstallmentPlans _installmentPlans;
    private RequestHeader? _requestHeader;
    private readonly ReAuthFunctionality _reAuthFunctionality;
    private readonly ChargeFunctionality _chargeFunctionality;
    private readonly FullCaptureFunctionality _fullCaptureFunctionality;
    private readonly Settings _settings;
    private readonly SettingsSave _settingsSave;
    private readonly EnvConfig _envConfig;

    public GatewaysReAuthChargeFullCaptureTests()
    {
        Console.WriteLine("Starting Setup");
        _installmentPlans = new InstallmentPlans();
        _settings = new Settings();
        _settingsSave = new SettingsSave();
        _reAuthFunctionality = new ReAuthFunctionality();
        _chargeFunctionality = new ChargeFunctionality();
        _fullCaptureFunctionality = new FullCaptureFunctionality();
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

    [TestCase(Category = "GatewaysReAuthChargeFullCaptureTests")]
    [Test(Description = "CreatePlanWithTerminalCyberSourceReAuthChargeFullCapture")]
    public async Task CreatePlanWithTerminalCyberSourceReAuthChargeFullCapture()
    {
        try
        {
            Console.WriteLine("\nStarting CreatePlanWithTerminalCyberSourceReAuthChargeFullCapture");
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            createPlanDefaultValues.paymentMethod.card.cardNumber = _envConfig.Card4456;
            createPlanDefaultValues.paymentMethod.card.cardExpMonth =
                int.Parse(_envConfig.Card4456mth);
            createPlanDefaultValues.paymentMethod.card.cardExpYear =
                int.Parse(_envConfig.Card4456year);
            createPlanDefaultValues.planData.currency = "GBP";
            createPlanDefaultValues.planData.totalAmount = AmountGenerator.GenerateAmountWithMinMaxValues(10, 999);
            var planCreateResponse = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!, 
                _requestHeader!, "Active", new Random().Next(4, 6), _envConfig.CyberSourceTerminal,
                createPlanDefaultValues);
            var responseReAuth =
                await _reAuthFunctionality.SendPostRequestReAuthAsync( _requestHeader!, planCreateResponse.InstallmentPlanNumber);
            Assert.That(responseReAuth.ResponseHeader.Succeeded);
            var responseCharge =
                await _chargeFunctionality.SendPostRequestChargeFunctionalityAsync( _requestHeader!,
                    planCreateResponse.InstallmentPlanNumber);
            Assert.That(responseCharge.ResponseHeader.Succeeded);
            var jResponseFullCapture =
                await _fullCaptureFunctionality.SendPostRequestFullCaptureAsync( _requestHeader!,
                    planCreateResponse.InstallmentPlanNumber, 0);
            Assert.That(jResponseFullCapture.ResponseHeader.Succeeded);
            Console.WriteLine("CreatePlanWithTerminalCyberSourceReAuthChargeFullCapture is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in CreatePlanWithTerminalCyberSourceReAuthChargeFullCapture \n" + exception + "\n");
        }
    }

    [TestCase(Category = "GatewaysReAuthChargeFullCaptureTests")]
    [Test(Description = "CreatePlanWithTerminalCheckoutReAuthChargeFullCapture")]
    public async Task CreatePlanWithTerminalCheckoutReAuthChargeFullCapture()
    {
        try
        {
            Console.WriteLine("\nStarting CreatePlanWithTerminalCheckoutReAuthChargeFullCapture");
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            var planCreateResponse = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!, 
                _requestHeader!, "Active",
                new Random().Next(4, 6), _envConfig.CheckoutTerminal, createPlanDefaultValues);
            var responseReAuth =
                await _reAuthFunctionality.SendPostRequestReAuthAsync( _requestHeader!, planCreateResponse.InstallmentPlanNumber);
            Assert.That(responseReAuth.ResponseHeader.Succeeded);
            var responseCharge =
                await _chargeFunctionality.SendPostRequestChargeFunctionalityAsync( _requestHeader!,
                    planCreateResponse.InstallmentPlanNumber);
            Assert.That(responseCharge.ResponseHeader.Succeeded);
            var jResponseFullCapture =
                await _fullCaptureFunctionality.SendPostRequestFullCaptureAsync( _requestHeader!,
                    planCreateResponse.InstallmentPlanNumber, 0);
            Assert.That(jResponseFullCapture.ResponseHeader.Succeeded);
            Console.WriteLine("CreatePlanWithTerminalCheckoutReAuthChargeFullCapture is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in CreatePlanWithTerminalCheckoutReAuthChargeFullCapture \n" + exception + "\n");
        }
    }

    [TestCase(Category = "GatewaysReAuthChargeFullCaptureTests")]
    [Test(Description = "CreatePlanWithTerminalBluesnapMorReAuthChargeFullCapture")]
    public async Task CreatePlanWithTerminalBluesnapMorReAuthChargeFullCapture()
    {
        try
        {
            Console.WriteLine("\nStarting CreatePlanWithTerminalBluesnapMorReAuthChargeFullCapture");
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            var planCreateResponse = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!, 
                _requestHeader!, "Active",
                new Random().Next(4, 6), _envConfig.BlueSnapMORTerminal,
                createPlanDefaultValues);
            var responseReAuth =
                await _reAuthFunctionality.SendPostRequestReAuthAsync( _requestHeader!, planCreateResponse.InstallmentPlanNumber);
            Assert.That(responseReAuth.ResponseHeader.Succeeded);
            var responseCharge =
                await _chargeFunctionality.SendPostRequestChargeFunctionalityAsync( _requestHeader!,
                    planCreateResponse.InstallmentPlanNumber);
            Assert.That(responseCharge.ResponseHeader.Succeeded);
            var jResponseFullCapture =
                await _fullCaptureFunctionality.SendPostRequestFullCaptureAsync( _requestHeader!,
                    planCreateResponse.InstallmentPlanNumber, 0);
            Assert.That(jResponseFullCapture.ResponseHeader.Succeeded);
            Console.WriteLine("CreatePlanWithTerminalBluesnapMorReAuthChargeFullCapture is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in CreatePlanWithTerminalBluesnapMorReAuthChargeFullCapture \n" + exception + "\n");
        }
    }

    [TestCase(Category = "GatewaysReAuthChargeFullCaptureTests")]
    [Test(Description = "CreatePlanWithTerminalBluesnapDirectReAuthChargeFullCapture")]
    public async Task CreatePlanWithTerminalBluesnapDirectReAuthChargeFullCapture()
    {
        try
        {
            Console.WriteLine("\nStarting CreatePlanWithTerminalBluesnapDirectReAuthChargeFullCapture");
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            var planCreateResponse = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!, 
                _requestHeader!, "Active",
                new Random().Next(4, 6), _envConfig.BlueSnapDirectTerminal,
                createPlanDefaultValues);
            var responseReAuth =
                await _reAuthFunctionality.SendPostRequestReAuthAsync( _requestHeader!, planCreateResponse.InstallmentPlanNumber);
            Assert.That(responseReAuth.ResponseHeader.Succeeded);
            var responseCharge =
                await _chargeFunctionality.SendPostRequestChargeFunctionalityAsync( _requestHeader!,
                    planCreateResponse.InstallmentPlanNumber);
            Assert.That(responseCharge.ResponseHeader.Succeeded);
            var jResponseFullCapture =
                await _fullCaptureFunctionality.SendPostRequestFullCaptureAsync( _requestHeader!,
                    planCreateResponse.InstallmentPlanNumber, 0);
            Assert.That(jResponseFullCapture.ResponseHeader.Succeeded);
            Console.WriteLine("CreatePlanWithTerminalBluesnapDirectReAuthChargeFullCapture is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in CreatePlanWithTerminalBluesnapDirectReAuthChargeFullCapture \n" + exception + "\n");
        }
    }

    // [TestCase(Category = "GatewaysReAuthChargeFullCaptureTests")]
    // [Test(Description = "CreatePlanWithTerminalBamboraReAuthChargeFullCapture")]
    // public async Task CreatePlanWithTerminalBamboraReAuthChargeFullCapture()
    // {
    //     try
    //     {
    //         Console.WriteLine("\nStarting CreatePlanWithTerminalBamboraReAuthChargeFullCapture");
    //         var createPlanDefaultValues = new CreatePlanDefaultValues();
    //         var is3DsVarsCreation = new Is3DsVarsCreation();
    //         var jResponse = await _settings.SendGetSettingsRequestAsync( _requestHeader!,
    //             int.Parse(_envConfig.NgMockV2BU));
    //         jResponse!.MerchantSettings.DefaultPlanStrategy = "SecuredPlan4";
    //         var jResponseUpdated = await _settingsSave.SendUpdateSettingsRequestAsync( _requestHeader!, jResponse);
    //         Assert.That(jResponseUpdated.MerchantSettings.DefaultPlanStrategy, Is.EqualTo("SecuredPlan4"));
    //         createPlanDefaultValues.planData.totalAmount = AmountGenerator.GenerateAmountWithMinMaxValues(10, 99);
    //         createPlanDefaultValues.paymentMethod.card.cardNumber = _envConfig.Card5100;
    //         createPlanDefaultValues.paymentMethod.card.cardCvv = "123";
    //         createPlanDefaultValues.planData.currency = "GBP";
    //         createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
    //         var planCreateResponse = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!, 
    //             _requestHeader!, "Active",
    //             new Random().Next(4, 6), _envConfig.BamboraTerminal, createPlanDefaultValues);
    //         var responseReAuth =
    //             await _reAuthFunctionality.SendPostRequestReAuthAsync( _requestHeader!, planCreateResponse.InstallmentPlanNumber);
    //         Assert.That(responseReAuth.ResponseHeader.Succeeded);
    //         var responseCharge =
    //             await _chargeFunctionality.SendPostRequestChargeFunctionalityAsync( _requestHeader!,
    //                 planCreateResponse.InstallmentPlanNumber);
    //         Assert.That(responseCharge.ResponseHeader.Succeeded);
    //         var jResponseFullCapture =
    //             await _fullCaptureFunctionality.SendPostRequestFullCaptureAsync( _requestHeader!,
    //                 planCreateResponse.InstallmentPlanNumber, 0);
    //         Assert.That(jResponseFullCapture.ResponseHeader.Succeeded);
    //         Console.WriteLine("CreatePlanWithTerminalBamboraReAuthChargeFullCapture is Done\n");
    //     }
    //     catch (Exception exception)
    //     {
    //         Assert.Fail("Error in CreatePlanWithTerminalBamboraReAuthChargeFullCapture \n" + exception + "\n");
    //     }
    // }

    [TestCase(Category = "GatewaysReAuthChargeFullCaptureTests")]
    [Test(Description = "CreatePlanWithTerminalAuthNetV2ReAuthChargeFullCapture")]
    public async Task CreatePlanWithTerminalAuthNetV2ReAuthChargeFullCapture()
    {
        try
        {
            Console.WriteLine("\nStarting CreatePlanWithTerminalAuthNetV2ReAuthChargeFullCapture");
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            var planCreateResponse = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!, 
                _requestHeader!, "Active",
                new Random().Next(4, 6), _envConfig.AuthorizeNetV2, createPlanDefaultValues);
            var responseReAuth =
                await _reAuthFunctionality.SendPostRequestReAuthAsync( _requestHeader!, planCreateResponse.InstallmentPlanNumber);
            Assert.That(responseReAuth.ResponseHeader.Succeeded);
            var responseCharge =
                await _chargeFunctionality.SendPostRequestChargeFunctionalityAsync( _requestHeader!,
                    planCreateResponse.InstallmentPlanNumber);
            Assert.That(responseCharge.ResponseHeader.Succeeded);
            var jResponseFullCapture =
                await _fullCaptureFunctionality.SendPostRequestFullCaptureAsync( _requestHeader!,
                    planCreateResponse.InstallmentPlanNumber, 0);
            Assert.That(jResponseFullCapture.ResponseHeader.Succeeded);
            Console.WriteLine("CreatePlanWithTerminalAuthNetV2ReAuthChargeFullCapture is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in CreatePlanWithTerminalAuthNetV2ReAuthChargeFullCapture \n" + exception + "\n");
        }
    }

    [TestCase(Category = "GatewaysReAuthChargeFullCaptureTests")]
    [Test(Description = "CreatePlanWithTerminalWorldPayReAuthChargeFullCapture")]
    public async Task CreatePlanWithTerminalWorldPayReAuthChargeFullCapture()
    {
        try
        {
            Console.WriteLine("\nStarting CreatePlanWithTerminalWorldPayReAuthChargeFullCapture");
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            var jResponse = await _settings.SendGetSettingsRequestAsync( _requestHeader!,
                int.Parse(_envConfig.NgMockV2BU));
            jResponse!.MerchantSettings.DefaultPlanStrategy = "SecuredPlan3";
            var jResponseUpdated = await _settingsSave.SendUpdateSettingsRequestAsync( _requestHeader!, jResponse);
            Assert.That(jResponseUpdated.MerchantSettings.DefaultPlanStrategy, Is.EqualTo("SecuredPlan3"));
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            createPlanDefaultValues.paymentMethod.card.cardNumber = Environment.GetEnvironmentVariable("Card5454")!;
            createPlanDefaultValues.planData.totalAmount = AmountGenerator.GenerateAmountWithMinMaxValues(100, 4000);
            var planCreateResponse = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!, 
                _requestHeader!, "Active",
                new Random().Next(4, 6), _envConfig.WorldPayTerminal, createPlanDefaultValues);
            var responseReAuth =
                await _reAuthFunctionality.SendPostRequestReAuthAsync( _requestHeader!, planCreateResponse.InstallmentPlanNumber);
            Assert.That(responseReAuth.ResponseHeader.Succeeded);
            var responseCharge =
                await _chargeFunctionality.SendPostRequestChargeFunctionalityAsync( _requestHeader!,
                    planCreateResponse.InstallmentPlanNumber);
            Assert.That(responseCharge.ResponseHeader.Succeeded);
            var jResponseFullCapture =
                await _fullCaptureFunctionality.SendPostRequestFullCaptureAsync( _requestHeader!,
                    planCreateResponse.InstallmentPlanNumber, 0);
            Assert.That(jResponseFullCapture.ResponseHeader.Succeeded);
            Console.WriteLine("CreatePlanWithTerminalWorldPayReAuthChargeFullCapture is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in CreatePlanWithTerminalWorldPayReAuthChargeFullCapture \n" + exception + "\n");
        }
    }

    [TestCase(Category = "GatewaysReAuthChargeFullCaptureTests")]
    [Test(Description = "CreatePlanWithTerminalStripeDirectReAuthChargeFullCapture")]
    public async Task CreatePlanWithTerminalStripeDirectReAuthChargeFullCapture()
    {
        try
        {
            Console.WriteLine("\nStarting CreatePlanWithTerminalStripeDirectReAuthChargeFullCapture");
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            var jResponse = await _settings.SendGetSettingsRequestAsync( _requestHeader!,
                int.Parse(_envConfig.NgMockV2BU));
            jResponse!.MerchantSettings.DefaultPlanStrategy = "SecuredPlan";
            var jResponseUpdated = await _settingsSave.SendUpdateSettingsRequestAsync( _requestHeader!, jResponse);
            Assert.That(jResponseUpdated.MerchantSettings.DefaultPlanStrategy, Is.EqualTo("SecuredPlan"));
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            var planCreateResponse = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!, 
                _requestHeader!, "Active",
                new Random().Next(4, 6), _envConfig.StripeDirectTerminal,
                createPlanDefaultValues);
            var responseReAuth =
                await _reAuthFunctionality.SendPostRequestReAuthAsync( _requestHeader!, planCreateResponse.InstallmentPlanNumber);
            Assert.That(responseReAuth.ResponseHeader.Succeeded);
            var responseCharge =
                await _chargeFunctionality.SendPostRequestChargeFunctionalityAsync( _requestHeader!,
                    planCreateResponse.InstallmentPlanNumber);
            Assert.That(responseCharge.ResponseHeader.Succeeded);
            var jResponseFullCapture =
                await _fullCaptureFunctionality.SendPostRequestFullCaptureAsync( _requestHeader!,
                    planCreateResponse.InstallmentPlanNumber, 0);
            Assert.That(jResponseFullCapture.ResponseHeader.Succeeded);
            Console.WriteLine("CreatePlanWithTerminalStripeDirectReAuthChargeFullCapture is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in CreatePlanWithTerminalStripeDirectReAuthChargeFullCapture \n" + exception + "\n");
        }
    }

    [TestCase(Category = "GatewaysReAuthChargeFullCaptureTests")]
    [Test(Description = "CreatePlanWithTerminalReachReAuthChargeFullCapture")]
    public async Task CreatePlanWithTerminalReachReAuthChargeFullCapture()
    {
        try
        {
            Console.WriteLine("\nStarting CreatePlanWithTerminalReachReAuthChargeFullCapture");
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            var planCreateResponse = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!, 
                _requestHeader!, "Active",
                new Random().Next(4, 6), _envConfig.ReachTerminal, createPlanDefaultValues);
            var responseReAuth =
                await _reAuthFunctionality.SendPostRequestReAuthAsync( _requestHeader!, planCreateResponse.InstallmentPlanNumber);
            Assert.That(responseReAuth.ResponseHeader.Succeeded);
            var responseCharge =
                await _chargeFunctionality.SendPostRequestChargeFunctionalityAsync( _requestHeader!,
                    planCreateResponse.InstallmentPlanNumber);
            Assert.That(responseCharge.ResponseHeader.Succeeded);
            Console.WriteLine("CreatePlanWithTerminalReachReAuthChargeFullCapture is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in CreatePlanWithTerminalReachReAuthChargeFullCapture \n" + exception + "\n");
        }
    }

    [TestCase(Category = "GatewaysReAuthChargeFullCaptureTests")]
    [Test(Description = "CreatePlanWithTerminalPaySafeReAuthChargeFullCapture")]
    public async Task CreatePlanWithTerminalPaySafeReAuthChargeFullCapture()
    {
        try
        {
            Console.WriteLine("\nStarting CreatePlanWithTerminalPaySafeReAuthChargeFullCapture");
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            var planCreateResponse = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!, 
                _requestHeader!, "Active",
                new Random().Next(4, 6), _envConfig.PaySafeTerminal, createPlanDefaultValues);
            var responseReAuth =
                await _reAuthFunctionality.SendPostRequestReAuthAsync( _requestHeader!, planCreateResponse.InstallmentPlanNumber);
            Assert.That(responseReAuth.ResponseHeader.Succeeded);
            var responseCharge =
                await _chargeFunctionality.SendPostRequestChargeFunctionalityAsync( _requestHeader!,
                    planCreateResponse.InstallmentPlanNumber);
            Assert.That(responseCharge.ResponseHeader.Succeeded);
            var jResponseFullCapture =
                await _fullCaptureFunctionality.SendPostRequestFullCaptureAsync( _requestHeader!,
                    planCreateResponse.InstallmentPlanNumber, 0);
            Assert.That(jResponseFullCapture.ResponseHeader.Succeeded);
            Console.WriteLine("CreatePlanWithTerminalPaySafeReAuthChargeFullCapture is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in CreatePlanWithTerminalPaySafeReAuthChargeFullCapture \n" + exception + "\n");
        }
    }

    [TestCase(Category = "GatewaysReAuthChargeFullCaptureTests")]
    [Test(Description = "CreatePlanWithTerminalAdyenReAuthChargeFullCapture")]
    public async Task CreatePlanWithTerminalAdyenReAuthChargeFullCapture()
    {
        try
        {
            Console.WriteLine("\nStarting CreatePlanWithTerminalAdyenReAuthChargeFullCapture");
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            var planCreateResponse = await _installmentPlans.CreatePlanAsync(
                Environment.GetEnvironmentVariable("ApiV3")!,  _requestHeader!, "Active",
                new Random().Next(4, 6), _envConfig.AdyenTerminal, createPlanDefaultValues);
            var responseReAuth = await _reAuthFunctionality.SendPostRequestReAuthAsync( _requestHeader!, planCreateResponse.InstallmentPlanNumber);
            Assert.That(responseReAuth.ResponseHeader.Succeeded);
            var responseCharge = await _chargeFunctionality.SendPostRequestChargeFunctionalityAsync( _requestHeader!, planCreateResponse.InstallmentPlanNumber);
            Assert.That(responseCharge.ResponseHeader.Succeeded);
            var jResponseFullCapture = await _fullCaptureFunctionality.SendPostRequestFullCaptureAsync( _requestHeader!, planCreateResponse.InstallmentPlanNumber, 0);
            Assert.That(jResponseFullCapture.ResponseHeader.Succeeded);
            Console.WriteLine("CreatePlanWithTerminalAdyenReAuthChargeFullCapture is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in CreatePlanWithTerminalAdyenReAuthChargeFullCapture \n" + exception + "\n");
        }
    }
    
    [TestCase(Category = "GatewaysReAuthChargeFullCaptureTests")]
    [Test(Description = "CreatePlanWithTerminalSpreedlyTerminalReAuthChargeFullCapture")]
    public async Task CreatePlanWithTerminalSpreedlyTerminalReAuthChargeFullCapture()
    {
        try
        {
            Console.WriteLine("\nStarting CreatePlanWithTerminalSpreedlyTerminalReAuthChargeFullCapture");
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            var planCreateResponse = await _installmentPlans.CreatePlanAsync(
                Environment.GetEnvironmentVariable("ApiV3")!,  _requestHeader!, "Active",
                new Random().Next(4, 6), _envConfig.AdyenTerminal, createPlanDefaultValues);
            var responseReAuth = await _reAuthFunctionality.SendPostRequestReAuthAsync( _requestHeader!, planCreateResponse.InstallmentPlanNumber);
            Assert.That(responseReAuth.ResponseHeader.Succeeded);
            var responseCharge = await _chargeFunctionality.SendPostRequestChargeFunctionalityAsync( _requestHeader!, planCreateResponse.InstallmentPlanNumber);
            Assert.That(responseCharge.ResponseHeader.Succeeded);
            var jResponseFullCapture = await _fullCaptureFunctionality.SendPostRequestFullCaptureAsync( _requestHeader!, planCreateResponse.InstallmentPlanNumber, 0);
            Assert.That(jResponseFullCapture.ResponseHeader.Succeeded);
            Console.WriteLine("CreatePlanWithTerminalSpreedlyTerminalReAuthChargeFullCapture is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in CreatePlanWithTerminalSpreedlyTerminalReAuthChargeFullCapture \n" + exception + "\n");
        }
    }
}