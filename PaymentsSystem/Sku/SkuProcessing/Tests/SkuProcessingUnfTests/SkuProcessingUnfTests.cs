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

namespace Splitit.Automation.NG.PaymentsSystem.Sku.SkuProcessing.Tests.SkuProcessingUnfTests;

[TestFixture]
[AllureNUnit]
[AllureSuite("SkuUnfProcessingTests")]
[AllureDisplayIgnored]
public class SkuProcessingUnfTests
{
    private RequestHeader? _requestHeader;
    private readonly InstallmentPlans _installmentPlans;
    private readonly MongoHandler _mongoHandler;
    private string? _terminalApiKey;
    private readonly Processing _processing;
    private IMongoDatabase? _db;
    private readonly EnvConfig _envConfig;
    

    public SkuProcessingUnfTests()
    {
        Console.WriteLine("Staring SKU processing Setup");
        _installmentPlans = new InstallmentPlans();
        _mongoHandler = new MongoHandler();
        _processing = new Processing();
        _envConfig = new EnvConfig();
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
    
    [TestCase(Category = "SkuUnfProcessingTests")]
    [Test(Description = "TestValidate_MER_UNF_STL_BIW_FTR_CRC_U03_Processing with generated merchant"), CancelAfter(480*1000)]
    public async Task TestValidate_MER_UNF_STL_BIW_FTR_CRC_U03_Processing()
    {
        try
        {
            Console.WriteLine("\nStarting TestValidate_MER_UNF_STL_BIW_FTR_CRC_U03_Processing");
            _terminalApiKey = _envConfig.MER_UNF_STL_BIW_FTR_CRC_U03_Processing;
            Assert.That(_terminalApiKey, Is.Not.EqualTo(""));
            Console.WriteLine("Done with creation of a new merchant , settings and terminal process");
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            var planCreateResponse = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!,  _requestHeader!,
                "PendingCapture", 3, _terminalApiKey, createPlanDefaultValues);
            var response = await _processing.SendGetForProcessingAsync( _requestHeader!, planCreateResponse.InstallmentPlanNumber);
            Assert.That(response, Is.EqualTo(""));
            var (activity, id) = await _mongoHandler.SendMongoQueryForStatusAndIdAsync(_db!, "Payment_Records", "InstallmentPlanNumber",
                planCreateResponse.InstallmentPlanNumber, "Status", "_id");
            Assert.That(activity, Is.Not.Null);
            Assert.That(id, Is.Not.Null);
            Console.WriteLine("Done TestValidate_MER_UNF_STL_BIW_FTR_CRC_U03_Processing");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidate_MER_UNF_STL_BIW_FTR_CRC_U03_Processing \n" + exception);
        }
    }
}