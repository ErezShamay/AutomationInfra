using MongoDB.Driver;
using Allure.NUnit;
using Allure.NUnit.Attributes;
using NUnit.Framework;
using Splitit.Automation.NG.Backend.BaseActions;
using Splitit.Automation.NG.Backend.Services.V3.DefaultValues;
using Splitit.Automation.NG.Backend.Services.V3.InstallmentPlan.InstallmentPlanApiEndPoint;
using Splitit.Automation.NG.Backend.Tests.TestsSetups;
using Splitit.Automation.NG.Utilities.EnvironmentsConfig;

namespace Splitit.Automation.NG.PaymentsSystem.Sku.SkuMatching.Tests.SkuFunTests;

[TestFixture]
[AllureNUnit]
[AllureSuite("SkuFunBiwTests")]
[AllureDisplayIgnored]
[Parallelizable(ParallelScope.All)]
public class SkuFunBiwTests
{
    private RequestHeader? _requestHeader;
    private readonly ExcelFileController _excelFileController;
    private readonly SkuTestsData.SkuTestsData _skuTestsData;
    private readonly InstallmentPlans _installmentPlans;
    private Dictionary<string, SkuTestsData.SkuTestsData.PaymentSettings>? _skuDict;
    private readonly MongoHandler _mongoHandler;
    private IMongoDatabase? _db;
    private readonly EnvConfig _envConfig;


    public SkuFunBiwTests()
    {
        Console.WriteLine("Staring SkuFunBiwTests Setup");
        _excelFileController = new ExcelFileController();
        _skuTestsData = new SkuTestsData.SkuTestsData();
        _installmentPlans = new InstallmentPlans();
        _mongoHandler = new MongoHandler();
        _envConfig = new EnvConfig();
        Console.WriteLine("SkuFunBiwTests Setup Succeeded\n");
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
        var csv = _excelFileController.ReadExcelFile("matching");
        (_skuDict, _, _) = _skuTestsData.BuildPaymentSettingsDictionary(csv);
        _db = _mongoHandler.MongoConnect(_envConfig.MongoConnection, "Splitit_Payments");
    }

    [TestCase(Category = "SkuFunBIWTests")]
    [Test(Description = "TestValidate_MER_FUN_STL_BIW_FTR_DRC_F02 with generated merchant"), CancelAfter(120 * 1000)]
    public async Task TestValidate_MER_FUN_STL_BIW_FTR_DRC_F02()
    {
        try
        {
            Console.WriteLine("\nStarting TestValidate_MER_FUN_STL_BIW_FTR_DRC_F02");
            var testName = TestContext.CurrentContext.Test.Name;
            var terminalApiKey = _envConfig.MER_FUN_STL_BIW_FTR;
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            createPlanDefaultValues.paymentMethod.card.cardNumber = Environment.GetEnvironmentVariable("DebitCard")!;
            var planCreateResponse = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!,
                _requestHeader!, "PendingCapture", 2, terminalApiKey, createPlanDefaultValues);
            var skuQueryResult = await _mongoHandler.SendMongoQueryForSkuAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", planCreateResponse.InstallmentPlanNumber, "Sku");
            Assert.That(_skuTestsData.ValidateSkuValue(skuQueryResult, testName.Substring(0, 40)), Is.True);
            var listActivityValues = await _mongoHandler.SendMongoQueryActivityAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber",
                planCreateResponse.InstallmentPlanNumber, "Activity");
            if (listActivityValues.Count is not (2 or 1))
                Assert.Fail("listActivityValues.Count has wrong value");
            Assert.That(
                _mongoHandler.ValidateActivityValueExistence(listActivityValues, _skuDict![testName.Substring(0, 40)]),
                Is.True);
            Console.WriteLine("Done TestValidate_MER_FUN_STL_BIW_FTR_DRC_F02");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidate_MER_FUN_STL_BIW_FTR_DRC_F02 \n" + exception + "\n");
        }
    }

    [TestCase(Category = "SkuFunBIWTests")]
    [Test(Description = "TestValidate_MER_FUN_STL_BIW_FTR_CRC_F02 with generated merchant"), CancelAfter(120 * 1000)]
    public async Task TestValidate_MER_FUN_STL_BIW_FTR_CRC_F02()
    {
        try
        {
            Console.WriteLine("\nStarting TestValidate_MER_FUN_STL_BIW_FTR_CRC_F02");
            var testName = TestContext.CurrentContext.Test.Name;
            var terminalApiKey = _envConfig.MER_FUN_STL_BIW_FTR;
            
            
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            var planCreateResponse = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!, 
                _requestHeader!, "Active", 2, terminalApiKey, createPlanDefaultValues);
            var skuQueryResult = await _mongoHandler.SendMongoQueryForSkuAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", planCreateResponse.InstallmentPlanNumber, "Sku");
            Assert.That(_skuTestsData.ValidateSkuValue(skuQueryResult, testName.Substring(0, 40)), Is.True);
            var listActivityValues = await _mongoHandler.SendMongoQueryActivityAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber",
                planCreateResponse.InstallmentPlanNumber, "Activity");
            if (listActivityValues.Count is not (2 or 1))
                Assert.Fail("listActivityValues.Count has wrong value");
            Assert.That(
                _mongoHandler.ValidateActivityValueExistence(listActivityValues, _skuDict![testName.Substring(0, 40)]),
                Is.True);
            Console.WriteLine("Done TestValidate_MER_FUN_STL_BIW_FTR_CRC_F02");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidate_MER_FUN_STL_BIW_FTR_CRC_F02 \n" + exception + "\n");
        }
    }

    [TestCase(Category = "SkuFunBiwTests")]
    [TestCase(Category = "PaymentSystemSmoke")]
    [Test(Description = "TestValidate_MER_FUN_STL_BIW_FTR_DRC_F03 with generated merchant"), CancelAfter(120 * 1000)]
    public async Task TestValidate_MER_FUN_STL_BIW_FTR_DRC_F03()
    {
        try
        {
            Console.WriteLine("\nStarting TestValidate_MER_FUN_STL_BIW_FTR_DRC_F03");
            var testName = TestContext.CurrentContext.Test.Name;
            var terminalApiKey = _envConfig.MER_FUN_STL_BIW_FTR;
            
            
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            createPlanDefaultValues.paymentMethod.card.cardNumber = Environment.GetEnvironmentVariable("DebitCard")!;
            var planCreateResponse = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!, 
                _requestHeader!, "PendingCapture", 3, terminalApiKey, createPlanDefaultValues);
            var skuQueryResult = await _mongoHandler.SendMongoQueryForSkuAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", planCreateResponse.InstallmentPlanNumber, "Sku");
            Assert.That(_skuTestsData.ValidateSkuValue(skuQueryResult, testName.Substring(0, 40)), Is.True);
            var listActivityValues = await _mongoHandler.SendMongoQueryActivityAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber",
                planCreateResponse.InstallmentPlanNumber, "Activity");
            if (listActivityValues.Count is not (2 or 1))
                Assert.Fail("listActivityValues.Count has wrong value");
            Assert.That(
                _mongoHandler.ValidateActivityValueExistence(listActivityValues, _skuDict![testName.Substring(0, 40)]),
                Is.True);
            Console.WriteLine("Done TestValidate_MER_FUN_STL_BIW_FTR_DRC_F03");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidate_MER_FUN_STL_BIW_FTR_DRC_F03 \n" + exception + "\n");
        }
    }

    [TestCase(Category = "SkuFunBiwTests")]
    [Test(Description = "TestValidate_MER_FUN_STL_BIW_FTR_CRC_F03 with generated merchant"), CancelAfter(120 * 1000)]
    public async Task TestValidate_MER_FUN_STL_BIW_FTR_CRC_F03()
    {
        try
        {
            Console.WriteLine("\nStarting TestValidate_MER_FUN_STL_BIW_FTR_CRC_F03");
            var testName = TestContext.CurrentContext.Test.Name;
            var terminalApiKey = _envConfig.MER_FUN_STL_BIW_FTR;
            
            
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            var planCreateResponse = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!, 
                _requestHeader!, "Active", 3, terminalApiKey, createPlanDefaultValues);
            var skuQueryResult = await _mongoHandler.SendMongoQueryForSkuAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", planCreateResponse.InstallmentPlanNumber, "Sku");
            Assert.That(_skuTestsData.ValidateSkuValue(skuQueryResult, testName.Substring(0, 40)), Is.True);
            var listActivityValues = await _mongoHandler.SendMongoQueryActivityAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber",
                planCreateResponse.InstallmentPlanNumber, "Activity");
            if (listActivityValues.Count is not (2 or 1))
                Assert.Fail("listActivityValues.Count has wrong value");
            Assert.That(
                _mongoHandler.ValidateActivityValueExistence(listActivityValues, _skuDict![testName.Substring(0, 40)]),
                Is.True);
            Console.WriteLine("Done TestValidate_MER_FUN_STL_BIW_FTR_CRC_F03");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidate_MER_FUN_STL_BIW_FTR_CRC_F03 \n" + exception + "\n");
        }
    }

    [TestCase(Category = "SkuFunBIWTests")]
    [Test(Description = "TestValidate_MER_FUN_STL_BIW_FTR_DRC_F04 with generated merchant"), CancelAfter(120 * 1000)]
    public async Task TestValidate_MER_FUN_STL_BIW_FTR_DRC_F04()
    {
        try
        {
            Console.WriteLine("\nStarting TestValidate_MER_FUN_STL_BIW_FTR_DRC_F04");
            var testName = TestContext.CurrentContext.Test.Name;
            var terminalApiKey = _envConfig.MER_FUN_STL_BIW_FTR;
            
            
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            createPlanDefaultValues.paymentMethod.card.cardNumber = Environment.GetEnvironmentVariable("DebitCard")!;
            var planCreateResponse = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!, 
                _requestHeader!, "PendingCapture", 4, terminalApiKey, createPlanDefaultValues);
            var skuQueryResult = await _mongoHandler.SendMongoQueryForSkuAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", planCreateResponse.InstallmentPlanNumber, "Sku");
            Assert.That(_skuTestsData.ValidateSkuValue(skuQueryResult, testName.Substring(0, 40)), Is.True);
            var listActivityValues = await _mongoHandler.SendMongoQueryActivityAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber",
                planCreateResponse.InstallmentPlanNumber, "Activity");
            if (listActivityValues.Count is not (2 or 1))
                Assert.Fail("listActivityValues.Count has wrong value");
            Assert.That(
                _mongoHandler.ValidateActivityValueExistence(listActivityValues, _skuDict![testName.Substring(0, 40)]),
                Is.True);
            Console.WriteLine("Done TestValidate_MER_FUN_STL_BIW_FTR_DRC_F04");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidate_MER_FUN_STL_BIW_FTR_DRC_F04 \n" + exception + "\n");
        }
    }

    [TestCase(Category = "SkuFunBIWTests")]
    [Test(Description = "TestValidate_MER_FUN_STL_BIW_FTR_CRC_F04 with generated merchant"), CancelAfter(120 * 1000)]
    public async Task TestValidate_MER_FUN_STL_BIW_FTR_CRC_F04()
    {
        try
        {
            Console.WriteLine("\nStarting TestValidate_MER_FUN_STL_BIW_FTR_CRC_F04");
            var testName = TestContext.CurrentContext.Test.Name;
            var terminalApiKey = _envConfig.MER_FUN_STL_BIW_FTR;
            
            
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            var planCreateResponse = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!, 
                _requestHeader!, "Active", 4, terminalApiKey, createPlanDefaultValues);
            var skuQueryResult = await _mongoHandler.SendMongoQueryForSkuAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", planCreateResponse.InstallmentPlanNumber, "Sku");
            Assert.That(_skuTestsData.ValidateSkuValue(skuQueryResult, testName.Substring(0, 40)), Is.True);
            var listActivityValues = await _mongoHandler.SendMongoQueryActivityAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber",
                planCreateResponse.InstallmentPlanNumber, "Activity");
            if (listActivityValues.Count is not (2 or 1))
                Assert.Fail("listActivityValues.Count has wrong value");
            Assert.That(
                _mongoHandler.ValidateActivityValueExistence(listActivityValues, _skuDict![testName.Substring(0, 40)]),
                Is.True);
            Console.WriteLine("Done TestValidate_MER_FUN_STL_BIW_FTR_CRC_F04");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidate_MER_FUN_STL_BIW_FTR_CRC_F04 \n" + exception + "\n");
        }
    }
    
    [TestCase(Category = "SkuFunBIWTests")]
    [Test(Description = "TestValidate_MER_FUN_STL_BIW_FTR_CRC_F05 with generated merchant"), CancelAfter(120 * 1000)]
    public async Task TestValidate_MER_FUN_STL_BIW_FTR_CRC_F05()
    {
        try
        {
            Console.WriteLine("\nStarting TestValidate_MER_FUN_STL_BIW_FTR_CRC_F05");
            var testName = TestContext.CurrentContext.Test.Name;
            var terminalApiKey = _envConfig.MER_FUN_STL_BIW_FTR;
            
            
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            var planCreateResponse = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!, 
                _requestHeader!, "Active", 5, terminalApiKey, createPlanDefaultValues);
            var skuQueryResult = await _mongoHandler.SendMongoQueryForSkuAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", planCreateResponse.InstallmentPlanNumber, "Sku");
            Assert.That(_skuTestsData.ValidateSkuValue(skuQueryResult, testName.Substring(0, 40)), Is.True);
            var listActivityValues = await _mongoHandler.SendMongoQueryActivityAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber",
                planCreateResponse.InstallmentPlanNumber, "Activity");
            if (listActivityValues.Count is not (2 or 1))
                Assert.Fail("listActivityValues.Count has wrong value");
            Assert.That(
                _mongoHandler.ValidateActivityValueExistence(listActivityValues, _skuDict![testName.Substring(0, 40)]),
                Is.True);
            Console.WriteLine("Done TestValidate_MER_FUN_STL_BIW_FTR_CRC_F05");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidate_MER_FUN_STL_BIW_FTR_CRC_F05 \n" + exception + "\n");
        }
    }

    [TestCase(Category = "SkuFunBiwTests")]
    [Test(Description = "TestValidate_MER_FUN_STL_BIW_FTR_DRC_F06 with generated merchant"), CancelAfter(120 * 1000)]
    public async Task TestValidate_MER_FUN_STL_BIW_FTR_DRC_F06()
    {
        try
        {
            Console.WriteLine("\nStarting TestValidate_MER_FUN_STL_BIW_FTR_DRC_F06");
            var testName = TestContext.CurrentContext.Test.Name;
            var terminalApiKey = _envConfig.MER_FUN_STL_BIW_FTR;
            
            
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            createPlanDefaultValues.paymentMethod.card.cardNumber = Environment.GetEnvironmentVariable("DebitCard")!;
            var planCreateResponse = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!, 
                _requestHeader!, "PendingCapture", 6, terminalApiKey, createPlanDefaultValues);
            var skuQueryResult = await _mongoHandler.SendMongoQueryForSkuAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", planCreateResponse.InstallmentPlanNumber, "Sku");
            Assert.That(_skuTestsData.ValidateSkuValue(skuQueryResult, testName.Substring(0, 40)), Is.True);
            var listActivityValues = await _mongoHandler.SendMongoQueryActivityAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber",
                planCreateResponse.InstallmentPlanNumber, "Activity");
            if (listActivityValues.Count is not (2 or 1))
                Assert.Fail("listActivityValues.Count has wrong value");
            Assert.That(
                _mongoHandler.ValidateActivityValueExistence(listActivityValues, _skuDict![testName.Substring(0, 40)]),
                Is.True);
            Console.WriteLine("Done TestValidate_MER_FUN_STL_BIW_FTR_DRC_F06");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidate_MER_FUN_STL_BIW_FTR_DRC_F06 \n" + exception + "\n");
        }
    }

    [TestCase(Category = "SkuFunBiwTests")]
    [Test(Description = "TestValidate_MER_FUN_STL_BIW_FTR_CRC_F06 with generated merchant"), CancelAfter(120 * 1000)]
    public async Task TestValidate_MER_FUN_STL_BIW_FTR_CRC_F06()
    {
        try
        {
            Console.WriteLine("\nStarting TestValidate_MER_FUN_STL_BIW_FTR_CRC_F06");
            var testName = TestContext.CurrentContext.Test.Name;
            var terminalApiKey = _envConfig.MER_FUN_STL_BIW_FTR;
            
            
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            var planCreateResponse = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!, 
                _requestHeader!, "Active", 6, terminalApiKey, createPlanDefaultValues);
            var skuQueryResult = await _mongoHandler.SendMongoQueryForSkuAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", planCreateResponse.InstallmentPlanNumber, "Sku");
            Assert.That(_skuTestsData.ValidateSkuValue(skuQueryResult, testName.Substring(0, 40)), Is.True);
            var listActivityValues = await _mongoHandler.SendMongoQueryActivityAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber",
                planCreateResponse.InstallmentPlanNumber, "Activity");
            if (listActivityValues.Count is not (2 or 1))
                Assert.Fail("listActivityValues.Count has wrong value");
            Assert.That(
                _mongoHandler.ValidateActivityValueExistence(listActivityValues, _skuDict![testName.Substring(0, 40)]),
                Is.True);
            Console.WriteLine("Done TestValidate_MER_FUN_STL_BIW_FTR_CRC_F06");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidate_MER_FUN_STL_BIW_FTR_CRC_F06 \n" + exception + "\n");
        }
    }

    [TestCase(Category = "SkuFunBIWTests")]
    [Test(Description = "TestValidate_MER_FUN_STL_BIW_FTR_DRC_F07 with generated merchant"), CancelAfter(120 * 1000)]
    public async Task TestValidate_MER_FUN_STL_BIW_FTR_DRC_F07()
    {
        try
        {
            Console.WriteLine("\nStarting TestValidate_MER_FUN_STL_BIW_FTR_DRC_F07");
            var testName = TestContext.CurrentContext.Test.Name;
            var terminalApiKey = _envConfig.MER_FUN_STL_BIW_FTR;
            
            
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            createPlanDefaultValues.paymentMethod.card.cardNumber = Environment.GetEnvironmentVariable("DebitCard")!;
            var planCreateResponse = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!, 
                _requestHeader!, "PendingCapture", 7, terminalApiKey, createPlanDefaultValues);
            var skuQueryResult = await _mongoHandler.SendMongoQueryForSkuAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", planCreateResponse.InstallmentPlanNumber, "Sku");
            Assert.That(_skuTestsData.ValidateSkuValue(skuQueryResult, testName.Substring(0, 40)), Is.True);
            var listActivityValues = await _mongoHandler.SendMongoQueryActivityAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber",
                planCreateResponse.InstallmentPlanNumber, "Activity");
            if (listActivityValues.Count is not (2 or 1))
                Assert.Fail("listActivityValues.Count has wrong value");
            Assert.That(
                _mongoHandler.ValidateActivityValueExistence(listActivityValues, _skuDict![testName.Substring(0, 40)]),
                Is.True);
            Console.WriteLine("Done TestValidate_MER_FUN_STL_BIW_FTR_DRC_F07");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidate_MER_FUN_STL_BIW_FTR_DRC_F07\n" + exception + "\n");
        }
    }

    [TestCase(Category = "SkuFunBIWTests")]
    [Test(Description = "TestValidate_MER_FUN_STL_BIW_FTR_CRC_F07 with generated merchant"), CancelAfter(120 * 1000)]
    public async Task TestValidate_MER_FUN_STL_BIW_FTR_CRC_F07()
    {
        try
        {
            Console.WriteLine("\nStarting TestValidate_MER_FUN_STL_BIW_FTR_CRC_F07");
            var testName = TestContext.CurrentContext.Test.Name;
            var terminalApiKey = _envConfig.MER_FUN_STL_BIW_FTR;
            
            
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            var planCreateResponse = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!, 
                _requestHeader!, "Active", 7, terminalApiKey, createPlanDefaultValues);
            var skuQueryResult = await _mongoHandler.SendMongoQueryForSkuAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", planCreateResponse.InstallmentPlanNumber, "Sku");
            Assert.That(_skuTestsData.ValidateSkuValue(skuQueryResult, testName.Substring(0, 40)), Is.True);
            var listActivityValues = await _mongoHandler.SendMongoQueryActivityAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber",
                planCreateResponse.InstallmentPlanNumber, "Activity");
            if (listActivityValues.Count is not (2 or 1))
                Assert.Fail("listActivityValues.Count has wrong value");
            Assert.That(
                _mongoHandler.ValidateActivityValueExistence(listActivityValues, _skuDict![testName.Substring(0, 40)]),
                Is.True);
            Console.WriteLine("Done TestValidate_MER_FUN_STL_BIW_FTR_CRC_F07");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidate_MER_FUN_STL_BIW_FTR_CRC_F07\n" + exception + "\n");
        }
    }

    [TestCase(Category = "SkuFunBIWTests")]
    [Test(Description = "TestValidate_MER_FUN_STL_BIW_FTR_DRC_F08 with generated merchant"), CancelAfter(120 * 1000)]
    public async Task TestValidate_MER_FUN_STL_BIW_FTR_DRC_F08()
    {
        try
        {
            Console.WriteLine("\nStarting TestValidate_MER_FUN_STL_BIW_FTR_DRC_F08");
            var testName = TestContext.CurrentContext.Test.Name;
            var terminalApiKey = _envConfig.MER_FUN_STL_BIW_FTR;
            
            
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            createPlanDefaultValues.paymentMethod.card.cardNumber = Environment.GetEnvironmentVariable("DebitCard")!;
            var planCreateResponse = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!, 
                _requestHeader!, "PendingCapture", 8, terminalApiKey, createPlanDefaultValues);
            var skuQueryResult = await _mongoHandler.SendMongoQueryForSkuAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", planCreateResponse.InstallmentPlanNumber, "Sku");
            Assert.That(_skuTestsData.ValidateSkuValue(skuQueryResult, testName.Substring(0, 40)), Is.True);
            var listActivityValues = await _mongoHandler.SendMongoQueryActivityAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber",
                planCreateResponse.InstallmentPlanNumber, "Activity");
            if (listActivityValues.Count is not (2 or 1))
                Assert.Fail("listActivityValues.Count has wrong value");
            Assert.That(
                _mongoHandler.ValidateActivityValueExistence(listActivityValues, _skuDict![testName.Substring(0, 40)]),
                Is.True);
            Console.WriteLine("Done TestValidate_MER_FUN_STL_BIW_FTR_DRC_F08");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidate_MER_FUN_STL_BIW_FTR_DRC_F08 \n" + exception + "\n");
        }
    }

    [TestCase(Category = "SkuFunBIWTests")]
    [Test(Description = "TestValidate_MER_FUN_STL_BIW_FTR_CRC_F08 with generated merchant"), CancelAfter(120 * 1000)]
    public async Task TestValidate_MER_FUN_STL_BIW_FTR_CRC_F08()
    {
        try
        {
            Console.WriteLine("\nStarting TestValidate_MER_FUN_STL_BIW_FTR_CRC_F08");
            var testName = TestContext.CurrentContext.Test.Name;
            var terminalApiKey = _envConfig.MER_FUN_STL_BIW_FTR;
            
            
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            var planCreateResponse = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!, 
                _requestHeader!, "Active", 8, terminalApiKey, createPlanDefaultValues);
            var skuQueryResult = await _mongoHandler.SendMongoQueryForSkuAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", planCreateResponse.InstallmentPlanNumber, "Sku");
            Assert.That(_skuTestsData.ValidateSkuValue(skuQueryResult, testName.Substring(0, 40)), Is.True);
            var listActivityValues = await _mongoHandler.SendMongoQueryActivityAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber",
                planCreateResponse.InstallmentPlanNumber, "Activity");
            if (listActivityValues.Count is not (2 or 1))
                Assert.Fail("listActivityValues.Count has wrong value");
            Assert.That(
                _mongoHandler.ValidateActivityValueExistence(listActivityValues, _skuDict![testName.Substring(0, 40)]),
                Is.True);
            Console.WriteLine("Done TestValidate_MER_FUN_STL_BIW_FTR_CRC_F08");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidate_MER_FUN_STL_BIW_FTR_CRC_F08 \n" + exception + "\n");
        }
    }

    [TestCase(Category = "SkuFunBiwTests")]
    [Test(Description = "TestValidate_MER_FUN_STL_BIW_FTR_DRC_F09 with generated merchant"), CancelAfter(120 * 1000)]
    public async Task TestValidate_MER_FUN_STL_BIW_FTR_DRC_F09()
    {
        try
        {
            Console.WriteLine("\nStarting TestValidate_MER_FUN_STL_BIW_FTR_DRC_F09");
            var testName = TestContext.CurrentContext.Test.Name;
            var terminalApiKey = _envConfig.MER_FUN_STL_BIW_FTR;
            
            
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            createPlanDefaultValues.paymentMethod.card.cardNumber = Environment.GetEnvironmentVariable("DebitCard")!;
            var planCreateResponse = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!, 
                _requestHeader!, "PendingCapture", 9, terminalApiKey, createPlanDefaultValues);
            var skuQueryResult = await _mongoHandler.SendMongoQueryForSkuAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", planCreateResponse.InstallmentPlanNumber, "Sku");
            Assert.That(_skuTestsData.ValidateSkuValue(skuQueryResult, testName.Substring(0, 40)), Is.True);
            var listActivityValues = await _mongoHandler.SendMongoQueryActivityAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber",
                planCreateResponse.InstallmentPlanNumber, "Activity");
            if (listActivityValues.Count is not (2 or 1))
                Assert.Fail("listActivityValues.Count has wrong value");
            Assert.That(
                _mongoHandler.ValidateActivityValueExistence(listActivityValues, _skuDict![testName.Substring(0, 40)]),
                Is.True);
            Console.WriteLine("Done TestValidate_MER_FUN_STL_BIW_FTR_DRC_F09");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidate_MER_FUN_STL_BIW_FTR_DRC_F09 \n" + exception + "\n");
        }
    }

    [TestCase(Category = "SkuFunBiwTests")]
    [Test(Description = "TestValidate_MER_FUN_STL_BIW_FTR_CRC_F09 with generated merchant"), CancelAfter(120 * 1000)]
    public async Task TestValidate_MER_FUN_STL_BIW_FTR_CRC_F09()
    {
        try
        {
            Console.WriteLine("\nStarting TestValidate_MER_FUN_STL_BIW_FTR_CRC_F09");
            var testName = TestContext.CurrentContext.Test.Name;
            var terminalApiKey = _envConfig.MER_FUN_STL_BIW_FTR;
            
            
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            var planCreateResponse = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!, 
                _requestHeader!, "Active", 9, terminalApiKey, createPlanDefaultValues);
            var skuQueryResult = await _mongoHandler.SendMongoQueryForSkuAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", planCreateResponse.InstallmentPlanNumber, "Sku");
            Assert.That(_skuTestsData.ValidateSkuValue(skuQueryResult, testName.Substring(0, 40)), Is.True);
            var listActivityValues = await _mongoHandler.SendMongoQueryActivityAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber",
                planCreateResponse.InstallmentPlanNumber, "Activity");
            if (listActivityValues.Count is not (2 or 1))
                Assert.Fail("listActivityValues.Count has wrong value");
            Assert.That(
                _mongoHandler.ValidateActivityValueExistence(listActivityValues, _skuDict![testName.Substring(0, 40)]),
                Is.True);
            Console.WriteLine("Done TestValidate_MER_FUN_STL_BIW_FTR_CRC_F09");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidate_MER_FUN_STL_BIW_FTR_CRC_F09 \n" + exception + "\n");
        }
    }

    [TestCase(Category = "SkuFunBIWTests")]
    [Test(Description = "TestValidate_MER_FUN_STL_BIW_FTR_DRC_F10 with generated merchant"), CancelAfter(120 * 1000)]
    public async Task TestValidate_MER_FUN_STL_BIW_FTR_DRC_F10()
    {
        try
        {
            Console.WriteLine("\nStarting TestValidate_MER_FUN_STL_BIW_FTR_DRC_F10");
            var testName = TestContext.CurrentContext.Test.Name;
            var terminalApiKey = _envConfig.MER_FUN_STL_BIW_FTR;
            
            
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            createPlanDefaultValues.paymentMethod.card.cardNumber = Environment.GetEnvironmentVariable("DebitCard")!;
            var planCreateResponse = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!, 
                _requestHeader!, "PendingCapture", 10, terminalApiKey, createPlanDefaultValues);
            var skuQueryResult = await _mongoHandler.SendMongoQueryForSkuAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", planCreateResponse.InstallmentPlanNumber, "Sku");
            Assert.That(_skuTestsData.ValidateSkuValue(skuQueryResult, testName.Substring(0, 40)), Is.True);
            var listActivityValues = await _mongoHandler.SendMongoQueryActivityAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber",
                planCreateResponse.InstallmentPlanNumber, "Activity");
            if (listActivityValues.Count is not (2 or 1))
                Assert.Fail("listActivityValues.Count has wrong value");
            Assert.That(
                _mongoHandler.ValidateActivityValueExistence(listActivityValues, _skuDict![testName.Substring(0, 40)]),
                Is.True);
            Console.WriteLine("Done TestValidate_MER_FUN_STL_BIW_FTR_DRC_F10");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidate_MER_FUN_STL_BIW_FTR_DRC_F10 \n" + exception + "\n");
        }
    }

    [TestCase(Category = "SkuFunBIWTests")]
    [Test(Description = "TestValidate_MER_FUN_STL_BIW_FTR_CRC_F10 with generated merchant"), CancelAfter(120 * 1000)]
    public async Task TestValidate_MER_FUN_STL_BIW_FTR_CRC_F10()
    {
        try
        {
            Console.WriteLine("\nStarting TestValidate_MER_FUN_STL_BIW_FTR_CRC_F10");
            var testName = TestContext.CurrentContext.Test.Name;
            var terminalApiKey = _envConfig.MER_FUN_STL_BIW_FTR;
            
            
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            var planCreateResponse = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!, 
                _requestHeader!, "Active", 10, terminalApiKey, createPlanDefaultValues);
            var skuQueryResult = await _mongoHandler.SendMongoQueryForSkuAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", planCreateResponse.InstallmentPlanNumber, "Sku");
            Assert.That(_skuTestsData.ValidateSkuValue(skuQueryResult, testName.Substring(0, 40)), Is.True);
            var listActivityValues = await _mongoHandler.SendMongoQueryActivityAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber",
                planCreateResponse.InstallmentPlanNumber, "Activity");
            if (listActivityValues.Count is not (2 or 1))
                Assert.Fail("listActivityValues.Count has wrong value");
            Assert.That(
                _mongoHandler.ValidateActivityValueExistence(listActivityValues, _skuDict![testName.Substring(0, 40)]),
                Is.True);
            Console.WriteLine("Done TestValidate_MER_FUN_STL_BIW_FTR_CRC_F10");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidate_MER_FUN_STL_BIW_FTR_CRC_F10 \n" + exception + "\n");
        }
    }

    [TestCase(Category = "SkuFunBIWTests")]
    [Test(Description = "TestValidate_MER_FUN_STL_BIW_FTR_DRC_F11 with generated merchant"), CancelAfter(120 * 1000)]
    public async Task TestValidate_MER_FUN_STL_BIW_FTR_DRC_F11()
    {
        try
        {
            Console.WriteLine("\nStarting TestValidate_MER_FUN_STL_BIW_FTR_DRC_F11");
            var testName = TestContext.CurrentContext.Test.Name;
            var terminalApiKey = _envConfig.MER_FUN_STL_BIW_FTR;
            
            
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            createPlanDefaultValues.paymentMethod.card.cardNumber = Environment.GetEnvironmentVariable("DebitCard")!;
            var planCreateResponse = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!, 
                _requestHeader!, "PendingCapture", 11, terminalApiKey, createPlanDefaultValues);
            var skuQueryResult = await _mongoHandler.SendMongoQueryForSkuAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", planCreateResponse.InstallmentPlanNumber, "Sku");
            Assert.That(_skuTestsData.ValidateSkuValue(skuQueryResult, testName.Substring(0, 40)), Is.True);
            var listActivityValues = await _mongoHandler.SendMongoQueryActivityAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber",
                planCreateResponse.InstallmentPlanNumber, "Activity");
            if (listActivityValues.Count is not (2 or 1))
                Assert.Fail("listActivityValues.Count has wrong value");
            Assert.That(
                _mongoHandler.ValidateActivityValueExistence(listActivityValues, _skuDict![testName.Substring(0, 40)]),
                Is.True);
            Console.WriteLine("Done TestValidate_MER_FUN_STL_BIW_FTR_DRC_F11");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidate_MER_FUN_STL_BIW_FTR_DRC_F11 \n" + exception + "\n");
        }
    }

    [TestCase(Category = "SkuFunBIWTests")]
    [Test(Description = "TestValidate_MER_FUN_STL_BIW_FTR_CRC_F11 with generated merchant"), CancelAfter(120 * 1000)]
    public async Task TestValidate_MER_FUN_STL_BIW_FTR_CRC_F11()
    {
        try
        {
            Console.WriteLine("\nStarting TestValidate_MER_FUN_STL_BIW_FTR_CRC_F11");
            var testName = TestContext.CurrentContext.Test.Name;
            var terminalApiKey = _envConfig.MER_FUN_STL_BIW_FTR;
            
            
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            var planCreateResponse = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!, 
                _requestHeader!, "Active", 11, terminalApiKey, createPlanDefaultValues);
            var skuQueryResult = await _mongoHandler.SendMongoQueryForSkuAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", planCreateResponse.InstallmentPlanNumber, "Sku");
            Assert.That(_skuTestsData.ValidateSkuValue(skuQueryResult, testName.Substring(0, 40)), Is.True);
            var listActivityValues = await _mongoHandler.SendMongoQueryActivityAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber",
                planCreateResponse.InstallmentPlanNumber, "Activity");
            if (listActivityValues.Count is not (2 or 1))
                Assert.Fail("listActivityValues.Count has wrong value");
            Assert.That(
                _mongoHandler.ValidateActivityValueExistence(listActivityValues, _skuDict![testName.Substring(0, 40)]),
                Is.True);
            Console.WriteLine("Done TestValidate_MER_FUN_STL_BIW_FTR_CRC_F11");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidate_MER_FUN_STL_BIW_FTR_CRC_F11 \n" + exception + "\n");
        }
    }

    [TestCase(Category = "SkuFunBiwTests")]
    [Test(Description = "TestValidate_MER_FUN_STL_BIW_FTR_DRC_F12 with generated merchant"), CancelAfter(120 * 1000)]
    public async Task TestValidate_MER_FUN_STL_BIW_FTR_DRC_F12()
    {
        try
        {
            Console.WriteLine("\nStarting TestValidate_MER_FUN_STL_BIW_FTR_DRC_F12");
            var testName = TestContext.CurrentContext.Test.Name;
            var terminalApiKey = _envConfig.MER_FUN_STL_BIW_FTR;
            
            
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            createPlanDefaultValues.paymentMethod.card.cardNumber = Environment.GetEnvironmentVariable("DebitCard")!;
            var planCreateResponse = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!, 
                _requestHeader!, "PendingCapture", 12, terminalApiKey, createPlanDefaultValues);
            var skuQueryResult = await _mongoHandler.SendMongoQueryForSkuAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", planCreateResponse.InstallmentPlanNumber, "Sku");
            Assert.That(_skuTestsData.ValidateSkuValue(skuQueryResult, testName.Substring(0, 40)), Is.True);
            var listActivityValues = await _mongoHandler.SendMongoQueryActivityAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber",
                planCreateResponse.InstallmentPlanNumber, "Activity");
            if (listActivityValues.Count is not (2 or 1))
                Assert.Fail("listActivityValues.Count has wrong value");
            Assert.That(
                _mongoHandler.ValidateActivityValueExistence(listActivityValues, _skuDict![testName.Substring(0, 40)]),
                Is.True);
            Console.WriteLine("Done TestValidate_MER_FUN_STL_BIW_FTR_DRC_F12");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidate_MER_FUN_STL_BIW_FTR_DRC_F12 \n" + exception + "\n");
        }
    }

    [TestCase(Category = "SkuFunBiwTests")]
    [Test(Description = "TestValidate_MER_FUN_STL_BIW_FTR_CRC_F12 with generated merchant"), CancelAfter(120 * 1000)]
    public async Task TestValidate_MER_FUN_STL_BIW_FTR_CRC_F12()
    {
        try
        {
            Console.WriteLine("\nStarting TestValidate_MER_FUN_STL_BIW_FTR_CRC_F12");
            var testName = TestContext.CurrentContext.Test.Name;
            var terminalApiKey = _envConfig.MER_FUN_STL_BIW_FTR;
            
            
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            var planCreateResponse = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!, 
                _requestHeader!, "Active", 12, terminalApiKey, createPlanDefaultValues);
            var skuQueryResult = await _mongoHandler.SendMongoQueryForSkuAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", planCreateResponse.InstallmentPlanNumber, "Sku");
            Assert.That(_skuTestsData.ValidateSkuValue(skuQueryResult, testName.Substring(0, 40)), Is.True);
            var listActivityValues = await _mongoHandler.SendMongoQueryActivityAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber",
                planCreateResponse.InstallmentPlanNumber, "Activity");
            if (listActivityValues.Count is not (2 or 1))
                Assert.Fail("listActivityValues.Count has wrong value");
            Assert.That(
                _mongoHandler.ValidateActivityValueExistence(listActivityValues, _skuDict![testName.Substring(0, 40)]),
                Is.True);
            Console.WriteLine("Done TestValidate_MER_FUN_STL_BIW_FTR_CRC_F12");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidate_MER_FUN_STL_BIW_FTR_CRC_F12 \n" + exception + "\n");
        }
    }

    [TestCase(Category = "SkuFunBIWTests")]
    [Test(Description = "TestValidate_MER_FUN_STL_BIW_FTR_DRC_F13 with generated merchant"), CancelAfter(120 * 1000)]
    public async Task TestValidate_MER_FUN_STL_BIW_FTR_DRC_F13()
    {
        try
        {
            Console.WriteLine("\nStarting TestValidate_MER_FUN_STL_BIW_FTR_DRC_F13");
            var testName = TestContext.CurrentContext.Test.Name;
            var terminalApiKey = _envConfig.MER_FUN_STL_BIW_FTR;
            
            
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            createPlanDefaultValues.paymentMethod.card.cardNumber = Environment.GetEnvironmentVariable("DebitCard")!;
            var planCreateResponse = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!, 
                _requestHeader!, "PendingCapture", 13, terminalApiKey, createPlanDefaultValues);
            var skuQueryResult = await _mongoHandler.SendMongoQueryForSkuAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", planCreateResponse.InstallmentPlanNumber, "Sku");
            Assert.That(_skuTestsData.ValidateSkuValue(skuQueryResult, testName.Substring(0, 40)), Is.True);
            var listActivityValues = await _mongoHandler.SendMongoQueryActivityAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber",
                planCreateResponse.InstallmentPlanNumber, "Activity");
            if (listActivityValues.Count is not (2 or 1))
                Assert.Fail("listActivityValues.Count has wrong value");
            Assert.That(
                _mongoHandler.ValidateActivityValueExistence(listActivityValues, _skuDict![testName.Substring(0, 40)]),
                Is.True);
            Console.WriteLine("Done TestValidate_MER_FUN_STL_BIW_FTR_DRC_F13");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidate_MER_FUN_STL_BIW_FTR_DRC_F13 \n" + exception + "\n");
        }
    }

    [TestCase(Category = "SkuFunBIWTests")]
    [Test(Description = "TestValidate_MER_FUN_STL_BIW_FTR_CRC_F13 with generated merchant"), CancelAfter(120 * 1000)]
    public async Task TestValidate_MER_FUN_STL_BIW_FTR_CRC_F13()
    {
        try
        {
            Console.WriteLine("\nStarting TestValidate_MER_FUN_STL_BIW_FTR_CRC_F13");
            var testName = TestContext.CurrentContext.Test.Name;
            var terminalApiKey = _envConfig.MER_FUN_STL_BIW_FTR;
            
            
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            var planCreateResponse = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!, 
                _requestHeader!, "Active", 13, terminalApiKey, createPlanDefaultValues);
            var skuQueryResult = await _mongoHandler.SendMongoQueryForSkuAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", planCreateResponse.InstallmentPlanNumber, "Sku");
            Assert.That(_skuTestsData.ValidateSkuValue(skuQueryResult, testName.Substring(0, 40)), Is.True);
            var listActivityValues = await _mongoHandler.SendMongoQueryActivityAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber",
                planCreateResponse.InstallmentPlanNumber, "Activity");
            if (listActivityValues.Count is not (2 or 1))
                Assert.Fail("listActivityValues.Count has wrong value");
            Assert.That(
                _mongoHandler.ValidateActivityValueExistence(listActivityValues, _skuDict![testName.Substring(0, 40)]),
                Is.True);
            Console.WriteLine("Done TestValidate_MER_FUN_STL_BIW_FTR_CRC_F13");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidate_MER_FUN_STL_BIW_FTR_CRC_F13 \n" + exception + "\n");
        }
    }

    [TestCase(Category = "SkuFunBIWTests")]
    [Test(Description = "TestValidate_MER_FUN_STL_BIW_FTR_DRC_F14 with generated merchant"), CancelAfter(120 * 1000)]
    public async Task TestValidate_MER_FUN_STL_BIW_FTR_DRC_F14()
    {
        try
        {
            Console.WriteLine("\nStarting TestValidate_MER_FUN_STL_BIW_FTR_DRC_F14");
            var testName = TestContext.CurrentContext.Test.Name;
            var terminalApiKey = _envConfig.MER_FUN_STL_BIW_FTR;
            
            
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            createPlanDefaultValues.paymentMethod.card.cardNumber = Environment.GetEnvironmentVariable("DebitCard")!;
            var planCreateResponse = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!, 
                _requestHeader!, "PendingCapture", 14, terminalApiKey, createPlanDefaultValues);
            var skuQueryResult = await _mongoHandler.SendMongoQueryForSkuAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", planCreateResponse.InstallmentPlanNumber, "Sku");
            Assert.That(_skuTestsData.ValidateSkuValue(skuQueryResult, testName.Substring(0, 40)), Is.True);
            var listActivityValues = await _mongoHandler.SendMongoQueryActivityAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber",
                planCreateResponse.InstallmentPlanNumber, "Activity");
            if (listActivityValues.Count is not (2 or 1))
                Assert.Fail("listActivityValues.Count has wrong value");
            Assert.That(
                _mongoHandler.ValidateActivityValueExistence(listActivityValues, _skuDict![testName.Substring(0, 40)]),
                Is.True);
            Console.WriteLine("Done TestValidate_MER_FUN_STL_BIW_FTR_DRC_F14");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidate_MER_FUN_STL_BIW_FTR_DRC_F14 \n" + exception + "\n");
        }
    }

    [TestCase(Category = "SkuFunBIWTests")]
    [Test(Description = "TestValidate_MER_FUN_STL_BIW_FTR_CRC_F14 with generated merchant"), CancelAfter(120 * 1000)]
    public async Task TestValidate_MER_FUN_STL_BIW_FTR_CRC_F14()
    {
        try
        {
            Console.WriteLine("\nStarting TestValidate_MER_FUN_STL_BIW_FTR_CRC_F14");
            var testName = TestContext.CurrentContext.Test.Name;
            var terminalApiKey = _envConfig.MER_FUN_STL_BIW_FTR;
            
            
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            var planCreateResponse = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!, 
                _requestHeader!, "Active", 14, terminalApiKey, createPlanDefaultValues);
            var skuQueryResult = await _mongoHandler.SendMongoQueryForSkuAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", planCreateResponse.InstallmentPlanNumber, "Sku");
            Assert.That(_skuTestsData.ValidateSkuValue(skuQueryResult, testName.Substring(0, 40)), Is.True);
            var listActivityValues = await _mongoHandler.SendMongoQueryActivityAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber",
                planCreateResponse.InstallmentPlanNumber, "Activity");
            if (listActivityValues.Count is not (2 or 1))
                Assert.Fail("listActivityValues.Count has wrong value");
            Assert.That(
                _mongoHandler.ValidateActivityValueExistence(listActivityValues, _skuDict![testName.Substring(0, 40)]),
                Is.True);
            Console.WriteLine("Done TestValidate_MER_FUN_STL_BIW_FTR_CRC_F14");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidate_MER_FUN_STL_BIW_FTR_CRC_F14 \n" + exception + "\n");
        }
    }

    [TestCase(Category = "SkuFunBiwTests")]
    [Test(Description = "TestValidate_MER_FUN_STL_BIW_FTR_DRC_F15 with generated merchant"), CancelAfter(120 * 1000)]
    public async Task TestValidate_MER_FUN_STL_BIW_FTR_DRC_F15()
    {
        try
        {
            Console.WriteLine("\nStarting TestValidate_MER_FUN_STL_BIW_FTR_DRC_F15");
            var testName = TestContext.CurrentContext.Test.Name;
            var terminalApiKey = _envConfig.MER_FUN_STL_BIW_FTR;
            
            
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            createPlanDefaultValues.paymentMethod.card.cardNumber = Environment.GetEnvironmentVariable("DebitCard")!;
            var planCreateResponse = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!, 
                _requestHeader!, "PendingCapture", 15, terminalApiKey, createPlanDefaultValues);
            var skuQueryResult = await _mongoHandler.SendMongoQueryForSkuAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", planCreateResponse.InstallmentPlanNumber, "Sku");
            Assert.That(_skuTestsData.ValidateSkuValue(skuQueryResult, testName.Substring(0, 40)), Is.True);
            var listActivityValues = await _mongoHandler.SendMongoQueryActivityAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber",
                planCreateResponse.InstallmentPlanNumber, "Activity");
            if (listActivityValues.Count is not (2 or 1))
                Assert.Fail("listActivityValues.Count has wrong value");
            Assert.That(
                _mongoHandler.ValidateActivityValueExistence(listActivityValues, _skuDict![testName.Substring(0, 40)]),
                Is.True);
            Console.WriteLine("Done TestValidate_MER_FUN_STL_BIW_FTR_DRC_F15");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidate_MER_FUN_STL_BIW_FTR_DRC_F15 \n" + exception + "\n");
        }
    }

    [TestCase(Category = "SkuFunBiwTests")]
    [Test(Description = "TestValidate_MER_FUN_STL_BIW_FTR_CRC_F15 with generated merchant"), CancelAfter(120 * 1000)]
    public async Task TestValidate_MER_FUN_STL_BIW_FTR_CRC_F15()
    {
        try
        {
            Console.WriteLine("\nStarting TestValidate_MER_FUN_STL_BIW_FTR_CRC_F15");
            var testName = TestContext.CurrentContext.Test.Name;
            var terminalApiKey = _envConfig.MER_FUN_STL_BIW_FTR;
            
            
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            var planCreateResponse = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!, 
                _requestHeader!, "Active", 15, terminalApiKey, createPlanDefaultValues);
            var skuQueryResult = await _mongoHandler.SendMongoQueryForSkuAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", planCreateResponse.InstallmentPlanNumber, "Sku");
            Assert.That(_skuTestsData.ValidateSkuValue(skuQueryResult, testName.Substring(0, 40)), Is.True);
            var listActivityValues = await _mongoHandler.SendMongoQueryActivityAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber",
                planCreateResponse.InstallmentPlanNumber, "Activity");
            if (listActivityValues.Count is not (2 or 1))
                Assert.Fail("listActivityValues.Count has wrong value");
            Assert.That(
                _mongoHandler.ValidateActivityValueExistence(listActivityValues, _skuDict![testName.Substring(0, 40)]),
                Is.True);
            Console.WriteLine("Done TestValidate_MER_FUN_STL_BIW_FTR_CRC_F15");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidate_MER_FUN_STL_BIW_FTR_CRC_F15 \n" + exception + "\n");
        }
    }

    [TestCase(Category = "SkuFunBIWTests")]
    [Test(Description = "TestValidate_MER_FUN_STL_BIW_FTR_DRC_F16 with generated merchant"), CancelAfter(120 * 1000)]
    public async Task TestValidate_MER_FUN_STL_BIW_FTR_DRC_F16()
    {
        try
        {
            Console.WriteLine("\nStarting TestValidate_MER_FUN_STL_BIW_FTR_DRC_F16");
            var testName = TestContext.CurrentContext.Test.Name;
            var terminalApiKey = _envConfig.MER_FUN_STL_BIW_FTR;
            
            
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            createPlanDefaultValues.paymentMethod.card.cardNumber = Environment.GetEnvironmentVariable("DebitCard")!;
            var planCreateResponse = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!, 
                _requestHeader!, "PendingCapture", 16, terminalApiKey, createPlanDefaultValues);
            var skuQueryResult = await _mongoHandler.SendMongoQueryForSkuAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", planCreateResponse.InstallmentPlanNumber, "Sku");
            Assert.That(_skuTestsData.ValidateSkuValue(skuQueryResult, testName.Substring(0, 40)), Is.True);
            var listActivityValues = await _mongoHandler.SendMongoQueryActivityAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber",
                planCreateResponse.InstallmentPlanNumber, "Activity");
            if (listActivityValues.Count is not (2 or 1))
                Assert.Fail("listActivityValues.Count has wrong value");
            Assert.That(
                _mongoHandler.ValidateActivityValueExistence(listActivityValues, _skuDict![testName.Substring(0, 40)]),
                Is.True);
            Console.WriteLine("Done TestValidate_MER_FUN_STL_BIW_FTR_DRC_F16");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidate_MER_FUN_STL_BIW_FTR_DRC_F16 \n" + exception + "\n");
        }
    }

    [TestCase(Category = "SkuFunBIWTests")]
    [Test(Description = "TestValidate_MER_FUN_STL_BIW_FTR_CRC_F16 with generated merchant"), CancelAfter(120 * 1000)]
    public async Task TestValidate_MER_FUN_STL_BIW_FTR_CRC_F16()
    {
        try
        {
            Console.WriteLine("\nStarting TestValidate_MER_FUN_STL_BIW_FTR_CRC_F16");
            var testName = TestContext.CurrentContext.Test.Name;
            var terminalApiKey = _envConfig.MER_FUN_STL_BIW_FTR;
            
            
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            var planCreateResponse = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!, 
                _requestHeader!, "Active", 16, terminalApiKey, createPlanDefaultValues);
            var skuQueryResult = await _mongoHandler.SendMongoQueryForSkuAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", planCreateResponse.InstallmentPlanNumber, "Sku");
            Assert.That(_skuTestsData.ValidateSkuValue(skuQueryResult, testName.Substring(0, 40)), Is.True);
            var listActivityValues = await _mongoHandler.SendMongoQueryActivityAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber",
                planCreateResponse.InstallmentPlanNumber, "Activity");
            if (listActivityValues.Count is not (2 or 1))
                Assert.Fail("listActivityValues.Count has wrong value");
            Assert.That(
                _mongoHandler.ValidateActivityValueExistence(listActivityValues, _skuDict![testName.Substring(0, 40)]),
                Is.True);
            Console.WriteLine("Done TestValidate_MER_FUN_STL_BIW_FTR_CRC_F16");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidate_MER_FUN_STL_BIW_FTR_CRC_F16 \n" + exception + "\n");
        }
    }

    [TestCase(Category = "SkuFunBIWTests")]
    [Test(Description = "TestValidate_MER_FUN_STL_BIW_FTR_DRC_F17 with generated merchant"), CancelAfter(120 * 1000)]
    public async Task TestValidate_MER_FUN_STL_BIW_FTR_DRC_F17()
    {
        try
        {
            Console.WriteLine("\nStarting TestValidate_MER_FUN_STL_BIW_FTR_DRC_F17");
            var testName = TestContext.CurrentContext.Test.Name;
            var terminalApiKey = _envConfig.MER_FUN_STL_BIW_FTR;
            
            
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            createPlanDefaultValues.paymentMethod.card.cardNumber = Environment.GetEnvironmentVariable("DebitCard")!;
            var planCreateResponse = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!, 
                _requestHeader!, "PendingCapture", 17, terminalApiKey, createPlanDefaultValues);
            var skuQueryResult = await _mongoHandler.SendMongoQueryForSkuAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", planCreateResponse.InstallmentPlanNumber, "Sku");
            Assert.That(_skuTestsData.ValidateSkuValue(skuQueryResult, testName.Substring(0, 40)), Is.True);
            var listActivityValues = await _mongoHandler.SendMongoQueryActivityAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber",
                planCreateResponse.InstallmentPlanNumber, "Activity");
            if (listActivityValues.Count is not (2 or 1))
                Assert.Fail("listActivityValues.Count has wrong value");
            Assert.That(
                _mongoHandler.ValidateActivityValueExistence(listActivityValues, _skuDict![testName.Substring(0, 40)]),
                Is.True);
            Console.WriteLine("Done TestValidate_MER_FUN_STL_BIW_FTR_DRC_F17");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidate_MER_FUN_STL_BIW_FTR_DRC_F17 \n" + exception + "\n");
        }
    }

    [TestCase(Category = "SkuFunBIWTests")]
    [Test(Description = "TestValidate_MER_FUN_STL_BIW_FTR_CRC_F17 with generated merchant"), CancelAfter(120 * 1000)]
    public async Task TestValidate_MER_FUN_STL_BIW_FTR_CRC_F17()
    {
        try
        {
            Console.WriteLine("\nStarting TestValidate_MER_FUN_STL_BIW_FTR_CRC_F17");
            var testName = TestContext.CurrentContext.Test.Name;
            var terminalApiKey = _envConfig.MER_FUN_STL_BIW_FTR;
            
            
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            var planCreateResponse = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!, 
                _requestHeader!, "Active", 17, terminalApiKey, createPlanDefaultValues);
            var skuQueryResult = await _mongoHandler.SendMongoQueryForSkuAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", planCreateResponse.InstallmentPlanNumber, "Sku");
            Assert.That(_skuTestsData.ValidateSkuValue(skuQueryResult, testName.Substring(0, 40)), Is.True);
            var listActivityValues = await _mongoHandler.SendMongoQueryActivityAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber",
                planCreateResponse.InstallmentPlanNumber, "Activity");
            if (listActivityValues.Count is not (2 or 1))
                Assert.Fail("listActivityValues.Count has wrong value");
            Assert.That(
                _mongoHandler.ValidateActivityValueExistence(listActivityValues, _skuDict![testName.Substring(0, 40)]),
                Is.True);
            Console.WriteLine("Done TestValidate_MER_FUN_STL_BIW_FTR_CRC_F17");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidate_MER_FUN_STL_BIW_FTR_CRC_F17 \n" + exception + "\n");
        }
    }

    [TestCase(Category = "SkuFunBiwTests")]
    [Test(Description = "TestValidate_MER_FUN_STL_BIW_FTR_DRC_F18 with generated merchant"), CancelAfter(120 * 1000)]
    public async Task TestValidate_MER_FUN_STL_BIW_FTR_DRC_F18()
    {
        try
        {
            Console.WriteLine("\nStarting TestValidate_MER_FUN_STL_BIW_FTR_DRC_F18");
            var testName = TestContext.CurrentContext.Test.Name;
            var terminalApiKey = _envConfig.MER_FUN_STL_BIW_FTR;
            
            
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            createPlanDefaultValues.paymentMethod.card.cardNumber = Environment.GetEnvironmentVariable("DebitCard")!;
            var planCreateResponse = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!, 
                _requestHeader!, "PendingCapture", 18, terminalApiKey, createPlanDefaultValues);
            var skuQueryResult = await _mongoHandler.SendMongoQueryForSkuAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", planCreateResponse.InstallmentPlanNumber, "Sku");
            Assert.That(_skuTestsData.ValidateSkuValue(skuQueryResult, testName.Substring(0, 40)), Is.True);
            var listActivityValues = await _mongoHandler.SendMongoQueryActivityAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber",
                planCreateResponse.InstallmentPlanNumber, "Activity");
            if (listActivityValues.Count is not (2 or 1))
                Assert.Fail("listActivityValues.Count has wrong value");
            Assert.That(
                _mongoHandler.ValidateActivityValueExistence(listActivityValues, _skuDict![testName.Substring(0, 40)]),
                Is.True);
            Console.WriteLine("Done TestValidate_MER_FUN_STL_BIW_FTR_DRC_F18");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidate_MER_FUN_STL_BIW_FTR_DRC_F18 \n" + exception + "\n");
        }
    }

    [TestCase(Category = "SkuFunBiwTests")]
    [Test(Description = "TestValidate_MER_FUN_STL_BIW_FTR_CRC_F18 with generated merchant"), CancelAfter(120 * 1000)]
    public async Task TestValidate_MER_FUN_STL_BIW_FTR_CRC_F18()
    {
        try
        {
            Console.WriteLine("\nStarting TestValidate_MER_FUN_STL_BIW_FTR_CRC_F18");
            var testName = TestContext.CurrentContext.Test.Name;
            var terminalApiKey = _envConfig.MER_FUN_STL_BIW_FTR;
            
            
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            var planCreateResponse = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!, 
                _requestHeader!, "Active", 18, terminalApiKey, createPlanDefaultValues);
            var skuQueryResult = await _mongoHandler.SendMongoQueryForSkuAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", planCreateResponse.InstallmentPlanNumber, "Sku");
            Assert.That(_skuTestsData.ValidateSkuValue(skuQueryResult, testName.Substring(0, 40)), Is.True);
            var listActivityValues = await _mongoHandler.SendMongoQueryActivityAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber",
                planCreateResponse.InstallmentPlanNumber, "Activity");
            if (listActivityValues.Count is not (2 or 1))
                Assert.Fail("listActivityValues.Count has wrong value");
            Assert.That(
                _mongoHandler.ValidateActivityValueExistence(listActivityValues, _skuDict![testName.Substring(0, 40)]),
                Is.True);
            Console.WriteLine("Done TestValidate_MER_FUN_STL_BIW_FTR_CRC_F18");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidate_MER_FUN_STL_BIW_FTR_CRC_F18 \n" + exception + "\n");
        }
    }

    [TestCase(Category = "SkuFunBIWTests")]
    [Test(Description = "TestValidate_MER_FUN_STL_BIW_FTR_DRC_F19 with generated merchant"), CancelAfter(120 * 1000)]
    public async Task TestValidate_MER_FUN_STL_BIW_FTR_DRC_F19()
    {
        try
        {
            Console.WriteLine("\nStarting TestValidate_MER_FUN_STL_BIW_FTR_DRC_F19");
            var testName = TestContext.CurrentContext.Test.Name;
            var terminalApiKey = _envConfig.MER_FUN_STL_BIW_FTR;
            
            
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            createPlanDefaultValues.paymentMethod.card.cardNumber = Environment.GetEnvironmentVariable("DebitCard")!;
            var planCreateResponse = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!, 
                _requestHeader!, "PendingCapture", 19, terminalApiKey, createPlanDefaultValues);
            var skuQueryResult = await _mongoHandler.SendMongoQueryForSkuAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", planCreateResponse.InstallmentPlanNumber, "Sku");
            Assert.That(_skuTestsData.ValidateSkuValue(skuQueryResult, testName.Substring(0, 40)), Is.True);
            var listActivityValues = await _mongoHandler.SendMongoQueryActivityAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber",
                planCreateResponse.InstallmentPlanNumber, "Activity");
            if (listActivityValues.Count is not (2 or 1))
                Assert.Fail("listActivityValues.Count has wrong value");
            Assert.That(
                _mongoHandler.ValidateActivityValueExistence(listActivityValues, _skuDict![testName.Substring(0, 40)]),
                Is.True);
            Console.WriteLine("Done TestValidate_MER_FUN_STL_BIW_FTR_DRC_F19");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidate_MER_FUN_STL_BIW_FTR_DRC_F19 \n" + exception + "\n");
        }
    }

    [TestCase(Category = "SkuFunBIWTests")]
    [Test(Description = "TestValidate_MER_FUN_STL_BIW_FTR_CRC_F19 with generated merchant"), CancelAfter(120 * 1000)]
    public async Task TestValidate_MER_FUN_STL_BIW_FTR_CRC_F19()
    {
        try
        {
            Console.WriteLine("\nStarting TestValidate_MER_FUN_STL_BIW_FTR_CRC_F19");
            var testName = TestContext.CurrentContext.Test.Name;
            var terminalApiKey = _envConfig.MER_FUN_STL_BIW_FTR;
            
            
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            var planCreateResponse = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!, 
                _requestHeader!, "Active", 19, terminalApiKey, createPlanDefaultValues);
            var skuQueryResult = await _mongoHandler.SendMongoQueryForSkuAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", planCreateResponse.InstallmentPlanNumber, "Sku");
            Assert.That(_skuTestsData.ValidateSkuValue(skuQueryResult, testName.Substring(0, 40)), Is.True);
            var listActivityValues = await _mongoHandler.SendMongoQueryActivityAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber",
                planCreateResponse.InstallmentPlanNumber, "Activity");
            if (listActivityValues.Count is not (2 or 1))
                Assert.Fail("listActivityValues.Count has wrong value");
            Assert.That(
                _mongoHandler.ValidateActivityValueExistence(listActivityValues, _skuDict![testName.Substring(0, 40)]),
                Is.True);
            Console.WriteLine("Done TestValidate_MER_FUN_STL_BIW_FTR_CRC_F19");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidate_MER_FUN_STL_BIW_FTR_CRC_F19 \n" + exception + "\n");
        }
    }

    [TestCase(Category = "SkuFunBIWTests")]
    [Test(Description = "TestValidate_MER_FUN_STL_BIW_FTR_DRC_F20 with generated merchant"), CancelAfter(120 * 1000)]
    public async Task TestValidate_MER_FUN_STL_BIW_FTR_DRC_F20()
    {
        try
        {
            Console.WriteLine("\nStarting TestValidate_MER_FUN_STL_BIW_FTR_DRC_F20");
            var testName = TestContext.CurrentContext.Test.Name;
            var terminalApiKey = _envConfig.MER_FUN_STL_BIW_FTR;
            
            
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            createPlanDefaultValues.paymentMethod.card.cardNumber = Environment.GetEnvironmentVariable("DebitCard")!;
            var planCreateResponse = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!, 
                _requestHeader!, "PendingCapture", 20, terminalApiKey, createPlanDefaultValues);
            var skuQueryResult = await _mongoHandler.SendMongoQueryForSkuAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", planCreateResponse.InstallmentPlanNumber, "Sku");
            Assert.That(_skuTestsData.ValidateSkuValue(skuQueryResult, testName.Substring(0, 40)), Is.True);
            var listActivityValues = await _mongoHandler.SendMongoQueryActivityAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber",
                planCreateResponse.InstallmentPlanNumber, "Activity");
            if (listActivityValues.Count is not (2 or 1))
                Assert.Fail("listActivityValues.Count has wrong value");
            Assert.That(
                _mongoHandler.ValidateActivityValueExistence(listActivityValues, _skuDict![testName.Substring(0, 40)]),
                Is.True);
            Console.WriteLine("Done TestValidate_MER_FUN_STL_BIW_FTR_DRC_F20");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidate_MER_FUN_STL_BIW_FTR_DRC_F20 \n" + exception + "\n");
        }
    }

    [TestCase(Category = "SkuFunBIWTests")]
    [Test(Description = "TestValidate_MER_FUN_STL_BIW_FTR_CRC_F20 with generated merchant"), CancelAfter(120 * 1000)]
    public async Task TestValidate_MER_FUN_STL_BIW_FTR_CRC_F20()
    {
        try
        {
            Console.WriteLine("\nStarting TestValidate_MER_FUN_STL_BIW_FTR_CRC_F20");
            var testName = TestContext.CurrentContext.Test.Name;
            var terminalApiKey = _envConfig.MER_FUN_STL_BIW_FTR;
            
            
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            var planCreateResponse = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!, 
                _requestHeader!, "Active", 20, terminalApiKey, createPlanDefaultValues);
            var skuQueryResult = await _mongoHandler.SendMongoQueryForSkuAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", planCreateResponse.InstallmentPlanNumber, "Sku");
            Assert.That(_skuTestsData.ValidateSkuValue(skuQueryResult, testName.Substring(0, 40)), Is.True);
            var listActivityValues = await _mongoHandler.SendMongoQueryActivityAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber",
                planCreateResponse.InstallmentPlanNumber, "Activity");
            if (listActivityValues.Count is not (2 or 1))
                Assert.Fail("listActivityValues.Count has wrong value");
            Assert.That(
                _mongoHandler.ValidateActivityValueExistence(listActivityValues, _skuDict![testName.Substring(0, 40)]),
                Is.True);
            Console.WriteLine("Done TestValidate_MER_FUN_STL_BIW_FTR_CRC_F20");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidate_MER_FUN_STL_BIW_FTR_CRC_F20 \n" + exception + "\n");
        }
    }

    [TestCase(Category = "SkuFunBiwTests")]
    [Test(Description = "TestValidate_MER_FUN_STL_BIW_FTR_DRC_F21 with generated merchant"), CancelAfter(120 * 1000)]
    public async Task TestValidate_MER_FUN_STL_BIW_FTR_DRC_F21()
    {
        try
        {
            Console.WriteLine("\nStarting TestValidate_MER_FUN_STL_BIW_FTR_DRC_F21");
            var testName = TestContext.CurrentContext.Test.Name;
            var terminalApiKey = _envConfig.MER_FUN_STL_BIW_FTR;
            
            
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            createPlanDefaultValues.paymentMethod.card.cardNumber = Environment.GetEnvironmentVariable("DebitCard")!;
            var planCreateResponse = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!, 
                _requestHeader!, "PendingCapture", 21, terminalApiKey, createPlanDefaultValues);
            var skuQueryResult = await _mongoHandler.SendMongoQueryForSkuAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", planCreateResponse.InstallmentPlanNumber, "Sku");
            Assert.That(_skuTestsData.ValidateSkuValue(skuQueryResult, testName.Substring(0, 40)), Is.True);
            var listActivityValues = await _mongoHandler.SendMongoQueryActivityAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber",
                planCreateResponse.InstallmentPlanNumber, "Activity");
            if (listActivityValues.Count is not (2 or 1))
                Assert.Fail("listActivityValues.Count has wrong value");
            Assert.That(
                _mongoHandler.ValidateActivityValueExistence(listActivityValues, _skuDict![testName.Substring(0, 40)]),
                Is.True);
            Console.WriteLine("Done TestValidate_MER_FUN_STL_BIW_FTR_DRC_F21");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidate_MER_FUN_STL_BIW_FTR_DRC_F21 \n" + exception + "\n");
        }
    }

    [TestCase(Category = "SkuFunBiwTests")]
    [Test(Description = "TestValidate_MER_FUN_STL_BIW_FTR_CRC_F21 with generated merchant"), CancelAfter(120 * 1000)]
    public async Task TestValidate_MER_FUN_STL_BIW_FTR_CRC_F21()
    {
        try
        {
            Console.WriteLine("\nStarting TestValidate_MER_FUN_STL_BIW_FTR_CRC_F21");
            var testName = TestContext.CurrentContext.Test.Name;
            var terminalApiKey = _envConfig.MER_FUN_STL_BIW_FTR;
            
            
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            var planCreateResponse = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!, 
                _requestHeader!, "Active", 21, terminalApiKey, createPlanDefaultValues);
            var skuQueryResult = await _mongoHandler.SendMongoQueryForSkuAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", planCreateResponse.InstallmentPlanNumber, "Sku");
            Assert.That(_skuTestsData.ValidateSkuValue(skuQueryResult, testName.Substring(0, 40)), Is.True);
            var listActivityValues = await _mongoHandler.SendMongoQueryActivityAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber",
                planCreateResponse.InstallmentPlanNumber, "Activity");
            if (listActivityValues.Count is not (2 or 1))
                Assert.Fail("listActivityValues.Count has wrong value");
            Assert.That(
                _mongoHandler.ValidateActivityValueExistence(listActivityValues, _skuDict![testName.Substring(0, 40)]),
                Is.True);
            Console.WriteLine("Done TestValidate_MER_FUN_STL_BIW_FTR_CRC_F21");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidate_MER_FUN_STL_BIW_FTR_CRC_F21 \n" + exception + "\n");
        }
    }


    [TestCase(Category = "SkuFunBIWTests")]
    [Test(Description = "TestValidate_MER_FUN_STL_BIW_FTR_DRC_F22 with generated merchant"), CancelAfter(120 * 1000)]
    public async Task TestValidate_MER_FUN_STL_BIW_FTR_DRC_F22()
    {
        try
        {
            Console.WriteLine("\nStarting TestValidate_MER_FUN_STL_BIW_FTR_DRC_F22");
            var testName = TestContext.CurrentContext.Test.Name;
            var terminalApiKey = _envConfig.MER_FUN_STL_BIW_FTR;
            
            
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            createPlanDefaultValues.paymentMethod.card.cardNumber = Environment.GetEnvironmentVariable("DebitCard")!;
            var planCreateResponse = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!, 
                _requestHeader!, "PendingCapture", 22, terminalApiKey, createPlanDefaultValues);
            var skuQueryResult = await _mongoHandler.SendMongoQueryForSkuAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", planCreateResponse.InstallmentPlanNumber, "Sku");
            Assert.That(_skuTestsData.ValidateSkuValue(skuQueryResult, testName.Substring(0, 40)), Is.True);
            var listActivityValues = await _mongoHandler.SendMongoQueryActivityAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber",
                planCreateResponse.InstallmentPlanNumber, "Activity");
            if (listActivityValues.Count is not (2 or 1))
                Assert.Fail("listActivityValues.Count has wrong value");
            Assert.That(
                _mongoHandler.ValidateActivityValueExistence(listActivityValues, _skuDict![testName.Substring(0, 40)]),
                Is.True);
            Console.WriteLine("Done TestValidate_MER_FUN_STL_BIW_FTR_DRC_F22");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidate_MER_FUN_STL_BIW_FTR_DRC_F22 \n" + exception + "\n");
        }
    }

    [TestCase(Category = "SkuFunBIWTests")]
    [Test(Description = "TestValidate_MER_FUN_STL_BIW_FTR_CRC_F22 with generated merchant"), CancelAfter(120 * 1000)]
    public async Task TestValidate_MER_FUN_STL_BIW_FTR_CRC_F22()
    {
        try
        {
            Console.WriteLine("\nStarting TestValidate_MER_FUN_STL_BIW_FTR_CRC_F22");
            var testName = TestContext.CurrentContext.Test.Name;
            var terminalApiKey = _envConfig.MER_FUN_STL_BIW_FTR;
            
            
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            var planCreateResponse = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!, 
                _requestHeader!, "Active", 22, terminalApiKey, createPlanDefaultValues);
            var skuQueryResult = await _mongoHandler.SendMongoQueryForSkuAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", planCreateResponse.InstallmentPlanNumber, "Sku");
            Assert.That(_skuTestsData.ValidateSkuValue(skuQueryResult, testName.Substring(0, 40)), Is.True);
            var listActivityValues = await _mongoHandler.SendMongoQueryActivityAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber",
                planCreateResponse.InstallmentPlanNumber, "Activity");
            if (listActivityValues.Count is not (2 or 1))
                Assert.Fail("listActivityValues.Count has wrong value");
            Assert.That(
                _mongoHandler.ValidateActivityValueExistence(listActivityValues, _skuDict![testName.Substring(0, 40)]),
                Is.True);
            Console.WriteLine("Done TestValidate_MER_FUN_STL_BIW_FTR_CRC_F22");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidate_MER_FUN_STL_BIW_FTR_CRC_F22 \n" + exception + "\n");
        }
    }

    [TestCase(Category = "SkuFunBIWTests")]
    [Test(Description = "TestValidate_MER_FUN_STL_BIW_FTR_DRC_F23 with generated merchant"), CancelAfter(120 * 1000)]
    public async Task TestValidate_MER_FUN_STL_BIW_FTR_DRC_F23()
    {
        try
        {
            Console.WriteLine("\nStarting TestValidate_MER_FUN_STL_BIW_FTR_DRC_F23");
            var testName = TestContext.CurrentContext.Test.Name;
            var terminalApiKey = _envConfig.MER_FUN_STL_BIW_FTR;
            
            
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            createPlanDefaultValues.paymentMethod.card.cardNumber = Environment.GetEnvironmentVariable("DebitCard")!;
            var planCreateResponse = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!, 
                _requestHeader!, "PendingCapture", 23, terminalApiKey, createPlanDefaultValues);
            var skuQueryResult = await _mongoHandler.SendMongoQueryForSkuAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", planCreateResponse.InstallmentPlanNumber, "Sku");
            Assert.That(_skuTestsData.ValidateSkuValue(skuQueryResult, testName.Substring(0, 40)), Is.True);
            var listActivityValues = await _mongoHandler.SendMongoQueryActivityAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber",
                planCreateResponse.InstallmentPlanNumber, "Activity");
            if (listActivityValues.Count is not (2 or 1))
                Assert.Fail("listActivityValues.Count has wrong value");
            Assert.That(
                _mongoHandler.ValidateActivityValueExistence(listActivityValues, _skuDict![testName.Substring(0, 40)]),
                Is.True);
            Console.WriteLine("Done TestValidate_MER_FUN_STL_BIW_FTR_DRC_F23");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidate_MER_FUN_STL_BIW_FTR_DRC_F23 \n" + exception + "\n");
        }
    }

    [TestCase(Category = "SkuFunBIWTests")]
    [Test(Description = "TestValidate_MER_FUN_STL_BIW_FTR_CRC_F23 with generated merchant"), CancelAfter(120 * 1000)]
    public async Task TestValidate_MER_FUN_STL_BIW_FTR_CRC_F23()
    {
        try
        {
            Console.WriteLine("\nStarting TestValidate_MER_FUN_STL_BIW_FTR_CRC_F23");
            var testName = TestContext.CurrentContext.Test.Name;
            var terminalApiKey = _envConfig.MER_FUN_STL_BIW_FTR;
            
            
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            var planCreateResponse = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!, 
                _requestHeader!, "Active", 23, terminalApiKey, createPlanDefaultValues);
            var skuQueryResult = await _mongoHandler.SendMongoQueryForSkuAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", planCreateResponse.InstallmentPlanNumber, "Sku");
            Assert.That(_skuTestsData.ValidateSkuValue(skuQueryResult, testName.Substring(0, 40)), Is.True);
            var listActivityValues = await _mongoHandler.SendMongoQueryActivityAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber",
                planCreateResponse.InstallmentPlanNumber, "Activity");
            if (listActivityValues.Count is not (2 or 1))
                Assert.Fail("listActivityValues.Count has wrong value");
            Assert.That(
                _mongoHandler.ValidateActivityValueExistence(listActivityValues, _skuDict![testName.Substring(0, 40)]),
                Is.True);
            Console.WriteLine("Done TestValidate_MER_FUN_STL_BIW_FTR_CRC_F23");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidate_MER_FUN_STL_BIW_FTR_CRC_F23 \n" + exception + "\n");
        }
    }

    [TestCase(Category = "SkuFunBiwTests")]
    [Test(Description = "TestValidate_MER_FUN_STL_BIW_FTR_DRC_F24 with generated merchant"), CancelAfter(120 * 1000)]
    public async Task TestValidate_MER_FUN_STL_BIW_FTR_DRC_F24()
    {
        try
        {
            Console.WriteLine("\nStarting TestValidate_MER_FUN_STL_BIW_FTR_DRC_F24");
            var testName = TestContext.CurrentContext.Test.Name;
            var terminalApiKey = _envConfig.MER_FUN_STL_BIW_FTR;
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            createPlanDefaultValues.paymentMethod.card.cardNumber = Environment.GetEnvironmentVariable("DebitCard")!;
            var planCreateResponse = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!, 
                _requestHeader!, "PendingCapture", 24, terminalApiKey, createPlanDefaultValues);
            var skuQueryResult = await _mongoHandler.SendMongoQueryForSkuAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", planCreateResponse.InstallmentPlanNumber, "Sku");
            Assert.That(_skuTestsData.ValidateSkuValue(skuQueryResult, testName.Substring(0, 40)), Is.True);
            var listActivityValues = await _mongoHandler.SendMongoQueryActivityAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber",
                planCreateResponse.InstallmentPlanNumber, "Activity");
            if (listActivityValues.Count is not (2 or 1))
                Assert.Fail("listActivityValues.Count has wrong value");
            Assert.That(
                _mongoHandler.ValidateActivityValueExistence(listActivityValues, _skuDict![testName.Substring(0, 40)]),
                Is.True);
            Console.WriteLine("Done TestValidate_MER_FUN_STL_BIW_FTR_DRC_F24");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidate_MER_FUN_STL_BIW_FTR_DRC_F24 \n" + exception + "\n");
        }
    }

    [TestCase(Category = "SkuFunBiwTests")]
    [Test(Description = "TestValidate_DBS_FUN_STL_BIW_FTR_DRC_F03 with generated merchant"), CancelAfter(120 * 1000)]
    public async Task TestValidate_DBS_FUN_STL_BIW_FTR_DRC_F03()
    {
        try
        {
            Console.WriteLine("\nStarting TestValidate_DBS_FUN_STL_BIW_FTR_DRC_F03");
            var testName = TestContext.CurrentContext.Test.Name;
            var terminalApiKey = _envConfig.DBS_FUN_STL_BIW_FTR;
            
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            createPlanDefaultValues.paymentMethod.card.cardNumber = Environment.GetEnvironmentVariable("DebitCard")!;
            var planCreateResponse = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!, 
                _requestHeader!, "PendingCapture", 3, terminalApiKey, createPlanDefaultValues);
            var skuQueryResult = await _mongoHandler.SendMongoQueryForSkuAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", planCreateResponse.InstallmentPlanNumber, "Sku");
            Assert.That(_skuTestsData.ValidateSkuValue(skuQueryResult, testName.Substring(0, 40)), Is.True);
            var listActivityValues = await _mongoHandler.SendMongoQueryActivityAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber",
                planCreateResponse.InstallmentPlanNumber, "Activity");
            if (listActivityValues.Count is not (2 or 1))
                Assert.Fail("listActivityValues.Count has wrong value");
            Assert.That(
                _mongoHandler.ValidateActivityValueExistence(listActivityValues, _skuDict![testName.Substring(0, 40)]),
                Is.True);
            Console.WriteLine("Done TestValidate_DBS_FUN_STL_BIW_FTR_DRC_F03");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidate_DBS_FUN_STL_BIW_FTR_DRC_F03 \n" + exception + "\n");
        }
    }

    [TestCase(Category = "SkuFunBiwTests")]
    [Test(Description = "TestValidate_DBS_FUN_STL_BIW_FTR_CRC_F03 with generated merchant"), CancelAfter(120 * 1000)]
    public async Task TestValidate_DBS_FUN_STL_BIW_FTR_CRC_F03()
    {
        try
        {
            Console.WriteLine("\nStarting TestValidate_DBS_FUN_STL_BIW_FTR_CRC_F03");
            var testName = TestContext.CurrentContext.Test.Name;
            var terminalApiKey = _envConfig.DBS_FUN_STL_BIW_FTR;
            
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            var planCreateResponse = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!, 
                _requestHeader!, "Active", 3, terminalApiKey, createPlanDefaultValues);
            var skuQueryResult = await _mongoHandler.SendMongoQueryForSkuAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", planCreateResponse.InstallmentPlanNumber, "Sku");
            Assert.That(_skuTestsData.ValidateSkuValue(skuQueryResult, testName.Substring(0, 40)), Is.True);
            var listActivityValues = await _mongoHandler.SendMongoQueryActivityAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber",
                planCreateResponse.InstallmentPlanNumber, "Activity");
            if (listActivityValues.Count is not (2 or 1))
                Assert.Fail("listActivityValues.Count has wrong value");
            Assert.That(
                _mongoHandler.ValidateActivityValueExistence(listActivityValues, _skuDict![testName.Substring(0, 40)]),
                Is.True);
            Console.WriteLine("Done TestValidate_DBS_FUN_STL_BIW_FTR_CRC_F03");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidate_DBS_FUN_STL_BIW_FTR_CRC_F03 \n" + exception + "\n");
        }
    }

    [TestCase(Category = "SkuFunBiwTests")]
    [Test(Description = "TestValidate_DBS_FUN_STL_BIW_FTR_DRC_F06 with generated merchant"), CancelAfter(120 * 1000)]
    public async Task TestValidate_DBS_FUN_STL_BIW_FTR_DRC_F06()
    {
        try
        {
            Console.WriteLine("\nStarting TestValidate_DBS_FUN_STL_BIW_FTR_DRC_F06");
            var testName = TestContext.CurrentContext.Test.Name;
            var terminalApiKey = _envConfig.DBS_FUN_STL_BIW_FTR;
            
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            createPlanDefaultValues.paymentMethod.card.cardNumber = Environment.GetEnvironmentVariable("DebitCard")!;
            var planCreateResponse = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!, 
                _requestHeader!, "PendingCapture", 6, terminalApiKey, createPlanDefaultValues);
            var skuQueryResult = await _mongoHandler.SendMongoQueryForSkuAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", planCreateResponse.InstallmentPlanNumber, "Sku");
            Assert.That(_skuTestsData.ValidateSkuValue(skuQueryResult, testName.Substring(0, 40)), Is.True);
            var listActivityValues = await _mongoHandler.SendMongoQueryActivityAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber",
                planCreateResponse.InstallmentPlanNumber, "Activity");
            if (listActivityValues.Count is not (2 or 1))
                Assert.Fail("listActivityValues.Count has wrong value");
            Assert.That(
                _mongoHandler.ValidateActivityValueExistence(listActivityValues, _skuDict![testName.Substring(0, 40)]),
                Is.True);
            Console.WriteLine("Done TestValidate_DBS_FUN_STL_BIW_FTR_DRC_F06");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidate_DBS_FUN_STL_BIW_FTR_DRC_F06 \n" + exception + "\n");
        }
    }

    [TestCase(Category = "SkuFunBiwTests")]
    [Test(Description = "TestValidate_DBS_FUN_STL_BIW_FTR_CRC_F06 with generated merchant"), CancelAfter(120 * 1000)]
    public async Task TestValidate_DBS_FUN_STL_BIW_FTR_CRC_F06()
    {
        try
        {
            Console.WriteLine("\nStarting TestValidate_DBS_FUN_STL_BIW_FTR_CRC_F06");
            var testName = TestContext.CurrentContext.Test.Name;
            var terminalApiKey = _envConfig.DBS_FUN_STL_BIW_FTR;
            
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            var planCreateResponse = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!, 
                _requestHeader!, "Active", 6, terminalApiKey, createPlanDefaultValues);
            var skuQueryResult = await _mongoHandler.SendMongoQueryForSkuAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", planCreateResponse.InstallmentPlanNumber, "Sku");
            Assert.That(_skuTestsData.ValidateSkuValue(skuQueryResult, testName.Substring(0, 40)), Is.True);
            var listActivityValues = await _mongoHandler.SendMongoQueryActivityAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", planCreateResponse.InstallmentPlanNumber, "Activity");
            if (listActivityValues.Count is not (2 or 1))
                Assert.Fail("listActivityValues.Count has wrong value");
            Assert.That(
                _mongoHandler.ValidateActivityValueExistence(listActivityValues, _skuDict![testName.Substring(0, 40)]),
                Is.True);
            Console.WriteLine("Done TestValidate_DBS_FUN_STL_BIW_FTR_CRC_F06");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidate_DBS_FUN_STL_BIW_FTR_CRC_F06 \n" + exception + "\n");
        }
    }

    [TestCase(Category = "SkuFunBiwTests")]
    [Test(Description = "TestValidate_DBS_FUN_STL_BIW_FTR_DRC_F09 with generated merchant"), CancelAfter(120 * 1000)]
    public async Task TestValidate_DBS_FUN_STL_BIW_FTR_DRC_F09()
    {
        try
        {
            Console.WriteLine("\nStarting TestValidate_DBS_FUN_STL_BIW_FTR_DRC_F09");
            var testName = TestContext.CurrentContext.Test.Name;
            var terminalApiKey = _envConfig.DBS_FUN_STL_BIW_FTR;
            
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            createPlanDefaultValues.paymentMethod.card.cardNumber = Environment.GetEnvironmentVariable("DebitCard")!;
            var planCreateResponse = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!, 
                _requestHeader!, "PendingCapture", 9, terminalApiKey, createPlanDefaultValues);
            var skuQueryResult = await _mongoHandler.SendMongoQueryForSkuAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", planCreateResponse.InstallmentPlanNumber, "Sku");
            Assert.That(_skuTestsData.ValidateSkuValue(skuQueryResult, testName.Substring(0, 40)), Is.True);
            var listActivityValues = await _mongoHandler.SendMongoQueryActivityAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber",
                planCreateResponse.InstallmentPlanNumber, "Activity");
            if (listActivityValues.Count is not (2 or 1))
                Assert.Fail("listActivityValues.Count has wrong value");
            Assert.That(
                _mongoHandler.ValidateActivityValueExistence(listActivityValues, _skuDict![testName.Substring(0, 40)]),
                Is.True);
            Console.WriteLine("Done TestValidate_DBS_FUN_STL_BIW_FTR_DRC_F09");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidate_DBS_FUN_STL_BIW_FTR_DRC_F09 \n" + exception + "\n");
        }
    }

    [TestCase(Category = "SkuFunBiwTests")]
    [Test(Description = "TestValidate_DBS_FUN_STL_BIW_FTR_CRC_F09 with generated merchant"), CancelAfter(120 * 1000)]
    public async Task TestValidate_DBS_FUN_STL_BIW_FTR_CRC_F09()
    {
        try
        {
            Console.WriteLine("\nStarting TestValidate_DBS_FUN_STL_BIW_FTR_CRC_F09");
            var testName = TestContext.CurrentContext.Test.Name;
            var terminalApiKey = _envConfig.DBS_FUN_STL_BIW_FTR;
            
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            var planCreateResponse = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!, 
                _requestHeader!, "Active", 9, terminalApiKey, createPlanDefaultValues);
            var skuQueryResult = await _mongoHandler.SendMongoQueryForSkuAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", planCreateResponse.InstallmentPlanNumber, "Sku");
            Assert.That(_skuTestsData.ValidateSkuValue(skuQueryResult, testName.Substring(0, 40)), Is.True);
            var listActivityValues = await _mongoHandler.SendMongoQueryActivityAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber",
                planCreateResponse.InstallmentPlanNumber, "Activity");
            if (listActivityValues.Count is not (2 or 1))
                Assert.Fail("listActivityValues.Count has wrong value");
            Assert.That(
                _mongoHandler.ValidateActivityValueExistence(listActivityValues, _skuDict![testName.Substring(0, 40)]),
                Is.True);
            Console.WriteLine("Done TestValidate_DBS_FUN_STL_BIW_FTR_CRC_F09");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidate_DBS_FUN_STL_BIW_FTR_CRC_F09 \n" + exception + "\n");
        }
    }

    [TestCase(Category = "SkuFunBiwTests")]
    [Test(Description = "TestValidate_DBS_FUN_STL_BIW_FTR_DRC_F12 with generated merchant"), CancelAfter(120 * 1000)]
    public async Task TestValidate_DBS_FUN_STL_BIW_FTR_DRC_F12()
    {
        try
        {
            Console.WriteLine("\nStarting TestValidate_DBS_FUN_STL_BIW_FTR_DRC_F12");
            var testName = TestContext.CurrentContext.Test.Name;
            var terminalApiKey = _envConfig.DBS_FUN_STL_BIW_FTR;
            
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            createPlanDefaultValues.paymentMethod.card.cardNumber = Environment.GetEnvironmentVariable("DebitCard")!;
            var planCreateResponse = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!, 
                _requestHeader!, "PendingCapture", 12, terminalApiKey, createPlanDefaultValues);
            var skuQueryResult = await _mongoHandler.SendMongoQueryForSkuAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", planCreateResponse.InstallmentPlanNumber, "Sku");
            Assert.That(_skuTestsData.ValidateSkuValue(skuQueryResult, testName.Substring(0, 40)), Is.True);
            var listActivityValues = await _mongoHandler.SendMongoQueryActivityAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber",
                planCreateResponse.InstallmentPlanNumber, "Activity");
            if (listActivityValues.Count is not (2 or 1))
                Assert.Fail("listActivityValues.Count has wrong value");
            Assert.That(
                _mongoHandler.ValidateActivityValueExistence(listActivityValues, _skuDict![testName.Substring(0, 40)]),
                Is.True);
            Console.WriteLine("Done TestValidate_DBS_FUN_STL_BIW_FTR_DRC_F12");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidate_DBS_FUN_STL_BIW_FTR_DRC_F12 \n" + exception + "\n");
        }
    }

    [TestCase(Category = "SkuFunBiwTests")]
    [Test(Description = "TestValidate_DBS_FUN_STL_BIW_FTR_CRC_F12 with generated merchant"), CancelAfter(120 * 1000)]
    public async Task TestValidate_DBS_FUN_STL_BIW_FTR_CRC_F12()
    {
        try
        {
            Console.WriteLine("\nStarting TestValidate_DBS_FUN_STL_BIW_FTR_CRC_F12");
            var testName = TestContext.CurrentContext.Test.Name;
            var terminalApiKey = _envConfig.DBS_FUN_STL_BIW_FTR;
            
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            var planCreateResponse = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!, 
                _requestHeader!, "Active", 12, terminalApiKey, createPlanDefaultValues);
            var skuQueryResult = await _mongoHandler.SendMongoQueryForSkuAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", planCreateResponse.InstallmentPlanNumber, "Sku");
            Assert.That(_skuTestsData.ValidateSkuValue(skuQueryResult, testName.Substring(0, 40)), Is.True);
            var listActivityValues = await _mongoHandler.SendMongoQueryActivityAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber",
                planCreateResponse.InstallmentPlanNumber, "Activity");
            if (listActivityValues.Count is not (2 or 1))
                Assert.Fail("listActivityValues.Count has wrong value");
            Assert.That(
                _mongoHandler.ValidateActivityValueExistence(listActivityValues, _skuDict![testName.Substring(0, 40)]),
                Is.True);
            Console.WriteLine("Done TestValidate_DBS_FUN_STL_BIW_FTR_CRC_F12");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidate_DBS_FUN_STL_BIW_FTR_CRC_F12 \n" + exception + "\n");
        }
    }

    [TestCase(Category = "SkuFunBiwTests")]
    [Test(Description = "TestValidate_DBS_FUN_STL_BIW_FTR_DRC_F15 with generated merchant"), CancelAfter(120 * 1000)]
    public async Task TestValidate_DBS_FUN_STL_BIW_FTR_DRC_F15()
    {
        try
        {
            Console.WriteLine("\nStarting TestValidate_DBS_FUN_STL_BIW_FTR_DRC_F15");
            var testName = TestContext.CurrentContext.Test.Name;
            var terminalApiKey = _envConfig.DBS_FUN_STL_BIW_FTR;
            
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            createPlanDefaultValues.paymentMethod.card.cardNumber = Environment.GetEnvironmentVariable("DebitCard")!;
            var planCreateResponse = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!, 
                _requestHeader!, "PendingCapture", 15, terminalApiKey, createPlanDefaultValues);
            var skuQueryResult = await _mongoHandler.SendMongoQueryForSkuAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", planCreateResponse.InstallmentPlanNumber, "Sku");
            Assert.That(_skuTestsData.ValidateSkuValue(skuQueryResult, testName.Substring(0, 40)), Is.True);
            var listActivityValues = await _mongoHandler.SendMongoQueryActivityAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber",
                planCreateResponse.InstallmentPlanNumber, "Activity");
            if (listActivityValues.Count is not (2 or 1))
                Assert.Fail("listActivityValues.Count has wrong value");
            Assert.That(
                _mongoHandler.ValidateActivityValueExistence(listActivityValues, _skuDict![testName.Substring(0, 40)]),
                Is.True);
            Console.WriteLine("Done TestValidate_DBS_FUN_STL_BIW_FTR_DRC_F15");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidate_DBS_FUN_STL_BIW_FTR_DRC_F15 \n" + exception + "\n");
        }
    }

    [TestCase(Category = "SkuFunBiwTests")]
    [Test(Description = "TestValidate_DBS_FUN_STL_BIW_FTR_CRC_F15 with generated merchant"), CancelAfter(120 * 1000)]
    public async Task TestValidate_DBS_FUN_STL_BIW_FTR_CRC_F15()
    {
        try
        {
            Console.WriteLine("\nStarting TestValidate_DBS_FUN_STL_BIW_FTR_CRC_F15");
            var testName = TestContext.CurrentContext.Test.Name;
            var terminalApiKey = _envConfig.DBS_FUN_STL_BIW_FTR;
            
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            var planCreateResponse = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!, 
                _requestHeader!, "Active", 15, terminalApiKey, createPlanDefaultValues);
            var skuQueryResult = await _mongoHandler.SendMongoQueryForSkuAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", planCreateResponse.InstallmentPlanNumber, "Sku");
            Assert.That(_skuTestsData.ValidateSkuValue(skuQueryResult, testName.Substring(0, 40)), Is.True);
            var listActivityValues = await _mongoHandler.SendMongoQueryActivityAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber",
                planCreateResponse.InstallmentPlanNumber, "Activity");
            if (listActivityValues.Count is not (2 or 1))
                Assert.Fail("listActivityValues.Count has wrong value");
            Assert.That(
                _mongoHandler.ValidateActivityValueExistence(listActivityValues, _skuDict![testName.Substring(0, 40)]),
                Is.True);
            Console.WriteLine("Done TestValidate_DBS_FUN_STL_BIW_FTR_CRC_F15");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidate_DBS_FUN_STL_BIW_FTR_CRC_F15 \n" + exception + "\n");
        }
    }

    [TestCase(Category = "SkuFunBiwTests")]
    [Test(Description = "TestValidate_DBS_FUN_STL_BIW_FTR_DRC_F18 with generated merchant"), CancelAfter(120 * 1000)]
    public async Task TestValidate_DBS_FUN_STL_BIW_FTR_DRC_F18()
    {
        try
        {
            Console.WriteLine("\nStarting TestValidate_DBS_FUN_STL_BIW_FTR_DRC_F18");
            var testName = TestContext.CurrentContext.Test.Name;
            var terminalApiKey = _envConfig.DBS_FUN_STL_BIW_FTR;
            
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            createPlanDefaultValues.paymentMethod.card.cardNumber = Environment.GetEnvironmentVariable("DebitCard")!;
            var planCreateResponse = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!, 
                _requestHeader!, "PendingCapture", 18, terminalApiKey, createPlanDefaultValues);
            var skuQueryResult = await _mongoHandler.SendMongoQueryForSkuAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", planCreateResponse.InstallmentPlanNumber, "Sku");
            Assert.That(_skuTestsData.ValidateSkuValue(skuQueryResult, testName.Substring(0, 40)), Is.True);
            var listActivityValues = await _mongoHandler.SendMongoQueryActivityAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber",
                planCreateResponse.InstallmentPlanNumber, "Activity");
            if (listActivityValues.Count is not (2 or 1))
                Assert.Fail("listActivityValues.Count has wrong value");
            Assert.That(
                _mongoHandler.ValidateActivityValueExistence(listActivityValues, _skuDict![testName.Substring(0, 40)]),
                Is.True);
            Console.WriteLine("Done TestValidate_DBS_FUN_STL_BIW_FTR_DRC_F18");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidate_DBS_FUN_STL_BIW_FTR_DRC_F18 \n" + exception + "\n");
        }
    }

    [TestCase(Category = "SkuFunBiwTests")]
    [Test(Description = "TestValidate_DBS_FUN_STL_BIW_FTR_CRC_F18 with generated merchant"), CancelAfter(120 * 1000)]
    public async Task TestValidate_DBS_FUN_STL_BIW_FTR_CRC_F18()
    {
        try
        {
            Console.WriteLine("\nStarting TestValidate_DBS_FUN_STL_BIW_FTR_CRC_F18");
            var testName = TestContext.CurrentContext.Test.Name;
            var terminalApiKey = _envConfig.DBS_FUN_STL_BIW_FTR;
            
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            var planCreateResponse = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!, 
                _requestHeader!, "Active", 18, terminalApiKey, createPlanDefaultValues);

            var skuQueryResult = await _mongoHandler.SendMongoQueryForSkuAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", planCreateResponse.InstallmentPlanNumber, "Sku");
            Assert.That(_skuTestsData.ValidateSkuValue(skuQueryResult, testName.Substring(0, 40)), Is.True);
            var listActivityValues = await _mongoHandler.SendMongoQueryActivityAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber",
                planCreateResponse.InstallmentPlanNumber, "Activity");
            if (listActivityValues.Count is not (2 or 1))
                Assert.Fail("listActivityValues.Count has wrong value");
            Assert.That(
                _mongoHandler.ValidateActivityValueExistence(listActivityValues, _skuDict![testName.Substring(0, 40)]),
                Is.True);
            Console.WriteLine("Done TestValidate_DBS_FUN_STL_BIW_FTR_CRC_F18");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidate_DBS_FUN_STL_BIW_FTR_CRC_F18 \n" + exception + "\n");
        }
    }

    [TestCase(Category = "SkuFunBiwTests")]
    [Test(Description = "TestValidate_DBS_FUN_STL_BIW_FTR_DRC_F21 with generated merchant"), CancelAfter(120 * 1000)]
    public async Task TestValidate_DBS_FUN_STL_BIW_FTR_DRC_F21()
    {
        try
        {
            Console.WriteLine("\nStarting TestValidate_DBS_FUN_STL_BIW_FTR_DRC_F21");
            var testName = TestContext.CurrentContext.Test.Name;
            var terminalApiKey = _envConfig.DBS_FUN_STL_BIW_FTR;
            
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            createPlanDefaultValues.paymentMethod.card.cardNumber = Environment.GetEnvironmentVariable("DebitCard")!;
            var planCreateResponse = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!, 
                _requestHeader!, "PendingCapture", 21, terminalApiKey, createPlanDefaultValues);
            var skuQueryResult = await _mongoHandler.SendMongoQueryForSkuAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", planCreateResponse.InstallmentPlanNumber, "Sku");
            Assert.That(_skuTestsData.ValidateSkuValue(skuQueryResult, testName.Substring(0, 40)), Is.True);
            var listActivityValues = await _mongoHandler.SendMongoQueryActivityAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber",
                planCreateResponse.InstallmentPlanNumber, "Activity");
            if (listActivityValues.Count is not (2 or 1))
                Assert.Fail("listActivityValues.Count has wrong value");
            Assert.That(
                _mongoHandler.ValidateActivityValueExistence(listActivityValues, _skuDict![testName.Substring(0, 40)]),
                Is.True);
            Console.WriteLine("Done TestValidate_DBS_FUN_STL_BIW_FTR_DRC_F21");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidate_DBS_FUN_STL_BIW_FTR_DRC_F21 \n" + exception + "\n");
        }
    }

    [TestCase(Category = "SkuFunBiwTests")]
    [Test(Description = "TestValidate_DBS_FUN_STL_BIW_FTR_CRC_F21 with generated merchant"), CancelAfter(120 * 1000)]
    public async Task TestValidate_DBS_FUN_STL_BIW_FTR_CRC_F21()
    {
        try
        {
            Console.WriteLine("\nStarting TestValidate_DBS_FUN_STL_BIW_FTR_CRC_F21");
            var testName = TestContext.CurrentContext.Test.Name;
            var terminalApiKey = _envConfig.DBS_FUN_STL_BIW_FTR;
            
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            var planCreateResponse = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!, 
                _requestHeader!, "Active", 21, terminalApiKey, createPlanDefaultValues);
            var skuQueryResult = await _mongoHandler.SendMongoQueryForSkuAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", planCreateResponse.InstallmentPlanNumber, "Sku");
            Assert.That(_skuTestsData.ValidateSkuValue(skuQueryResult, testName.Substring(0, 40)), Is.True);
            var listActivityValues = await _mongoHandler.SendMongoQueryActivityAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber",
                planCreateResponse.InstallmentPlanNumber, "Activity");
            if (listActivityValues.Count is not (2 or 1))
                Assert.Fail("listActivityValues.Count has wrong value");
            Assert.That(
                _mongoHandler.ValidateActivityValueExistence(listActivityValues, _skuDict![testName.Substring(0, 40)]),
                Is.True);
            Console.WriteLine("Done TestValidate_DBS_FUN_STL_BIW_FTR_CRC_F21");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidate_DBS_FUN_STL_BIW_FTR_CRC_F21 \n" + exception + "\n");
        }
    }

    [TestCase(Category = "SkuFunBiwTests")]
    [Test(Description = "TestValidate_DBS_FUN_STL_BIW_FTR_DRC_F24 with generated merchant"), CancelAfter(120 * 1000)]
    public async Task TestValidate_DBS_FUN_STL_BIW_FTR_DRC_F24()
    {
        try
        {
            Console.WriteLine("\nStarting TestValidate_DBS_FUN_STL_BIW_FTR_DRC_F24");
            var testName = TestContext.CurrentContext.Test.Name;
            var terminalApiKey = _envConfig.DBS_FUN_STL_BIW_FTR;
            
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            createPlanDefaultValues.paymentMethod.card.cardNumber = Environment.GetEnvironmentVariable("DebitCard")!;
            var planCreateResponse = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!, 
                _requestHeader!, "PendingCapture", 24, terminalApiKey, createPlanDefaultValues);
            var skuQueryResult = await _mongoHandler.SendMongoQueryForSkuAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", planCreateResponse.InstallmentPlanNumber, "Sku");
            Assert.That(_skuTestsData.ValidateSkuValue(skuQueryResult, testName.Substring(0, 40)), Is.True);
            var listActivityValues = await _mongoHandler.SendMongoQueryActivityAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber",
                planCreateResponse.InstallmentPlanNumber, "Activity");
            if (listActivityValues.Count is not (2 or 1))
                Assert.Fail("listActivityValues.Count has wrong value");
            Assert.That(
                _mongoHandler.ValidateActivityValueExistence(listActivityValues, _skuDict![testName.Substring(0, 40)]),
                Is.True);
            Console.WriteLine("Done TestValidate_DBS_FUN_STL_BIW_FTR_DRC_F24");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidate_DBS_FUN_STL_BIW_FTR_DRC_F24 \n" + exception + "\n");
        }
    }

    [TestCase(Category = "SkuFunBiwTests")]
    [Test(Description = "TestValidate_DBS_FUN_STL_BIW_FTR_CRC_F24 with generated merchant"), CancelAfter(120 * 1000)]
    public async Task TestValidate_DBS_FUN_STL_BIW_FTR_CRC_F24()
    {
        try
        {
            Console.WriteLine("\nStarting TestValidate_DBS_FUN_STL_BIW_FTR_CRC_F24");
            var testName = TestContext.CurrentContext.Test.Name;
            var terminalApiKey = _envConfig.DBS_FUN_STL_BIW_FTR;
            
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            var planCreateResponse = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!, 
                _requestHeader!, "Active", 24, terminalApiKey, createPlanDefaultValues);
            var skuQueryResult = await _mongoHandler.SendMongoQueryForSkuAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", planCreateResponse.InstallmentPlanNumber, "Sku");
            Assert.That(_skuTestsData.ValidateSkuValue(skuQueryResult, testName.Substring(0, 40)), Is.True);
            var listActivityValues = await _mongoHandler.SendMongoQueryActivityAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber",
                planCreateResponse.InstallmentPlanNumber, "Activity");
            if (listActivityValues.Count is not (2 or 1))
                Assert.Fail("listActivityValues.Count has wrong value");
            Assert.That(
                _mongoHandler.ValidateActivityValueExistence(listActivityValues, _skuDict![testName.Substring(0, 40)]),
                Is.True);
            Console.WriteLine("Done TestValidate_DBS_FUN_STL_BIW_FTR_CRC_F24");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidate_DBS_FUN_STL_BIW_FTR_CRC_F24 \n" + exception + "\n");
        }
    }

    [TestCase(Category = "SkuFunBiwTests")]
    [Test(Description = "TestValidate_PCO_FUN_STL_BIW_FTR_DRC_F03 with generated merchant"), CancelAfter(120 * 1000)]
    public async Task TestValidate_PCO_FUN_STL_BIW_FTR_DRC_F03()
    {
        try
        {
            Console.WriteLine("\nStarting TestValidate_PCO_FUN_STL_BIW_FTR_DRC_F03");
            var testName = TestContext.CurrentContext.Test.Name;
            var terminalApiKey = _envConfig.PCO_FUN_STL_BIW_FTR;
            
            
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            createPlanDefaultValues.paymentMethod.card.cardNumber = Environment.GetEnvironmentVariable("DebitCard")!;
            var planCreateResponse = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!, 
                _requestHeader!, "PendingCapture", 3, terminalApiKey, createPlanDefaultValues);
            var skuQueryResult = await _mongoHandler.SendMongoQueryForSkuAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", planCreateResponse.InstallmentPlanNumber, "Sku");
            Assert.That(_skuTestsData.ValidateSkuValue(skuQueryResult, testName.Substring(0, 40)), Is.True);
            var listActivityValues = await _mongoHandler.SendMongoQueryActivityAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber",
                planCreateResponse.InstallmentPlanNumber, "Activity");
            if (listActivityValues.Count is not (2 or 1))
                Assert.Fail("listActivityValues.Count has wrong value");
            Assert.That(
                _mongoHandler.ValidateActivityValueExistence(listActivityValues, _skuDict![testName.Substring(0, 40)]),
                Is.True);
            Console.WriteLine("Done TestValidate_PCO_FUN_STL_BIW_FTR_DRC_F03");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidate_PCO_FUN_STL_BIW_FTR_DRC_F03 \n" + exception + "\n");
        }
    }

    [TestCase(Category = "SkuFunBiwTests")]
    [Test(Description = "TestValidate_PCO_FUN_STL_BIW_FTR_CRC_F03 with generated merchant"), CancelAfter(120 * 1000)]
    public async Task TestValidate_PCO_FUN_STL_BIW_FTR_CRC_F03()
    {
        try
        {
            Console.WriteLine("\nStarting TestValidate_PCO_FUN_STL_BIW_FTR_CRC_F03");
            var testName = TestContext.CurrentContext.Test.Name;
            var terminalApiKey = _envConfig.PCO_FUN_STL_BIW_FTR;
            
            
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            var planCreateResponse = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!, 
                _requestHeader!, "Active", 3, terminalApiKey, createPlanDefaultValues);
            var skuQueryResult = await _mongoHandler.SendMongoQueryForSkuAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", planCreateResponse.InstallmentPlanNumber, "Sku");
            Assert.That(_skuTestsData.ValidateSkuValue(skuQueryResult, testName.Substring(0, 40)), Is.True);
            var listActivityValues = await _mongoHandler.SendMongoQueryActivityAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber",
                planCreateResponse.InstallmentPlanNumber, "Activity");
            if (listActivityValues.Count is not (2 or 1))
                Assert.Fail("listActivityValues.Count has wrong value");
            Assert.That(
                _mongoHandler.ValidateActivityValueExistence(listActivityValues, _skuDict![testName.Substring(0, 40)]),
                Is.True);
            Console.WriteLine("Done TestValidate_PCO_FUN_STL_BIW_FTR_CRC_F03");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidate_PCO_FUN_STL_BIW_FTR_CRC_F03 \n" + exception + "\n");
        }
    }

    [TestCase(Category = "SkuFunBiwTests")]
    [Test(Description = "TestValidate_PCO_FUN_STL_BIW_FTR_DRC_F06 with generated merchant"), CancelAfter(120 * 1000)]
    public async Task TestValidate_PCO_FUN_STL_BIW_FTR_DRC_F06()
    {
        try
        {
            Console.WriteLine("\nStarting TestValidate_PCO_FUN_STL_BIW_FTR_DRC_F06");
            var testName = TestContext.CurrentContext.Test.Name;
            var terminalApiKey = _envConfig.PCO_FUN_STL_BIW_FTR;
            
            
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            createPlanDefaultValues.paymentMethod.card.cardNumber = Environment.GetEnvironmentVariable("DebitCard")!;
            var planCreateResponse = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!, 
                _requestHeader!, "PendingCapture", 6, terminalApiKey, createPlanDefaultValues);
            var skuQueryResult = await _mongoHandler.SendMongoQueryForSkuAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", planCreateResponse.InstallmentPlanNumber, "Sku");
            Assert.That(_skuTestsData.ValidateSkuValue(skuQueryResult, testName.Substring(0, 40)), Is.True);
            var listActivityValues = await _mongoHandler.SendMongoQueryActivityAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber",
                planCreateResponse.InstallmentPlanNumber, "Activity");
            if (listActivityValues.Count is not (2 or 1))
                Assert.Fail("listActivityValues.Count has wrong value");
            Assert.That(
                _mongoHandler.ValidateActivityValueExistence(listActivityValues, _skuDict![testName.Substring(0, 40)]),
                Is.True);
            Console.WriteLine("Done TestValidate_PCO_FUN_STL_BIW_FTR_DRC_F06");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidate_PCO_FUN_STL_BIW_FTR_DRC_F06 \n" + exception + "\n");
        }
    }

    [TestCase(Category = "SkuFunBiwTests")]
    [Test(Description = "TestValidate_PCO_FUN_STL_BIW_FTR_CRC_F06 with generated merchant"), CancelAfter(120 * 1000)]
    public async Task TestValidate_PCO_FUN_STL_BIW_FTR_CRC_F06()
    {
        try
        {
            Console.WriteLine("\nStarting TestValidate_PCO_FUN_STL_BIW_FTR_CRC_F06");
            var testName = TestContext.CurrentContext.Test.Name;
            var terminalApiKey = _envConfig.PCO_FUN_STL_BIW_FTR;
            
            
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            var planCreateResponse = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!, 
                _requestHeader!, "Active", 6, terminalApiKey, createPlanDefaultValues);
            var skuQueryResult = await _mongoHandler.SendMongoQueryForSkuAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", planCreateResponse.InstallmentPlanNumber, "Sku");
            Assert.That(_skuTestsData.ValidateSkuValue(skuQueryResult, testName.Substring(0, 40)), Is.True);
            var listActivityValues = await _mongoHandler.SendMongoQueryActivityAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber",
                planCreateResponse.InstallmentPlanNumber, "Activity");
            if (listActivityValues.Count is not (2 or 1))
                Assert.Fail("listActivityValues.Count has wrong value");
            Assert.That(
                _mongoHandler.ValidateActivityValueExistence(listActivityValues, _skuDict![testName.Substring(0, 40)]),
                Is.True);
            Console.WriteLine("Done TestValidate_PCO_FUN_STL_BIW_FTR_CRC_F06");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidate_PCO_FUN_STL_BIW_FTR_CRC_F06 \n" + exception + "\n");
        }
    }

    [TestCase(Category = "SkuFunBiwTests")]
    [Test(Description = "TestValidate_PCO_FUN_STL_BIW_FTR_DRC_F09 with generated merchant"), CancelAfter(120 * 1000)]
    public async Task TestValidate_PCO_FUN_STL_BIW_FTR_DRC_F09()
    {
        try
        {
            Console.WriteLine("\nStarting TestValidate_PCO_FUN_STL_BIW_FTR_DRC_F09");
            var testName = TestContext.CurrentContext.Test.Name;
            var terminalApiKey = _envConfig.PCO_FUN_STL_BIW_FTR;
            
            
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            createPlanDefaultValues.paymentMethod.card.cardNumber = Environment.GetEnvironmentVariable("DebitCard")!;
            var planCreateResponse = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!, 
                _requestHeader!, "PendingCapture", 9, terminalApiKey, createPlanDefaultValues);
            var skuQueryResult = await _mongoHandler.SendMongoQueryForSkuAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", planCreateResponse.InstallmentPlanNumber, "Sku");
            Assert.That(_skuTestsData.ValidateSkuValue(skuQueryResult, testName.Substring(0, 40)), Is.True);
            var listActivityValues = await _mongoHandler.SendMongoQueryActivityAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber",
                planCreateResponse.InstallmentPlanNumber, "Activity");
            if (listActivityValues.Count is not (2 or 1))
                Assert.Fail("listActivityValues.Count has wrong value");
            Assert.That(
                _mongoHandler.ValidateActivityValueExistence(listActivityValues, _skuDict![testName.Substring(0, 40)]),
                Is.True);
            Console.WriteLine("Done TestValidate_PCO_FUN_STL_BIW_FTR_DRC_F09");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidate_PCO_FUN_STL_BIW_FTR_DRC_F09 \n" + exception + "\n");
        }
    }

    [TestCase(Category = "SkuFunBiwTests")]
    [Test(Description = "TestValidate_PCO_FUN_STL_BIW_FTR_CRC_F09 with generated merchant"), CancelAfter(120 * 1000)]
    public async Task TestValidate_PCO_FUN_STL_BIW_FTR_CRC_F09()
    {
        try
        {
            Console.WriteLine("\nStarting TestValidate_PCO_FUN_STL_BIW_FTR_CRC_F09");
            var testName = TestContext.CurrentContext.Test.Name;
            var terminalApiKey = _envConfig.PCO_FUN_STL_BIW_FTR;
            
            
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            var planCreateResponse = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!, 
                _requestHeader!, "Active", 9, terminalApiKey, createPlanDefaultValues);
            var skuQueryResult = await _mongoHandler.SendMongoQueryForSkuAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", planCreateResponse.InstallmentPlanNumber, "Sku");
            Assert.That(_skuTestsData.ValidateSkuValue(skuQueryResult, testName.Substring(0, 40)), Is.True);
            var listActivityValues = await _mongoHandler.SendMongoQueryActivityAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber",
                planCreateResponse.InstallmentPlanNumber, "Activity");
            if (listActivityValues.Count is not (2 or 1))
                Assert.Fail("listActivityValues.Count has wrong value");
            Assert.That(
                _mongoHandler.ValidateActivityValueExistence(listActivityValues, _skuDict![testName.Substring(0, 40)]),
                Is.True);
            Console.WriteLine("Done TestValidate_PCO_FUN_STL_BIW_FTR_CRC_F09");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidate_PCO_FUN_STL_BIW_FTR_CRC_F09 \n" + exception + "\n");
        }
    }

    [TestCase(Category = "SkuFunBiwTests")]
    [Test(Description = "TestValidate_PCO_FUN_STL_BIW_FTR_DRC_F12 with generated merchant"), CancelAfter(120 * 1000)]
    public async Task TestValidate_PCO_FUN_STL_BIW_FTR_DRC_F12()
    {
        try
        {
            Console.WriteLine("\nStarting TestValidate_PCO_FUN_STL_BIW_FTR_DRC_F12");
            var testName = TestContext.CurrentContext.Test.Name;
            var terminalApiKey = _envConfig.PCO_FUN_STL_BIW_FTR;
            
            
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            createPlanDefaultValues.paymentMethod.card.cardNumber = Environment.GetEnvironmentVariable("DebitCard")!;
            var planCreateResponse = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!, 
                _requestHeader!, "PendingCapture",
                12, terminalApiKey, createPlanDefaultValues);
            var skuQueryResult = await _mongoHandler.SendMongoQueryForSkuAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", planCreateResponse.InstallmentPlanNumber, "Sku");
            Assert.That(_skuTestsData.ValidateSkuValue(skuQueryResult, testName.Substring(0, 40)), Is.True);
            var listActivityValues = await _mongoHandler.SendMongoQueryActivityAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber",
                planCreateResponse.InstallmentPlanNumber, "Activity");
            if (listActivityValues.Count is not (2 or 1))
                Assert.Fail("listActivityValues.Count has wrong value");
            Assert.That(
                _mongoHandler.ValidateActivityValueExistence(listActivityValues, _skuDict![testName.Substring(0, 40)]),
                Is.True);
            Console.WriteLine("Done TestValidate_PCO_FUN_STL_BIW_FTR_DRC_F12");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidate_PCO_FUN_STL_BIW_FTR_DRC_F12 \n" + exception + "\n");
        }
    }

    [TestCase(Category = "SkuFunBiwTests")]
    [Test(Description = "TestValidate_PCO_FUN_STL_BIW_FTR_CRC_F12 with generated merchant"), CancelAfter(120 * 1000)]
    public async Task TestValidate_PCO_FUN_STL_BIW_FTR_CRC_F12()
    {
        try
        {
            Console.WriteLine("\nStarting TestValidate_PCO_FUN_STL_BIW_FTR_CRC_F12");
            var testName = TestContext.CurrentContext.Test.Name;
            var terminalApiKey = _envConfig.PCO_FUN_STL_BIW_FTR;
            
            
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            var planCreateResponse = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!, 
                _requestHeader!, "Active", 12, terminalApiKey, createPlanDefaultValues);
            var skuQueryResult = await _mongoHandler.SendMongoQueryForSkuAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", planCreateResponse.InstallmentPlanNumber, "Sku");
            Assert.That(_skuTestsData.ValidateSkuValue(skuQueryResult, testName.Substring(0, 40)), Is.True);
            var listActivityValues = await _mongoHandler.SendMongoQueryActivityAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber",
                planCreateResponse.InstallmentPlanNumber, "Activity");
            if (listActivityValues.Count is not (2 or 1))
                Assert.Fail("listActivityValues.Count has wrong value");
            Assert.That(
                _mongoHandler.ValidateActivityValueExistence(listActivityValues, _skuDict![testName.Substring(0, 40)]),
                Is.True);
            Console.WriteLine("Done TestValidate_PCO_FUN_STL_BIW_FTR_CRC_F12");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidate_PCO_FUN_STL_BIW_FTR_CRC_F12 \n" + exception + "\n");
        }
    }

    [TestCase(Category = "SkuFunBiwTests")]
    [Test(Description = "TestValidate_PCO_FUN_STL_BIW_FTR_DRC_F15 with generated merchant"), CancelAfter(120 * 1000)]
    public async Task TestValidate_PCO_FUN_STL_BIW_FTR_DRC_F15()
    {
        try
        {
            Console.WriteLine("\nStarting TestValidate_PCO_FUN_STL_BIW_FTR_DRC_F15");
            var testName = TestContext.CurrentContext.Test.Name;
            var terminalApiKey = _envConfig.PCO_FUN_STL_BIW_FTR;
            
            
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            createPlanDefaultValues.paymentMethod.card.cardNumber = Environment.GetEnvironmentVariable("DebitCard")!;
            var planCreateResponse = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!, 
                _requestHeader!, "PendingCapture", 15, terminalApiKey, createPlanDefaultValues);
            var skuQueryResult = await _mongoHandler.SendMongoQueryForSkuAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", planCreateResponse.InstallmentPlanNumber, "Sku");
            Assert.That(_skuTestsData.ValidateSkuValue(skuQueryResult, testName.Substring(0, 40)), Is.True);
            var listActivityValues = await _mongoHandler.SendMongoQueryActivityAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber",
                planCreateResponse.InstallmentPlanNumber, "Activity");
            if (listActivityValues.Count is not (2 or 1))
                Assert.Fail("listActivityValues.Count has wrong value");
            Assert.That(
                _mongoHandler.ValidateActivityValueExistence(listActivityValues, _skuDict![testName.Substring(0, 40)]),
                Is.True);
            Console.WriteLine("Done TestValidate_PCO_FUN_STL_BIW_FTR_DRC_F15");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidate_PCO_FUN_STL_BIW_FTR_DRC_F15 \n" + exception + "\n");
        }
    }

    [TestCase(Category = "SkuFunBiwTests")]
    [Test(Description = "TestValidate_PCO_FUN_STL_BIW_FTR_CRC_F15 with generated merchant"), CancelAfter(120 * 1000)]
    public async Task TestValidate_PCO_FUN_STL_BIW_FTR_CRC_F15()
    {
        try
        {
            Console.WriteLine("\nStarting TestValidate_PCO_FUN_STL_BIW_FTR_CRC_F15");
            var testName = TestContext.CurrentContext.Test.Name;
            var terminalApiKey = _envConfig.PCO_FUN_STL_BIW_FTR;
            
            
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            var planCreateResponse = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!, 
                _requestHeader!, "Active", 15, terminalApiKey, createPlanDefaultValues);
            var skuQueryResult = await _mongoHandler.SendMongoQueryForSkuAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", planCreateResponse.InstallmentPlanNumber, "Sku");
            Assert.That(_skuTestsData.ValidateSkuValue(skuQueryResult, testName.Substring(0, 40)), Is.True);
            var listActivityValues = await _mongoHandler.SendMongoQueryActivityAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber",
                planCreateResponse.InstallmentPlanNumber, "Activity");
            if (listActivityValues.Count is not (2 or 1))
                Assert.Fail("listActivityValues.Count has wrong value");
            Assert.That(
                _mongoHandler.ValidateActivityValueExistence(listActivityValues, _skuDict![testName.Substring(0, 40)]),
                Is.True);
            Console.WriteLine("Done TestValidate_PCO_FUN_STL_BIW_FTR_CRC_F15");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidate_PCO_FUN_STL_BIW_FTR_CRC_F15 \n" + exception + "\n");
        }
    }

    [TestCase(Category = "SkuFunBiwTests")]
    [Test(Description = "TestValidate_PCO_FUN_STL_BIW_FTR_DRC_F18 with generated merchant"), CancelAfter(120 * 1000)]
    public async Task TestValidate_PCO_FUN_STL_BIW_FTR_DRC_F18()
    {
        try
        {
            Console.WriteLine("\nStarting TestValidate_PCO_FUN_STL_BIW_FTR_DRC_F18");
            var testName = TestContext.CurrentContext.Test.Name;
            var terminalApiKey = _envConfig.PCO_FUN_STL_BIW_FTR;
            
            
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            createPlanDefaultValues.paymentMethod.card.cardNumber = Environment.GetEnvironmentVariable("DebitCard")!;
            var planCreateResponse = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!, 
                _requestHeader!, "PendingCapture", 18, terminalApiKey, createPlanDefaultValues);
            var skuQueryResult = await _mongoHandler.SendMongoQueryForSkuAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", planCreateResponse.InstallmentPlanNumber, "Sku");
            Assert.That(_skuTestsData.ValidateSkuValue(skuQueryResult, testName.Substring(0, 40)), Is.True);
            var listActivityValues = await _mongoHandler.SendMongoQueryActivityAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber",
                planCreateResponse.InstallmentPlanNumber, "Activity");
            if (listActivityValues.Count is not (2 or 1))
                Assert.Fail("listActivityValues.Count has wrong value");
            Assert.That(
                _mongoHandler.ValidateActivityValueExistence(listActivityValues, _skuDict![testName.Substring(0, 40)]),
                Is.True);
            Console.WriteLine("Done TestValidate_PCO_FUN_STL_BIW_FTR_DRC_F18");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidate_PCO_FUN_STL_BIW_FTR_DRC_F18 \n" + exception + "\n");
        }
    }

    [TestCase(Category = "SkuFunBiwTests")]
    [Test(Description = "TestValidate_PCO_FUN_STL_BIW_FTR_CRC_F18 with generated merchant"), CancelAfter(120 * 1000)]
    public async Task TestValidate_PCO_FUN_STL_BIW_FTR_CRC_F18()
    {
        try
        {
            Console.WriteLine("\nStarting TestValidate_PCO_FUN_STL_BIW_FTR_CRC_F18");
            var testName = TestContext.CurrentContext.Test.Name;
            var terminalApiKey = _envConfig.PCO_FUN_STL_BIW_FTR;
            
            
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            var planCreateResponse = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!, 
                _requestHeader!, "Active", 18, terminalApiKey, createPlanDefaultValues);
            var skuQueryResult = await _mongoHandler.SendMongoQueryForSkuAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", planCreateResponse.InstallmentPlanNumber, "Sku");
            Assert.That(_skuTestsData.ValidateSkuValue(skuQueryResult, testName.Substring(0, 40)), Is.True);
            var listActivityValues = await _mongoHandler.SendMongoQueryActivityAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber",
                planCreateResponse.InstallmentPlanNumber, "Activity");
            if (listActivityValues.Count is not (2 or 1))
                Assert.Fail("listActivityValues.Count has wrong value");
            Assert.That(
                _mongoHandler.ValidateActivityValueExistence(listActivityValues, _skuDict![testName.Substring(0, 40)]),
                Is.True);
            Console.WriteLine("Done TestValidate_PCO_FUN_STL_BIW_FTR_CRC_F18");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidate_PCO_FUN_STL_BIW_FTR_CRC_F18 \n" + exception + "\n");
        }
    }

    [TestCase(Category = "SkuFunBiwTests")]
    [Test(Description = "TestValidate_PCO_FUN_STL_BIW_FTR_DRC_F21 with generated merchant"), CancelAfter(120 * 1000)]
    public async Task TestValidate_PCO_FUN_STL_BIW_FTR_DRC_F21()
    {
        try
        {
            Console.WriteLine("\nStarting TestValidate_PCO_FUN_STL_BIW_FTR_DRC_F21");
            var testName = TestContext.CurrentContext.Test.Name;
            var terminalApiKey = _envConfig.PCO_FUN_STL_BIW_FTR;
            
            
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            createPlanDefaultValues.paymentMethod.card.cardNumber = Environment.GetEnvironmentVariable("DebitCard")!;
            var planCreateResponse = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!, 
                _requestHeader!, "PendingCapture", 21, terminalApiKey, createPlanDefaultValues);
            var skuQueryResult = await _mongoHandler.SendMongoQueryForSkuAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", planCreateResponse.InstallmentPlanNumber, "Sku");
            Assert.That(_skuTestsData.ValidateSkuValue(skuQueryResult, testName.Substring(0, 40)), Is.True);
            var listActivityValues = await _mongoHandler.SendMongoQueryActivityAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber",
                planCreateResponse.InstallmentPlanNumber, "Activity");
            if (listActivityValues.Count is not (2 or 1))
                Assert.Fail("listActivityValues.Count has wrong value");
            Assert.That(
                _mongoHandler.ValidateActivityValueExistence(listActivityValues, _skuDict![testName.Substring(0, 40)]),
                Is.True);
            Console.WriteLine("Done TestValidate_PCO_FUN_STL_BIW_FTR_DRC_F21");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidate_PCO_FUN_STL_BIW_FTR_DRC_F21 \n" + exception + "\n");
        }
    }

    [TestCase(Category = "SkuFunBiwTests")]
    [Test(Description = "TestValidate_PCO_FUN_STL_BIW_FTR_CRC_F21 with generated merchant"), CancelAfter(120 * 1000)]
    public async Task TestValidate_PCO_FUN_STL_BIW_FTR_CRC_F21()
    {
        try
        {
            Console.WriteLine("\nStarting TestValidate_PCO_FUN_STL_BIW_FTR_CRC_F21");
            var testName = TestContext.CurrentContext.Test.Name;
            var terminalApiKey = _envConfig.PCO_FUN_STL_BIW_FTR;
            
            
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            var planCreateResponse = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!, 
                _requestHeader!, "Active", 21, terminalApiKey, createPlanDefaultValues);
            var skuQueryResult = await _mongoHandler.SendMongoQueryForSkuAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", planCreateResponse.InstallmentPlanNumber, "Sku");
            Assert.That(_skuTestsData.ValidateSkuValue(skuQueryResult, testName.Substring(0, 40)), Is.True);
            var listActivityValues = await _mongoHandler.SendMongoQueryActivityAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber",
                planCreateResponse.InstallmentPlanNumber, "Activity");
            if (listActivityValues.Count is not (2 or 1))
                Assert.Fail("listActivityValues.Count has wrong value");
            Assert.That(
                _mongoHandler.ValidateActivityValueExistence(listActivityValues, _skuDict![testName.Substring(0, 40)]),
                Is.True);
            Console.WriteLine("Done TestValidate_PCO_FUN_STL_BIW_FTR_CRC_F21");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidate_PCO_FUN_STL_BIW_FTR_CRC_F21 \n" + exception + "\n");
        }
    }

    [TestCase(Category = "SkuFunBiwTests")]
    [Test(Description = "TestValidate_PCO_FUN_STL_BIW_FTR_DRC_F24 with generated merchant"), CancelAfter(120 * 1000)]
    public async Task TestValidate_PCO_FUN_STL_BIW_FTR_DRC_F24()
    {
        try
        {
            Console.WriteLine("\nStarting TestValidate_PCO_FUN_STL_BIW_FTR_DRC_F24");
            var testName = TestContext.CurrentContext.Test.Name;
            var terminalApiKey = _envConfig.PCO_FUN_STL_BIW_FTR;
            
            
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            createPlanDefaultValues.paymentMethod.card.cardNumber = Environment.GetEnvironmentVariable("DebitCard")!;
            var planCreateResponse = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!, 
                _requestHeader!, "PendingCapture", 24, terminalApiKey, createPlanDefaultValues);
            var skuQueryResult = await _mongoHandler.SendMongoQueryForSkuAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", planCreateResponse.InstallmentPlanNumber, "Sku");
            Assert.That(_skuTestsData.ValidateSkuValue(skuQueryResult, testName.Substring(0, 40)), Is.True);
            var listActivityValues = await _mongoHandler.SendMongoQueryActivityAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber",
                planCreateResponse.InstallmentPlanNumber, "Activity");
            if (listActivityValues.Count is not (2 or 1))
                Assert.Fail("listActivityValues.Count has wrong value");
            Assert.That(
                _mongoHandler.ValidateActivityValueExistence(listActivityValues, _skuDict![testName.Substring(0, 40)]),
                Is.True);
            Console.WriteLine("Done TestValidate_PCO_FUN_STL_BIW_FTR_DRC_F24");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidate_PCO_FUN_STL_BIW_FTR_DRC_F24 \n" + exception + "\n");
        }
    }

    [TestCase(Category = "SkuFunBiwTests")]
    [Test(Description = "TestValidate_PCO_FUN_STL_BIW_FTR_CRC_F24 with generated merchant"), CancelAfter(120 * 1000)]
    public async Task TestValidate_PCO_FUN_STL_BIW_FTR_CRC_F24()
    {
        try
        {
            Console.WriteLine("\nStarting TestValidate_PCO_FUN_STL_BIW_FTR_CRC_F24");
            var testName = TestContext.CurrentContext.Test.Name;
            var terminalApiKey = _envConfig.PCO_FUN_STL_BIW_FTR;
            
            
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            var planCreateResponse = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!, 
                _requestHeader!, "Active", 24, terminalApiKey, createPlanDefaultValues);
            var skuQueryResult = await _mongoHandler.SendMongoQueryForSkuAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", planCreateResponse.InstallmentPlanNumber, "Sku");
            Assert.That(_skuTestsData.ValidateSkuValue(skuQueryResult, testName.Substring(0, 40)), Is.True);
            var listActivityValues = await _mongoHandler.SendMongoQueryActivityAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber",
                planCreateResponse.InstallmentPlanNumber, "Activity");
            if (listActivityValues.Count is not (2 or 1))
                Assert.Fail("listActivityValues.Count has wrong value");
            Assert.That(
                _mongoHandler.ValidateActivityValueExistence(listActivityValues, _skuDict![testName.Substring(0, 40)]),
                Is.True);
            Console.WriteLine("Done TestValidate_PCO_FUN_STL_BIW_FTR_CRC_F24");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidate_PCO_FUN_STL_BIW_FTR_CRC_F24 \n" + exception + "\n");
        }
    }
}