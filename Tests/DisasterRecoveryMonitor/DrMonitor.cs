using Allure.NUnit;
using Allure.NUnit.Attributes;
using NUnit.Framework;
using Splitit.Automation.NG.Backend.BaseActions;
using Splitit.Automation.NG.Backend.Services.PlanCreate.Functionality;
using Splitit.Automation.NG.Backend.Services.V3.DefaultValues;
using Splitit.Automation.NG.Backend.Services.V3.InstallmentPlan.InstallmentPlanApiEndPoint;
using Splitit.Automation.NG.Backend.Tests.TestsSetups;
using FullPlanInfoIpn = Splitit.Automation.NG.Backend.Services.AdminApi.InstallmentPlan.InstallmentsPlanFunctionality.FullPlanInfoIpn;

namespace Splitit.Automation.NG.Backend.Tests.DisasterRecoveryMonitor;

[TestFixture]
[AllureNUnit]
[AllureSuite("DisasterRecoveryMonitorTests")]
[AllureDisplayIgnored]
[Parallelizable(ParallelScope.All)]
public class DrMonitor
{
    private RequestHeader? _requestHeader;
    private readonly InstallmentPlans _installmentPlans;
    private readonly FullPlanInfoIpn _fullPlanInfoIpn;
    private readonly DispatchDrPlanFunctionality _dispatchDrPlanFunctionality;

    public DrMonitor()
    {
        Console.WriteLine("\nStaring DrMonitor Setup");
        _installmentPlans = new InstallmentPlans();
        _fullPlanInfoIpn = new FullPlanInfoIpn();
        _dispatchDrPlanFunctionality = new DispatchDrPlanFunctionality();
        Console.WriteLine("DrMonitor Setup Succeeded\n");
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
    
    [Category("DrTests")]
    [Test(Description = "TestValidateCreatePlanInDrAndValidateInAdminApi"), CancelAfter(240*1000)]
    public async Task TestValidateCreatePlanInDrAndValidateInAdminApi()
    {
        try
        {
            Console.WriteLine("\nStarting TestValidateCreatePlanInDrAndValidateInAdminApi");
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            var jResponsePlanCreate = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3_Dr")!,  _requestHeader!,
                "Active", new Random().Next(2, 6), Environment.GetEnvironmentVariable("NgMockV2Terminal")!,
                createPlanDefaultValues);
            var jResponseDispatch = await _dispatchDrPlanFunctionality.SendPostRequestDispatchDrPlanAsync( 
                _requestHeader!, "100", "50");
            Assert.That(jResponseDispatch.StatusCode == 200);
            Assert.That(jResponseDispatch.IsSuccess);
            Assert.That(jResponseDispatch.Errors, Is.Null);
            var jResponseFullPlanInfoIpn = await _fullPlanInfoIpn.SendGetFullPlanInfoIpnAsync( _requestHeader!,
                jResponsePlanCreate.InstallmentPlanNumber);
            Console.WriteLine("Validating Plan status");
            if (!jResponseFullPlanInfoIpn.InstallmentPlan.InstallmentPlanStatus.Code.Equals("Initializing") &&
                !jResponseFullPlanInfoIpn.InstallmentPlan.InstallmentPlanStatus.Code.Equals("InProgress"))
            {
                Assert.Fail("Plan Status is not in the right state");
            }
            Console.WriteLine("Plan status Validated");
            Console.WriteLine("Validating Plan Amount");
            Assert.That(jResponseFullPlanInfoIpn.InstallmentPlan.Amount.Value.Equals(jResponsePlanCreate.Amount));
            Console.WriteLine("Plan Amount Validated");
            Console.WriteLine("Validating Plan Installments count");
            Assert.That(jResponseFullPlanInfoIpn.InstallmentPlan.Installments.Count == jResponsePlanCreate.Installments.Count);
            Console.WriteLine("Plan Installments count validated");
            Console.WriteLine("TestValidateCreatePlanInDrAndValidateInAdminApi is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidateCreatePlanInDrAndValidateInAdminApi\n" + exception + "\n");
        }
    }
    
    [Category("DrTests")]
    [Test(Description = "TestValidateOmsStuckCreatePlanNo3dsStandard"), CancelAfter(240*1000)]
    public async Task TestValidateOmsStuckCreatePlanNo3dsStandard()
    {
        try
        {
            Console.WriteLine("\nStarting TestValidateOmsStuckCreatePlanNo3dsStandard");
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            createPlanDefaultValues.planData.refOrderNumber = "SIMULATE_DO_NOT_PROCESS";
            var jResponsePlanCreate = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3_Dr")!,  _requestHeader!,
                "Active", new Random().Next(2, 6), Environment.GetEnvironmentVariable("NgMockV2Terminal")!,
                createPlanDefaultValues);
            var jResponseDispatch = await _dispatchDrPlanFunctionality.SendPostRequestDispatchDrPlanAsync( _requestHeader!, "100", "50");
            Assert.That(jResponseDispatch.StatusCode == 200);
            Assert.That(jResponseDispatch.IsSuccess);
            Assert.That(jResponseDispatch.Errors, Is.Null);
            var jResponseFullPlanInfoIpn = await _fullPlanInfoIpn.SendGetFullPlanInfoIpnAsync( _requestHeader!, 
                jResponsePlanCreate.InstallmentPlanNumber, "yes");
            Assert.That(jResponseFullPlanInfoIpn.ResponseHeader.Succeeded);
            Console.WriteLine("TestValidateOmsStuckCreatePlanNo3dsStandard is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidateOmsStuckCreatePlanNo3dsStandard\n" + exception + "\n");
        }
    }
    
    [Category("DrTests")]
    [Test(Description = "TestValidateSuccessInitiatePlanNo3dsStandard"), CancelAfter(240*1000)]
    public async Task TestValidateSuccessInitiatePlanNo3dsStandard()
    {
        try
        {
            Console.WriteLine("\nStarting TestValidateSuccessInitiatePlanNo3dsStandard");
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            var jResponsePlanCreate = await _installmentPlans.CreatePlanInitiateAsync(Environment.GetEnvironmentVariable("ApiV3_Dr")!,  _requestHeader!,
                "Active", new Random().Next(2, 6), Environment.GetEnvironmentVariable("NgMockV2Terminal")!,
                createPlanDefaultValues);
            Assert.That(jResponsePlanCreate.Status.Equals("Initialized"));
            var jResponseDispatch = await _dispatchDrPlanFunctionality.SendPostRequestDispatchDrPlanAsync( _requestHeader!, "100", "50");
            Assert.That(jResponseDispatch.StatusCode == 200);
            Assert.That(jResponseDispatch.IsSuccess);
            Assert.That(jResponseDispatch.Errors, Is.Null);
            var jResponseFullPlanInfoIpn = await _fullPlanInfoIpn.SendGetFullPlanInfoIpnAsync( _requestHeader!, 
                jResponsePlanCreate.InstallmentPlanNumber, "yes");
            Assert.That(jResponseFullPlanInfoIpn.ResponseHeader.Succeeded);
            Console.WriteLine("TestValidateSuccessInitiatePlanNo3dsStandard is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidateSuccessInitiatePlanNo3dsStandard\n" + exception + "\n");
        }
    }
}