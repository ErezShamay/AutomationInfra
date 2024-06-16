using Allure.NUnit;
using Allure.NUnit.Attributes;
using NUnit.Framework;
using Splitit.Automation.NG.Backend.BaseActions;
using Splitit.Automation.NG.Backend.Services.AdminApi.Merchant.MerchantFunctionality;
using Splitit.Automation.NG.Backend.Services.V1.InstallmentPlan.InstallmentPlanApiEndPoints;
using Splitit.Automation.NG.Backend.Services.V3.DefaultValues;
using Splitit.Automation.NG.Backend.Services.V3.InstallmentPlan.InstallmentPlanApiEndPoint;
using Splitit.Automation.NG.Backend.Tests.TestsSetups;
using Splitit.Automation.NG.SharedResources.BaseActions;
using Splitit.Automation.NG.SharedResources.Pages;
using Splitit.Automation.NG.Utilities.EnvironmentsConfig;

namespace Splitit.Automation.NG.SharedResources.Tests.PF4TestSuite;

[TestFixture]
[AllureNUnit]
[AllureSuite("PaymentFormV4TestsSuite")]
[AllureDisplayIgnored]
public class Pf4Tests
{
    private readonly InstallmentPlans _installmentPlans;
    private RequestHeader? _requestHeader;
    private readonly DoTheChallenge _doTheChallenge;
    private readonly Settings _settings;
    private readonly SettingsSave _settingsSave;
    private readonly PaymentFormV4Functionality _paymentFormV4Functionality;
    private readonly PaymentFormV4 _paymentFormV4;
    private readonly GetPgtl _getPgtl;
    private readonly EnvConfig _envConfig;

    public Pf4Tests()
    {
        Console.WriteLine("Starting Pf4Setup");
        _installmentPlans = new InstallmentPlans();
        _doTheChallenge = new DoTheChallenge();
        _settings = new Settings();
        _settingsSave = new SettingsSave();
        _paymentFormV4Functionality = new PaymentFormV4Functionality();
        _paymentFormV4 = new PaymentFormV4();
        _getPgtl = new GetPgtl();
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

    [Category("PaymentFormV4Test")]
    [Test(Description = "CreatePlanWithDebitCard")]
    public async Task CreatePlanWithDebitCard()
    {
        try
        {
            Console.WriteLine("\nStarting CreatePlanWithDebitCard");
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            createPlanDefaultValues.paymentMethod.card.cardNumber = Environment.GetEnvironmentVariable("DebitCard")!;
            var planCreateResponse = await _installmentPlans.CreatePlanInitiateAsync(Environment.GetEnvironmentVariable("ApiV3")!,  _requestHeader!, "Active", 
                1, Environment.GetEnvironmentVariable("NgMockV2Terminal")!, createPlanDefaultValues);
            Assert.That(planCreateResponse.Status, Is.EqualTo("Initialized"));
            Assert.That(await _doTheChallenge.DoTheChallengeV4Pf4Async(planCreateResponse, createPlanDefaultValues, 1, null!,null,  _requestHeader!), Is.True);
            Console.WriteLine("CreatePlanWithDebitCard is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in CreatePlanWithDebitCard \n" + exception + "\n");
        }
    }  
    
    [Category("PaymentFormV4Test")]
    [Test(Description = "CreatePlanWith3Ds")]
    public async Task CreatePlanWith3Ds()
    {
        try
        {
            Console.WriteLine("\nStarting CreatePlanWith3Ds");
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "yes");
            var planCreateResponse = await _installmentPlans.CreatePlanInitiateAsync(Environment.GetEnvironmentVariable("ApiV3")!,  _requestHeader!, "Active", 
                1, Environment.GetEnvironmentVariable("NgMockV2Terminal")!, createPlanDefaultValues);
            Assert.That(planCreateResponse.Status, Is.EqualTo("Initialized"));
            Assert.That(await _doTheChallenge.DoTheChallengeV4Pf4Async(planCreateResponse, createPlanDefaultValues, 12, null!,null,  _requestHeader!), Is.True);
            Console.WriteLine("CreatePlanWith3Ds is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in CreatePlanWith3Ds\n" + exception + "\n");
        }
    }
    
    [Category("PaymentFormV4Test")]
    [Test(Description = "CreatePlanPf4")]
    public async Task CreatePlanPf4()
    {
        try
        {
            Console.WriteLine("\nStarting CreatePlanPf4");
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            var planCreateResponse = await _installmentPlans.CreatePlanInitiateAsync(Environment.GetEnvironmentVariable("ApiV3")!,  _requestHeader!, "Active", 
                1, Environment.GetEnvironmentVariable("NgMockV2Terminal")!, createPlanDefaultValues);
            Assert.That(planCreateResponse.Status, Is.EqualTo("Initialized"));
            Assert.That(await _doTheChallenge.DoTheChallengeV4Pf4Async(planCreateResponse, createPlanDefaultValues, 1, null!,null,  _requestHeader!), Is.True);
            Console.WriteLine("CreatePlanPf4 is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in CreatePlanPf4\n" + exception + "\n");
        }
    }  
    
    [Category("PaymentFormV4Test")]
    [Test(Description = "CreatePlanPf4ValidateNumberOfInstallments1 options")]
    public async Task CreatePlanPf4ValidateNumberOfInstallments1()
    {
        try
        {
            Console.WriteLine("\nStarting CreatePlanPf4ValidateNumberOfInstallments1");
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            var jResponse = await _settings.SendGetSettingsRequestAsync( _requestHeader!, int.Parse(_envConfig.NgMockV2BU));
            jResponse!.MerchantSettings.CustomNumberOfInstallments = "1";
            var jResponseUpdated = await _settingsSave.SendUpdateSettingsRequestAsync( _requestHeader!, jResponse);
            Assert.That(jResponseUpdated.MerchantSettings.CustomNumberOfInstallments, Is.EqualTo("1"));
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            var planCreateResponse = await _installmentPlans.CreatePlanInitiateAsync(Environment.GetEnvironmentVariable("ApiV3")!,  _requestHeader!, "Active", 
                1, Environment.GetEnvironmentVariable("NgMockV2Terminal")!, createPlanDefaultValues);
            Assert.That(planCreateResponse.Status, Is.EqualTo("Initialized"));
            Assert.That(await _doTheChallenge.DoTheChallengeV4Pf4Async(planCreateResponse, createPlanDefaultValues, 1, null!,null,  _requestHeader!), Is.True);
            var jResponse1 = await _settings.SendGetSettingsRequestAsync( _requestHeader!, int.Parse(_envConfig.NgMockV2BU));
            jResponse1!.MerchantSettings.CustomNumberOfInstallments = "";
            var jResponseUpdated1 = await _settingsSave.SendUpdateSettingsRequestAsync( _requestHeader!, jResponse1);
            Assert.That(jResponseUpdated1.MerchantSettings.CustomNumberOfInstallments, Is.EqualTo(""));
            Console.WriteLine("CreatePlanPf4ValidateNumberOfInstallments1 is Done\n");
        }
        catch (Exception exception)
        {
            Console.WriteLine("\nReturning to default merchant settings");
            var jResponse1 = await _settings.SendGetSettingsRequestAsync( _requestHeader!, int.Parse(_envConfig.NgMockV2BU));
            jResponse1!.MerchantSettings.CustomNumberOfInstallments = "";
            var jResponseUpdated1 = await _settingsSave.SendUpdateSettingsRequestAsync( _requestHeader!, jResponse1);
            Console.WriteLine("Validating CustomNumberOfInstallments was updated -> expected null -> actual -> " + jResponseUpdated1.MerchantSettings.CustomNumberOfInstallments);
            Assert.Fail("\nError in CreatePlanPf4ValidateNumberOfInstallment5\n" + exception + "\n");
        }
    }  
    
    [Category("PaymentFormV4Test")]
    [Test(Description = "CreatePlanPf4ValidateNumberOfInstallments2 options")]
    public async Task CreatePlanPf4ValidateNumberOfInstallments2()
    {
        try
        {
            Console.WriteLine("\nStarting CreatePlanPf4ValidateNumberOfInstallments2");
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            var jResponse = await _settings.SendGetSettingsRequestAsync( _requestHeader!, int.Parse(_envConfig.NgMockV2BU));
            jResponse!.MerchantSettings.CustomNumberOfInstallments = "1,2";
            var jResponseUpdated = await _settingsSave.SendUpdateSettingsRequestAsync( _requestHeader!, jResponse);
            Assert.That(jResponseUpdated.MerchantSettings.CustomNumberOfInstallments, Is.EqualTo("1,2"));
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            var planCreateResponse = await _installmentPlans.CreatePlanInitiateAsync(Environment.GetEnvironmentVariable("ApiV3")!,  _requestHeader!, "Active", 
                1, Environment.GetEnvironmentVariable("NgMockV2Terminal")!, createPlanDefaultValues);
            Assert.That(planCreateResponse.Status, Is.EqualTo("Initialized"));
            Assert.That(await _doTheChallenge.DoTheChallengeV4Pf4Async(planCreateResponse, createPlanDefaultValues, 2, null!,null,  _requestHeader!), Is.True);
            var jResponse1 = await _settings.SendGetSettingsRequestAsync( _requestHeader!, int.Parse(_envConfig.NgMockV2BU));
            jResponse1!.MerchantSettings.CustomNumberOfInstallments = "";
            var jResponseUpdated1 = await _settingsSave.SendUpdateSettingsRequestAsync( _requestHeader!, jResponse1);
            Assert.That(jResponseUpdated1.MerchantSettings.CustomNumberOfInstallments, Is.EqualTo(""));
            Console.WriteLine("CreatePlanPf4ValidateNumberOfInstallments2 is Done\n");
        }
        catch (Exception exception)
        {
            Console.WriteLine("\nReturning to default merchant settings");
            var jResponse1 = await _settings.SendGetSettingsRequestAsync( _requestHeader!, int.Parse(_envConfig.NgMockV2BU));
            jResponse1!.MerchantSettings.CustomNumberOfInstallments = "";
            var jResponseUpdated1 = await _settingsSave.SendUpdateSettingsRequestAsync( _requestHeader!, jResponse1);
            Console.WriteLine("Validating CustomNumberOfInstallments was updated -> expected null -> actual -> " + jResponseUpdated1.MerchantSettings.CustomNumberOfInstallments);
            Assert.Fail("\nError in CreatePlanPf4ValidateNumberOfInstallment5\n" + exception + "\n");
        }
    }
    
    [Category("PaymentFormV4Test")]
    [Test(Description = "CreatePlanPf4ValidateNumberOfInstallments3 options")]
    public async Task CreatePlanPf4ValidateNumberOfInstallments3()
    {
        try
        {
            Console.WriteLine("\nStarting CreatePlanPf4ValidateNumberOfInstallments3");
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            var jResponse = await _settings.SendGetSettingsRequestAsync( _requestHeader!, int.Parse(_envConfig.NgMockV2BU));
            jResponse!.MerchantSettings.CustomNumberOfInstallments = "1,2,3";
            var jResponseUpdated = await _settingsSave.SendUpdateSettingsRequestAsync( _requestHeader!, jResponse);
            Assert.That(jResponseUpdated.MerchantSettings.CustomNumberOfInstallments, Is.EqualTo("1,2,3"));
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            var planCreateResponse = await _installmentPlans.CreatePlanInitiateAsync(Environment.GetEnvironmentVariable("ApiV3")!,  _requestHeader!, "Active", 
                1, Environment.GetEnvironmentVariable("NgMockV2Terminal")!, createPlanDefaultValues);
            Assert.That(planCreateResponse.Status, Is.EqualTo("Initialized"));
            Assert.That(await _doTheChallenge.DoTheChallengeV4Pf4Async(planCreateResponse, createPlanDefaultValues, 3, null!,null,  _requestHeader!), Is.True);
            var jResponse1 = await _settings.SendGetSettingsRequestAsync( _requestHeader!, int.Parse(_envConfig.NgMockV2BU));
            jResponse1!.MerchantSettings.CustomNumberOfInstallments = "";
            var jResponseUpdated1 = await _settingsSave.SendUpdateSettingsRequestAsync( _requestHeader!, jResponse1);
            Assert.That(jResponseUpdated1.MerchantSettings.CustomNumberOfInstallments, Is.EqualTo(""));
            Console.WriteLine("CreatePlanPf4ValidateNumberOfInstallments2 is Done\n");
        }
        catch (Exception exception)
        {
            Console.WriteLine("\nReturning to default merchant settings");
            var jResponse1 = await _settings.SendGetSettingsRequestAsync( _requestHeader!, int.Parse(_envConfig.NgMockV2BU));
            jResponse1!.MerchantSettings.CustomNumberOfInstallments = "";
            var jResponseUpdated1 = await _settingsSave.SendUpdateSettingsRequestAsync( _requestHeader!, jResponse1);
            Console.WriteLine("Validating CustomNumberOfInstallments was updated -> expected null -> actual -> " + jResponseUpdated1.MerchantSettings.CustomNumberOfInstallments);
            Assert.Fail("\nError in CreatePlanPf4ValidateNumberOfInstallment5\n" + exception + "\n");
        }
    }
    
    [Category("PaymentFormV4Test")]
    [Test(Description = "CreatePlanPf4ValidateNumberOfInstallments4 options")]
    public async Task CreatePlanPf4ValidateNumberOfInstallments4()
    {
        try
        {
            Console.WriteLine("\nStarting CreatePlanPf4ValidateNumberOfInstallments4");
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            var jResponse = await _settings.SendGetSettingsRequestAsync( _requestHeader!, int.Parse(_envConfig.NgMockV2BU));
            jResponse!.MerchantSettings.CustomNumberOfInstallments = "1,2,3,4";
            var jResponseUpdated = await _settingsSave.SendUpdateSettingsRequestAsync( _requestHeader!, jResponse);
            Assert.That(jResponseUpdated.MerchantSettings.CustomNumberOfInstallments, Is.EqualTo("1,2,3,4"));
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            var planCreateResponse = await _installmentPlans.CreatePlanInitiateAsync(Environment.GetEnvironmentVariable("ApiV3")!,  _requestHeader!, "Active", 
                1, Environment.GetEnvironmentVariable("NgMockV2Terminal")!, createPlanDefaultValues);
            Assert.That(planCreateResponse.Status, Is.EqualTo("Initialized"));
            Assert.That(await _doTheChallenge.DoTheChallengeV4Pf4Async(planCreateResponse, createPlanDefaultValues, 4, null!,null,  _requestHeader!), Is.True);
            var jResponse1 = await _settings.SendGetSettingsRequestAsync( _requestHeader!, int.Parse(_envConfig.NgMockV2BU));
            jResponse1!.MerchantSettings.CustomNumberOfInstallments = "";
            var jResponseUpdated1 = await _settingsSave.SendUpdateSettingsRequestAsync( _requestHeader!, jResponse1);
            Assert.That(jResponseUpdated1.MerchantSettings.CustomNumberOfInstallments, Is.EqualTo(""));
            Console.WriteLine("CreatePlanPf4ValidateNumberOfInstallments4 is Done\n");
        }
        catch (Exception exception)
        {
            Console.WriteLine("\nReturning to default merchant settings");
            var jResponse1 = await _settings.SendGetSettingsRequestAsync( _requestHeader!, int.Parse(_envConfig.NgMockV2BU));
            jResponse1!.MerchantSettings.CustomNumberOfInstallments = "";
            var jResponseUpdated1 = await _settingsSave.SendUpdateSettingsRequestAsync( _requestHeader!, jResponse1);
            Console.WriteLine("Validating CustomNumberOfInstallments was updated -> expected null -> actual -> " + jResponseUpdated1.MerchantSettings.CustomNumberOfInstallments);
            Assert.Fail("\nError in CreatePlanPf4ValidateNumberOfInstallment5\n" + exception + "\n");
        }
    }
    
    [Category("PaymentFormV4Test")]
    [Test(Description = "CreatePlanPf4ValidateNumberOfInstallments5 options")]
    public async Task CreatePlanPf4ValidateNumberOfInstallments5()
    {
        try
        {
            Console.WriteLine("\nStarting CreatePlanPf4ValidateNumberOfInstallments5");
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            var jResponse = await _settings.SendGetSettingsRequestAsync( _requestHeader!, int.Parse(_envConfig.NgMockV2BU));
            jResponse!.MerchantSettings.CustomNumberOfInstallments = "1,2,3,4,5";
            var jResponseUpdated = await _settingsSave.SendUpdateSettingsRequestAsync( _requestHeader!, jResponse);
            Assert.That(jResponseUpdated.MerchantSettings.CustomNumberOfInstallments, Is.EqualTo("1,2,3,4,5"));
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            var planCreateResponse = await _installmentPlans.CreatePlanInitiateAsync(Environment.GetEnvironmentVariable("ApiV3")!,  _requestHeader!, "Active", 
                1, Environment.GetEnvironmentVariable("NgMockV2Terminal")!, createPlanDefaultValues);
            Assert.That(planCreateResponse.Status, Is.EqualTo("Initialized"));
            Assert.That(await _doTheChallenge.DoTheChallengeV4Pf4Async(planCreateResponse, createPlanDefaultValues, 5, null!,null,  _requestHeader!), Is.True);
            var jResponse1 = await _settings.SendGetSettingsRequestAsync( _requestHeader!, int.Parse(_envConfig.NgMockV2BU));
            jResponse1!.MerchantSettings.CustomNumberOfInstallments = "";
            var jResponseUpdated1 = await _settingsSave.SendUpdateSettingsRequestAsync( _requestHeader!, jResponse1);
            Assert.That(jResponseUpdated1.MerchantSettings.CustomNumberOfInstallments, Is.EqualTo(""));
            Console.WriteLine("CreatePlanPf4ValidateNumberOfInstallments5 is Done\n");
        }
        catch (Exception exception)
        {
            Console.WriteLine("\nReturning to default merchant settings");
            var jResponse1 = await _settings.SendGetSettingsRequestAsync( _requestHeader!, int.Parse(_envConfig.NgMockV2BU));
            jResponse1!.MerchantSettings.CustomNumberOfInstallments = "";
            var jResponseUpdated1 = await _settingsSave.SendUpdateSettingsRequestAsync( _requestHeader!, jResponse1);
            Console.WriteLine("Validating CustomNumberOfInstallments was updated -> expected null -> actual -> " + jResponseUpdated1.MerchantSettings.CustomNumberOfInstallments);
            Assert.Fail("\nError in CreatePlanPf4ValidateNumberOfInstallment5\n" + exception + "\n");
        }
    }
    
    [Category("PaymentFormV4Test")]
    [Test(Description = "CreatePlanWithoutBillingInfo")]
    public async Task CreatePlanWithoutBillingInfo()
    {
        try
        {
            Console.WriteLine("\nStarting CreatePlanWithoutBillingInfo");
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            createPlanDefaultValues.billingAddress.addressLine1 = null!;
            createPlanDefaultValues.billingAddress.addressLine2 = null!;
            createPlanDefaultValues.billingAddress.city = null!;
            createPlanDefaultValues.billingAddress.country = null!;
            createPlanDefaultValues.billingAddress.state = null!;
            createPlanDefaultValues.billingAddress.zip = null!;
            var planCreateResponse = await _installmentPlans.CreatePlanInitiateAsync(Environment.GetEnvironmentVariable("ApiV3")!,  _requestHeader!, "Active", 
                new Random().Next(2, 6), Environment.GetEnvironmentVariable("NgMockV2Terminal")!, createPlanDefaultValues);
            Assert.That(planCreateResponse.Status, Is.EqualTo("Initialized"));
            Assert.That(_paymentFormV4Functionality.ValidatePf4Fields(planCreateResponse, "yes"));
            Console.WriteLine("CreatePlanWithoutBillingInfo is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in CreatePlanWithoutBillingInfo\n" + exception + "\n");
        }
    }
    
    [Category("PaymentFormV4Test")]
    [Test(Description = "CreatePlanPartialBillingInfo")]
    public async Task CreatePlanPartialBillingInfo()
    {
        try
        {
            Console.WriteLine("\nStarting CreatePlanPartialBillingInfo");
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            createPlanDefaultValues.billingAddress.addressLine1 = null!;
            var planCreateResponse = await _installmentPlans.CreatePlanInitiateAsync(Environment.GetEnvironmentVariable("ApiV3")!,  _requestHeader!, "Active", 
                new Random().Next(2, 6), Environment.GetEnvironmentVariable("NgMockV2Terminal")!, createPlanDefaultValues);
            Assert.That(planCreateResponse.Status, Is.EqualTo("Initialized"));
            Assert.That(_paymentFormV4Functionality.ValidatePf4Fields(planCreateResponse, "yes"));
            Console.WriteLine("CreatePlanPartialBillingInfo is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in CreatePlanPartialBillingInfo\n" + exception + "\n");
        }
    }
    
    [Category("PaymentFormV4Test")]
    [Test(Description = "CreatePlanFullBillingInfo")]
    public async Task CreatePlanFullBillingInfo()
    {
        try
        {
            Console.WriteLine("\nStarting CreatePlanFullBillingInfo");
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            var planCreateResponse = await _installmentPlans.CreatePlanInitiateAsync(Environment.GetEnvironmentVariable("ApiV3")!,  _requestHeader!, "Active", 
                1, Environment.GetEnvironmentVariable("NgMockV2Terminal")!, createPlanDefaultValues);
            Assert.That(planCreateResponse.Status, Is.EqualTo("Initialized"));
            Assert.That(await _doTheChallenge.DoTheChallengeV4Pf4Async(planCreateResponse, createPlanDefaultValues, 1, null!,null,  _requestHeader!), Is.True);
            Console.WriteLine("CreatePlanFullBillingInfo is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in CreatePlanFullBillingInfo\n" + exception + "\n");
        }
    }
    
    [Category("PaymentFormV4Test")]
    [Test(Description = "CreatePlanWithoutEmailInfo")]
    public async Task CreatePlanWithoutEmailInfo()
    {
        try
        {
            Console.WriteLine("\nStarting CreatePlanWithoutEmailInfo");
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            createPlanDefaultValues.shopper.email = null!;
            var planCreateResponse = await _installmentPlans.CreatePlanInitiateAsync(Environment.GetEnvironmentVariable("ApiV3")!,  _requestHeader!, "Active", 
                new Random().Next(2, 6), Environment.GetEnvironmentVariable("NgMockV2Terminal")!, createPlanDefaultValues);
            Assert.That(planCreateResponse.Status, Is.EqualTo("Initialized"));
            Assert.That(_paymentFormV4Functionality.ValidatePf4Fields(planCreateResponse, null, null, "yes"));
            Console.WriteLine("CreatePlanWithoutEmailInfo is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in CreatePlanWithoutEmailInfo\n" + exception + "\n");
        }
    }
    
    [Category("PaymentFormV4Test")]
    [Test(Description = "CreatePlanWithoutBillingObject")]
    public async Task CreatePlanWithoutBillingObject()
    {
        try
        {
            Console.WriteLine("\nStarting CreatePlanWithoutBillingObject");
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            createPlanDefaultValues.billingAddress = null!;
            var planCreateResponse = await _installmentPlans.CreatePlanInitiateAsync(Environment.GetEnvironmentVariable("ApiV3")!,  _requestHeader!, "Active", 
                new Random().Next(2, 6), Environment.GetEnvironmentVariable("NgMockV2Terminal")!, createPlanDefaultValues);
            Assert.That(planCreateResponse.Status, Is.EqualTo("Initialized"));
            Assert.That(_paymentFormV4Functionality.ValidatePf4Fields(planCreateResponse, "yes"));
            Console.WriteLine("CreatePlanWithoutBillingObject is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in CreatePlanWithoutBillingObject\n" + exception + "\n");
        }
    }
    
    [Category("PaymentFormV4Test")]
    [Test(Description = "ValidateBillingMandatoryFields")]
    public async Task ValidateBillingMandatoryFields()
    {
        try
        {
            Console.WriteLine("\nStarting ValidateMandatoryFields");
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            createPlanDefaultValues.billingAddress = null!;
            var planCreateResponse = await _installmentPlans.CreatePlanInitiateAsync(Environment.GetEnvironmentVariable("ApiV3")!,  _requestHeader!, "Active", 
                new Random().Next(2, 6), Environment.GetEnvironmentVariable("NgMockV2Terminal")!, createPlanDefaultValues);
            Assert.That(planCreateResponse.Status, Is.EqualTo("Initialized"));
            Assert.That(_paymentFormV4Functionality.ValidateMandatoryFields(planCreateResponse,  _paymentFormV4.AddressLine, 
                _paymentFormV4.City, _paymentFormV4.ErrorBillingInfoMessage));
            Console.WriteLine("ValidateBillingMandatoryFields is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in ValidateBillingMandatoryFields\n" + exception + "\n");
        }
    }
    
    [Category("PaymentFormV4Test")]
    [Test(Description = "ValidateCardHolderNameMandatoryFields")]
    public async Task ValidateCardHolderNameMandatoryFields()
    {
        try
        {
            Console.WriteLine("\nStarting ValidateCardHolderNameMandatoryFields");
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            var planCreateResponse = await _installmentPlans.CreatePlanInitiateAsync(Environment.GetEnvironmentVariable("ApiV3")!,  _requestHeader!, "Active", 
                new Random().Next(2, 6), Environment.GetEnvironmentVariable("NgMockV2Terminal")!, createPlanDefaultValues);
            Assert.That(planCreateResponse.Status, Is.EqualTo("Initialized"));
            Assert.That(_paymentFormV4Functionality.ValidateMandatoryFields(planCreateResponse, _paymentFormV4.CardNumber, 
                _paymentFormV4.Cvv, _paymentFormV4.ErrorPaymentsMessage));
            Console.WriteLine("ValidateCardHolderNameMandatoryFields is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in ValidateCardHolderNameMandatoryFields\n" + exception + "\n");
        }
    }
    
    [Category("PaymentFormV4Test")]
    [Test(Description = "ValidateCvvMandatoryFields")]
    public async Task ValidateCvvMandatoryFields()
    {
        try
        {
            Console.WriteLine("\nStarting ValidateCvvMandatoryFields");
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            var planCreateResponse = await _installmentPlans.CreatePlanInitiateAsync(Environment.GetEnvironmentVariable("ApiV3")!,  _requestHeader!, "Active", 
                new Random().Next(2, 6), Environment.GetEnvironmentVariable("NgMockV2Terminal")!, createPlanDefaultValues);
            Assert.That(planCreateResponse.Status, Is.EqualTo("Initialized"));
            Assert.That(_paymentFormV4Functionality.ValidateMandatoryFields(planCreateResponse, _paymentFormV4.Cvv, 
                _paymentFormV4.CardNumber, _paymentFormV4.CvvErrorMessage));
            Console.WriteLine("ValidateCvvMandatoryFields is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in ValidateCvvMandatoryFields\n" + exception + "\n");
        }
    }
    
    [Category("PaymentFormV4Test")]
    [Test(Description = "ValidateExpMandatoryFields")]
    public async Task ValidateExpMandatoryFields()
    {
        try
        {
            Console.WriteLine("\nStarting ValidateExpMandatoryFields");
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            var planCreateResponse = await _installmentPlans.CreatePlanInitiateAsync(Environment.GetEnvironmentVariable("ApiV3")!,  _requestHeader!, "Active", 
                new Random().Next(2, 6), Environment.GetEnvironmentVariable("NgMockV2Terminal")!, createPlanDefaultValues);
            Assert.That(planCreateResponse.Status, Is.EqualTo("Initialized"));
            Assert.That(_paymentFormV4Functionality.ValidateMandatoryFields(planCreateResponse, _paymentFormV4.Exp, 
                _paymentFormV4.CardNumber, _paymentFormV4.ErrorExpMessage));
            Console.WriteLine("ValidateExpMandatoryFields is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in ValidateExpMandatoryFields\n" + exception + "\n");
        }
    }
    
    [Category("PaymentFormV4Test")]
    [Test(Description = "ValidateTermsAndConditions")]
    public async Task ValidateTermsAndConditions()
    {
        try
        {
            Console.WriteLine("\nStarting ValidateTermsAndConditions");
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            var planCreateResponse = await _installmentPlans.CreatePlanInitiateAsync(Environment.GetEnvironmentVariable("ApiV3")!,  _requestHeader!, "Active", 
                new Random().Next(2, 6), Environment.GetEnvironmentVariable("NgMockV2Terminal")!, createPlanDefaultValues);
            Assert.That(planCreateResponse.Status, Is.EqualTo("Initialized"));
            Assert.That(_paymentFormV4Functionality.ValidateLinksFlow(planCreateResponse, _paymentFormV4.TermAndConditionsLink, _paymentFormV4.TermAndConditionsIframe), Is.True);
            Console.WriteLine("ValidateTermsAndConditions is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in ValidateTermsAndConditions\n" + exception + "\n");
        }
    }
    
    [Category("PaymentFormV4Test")]
    [Test(Description = "ValidatePrivacyPolicy")]
    public async Task ValidatePrivacyPolicy()
    {
        try
        {
            Console.WriteLine("\nStarting ValidatePrivacyPolicy");
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            var planCreateResponse = await _installmentPlans.CreatePlanInitiateAsync(Environment.GetEnvironmentVariable("ApiV3")!,  _requestHeader!, "Active", 
                new Random().Next(2, 6), Environment.GetEnvironmentVariable("NgMockV2Terminal")!, createPlanDefaultValues);
            Assert.That(planCreateResponse.Status, Is.EqualTo("Initialized"));
            Assert.That(_paymentFormV4Functionality.ValidateLinksFlow(planCreateResponse, _paymentFormV4.PrivacyPolicyLink, _paymentFormV4.PrivacyPolicyIframe), Is.True);
            Console.WriteLine("ValidatePrivacyPolicy is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in ValidatePrivacyPolicy\n" + exception + "\n");
        }
    }
    
    [Category("PaymentFormV4Test")]
    [Test(Description = "CreatePlanWithAuthError")]
    public async Task CreatePlanWithAuthError()
    {
        try
        {
            Console.WriteLine("\nStarting CreatePlanWithAuthError");
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            createPlanDefaultValues.billingAddress.addressLine1 = "Simulate failure MIT";
            var planCreateResponse = await _installmentPlans.CreatePlanInitiateAsync(Environment.GetEnvironmentVariable("ApiV3")!,  _requestHeader!, "Active", 
                1, Environment.GetEnvironmentVariable("NgMockV2Terminal")!, createPlanDefaultValues);
            Assert.That(planCreateResponse.Status, Is.EqualTo("Initialized"));
            Assert.That(await _doTheChallenge.DoTheChallengeV4Pf4Async(planCreateResponse, createPlanDefaultValues, 
                new Random().Next(2, 6), null!,null,
                 _requestHeader!), Is.True);
            var jResponsePgtl = await _getPgtl.ValidatePgtlKeyValueInnerAsync(_envConfig.StoreProcedureUrl,
                _requestHeader!, planCreateResponse.InstallmentPlanNumber, "Type", "Authorize", "TransactionId");
            Assert.That(jResponsePgtl!.Contains("Authorize"));
            Console.WriteLine("CreatePlanWithAuthError is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in CreatePlanWithAuthError \n" + exception + "\n");
        }
    }  
    
    [Category("PaymentFormV4Test")]
    [Test(Description = "CreatePlanWith3DsWithAuthError")]
    public async Task CreatePlanWith3DsWithAuthError()
    {
        try
        {
            Console.WriteLine("\nStarting CreatePlanWith3DsWithAuthError");
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "yes");
            createPlanDefaultValues.billingAddress.addressLine1 = "Simulate failure MIT";
            var planCreateResponse = await _installmentPlans.CreatePlanInitiateAsync(Environment.GetEnvironmentVariable("ApiV3")!,  _requestHeader!, "Active", 
                1, Environment.GetEnvironmentVariable("NgMockV2Terminal")!, createPlanDefaultValues);
            Assert.That(planCreateResponse.Status, Is.EqualTo("Initialized"));
            Assert.That(await _doTheChallenge.DoTheChallengeV4Pf4Async(planCreateResponse, createPlanDefaultValues, 
                new Random().Next(2, 6), null!,null,
                 _requestHeader!), Is.True);
            var jResponsePgtl = await _getPgtl.ValidatePgtlKeyValueInnerAsync(_envConfig.StoreProcedureUrl,
                _requestHeader!, planCreateResponse.InstallmentPlanNumber, "Type", "Authorize", "TransactionId");
            Assert.That(jResponsePgtl!.Contains("Authorize"));
            Console.WriteLine("CreatePlanWith3DsWithAuthError is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in CreatePlanWith3DsWithAuthError \n" + exception + "\n");
        }
    }  
}