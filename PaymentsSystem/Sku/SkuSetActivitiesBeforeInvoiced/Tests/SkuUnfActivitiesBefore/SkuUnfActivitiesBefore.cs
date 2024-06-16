using MongoDB.Driver;
using Allure.NUnit;
using Allure.NUnit.Attributes;
using NUnit.Framework;
using Splitit.Automation.NG.Backend.BaseActions;
using Splitit.Automation.NG.Backend.Services.AdminApi.Merchant.MerchantFunctionality;
using Splitit.Automation.NG.Backend.Services.V3.DefaultValues;
using Splitit.Automation.NG.Backend.Services.V3.InstallmentPlan.InstallmentPlanApiEndPoint;
using Splitit.Automation.NG.Backend.Tests.TestsSetups;
using Splitit.Automation.NG.PaymentsSystem.SplititOperations.EndPointsFunctionality;
using Splitit.Automation.NG.Utilities.EnvironmentsConfig;

namespace Splitit.Automation.NG.PaymentsSystem.Sku.SkuSetActivitiesBeforeInvoiced.Tests.SkuUnfActivitiesBefore;

[TestFixture]
[AllureNUnit]
[AllureSuite("SkuUnfActivitiesBefore")]
[AllureDisplayIgnored]
public class SkuUnfActivitiesBefore
{
    private RequestHeader? _requestHeader;
    private readonly InstallmentPlans _installmentPlans;
    private readonly MongoHandler _mongoHandler;
    private readonly MovePlanToFnd _movePlanToFnd;
    private readonly Settings _settings;
    private readonly SettingsSave _settingsSave;
    private IMongoDatabase? _db;
    private readonly EnvConfig _envConfig;
    

    public SkuUnfActivitiesBefore()
    {
        Console.WriteLine("Staring SKU Setup");
        _installmentPlans = new InstallmentPlans();
        _mongoHandler = new MongoHandler();
        _movePlanToFnd = new MovePlanToFnd();
        _settings = new Settings();
        _settingsSave = new SettingsSave();
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
        _db = _mongoHandler.MongoConnect(_envConfig.MongoConnection, "Splitit_Payments");
    }
    
    [TestCase(Category = "SkuUnfActivitiesBefore")]
    [TestCase(Category = "PaymentSystemSmoke")]
    [Test(Description = "TestValidate_MER_UNF_STL_MON_FTR_CRC_U03_ActivitiesBefore with generated merchant"), CancelAfter(120*1000)]
    public async Task TestValidate_MER_UNF_STL_MON_FTR_CRC_U03_ActivitiesBefore()
    {
        try
        {
            Console.WriteLine("\nStarting TestValidate_MER_UNF_STL_MON_FTR_CRC_U03_ActivitiesBefore");
            var terminalApiKey = _envConfig.MER_UNF_STL_MON_FTR_CRC_U03;
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            var planCreateResponse = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!,  _requestHeader!,
                "Active", 3, terminalApiKey, createPlanDefaultValues);
            var (statusCol, idCol) = await _mongoHandler.SendMongoQueryWith2KeysAsync(_db!, "Payment_Records", "InstallmentPlanNumber",
                planCreateResponse.InstallmentPlanNumber, "Activity", "COL", "Status", "_id");
            Assert.That(statusCol.Equals("New"));
            Assert.That(idCol, Is.Not.Null);
            var jResponseSettingsRequest = await _settings.SendGetSettingsRequestAsync( _requestHeader!, _envConfig.MER_UNF_STL_MON_FTR_CRC_U03_BU);
            jResponseSettingsRequest!.MerchantSettings.PaymentSettings.CreditLine = "99999";
            jResponseSettingsRequest.MerchantSettings.PaymentSettings.FundingStartDate = DateTime.Today.AddDays(-1);
            jResponseSettingsRequest.MerchantSettings.PaymentSettings.FundingTrigger = "PlanActivation";
            var jResponseUpdateSettingsRequest = await _settingsSave.SendUpdateSettingsRequestAsync( _requestHeader!, jResponseSettingsRequest);
            Assert.That(jResponseUpdateSettingsRequest.ResponseHeader.Succeeded);
            var jResponse = await _movePlanToFnd.SendPostRequestMovePlanToFndAsync( _requestHeader!, planCreateResponse.InstallmentPlanNumber);
            Assert.That(jResponse.Contains(""));
            var docsList = await _mongoHandler.SendMongoQueryWith2KeysAndReturnListOfResultAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber",
                planCreateResponse.InstallmentPlanNumber, "Activity", "COL", "Sku", "_id");
            Assert.That(_mongoHandler.ValidateValueInDocsList(docsList!, "Sku", "FUN"));
            Console.WriteLine("Done TestValidate_MER_UNF_STL_MON_FTR_CRC_U03_ActivitiesBefore");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidate_MER_UNF_STL_MON_FTR_CRC_U03_ActivitiesBefore \n" + exception + "\n");
        }
    }
}