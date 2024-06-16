using MongoDB.Driver;
using Allure.NUnit;
using Allure.NUnit.Attributes;
using NUnit.Framework;
using Splitit.Automation.NG.Backend.BaseActions;
using Splitit.Automation.NG.Backend.Services.V3.DefaultValues;
using Splitit.Automation.NG.Backend.Services.V3.InstallmentPlan.InstallmentPlanApiEndPoint;
using Splitit.Automation.NG.Backend.Tests.TestsSetups;
using Splitit.Automation.NG.Utilities.EnvironmentsConfig;

namespace Splitit.Automation.NG.PaymentsSystem.Sku.SkuMatching.Tests.SkuUnfTests;

[TestFixture]
[AllureNUnit]
[AllureSuite("SkuUnfBiwTests")]
[AllureDisplayIgnored]
[Parallelizable(ParallelScope.All)]
public class SkuUnfBiwTests
{
    private RequestHeader? _requestHeader;
    private readonly ExcelFileController _excelFileController;
    private readonly SkuTestsData.SkuTestsData _skuTestsData;
    private readonly InstallmentPlans _installmentPlans;
    private Dictionary<string, SkuTestsData.SkuTestsData.PaymentSettings>? _skuDict;
    private readonly MongoHandler _mongoHandler;
    private IMongoDatabase? _db;
    private readonly EnvConfig _envConfig;


    public SkuUnfBiwTests()
    {
        Console.WriteLine("Staring SKU Setup");
        _excelFileController = new ExcelFileController();
        _skuTestsData = new SkuTestsData.SkuTestsData();
        _installmentPlans = new InstallmentPlans();
        _mongoHandler = new MongoHandler();
        _envConfig = new EnvConfig();
        Console.WriteLine("SKU Setup Succeeded\n");
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

    [TestCase(Category = "SkuUnfBiwTests")]
    [Test(Description = "TestValidate_MER_UNF_STL_BIW_FTR_DRC_U03 with generated merchant"), CancelAfter(120 * 1000)]
    public async Task TestValidate_MER_UNF_STL_BIW_FTR_DRC_U03()
    {
        try
        {
            Console.WriteLine("\nStarting TestValidate_MER_UNF_STL_BIW_FTR_DRC_U03");
            var testName = TestContext.CurrentContext.Test.Name;
            var terminalApiKey = _envConfig.MER_UNF_STL_BIW_FTR;
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            createPlanDefaultValues.paymentMethod.card.cardNumber = Environment.GetEnvironmentVariable("DebitCard")!;
            var planCreateResponse = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!,
                _requestHeader!, "PendingCapture", 3, terminalApiKey, createPlanDefaultValues);
            var skuQueryResult = await _mongoHandler.SendMongoQueryForSkuAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber",
                planCreateResponse.InstallmentPlanNumber, "Sku");
            Console.WriteLine("skuQueryResult --------> " + skuQueryResult + " for ipn ----------> " +
                              planCreateResponse.InstallmentPlanNumber);
            Assert.That(_skuTestsData.ValidateSkuValue(skuQueryResult, testName.Substring(0, 40)), Is.True);
            var listActivityValues = await _mongoHandler.SendMongoQueryActivityAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber",
                planCreateResponse.InstallmentPlanNumber, "Activity");
            Assert.That(listActivityValues.Count >= 1);
            Assert.That(_skuDict![testName.Substring(0, 40)].SetActivity, Does.Contain(listActivityValues[0]!));
            Console.WriteLine("Done TestValidate_MER_UNF_STL_BIW_FTR_DRC_U03");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidate_MER_UNF_STL_BIW_FTR_DRC_U03 \n" + exception + "\n");
        }
    }


    [TestCase(Category = "SkuUnfBiwTests")]
    [Test(Description = "TestValidate_MER_UNF_STL_BIW_FTR_CRC_U03 with generated merchant"), CancelAfter(120 * 1000)]
    public async Task TestValidate_MER_UNF_STL_BIW_FTR_CRC_U03()
    {
        try
        {
            Console.WriteLine("\nStarting TestValidate_MER_UNF_STL_BIW_FTR_CRC_U03");
            var testName = TestContext.CurrentContext.Test.Name;
            var terminalApiKey = _envConfig.MER_UNF_STL_BIW_FTR;
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            var planCreateResponse = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!,
                _requestHeader!,
                "Active", 3, terminalApiKey, createPlanDefaultValues);
            var skuQueryResult = await _mongoHandler.SendMongoQueryForSkuAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber",
                planCreateResponse.InstallmentPlanNumber, "Sku");
            Assert.That(_skuTestsData.ValidateSkuValue(skuQueryResult, testName.Substring(0, 40)), Is.True);
            var listActivityValues = await _mongoHandler.SendMongoQueryActivityAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber",
                planCreateResponse.InstallmentPlanNumber, "Activity");
            Assert.That(listActivityValues.Count >= 1);
            Assert.That(_skuDict![testName.Substring(0, 40)].SetActivity, Does.Contain(listActivityValues[0]!));
            Console.WriteLine("Done TestValidate_MER_UNF_STL_BIW_FTR_CRC_U03");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidate_MER_UNF_STL_BIW_FTR_CRC_U03 \n" + exception + "\n");
        }
    }
    
     [TestCase(Category = "SkuUnfBiwTests")]
     [Test(Description = "TestValidate_MER_UNF_STL_BIW_FTR_DRC_U04 with generated merchant"), CancelAfter(120 * 1000)]
     public async Task TestValidate_MER_UNF_STL_BIW_FTR_DRC_U04()
     {
         try
         {
             Console.WriteLine("\nStarting TestValidate_MER_UNF_STL_BIW_FTR_DRC_U04");
             var testName = TestContext.CurrentContext.Test.Name;
             var terminalApiKey = _envConfig.MER_UNF_STL_BIW_FTR;
             Console.WriteLine("Done with creation of a new merchant , settings and terminal process");
             var createPlanDefaultValues = new CreatePlanDefaultValues();
             var is3DsVarsCreation = new Is3DsVarsCreation();
             createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
             createPlanDefaultValues.paymentMethod.card.cardNumber = Environment.GetEnvironmentVariable("DebitCard")!;
             var planCreateResponse = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!, 
                 _requestHeader!,
                 "PendingCapture", 4, terminalApiKey, createPlanDefaultValues);
             var skuQueryResult = await _mongoHandler.SendMongoQueryForSkuAsync(_db!, "Payment_Records",
                 "InstallmentPlanNumber",
                 planCreateResponse.InstallmentPlanNumber, "Sku");
             Console.WriteLine("skuQueryResult --------> " + skuQueryResult + " for ipn ----------> " +
                               planCreateResponse.InstallmentPlanNumber);
             Assert.That(_skuTestsData.ValidateSkuValue(skuQueryResult, testName.Substring(0, 40)), Is.True);
             var listActivityValues = await _mongoHandler.SendMongoQueryActivityAsync(_db!, "Payment_Records",
                 "InstallmentPlanNumber",
                 planCreateResponse.InstallmentPlanNumber, "Activity");
             Assert.That(listActivityValues, Has.Count.EqualTo(1));
             Assert.That(_skuDict![testName.Substring(0, 40)].SetActivity, Does.Contain(listActivityValues[0]!));
             Console.WriteLine("Done TestValidate_MER_UNF_STL_BIW_FTR_DRC_U04");
         }
         catch (Exception exception)
         {
             Assert.Fail("Error in TestValidate_MER_UNF_STL_BIW_FTR_DRC_U04\n" + exception + "\n");
         }
     }
    
    
     [TestCase(Category = "SkuUnfBiwTests")]
     [Test(Description = "TestValidate_MER_UNF_STL_BIW_FTR_CRC_U04 with generated merchant"), CancelAfter(120 * 1000)]
     public async Task TestValidate_MER_UNF_STL_BIW_FTR_CRC_U04()
     {
         try
         {
             Console.WriteLine("\nStarting TestValidate_MER_UNF_STL_BIW_FTR_CRC_U04");
             var testName = TestContext.CurrentContext.Test.Name;
             var terminalApiKey = _envConfig.MER_UNF_STL_BIW_FTR;
             Console.WriteLine("Done with creation of a new merchant , settings and terminal process");
             var createPlanDefaultValues = new CreatePlanDefaultValues();
             var is3DsVarsCreation = new Is3DsVarsCreation();
             createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
             var planCreateResponse = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!, 
                 _requestHeader!,
                 "Active", 4, terminalApiKey, createPlanDefaultValues);
             var skuQueryResult = await _mongoHandler.SendMongoQueryForSkuAsync(_db!, "Payment_Records",
                 "InstallmentPlanNumber",
                 planCreateResponse.InstallmentPlanNumber, "Sku");
             Assert.That(_skuTestsData.ValidateSkuValue(skuQueryResult, testName.Substring(0, 40)), Is.True);
             var listActivityValues = await _mongoHandler.SendMongoQueryActivityAsync(_db!, "Payment_Records",
                 "InstallmentPlanNumber",
                 planCreateResponse.InstallmentPlanNumber, "Activity");
             Assert.That(listActivityValues, Has.Count.EqualTo(1));
             Assert.That(_skuDict![testName.Substring(0, 40)].SetActivity, Does.Contain(listActivityValues[0]!));
             Console.WriteLine("Done TestValidate_MER_UNF_STL_BIW_FTR_CRC_U04");
         }
         catch (Exception exception)
         {
             Assert.Fail("Error in TestValidate_MER_UNF_STL_BIW_FTR_CRC_U04\n" + exception + "\n");
         }
     }
     [TestCase(Category = "SkuUnfBiwTests")]
     [Test(Description = "TestValidate_MER_UNF_STL_BIW_FTR_DRC_U05 with generated merchant"), CancelAfter(120 * 1000)]
     public async Task TestValidate_MER_UNF_STL_BIW_FTR_DRC_U05()
     {
         try
         {
             Console.WriteLine("\nStarting TestValidate_MER_UNF_STL_BIW_FTR_DRC_U05");
             var testName = TestContext.CurrentContext.Test.Name;
             var terminalApiKey = _envConfig.MER_UNF_STL_BIW_FTR;
             Console.WriteLine("Done with creation of a new merchant , settings and terminal process");
             var createPlanDefaultValues = new CreatePlanDefaultValues();
             var is3DsVarsCreation = new Is3DsVarsCreation();
             createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
             createPlanDefaultValues.paymentMethod.card.cardNumber = Environment.GetEnvironmentVariable("DebitCard")!;
             var planCreateResponse = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!, 
                 _requestHeader!,
                 "PendingCapture", 5, terminalApiKey, createPlanDefaultValues);
             var skuQueryResult = await _mongoHandler.SendMongoQueryForSkuAsync(_db!, "Payment_Records",
                 "InstallmentPlanNumber",
                 planCreateResponse.InstallmentPlanNumber, "Sku");
             Console.WriteLine("skuQueryResult --------> " + skuQueryResult + " for ipn ----------> " +
                               planCreateResponse.InstallmentPlanNumber);
             Assert.That(_skuTestsData.ValidateSkuValue(skuQueryResult, testName.Substring(0, 40)), Is.True);
             var listActivityValues = await _mongoHandler.SendMongoQueryActivityAsync(_db!, "Payment_Records",
                 "InstallmentPlanNumber",
                 planCreateResponse.InstallmentPlanNumber, "Activity");
             Assert.That(listActivityValues, Has.Count.EqualTo(1));
             Assert.That(_skuDict![testName.Substring(0, 40)].SetActivity, Does.Contain(listActivityValues[0]!));
             Console.WriteLine("Done TestValidate_MER_UNF_STL_BIW_FTR_DRC_U05");
         }
         catch (Exception exception)
         {
             Assert.Fail("Error in TestValidate_MER_UNF_STL_BIW_FTR_DRC_U05\n" + exception + "\n");
         }
     }
    
    
     [TestCase(Category = "SkuUnfBiwTests")]
     [Test(Description = "TestValidate_MER_UNF_STL_BIW_FTR_CRC_U05 with generated merchant"), CancelAfter(120 * 1000)]
     public async Task TestValidate_MER_UNF_STL_BIW_FTR_CRC_U05()
     {
         try
         {
             Console.WriteLine("\nStarting TestValidate_MER_UNF_STL_BIW_FTR_CRC_U05");
             var testName = TestContext.CurrentContext.Test.Name;
             var terminalApiKey = _envConfig.MER_UNF_STL_BIW_FTR;
             Console.WriteLine("Done with creation of a new merchant , settings and terminal process");
             var createPlanDefaultValues = new CreatePlanDefaultValues();
             var is3DsVarsCreation = new Is3DsVarsCreation();
             createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
             var planCreateResponse = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!, 
                 _requestHeader!,
                 "Active", 5, terminalApiKey, createPlanDefaultValues);
             var skuQueryResult = await _mongoHandler.SendMongoQueryForSkuAsync(_db!, "Payment_Records",
                 "InstallmentPlanNumber",
                 planCreateResponse.InstallmentPlanNumber, "Sku");
             Assert.That(_skuTestsData.ValidateSkuValue(skuQueryResult, testName.Substring(0, 40)), Is.True);
             var listActivityValues = await _mongoHandler.SendMongoQueryActivityAsync(_db!, "Payment_Records",
                 "InstallmentPlanNumber",
                 planCreateResponse.InstallmentPlanNumber, "Activity");
             Assert.That(listActivityValues, Has.Count.EqualTo(1));
             Assert.That(_skuDict![testName.Substring(0, 40)].SetActivity, Does.Contain(listActivityValues[0]!));
             Console.WriteLine("Done TestValidate_MER_UNF_STL_BIW_FTR_CRC_U05");
         }
         catch (Exception exception)
         {
             Assert.Fail("Error in TestValidate_MER_UNF_STL_BIW_FTR_CRC_U05\n" + exception + "\n");
         }
     }

     [TestCase(Category = "SkuUnfBiwTests")]
     [Test(Description = "TestValidate_MER_UNF_STL_BIW_FTR_DRC_U07 with generated merchant"), CancelAfter(120 * 1000)]
     public async Task TestValidate_MER_UNF_STL_BIW_FTR_DRC_U07()
     {
         try
         {
             Console.WriteLine("\nStarting TestValidate_MER_UNF_STL_BIW_FTR_DRC_U07");
             var testName = TestContext.CurrentContext.Test.Name;
             var terminalApiKey = _envConfig.MER_UNF_STL_BIW_FTR;
             Console.WriteLine("Done with creation of a new merchant , settings and terminal process");
             var createPlanDefaultValues = new CreatePlanDefaultValues();
             var is3DsVarsCreation = new Is3DsVarsCreation();
             createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
             createPlanDefaultValues.paymentMethod.card.cardNumber = Environment.GetEnvironmentVariable("DebitCard")!;
             var planCreateResponse = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!, 
                 _requestHeader!,
                 "PendingCapture", 7, terminalApiKey, createPlanDefaultValues);
             var skuQueryResult = await _mongoHandler.SendMongoQueryForSkuAsync(_db!, "Payment_Records",
                 "InstallmentPlanNumber",
                 planCreateResponse.InstallmentPlanNumber, "Sku");
             Assert.That(_skuTestsData.ValidateSkuValue(skuQueryResult, testName.Substring(0, 40)), Is.True);
             var listActivityValues = await _mongoHandler.SendMongoQueryActivityAsync(_db!, "Payment_Records",
                 "InstallmentPlanNumber",
                 planCreateResponse.InstallmentPlanNumber, "Activity");
             Assert.That(listActivityValues, Has.Count.EqualTo(1));
             Assert.That(_skuDict![testName.Substring(0, 40)].SetActivity, Does.Contain(listActivityValues[0]!));
             Console.WriteLine("Done TestValidate_MER_UNF_STL_BIW_FTR_DRC_U07");
         }
         catch (Exception exception)
         {
             Assert.Fail("Error in TestValidate_MER_UNF_STL_BIW_FTR_DRC_U07 \n" + exception + "\n");
         }
     }
    
    
     [TestCase(Category = "SkuUnfBiwTests")]
     [Test(Description = "TestValidate_MER_UNF_STL_BIW_FTR_CRC_U07 with generated merchant"), CancelAfter(120 * 1000)]
     public async Task TestValidate_MER_UNF_STL_BIW_FTR_CRC_U07()
     {
         try
         {
             Console.WriteLine("\nStarting TestValidate_MER_UNF_STL_BIW_FTR_CRC_U07");
             var testName = TestContext.CurrentContext.Test.Name;
             var terminalApiKey = _envConfig.MER_UNF_STL_BIW_FTR;
             Console.WriteLine("Done with creation of a new merchant , settings and terminal process");
             var createPlanDefaultValues = new CreatePlanDefaultValues();
             var is3DsVarsCreation = new Is3DsVarsCreation();
             createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
             var planCreateResponse = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!, 
                 _requestHeader!,
                 "Active", 7, terminalApiKey, createPlanDefaultValues);
             var skuQueryResult = await _mongoHandler.SendMongoQueryForSkuAsync(_db!, "Payment_Records",
                 "InstallmentPlanNumber",
                 planCreateResponse.InstallmentPlanNumber, "Sku");
             Assert.That(_skuTestsData.ValidateSkuValue(skuQueryResult, testName.Substring(0, 40)), Is.True);
             var listActivityValues = await _mongoHandler.SendMongoQueryActivityAsync(_db!, "Payment_Records",
                 "InstallmentPlanNumber",
                 planCreateResponse.InstallmentPlanNumber, "Activity");
             Assert.That(listActivityValues, Has.Count.EqualTo(1));
             Assert.That(_skuDict![testName.Substring(0, 40)].SetActivity, Does.Contain(listActivityValues[0]!));
             Console.WriteLine("Done TestValidate_MER_UNF_STL_BIW_FTR_CRC_U07");
         }
         catch (Exception exception)
         {
             Assert.Fail("Error in TestValidate_MER_UNF_STL_BIW_FTR_CRC_U07\n" + exception + "\n");
         }
     }
    
     [TestCase(Category = "SkuUnfBiwTests")]
     [Test(Description = "TestValidate_MER_UNF_STL_BIW_FTR_DRC_U08 with generated merchant"), CancelAfter(120 * 1000)]
     public async Task TestValidate_MER_UNF_STL_BIW_FTR_DRC_U08()
     {
         try
         {
             Console.WriteLine("\nStarting TestValidate_MER_UNF_STL_BIW_FTR_DRC_U08");
             var testName = TestContext.CurrentContext.Test.Name;
             var terminalApiKey = _envConfig.MER_UNF_STL_BIW_FTR;
             Console.WriteLine("Done with creation of a new merchant , settings and terminal process");
             var createPlanDefaultValues = new CreatePlanDefaultValues();
             var is3DsVarsCreation = new Is3DsVarsCreation();
             createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
             createPlanDefaultValues.paymentMethod.card.cardNumber = Environment.GetEnvironmentVariable("DebitCard")!;
             var planCreateResponse = await _installmentPlans.CreatePlanAsync(
                 Environment.GetEnvironmentVariable("ApiV3")!,  _requestHeader!, "PendingCapture",
                 8, terminalApiKey, createPlanDefaultValues);
             var skuQueryResult = await _mongoHandler.SendMongoQueryForSkuAsync(_db!, "Payment_Records",
                 "InstallmentPlanNumber", planCreateResponse.InstallmentPlanNumber, "Sku");
             Assert.That(_skuTestsData.ValidateSkuValue(skuQueryResult, testName.Substring(0, 40)), Is.True);
             var listActivityValues = await _mongoHandler.SendMongoQueryActivityAsync(_db!, "Payment_Records",
                 "InstallmentPlanNumber", planCreateResponse.InstallmentPlanNumber, "Activity");
             Assert.That(listActivityValues, Has.Count.EqualTo(1));
             Assert.That(_skuDict![testName.Substring(0, 40)].SetActivity, Does.Contain(listActivityValues[0]!));
             Console.WriteLine("Done TestValidate_MER_UNF_STL_BIW_FTR_DRC_U08");
         }
         catch (Exception exception)
         {
             Assert.Fail("Error in TestValidate_MER_UNF_STL_BIW_FTR_DRC_U08 \n" + exception + "\n");
         }
     }
    
    
     [TestCase(Category = "SkuUnfBiwTests")]
     [Test(Description = "TestValidate_MER_UNF_STL_BIW_FTR_CRC_U08 with generated merchant"), CancelAfter(120 * 1000)]
     public async Task TestValidate_MER_UNF_STL_BIW_FTR_CRC_U08()
     {
         try
         {
             Console.WriteLine("\nStarting TestValidate_MER_UNF_STL_BIW_FTR_CRC_U08");
             var testName = TestContext.CurrentContext.Test.Name;
             var terminalApiKey = _envConfig.MER_UNF_STL_BIW_FTR;
             Console.WriteLine("Done with creation of a new merchant , settings and terminal process");
             var createPlanDefaultValues = new CreatePlanDefaultValues();
             var is3DsVarsCreation = new Is3DsVarsCreation();
             createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
             var planCreateResponse = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!, 
                 _requestHeader!,
                 "Active", 8, terminalApiKey, createPlanDefaultValues);
             var skuQueryResult = await _mongoHandler.SendMongoQueryForSkuAsync(_db!, "Payment_Records",
                 "InstallmentPlanNumber",
                 planCreateResponse.InstallmentPlanNumber, "Sku");
             Assert.That(_skuTestsData.ValidateSkuValue(skuQueryResult, testName.Substring(0, 40)), Is.True);
             var listActivityValues = await _mongoHandler.SendMongoQueryActivityAsync(_db!, "Payment_Records",
                 "InstallmentPlanNumber",
                 planCreateResponse.InstallmentPlanNumber, "Activity");
             Assert.That(listActivityValues, Has.Count.EqualTo(1));
             Assert.That(_skuDict![testName.Substring(0, 40)].SetActivity, Does.Contain(listActivityValues[0]!));
             Console.WriteLine("Done TestValidate_MER_UNF_STL_BIW_FTR_CRC_U08");
         }
         catch (Exception exception)
         {
             Assert.Fail("Error in TestValidate_MER_UNF_STL_BIW_FTR_CRC_U08\n" + exception + "\n");
         }
     }
    
    
     [TestCase(Category = "SkuUnfBiwTests")]
     [Test(Description = "TestValidate_MER_UNF_STL_BIW_FTR_DRC_U10 with generated merchant"), CancelAfter(120 * 1000)]
     public async Task TestValidate_MER_UNF_STL_BIW_FTR_DRC_U10()
     {
         try
         {
             Console.WriteLine("\nStarting TestValidate_MER_UNF_STL_BIW_FTR_DRC_U10");
             var testName = TestContext.CurrentContext.Test.Name;
             var terminalApiKey = _envConfig.MER_UNF_STL_BIW_FTR;
             Console.WriteLine("Done with creation of a new merchant , settings and terminal process");
             var createPlanDefaultValues = new CreatePlanDefaultValues();
             var is3DsVarsCreation = new Is3DsVarsCreation();
             createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
             createPlanDefaultValues.paymentMethod.card.cardNumber = Environment.GetEnvironmentVariable("DebitCard")!;
             var planCreateResponse = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!, 
                 _requestHeader!,
                 "PendingCapture", 10, terminalApiKey, createPlanDefaultValues);
             var skuQueryResult = await _mongoHandler.SendMongoQueryForSkuAsync(_db!, "Payment_Records",
                 "InstallmentPlanNumber",
                 planCreateResponse.InstallmentPlanNumber, "Sku");
             Assert.That(_skuTestsData.ValidateSkuValue(skuQueryResult, testName.Substring(0, 40)), Is.True);
             var listActivityValues = await _mongoHandler.SendMongoQueryActivityAsync(_db!, "Payment_Records",
                 "InstallmentPlanNumber",
                 planCreateResponse.InstallmentPlanNumber, "Activity");
             Assert.That(listActivityValues, Has.Count.EqualTo(1));
             Assert.That(_skuDict![testName.Substring(0, 40)].SetActivity, Does.Contain(listActivityValues[0]!));
             Console.WriteLine("Done TestValidate_MER_UNF_STL_BIW_FTR_DRC_U10");
         }
         catch (Exception exception)
         {
             Assert.Fail("Error in TestValidate_MER_UNF_STL_BIW_FTR_DRC_U10\n" + exception + "\n");
         }
     }
    
    
     [TestCase(Category = "SkuUnfBiwTests")]
     [Test(Description = "TestValidate_MER_UNF_STL_BIW_FTR_CRC_U10 with generated merchant"), CancelAfter(120 * 1000)]
     public async Task TestValidate_MER_UNF_STL_BIW_FTR_CRC_U10()
     {
         try
         {
             Console.WriteLine("\nStarting TestValidate_MER_UNF_STL_BIW_FTR_CRC_U10");
             var testName = TestContext.CurrentContext.Test.Name;
             var terminalApiKey = _envConfig.MER_UNF_STL_BIW_FTR;
             Console.WriteLine("Done with creation of a new merchant , settings and terminal process");
             var createPlanDefaultValues = new CreatePlanDefaultValues();
             var is3DsVarsCreation = new Is3DsVarsCreation();
             createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
             var planCreateResponse = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!, 
                 _requestHeader!,
                 "Active", 10, terminalApiKey, createPlanDefaultValues);
             var skuQueryResult = await _mongoHandler.SendMongoQueryForSkuAsync(_db!, "Payment_Records",
                 "InstallmentPlanNumber",
                 planCreateResponse.InstallmentPlanNumber, "Sku");
             Assert.That(_skuTestsData.ValidateSkuValue(skuQueryResult, testName.Substring(0, 40)), Is.True);
             var listActivityValues = await _mongoHandler.SendMongoQueryActivityAsync(_db!, "Payment_Records",
                 "InstallmentPlanNumber",
                 planCreateResponse.InstallmentPlanNumber, "Activity");
             Assert.That(listActivityValues, Has.Count.EqualTo(1));
             Assert.That(_skuDict![testName.Substring(0, 40)].SetActivity, Does.Contain(listActivityValues[0]!));
             Console.WriteLine("Done TestValidate_MER_UNF_STL_BIW_FTR_CRC_U10");
         }
         catch (Exception exception)
         {
             Assert.Fail("Error in TestValidate_MER_UNF_STL_BIW_FTR_CRC_U10 \n" + exception + "\n");
         }
     }
    
     [TestCase(Category = "SkuUnfBiwTests")]
     [Test(Description = "TestValidate_MER_UNF_STL_BIW_FTR_DRC_U11 with generated merchant"), CancelAfter(120 * 1000)]
     public async Task TestValidate_MER_UNF_STL_BIW_FTR_DRC_U11()
     {
         try
         {
             Console.WriteLine("\nStarting TestValidate_MER_UNF_STL_BIW_FTR_DRC_U11");
             var testName = TestContext.CurrentContext.Test.Name;
             var terminalApiKey = _envConfig.MER_UNF_STL_BIW_FTR;
             Console.WriteLine("Done with creation of a new merchant , settings and terminal process");
             var createPlanDefaultValues = new CreatePlanDefaultValues();
             var is3DsVarsCreation = new Is3DsVarsCreation();
             createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
             createPlanDefaultValues.paymentMethod.card.cardNumber = Environment.GetEnvironmentVariable("DebitCard")!;
             var planCreateResponse = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!, 
                 _requestHeader!,
                 "PendingCapture", 11, terminalApiKey, createPlanDefaultValues);
             var skuQueryResult = await _mongoHandler.SendMongoQueryForSkuAsync(_db!, "Payment_Records",
                 "InstallmentPlanNumber",
                 planCreateResponse.InstallmentPlanNumber, "Sku");
             Assert.That(_skuTestsData.ValidateSkuValue(skuQueryResult, testName.Substring(0, 40)), Is.True);
             var listActivityValues = await _mongoHandler.SendMongoQueryActivityAsync(_db!, "Payment_Records",
                 "InstallmentPlanNumber",
                 planCreateResponse.InstallmentPlanNumber, "Activity");
             Assert.That(listActivityValues, Has.Count.EqualTo(1));
             Assert.That(_skuDict![testName.Substring(0, 40)].SetActivity, Does.Contain(listActivityValues[0]!));
             Console.WriteLine("Done TestValidate_MER_UNF_STL_BIW_FTR_DRC_U11");
         }
         catch (Exception exception)
         {
             Assert.Fail("Error in TestValidate_MER_UNF_STL_BIW_FTR_DRC_U11\n" + exception + "\n");
         }
     }
    
    
     [TestCase(Category = "SkuUnfBiwTests")]
     [Test(Description = "TestValidate_MER_UNF_STL_BIW_FTR_CRC_U11 with generated merchant"), CancelAfter(120 * 1000)]
     public async Task TestValidate_MER_UNF_STL_BIW_FTR_CRC_U11()
     {
         try
         {
             Console.WriteLine("\nStarting TestValidate_MER_UNF_STL_BIW_FTR_CRC_U11");
             var testName = TestContext.CurrentContext.Test.Name;
             var terminalApiKey = _envConfig.MER_UNF_STL_BIW_FTR;
             Console.WriteLine("Done with creation of a new merchant , settings and terminal process");
             var createPlanDefaultValues = new CreatePlanDefaultValues();
             var is3DsVarsCreation = new Is3DsVarsCreation();
             createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
             var planCreateResponse = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!, 
                 _requestHeader!,
                 "Active", 11, terminalApiKey, createPlanDefaultValues);
             var skuQueryResult = await _mongoHandler.SendMongoQueryForSkuAsync(_db!, "Payment_Records",
                 "InstallmentPlanNumber",
                 planCreateResponse.InstallmentPlanNumber, "Sku");
             Assert.That(_skuTestsData.ValidateSkuValue(skuQueryResult, testName.Substring(0, 40)), Is.True);
             var listActivityValues = await _mongoHandler.SendMongoQueryActivityAsync(_db!, "Payment_Records",
                 "InstallmentPlanNumber",
                 planCreateResponse.InstallmentPlanNumber, "Activity");
             Assert.That(listActivityValues, Has.Count.EqualTo(1));
             Assert.That(_skuDict![testName.Substring(0, 40)].SetActivity, Does.Contain(listActivityValues[0]!));
             Console.WriteLine("Done TestValidate_MER_UNF_STL_BIW_FTR_CRC_U11");
         }
         catch (Exception exception)
         {
             Assert.Fail("Error in TestValidate_MER_UNF_STL_BIW_FTR_CRC_U11 \n" + exception + "\n");
         }
     }
    
    [TestCase(Category = "SkuUnfBiwTests")]
    [Test(Description = "TestValidate_MER_UNF_STL_BIW_FTR_DRC_U06 with generated merchant"), CancelAfter(120 * 1000)]
    public async Task TestValidate_MER_UNF_STL_BIW_FTR_DRC_U06()
    {
        try
        {
            Console.WriteLine("\nStarting TestValidate_MER_UNF_STL_BIW_FTR_DRC_U06");
            var testName = TestContext.CurrentContext.Test.Name;
            var terminalApiKey = _envConfig.MER_UNF_STL_BIW_FTR;
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            createPlanDefaultValues.paymentMethod.card.cardNumber = Environment.GetEnvironmentVariable("DebitCard")!;
            var planCreateResponse = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!,
                _requestHeader!,
                "PendingCapture", 6, terminalApiKey, createPlanDefaultValues);
            var skuQueryResult = await _mongoHandler.SendMongoQueryForSkuAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber",
                planCreateResponse.InstallmentPlanNumber, "Sku");
            Assert.That(_skuTestsData.ValidateSkuValue(skuQueryResult, testName.Substring(0, 40)), Is.True);
            var listActivityValues = await _mongoHandler.SendMongoQueryActivityAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber",
                planCreateResponse.InstallmentPlanNumber, "Activity");
            Assert.That(listActivityValues.Count >= 1);
            Assert.That(_skuDict![testName.Substring(0, 40)].SetActivity, Does.Contain(listActivityValues[0]!));
            Console.WriteLine("Done TestValidate_MER_UNF_STL_BIW_FTR_DRC_U06");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidate_MER_UNF_STL_BIW_FTR_DRC_U06 \n" + exception + "\n");
        }
    }


    [TestCase(Category = "SkuUnfBiwTests")]
    [Test(Description = "TestValidate_MER_UNF_STL_BIW_FTR_CRC_U06 with generated merchant"), CancelAfter(120 * 1000)]
    public async Task TestValidate_MER_UNF_STL_BIW_FTR_CRC_U06()
    {
        try
        {
            Console.WriteLine("\nStarting TestValidate_MER_UNF_STL_BIW_FTR_CRC_U06");
            var testName = TestContext.CurrentContext.Test.Name;
            var terminalApiKey = _envConfig.MER_UNF_STL_BIW_FTR;
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            var planCreateResponse = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!,
                _requestHeader!,
                "Active", 6, terminalApiKey, createPlanDefaultValues);
            var skuQueryResult = await _mongoHandler.SendMongoQueryForSkuAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber",
                planCreateResponse.InstallmentPlanNumber, "Sku");
            Assert.That(_skuTestsData.ValidateSkuValue(skuQueryResult, testName.Substring(0, 40)), Is.True);
            var listActivityValues = await _mongoHandler.SendMongoQueryActivityAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber",
                planCreateResponse.InstallmentPlanNumber, "Activity");
            Assert.That(listActivityValues.Count >= 1);
            Assert.That(_skuDict![testName.Substring(0, 40)].SetActivity, Does.Contain(listActivityValues[0]!));
            Console.WriteLine("Done TestValidate_MER_UNF_STL_BIW_FTR_CRC_U06");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidate_MER_UNF_STL_BIW_FTR_CRC_U06 \n" + exception + "\n");
        }
    }
    
     [TestCase(Category = "SkuUnfBiwTests")]
     [Test(Description = "TestValidate_MER_UNF_STL_BIW_FTR_DRC_U13 with generated merchant"), CancelAfter(120 * 1000)]
     public async Task TestValidate_MER_UNF_STL_BIW_FTR_DRC_U13()
     {
         try
         {
             Console.WriteLine("\nStarting TestValidate_MER_UNF_STL_BIW_FTR_DRC_U13");
             var testName = TestContext.CurrentContext.Test.Name;
             var terminalApiKey = _envConfig.MER_UNF_STL_BIW_FTR;
             Console.WriteLine("Done with creation of a new merchant , settings and terminal process");
             var createPlanDefaultValues = new CreatePlanDefaultValues();
             var is3DsVarsCreation = new Is3DsVarsCreation();
             createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
             createPlanDefaultValues.paymentMethod.card.cardNumber = Environment.GetEnvironmentVariable("DebitCard")!;
             var planCreateResponse = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!, 
                 _requestHeader!,
                 "PendingCapture", 13, terminalApiKey, createPlanDefaultValues);
             var skuQueryResult = await _mongoHandler.SendMongoQueryForSkuAsync(_db!, "Payment_Records",
                 "InstallmentPlanNumber",
                 planCreateResponse.InstallmentPlanNumber, "Sku");
             Assert.That(_skuTestsData.ValidateSkuValue(skuQueryResult, testName.Substring(0, 40)), Is.True);
             var listActivityValues = await _mongoHandler.SendMongoQueryActivityAsync(_db!, "Payment_Records",
                 "InstallmentPlanNumber",
                 planCreateResponse.InstallmentPlanNumber, "Activity");
             Assert.That(listActivityValues, Has.Count.EqualTo(1));
             Assert.That(_skuDict![testName.Substring(0, 40)].SetActivity, Does.Contain(listActivityValues[0]!));
             Console.WriteLine("Done TestValidate_MER_UNF_STL_BIW_FTR_DRC_U13");
         }
         catch (Exception exception)
         {
             Assert.Fail("Error in TestValidate_MER_UNF_STL_BIW_FTR_DRC_U13\n" + exception + "\n");
         }
     }
    
    
     [TestCase(Category = "SkuUnfBiwTests")]
     [Test(Description = "TestValidate_MER_UNF_STL_BIW_FTR_CRC_U13 with generated merchant"), CancelAfter(120 * 1000)]
     public async Task TestValidate_MER_UNF_STL_BIW_FTR_CRC_U13()
     {
         try
         {
             Console.WriteLine("\nStarting TestValidate_MER_UNF_STL_BIW_FTR_CRC_U13");
             var testName = TestContext.CurrentContext.Test.Name;
             var terminalApiKey = _envConfig.MER_UNF_STL_BIW_FTR;
             Console.WriteLine("Done with creation of a new merchant , settings and terminal process");
             var createPlanDefaultValues = new CreatePlanDefaultValues();
             var is3DsVarsCreation = new Is3DsVarsCreation();
             createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
             var planCreateResponse = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!, 
                 _requestHeader!,
                 "Active", 13, terminalApiKey, createPlanDefaultValues);
             var skuQueryResult = await _mongoHandler.SendMongoQueryForSkuAsync(_db!, "Payment_Records",
                 "InstallmentPlanNumber",
                 planCreateResponse.InstallmentPlanNumber, "Sku");
             Assert.That(_skuTestsData.ValidateSkuValue(skuQueryResult, testName.Substring(0, 40)), Is.True);
             var listActivityValues = await _mongoHandler.SendMongoQueryActivityAsync(_db!, "Payment_Records",
                 "InstallmentPlanNumber",
                 planCreateResponse.InstallmentPlanNumber, "Activity");
             Assert.That(listActivityValues, Has.Count.EqualTo(1));
             Assert.That(_skuDict![testName.Substring(0, 40)].SetActivity, Does.Contain(listActivityValues[0]!));
             Console.WriteLine("Done TestValidate_MER_UNF_STL_BIW_FTR_CRC_U13");
         }
         catch (Exception exception)
         {
             Assert.Fail("Error in TestValidate_MER_UNF_STL_BIW_FTR_CRC_U13 \n" + exception + "\n");
         }
     }
    
     [TestCase(Category = "SkuUnfBiwTests")]
     [Test(Description = "TestValidate_MER_UNF_STL_BIW_FTR_DRC_U14 with generated merchant"), CancelAfter(120 * 1000)]
     public async Task TestValidate_MER_UNF_STL_BIW_FTR_DRC_U14()
     {
         try
         {
             Console.WriteLine("\nStarting TestValidate_MER_UNF_STL_BIW_FTR_DRC_U14");
             var testName = TestContext.CurrentContext.Test.Name;
             var terminalApiKey = _envConfig.MER_UNF_STL_BIW_FTR;
             Console.WriteLine("Done with creation of a new merchant , settings and terminal process");
             var createPlanDefaultValues = new CreatePlanDefaultValues();
             var is3DsVarsCreation = new Is3DsVarsCreation();
             createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
             createPlanDefaultValues.paymentMethod.card.cardNumber = Environment.GetEnvironmentVariable("DebitCard")!;
             var planCreateResponse = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!, 
                 _requestHeader!,
                 "PendingCapture", 14, terminalApiKey, createPlanDefaultValues);
             var skuQueryResult = await _mongoHandler.SendMongoQueryForSkuAsync(_db!, "Payment_Records",
                 "InstallmentPlanNumber",
                 planCreateResponse.InstallmentPlanNumber, "Sku");
             Assert.That(_skuTestsData.ValidateSkuValue(skuQueryResult, testName.Substring(0, 40)), Is.True);
             var listActivityValues = await _mongoHandler.SendMongoQueryActivityAsync(_db!, "Payment_Records",
                 "InstallmentPlanNumber",
                 planCreateResponse.InstallmentPlanNumber, "Activity");
             Assert.That(listActivityValues, Has.Count.EqualTo(1));
             Assert.That(_skuDict![testName.Substring(0, 40)].SetActivity, Does.Contain(listActivityValues[0]!));
             Console.WriteLine("Done TestValidate_MER_UNF_STL_BIW_FTR_DRC_U14");
         }
         catch (Exception exception)
         {
             Assert.Fail("Error in TestValidate_MER_UNF_STL_BIW_FTR_DRC_U14\n" + exception + "\n");
         }
     }
    
    
     [TestCase(Category = "SkuUnfBiwTests")]
     [Test(Description = "TestValidate_MER_UNF_STL_BIW_FTR_CRC_U14 with generated merchant"), CancelAfter(120 * 1000)]
     public async Task TestValidate_MER_UNF_STL_BIW_FTR_CRC_U14()
     {
         try
         {
             Console.WriteLine("\nStarting TestValidate_MER_UNF_STL_BIW_FTR_CRC_U14");
             var testName = TestContext.CurrentContext.Test.Name;
             var terminalApiKey = _envConfig.MER_UNF_STL_BIW_FTR;
             Console.WriteLine("Done with creation of a new merchant , settings and terminal process");
             var createPlanDefaultValues = new CreatePlanDefaultValues();
             var is3DsVarsCreation = new Is3DsVarsCreation();
             createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
             var planCreateResponse = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!, 
                 _requestHeader!,
                 "Active", 14, terminalApiKey, createPlanDefaultValues);
             var skuQueryResult = await _mongoHandler.SendMongoQueryForSkuAsync(_db!, "Payment_Records",
                 "InstallmentPlanNumber",
                 planCreateResponse.InstallmentPlanNumber, "Sku");
             Assert.That(_skuTestsData.ValidateSkuValue(skuQueryResult, testName.Substring(0, 40)), Is.True);
             var listActivityValues = await _mongoHandler.SendMongoQueryActivityAsync(_db!, "Payment_Records",
                 "InstallmentPlanNumber",
                 planCreateResponse.InstallmentPlanNumber, "Activity");
             Assert.That(listActivityValues, Has.Count.EqualTo(1));
             Assert.That(_skuDict![testName.Substring(0, 40)].SetActivity, Does.Contain(listActivityValues[0]!));
             Console.WriteLine("Done TestValidate_MER_UNF_STL_BIW_FTR_CRC_U14");
         }
         catch (Exception exception)
         {
             Assert.Fail("Error in TestValidate_MER_UNF_STL_BIW_FTR_CRC_U14 \n" + exception + "\n");
         }
     }
    
   
     [TestCase(Category = "SkuUnfBiwTests")]
     [Test(Description = "TestValidate_MER_UNF_STL_BIW_FTR_DRC_U16 with generated merchant"), CancelAfter(120 * 1000)]
     public async Task TestValidate_MER_UNF_STL_BIW_FTR_DRC_U16()
     {
         try
         {
             Console.WriteLine("\nStarting TestValidate_MER_UNF_STL_BIW_FTR_DRC_U16");
             var testName = TestContext.CurrentContext.Test.Name;
             var terminalApiKey = _envConfig.MER_UNF_STL_BIW_FTR;
             Console.WriteLine("Done with creation of a new merchant , settings and terminal process");
             var createPlanDefaultValues = new CreatePlanDefaultValues();
             var is3DsVarsCreation = new Is3DsVarsCreation();
             createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
             createPlanDefaultValues.paymentMethod.card.cardNumber = Environment.GetEnvironmentVariable("DebitCard")!;
             var planCreateResponse = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!, 
                 _requestHeader!,
                 "PendingCapture", 16, terminalApiKey, createPlanDefaultValues);
             var skuQueryResult = await _mongoHandler.SendMongoQueryForSkuAsync(_db!, "Payment_Records",
                 "InstallmentPlanNumber",
                 planCreateResponse.InstallmentPlanNumber, "Sku");
             Assert.That(_skuTestsData.ValidateSkuValue(skuQueryResult, testName.Substring(0, 40)), Is.True);
             var listActivityValues = await _mongoHandler.SendMongoQueryActivityAsync(_db!, "Payment_Records",
                 "InstallmentPlanNumber",
                 planCreateResponse.InstallmentPlanNumber, "Activity");
             Assert.That(listActivityValues, Has.Count.EqualTo(1));
             Assert.That(_skuDict![testName.Substring(0, 40)].SetActivity, Does.Contain(listActivityValues[0]!));
             Console.WriteLine("Done TestValidate_MER_UNF_STL_BIW_FTR_DRC_U16");
         }
         catch (Exception exception)
         {
             Assert.Fail("Error in TestValidate_MER_UNF_STL_BIW_FTR_DRC_U16\n" + exception + "\n");
         }
     }
    
    
     [TestCase(Category = "SkuUnfBiwTests")]
     [Test(Description = "TestValidate_MER_UNF_STL_BIW_FTR_CRC_U16 with generated merchant"), CancelAfter(120 * 1000)]
     public async Task TestValidate_MER_UNF_STL_BIW_FTR_CRC_U16()
     {
         try
         {
             Console.WriteLine("\nStarting TestValidate_MER_UNF_STL_BIW_FTR_CRC_U16");
             var testName = TestContext.CurrentContext.Test.Name;
             var terminalApiKey = _envConfig.MER_UNF_STL_BIW_FTR;
             Console.WriteLine("Done with creation of a new merchant , settings and terminal process");
             var createPlanDefaultValues = new CreatePlanDefaultValues();
             var is3DsVarsCreation = new Is3DsVarsCreation();
             createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
             var planCreateResponse = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!, 
                 _requestHeader!,
                 "Active", 16, terminalApiKey, createPlanDefaultValues);
             var skuQueryResult = await _mongoHandler.SendMongoQueryForSkuAsync(_db!, "Payment_Records",
                 "InstallmentPlanNumber",
                 planCreateResponse.InstallmentPlanNumber, "Sku");
             Assert.That(_skuTestsData.ValidateSkuValue(skuQueryResult, testName.Substring(0, 40)), Is.True);
             var listActivityValues = await _mongoHandler.SendMongoQueryActivityAsync(_db!, "Payment_Records",
                 "InstallmentPlanNumber",
                 planCreateResponse.InstallmentPlanNumber, "Activity");
             Assert.That(listActivityValues, Has.Count.EqualTo(1));
             Assert.That(_skuDict![testName.Substring(0, 40)].SetActivity, Does.Contain(listActivityValues[0]!));
             Console.WriteLine("Done TestValidate_MER_UNF_STL_BIW_FTR_CRC_U16");
         }
         catch (Exception exception)
         {
             Assert.Fail("Error in TestValidate_MER_UNF_STL_BIW_FTR_CRC_U16 \n" + exception + "\n");
         }
     }
    
     [TestCase(Category = "SkuUnfBiwTests")]
     [Test(Description = "TestValidate_MER_UNF_STL_BIW_FTR_DRC_U17 with generated merchant"), CancelAfter(120 * 1000)]
     public async Task TestValidate_MER_UNF_STL_BIW_FTR_DRC_U17()
     {
         try
         {
             Console.WriteLine("\nStarting TestValidate_MER_UNF_STL_BIW_FTR_DRC_U17");
             var testName = TestContext.CurrentContext.Test.Name;
             var terminalApiKey = _envConfig.MER_UNF_STL_BIW_FTR;
             Console.WriteLine("Done with creation of a new merchant , settings and terminal process");
             var createPlanDefaultValues = new CreatePlanDefaultValues();
             var is3DsVarsCreation = new Is3DsVarsCreation();
             createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
             createPlanDefaultValues.paymentMethod.card.cardNumber = Environment.GetEnvironmentVariable("DebitCard")!;
             var planCreateResponse = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!, 
                 _requestHeader!,
                 "PendingCapture", 17, terminalApiKey, createPlanDefaultValues);
             var skuQueryResult = await _mongoHandler.SendMongoQueryForSkuAsync(_db!, "Payment_Records",
                 "InstallmentPlanNumber",
                 planCreateResponse.InstallmentPlanNumber, "Sku");
             Assert.That(_skuTestsData.ValidateSkuValue(skuQueryResult, testName.Substring(0, 40)), Is.True);
             var listActivityValues = await _mongoHandler.SendMongoQueryActivityAsync(_db!, "Payment_Records",
                 "InstallmentPlanNumber",
                 planCreateResponse.InstallmentPlanNumber, "Activity");
             Assert.That(listActivityValues, Has.Count.EqualTo(1));
             Assert.That(_skuDict![testName.Substring(0, 40)].SetActivity, Does.Contain(listActivityValues[0]!));
             Console.WriteLine("Done TestValidate_MER_UNF_STL_BIW_FTR_DRC_U17");
         }
         catch (Exception exception)
         {
             Assert.Fail("Error in TestValidate_MER_UNF_STL_BIW_FTR_DRC_U17\n" + exception + "\n");
         }
     }
    
    
     [TestCase(Category = "SkuUnfBiwTests")]
     [Test(Description = "TestValidate_MER_UNF_STL_BIW_FTR_CRC_U17 with generated merchant"), CancelAfter(120 * 1000)]
     public async Task TestValidate_MER_UNF_STL_BIW_FTR_CRC_U17()
     {
         try
         {
             Console.WriteLine("\nStarting TestValidate_MER_UNF_STL_BIW_FTR_CRC_U17");
             var testName = TestContext.CurrentContext.Test.Name;
             var terminalApiKey = _envConfig.MER_UNF_STL_BIW_FTR;
             Console.WriteLine("Done with creation of a new merchant , settings and terminal process");
             var createPlanDefaultValues = new CreatePlanDefaultValues();
             var is3DsVarsCreation = new Is3DsVarsCreation();
             createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
             var planCreateResponse = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!, 
                 _requestHeader!,
                 "Active", 17, terminalApiKey, createPlanDefaultValues);
             var skuQueryResult = await _mongoHandler.SendMongoQueryForSkuAsync(_db!, "Payment_Records",
                 "InstallmentPlanNumber",
                 planCreateResponse.InstallmentPlanNumber, "Sku");
             Assert.That(_skuTestsData.ValidateSkuValue(skuQueryResult, testName.Substring(0, 40)), Is.True);
             var listActivityValues = await _mongoHandler.SendMongoQueryActivityAsync(_db!, "Payment_Records",
                 "InstallmentPlanNumber",
                 planCreateResponse.InstallmentPlanNumber, "Activity");
             Assert.That(listActivityValues, Has.Count.EqualTo(1));
             Assert.That(_skuDict![testName.Substring(0, 40)].SetActivity, Does.Contain(listActivityValues[0]!));
             Console.WriteLine("Done TestValidate_MER_UNF_STL_BIW_FTR_CRC_U17");
         }
         catch (Exception exception)
         {
             Assert.Fail("Error in TestValidate_MER_UNF_STL_BIW_FTR_CRC_U17\n" + exception + "\n");
         }
     }
    
     [TestCase(Category = "SkuUnfBiwTests")]
     [Test(Description = "TestValidate_MER_UNF_STL_BIW_FTR_DRC_U20 with generated merchant"), CancelAfter(120 * 1000)]
     public async Task TestValidate_MER_UNF_STL_BIW_FTR_DRC_U20()
     {
         try
         {
             Console.WriteLine("\nStarting TestValidate_MER_UNF_STL_BIW_FTR_DRC_U20");
             var testName = TestContext.CurrentContext.Test.Name;
             var terminalApiKey = _envConfig.MER_UNF_STL_BIW_FTR;
             Console.WriteLine("Done with creation of a new merchant , settings and terminal process");
             var createPlanDefaultValues = new CreatePlanDefaultValues();
             var is3DsVarsCreation = new Is3DsVarsCreation();
             createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
             createPlanDefaultValues.paymentMethod.card.cardNumber = Environment.GetEnvironmentVariable("DebitCard")!;
             var planCreateResponse = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!, 
                 _requestHeader!,
                 "PendingCapture", 20, terminalApiKey, createPlanDefaultValues);
              var skuQueryResult = await _mongoHandler.SendMongoQueryForSkuAsync(_db!, "Payment_Records",
                 "InstallmentPlanNumber",
                 planCreateResponse.InstallmentPlanNumber, "Sku");
             Assert.That(_skuTestsData.ValidateSkuValue(skuQueryResult, testName.Substring(0, 40)), Is.True);
             var listActivityValues = await _mongoHandler.SendMongoQueryActivityAsync(_db!, "Payment_Records",
                 "InstallmentPlanNumber",
                 planCreateResponse.InstallmentPlanNumber, "Activity");
             Assert.That(listActivityValues, Has.Count.EqualTo(1));
             Assert.That(_skuDict![testName.Substring(0, 40)].SetActivity, Does.Contain(listActivityValues[0]!));
             Console.WriteLine("Done TestValidate_MER_UNF_STL_BIW_FTR_DRC_U20");
         }
         catch (Exception exception)
         {
             Assert.Fail("Error in TestValidate_MER_UNF_STL_BIW_FTR_DRC_U20\n" + exception + "\n");
         }
     }
    
    
     [TestCase(Category = "SkuUnfBiwTests")]
     [Test(Description = "TestValidate_MER_UNF_STL_BIW_FTR_CRC_U20 with generated merchant"), CancelAfter(120 * 1000)]
     public async Task TestValidate_MER_UNF_STL_BIW_FTR_CRC_U20()
     {
         try
         {
             Console.WriteLine("\nStarting TestValidate_MER_UNF_STL_BIW_FTR_CRC_U20");
             var testName = TestContext.CurrentContext.Test.Name;
             var terminalApiKey = _envConfig.MER_UNF_STL_BIW_FTR_CRC_U20;
             Console.WriteLine("Done with creation of a new merchant , settings and terminal process");
             var createPlanDefaultValues = new CreatePlanDefaultValues();
             var is3DsVarsCreation = new Is3DsVarsCreation();
             createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
             var planCreateResponse = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!, 
                 _requestHeader!, "Active", 20, terminalApiKey, createPlanDefaultValues);
             var skuQueryResult = await _mongoHandler.SendMongoQueryForSkuAsync(_db!, "Payment_Records",
                 "InstallmentPlanNumber",
                 planCreateResponse.InstallmentPlanNumber, "Sku");
             Assert.That(_skuTestsData.ValidateSkuValue(skuQueryResult, testName.Substring(0, 40)), Is.True);
             var listActivityValues = await _mongoHandler.SendMongoQueryActivityAsync(_db!, "Payment_Records",
                 "InstallmentPlanNumber",
                 planCreateResponse.InstallmentPlanNumber, "Activity");
             Assert.That(listActivityValues, Has.Count.EqualTo(1));
             Assert.That(_skuDict![testName.Substring(0, 40)].SetActivity, Does.Contain(listActivityValues[0]!));
             Console.WriteLine("Done TestValidate_MER_UNF_STL_BIW_FTR_CRC_U20");
         }
         catch (Exception exception)
         {
             Assert.Fail("Error in TestValidate_MER_UNF_STL_BIW_FTR_CRC_U20\n" + exception + "\n");
         }
     }

    [TestCase(Category = "SkuUnfBiwTests")]
    [Test(Description = "TestValidate_MER_UNF_STL_BIW_FTR_DRC_U09 with generated merchant"), CancelAfter(120 * 1000)]
    public async Task TestValidate_MER_UNF_STL_BIW_FTR_DRC_U09()
    {
        try
        {
            Console.WriteLine("\nStarting TestValidate_MER_UNF_STL_BIW_FTR_DRC_U09");
            var testName = TestContext.CurrentContext.Test.Name;
            var terminalApiKey = _envConfig.MER_UNF_STL_BIW_FTR;
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            createPlanDefaultValues.paymentMethod.card.cardNumber = Environment.GetEnvironmentVariable("DebitCard")!;
            var planCreateResponse = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!,
                _requestHeader!,
                "PendingCapture", 9, terminalApiKey, createPlanDefaultValues);
            var skuQueryResult = await _mongoHandler.SendMongoQueryForSkuAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber",
                planCreateResponse.InstallmentPlanNumber, "Sku");
            Assert.That(_skuTestsData.ValidateSkuValue(skuQueryResult, testName.Substring(0, 40)), Is.True);
            var listActivityValues = await _mongoHandler.SendMongoQueryActivityAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber",
                planCreateResponse.InstallmentPlanNumber, "Activity");
            Assert.That(listActivityValues.Count >= 1);
            Assert.That(_skuDict![testName.Substring(0, 40)].SetActivity, Does.Contain(listActivityValues[0]!));
            Console.WriteLine("Done TestValidate_MER_UNF_STL_BIW_FTR_DRC_U09");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidate_MER_UNF_STL_BIW_FTR_DRC_U09 \n" + exception + "\n");
        }
    }


    [TestCase(Category = "SkuUnfBiwTests")]
    [Test(Description = "TestValidate_MER_UNF_STL_BIW_FTR_CRC_U09 with generated merchant"), CancelAfter(120 * 1000)]
    public async Task TestValidate_MER_UNF_STL_BIW_FTR_CRC_U09()
    {
        try
        {
            Console.WriteLine("\nStarting TestValidate_MER_UNF_STL_BIW_FTR_CRC_U09");
            var testName = TestContext.CurrentContext.Test.Name;
            var terminalApiKey = _envConfig.MER_UNF_STL_BIW_FTR_CRC_U09;
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            var planCreateResponse = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!,
                _requestHeader!,
                "Active", 9, terminalApiKey, createPlanDefaultValues);
            var skuQueryResult = await _mongoHandler.SendMongoQueryForSkuAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber",
                planCreateResponse.InstallmentPlanNumber, "Sku");
            Assert.That(_skuTestsData.ValidateSkuValue(skuQueryResult, testName.Substring(0, 40)), Is.True);
            var listActivityValues = await _mongoHandler.SendMongoQueryActivityAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber",
                planCreateResponse.InstallmentPlanNumber, "Activity");
            Assert.That(listActivityValues.Count >= 1);
            Assert.That(_skuDict![testName.Substring(0, 40)].SetActivity, Does.Contain(listActivityValues[0]!));
            Console.WriteLine("Done TestValidate_MER_UNF_STL_BIW_FTR_CRC_U09");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidate_MER_UNF_STL_BIW_FTR_CRC_U09 \n" + exception + "\n");
        }
    }
    
     [TestCase(Category = "SkuUnfBiwTests")]
     [Test(Description = "TestValidate_MER_UNF_STL_BIW_FTR_DRC_U22 with generated merchant"), CancelAfter(120 * 1000)]
     public async Task TestValidate_MER_UNF_STL_BIW_FTR_DRC_U22()
     {
         try
         {
             Console.WriteLine("\nStarting TestValidate_MER_UNF_STL_BIW_FTR_DRC_U22");
             var testName = TestContext.CurrentContext.Test.Name;
             var terminalApiKey = _envConfig.MER_UNF_STL_BIW_FTR;
             Console.WriteLine("Done with creation of a new merchant , settings and terminal process");
             var createPlanDefaultValues = new CreatePlanDefaultValues();
             var is3DsVarsCreation = new Is3DsVarsCreation();
             createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
             createPlanDefaultValues.paymentMethod.card.cardNumber = Environment.GetEnvironmentVariable("DebitCard")!;
             var planCreateResponse = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!, 
                 _requestHeader!,
                 "PendingCapture", 22, terminalApiKey, createPlanDefaultValues);
             var skuQueryResult = await _mongoHandler.SendMongoQueryForSkuAsync(_db!, "Payment_Records",
                 "InstallmentPlanNumber",
                 planCreateResponse.InstallmentPlanNumber, "Sku");
             Assert.That(_skuTestsData.ValidateSkuValue(skuQueryResult, testName.Substring(0, 40)), Is.True);
             var listActivityValues = await _mongoHandler.SendMongoQueryActivityAsync(_db!, "Payment_Records",
                 "InstallmentPlanNumber",
                 planCreateResponse.InstallmentPlanNumber, "Activity");
             Assert.That(listActivityValues, Has.Count.EqualTo(1));
             Assert.That(_skuDict![testName.Substring(0, 40)].SetActivity, Does.Contain(listActivityValues[0]!));
             Console.WriteLine("Done TestValidate_MER_UNF_STL_BIW_FTR_DRC_U22");
         }
         catch (Exception exception)
         {
             Assert.Fail("Error in TestValidate_MER_UNF_STL_BIW_FTR_DRC_U22\n" + exception + "\n");
         }
     }
    
    
     [TestCase(Category = "SkuUnfBiwTests")]
     [Test(Description = "TestValidate_MER_UNF_STL_BIW_FTR_CRC_U22 with generated merchant"), CancelAfter(120 * 1000)]
     public async Task TestValidate_MER_UNF_STL_BIW_FTR_CRC_U22()
     {
         try
         {
             Console.WriteLine("\nStarting TestValidate_MER_UNF_STL_BIW_FTR_CRC_U19");
             var testName = TestContext.CurrentContext.Test.Name;
             var terminalApiKey = _envConfig.MER_UNF_STL_BIW_FTR;
             Console.WriteLine("Done with creation of a new merchant , settings and terminal process");
             var createPlanDefaultValues = new CreatePlanDefaultValues();
             var is3DsVarsCreation = new Is3DsVarsCreation();
             createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
             var planCreateResponse = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!, 
                 _requestHeader!,
                 "Active", 22, terminalApiKey, createPlanDefaultValues);
             var skuQueryResult = await _mongoHandler.SendMongoQueryForSkuAsync(_db!, "Payment_Records",
                 "InstallmentPlanNumber",
                 planCreateResponse.InstallmentPlanNumber, "Sku");
             Assert.That(_skuTestsData.ValidateSkuValue(skuQueryResult, testName.Substring(0, 40)), Is.True);
             var listActivityValues = await _mongoHandler.SendMongoQueryActivityAsync(_db!, "Payment_Records",
                 "InstallmentPlanNumber",
                 planCreateResponse.InstallmentPlanNumber, "Activity");
             Assert.That(listActivityValues, Has.Count.EqualTo(1));
             Assert.That(_skuDict![testName.Substring(0, 40)].SetActivity, Does.Contain(listActivityValues[0]!));
             Console.WriteLine("Done TestValidate_MER_UNF_STL_BIW_FTR_CRC_U22");
         }
         catch (Exception exception)
         {
             Assert.Fail("Error in TestValidate_MER_UNF_STL_BIW_FTR_CRC_U22 \n" + exception + "\n");
         }
     }
    
     [TestCase(Category = "SkuUnfBiwTests")]
     [Test(Description = "TestValidate_MER_UNF_STL_BIW_FTR_DRC_U23 with generated merchant"), CancelAfter(120 * 1000)]
     public async Task TestValidate_MER_UNF_STL_BIW_FTR_DRC_U23()
     {
         try
         {
             Console.WriteLine("\nStarting TestValidate_MER_UNF_STL_BIW_FTR_DRC_U23");
             var testName = TestContext.CurrentContext.Test.Name;
             var terminalApiKey = _envConfig.MER_UNF_STL_BIW_FTR;
             Console.WriteLine("Done with creation of a new merchant , settings and terminal process");
             var createPlanDefaultValues = new CreatePlanDefaultValues();
             var is3DsVarsCreation = new Is3DsVarsCreation();
             createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
             createPlanDefaultValues.paymentMethod.card.cardNumber = Environment.GetEnvironmentVariable("DebitCard")!;
             var planCreateResponse = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!, 
                 _requestHeader!,
                 "PendingCapture", 20, terminalApiKey, createPlanDefaultValues);
             var skuQueryResult = await _mongoHandler.SendMongoQueryForSkuAsync(_db!, "Payment_Records",
                 "InstallmentPlanNumber",
                 planCreateResponse.InstallmentPlanNumber, "Sku");
             Assert.That(_skuTestsData.ValidateSkuValue(skuQueryResult, testName.Substring(0, 40)), Is.True);
             var listActivityValues = await _mongoHandler.SendMongoQueryActivityAsync(_db!, "Payment_Records",
                 "InstallmentPlanNumber",
                 planCreateResponse.InstallmentPlanNumber, "Activity");
             Assert.That(listActivityValues, Has.Count.EqualTo(1));
             Assert.That(_skuDict![testName.Substring(0, 40)].SetActivity, Does.Contain(listActivityValues[0]!));
             Console.WriteLine("Done TestValidate_MER_UNF_STL_BIW_FTR_DRC_U23");
         }
         catch (Exception exception)
         {
             Assert.Fail("Error in TestValidate_MER_UNF_STL_BIW_FTR_DRC_U23\n" + exception + "\n");
         }
     }
    
    
     [TestCase(Category = "SkuUnfBiwTests")]
     [Test(Description = "TestValidate_MER_UNF_STL_BIW_FTR_CRC_U23 with generated merchant"), CancelAfter(120 * 1000)]
     public async Task TestValidate_MER_UNF_STL_BIW_FTR_CRC_U23()
     {
         try
         {
             Console.WriteLine("\nStarting TestValidate_MER_UNF_STL_BIW_FTR_CRC_U20");
             var testName = TestContext.CurrentContext.Test.Name;
             var terminalApiKey = _envConfig.MER_UNF_STL_BIW_FTR;
             Console.WriteLine("Done with creation of a new merchant , settings and terminal process");
             var createPlanDefaultValues = new CreatePlanDefaultValues();
             var is3DsVarsCreation = new Is3DsVarsCreation();
             createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
             var planCreateResponse = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!, 
                 _requestHeader!, "Active", 23, terminalApiKey, createPlanDefaultValues);
             var skuQueryResult = await _mongoHandler.SendMongoQueryForSkuAsync(_db!, "Payment_Records",
                 "InstallmentPlanNumber",
                 planCreateResponse.InstallmentPlanNumber, "Sku");
             Assert.That(_skuTestsData.ValidateSkuValue(skuQueryResult, testName.Substring(0, 40)), Is.True);
             var listActivityValues = await _mongoHandler.SendMongoQueryActivityAsync(_db!, "Payment_Records",
                 "InstallmentPlanNumber",
                 planCreateResponse.InstallmentPlanNumber, "Activity");
             Assert.That(listActivityValues, Has.Count.EqualTo(1));
             Assert.That(_skuDict![testName.Substring(0, 40)].SetActivity, Does.Contain(listActivityValues[0]!));
             Console.WriteLine("Done TestValidate_MER_UNF_STL_BIW_FTR_CRC_U23");
         }
         catch (Exception exception)
         {
             Assert.Fail("Error in TestValidate_MER_UNF_STL_BIW_FTR_CRC_U23\n" + exception + "\n");
         }
     }
    
     [TestCase(Category = "SkuUnfBiwTests")]
     [Test(Description = "TestValidate_MER_UNF_STL_BIW_FTR_DRC_U25 with generated merchant"), CancelAfter(120 * 1000)]
     public async Task TestValidate_MER_UNF_STL_BIW_FTR_DRC_U25()
     {
         try
         {
             Console.WriteLine("\nStarting TestValidate_MER_UNF_STL_BIW_FTR_DRC_U25");
             var testName = TestContext.CurrentContext.Test.Name;
             var terminalApiKey = _envConfig.MER_UNF_STL_BIW_FTR;
             Console.WriteLine("Done with creation of a new merchant , settings and terminal process");
             var createPlanDefaultValues = new CreatePlanDefaultValues();
             var is3DsVarsCreation = new Is3DsVarsCreation();
             createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
             createPlanDefaultValues.paymentMethod.card.cardNumber = Environment.GetEnvironmentVariable("DebitCard")!;
             var planCreateResponse = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!, 
                 _requestHeader!,
                 "PendingCapture", 25, terminalApiKey, createPlanDefaultValues);
             var skuQueryResult = await _mongoHandler.SendMongoQueryForSkuAsync(_db!, "Payment_Records",
                 "InstallmentPlanNumber",
                 planCreateResponse.InstallmentPlanNumber, "Sku");
             Assert.That(_skuTestsData.ValidateSkuValue(skuQueryResult, testName.Substring(0, 40)), Is.True);
             var listActivityValues = await _mongoHandler.SendMongoQueryActivityAsync(_db!, "Payment_Records",
                 "InstallmentPlanNumber",
                 planCreateResponse.InstallmentPlanNumber, "Activity");
             Assert.That(listActivityValues, Has.Count.EqualTo(1));
             Assert.That(_skuDict![testName.Substring(0, 40)].SetActivity, Does.Contain(listActivityValues[0]!));
             Console.WriteLine("Done TestValidate_MER_UNF_STL_BIW_FTR_DRC_U25");
         }
         catch (Exception exception)
         {
             Assert.Fail("Error in TestValidate_MER_UNF_STL_BIW_FTR_DRC_U25\n" + exception + "\n");
         }
     }
    
    
     [TestCase(Category = "SkuUnfBiwTests")]
     [Test(Description = "TestValidate_MER_UNF_STL_BIW_FTR_CRC_U25 with generated merchant"), CancelAfter(120 * 1000)]
     public async Task TestValidate_MER_UNF_STL_BIW_FTR_CRC_U25()
     {
         try
         {
             Console.WriteLine("\nStarting TestValidate_MER_UNF_STL_BIW_FTR_CRC_U25");
             var testName = TestContext.CurrentContext.Test.Name;
             var terminalApiKey = _envConfig.MER_UNF_STL_BIW_FTR;
             Console.WriteLine("Done with creation of a new merchant , settings and terminal process");
             var createPlanDefaultValues = new CreatePlanDefaultValues();
             var is3DsVarsCreation = new Is3DsVarsCreation();
             createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
             var planCreateResponse = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!, 
                 _requestHeader!,
                 "Active", 25, terminalApiKey, createPlanDefaultValues);
             var skuQueryResult = await _mongoHandler.SendMongoQueryForSkuAsync(_db!, "Payment_Records",
                 "InstallmentPlanNumber",
                 planCreateResponse.InstallmentPlanNumber, "Sku");
             Assert.That(_skuTestsData.ValidateSkuValue(skuQueryResult, testName.Substring(0, 40)), Is.True);
             var listActivityValues = await _mongoHandler.SendMongoQueryActivityAsync(_db!, "Payment_Records",
                 "InstallmentPlanNumber",
                 planCreateResponse.InstallmentPlanNumber, "Activity");
             Assert.That(listActivityValues, Has.Count.EqualTo(1));
             Assert.That(_skuDict![testName.Substring(0, 40)].SetActivity, Does.Contain(listActivityValues[0]!));
             Console.WriteLine("Done TestValidate_MER_UNF_STL_BIW_FTR_CRC_U25");
         }
         catch (Exception exception)
         {
             Assert.Fail("Error in TestValidate_MER_UNF_STL_BIW_FTR_CRC_U25 \n" + exception + "\n");
         }
     }
    
     [TestCase(Category = "SkuUnfBiwTests")]
     [Test(Description = "TestValidate_MER_UNF_STL_BIW_FTR_DRC_U26 with generated merchant"), CancelAfter(120 * 1000)]
     public async Task TestValidate_MER_UNF_STL_BIW_FTR_DRC_U26()
     {
         try
         {
             Console.WriteLine("\nStarting TestValidate_MER_UNF_STL_BIW_FTR_DRC_U26");
             var testName = TestContext.CurrentContext.Test.Name;
             var terminalApiKey = _envConfig.MER_UNF_STL_BIW_FTR;
             Console.WriteLine("Done with creation of a new merchant , settings and terminal process");
             var createPlanDefaultValues = new CreatePlanDefaultValues();
             var is3DsVarsCreation = new Is3DsVarsCreation();
             createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
             createPlanDefaultValues.paymentMethod.card.cardNumber = Environment.GetEnvironmentVariable("DebitCard")!;
             var planCreateResponse = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!, 
                 _requestHeader!,
                 "PendingCapture", 26, terminalApiKey, createPlanDefaultValues);
             var skuQueryResult = await _mongoHandler.SendMongoQueryForSkuAsync(_db!, "Payment_Records",
                 "InstallmentPlanNumber",
                 planCreateResponse.InstallmentPlanNumber, "Sku");
             Assert.That(_skuTestsData.ValidateSkuValue(skuQueryResult, testName.Substring(0, 40)), Is.True);
             var listActivityValues = await _mongoHandler.SendMongoQueryActivityAsync(_db!, "Payment_Records",
                 "InstallmentPlanNumber",
                 planCreateResponse.InstallmentPlanNumber, "Activity");
             Assert.That(listActivityValues, Has.Count.EqualTo(1));
             Assert.That(_skuDict![testName.Substring(0, 40)].SetActivity, Does.Contain(listActivityValues[0]!));
             Console.WriteLine("Done TestValidate_MER_UNF_STL_BIW_FTR_DRC_U26");
         }
         catch (Exception exception)
         {
             Assert.Fail("Error in TestValidate_MER_UNF_STL_BIW_FTR_DRC_U26\n" + exception + "\n");
         }
     }
    
    
     [TestCase(Category = "SkuUnfBiwTests")]
     [Test(Description = "TestValidate_MER_UNF_STL_BIW_FTR_CRC_U26 with generated merchant"), CancelAfter(120 * 1000)]
     public async Task TestValidate_MER_UNF_STL_BIW_FTR_CRC_U26()
     {
         try
         {
             Console.WriteLine("\nStarting TestValidate_MER_UNF_STL_BIW_FTR_CRC_U26");
             var testName = TestContext.CurrentContext.Test.Name;
             var terminalApiKey = _envConfig.MER_UNF_STL_BIW_FTR;
             Console.WriteLine("Done with creation of a new merchant , settings and terminal process");
             var createPlanDefaultValues = new CreatePlanDefaultValues();
             var is3DsVarsCreation = new Is3DsVarsCreation();
             createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
             var planCreateResponse = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!, 
                 _requestHeader!,
                 "Active", 26, terminalApiKey, createPlanDefaultValues);
             var skuQueryResult = await _mongoHandler.SendMongoQueryForSkuAsync(_db!, "Payment_Records",
                 "InstallmentPlanNumber",
                 planCreateResponse.InstallmentPlanNumber, "Sku");
             Assert.That(_skuTestsData.ValidateSkuValue(skuQueryResult, testName.Substring(0, 40)), Is.True);
             var listActivityValues = await _mongoHandler.SendMongoQueryActivityAsync(_db!, "Payment_Records",
                 "InstallmentPlanNumber",
                 planCreateResponse.InstallmentPlanNumber, "Activity");
             Assert.That(listActivityValues, Has.Count.EqualTo(1));
             Assert.That(_skuDict![testName.Substring(0, 40)].SetActivity, Does.Contain(listActivityValues[0]!));
             Console.WriteLine("Done TestValidate_MER_UNF_STL_BIW_FTR_CRC_U26");
         }
         catch (Exception exception)
         {
             Assert.Fail("Error in TestValidate_MER_UNF_STL_BIW_FTR_CRC_U26\n" + exception + "\n");
         }
     }
     [TestCase(Category = "SkuUnfBiwTests")]
     [Test(Description = "TestValidate_MER_UNF_STL_BIW_FTR_DRC_U27 with generated merchant"), CancelAfter(120 * 1000)]
     public async Task TestValidate_MER_UNF_STL_BIW_FTR_DRC_U27()
     {
         try
         {
             Console.WriteLine("\nStarting TestValidate_MER_UNF_STL_BIW_FTR_DRC_U27");
             var testName = TestContext.CurrentContext.Test.Name;
             var terminalApiKey = _envConfig.MER_UNF_STL_BIW_FTR;
             Console.WriteLine("Done with creation of a new merchant , settings and terminal process");
             var createPlanDefaultValues = new CreatePlanDefaultValues();
             var is3DsVarsCreation = new Is3DsVarsCreation();
             createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
             createPlanDefaultValues.paymentMethod.card.cardNumber = Environment.GetEnvironmentVariable("DebitCard")!;
             var planCreateResponse = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!, 
                 _requestHeader!,
                 "PendingCapture", 27, terminalApiKey, createPlanDefaultValues);
             var skuQueryResult = await _mongoHandler.SendMongoQueryForSkuAsync(_db!, "Payment_Records",
                 "InstallmentPlanNumber",
                 planCreateResponse.InstallmentPlanNumber, "Sku");
             Assert.That(_skuTestsData.ValidateSkuValue(skuQueryResult, testName.Substring(0, 40)), Is.True);
             var listActivityValues = await _mongoHandler.SendMongoQueryActivityAsync(_db!, "Payment_Records",
                 "InstallmentPlanNumber",
                 planCreateResponse.InstallmentPlanNumber, "Activity");
             Assert.That(listActivityValues, Has.Count.EqualTo(1));
             Assert.That(_skuDict![testName.Substring(0, 40)].SetActivity, Does.Contain(listActivityValues[0]!));
             Console.WriteLine("Done TestValidate_MER_UNF_STL_BIW_FTR_DRC_U27");
         }
         catch (Exception exception)
         {
             Assert.Fail("Error in TestValidate_MER_UNF_STL_BIW_FTR_DRC_U27\n" + exception + "\n");
         }
     }
    
    
     [TestCase(Category = "SkuUnfBiwTests")]
     [Test(Description = "TestValidate_MER_UNF_STL_BIW_FTR_CRC_U27 with generated merchant"), CancelAfter(120 * 1000)]
     public async Task TestValidate_MER_UNF_STL_BIW_FTR_CRC_U27()
     {
         try
         {
             Console.WriteLine("\nStarting TestValidate_MER_UNF_STL_BIW_FTR_CRC_U27");
             var testName = TestContext.CurrentContext.Test.Name;
             var terminalApiKey = _envConfig.MER_UNF_STL_BIW_FTR;
             Console.WriteLine("Done with creation of a new merchant , settings and terminal process");
             var createPlanDefaultValues = new CreatePlanDefaultValues();
             var is3DsVarsCreation = new Is3DsVarsCreation();
             createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
             var planCreateResponse = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!, 
                 _requestHeader!,
                 "Active", 27, terminalApiKey, createPlanDefaultValues);
             var skuQueryResult = await _mongoHandler.SendMongoQueryForSkuAsync(_db!, "Payment_Records",
                 "InstallmentPlanNumber",
                 planCreateResponse.InstallmentPlanNumber, "Sku");
             Assert.That(_skuTestsData.ValidateSkuValue(skuQueryResult, testName.Substring(0, 40)), Is.True);
             var listActivityValues = await _mongoHandler.SendMongoQueryActivityAsync(_db!, "Payment_Records",
                 "InstallmentPlanNumber",
                 planCreateResponse.InstallmentPlanNumber, "Activity");
             Assert.That(listActivityValues, Has.Count.EqualTo(1));
             Assert.That(_skuDict![testName.Substring(0, 40)].SetActivity, Does.Contain(listActivityValues[0]!));
             Console.WriteLine("Done TestValidate_MER_UNF_STL_BIW_FTR_CRC_U27");
         }
         catch (Exception exception)
         {
             Assert.Fail("Error in TestValidate_MER_UNF_STL_BIW_FTR_CRC_U27 \n" + exception + "\n");
         }
     }
    
     [TestCase(Category = "SkuUnfBiwTests")]
     [Test(Description = "TestValidate_MER_UNF_STL_BIW_FTR_DRC_U28 with generated merchant"), CancelAfter(120 * 1000)]
     public async Task TestValidate_MER_UNF_STL_BIW_FTR_DRC_U28()
     {
         try
         {
             Console.WriteLine("\nStarting TestValidate_MER_UNF_STL_BIW_FTR_DRC_U28");
             var testName = TestContext.CurrentContext.Test.Name;
             var terminalApiKey = _envConfig.MER_UNF_STL_BIW_FTR;
             Console.WriteLine("Done with creation of a new merchant , settings and terminal process");
             var createPlanDefaultValues = new CreatePlanDefaultValues();
             var is3DsVarsCreation = new Is3DsVarsCreation();
             createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
             createPlanDefaultValues.paymentMethod.card.cardNumber = Environment.GetEnvironmentVariable("DebitCard")!;
             var planCreateResponse = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!, 
                 _requestHeader!,
                 "PendingCapture", 28, terminalApiKey, createPlanDefaultValues);
             var skuQueryResult = await _mongoHandler.SendMongoQueryForSkuAsync(_db!, "Payment_Records",
                 "InstallmentPlanNumber",
                 planCreateResponse.InstallmentPlanNumber, "Sku");
             Assert.That(_skuTestsData.ValidateSkuValue(skuQueryResult, testName.Substring(0, 40)), Is.True);
             var listActivityValues = await _mongoHandler.SendMongoQueryActivityAsync(_db!, "Payment_Records",
                 "InstallmentPlanNumber",
                 planCreateResponse.InstallmentPlanNumber, "Activity");
             Assert.That(listActivityValues, Has.Count.EqualTo(1));
             Assert.That(_skuDict![testName.Substring(0, 40)].SetActivity, Does.Contain(listActivityValues[0]!));
             Console.WriteLine("Done TestValidate_MER_UNF_STL_BIW_FTR_DRC_U28");
         }
         catch (Exception exception)
         {
             Assert.Fail("Error in TestValidate_MER_UNF_STL_BIW_FTR_DRC_U28\n" + exception + "\n");
         }
     }
    
    
     [TestCase(Category = "SkuUnfBiwTests")]
     [Test(Description = "TestValidate_MER_UNF_STL_BIW_FTR_CRC_U28 with generated merchant"), CancelAfter(120 * 1000)]
     public async Task TestValidate_MER_UNF_STL_BIW_FTR_CRC_U28()
     {
         try
         {
             Console.WriteLine("\nStarting TestValidate_MER_UNF_STL_BIW_FTR_CRC_U28");
             var testName = TestContext.CurrentContext.Test.Name;
             var terminalApiKey = _envConfig.MER_UNF_STL_BIW_FTR;
             Console.WriteLine("Done with creation of a new merchant , settings and terminal process");
             var createPlanDefaultValues = new CreatePlanDefaultValues();
             var is3DsVarsCreation = new Is3DsVarsCreation();
             createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
             var planCreateResponse = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!, 
                 _requestHeader!,
                 "Active", 28, terminalApiKey, createPlanDefaultValues);
             var skuQueryResult = await _mongoHandler.SendMongoQueryForSkuAsync(_db!, "Payment_Records",
                 "InstallmentPlanNumber",
                 planCreateResponse.InstallmentPlanNumber, "Sku");
             Assert.That(_skuTestsData.ValidateSkuValue(skuQueryResult, testName.Substring(0, 40)), Is.True);
             var listActivityValues = await _mongoHandler.SendMongoQueryActivityAsync(_db!, "Payment_Records",
                 "InstallmentPlanNumber",
                 planCreateResponse.InstallmentPlanNumber, "Activity");
             Assert.That(listActivityValues, Has.Count.EqualTo(1));
             Assert.That(_skuDict![testName.Substring(0, 40)].SetActivity, Does.Contain(listActivityValues[0]!));
             Console.WriteLine("Done TestValidate_MER_UNF_STL_BIW_FTR_CRC_U28");
         }
         catch (Exception exception)
         {
             Assert.Fail("Error in TestValidate_MER_UNF_STL_BIW_FTR_CRC_U28\n" + exception + "\n");
         }
     }

     [TestCase(Category = "SkuUnfBiwTests")]
     [Test(Description = "TestValidate_MER_UNF_STL_BIW_FTR_CRC_U29 with generated merchant"), CancelAfter(120 * 1000)]
     public async Task TestValidate_MER_UNF_STL_BIW_FTR_CRC_U29()
     {
         try
         {
             Console.WriteLine("\nStarting TestValidate_MER_UNF_STL_BIW_FTR_CRC_U25");
             var testName = TestContext.CurrentContext.Test.Name;
             var terminalApiKey = _envConfig.MER_UNF_STL_BIW_FTR;
             Console.WriteLine("Done with creation of a new merchant , settings and terminal process");
             var createPlanDefaultValues = new CreatePlanDefaultValues();
             var is3DsVarsCreation = new Is3DsVarsCreation();
             createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
             var planCreateResponse = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!, 
                 _requestHeader!,
                 "Active", 29, terminalApiKey, createPlanDefaultValues);
             var skuQueryResult = await _mongoHandler.SendMongoQueryForSkuAsync(_db!, "Payment_Records",
                 "InstallmentPlanNumber",
                 planCreateResponse.InstallmentPlanNumber, "Sku");
             Assert.That(_skuTestsData.ValidateSkuValue(skuQueryResult, testName.Substring(0, 40)), Is.True);
             var listActivityValues = await _mongoHandler.SendMongoQueryActivityAsync(_db!, "Payment_Records",
                 "InstallmentPlanNumber",
                 planCreateResponse.InstallmentPlanNumber, "Activity");
             Assert.That(listActivityValues, Has.Count.EqualTo(1));
             Assert.That(_skuDict![testName.Substring(0, 40)].SetActivity, Does.Contain(listActivityValues[0]!));
             Console.WriteLine("Done TestValidate_MER_UNF_STL_BIW_FTR_CRC_U29");
         }
         catch (Exception exception)
         {
             Assert.Fail("Error in TestValidate_MER_UNF_STL_BIW_FTR_CRC_U29 \n" + exception + "\n");
         }
     }
    
     [TestCase(Category = "SkuUnfBiwTests")]
     [Test(Description = "TestValidate_MER_UNF_STL_BIW_FTR_DRC_U30 with generated merchant"), CancelAfter(120 * 1000)]
     public async Task TestValidate_MER_UNF_STL_BIW_FTR_DRC_U30()
     {
         try
         {
             Console.WriteLine("\nStarting TestValidate_MER_UNF_STL_BIW_FTR_DRC_U30");
             var testName = TestContext.CurrentContext.Test.Name;
             var terminalApiKey = _envConfig.MER_UNF_STL_BIW_FTR;
             Console.WriteLine("Done with creation of a new merchant , settings and terminal process");
             var createPlanDefaultValues = new CreatePlanDefaultValues();
             var is3DsVarsCreation = new Is3DsVarsCreation();
             createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
             createPlanDefaultValues.paymentMethod.card.cardNumber = Environment.GetEnvironmentVariable("DebitCard")!;
             var planCreateResponse = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!, 
                 _requestHeader!,
                 "PendingCapture", 30, terminalApiKey, createPlanDefaultValues);
             var skuQueryResult = await _mongoHandler.SendMongoQueryForSkuAsync(_db!, "Payment_Records",
                 "InstallmentPlanNumber",
                 planCreateResponse.InstallmentPlanNumber, "Sku");
             Assert.That(_skuTestsData.ValidateSkuValue(skuQueryResult, testName.Substring(0, 40)), Is.True);
             var listActivityValues = await _mongoHandler.SendMongoQueryActivityAsync(_db!, "Payment_Records",
                 "InstallmentPlanNumber",
                 planCreateResponse.InstallmentPlanNumber, "Activity");
             Assert.That(listActivityValues, Has.Count.EqualTo(1));
             Assert.That(_skuDict![testName.Substring(0, 40)].SetActivity, Does.Contain(listActivityValues[0]!));
             Console.WriteLine("Done TestValidate_MER_UNF_STL_BIW_FTR_DRC_U30");
         }
         catch (Exception exception)
         {
             Assert.Fail("Error in TestValidate_MER_UNF_STL_BIW_FTR_DRC_U30\n" + exception + "\n");
         }
     }
    
    
     [TestCase(Category = "SkuUnfBiwTests")]
     [Test(Description = "TestValidate_MER_UNF_STL_BIW_FTR_CRC_U30 with generated merchant"), CancelAfter(120 * 1000)]
     public async Task TestValidate_MER_UNF_STL_BIW_FTR_CRC_U30()
     {
         try
         {
             Console.WriteLine("\nStarting TestValidate_MER_UNF_STL_BIW_FTR_CRC_U30");
             var testName = TestContext.CurrentContext.Test.Name;
             var terminalApiKey = _envConfig.MER_UNF_STL_BIW_FTR;
             Console.WriteLine("Done with creation of a new merchant , settings and terminal process");
             var createPlanDefaultValues = new CreatePlanDefaultValues();
             var is3DsVarsCreation = new Is3DsVarsCreation();
             createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
             var planCreateResponse = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!, 
                 _requestHeader!,
                 "Active", 30, terminalApiKey, createPlanDefaultValues);
             var skuQueryResult = await _mongoHandler.SendMongoQueryForSkuAsync(_db!, "Payment_Records",
                 "InstallmentPlanNumber",
                 planCreateResponse.InstallmentPlanNumber, "Sku");
             Assert.That(_skuTestsData.ValidateSkuValue(skuQueryResult, testName.Substring(0, 40)), Is.True);
             var listActivityValues = await _mongoHandler.SendMongoQueryActivityAsync(_db!, "Payment_Records",
                 "InstallmentPlanNumber",
                 planCreateResponse.InstallmentPlanNumber, "Activity");
             Assert.That(listActivityValues, Has.Count.EqualTo(1));
             Assert.That(_skuDict![testName.Substring(0, 40)].SetActivity, Does.Contain(listActivityValues[0]!));
             Console.WriteLine("Done TestValidate_MER_UNF_STL_BIW_FTR_CRC_U30");
         }
         catch (Exception exception)
         {
             Assert.Fail("Error in TestValidate_MER_UNF_STL_BIW_FTR_CRC_U30\n" + exception + "\n");
         }
     }
     
     [TestCase(Category = "SkuUnfBiwTests")]
     [Test(Description = "TestValidate_MER_UNF_STL_BIW_FTR_DRC_U31 with generated merchant"), CancelAfter(120 * 1000)]
     public async Task TestValidate_MER_UNF_STL_BIW_FTR_DRC_U31()
     {
         try
         {
             Console.WriteLine("\nStarting TestValidate_MER_UNF_STL_BIW_FTR_DRC_U31");
             var testName = TestContext.CurrentContext.Test.Name;
             var terminalApiKey = _envConfig.MER_UNF_STL_BIW_FTR;
             Console.WriteLine("Done with creation of a new merchant , settings and terminal process");
             var createPlanDefaultValues = new CreatePlanDefaultValues();
             var is3DsVarsCreation = new Is3DsVarsCreation();
             createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
             createPlanDefaultValues.paymentMethod.card.cardNumber = Environment.GetEnvironmentVariable("DebitCard")!;
             var planCreateResponse = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!, 
                 _requestHeader!,
                 "PendingCapture", 31, terminalApiKey, createPlanDefaultValues);
             var skuQueryResult = await _mongoHandler.SendMongoQueryForSkuAsync(_db!, "Payment_Records",
                 "InstallmentPlanNumber",
                 planCreateResponse.InstallmentPlanNumber, "Sku");
             Assert.That(_skuTestsData.ValidateSkuValue(skuQueryResult, testName.Substring(0, 40)), Is.True);
             var listActivityValues = await _mongoHandler.SendMongoQueryActivityAsync(_db!, "Payment_Records",
                 "InstallmentPlanNumber",
                 planCreateResponse.InstallmentPlanNumber, "Activity");
             Assert.That(listActivityValues, Has.Count.EqualTo(1));
             Assert.That(_skuDict![testName.Substring(0, 40)].SetActivity, Does.Contain(listActivityValues[0]!));
             Console.WriteLine("Done TestValidate_MER_UNF_STL_BIW_FTR_DRC_U31");
         }
         catch (Exception exception)
         {
             Assert.Fail("Error in TestValidate_MER_UNF_STL_BIW_FTR_DRC_U31\n" + exception + "\n");
         }
     }
    
    
     [TestCase(Category = "SkuUnfBiwTests")]
     [Test(Description = "TestValidate_MER_UNF_STL_BIW_FTR_CRC_U31 with generated merchant"), CancelAfter(120 * 1000)]
     public async Task TestValidate_MER_UNF_STL_BIW_FTR_CRC_U31()
     {
         try
         {
             Console.WriteLine("\nStarting TestValidate_MER_UNF_STL_BIW_FTR_CRC_U31");
             var testName = TestContext.CurrentContext.Test.Name;
             var terminalApiKey = _envConfig.MER_UNF_STL_BIW_FTR;
             Console.WriteLine("Done with creation of a new merchant , settings and terminal process");
             var createPlanDefaultValues = new CreatePlanDefaultValues();
             var is3DsVarsCreation = new Is3DsVarsCreation();
             createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
             var planCreateResponse = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!, 
                 _requestHeader!,
                 "Active", 31, terminalApiKey, createPlanDefaultValues);
             var skuQueryResult = await _mongoHandler.SendMongoQueryForSkuAsync(_db!, "Payment_Records",
                 "InstallmentPlanNumber",
                 planCreateResponse.InstallmentPlanNumber, "Sku");
             Assert.That(_skuTestsData.ValidateSkuValue(skuQueryResult, testName.Substring(0, 40)), Is.True);
             var listActivityValues = await _mongoHandler.SendMongoQueryActivityAsync(_db!, "Payment_Records",
                 "InstallmentPlanNumber",
                 planCreateResponse.InstallmentPlanNumber, "Activity");
             Assert.That(listActivityValues, Has.Count.EqualTo(1));
             Assert.That(_skuDict![testName.Substring(0, 40)].SetActivity, Does.Contain(listActivityValues[0]!));
             Console.WriteLine("Done TestValidate_MER_UNF_STL_BIW_FTR_CRC_U31");
         }
         catch (Exception exception)
         {
             Assert.Fail("Error in TestValidate_MER_UNF_STL_BIW_FTR_CRC_U31 \n" + exception + "\n");
         }
     }
    
     [TestCase(Category = "SkuUnfBiwTests")]
     [Test(Description = "TestValidate_MER_UNF_STL_BIW_FTR_DRC_U32 with generated merchant"), CancelAfter(120 * 1000)]
     public async Task TestValidate_MER_UNF_STL_BIW_FTR_DRC_U32()
     {
         try
         {
             Console.WriteLine("\nStarting TestValidate_MER_UNF_STL_BIW_FTR_DRC_U32");
             var testName = TestContext.CurrentContext.Test.Name;
             var terminalApiKey = _envConfig.MER_UNF_STL_BIW_FTR;
             Console.WriteLine("Done with creation of a new merchant , settings and terminal process");
             var createPlanDefaultValues = new CreatePlanDefaultValues();
             var is3DsVarsCreation = new Is3DsVarsCreation();
             createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
             createPlanDefaultValues.paymentMethod.card.cardNumber = Environment.GetEnvironmentVariable("DebitCard")!;
             var planCreateResponse = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!, 
                 _requestHeader!,
                 "PendingCapture", 32, terminalApiKey, createPlanDefaultValues);
             var skuQueryResult = await _mongoHandler.SendMongoQueryForSkuAsync(_db!, "Payment_Records",
                 "InstallmentPlanNumber",
                 planCreateResponse.InstallmentPlanNumber, "Sku");
             Assert.That(_skuTestsData.ValidateSkuValue(skuQueryResult, testName.Substring(0, 40)), Is.True);
             var listActivityValues = await _mongoHandler.SendMongoQueryActivityAsync(_db!, "Payment_Records",
                 "InstallmentPlanNumber",
                 planCreateResponse.InstallmentPlanNumber, "Activity");
             Assert.That(listActivityValues, Has.Count.EqualTo(1));
             Assert.That(_skuDict![testName.Substring(0, 40)].SetActivity, Does.Contain(listActivityValues[0]!));
             Console.WriteLine("Done TestValidate_MER_UNF_STL_BIW_FTR_DRC_U32");
         }
         catch (Exception exception)
         {
             Assert.Fail("Error in TestValidate_MER_UNF_STL_BIW_FTR_DRC_U32\n" + exception + "\n");
         }
     }
    
    
     [TestCase(Category = "SkuUnfBiwTests")]
     [Test(Description = "TestValidate_MER_UNF_STL_BIW_FTR_CRC_U32 with generated merchant"), CancelAfter(120 * 1000)]
     public async Task TestValidate_MER_UNF_STL_BIW_FTR_CRC_U32()
     {
         try
         {
             Console.WriteLine("\nStarting TestValidate_MER_UNF_STL_BIW_FTR_CRC_U32");
             var testName = TestContext.CurrentContext.Test.Name;
             var terminalApiKey = _envConfig.MER_UNF_STL_BIW_FTR;
             Console.WriteLine("Done with creation of a new merchant , settings and terminal process");
             var createPlanDefaultValues = new CreatePlanDefaultValues();
             var is3DsVarsCreation = new Is3DsVarsCreation();
             createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
             var planCreateResponse = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!, 
                 _requestHeader!,
                 "Active", 32, terminalApiKey, createPlanDefaultValues);
             var skuQueryResult = await _mongoHandler.SendMongoQueryForSkuAsync(_db!, "Payment_Records",
                 "InstallmentPlanNumber",
                 planCreateResponse.InstallmentPlanNumber, "Sku");
             Assert.That(_skuTestsData.ValidateSkuValue(skuQueryResult, testName.Substring(0, 40)), Is.True);
             var listActivityValues = await _mongoHandler.SendMongoQueryActivityAsync(_db!, "Payment_Records",
                 "InstallmentPlanNumber",
                 planCreateResponse.InstallmentPlanNumber, "Activity");
             Assert.That(listActivityValues, Has.Count.EqualTo(1));
             Assert.That(_skuDict![testName.Substring(0, 40)].SetActivity, Does.Contain(listActivityValues[0]!));
             Console.WriteLine("Done TestValidate_MER_UNF_STL_BIW_FTR_CRC_U32");
         }
         catch (Exception exception)
         {
             Assert.Fail("Error in TestValidate_MER_UNF_STL_BIW_FTR_CRC_U32\n" + exception + "\n");
         }
     }
    
     [TestCase(Category = "SkuUnfBiwTests")]
     [Test(Description = "TestValidate_MER_UNF_STL_BIW_FTR_DRC_U33 with generated merchant"), CancelAfter(120 * 1000)]
     public async Task TestValidate_MER_UNF_STL_BIW_FTR_DRC_U33()
     {
         try
         {
             Console.WriteLine("\nStarting TestValidate_MER_UNF_STL_BIW_FTR_DRC_U33");
             var testName = TestContext.CurrentContext.Test.Name;
             var terminalApiKey = _envConfig.MER_UNF_STL_BIW_FTR;
             Console.WriteLine("Done with creation of a new merchant , settings and terminal process");
             var createPlanDefaultValues = new CreatePlanDefaultValues();
             var is3DsVarsCreation = new Is3DsVarsCreation();
             createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
             createPlanDefaultValues.paymentMethod.card.cardNumber = Environment.GetEnvironmentVariable("DebitCard")!;
             var planCreateResponse = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!, 
                 _requestHeader!,
                 "PendingCapture", 33, terminalApiKey, createPlanDefaultValues);
             var skuQueryResult = await _mongoHandler.SendMongoQueryForSkuAsync(_db!, "Payment_Records",
                 "InstallmentPlanNumber",
                 planCreateResponse.InstallmentPlanNumber, "Sku");
             Assert.That(_skuTestsData.ValidateSkuValue(skuQueryResult, testName.Substring(0, 40)), Is.True);
             var listActivityValues = await _mongoHandler.SendMongoQueryActivityAsync(_db!, "Payment_Records",
                 "InstallmentPlanNumber",
                 planCreateResponse.InstallmentPlanNumber, "Activity");
             Assert.That(listActivityValues, Has.Count.EqualTo(1));
             Assert.That(_skuDict![testName.Substring(0, 40)].SetActivity, Does.Contain(listActivityValues[0]!));
             Console.WriteLine("Done TestValidate_MER_UNF_STL_BIW_FTR_DRC_U33");
         }
         catch (Exception exception)
         {
             Assert.Fail("Error in TestValidate_MER_UNF_STL_BIW_FTR_DRC_U33\n" + exception + "\n");
         }
     }
    
    
     [TestCase(Category = "SkuUnfBiwTests")]
     [Test(Description = "TestValidate_MER_UNF_STL_BIW_FTR_CRC_U33 with generated merchant"), CancelAfter(120 * 1000)]
     public async Task TestValidate_MER_UNF_STL_BIW_FTR_CRC_U33()
     {
         try
         {
             Console.WriteLine("\nStarting TestValidate_MER_UNF_STL_BIW_FTR_CRC_U33");
             var testName = TestContext.CurrentContext.Test.Name;
             var terminalApiKey = _envConfig.MER_UNF_STL_BIW_FTR;
             Console.WriteLine("Done with creation of a new merchant , settings and terminal process");
             var createPlanDefaultValues = new CreatePlanDefaultValues();
             var is3DsVarsCreation = new Is3DsVarsCreation();
             createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
             var planCreateResponse = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!, 
                 _requestHeader!,
                 "Active", 33, terminalApiKey, createPlanDefaultValues);
             var skuQueryResult = await _mongoHandler.SendMongoQueryForSkuAsync(_db!, "Payment_Records",
                 "InstallmentPlanNumber",
                 planCreateResponse.InstallmentPlanNumber, "Sku");
             Assert.That(_skuTestsData.ValidateSkuValue(skuQueryResult, testName.Substring(0, 40)), Is.True);
             var listActivityValues = await _mongoHandler.SendMongoQueryActivityAsync(_db!, "Payment_Records",
                 "InstallmentPlanNumber",
                 planCreateResponse.InstallmentPlanNumber, "Activity");
             Assert.That(listActivityValues, Has.Count.EqualTo(1));
             Assert.That(_skuDict![testName.Substring(0, 40)].SetActivity, Does.Contain(listActivityValues[0]!));
             Console.WriteLine("Done TestValidate_MER_UNF_STL_BIW_FTR_CRC_U33");
         }
         catch (Exception exception)
         {
             Assert.Fail("Error in TestValidate_MER_UNF_STL_BIW_FTR_CRC_U33\n" + exception + "\n");
         }
     }
     [TestCase(Category = "SkuUnfBiwTests")]
     [Test(Description = "TestValidate_MER_UNF_STL_BIW_FTR_DRC_U34 with generated merchant"), CancelAfter(120 * 1000)]
     public async Task TestValidate_MER_UNF_STL_BIW_FTR_DRC_U34()
     {
         try
         {
             Console.WriteLine("\nStarting TestValidate_MER_UNF_STL_BIW_FTR_DRC_U34");
             var testName = TestContext.CurrentContext.Test.Name;
             var terminalApiKey = _envConfig.MER_UNF_STL_BIW_FTR;
             Console.WriteLine("Done with creation of a new merchant , settings and terminal process");
             var createPlanDefaultValues = new CreatePlanDefaultValues();
             var is3DsVarsCreation = new Is3DsVarsCreation();
             createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
             createPlanDefaultValues.paymentMethod.card.cardNumber = Environment.GetEnvironmentVariable("DebitCard")!;
             var planCreateResponse = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!, 
                 _requestHeader!,
                 "PendingCapture", 34, terminalApiKey, createPlanDefaultValues);
             var skuQueryResult = await _mongoHandler.SendMongoQueryForSkuAsync(_db!, "Payment_Records",
                 "InstallmentPlanNumber",
                 planCreateResponse.InstallmentPlanNumber, "Sku");
             Assert.That(_skuTestsData.ValidateSkuValue(skuQueryResult, testName.Substring(0, 40)), Is.True);
             var listActivityValues = await _mongoHandler.SendMongoQueryActivityAsync(_db!, "Payment_Records",
                 "InstallmentPlanNumber",
                 planCreateResponse.InstallmentPlanNumber, "Activity");
             Assert.That(listActivityValues, Has.Count.EqualTo(1));
             Assert.That(_skuDict![testName.Substring(0, 40)].SetActivity, Does.Contain(listActivityValues[0]!));
             Console.WriteLine("Done TestValidate_MER_UNF_STL_BIW_FTR_DRC_U34");
         }
         catch (Exception exception)
         {
             Assert.Fail("Error in TestValidate_MER_UNF_STL_BIW_FTR_DRC_U34\n" + exception + "\n");
         }
     }
    
    
     [TestCase(Category = "SkuUnfBiwTests")]
     [Test(Description = "TestValidate_MER_UNF_STL_BIW_FTR_CRC_U34 with generated merchant"), CancelAfter(120 * 1000)]
     public async Task TestValidate_MER_UNF_STL_BIW_FTR_CRC_U34()
     {
         try
         {
             Console.WriteLine("\nStarting TestValidate_MER_UNF_STL_BIW_FTR_CRC_U34");
             var testName = TestContext.CurrentContext.Test.Name;
             var terminalApiKey = _envConfig.MER_UNF_STL_BIW_FTR;
             Console.WriteLine("Done with creation of a new merchant , settings and terminal process");
             var createPlanDefaultValues = new CreatePlanDefaultValues();
             var is3DsVarsCreation = new Is3DsVarsCreation();
             createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
             var planCreateResponse = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!, 
                 _requestHeader!, "Active", 34, terminalApiKey, createPlanDefaultValues);
             var skuQueryResult = await _mongoHandler.SendMongoQueryForSkuAsync(_db!, "Payment_Records",
                 "InstallmentPlanNumber",
                 planCreateResponse.InstallmentPlanNumber, "Sku");
             Assert.That(_skuTestsData.ValidateSkuValue(skuQueryResult, testName.Substring(0, 40)), Is.True);
             var listActivityValues = await _mongoHandler.SendMongoQueryActivityAsync(_db!, "Payment_Records",
                 "InstallmentPlanNumber",
                 planCreateResponse.InstallmentPlanNumber, "Activity");
             Assert.That(listActivityValues, Has.Count.EqualTo(1));
             Assert.That(_skuDict![testName.Substring(0, 40)].SetActivity, Does.Contain(listActivityValues[0]!));
             Console.WriteLine("Done TestValidate_MER_UNF_STL_BIW_FTR_CRC_U34");
         }
         catch (Exception exception)
         {
             Assert.Fail("Error in TestValidate_MER_UNF_STL_BIW_FTR_CRC_U34 \n" + exception + "\n");
         }
     }
    
     [TestCase(Category = "SkuUnfBiwTests")]
     [Test(Description = "TestValidate_MER_UNF_STL_BIW_FTR_DRC_U35 with generated merchant"), CancelAfter(120 * 1000)]
     public async Task TestValidate_MER_UNF_STL_BIW_FTR_DRC_U35()
     {
         try
         {
             Console.WriteLine("\nStarting TestValidate_MER_UNF_STL_BIW_FTR_DRC_U35");
             var testName = TestContext.CurrentContext.Test.Name;
             var terminalApiKey = _envConfig.MER_UNF_STL_BIW_FTR;
             Console.WriteLine("Done with creation of a new merchant , settings and terminal process");
             var createPlanDefaultValues = new CreatePlanDefaultValues();
             var is3DsVarsCreation = new Is3DsVarsCreation();
             createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
             createPlanDefaultValues.paymentMethod.card.cardNumber = Environment.GetEnvironmentVariable("DebitCard")!;
             var planCreateResponse = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!, 
                 _requestHeader!,
                 "PendingCapture", 35, terminalApiKey, createPlanDefaultValues);
             var skuQueryResult = await _mongoHandler.SendMongoQueryForSkuAsync(_db!, "Payment_Records",
                 "InstallmentPlanNumber",
                 planCreateResponse.InstallmentPlanNumber, "Sku");
             Assert.That(_skuTestsData.ValidateSkuValue(skuQueryResult, testName.Substring(0, 40)), Is.True);
             var listActivityValues = await _mongoHandler.SendMongoQueryActivityAsync(_db!, "Payment_Records",
                 "InstallmentPlanNumber",
                 planCreateResponse.InstallmentPlanNumber, "Activity");
             Assert.That(listActivityValues, Has.Count.EqualTo(1));
             Assert.That(_skuDict![testName.Substring(0, 40)].SetActivity, Does.Contain(listActivityValues[0]!));
             Console.WriteLine("Done TestValidate_MER_UNF_STL_BIW_FTR_DRC_U35");
         }
         catch (Exception exception)
         {
             Assert.Fail("Error in TestValidate_MER_UNF_STL_BIW_FTR_DRC_U35\n" + exception + "\n");
         }
     }
    
    
     [TestCase(Category = "SkuUnfBiwTests")]
     [Test(Description = "TestValidate_MER_UNF_STL_BIW_FTR_CRC_U35 with generated merchant"), CancelAfter(120 * 1000)]
     public async Task TestValidate_MER_UNF_STL_BIW_FTR_CRC_U35()
     {
         try
         {
             Console.WriteLine("\nStarting TestValidate_MER_UNF_STL_BIW_FTR_CRC_U35");
             var testName = TestContext.CurrentContext.Test.Name;
             var terminalApiKey = _envConfig.MER_UNF_STL_BIW_FTR;
             Console.WriteLine("Done with creation of a new merchant , settings and terminal process");
             var createPlanDefaultValues = new CreatePlanDefaultValues();
             var is3DsVarsCreation = new Is3DsVarsCreation();
             createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
             var planCreateResponse = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!, 
                 _requestHeader!,
                 "Active", 35, terminalApiKey, createPlanDefaultValues);
             var skuQueryResult = await _mongoHandler.SendMongoQueryForSkuAsync(_db!, "Payment_Records",
                 "InstallmentPlanNumber",
                 planCreateResponse.InstallmentPlanNumber, "Sku");
             Assert.That(_skuTestsData.ValidateSkuValue(skuQueryResult, testName.Substring(0, 40)), Is.True);
             var listActivityValues = await _mongoHandler.SendMongoQueryActivityAsync(_db!, "Payment_Records",
                 "InstallmentPlanNumber",
                 planCreateResponse.InstallmentPlanNumber, "Activity");
             Assert.That(listActivityValues, Has.Count.EqualTo(1));
             Assert.That(_skuDict![testName.Substring(0, 40)].SetActivity, Does.Contain(listActivityValues[0]!));
             Console.WriteLine("Done TestValidate_MER_UNF_STL_BIW_FTR_CRC_U35");
         }
         catch (Exception exception)
         {
             Assert.Fail("Error in TestValidate_MER_UNF_STL_BIW_FTR_CRC_U35\n" + exception + "\n");
         }
     }

    [TestCase(Category = "SkuUnfBiwTests")]
    [Test(Description = "TestValidate_MER_UNF_STL_BIW_FTR_DRC_U12 with generated merchant"), CancelAfter(120 * 1000)]
    public async Task TestValidate_MER_UNF_STL_BIW_FTR_DRC_U12()
    {
        try
        {
            Console.WriteLine("\nStarting TestValidate_MER_UNF_STL_BIW_FTR_DRC_U12");
            var testName = TestContext.CurrentContext.Test.Name;
            var terminalApiKey = _envConfig.MER_UNF_STL_BIW_FTR;
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            createPlanDefaultValues.paymentMethod.card.cardNumber = Environment.GetEnvironmentVariable("DebitCard")!;
            var planCreateResponse = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!,
                _requestHeader!,
                "PendingCapture", 12, terminalApiKey, createPlanDefaultValues);
            var skuQueryResult = await _mongoHandler.SendMongoQueryForSkuAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber",
                planCreateResponse.InstallmentPlanNumber, "Sku");
            Assert.That(_skuTestsData.ValidateSkuValue(skuQueryResult, testName.Substring(0, 40)), Is.True);
            var listActivityValues = await _mongoHandler.SendMongoQueryActivityAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber",
                planCreateResponse.InstallmentPlanNumber, "Activity");
            Assert.That(listActivityValues.Count >= 1);
            Assert.That(_skuDict![testName.Substring(0, 40)].SetActivity, Does.Contain(listActivityValues[0]!));
            Console.WriteLine("Done TestValidate_MER_UNF_STL_BIW_FTR_DRC_U12");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidate_MER_UNF_STL_BIW_FTR_DRC_U12 \n" + exception + "\n");
        }
    }


    [TestCase(Category = "SkuUnfBiwTests")]
    [Test(Description = "TestValidate_MER_UNF_STL_BIW_FTR_CRC_U12 with generated merchant"), CancelAfter(120 * 1000)]
    public async Task TestValidate_MER_UNF_STL_BIW_FTR_CRC_U12()
    {
        try
        {
            Console.WriteLine("\nStarting TestValidate_MER_UNF_STL_BIW_FTR_CRC_U12");
            var testName = TestContext.CurrentContext.Test.Name;
            var terminalApiKey = _envConfig.MER_UNF_STL_BIW_FTR;
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            var planCreateResponse = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!,
                _requestHeader!,
                "Active", 12, terminalApiKey, createPlanDefaultValues);
            var skuQueryResult = await _mongoHandler.SendMongoQueryForSkuAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber",
                planCreateResponse.InstallmentPlanNumber, "Sku");
            Assert.That(_skuTestsData.ValidateSkuValue(skuQueryResult, testName.Substring(0, 40)), Is.True);
            var listActivityValues = await _mongoHandler.SendMongoQueryActivityAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber",
                planCreateResponse.InstallmentPlanNumber, "Activity");
            Assert.That(listActivityValues.Count >= 1);
            Assert.That(_skuDict![testName.Substring(0, 40)].SetActivity, Does.Contain(listActivityValues[0]!));
            Console.WriteLine("Done TestValidate_MER_UNF_STL_BIW_FTR_CRC_U12");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidate_MER_UNF_STL_BIW_FTR_CRC_U12 \n" + exception + "\n");
        }
    }
    
    [TestCase(Category = "SkuUnfBiwTests")]
    [Test(Description = "TestValidate_MER_UNF_STL_BIW_FTR_DRC_U15 with generated merchant"), CancelAfter(120 * 1000)]
    public async Task TestValidate_MER_UNF_STL_BIW_FTR_DRC_U15()
    {
        try
        {
            Console.WriteLine("\nStarting TestValidate_MER_UNF_STL_BIW_FTR_DRC_U15");
            var testName = TestContext.CurrentContext.Test.Name;
            var terminalApiKey = _envConfig.MER_UNF_STL_BIW_FTR;
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            createPlanDefaultValues.paymentMethod.card.cardNumber = Environment.GetEnvironmentVariable("DebitCard")!;
            var planCreateResponse = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!,
                _requestHeader!,
                "PendingCapture", 15, terminalApiKey, createPlanDefaultValues);
            var skuQueryResult = await _mongoHandler.SendMongoQueryForSkuAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber",
                planCreateResponse.InstallmentPlanNumber, "Sku");
            Assert.That(_skuTestsData.ValidateSkuValue(skuQueryResult, testName.Substring(0, 40)), Is.True);
            var listActivityValues = await _mongoHandler.SendMongoQueryActivityAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber",
                planCreateResponse.InstallmentPlanNumber, "Activity");
            Assert.That(listActivityValues.Count >= 1);
            Assert.That(_skuDict![testName.Substring(0, 40)].SetActivity, Does.Contain(listActivityValues[0]!));
            Console.WriteLine("Done TestValidate_MER_UNF_STL_BIW_FTR_DRC_U15");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidate_MER_UNF_STL_BIW_FTR_DRC_U15 \n" + exception + "\n");
        }
    }


    [TestCase(Category = "SkuUnfBiwTests")]
    [Test(Description = "TestValidate_MER_UNF_STL_BIW_FTR_CRC_U15 with generated merchant"), CancelAfter(120 * 1000)]
    public async Task TestValidate_MER_UNF_STL_BIW_FTR_CRC_U15()
    {
        try
        {
            Console.WriteLine("\nStarting TestValidate_MER_UNF_STL_BIW_FTR_CRC_U15");
            var testName = TestContext.CurrentContext.Test.Name;
            var terminalApiKey = _envConfig.MER_UNF_STL_BIW_FTR;
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            var planCreateResponse = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!,
                _requestHeader!,
                "Active", 15, terminalApiKey, createPlanDefaultValues);
            var skuQueryResult = await _mongoHandler.SendMongoQueryForSkuAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber",
                planCreateResponse.InstallmentPlanNumber, "Sku");
            Assert.That(_skuTestsData.ValidateSkuValue(skuQueryResult, testName.Substring(0, 40)), Is.True);
            var listActivityValues = await _mongoHandler.SendMongoQueryActivityAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber",
                planCreateResponse.InstallmentPlanNumber, "Activity");
            Assert.That(listActivityValues.Count >= 1);
            Assert.That(_skuDict![testName.Substring(0, 40)].SetActivity, Does.Contain(listActivityValues[0]!));
            Console.WriteLine("Done TestValidate_MER_UNF_STL_BIW_FTR_CRC_U15");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidate_MER_UNF_STL_BIW_FTR_CRC_U15 \n" + exception + "\n");
        }
    }

    [TestCase(Category = "SkuUnfBiwTests")]
    [Test(Description = "TestValidate_MER_UNF_STL_BIW_FTR_DRC_U18 with generated merchant"), CancelAfter(120 * 1000)]
    public async Task TestValidate_MER_UNF_STL_BIW_FTR_DRC_U18()
    {
        try
        {
            Console.WriteLine("\nStarting TestValidate_MER_UNF_STL_BIW_FTR_DRC_U18");
            var testName = TestContext.CurrentContext.Test.Name;
            var terminalApiKey = _envConfig.MER_UNF_STL_BIW_FTR;
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            createPlanDefaultValues.paymentMethod.card.cardNumber = Environment.GetEnvironmentVariable("DebitCard")!;
            var planCreateResponse = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!,
                _requestHeader!,
                "PendingCapture", 18, terminalApiKey, createPlanDefaultValues);
            var skuQueryResult = await _mongoHandler.SendMongoQueryForSkuAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber",
                planCreateResponse.InstallmentPlanNumber, "Sku");
            Assert.That(_skuTestsData.ValidateSkuValue(skuQueryResult, testName.Substring(0, 40)), Is.True);
            var listActivityValues = await _mongoHandler.SendMongoQueryActivityAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber",
                planCreateResponse.InstallmentPlanNumber, "Activity");
            Assert.That(listActivityValues.Count >= 1);
            Assert.That(_skuDict![testName.Substring(0, 40)].SetActivity, Does.Contain(listActivityValues[0]!));
            Console.WriteLine("Done TestValidate_MER_UNF_STL_BIW_FTR_DRC_U18");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidate_MER_UNF_STL_BIW_FTR_DRC_U18 \n" + exception + "\n");
        }
    }


    [TestCase(Category = "SkuUnfBiwTests")]
    [Test(Description = "TestValidate_MER_UNF_STL_BIW_FTR_CRC_U18 with generated merchant"), CancelAfter(120 * 1000)]
    public async Task TestValidate_MER_UNF_STL_BIW_FTR_CRC_U18()
    {
        try
        {
            Console.WriteLine("\nStarting TestValidate_MER_UNF_STL_BIW_FTR_CRC_U18");
            var testName = TestContext.CurrentContext.Test.Name;
            var terminalApiKey = _envConfig.MER_UNF_STL_BIW_FTR;
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            var planCreateResponse = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!,
                _requestHeader!,
                "Active", 18, terminalApiKey, createPlanDefaultValues);
            var skuQueryResult = await _mongoHandler.SendMongoQueryForSkuAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber",
                planCreateResponse.InstallmentPlanNumber, "Sku");
            Assert.That(_skuTestsData.ValidateSkuValue(skuQueryResult, testName.Substring(0, 40)), Is.True);
            var listActivityValues = await _mongoHandler.SendMongoQueryActivityAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber",
                planCreateResponse.InstallmentPlanNumber, "Activity");
            Assert.That(listActivityValues.Count >= 1);
            Assert.That(_skuDict![testName.Substring(0, 40)].SetActivity, Does.Contain(listActivityValues[0]!));
            Console.WriteLine("Done TestValidate_MER_UNF_STL_BIW_FTR_CRC_U18");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidate_MER_UNF_STL_BIW_FTR_CRC_U18 \n" + exception + "\n");
        }
    }

    [TestCase(Category = "SkuUnfBiwTests")]
    [Test(Description = "TestValidate_MER_UNF_STL_BIW_FTR_DRC_U19 with generated merchant"), CancelAfter(120 * 1000)]
    public async Task TestValidate_MER_UNF_STL_BIW_FTR_DRC_U19()
    {
        try
        {
            Console.WriteLine("\nStarting TestValidate_MER_UNF_STL_BIW_FTR_DRC_U16");
            var testName = TestContext.CurrentContext.Test.Name;
            var terminalApiKey = _envConfig.MER_UNF_STL_BIW_FTR;
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            createPlanDefaultValues.paymentMethod.card.cardNumber = Environment.GetEnvironmentVariable("DebitCard")!;
            var planCreateResponse = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!,
                _requestHeader!,
                "PendingCapture", 19, terminalApiKey, createPlanDefaultValues);
            var skuQueryResult = await _mongoHandler.SendMongoQueryForSkuAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber",
                planCreateResponse.InstallmentPlanNumber, "Sku");
            Assert.That(_skuTestsData.ValidateSkuValue(skuQueryResult, testName.Substring(0, 40)), Is.True);
            var listActivityValues = await _mongoHandler.SendMongoQueryActivityAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber",
                planCreateResponse.InstallmentPlanNumber, "Activity");
            Assert.That(listActivityValues.Count >= 1);
            Assert.That(_skuDict![testName.Substring(0, 40)].SetActivity, Does.Contain(listActivityValues[0]!));
            Console.WriteLine("Done TestValidate_MER_UNF_STL_BIW_FTR_DRC_U19");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidate_MER_UNF_STL_BIW_FTR_DRC_U19\n" + exception + "\n");
        }
    }


    [TestCase(Category = "SkuUnfBiwTests")]
    [Test(Description = "TestValidate_MER_UNF_STL_BIW_FTR_CRC_U19 with generated merchant"), CancelAfter(120 * 1000)]
    public async Task TestValidate_MER_UNF_STL_BIW_FTR_CRC_U19()
    {
        try
        {
            Console.WriteLine("\nStarting TestValidate_MER_UNF_STL_BIW_FTR_CRC_U19");
            var testName = TestContext.CurrentContext.Test.Name;
            var terminalApiKey = _envConfig.MER_UNF_STL_BIW_FTR_CRC_U19;
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            var planCreateResponse = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!,
                _requestHeader!,
                "Active", 19, terminalApiKey, createPlanDefaultValues);
            var skuQueryResult = await _mongoHandler.SendMongoQueryForSkuAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber",
                planCreateResponse.InstallmentPlanNumber, "Sku");
            Assert.That(_skuTestsData.ValidateSkuValue(skuQueryResult, testName.Substring(0, 40)), Is.True);
            var listActivityValues = await _mongoHandler.SendMongoQueryActivityAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber",
                planCreateResponse.InstallmentPlanNumber, "Activity");
            Assert.That(listActivityValues.Count >= 1);
            Assert.That(_skuDict![testName.Substring(0, 40)].SetActivity, Does.Contain(listActivityValues[0]!));
            Console.WriteLine("Done TestValidate_MER_UNF_STL_BIW_FTR_CRC_U19");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidate_MER_UNF_STL_BIW_FTR_CRC_U19 \n" + exception + "\n");
        }
    }

    [TestCase(Category = "SkuUnfBiwTests")]
    [Test(Description = "TestValidate_MER_UNF_STL_BIW_FTR_DRC_U21 with generated merchant"), CancelAfter(120 * 1000)]
    public async Task TestValidate_MER_UNF_STL_BIW_FTR_DRC_U21()
    {
        try
        {
            Console.WriteLine("\nStarting TestValidate_MER_UNF_STL_BIW_FTR_DRC_U21");
            var testName = TestContext.CurrentContext.Test.Name;
            var terminalApiKey = _envConfig.MER_UNF_STL_BIW_FTR;
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            createPlanDefaultValues.paymentMethod.card.cardNumber = Environment.GetEnvironmentVariable("DebitCard")!;
            var planCreateResponse = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!,
                _requestHeader!,
                "PendingCapture", 21, terminalApiKey, createPlanDefaultValues);
            var skuQueryResult = await _mongoHandler.SendMongoQueryForSkuAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber",
                planCreateResponse.InstallmentPlanNumber, "Sku");
            Assert.That(_skuTestsData.ValidateSkuValue(skuQueryResult, testName.Substring(0, 40)), Is.True);
            var listActivityValues = await _mongoHandler.SendMongoQueryActivityAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber",
                planCreateResponse.InstallmentPlanNumber, "Activity");
            Assert.That(listActivityValues.Count >= 1);
            Assert.That(_skuDict![testName.Substring(0, 40)].SetActivity, Does.Contain(listActivityValues[0]!));
            Console.WriteLine("Done TestValidate_MER_UNF_STL_BIW_FTR_DRC_U21");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidate_MER_UNF_STL_BIW_FTR_DRC_U21 \n" + exception + "\n");
        }
    }


    [TestCase(Category = "SkuUnfBiwTests")]
    [Test(Description = "TestValidate_MER_UNF_STL_BIW_FTR_CRC_U21 with generated merchant"), CancelAfter(120 * 1000)]
    public async Task TestValidate_MER_UNF_STL_BIW_FTR_CRC_U21()
    {
        try
        {
            Console.WriteLine("\nStarting TestValidate_MER_UNF_STL_BIW_FTR_CRC_U21");
            var testName = TestContext.CurrentContext.Test.Name;
            var terminalApiKey = _envConfig.MER_UNF_STL_BIW_FTR_CRC_U21;
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            var planCreateResponse = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!,
                _requestHeader!,
                "Active", 21, terminalApiKey, createPlanDefaultValues);
            var skuQueryResult = await _mongoHandler.SendMongoQueryForSkuAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber",
                planCreateResponse.InstallmentPlanNumber, "Sku");
            Assert.That(_skuTestsData.ValidateSkuValue(skuQueryResult, testName.Substring(0, 40)), Is.True);
            var listActivityValues = await _mongoHandler.SendMongoQueryActivityAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber",
                planCreateResponse.InstallmentPlanNumber, "Activity");
            Assert.That(listActivityValues.Count >= 1);
            Assert.That(_skuDict![testName.Substring(0, 40)].SetActivity, Does.Contain(listActivityValues[0]!));
            Console.WriteLine("Done TestValidate_MER_UNF_STL_BIW_FTR_CRC_U21");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidate_MER_UNF_STL_BIW_FTR_CRC_U21 \n" + exception + "\n");
        }
    }

    [TestCase(Category = "SkuUnfBiwTests")]
    [Test(Description = "TestValidate_MER_UNF_STL_BIW_FTR_DRC_U24 with generated merchant"), CancelAfter(120 * 1000)]
    public async Task TestValidate_MER_UNF_STL_BIW_FTR_DRC_U24()
    {
        try
        {
            Console.WriteLine("\nStarting TestValidate_MER_UNF_STL_BIW_FTR_DRC_U24");
            var testName = TestContext.CurrentContext.Test.Name;
            var terminalApiKey = _envConfig.MER_UNF_STL_BIW_FTR;
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            createPlanDefaultValues.paymentMethod.card.cardNumber = Environment.GetEnvironmentVariable("DebitCard")!;
            var planCreateResponse = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!,
                _requestHeader!,
                "PendingCapture", 24, terminalApiKey, createPlanDefaultValues);
            var skuQueryResult = await _mongoHandler.SendMongoQueryForSkuAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber",
                planCreateResponse.InstallmentPlanNumber, "Sku");
            Assert.That(_skuTestsData.ValidateSkuValue(skuQueryResult, testName.Substring(0, 40)), Is.True);
            var listActivityValues = await _mongoHandler.SendMongoQueryActivityAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber",
                planCreateResponse.InstallmentPlanNumber, "Activity");
            Assert.That(listActivityValues.Count >= 1);
            Assert.That(_skuDict![testName.Substring(0, 40)].SetActivity, Does.Contain(listActivityValues[0]!));
            Console.WriteLine("Done TestValidate_MER_UNF_STL_BIW_FTR_DRC_U24");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidate_MER_UNF_STL_BIW_FTR_DRC_U24 \n" + exception + "\n");
        }
    }


    [TestCase(Category = "SkuUnfBiwTests")]
    [Test(Description = "TestValidate_MER_UNF_STL_BIW_FTR_CRC_U24 with generated merchant"), CancelAfter(120 * 1000)]
    public async Task TestValidate_MER_UNF_STL_BIW_FTR_CRC_U24()
    {
        try
        {
            Console.WriteLine("\nStarting TestValidate_MER_UNF_STL_BIW_FTR_CRC_U24");
            var testName = TestContext.CurrentContext.Test.Name;
            var terminalApiKey = _envConfig.MER_UNF_STL_BIW_FTR;
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            var planCreateResponse = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!,
                _requestHeader!,
                "Active", 24, terminalApiKey, createPlanDefaultValues);
            var skuQueryResult = await _mongoHandler.SendMongoQueryForSkuAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber",
                planCreateResponse.InstallmentPlanNumber, "Sku");
            Assert.That(_skuTestsData.ValidateSkuValue(skuQueryResult, testName.Substring(0, 40)), Is.True);
            var listActivityValues = await _mongoHandler.SendMongoQueryActivityAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber",
                planCreateResponse.InstallmentPlanNumber, "Activity");
            Assert.That(listActivityValues.Count >= 1);
            Assert.That(_skuDict![testName.Substring(0, 40)].SetActivity, Does.Contain(listActivityValues[0]!));
            Console.WriteLine("Done TestValidate_MER_UNF_STL_BIW_FTR_CRC_U24");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidate_MER_UNF_STL_BIW_FTR_CRC_U24 \n" + exception + "\n");
        }
    }

    [TestCase(Category = "SkuUnfBiwTests")]
    [Test(Description = "TestValidate_MER_UNF_STL_BIW_FTR_DRC_U36 with generated merchant"), CancelAfter(120 * 1000)]
    public async Task TestValidate_MER_UNF_STL_BIW_FTR_DRC_U36()
    {
        try
        {
            Console.WriteLine("\nStarting TestValidate_MER_UNF_STL_BIW_FTR_DRC_U36");
            var testName = TestContext.CurrentContext.Test.Name;
            var terminalApiKey = _envConfig.MER_UNF_STL_BIW_FTR;
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            createPlanDefaultValues.paymentMethod.card.cardNumber = Environment.GetEnvironmentVariable("DebitCard")!;
            var planCreateResponse = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!,
                _requestHeader!,
                "PendingCapture", 36, terminalApiKey, createPlanDefaultValues);
            var skuQueryResult = await _mongoHandler.SendMongoQueryForSkuAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber",
                planCreateResponse.InstallmentPlanNumber, "Sku");
            Assert.That(_skuTestsData.ValidateSkuValue(skuQueryResult, testName.Substring(0, 40)), Is.True);
            var listActivityValues = await _mongoHandler.SendMongoQueryActivityAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber",
                planCreateResponse.InstallmentPlanNumber, "Activity");
            Assert.That(listActivityValues.Count >= 1);
            Assert.That(_skuDict![testName.Substring(0, 40)].SetActivity, Does.Contain(listActivityValues[0]!));
            Console.WriteLine("Done TestValidate_MER_UNF_STL_BIW_FTR_DRC_U36");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidate_MER_UNF_STL_BIW_FTR_DRC_U36 \n" + exception + "\n");
        }
    }


    [TestCase(Category = "SkuUnfBiwTests")]
    [Test(Description = "TestValidate_MER_UNF_STL_BIW_FTR_CRC_U36 with generated merchant"), CancelAfter(120 * 1000)]
    public async Task TestValidate_MER_UNF_STL_BIW_FTR_CRC_U36()
    {
        try
        {
            Console.WriteLine("\nStarting TestValidate_MER_UNF_STL_BIW_FTR_CRC_U36");
            var testName = TestContext.CurrentContext.Test.Name;
            var terminalApiKey = _envConfig.MER_UNF_STL_BIW_FTR_CRC_U36;
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            var planCreateResponse = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!,
                _requestHeader!, "Active", 36, terminalApiKey, createPlanDefaultValues);
            Assert.That(_db, Is.Not.Null);
            var skuQueryResult = await _mongoHandler.SendMongoQueryForSkuAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", planCreateResponse.InstallmentPlanNumber, "Sku");
            Assert.That(_skuTestsData.ValidateSkuValue(skuQueryResult, testName.Substring(0, 40)), Is.True);
            var listActivityValues = await _mongoHandler.SendMongoQueryActivityAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", planCreateResponse.InstallmentPlanNumber, "Activity");
            Assert.That(listActivityValues.Count >= 1);
            Assert.That(_skuDict![testName.Substring(0, 40)].SetActivity, Does.Contain(listActivityValues[0]!));
            Console.WriteLine("Done TestValidate_MER_UNF_STL_BIW_FTR_CRC_U36");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidate_MER_UNF_STL_BIW_FTR_CRC_U36 \n" + exception + "\n");
        }
    }
}