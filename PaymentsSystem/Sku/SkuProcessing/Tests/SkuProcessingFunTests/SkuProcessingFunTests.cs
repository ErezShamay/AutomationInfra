using MongoDB.Driver;
using Allure.NUnit;
using Allure.NUnit.Attributes;
using NUnit.Framework;
using Splitit.Automation.NG.Backend.BaseActions;
using Splitit.Automation.NG.Backend.Services.V3.DefaultValues;
using Splitit.Automation.NG.Backend.Services.V3.InstallmentPlan.InstallmentPlanApiEndPoint;
using Splitit.Automation.NG.Backend.Tests.TestsSetups;
using Splitit.Automation.NG.PaymentsSystem.PaymentProcessing.EndPointFunctionality;
using Splitit.Automation.NG.Utilities.EnvironmentsConfig;

namespace Splitit.Automation.NG.PaymentsSystem.Sku.SkuProcessing.Tests.SkuProcessingFunTests;

[TestFixture]
[AllureNUnit]
[AllureSuite("SkuFunProcessingTests")]
[AllureDisplayIgnored]
public class SkuProcessingFunTests
{
    private RequestHeader? _requestHeader;
    private readonly InstallmentPlans _installmentPlans;
    private readonly MongoHandler _mongoHandler;
    private readonly Processing _processing;
    private IMongoDatabase? _db;
    private readonly EnvConfig _envConfig;
    private readonly InstallmentPlanNumberRefund _installmentPlanNumberRefund;
    

    public SkuProcessingFunTests()
    {
        Console.WriteLine("Staring SKU processing Setup");
        _installmentPlans = new InstallmentPlans();
        _mongoHandler = new MongoHandler();
        _processing = new Processing();
        _envConfig = new EnvConfig();
        _installmentPlanNumberRefund = new InstallmentPlanNumberRefund();
        Console.WriteLine("SKU Setup Succeeded\nֿ");
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
    
    [TestCase(Category = "SkuFunProcessingTests")]
    [Test(Description = "TestValidate_MER_FUN_STL_MON_FTR_CRC_F09_Processing with generated merchant"), CancelAfter(480*1000)]
    public async Task TestValidate_MER_FUN_STL_MON_FTR_CRC_F09_Processing()
    {
        try
        {
            Console.WriteLine("\nStarting TestValidate_MER_FUN_STL_MON_FTR_CRC_F09_Processing");
            var terminalApiKey = _envConfig.MER_FUN_STL_MON_FTR_CRC_F09_Processing;
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            createPlanDefaultValues.planData.totalAmount = AmountGenerator.GenerateAmountWithMinMaxValues(3000, 4000);
            var planCreateResponse = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!,  _requestHeader!,"PendingCapture", 
                9, terminalApiKey, createPlanDefaultValues);
            var response = await _processing.SendGetForProcessingAsync( _requestHeader!, planCreateResponse.InstallmentPlanNumber);
            Assert.That(response, Is.EqualTo(""));
            var (activity, id) = await _mongoHandler.SendMongoQueryForStatusAndIdAsync(_db!, "Payment_Records", "InstallmentPlanNumber",
                planCreateResponse.InstallmentPlanNumber, "Status", "_id");
            Assert.That(activity, Is.EqualTo("ProcessingFailed"));
            Assert.That(id, Is.Not.Null);
            Console.WriteLine("Done TestValidate_MER_FUN_STL_MON_FTR_CRC_F09_Processing");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidate_MER_FUN_STL_MON_FTR_CRC_F09_Processing \n" + exception + "\n");
        }
    }
    
    [TestCase(Category = "SkuFunProcessingTests")]
    [Test(Description = "TestValidate_DBS_FUN_STL_MON_FTR_CRC_F09_Processing with generated merchant"), CancelAfter(480*1000)]
    public async Task TestValidate_DBS_FUN_STL_MON_FTR_CRC_F09_Processing()
    {
        try
        {
            Console.WriteLine("\nStarting TestValidate_DBS_FUN_STL_MON_FTR_CRC_F09_Processing");
            var terminalApiKey = _envConfig.DBS_FUN_STL_MON_FTR_CRC_F09;
            Console.WriteLine("Done with creation of a new merchant , settings and terminal process");
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            var planCreateResponse = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!,  _requestHeader!,"PendingCapture", 
                9, terminalApiKey, createPlanDefaultValues);
            await _installmentPlanNumberRefund.SendRefundRequestAsync(_requestHeader!, planCreateResponse.InstallmentPlanNumber,
                "PartialRefund", "ReduceFromLastInstallment");
            var responseProcessing = await _processing.SendGetForProcessingAsync(_requestHeader!, planCreateResponse.InstallmentPlanNumber);
            Assert.That(responseProcessing, Is.EqualTo(""));
            var docsListCol = await _mongoHandler.SendMongoQueryWith2KeysAndReturnListOfResultAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", planCreateResponse.InstallmentPlanNumber, 
                "Activity", "COL", "Sku", "_id");
            Assert.That(docsListCol!.Count == 2);
            var docsListFnd = await _mongoHandler.SendMongoQueryWith2KeysAndReturnListOfResultAsync(_db!,
                "Payment_Records", "InstallmentPlanNumber", planCreateResponse.InstallmentPlanNumber, 
                "Activity", "FND", "Sku", "_id");
            Assert.That(docsListFnd!.Count == 2);
            Assert.That(_mongoHandler.ValidateBillingAmountInDocsLists(docsListFnd, 1));
            var docsListRed = await _mongoHandler.SendMongoQueryWith2KeysAndReturnListOfResultAsync(_db!,
                "Payment_Records", "InstallmentPlanNumber", planCreateResponse.InstallmentPlanNumber, 
                "Activity", "RED", "Sku", "_id");
            Assert.That(docsListRed!.Count == 1);
            var docsListDel = await _mongoHandler.SendMongoQueryWith2KeysAndReturnListOfResultAsync(_db!,
                "Payment_Records",
                "InstallmentPlanNumber",
                planCreateResponse.InstallmentPlanNumber, "Activity", "DEL", "Sku", "_id");
            Assert.That(docsListDel!.Count == 1);
            Console.WriteLine("Done TestValidate_DBS_FUN_STL_MON_FTR_CRC_F09_Processing");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidate_DBS_FUN_STL_MON_FTR_CRC_F09_Processing \n" + exception + "\n");
        }
    }
}