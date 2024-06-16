using MongoDB.Driver;
using Allure.NUnit;
using Allure.NUnit.Attributes;
using NUnit.Framework;
using Splitit.Automation.NG.Backend.BaseActions;
using Splitit.Automation.NG.Backend.Services.V3.DefaultValues;
using Splitit.Automation.NG.Backend.Services.V3.InstallmentPlan.InstallmentPlanApiEndPoint;
using Splitit.Automation.NG.Backend.Tests.TestsSetups;
using Splitit.Automation.NG.Utilities.EnvironmentsConfig;

namespace Splitit.Automation.NG.PaymentsSystem.Sku.FundNonSecuredPlans.Tests;

[TestFixture]
[AllureNUnit]
[AllureSuite("FundNonSecuredPlansTestsSuite")]
[AllureDisplayIgnored]
[Parallelizable(ParallelScope.All)]
public class FundNonSecuredPlans
{
    private RequestHeader? _requestHeader;
    private readonly InstallmentPlans _installmentPlans;
    private readonly MongoHandler _mongoHandler;
    private IMongoDatabase? _db;
    private string? _terminalApiKey;
    private readonly EnvConfig _envConfig;
    
    public FundNonSecuredPlans()
    {
        Console.WriteLine("Staring FundNonSecuredPlansTests Setup");
        _installmentPlans = new InstallmentPlans();
        _mongoHandler = new MongoHandler();
        _envConfig = new EnvConfig();
        Console.WriteLine("FundNonSecuredPlansTests Setup Succeeded\n");
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
        _db = _mongoHandler.MongoConnect(_envConfig.MongoConnection, "Splitit_Payments");
    }
    
    [TestCase(Category = "FundNonSecuredPlansTests")]
    [TestCase(Category = "PaymentSystemSmoke")]
    [Test(Description = "TestValidate_MER_UNF_STL_MON_FTR_CRC_U03 with generated merchant"), CancelAfter(120*1000)]
    public async Task TestValidate_MER_UNF_STL_MON_FTR_CRC_U03()
    {
        try
        {
            Console.WriteLine("\nStarting TestValidate_MER_UNF_STL_MON_FTR_CRC_U03");
            _terminalApiKey = _envConfig.MER_UNF_STL_MON_FTR_CRC_U03_FundNonSecured;
            Console.WriteLine("Done with creation of a new merchant , settings and terminal process");
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            var planCreateResponse = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!,_requestHeader!,
                "PendingCapture", 3, _terminalApiKey, createPlanDefaultValues);
            var (activityStatusCol1Before, idStatusCol1Before) = await _mongoHandler.SendMongoQueryWith2KeysAsync(_db!,
                "Payment_Records",
                "InstallmentPlanNumber",
                planCreateResponse.InstallmentPlanNumber, "Activity", "COL", "Status", "_id");
            if (activityStatusCol1Before.Equals("New") || activityStatusCol1Before.Equals("PendingBatch"))
            {
                Console.WriteLine("activityStatusCol is in the right state");
            }
            else
            {
                Assert.Fail("activityStatusCol is not in the right state");
            }
            Assert.That(idStatusCol1Before, Is.Not.Null);
            Console.WriteLine("Done TestValidate_MER_UNF_STL_MON_FTR_CRC_U03");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidate_MER_UNF_STL_MON_FTR_CRC_U03 \n" + exception + "\n");
        }
    }
    
    [TestCase(Category = "FundNonSecuredPlansTests")]
    [Test(Description = "TestValidate_MER_FUN_STL_MON_FTR_CRC_F09 with generated merchant"), CancelAfter(120*1000)]
    public async Task TestValidate_MER_FUN_STL_MON_FTR_CRC_F09()
    {
        try
        {
            Console.WriteLine("\nStarting TestValidate_MER_FUN_STL_MON_FTR_CRC_F09");
            _terminalApiKey = _envConfig.MER_FUN_STL_MON_FTR_CRC_F09_FundNonSecured;
            Console.WriteLine("Done with creation of a new merchant , settings and terminal process");
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            var planCreateResponse = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!, _requestHeader!,"PendingCapture", 
                9, _terminalApiKey, createPlanDefaultValues);
            var (activityStatusCol1Before, idStatusCol1Before) = await _mongoHandler.SendMongoQueryWith2KeysAsync(_db!,
                "Payment_Records",
                "InstallmentPlanNumber",
                planCreateResponse.InstallmentPlanNumber, "Activity", "COL", "Status", "_id");
            if (activityStatusCol1Before.Equals("New") || activityStatusCol1Before.Equals("PendingBatch"))
            {
                Console.WriteLine("activityStatusCol is in the right state");
            }
            else
            {
                Assert.Fail("activityStatusCol is not in the right state");
            }
            Assert.That(idStatusCol1Before, Is.Not.Null);
            var (activityStatusFnd, idStatusFnd) = await _mongoHandler.SendMongoQueryWith2KeysAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber",
                planCreateResponse.InstallmentPlanNumber, "Activity", "FND", "Status", "_id");
            if (activityStatusFnd.Equals("New") || activityStatusFnd.Equals("PendingBatch"))
            {
                Console.WriteLine("activityStatusFnd is in the right state");
            }
            else
            {
                Assert.Fail("activityStatusFnd is not in the right state");
            }
            Assert.That(idStatusFnd, Is.Not.Null);
            Console.WriteLine("Done TestValidate_MER_FUN_STL_MON_FTR_CRC_F09");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidate_MER_FUN_STL_MON_FTR_CRC_F09 \n" + exception + "\n");
        }
    }
    
    [TestCase(Category = "FundNonSecuredPlansTests")]
    [Test(Description = "TestValidate_MER_FUN_STL_MON_FTR_CRC_F06 with generated merchant"), CancelAfter(120*1000)]
    public async Task TestValidate_MER_FUN_STL_MON_FTR_CRC_F06()
    {
        try
        {
            Console.WriteLine("\nStarting TestValidate_MER_FUN_STL_MON_FTR_CRC_F06");
            _terminalApiKey = _envConfig.MER_FUN_STL_MON_FTR_CRC_F06_FundNonSecured;
            Console.WriteLine("Done with creation of a new merchant , settings and terminal process");
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            var planCreateResponse = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!, _requestHeader!,"PendingCapture", 
                6, _terminalApiKey, createPlanDefaultValues);
            
            var (activityStatusCol1Before, idStatusCol1Before) = await _mongoHandler.SendMongoQueryWith2KeysAsync(_db!,
                "Payment_Records",
                "InstallmentPlanNumber",
                planCreateResponse.InstallmentPlanNumber, "Activity", "COL", "Status", "_id");
            if (activityStatusCol1Before.Equals("New") || activityStatusCol1Before.Equals("PendingBatch"))
            {
                Console.WriteLine("activityStatusCol is in the right state");
            }
            else
            {
                Assert.Fail("activityStatusCol is not in the right state");
            }
            Assert.That(idStatusCol1Before, Is.Not.Null);
            var (activityStatusFnd, idStatusFnd) = await _mongoHandler.SendMongoQueryWith2KeysAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber",
                planCreateResponse.InstallmentPlanNumber, "Activity", "FND", "Status", "_id");
            if (activityStatusFnd.Equals("New") || activityStatusFnd.Equals("PendingBatch"))
            {
                Console.WriteLine("activityStatusFnd is in the right state");
            }
            else
            {
                Assert.Fail("activityStatusFnd is not in the right state");
            }
            Assert.That(idStatusFnd, Is.Not.Null);
            Console.WriteLine("Done TestValidate_MER_FUN_STL_MON_FTR_CRC_F06");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidate_MER_FUN_STL_MON_FTR_CRC_F06 \n" + exception + "\n");
        }
    }
    
    [TestCase(Category = "FundNonSecuredPlansTests")]
    [Test(Description = "TestValidate_MER_FUN_STL_MON_FTR_CRC_F12 with generated merchant"), CancelAfter(120*1000)]
    public async Task TestValidate_MER_FUN_STL_MON_FTR_CRC_F12()
    {
        try
        {
            Console.WriteLine("\nStarting TestValidate_MER_FUN_STL_MON_FTR_CRC_F12");
            var terminalApiKey = _envConfig.MER_FUN_STL_MON_FTR_CRC_F12_FundNonSecured;
            Assert.That(_terminalApiKey, Is.Not.EqualTo(""));
            Console.WriteLine("Done with creation of a new merchant , settings and terminal process");
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            var planCreateResponse = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!, _requestHeader!,"PendingCapture", 
                12, terminalApiKey, createPlanDefaultValues);
            var (activityStatusCol1Before, idStatusCol1Before) = await _mongoHandler.SendMongoQueryWith2KeysAsync(_db!,
                "Payment_Records",
                "InstallmentPlanNumber",
                planCreateResponse.InstallmentPlanNumber, "Activity", "COL", "Status", "_id");
            if (activityStatusCol1Before.Equals("New") || activityStatusCol1Before.Equals("PendingBatch"))
            {
                Console.WriteLine("activityStatusCol is in the right state");
            }
            else
            {
                Assert.Fail("activityStatusCol is not in the right state");
            }
            Assert.That(idStatusCol1Before, Is.Not.Null);
            var (activityStatusFnd, idStatusFnd) = await _mongoHandler.SendMongoQueryWith2KeysAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber",
                planCreateResponse.InstallmentPlanNumber, "Activity", "FND", "Status", "_id");
            if (activityStatusFnd.Equals("New") || activityStatusFnd.Equals("PendingBatch"))
            {
                Console.WriteLine("activityStatusFnd is in the right state");
            }
            else
            {
                Assert.Fail("activityStatusFnd is not in the right state");
            }
            Assert.That(idStatusFnd, Is.Not.Null);
            Console.WriteLine("Done TestValidate_MER_FUN_STL_MON_FTR_CRC_F12");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidate_MER_FUN_STL_MON_FTR_CRC_F12 \n" + exception + "\n");
        }
    }
}