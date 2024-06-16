using Allure.NUnit;
using Allure.NUnit.Attributes;
using NUnit.Framework;
using Splitit.Automation.NG.Backend.BaseActions;
using Splitit.Automation.NG.Backend.Services.AdminApi.InstallmentPlan.InstallmentsPlanFunctionality;
using Splitit.Automation.NG.Backend.Services.V1.DefaultValues;
using Splitit.Automation.NG.Backend.Services.V1.InstallmentPlan.InstallmentPlanApiEndPoints;
using Splitit.Automation.NG.Backend.Services.V1.Login.LoginApiEndPoints;
using Splitit.Automation.NG.Backend.Services.V3.DefaultValues;
using Splitit.Automation.NG.Backend.Tests.TestsSetups;
using Splitit.Automation.NG.SharedResources.BaseActions;
using Splitit.Automation.NG.Utilities.EnvironmentsConfig;

namespace Splitit.Automation.NG.SharedResources.ProductionMonitor;

[TestFixture]
[AllureNUnit]
[AllureSuite("ProductionMonitor")]
[AllureDisplayIgnored]
public class MonitorProduction
{
    private readonly Login _login;
    private readonly Initiate _initiate;
    private RequestHeader? _requestHeader;
    private readonly DoTheChallenge _doTheChallenge;
    private readonly FullPlanInfoIpn _fullPlanInfoIpn;
    private readonly SendAdminLoginRequest _sendAdminLoginRequest;
    private readonly DriverFactory.DriverFactory _driverFactory;
    private readonly FlexFieldsFunctionality _flexFieldsFunctionality;
    private readonly EnvConfig _envConfig;

    public MonitorProduction()
    {
        Console.WriteLine("\nStarting Monitor Production Setup");
        _login = new Login();
        _initiate = new Initiate();
        _doTheChallenge = new DoTheChallenge();
        _fullPlanInfoIpn = new FullPlanInfoIpn();
        _sendAdminLoginRequest = new SendAdminLoginRequest();
        _driverFactory = new DriverFactory.DriverFactory();
        _flexFieldsFunctionality = new FlexFieldsFunctionality();
        _envConfig = new EnvConfig();
        Console.WriteLine("Monitor Production Setup Succeeded\n");
    }
    
    [OneTimeSetUp]
    public void InitSetUp()
    {
        var testsSetup = new TestsSetup();
        testsSetup.Setup();
    }

    [Category("ProductionMonitor")]
    [Test(Description = "CreatePlanWithPf4")]
    public async Task CreatePlanWithPf4()
    {
        try
        {
            Console.WriteLine("\nStarting CreatePlanWithPf4");
            _requestHeader = await _login.DoLoginV1( _requestHeader!, _envConfig.UserAutomationTestUserName,
                _envConfig.UserAutomationTestPasswordProduction, _envConfig.SplititMockTerminalProduction) ?? throw new InvalidOperationException();
            var v1InitiateDefaultValues = new V1InitiateDefaultValues
            {
                RequestHeader =
                {
                    apiKey = _requestHeader.apiKey,
                    sessionId = _requestHeader.sessionId
                }
            };
            var planCreateResponse = await _initiate.CreatePlanInitiate(_envConfig.BaseUrlProduction, _requestHeader, 
                new Random().Next(2, 6), v1InitiateDefaultValues);
            Assert.That(planCreateResponse!.InstallmentPlan.InstallmentPlanStatus.Description, Is.EqualTo("Initializing"));
            Assert.That(await _doTheChallenge.DoTheChallengeV4ForV1Async(planCreateResponse, v1InitiateDefaultValues), Is.True);
            _requestHeader = await _sendAdminLoginRequest.DoAdminLogin(
                Environment.GetEnvironmentVariable("AccessTokenURI")!,
                Environment.GetEnvironmentVariable("ClientSecret")!,
                Environment.GetEnvironmentVariable("SplititMockTerminal")!,
                Environment.GetEnvironmentVariable("clientId")!);
            var jResponse = await _fullPlanInfoIpn.SendGetFullPlanInfoIpnAsync( _requestHeader, planCreateResponse.InstallmentPlan.InstallmentPlanNumber);
            Assert.That(jResponse.InstallmentPlan.InstallmentPlanStatus.Code, Is.EqualTo("InProgress"));
            Console.WriteLine("CreatePlanWithPf4 is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in CreatePlanWithPf4\n" + exception + "\n");
        }
    }
    
    [Category("ProductionMonitor")]
    [Test(Description = "CreatePlanWithPf4With3Ds")]
    public async Task CreatePlanWithPf4With3Ds()
    {
        try
        {
            Console.WriteLine("\nStarting CreatePlanWithPf4With3Ds");
            _requestHeader = await _login.DoLoginV1( _requestHeader!, _envConfig.UserAutomationTestUserNameProduction,
                                 _envConfig.UserAutomationTestPassword, _envConfig.SplititMockTerminalProduction)
                             ?? throw new InvalidOperationException();
            var v1InitiateDefaultValues = new V1InitiateDefaultValues
            {
                RequestHeader =
                {
                    apiKey = _requestHeader.apiKey,
                    sessionId = _requestHeader.sessionId
                },
                planData =
                {
                    Attempt3DSecure = true
                }
            };
            var planCreateResponse = await _initiate.CreatePlanInitiate(_envConfig.BaseUrlProduction, _requestHeader, 
                new Random().Next(2, 6), v1InitiateDefaultValues);
            Assert.That(planCreateResponse!.InstallmentPlan.InstallmentPlanStatus.Description, Is.EqualTo("Initializing"));
            Assert.That(await _doTheChallenge.DoTheChallengeV4ForV1Async(planCreateResponse, v1InitiateDefaultValues), Is.True);
            _requestHeader = await _sendAdminLoginRequest.DoAdminLogin(
                Environment.GetEnvironmentVariable("AccessTokenURI")!,
                Environment.GetEnvironmentVariable("ClientSecret")!,
                Environment.GetEnvironmentVariable("SplititMockTerminal")!,
                Environment.GetEnvironmentVariable("clientId")!);
            var jResponse = await _fullPlanInfoIpn.SendGetFullPlanInfoIpnAsync( _requestHeader, planCreateResponse.InstallmentPlan.InstallmentPlanNumber);
            Assert.That(jResponse.InstallmentPlan.InstallmentPlanStatus.Code, Is.EqualTo("InProgress"));
            Console.WriteLine("CreatePlanWithPf4With3Ds is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in CreatePlanWithPf4With3Ds \n" + exception + "\n");
        }
    }

    [Category("ProductionMonitor")]
    [Test(Description = "ValidateFlexFieldsV2")]
    public async Task ValidateFlexFieldsV2()
    {
        var driver = _driverFactory.InitDriver();
        try
        {
            Console.WriteLine("Starting ValidateFlexFieldsV2");
            await _flexFieldsFunctionality.SetFlexFieldsConfig(driver);
            await _flexFieldsFunctionality.FillFlexFieldsFields(driver);
            Console.WriteLine("Done with ValidateFlexFieldsV2");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in ValidateFlexFieldsV2 \n" + exception + "\n");
        }
    }

    [Category("ProductionMonitor")]
    [Test(Description = "ValidatePf35")]
    public async Task ValidatePf35()
    {
        try
        {
            Console.WriteLine("\nStarting ValidatePf35");
            _requestHeader = await _login.DoLoginV1( _requestHeader!, _envConfig.UserAutomationTestUserNameV35Production,
                _envConfig.UserAutomationTestPasswordV35Production, _envConfig.V35MerchantTerminalProduction)
                                                        ?? throw new InvalidOperationException();
            var v1InitiateDefaultValues = new V1InitiateDefaultValues
            {
                RequestHeader =
                {
                    apiKey = _requestHeader.apiKey,
                    sessionId = _requestHeader.sessionId
                }
            };
            var planCreateResponse = await _initiate.CreatePlanInitiate(_envConfig.BaseUrlProduction, _requestHeader, 
                new Random().Next(2, 5), v1InitiateDefaultValues);
            Assert.That(planCreateResponse!.InstallmentPlan.InstallmentPlanStatus.Description, Is.EqualTo("Initializing"));
            Assert.That(await _doTheChallenge.DoTheChallengeV35Async(planCreateResponse.CheckoutUrl, _envConfig.Card4111Production,
                _envConfig.Card4111ExpProduction, _envConfig.Card4111CvvProduction, null!, 
                _requestHeader, planCreateResponse.InstallmentPlan.InstallmentPlanNumber), Is.True);
            _requestHeader = await _sendAdminLoginRequest.DoAdminLogin(
                Environment.GetEnvironmentVariable("AccessTokenURI")!,
                Environment.GetEnvironmentVariable("ClientSecret")!,
                Environment.GetEnvironmentVariable("SplititMockTerminal")!,
                Environment.GetEnvironmentVariable("clientId")!);
            var jResponse = await _fullPlanInfoIpn.SendGetFullPlanInfoIpnAsync( _requestHeader, planCreateResponse.InstallmentPlan.InstallmentPlanNumber);
            Console.WriteLine("Expected status code id InProgress , actual result is -> " + jResponse.InstallmentPlan.InstallmentPlanStatus.Code);
            Assert.That(jResponse.InstallmentPlan.InstallmentPlanStatus.Code, Is.EqualTo("InProgress"));
            Console.WriteLine("Done with ValidatePf35\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in ValidatePf35\n" + exception + "\n");
        }
    }
    
    [Category("ProductionMonitor")]
    [Test(Description = "ValidatePf35With3Ds")]
    public async Task ValidatePf35With3Ds()
    {
        try
        {
            Console.WriteLine("\nStarting ValidatePf35With3Ds");
            _requestHeader = await _login.DoLoginV1( _requestHeader!, _envConfig.UserAutomationTestUserNameV35Production,
                _envConfig.UserAutomationTestPasswordV35Production, _envConfig.V35MerchantTerminalProduction)
                                                        ?? throw new InvalidOperationException();
            var v1InitiateDefaultValues = new V1InitiateDefaultValues
            {
                RequestHeader =
                {
                    apiKey = _requestHeader.apiKey,
                    sessionId = _requestHeader.sessionId
                },
                planData =
                {
                    Attempt3DSecure = true
                }
            };
            var planCreateResponse = await _initiate.CreatePlanInitiate(_envConfig.BaseUrlProduction, _requestHeader, 
                new Random().Next(2, 5), v1InitiateDefaultValues);
            Assert.That(planCreateResponse!.InstallmentPlan.InstallmentPlanStatus.Description, Is.EqualTo("Initializing"));
            Assert.That(await _doTheChallenge.DoTheChallengeV35Async(planCreateResponse.CheckoutUrl, _envConfig.Card4111Production,
                _envConfig.Card4111ExpProduction, _envConfig.Card4111CvvProduction, "yes", 
                _requestHeader, planCreateResponse.InstallmentPlan.InstallmentPlanNumber), Is.True);
            _requestHeader = await _sendAdminLoginRequest.DoAdminLogin(
                Environment.GetEnvironmentVariable("AccessTokenURI")!,
                Environment.GetEnvironmentVariable("ClientSecret")!,
                Environment.GetEnvironmentVariable("SplititMockTerminal")!,
                Environment.GetEnvironmentVariable("clientId")!);
            var jResponse = await _fullPlanInfoIpn.SendGetFullPlanInfoIpnAsync( _requestHeader, planCreateResponse.InstallmentPlan.InstallmentPlanNumber);
            Console.WriteLine("Expected status code id InProgress , actual result is -> " + jResponse.InstallmentPlan.InstallmentPlanStatus.Code);
            Assert.That(jResponse.InstallmentPlan.InstallmentPlanStatus.Code, Is.EqualTo("InProgress"));
            Console.WriteLine("Done with ValidatePf35With3Ds\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in ValidatePf35With3Ds\n" + exception + "\n");
        }
    }
}
