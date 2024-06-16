using MongoDB.Driver;
using Allure.NUnit;
using Allure.NUnit.Attributes;
using NUnit.Framework;
using Splitit.Automation.NG.Backend.BaseActions;
using Splitit.Automation.NG.Backend.Services.V3.DefaultValues;
using Splitit.Automation.NG.Backend.Tests.TestsSetups;
using Splitit.Automation.NG.PaymentsSystem.Sku.SkuMatching.Tests.SkuTestsData;

namespace Splitit.Automation.NG.Backend.Tests.ReportTests.SettlementReportTestSuite;

[TestFixture]
[AllureNUnit]
[AllureSuite("ReportsTests")]
[AllureDisplayIgnored]
public class SettlementReportTests
{
    private RequestHeader? _requestHeader;
    private readonly ExcelFileController _excelFileController;
    private readonly SkuTestsData _skuTestsData;
    private Dictionary<string, Dictionary<string, string>>? _signDict;
    private Dictionary<string, SkuTestsData.FeesSettings>? _feesDict;
    private readonly MongoHandler _mongoHandler;
    private IMongoDatabase? _db;
    private Dictionary<string, string>? _splititActivityMerchantFacingList;
    private readonly TestsWrapper.TestsWrapper _testsWrapper;
    private readonly TestsHelper.TestsHelper _testsHelper;


    public SettlementReportTests()
    {
        Console.WriteLine("\nStaring FndMerchantsTests Setup");
        _excelFileController = new ExcelFileController();
        _skuTestsData = new SkuTestsData();
        _mongoHandler = new MongoHandler();
        _testsWrapper = new TestsWrapper.TestsWrapper();
        _testsHelper = new TestsHelper.TestsHelper();
        Console.WriteLine("FndMerchantsTests Setup Succeeded\n");
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
        var csv = _excelFileController.ReadExcelFile("settlementReport");
        (_, _signDict, _feesDict) = _skuTestsData.BuildPaymentSettingsDictionary(csv, null!, "yes");
        _db = _mongoHandler.MongoConnect(Environment.GetEnvironmentVariable("MongoConnection")!, "Splitit_Payments");
        _splititActivityMerchantFacingList = _testsHelper.ReturnSplititActivityMerchantFacingMapping();
    }

    
    [Category("ReportsTests")]
    [Test(Description = "TestValidateSettlementAndGrossReports")]
    public async Task TestValidateSettlementAndGrossReports()
    {
        try
        {
            Console.WriteLine("\nStarting TestValidateSettlementAndGrossReports");
            var terminalApiKeyMerUnfGrossSettle = Environment.GetEnvironmentVariable("MER_UNF_GROSS_SETTLE_Api_Key");
            var merchantIdMerUnfGrossSettle = Environment.GetEnvironmentVariable("MER_UNF_GROSS_SETTLE_Merchant_Id");
            var terminalApiKeyMerFndNetSettle = Environment.GetEnvironmentVariable("MER_FND_NET_SETTLE_Api_Key");
            var merchantIdMerFndNetSettle = Environment.GetEnvironmentVariable("MER_FND_NET_SETTLE_Merchant_Id");
            var terminalApiKeyMerFndGrossSettle = Environment.GetEnvironmentVariable("MER_FND_GROSS_SETTLE_Api_Key");
            var merchantIdMerFndGrossSettle = Environment.GetEnvironmentVariable("MER_FND_GROSS_SETTLE_Merchant_Id");
            var terminalApiKeyDbsNetSettle = Environment.GetEnvironmentVariable("DBS_NET_SETTLE_Api_Key");
            var merchantIdDbsNetSettle = Environment.GetEnvironmentVariable("DBS_NET_SETTLE_Merchant_Id");
            var terminalApiKeyAu = Environment.GetEnvironmentVariable("AU_FND_NETSETTLE_Api_Key");
            var merchantIdAu = Environment.GetEnvironmentVariable("AU_FND_NETSETTLE_Merchant_Id");
            
            
            var planCreateResponseU01 = await _testsWrapper.InitPlanAndQueryMongo(
                "TestValidate_MER_UNF_STL_MON_FTR_CRC_U01", terminalApiKeyMerUnfGrossSettle!, _requestHeader!, _db!,
                _feesDict!, 1);
            var planCreateResponseU03 = await _testsWrapper.InitPlanAndQueryMongo(
                "TestValidate_MER_UNF_STL_MON_FTR_CRC_U03", terminalApiKeyMerUnfGrossSettle!, _requestHeader!, _db!,
                _feesDict!, 3);
            var planCreateResponseU05 = await _testsWrapper.InitPlanAndQueryMongo(
                "TestValidate_MER_UNF_STL_MON_FTR_CRC_U05", terminalApiKeyMerUnfGrossSettle!, _requestHeader!, _db!,
                _feesDict!, 5);
            var (planCreateResponseF01, _) = await _testsWrapper.InitPlanAndQueryMongoFnd(
                "TestValidate_MER_FUN_STL_MON_FTR_CRC_F01", terminalApiKeyMerFndNetSettle!, _requestHeader!, _db!,
                _feesDict!, 1);
            var (planCreateResponseF03, _) = await _testsWrapper.InitPlanAndQueryMongoFnd(
                "TestValidate_MER_FUN_STL_MON_FTR_CRC_F03", terminalApiKeyMerFndNetSettle!, _requestHeader!, _db!,
                _feesDict!, 3);
            var (planCreateResponseF07, invoiceIdF07) = await _testsWrapper.InitPlanAndQueryMongoFnd(
                "TestValidate_MER_FUN_STL_MON_FTR_CRC_F07", terminalApiKeyMerFndNetSettle!, _requestHeader!, _db!,
                _feesDict!, 7);
            await _testsWrapper.DoRefundAndQueryIt(planCreateResponseF07, _requestHeader!, _db!,
                "TestValidate_MER_FUN_STL_MON_FTR_CRC_F07", invoiceIdF07, _feesDict!);
            var (planCreateResponseF01Gross, _) = await _testsWrapper.InitPlanAndQueryMongoFnd(
                "TestValidate_MER_FUN_STL_MON_FTR_CRC_F01", terminalApiKeyMerFndGrossSettle!, _requestHeader!, _db!,
                _feesDict!, 1);
            var (planCreateResponseF03Gross, _) = await _testsWrapper.InitPlanAndQueryMongoFnd(
                "TestValidate_MER_FUN_STL_MON_FTR_CRC_F03", terminalApiKeyMerFndGrossSettle!, _requestHeader!, _db!,
                _feesDict!, 3);
            var planCreateResponseDbs04 = await _testsWrapper.InitPlanAndQueryMongo(
                "TestValidate_DBS_FUN_STL_MON_FTR_CRC_F04", terminalApiKeyDbsNetSettle!, _requestHeader!, _db!,
                _feesDict!, 4);
            var (planCreateResponseDbs05, invoiceIdF05) = await _testsWrapper.InitPlanAndQueryMongoFnd(
                "TestValidate_DBS_FUN_STL_MON_FTR_CRC_F05", terminalApiKeyDbsNetSettle!, _requestHeader!, _db!,
                _feesDict!, 5);
            await _testsWrapper.DoRefundAndQueryIt(planCreateResponseDbs05, _requestHeader!, _db!,
                "TestValidate_DBS_FUN_STL_MON_FTR_CRC_F05", invoiceIdF05, _feesDict!);
            var planCreateResponseAuF05 = await _testsWrapper.InitPlanAndQueryMongo(
                "TestValidate_MER_FUN_STL_MON_FTR_CRC_F05", terminalApiKeyAu!, _requestHeader!, _db!,
                _feesDict!, 5);
            
            
            await _testsWrapper.GenerateReport();
            
            
            var filePathDownloadMerUnfGrossSettle = await _testsWrapper.DownloadReport(_requestHeader!, 
                merchantIdMerUnfGrossSettle!, "SettlementReport");
            var filePathDownloadMerFndNetSettle = await _testsWrapper.DownloadReport(_requestHeader!, 
                merchantIdMerFndNetSettle!, "SettlementReport");
            var filePathDownloadMerFndGrossSettleSettleReport = await _testsWrapper.DownloadReport(_requestHeader!, 
                merchantIdMerFndNetSettle!, "SettlementReport");
            var filePathDownloadMerFndGrossSettle = await _testsWrapper.DownloadReport(_requestHeader!, 
                merchantIdMerFndGrossSettle!, "SettlementReportGrossDebit");
            var filePathDownloadDbsFndNetSettle = await _testsWrapper.DownloadReport(_requestHeader!, 
                merchantIdDbsNetSettle!, "SettlementReport");
            var filePathDownloadAu = await _testsWrapper.DownloadReport(_requestHeader!, 
                merchantIdAu!, "SettlementReport");
            
            
            var settlementReportValuesListU01 = await _testsHelper.ConvertCsvToListDict(filePathDownloadMerUnfGrossSettle, 
                planCreateResponseU01.InstallmentPlanNumber);
            var settlementReportValuesListU03 = await _testsHelper.ConvertCsvToListDict(filePathDownloadMerUnfGrossSettle, 
                planCreateResponseU03.InstallmentPlanNumber);
            var settlementReportValuesListU05 = await _testsHelper.ConvertCsvToListDict(filePathDownloadMerUnfGrossSettle, 
                planCreateResponseU05.InstallmentPlanNumber);
            var settlementReportValuesListF01 = await _testsHelper.ConvertCsvToListDict(filePathDownloadMerFndNetSettle,
                planCreateResponseF01.InstallmentPlanNumber);
            var settlementReportValuesListF03 = await _testsHelper.ConvertCsvToListDict(filePathDownloadMerFndNetSettle,
                planCreateResponseF03.InstallmentPlanNumber);
            var settlementReportValuesListF07 = await _testsHelper.ConvertCsvToListDict(filePathDownloadMerFndNetSettle,
                planCreateResponseF07.InstallmentPlanNumber);
            var settlementReportValuesListF01Gross = await _testsHelper.ConvertCsvToListDict(filePathDownloadMerFndGrossSettle,
                planCreateResponseF01Gross.InstallmentPlanNumber);
            var settlementReportValuesListF01Set = await _testsHelper.ConvertCsvToListDict(filePathDownloadMerFndGrossSettleSettleReport,
                planCreateResponseF01Gross.InstallmentPlanNumber);
            var settlementReportValuesListF03Gross = await _testsHelper.ConvertCsvToListDict(filePathDownloadMerFndGrossSettle,
                planCreateResponseF03Gross.InstallmentPlanNumber);
            var settlementReportValuesListF03Set = await _testsHelper.ConvertCsvToListDict(filePathDownloadMerFndGrossSettleSettleReport,
                planCreateResponseF03Gross.InstallmentPlanNumber);
            var settlementReportValuesListF04Net = await _testsHelper.ConvertCsvToListDict(filePathDownloadDbsFndNetSettle,
                planCreateResponseDbs04.InstallmentPlanNumber);
            var settlementReportValuesListF05Net = await _testsHelper.ConvertCsvToListDict(filePathDownloadDbsFndNetSettle,
                planCreateResponseDbs05.InstallmentPlanNumber);
            var settlementReportValuesListAu = await _testsHelper.ConvertCsvToListDict(filePathDownloadAu,
                planCreateResponseAuF05.InstallmentPlanNumber);
            
            
            Assert.That(await _testsWrapper.ValidateReport("TestValidate_MER_UNF_STL_MON_FTR_CRC_U01", 
                2, settlementReportValuesListU01, _splititActivityMerchantFacingList!, 
                planCreateResponseU01, _signDict!, new List<string> {"COL", "FND"}));
            Assert.That(await _testsWrapper.ValidateReport("TestValidate_MER_UNF_STL_MON_FTR_CRC_U03", 
                2, settlementReportValuesListU03, _splititActivityMerchantFacingList!, 
                planCreateResponseU01, _signDict!, new List<string> {"COL", "FND"}));
            Assert.That(await _testsWrapper.ValidateReport("TestValidate_MER_UNF_STL_MON_FTR_CRC_U05", 
                2, settlementReportValuesListU05, _splititActivityMerchantFacingList!, 
                planCreateResponseU05, _signDict!, new List<string> {"COL", "FND"}));
            Assert.That(await _testsWrapper.ValidateReport("TestValidate_MER_FUN_STL_MON_FTR_CRC_F01", 
                2, settlementReportValuesListF01, _splititActivityMerchantFacingList!, 
                planCreateResponseF01, _signDict!, new List<string> {"COL", "FND"}));
            Assert.That(await _testsWrapper.ValidateReport("TestValidate_MER_FUN_STL_MON_FTR_CRC_F03", 
                2, settlementReportValuesListF03, _splititActivityMerchantFacingList!, 
                planCreateResponseF03, _signDict!, new List<string> {"COL", "FND"}));
            Assert.That(await _testsWrapper.ValidateReportFnd("TestValidate_MER_FUN_STL_MON_FTR_CRC_F07", 
                2, settlementReportValuesListF07, _splititActivityMerchantFacingList!, 
                planCreateResponseF07, _signDict!, new List<string> {"COL", "FND", "RED"}));
            Assert.That(await _testsWrapper.ValidateReportFnd("TestValidate_MER_FUN_STL_MON_FTR_CRC_F07", 
                2, settlementReportValuesListF07, _splititActivityMerchantFacingList!, 
                planCreateResponseF07, _signDict!, new List<string> {"COL", "FND", "RED"}));
            Assert.That(await _testsWrapper.ValidateReport("TestValidate_MER_FUN_STL_MON_FTR_CRC_F01", 
                1, settlementReportValuesListF01Set, _splititActivityMerchantFacingList!, 
                planCreateResponseF03, _signDict!, new List<string> {"COL"}));
            Assert.That(await _testsWrapper.ValidateReportFnd("TestValidate_MER_FUN_STL_MON_FTR_CRC_F01", 
                1, settlementReportValuesListF01Gross, _splititActivityMerchantFacingList!, 
                planCreateResponseF07, _signDict!, new List<string> {"FND"}));
            Assert.That(await _testsWrapper.ValidateReport("TestValidate_MER_FUN_STL_MON_FTR_CRC_F03", 
                1, settlementReportValuesListF03Set, _splititActivityMerchantFacingList!, 
                planCreateResponseF03, _signDict!, new List<string> {"COL"}));
            Assert.That(await _testsWrapper.ValidateReportFnd("TestValidate_MER_FUN_STL_MON_FTR_CRC_F03", 
                1, settlementReportValuesListF03Gross, _splititActivityMerchantFacingList!, 
                planCreateResponseF07, _signDict!, new List<string> {"FND"}));
            Assert.That(await _testsWrapper.ValidateReport("TestValidate_DBS_FUN_STL_MON_FTR_CRC_F04", 
                2, settlementReportValuesListF04Net, _splititActivityMerchantFacingList!, 
                planCreateResponseDbs04, _signDict!, new List<string> {"COL", "FND"}));
            Assert.That(await _testsWrapper.ValidateReport("TestValidate_DBS_FUN_STL_MON_FTR_CRC_F05", 
                2, settlementReportValuesListF05Net, _splititActivityMerchantFacingList!, 
                planCreateResponseDbs05, _signDict!, new List<string> {"FND", "RED"}));
            Assert.That(await _testsWrapper.ValidateReport("TestValidate_MER_FUN_STL_MON_FTR_CRC_F05", 
                2, settlementReportValuesListAu, _splititActivityMerchantFacingList!, 
                planCreateResponseAuF05, _signDict!, new List<string> {"COL", "FND"}));
            
            
            File.Delete(filePathDownloadMerUnfGrossSettle);
            File.Delete(filePathDownloadMerFndNetSettle);
            File.Delete(filePathDownloadMerFndGrossSettle);
            File.Delete(filePathDownloadMerFndGrossSettleSettleReport);
            Console.WriteLine("TestValidateSettlementAndGrossReports is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidateSettlementAndGrossReports\n" + exception + "\n");
        }
    }
}