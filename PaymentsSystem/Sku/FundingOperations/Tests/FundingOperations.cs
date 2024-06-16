using MongoDB.Driver;
using Allure.NUnit;
using Allure.NUnit.Attributes;
using NUnit.Framework;
using Splitit.Automation.NG.Backend.BaseActions;
using Splitit.Automation.NG.Backend.Services.V3.DefaultValues;
using Splitit.Automation.NG.Backend.Services.V3.InstallmentPlan.InstallmentPlanApiEndPoint;
using Splitit.Automation.NG.Backend.Tests.TestsSetups;
using Splitit.Automation.NG.PaymentsSystem.Sku.SkuMatching.Tests.SkuTestsData;
using Splitit.Automation.NG.PaymentsSystem.SplititOperations.EndPointsFunctionality;
using Splitit.Automation.NG.Utilities.EnvironmentsConfig;
using List = Splitit.Automation.NG.Backend.Services.AdminApi.Merchant.MerchantFunctionality.List;

namespace Splitit.Automation.NG.PaymentsSystem.Sku.FundingOperations.Tests;

[TestFixture]
[AllureNUnit]
[AllureSuite("FundingOperationsTestsSuite")]
[AllureDisplayIgnored]
[Parallelizable(ParallelScope.All)]
public class FundingOperations
{
    // private RequestHeader? _requestHeader;
    // private readonly ExcelFileController _excelFileController;
    // private readonly SkuTestsData _skuTestsData;
    // private readonly InstallmentPlans _installmentPlans;
    // private Dictionary<string, SkuTestsData.PaymentSettings>? _skuDict;
    // private readonly MongoHandler _mongoHandler;
    // private readonly GetPaymentFundingOperations _getPaymentFundingOperations;
    // private readonly List _list;
    // private readonly TestsHelper.TestsHelper _testsHelper;
    // private readonly GetPaymentRecordsFunctionality _getPaymentRecordsFunctionality;
    // private readonly GetFailedPaymentOperations _getFailedPaymentOperations;
    // private string? _ipn1, _ipn2, _ipn3, _ipn4, _ipn5;
    // private bool _flag;
    // private int _businessUnitId;
    // private string? _terminalApiKey;
    // private IMongoDatabase? _db;
    // private readonly EnvConfig _envConfig;
    //
    // public FundingOperations()
    // {
    //     Console.WriteLine("Staring FundingOperations Setup");
    //     _excelFileController = new ExcelFileController();
    //     _skuTestsData = new SkuTestsData();
    //     _installmentPlans = new InstallmentPlans();
    //     _mongoHandler = new MongoHandler();
    //     _getPaymentFundingOperations = new GetPaymentFundingOperations();
    //     _list = new List();
    //     _testsHelper = new TestsHelper.TestsHelper();
    //     _getPaymentRecordsFunctionality = new GetPaymentRecordsFunctionality();
    //     _getFailedPaymentOperations = new GetFailedPaymentOperations();
    //     _envConfig = new EnvConfig();
    //     Console.WriteLine("FundingOperations Setup Succeeded\n");
    // }
    //
    // [OneTimeSetUp]
    // public void InitSetUp()
    // {
    //     var testsSetup = new TestsSetup();
    //     testsSetup.Setup();
    //     var sendAdminLoginRequest = new SendAdminLoginRequest();
    //     _requestHeader = sendAdminLoginRequest.DoAdminLogin(_envConfig.AccessTokenURI,
    //         _envConfig.ClientSecret, _envConfig.SplititMockTerminal);
    //     var csv = _excelFileController.ReadExcelFile("fundingOperations");
    //     (_skuDict, _, _) = _skuTestsData.BuildPaymentSettingsDictionary(csv, "yes");
    //     _db = _mongoHandler.MongoConnect(_envConfig.MongoConnection, "Splitit_Payments");
    // }
    //
    // [TestCase(Category = "FundingOperations")]
    // [TestCase(Category = "PaymentSystemSmoke")]
    // [Test(Description = "TestValidate_MER_FUN_STL_MON_FTR_CRC_F09 with generated merchant"), CancelAfter(960 * 1000)]
    // public async Task TestValidate_MER_FUN_STL_MON_FTR_CRC_F09()
    // {
    //     try
    //     {
    //         Console.WriteLine("\nStarting TestValidate_MER_FUN_STL_MON_FTR_CRC_F09");
    //         var testName = TestContext.CurrentContext.Test.Name;
    //         (_terminalApiKey, _businessUnitId) = await _testsHelper.CreateMerchantFunctionality( _requestHeader!,
    //             testName, _db!, "PaymentWizard.v3.5", "NonSecuredPlan", "Monthly", _skuDict!, "315");
    //         Assert.That(_terminalApiKey, Is.Not.EqualTo(""));
    //         var jResponse = await _list.SendGetMerchantListRequestAsync( _requestHeader!, _businessUnitId);
    //         Console.WriteLine("Done with creation of a new merchant , settings and terminal process");
    //         var createPlanDefaultValues = new CreatePlanDefaultValues();
    //         var is3DsVarsCreation = new Is3DsVarsCreation();
    //         createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
    //         var json = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!,  _requestHeader!,
    //             "PendingCapture", 9, _terminalApiKey, createPlanDefaultValues);
    //         var db = _mongoHandler.MongoConnect(_envConfig.MongoConnection, "Splitit_Payments");
    //         var (activityStatusCol, idStatusCol) = await _mongoHandler.SendMongoQueryWith2KeysAsync(db!, "Payment_Records",
    //             "InstallmentPlanNumber", json.InstallmentPlanNumber, "Activity", "COL", "Status", "_id");
    //         if (activityStatusCol.Equals("New") || activityStatusCol.Equals("PendingBatch"))
    //         {
    //             Console.WriteLine("activityStatusCol is in the right state");
    //         }
    //         else
    //         {
    //             Assert.Fail("activityStatusCol is not in the right state");
    //         }
    //         Assert.That(idStatusCol, Is.Not.Null);
    //         var (activityStatusFnd, idStatusFnd) = await _mongoHandler.SendMongoQueryWith2KeysAsync(db!, "Payment_Records",
    //             "InstallmentPlanNumber", json.InstallmentPlanNumber, "Activity", 
    //             "FND", "Status", "_id");
    //         if (activityStatusFnd.Equals("New") || activityStatusFnd.Equals("PendingBatch"))
    //         {
    //             Console.WriteLine("activityStatusFnd is in the right state");
    //         }
    //         else
    //         {
    //             Assert.Fail("activityStatusFnd is not in the right state");
    //         }
    //         Assert.That(idStatusFnd, Is.Not.Null);
    //         var requestToGetPaymentFundingOperations = await _getPaymentFundingOperations.SendPostRequestToGetPaymentFundingOperationsAsync(
    //              _requestHeader!, jResponse!.Merchants[0].Id.ToString(), 
    //             10, 1, "merchantName", json.InstallmentPlanNumber);
    //         Assert.That(requestToGetPaymentFundingOperations!.paymentFundingOperationResponses[0].activitiesList.Count == 2);
    //         Assert.That(requestToGetPaymentFundingOperations.paymentFundingOperationResponses[0].collectNumberOfActivities == 1);
    //         Assert.That(requestToGetPaymentFundingOperations.paymentFundingOperationResponses[0].fundNumberOfActivities == 1);
    //         Console.WriteLine("Done TestValidate_MER_FUN_STL_MON_FTR_CRC_F09");
    //     }
    //     catch (Exception exception)
    //     {
    //         Assert.Fail("Error in TestValidate_MER_FUN_STL_MON_FTR_CRC_F09 \n" + exception + "\n");
    //     }
    // }
    //
    // [TestCase(Category = "FundingOperations")]
    // [Test(Description = "TestValidate_MER_FUN_STL_MON_FTR_CRC_F09_PartialRefund_OutstandingAmount with generated merchant"), CancelAfter(960 * 1000)]
    // public async Task TestValidate_MER_FUN_STL_MON_FTR_CRC_F09_PartialRefund_OutstandingAmount()
    // {
    //     try
    //     {
    //         Console.WriteLine("\nStarting TestValidate_MER_FUN_STL_MON_FTR_CRC_F09_PartialRefund_OutstandingAmount");
    //         var testName = TestContext.CurrentContext.Test.Name;
    //         (_terminalApiKey, _businessUnitId) = await _testsHelper.CreateMerchantFunctionality( _requestHeader!,
    //             testName, _db!,"PaymentWizard.v3.5", "NonSecuredPlan", "Monthly", _skuDict!, "315");
    //         Assert.That(_terminalApiKey, Is.Not.EqualTo(""));
    //         var jResponse = await _list.SendGetMerchantListRequestAsync( _requestHeader!, _businessUnitId);
    //         Console.WriteLine("Done with creation of a new merchant , settings and terminal process");
    //         var createPlanDefaultValues = new CreatePlanDefaultValues();
    //         var is3DsVarsCreation = new Is3DsVarsCreation();
    //         createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
    //         Console.WriteLine("Creating 5 plans and querying mongo");
    //         (_flag, _ipn1) = await _testsHelper.CreatePlanAndQueryMongo(_terminalApiKey, _businessUnitId, _db!,
    //             _requestHeader!, createPlanDefaultValues, 7, 9,
    //             "PartialRefund", null!);
    //         Assert.That(_flag);
    //         (_flag, _ipn2) = await _testsHelper.CreatePlanAndQueryMongo(_terminalApiKey, _businessUnitId, _db!,
    //             _requestHeader!, createPlanDefaultValues, 7, 9,
    //             "PartialRefund", null!);
    //         Assert.That(_flag);
    //         (_flag, _ipn3) = await _testsHelper.CreatePlanAndQueryMongo(_terminalApiKey, _businessUnitId, _db!,
    //             _requestHeader!, createPlanDefaultValues, 7, 9,
    //             "PartialRefund", null!);
    //         Assert.That(_flag);
    //         (_flag, _ipn4) = await _testsHelper.CreatePlanAndQueryMongo(_terminalApiKey, _businessUnitId, _db!,
    //             _requestHeader!, createPlanDefaultValues, 7, 9,
    //             "PartialRefund", null!);
    //         Assert.That(_flag);
    //         (_flag, _ipn5) = await _testsHelper.CreatePlanAndQueryMongo(_terminalApiKey, _businessUnitId, _db!,
    //             _requestHeader!, createPlanDefaultValues, 7, 9,
    //             "PartialRefund", null!);
    //         Assert.That(_flag);
    //         Console.WriteLine("Done Creating 5 plans and querying mongo\n");
    //         await Task.Delay(5 * 1000);
    //         var requestToGetPaymentFundingOperations =  await _getPaymentFundingOperations.SendPostRequestToGetPaymentFundingOperationsAsync(
    //              _requestHeader!, jResponse!.Merchants[0].Id.ToString(), 10, 1, "merchantName");
    //         Assert.That(requestToGetPaymentFundingOperations!.paymentFundingOperationResponses[0].activitiesList.Count == 2);
    //         Assert.That(requestToGetPaymentFundingOperations.paymentFundingOperationResponses[0].collectNumberOfActivities == 5);
    //         Assert.That(requestToGetPaymentFundingOperations.paymentFundingOperationResponses[0].fundNumberOfActivities == 5);
    //         Console.WriteLine("Done TestValidate_MER_FUN_STL_MON_FTR_CRC_F09_PartialRefund_OutstandingAmount");
    //     }
    //     catch (Exception exception)
    //     {
    //         Assert.Fail("Error in TestValidate_MER_FUN_STL_MON_FTR_CRC_F09_PartialRefund_OutstandingAmount \n" +
    //                     exception + "\n");
    //     }
    // }
    //
    // [TestCase(Category = "FundingOperations")]
    // [Test(Description = "TestValidate_DBS_FUN_STL_MON_FTR_CRC_F09_PartialRefund_OutstandingBalanceFirst_IncludePaymentsRecords with generated merchant"),
    //  CancelAfter(960 * 1000)]
    // public async Task TestValidate_DBS_FUN_STL_MON_FTR_CRC_F09_PartialRefund_OutstandingBalanceFirst_IncludePaymentsRecords()
    // {
    //     try
    //     {
    //         Console.WriteLine("\nStarting TestValidate_DBS_FUN_STL_MON_FTR_CRC_F09_PartialRefund_PastCharge_IncludePaymentsRecords");
    //         var testName = TestContext.CurrentContext.Test.Name;
    //         (_terminalApiKey, _businessUnitId) = await _testsHelper.CreateMerchantFunctionality( _requestHeader!,
    //             testName, _db!,"PaymentWizard.v3.5", "NonSecuredPlan", "Monthly", _skuDict!, "109");
    //         Assert.That(_terminalApiKey, Is.Not.EqualTo(""));
    //         var jResponse = await _list.SendGetMerchantListRequestAsync( _requestHeader!, _businessUnitId);
    //         Console.WriteLine("Done with creation of a new merchant , settings and terminal process");
    //         var createPlanDefaultValues = new CreatePlanDefaultValues();
    //         var is3DsVarsCreation = new Is3DsVarsCreation();
    //         createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
    //         Console.WriteLine("Creating 5 plans and querying mongo");
    //         (_flag, _ipn1) = await _testsHelper.CreatePlanAndQueryMongoWithFailedActivities(_terminalApiKey, _businessUnitId,_db!,
    //              _requestHeader!, createPlanDefaultValues, 7, 9,
    //             "PartialRefund", null!);
    //         Assert.That(_flag);
    //         (_flag, _ipn2) = await _testsHelper.CreatePlanAndQueryMongoWithFailedActivities(_terminalApiKey, _businessUnitId,_db!,
    //              _requestHeader!, createPlanDefaultValues, 7, 9,
    //             "PartialRefund", null!);
    //         Assert.That(_flag);
    //         (_flag, _ipn3) = await _testsHelper.CreatePlanAndQueryMongoWithFailedActivities(_terminalApiKey, _businessUnitId,_db!,
    //              _requestHeader!, createPlanDefaultValues, 7, 9,
    //             "PartialRefund", null!);
    //         Assert.That(_flag);
    //         (_flag, _ipn4) = await _testsHelper.CreatePlanAndQueryMongoWithFailedActivities(_terminalApiKey, _businessUnitId,_db!,
    //              _requestHeader!, createPlanDefaultValues, 7, 9,
    //             "PartialRefund", null!);
    //         Assert.That(_flag);
    //         (_flag, _ipn5) = await _testsHelper.CreatePlanAndQueryMongoWithFailedActivities(_terminalApiKey, _businessUnitId,_db!,
    //              _requestHeader!, createPlanDefaultValues, 7, 9,
    //             "PartialRefund", null!);
    //         Assert.That(_flag);
    //         await Task.Delay(5 * 1000);
    //         Console.WriteLine("Done Creating 5 plans and querying mongo\n");
    //         var requestToGetPaymentFundingOperations = 
    //             await _getPaymentFundingOperations.SendPostRequestToGetPaymentFundingOperationsAsync( _requestHeader!,
    //                 jResponse!.Merchants[0].Id.ToString(), 10, 1, "merchantName");
    //         Assert.That(requestToGetPaymentFundingOperations!.paymentFundingOperationResponses[0].activitiesList.Count == 2);
    //         Assert.That(requestToGetPaymentFundingOperations.paymentFundingOperationResponses[0].collectNumberOfActivities == 5);
    //         Assert.That(requestToGetPaymentFundingOperations.paymentFundingOperationResponses[0].fundNumberOfActivities == 5);
    //         var jResponseGetPaymentRecordRequest1 = await _getPaymentRecordsFunctionality.SendGetPaymentRecordRequestAsync(
    //              _requestHeader!, _ipn1);
    //         Assert.That(jResponseGetPaymentRecordRequest1.totalRecords > 0);
    //         var jResponseGetPaymentRecordRequest2 =
    //             await _getPaymentRecordsFunctionality.SendGetPaymentRecordRequestAsync( _requestHeader!, _ipn2);
    //         Assert.That(jResponseGetPaymentRecordRequest2.totalRecords > 0);
    //         var jResponseGetPaymentRecordRequest3 =
    //             await _getPaymentRecordsFunctionality.SendGetPaymentRecordRequestAsync( _requestHeader!, _ipn3);
    //         Assert.That(jResponseGetPaymentRecordRequest3.totalRecords > 0);
    //         var jResponseGetPaymentRecordRequest4 =
    //             await _getPaymentRecordsFunctionality.SendGetPaymentRecordRequestAsync( _requestHeader!, _ipn4);
    //         Assert.That(jResponseGetPaymentRecordRequest4.totalRecords > 0);
    //         var jResponseGetPaymentRecordRequest5 =
    //             await _getPaymentRecordsFunctionality.SendGetPaymentRecordRequestAsync( _requestHeader!, _ipn5);
    //         Assert.That(jResponseGetPaymentRecordRequest5.totalRecords > 0);
    //         Console.WriteLine("Done TestValidate_DBS_FUN_STL_MON_FTR_CRC_F09_PartialRefund_OutstandingBalanceFirst_IncludePaymentsRecords");
    //     }
    //     catch (Exception exception)
    //     {
    //         Assert.Fail("Error in TestValidate_DBS_FUN_STL_MON_FTR_CRC_F09_PartialRefund_OutstandingBalanceFirst_IncludePaymentsRecords \n" +
    //                     exception + "\n");
    //     }
    // }
    //
    // [TestCase(Category = "FundingOperations")]
    // [Test(Description = "TestValidate_DBS_FUN_STL_MON_FTR_CRC_F09_PartialRefund_PastCharge with generated merchant"),
    //  CancelAfter(960 * 1000)]
    // public async Task TestValidate_DBS_FUN_STL_MON_FTR_CRC_F09_PartialRefund_PastCharge()
    // {
    //     try
    //     {
    //         Console.WriteLine("\nStarting TestValidate_DBS_FUN_STL_MON_FTR_CRC_F09_PartialRefund_PastCharge");
    //         var testName = TestContext.CurrentContext.Test.Name;
    //         (_terminalApiKey, _businessUnitId) = await _testsHelper.CreateMerchantFunctionality( _requestHeader!,
    //             testName,_db!,
    //             "PaymentWizard.v3.5", "NonSecuredPlan", "Monthly", _skuDict!, "109");
    //         Assert.That(_terminalApiKey, Is.Not.EqualTo(""));
    //         var jResponse = await _list.SendGetMerchantListRequestAsync( _requestHeader!, _businessUnitId);
    //         Console.WriteLine("Done with creation of a new merchant , settings and terminal process");
    //         var createPlanDefaultValues = new CreatePlanDefaultValues();
    //         var is3DsVarsCreation = new Is3DsVarsCreation();
    //         createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
    //         Console.WriteLine("Creating 5 plans and querying mongo");
    //         await Task.Delay(5 * 1000);
    //         (_flag, _ipn1) = await _testsHelper.CreatePlanAndQueryMongo(_terminalApiKey, _businessUnitId, _db!,
    //             _requestHeader!, createPlanDefaultValues, 7, 9,
    //             "PartialRefund", "FutureInstallmentsNotAllowed");
    //         Assert.That(_flag);
    //         await Task.Delay(5 * 1000);
    //         (_flag, _ipn2) = await _testsHelper.CreatePlanAndQueryMongo(_terminalApiKey, _businessUnitId, _db!,
    //             _requestHeader!, createPlanDefaultValues, 7, 9,
    //             "PartialRefund", "FutureInstallmentsNotAllowed");
    //         Assert.That(_flag);
    //         await Task.Delay(5 * 1000);
    //         (_flag, _ipn3) = await _testsHelper.CreatePlanAndQueryMongo(_terminalApiKey, _businessUnitId, _db!,
    //             _requestHeader!, createPlanDefaultValues, 7, 9,
    //             "PartialRefund", "FutureInstallmentsNotAllowed");
    //         Assert.That(_flag);
    //         await Task.Delay(5 * 1000);
    //         (_flag, _ipn4) = await _testsHelper.CreatePlanAndQueryMongo(_terminalApiKey, _businessUnitId, _db!,
    //             _requestHeader!, createPlanDefaultValues, 7, 9,
    //             "PartialRefund", "FutureInstallmentsNotAllowed");
    //         Assert.That(_flag);
    //         await Task.Delay(5 * 1000);
    //         (_flag, _ipn5) = await _testsHelper.CreatePlanAndQueryMongo(_terminalApiKey, _businessUnitId, _db!,
    //             _requestHeader!, createPlanDefaultValues, 7, 9,
    //             "PartialRefund", "FutureInstallmentsNotAllowed");
    //         Assert.That(_flag);
    //         Console.WriteLine("Done Creating 5 plans and querying mongo\n");
    //         var requestToGetPaymentFundingOperations =
    //             await _getPaymentFundingOperations.SendPostRequestToGetPaymentFundingOperationsAsync( _requestHeader!,
    //                 jResponse!.Merchants[0].Id.ToString(), 10, 1, "merchantName");
    //         Assert.That(
    //             requestToGetPaymentFundingOperations!.paymentFundingOperationResponses[0].activitiesList.Count == 2);
    //         Assert.That(requestToGetPaymentFundingOperations.paymentFundingOperationResponses[0]
    //             .collectNumberOfActivities == 1);
    //         Assert.That(requestToGetPaymentFundingOperations.paymentFundingOperationResponses[0].fundNumberOfActivities == 1);
    //         var jResponseGetPaymentRecordRequest1 =
    //             await _getPaymentRecordsFunctionality.SendGetPaymentRecordRequestAsync( _requestHeader!, _ipn1);
    //         Assert.That(jResponseGetPaymentRecordRequest1.totalRecords > 0);
    //         var jResponseGetPaymentRecordRequest2 =
    //             await _getPaymentRecordsFunctionality.SendGetPaymentRecordRequestAsync( _requestHeader!, _ipn2);
    //         Assert.That(jResponseGetPaymentRecordRequest2.totalRecords > 0);
    //         var jResponseGetPaymentRecordRequest3 =
    //             await _getPaymentRecordsFunctionality.SendGetPaymentRecordRequestAsync( _requestHeader!, _ipn3);
    //         Assert.That(jResponseGetPaymentRecordRequest3.totalRecords > 0);
    //         var jResponseGetPaymentRecordRequest4 =
    //             await _getPaymentRecordsFunctionality.SendGetPaymentRecordRequestAsync( _requestHeader!, _ipn4);
    //         Assert.That(jResponseGetPaymentRecordRequest4.totalRecords > 0);
    //         var jResponseGetPaymentRecordRequest5 =
    //             await _getPaymentRecordsFunctionality.SendGetPaymentRecordRequestAsync( _requestHeader!, _ipn5);
    //         Assert.That(jResponseGetPaymentRecordRequest5.totalRecords > 0);
    //         Console.WriteLine("Done TestValidate_DBS_FUN_STL_MON_FTR_CRC_F09_PartialRefund_PastCharge");
    //     }
    //     catch (Exception exception)
    //     {
    //         Assert.Fail("Error in TestValidate_DBS_FUN_STL_MON_FTR_CRC_F09_PartialRefund_PastCharge \n" + exception + "\n");
    //     }
    // }
    //
    // [TestCase(Category = "FundingOperations")]
    // [Test(Description = "TestValidate_DBS_FUN_STL_MON_FTR_CRC_F06_PartialRefund_PastCharge_IncludePaymentsRecords with generated merchant"),
    //  CancelAfter(960 * 1000)]
    // public async Task TestValidate_DBS_FUN_STL_MON_FTR_CRC_F06_PartialRefund_PastCharge_IncludePaymentsRecords()
    // {
    //     try
    //     {
    //         Console.WriteLine("\nStarting TestValidate_DBS_FUN_STL_MON_FTR_CRC_F06_PartialRefund_PastCharge_IncludePaymentsRecords");
    //         var testName = TestContext.CurrentContext.Test.Name;
    //         (_terminalApiKey, _businessUnitId) = await _testsHelper.CreateMerchantFunctionality( _requestHeader!,
    //             testName, _db!,"PaymentWizard.v3.5", "NonSecuredPlan", "Monthly", _skuDict!, "109");
    //         Assert.That(_terminalApiKey, Is.Not.EqualTo(""));
    //         var jResponse = await _list.SendGetMerchantListRequestAsync( _requestHeader!, _businessUnitId);
    //         Console.WriteLine("Done with creation of a new merchant , settings and terminal process");
    //         var createPlanDefaultValues = new CreatePlanDefaultValues();
    //         var is3DsVarsCreation = new Is3DsVarsCreation();
    //         createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
    //         Console.WriteLine("Creating 5 plans and querying mongo");
    //         await Task.Delay(5 * 1000);
    //         (_flag, _ipn1) = await _testsHelper.CreatePlanAndQueryMongoWithFailed(_terminalApiKey, _businessUnitId, _db!,
    //             _requestHeader!, createPlanDefaultValues, 4, 6, 
    //             "PartialRefund", "FutureInstallmentsNotAllowed");
    //         Assert.That(_flag);
    //         await Task.Delay(5 * 1000);
    //         (_flag, _ipn2) = await _testsHelper.CreatePlanAndQueryMongoWithFailed(_terminalApiKey, _businessUnitId, _db!,
    //             _requestHeader!, createPlanDefaultValues, 4, 6,
    //             "PartialRefund", "FutureInstallmentsNotAllowed");
    //         Assert.That(_flag);
    //         await Task.Delay(5 * 1000);
    //         (_flag, _ipn3) = await _testsHelper.CreatePlanAndQueryMongoWithFailed(_terminalApiKey, _businessUnitId, _db!,
    //             _requestHeader!, createPlanDefaultValues, 4, 6,
    //             "PartialRefund", "FutureInstallmentsNotAllowed");
    //         Assert.That(_flag);
    //         await Task.Delay(5 * 1000);
    //         (_flag, _ipn4) = await _testsHelper.CreatePlanAndQueryMongoWithFailed(_terminalApiKey, _businessUnitId, _db!,
    //             _requestHeader!, createPlanDefaultValues, 4, 6,
    //             "PartialRefund", "FutureInstallmentsNotAllowed");
    //         Assert.That(_flag);
    //         await Task.Delay(5 * 1000);
    //         (_flag, _ipn5) = await _testsHelper.CreatePlanAndQueryMongoWithFailed(_terminalApiKey, _businessUnitId, _db!,
    //             _requestHeader!, createPlanDefaultValues, 4, 6,
    //             "PartialRefund", "FutureInstallmentsNotAllowed");
    //         Assert.That(_flag);
    //         Console.WriteLine("Done Creating 5 plans and querying mongo\n");
    //         var requestToGetPaymentFundingOperations =
    //             await _getPaymentFundingOperations.SendPostRequestToGetPaymentFundingOperationsAsync( _requestHeader!,
    //                 jResponse!.Merchants[0].Id.ToString(), 10, 1, "merchantName");
    //         Assert.That(requestToGetPaymentFundingOperations!.paymentFundingOperationResponses[0].activitiesList.Count == 3);
    //         Assert.That(requestToGetPaymentFundingOperations.paymentFundingOperationResponses[0].collectNumberOfActivities == 10);
    //         Assert.That(requestToGetPaymentFundingOperations.paymentFundingOperationResponses[0].fundNumberOfActivities == 5);
    //         var jResponseGetPaymentRecordRequest1 =
    //             await _getPaymentRecordsFunctionality.SendGetPaymentRecordRequestAsync( _requestHeader!, _ipn1);
    //         Assert.That(jResponseGetPaymentRecordRequest1.totalRecords > 0);
    //         var jResponseGetPaymentRecordRequest2 =
    //             await _getPaymentRecordsFunctionality.SendGetPaymentRecordRequestAsync( _requestHeader!, _ipn2);
    //         Assert.That(jResponseGetPaymentRecordRequest2.totalRecords > 0);
    //         var jResponseGetPaymentRecordRequest3 =
    //             await _getPaymentRecordsFunctionality.SendGetPaymentRecordRequestAsync( _requestHeader!, _ipn3);
    //         Assert.That(jResponseGetPaymentRecordRequest3.totalRecords > 0);
    //         var jResponseGetPaymentRecordRequest4 =
    //             await _getPaymentRecordsFunctionality.SendGetPaymentRecordRequestAsync( _requestHeader!, _ipn4);
    //         Assert.That(jResponseGetPaymentRecordRequest4.totalRecords > 0);
    //         var jResponseGetPaymentRecordRequest5 =
    //             await _getPaymentRecordsFunctionality.SendGetPaymentRecordRequestAsync( _requestHeader!, _ipn5);
    //         Assert.That(jResponseGetPaymentRecordRequest5.totalRecords > 0);
    //         Console.WriteLine("Done TestValidate_DBS_FUN_STL_MON_FTR_CRC_F06_PartialRefund_PastCharge_IncludePaymentsRecords");
    //     }
    //     catch (Exception exception)
    //     {
    //         Assert.Fail("Error in TestValidate_DBS_FUN_STL_MON_FTR_CRC_F06_PartialRefund_PastCharge_IncludePaymentsRecords \n" + exception + "\n");
    //     }
    // }
    //
    // [TestCase(Category = "FundingOperations")]
    // [Test(Description = "TestValidate_DBS_FUN_STL_MON_FTR_CRC_F09_PartialRefund_OutstandingAmount_IncludePaymentsRecords with generated merchant"),
    //  CancelAfter(960 * 1000)]
    // public async Task TestValidate_DBS_FUN_STL_MON_FTR_CRC_F09_PartialRefund_OutstandingAmount_IncludePaymentsRecords()
    // {
    //     try
    //     {
    //         Console.WriteLine("\nStarting TestValidate_DBS_FUN_STL_MON_FTR_CRC_F09_PartialRefund_OutstandingAmount_IncludePaymentsRecords");
    //         var testName = TestContext.CurrentContext.Test.Name;
    //         (_terminalApiKey, _businessUnitId) = await _testsHelper.CreateMerchantFunctionality( _requestHeader!,
    //             testName, _db!,"PaymentWizard.v3.5", "NonSecuredPlan", "Monthly", _skuDict!, "109");
    //         Assert.That(_terminalApiKey, Is.Not.EqualTo(""));
    //         var jResponse = await _list.SendGetMerchantListRequestAsync( _requestHeader!, _businessUnitId);
    //         Console.WriteLine("Done with creation of a new merchant , settings and terminal process");
    //         var createPlanDefaultValues = new CreatePlanDefaultValues();
    //         var is3DsVarsCreation = new Is3DsVarsCreation();
    //         createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
    //         Console.WriteLine("Creating 5 plans and querying mongo");
    //         await Task.Delay(5 * 1000);
    //         (_flag, _ipn1) = await _testsHelper.CreatePlanAndQueryMongoWithFailedActivities(_terminalApiKey, _businessUnitId,_db!,
    //              _requestHeader!, createPlanDefaultValues, 7, 9,
    //             "PartialRefund", null!);
    //         Assert.That(_flag);
    //         await Task.Delay(5 * 1000);
    //         (_flag, _ipn2) = await _testsHelper.CreatePlanAndQueryMongoWithFailedActivities(_terminalApiKey, _businessUnitId,_db!,
    //              _requestHeader!, createPlanDefaultValues, 7, 9,
    //             "PartialRefund", null!);
    //         Assert.That(_flag);
    //         await Task.Delay(5 * 1000);
    //         (_flag, _ipn3) = await _testsHelper.CreatePlanAndQueryMongoWithFailedActivities(_terminalApiKey, _businessUnitId,_db!,
    //              _requestHeader!, createPlanDefaultValues, 7, 9,
    //             "PartialRefund", null!);
    //         Assert.That(_flag);
    //         await Task.Delay(5 * 1000);
    //         (_flag, _ipn4) = await _testsHelper.CreatePlanAndQueryMongoWithFailedActivities(_terminalApiKey, _businessUnitId,_db!,
    //              _requestHeader!, createPlanDefaultValues, 7, 9,
    //             "PartialRefund", null!);
    //         Assert.That(_flag);
    //         await Task.Delay(5 * 1000);
    //         (_flag, _ipn5) = await _testsHelper.CreatePlanAndQueryMongoWithFailedActivities(_terminalApiKey, _businessUnitId,_db!,
    //              _requestHeader!, createPlanDefaultValues, 7, 9,
    //             "PartialRefund", null!);
    //         Assert.That(_flag);
    //         Console.WriteLine("Done Creating 5 plans and querying mongo\n");
    //         var requestToGetPaymentFundingOperations = await _getPaymentFundingOperations.SendPostRequestToGetPaymentFundingOperationsAsync( _requestHeader!,
    //                 jResponse!.Merchants[0].Id.ToString(), 10, 1, "merchantName");
    //         Assert.That(requestToGetPaymentFundingOperations!.paymentFundingOperationResponses[0].activitiesList.Count == 2);
    //         Assert.That(requestToGetPaymentFundingOperations.paymentFundingOperationResponses[0].collectNumberOfActivities == 5);
    //         Assert.That(requestToGetPaymentFundingOperations.paymentFundingOperationResponses[0].fundNumberOfActivities == 5);
    //         var jResponseGetFailedPaymentOperations1 = await _getFailedPaymentOperations.SendPostRequestToGetFailedPaymentOperationsAsync( _requestHeader!,
    //                 jResponse.Merchants[0].Id.ToString(), 10, 1, true, _ipn1);
    //         Assert.That(jResponseGetFailedPaymentOperations1!.totalRecords == 0);
    //         var jResponseGetFailedPaymentOperations2 = await _getFailedPaymentOperations.SendPostRequestToGetFailedPaymentOperationsAsync( _requestHeader!,
    //                 jResponse.Merchants[0].Id.ToString(), 10, 1, true, _ipn2);
    //         Assert.That(jResponseGetFailedPaymentOperations2!.totalRecords == 0);
    //         var jResponseGetFailedPaymentOperations3 = await _getFailedPaymentOperations.SendPostRequestToGetFailedPaymentOperationsAsync( _requestHeader!,
    //                 jResponse.Merchants[0].Id.ToString(), 10, 1, true, _ipn3);
    //         Assert.That(jResponseGetFailedPaymentOperations3!.totalRecords == 0);
    //         var jResponseGetFailedPaymentOperations4 = await _getFailedPaymentOperations.SendPostRequestToGetFailedPaymentOperationsAsync( _requestHeader!,
    //                 jResponse.Merchants[0].Id.ToString(), 10, 1, true, _ipn4);
    //         Assert.That(jResponseGetFailedPaymentOperations4!.totalRecords == 0);
    //         var jResponseGetFailedPaymentOperations5 = await _getFailedPaymentOperations.SendPostRequestToGetFailedPaymentOperationsAsync( _requestHeader!,
    //                 jResponse.Merchants[0].Id.ToString(), 10, 1, true, _ipn5);
    //         Assert.That(jResponseGetFailedPaymentOperations5!.totalRecords == 0);
    //         var jResponseGetPaymentRecordRequest1 =
    //             await _getPaymentRecordsFunctionality.SendGetPaymentRecordRequestAsync( _requestHeader!, _ipn1);
    //         Assert.That(jResponseGetPaymentRecordRequest1.totalRecords > 0);
    //         var jResponseGetPaymentRecordRequest2 =
    //             await _getPaymentRecordsFunctionality.SendGetPaymentRecordRequestAsync( _requestHeader!, _ipn2);
    //         Assert.That(jResponseGetPaymentRecordRequest2.totalRecords > 0);
    //         var jResponseGetPaymentRecordRequest3 =
    //             await _getPaymentRecordsFunctionality.SendGetPaymentRecordRequestAsync( _requestHeader!, _ipn3);
    //         Assert.That(jResponseGetPaymentRecordRequest3.totalRecords > 0);
    //         var jResponseGetPaymentRecordRequest4 =
    //             await _getPaymentRecordsFunctionality.SendGetPaymentRecordRequestAsync( _requestHeader!, _ipn4);
    //         Assert.That(jResponseGetPaymentRecordRequest4.totalRecords > 0);
    //         var jResponseGetPaymentRecordRequest5 =
    //             await _getPaymentRecordsFunctionality.SendGetPaymentRecordRequestAsync( _requestHeader!, _ipn5);
    //         Assert.That(jResponseGetPaymentRecordRequest5.totalRecords > 0);
    //
    //         Console.WriteLine("Done TestValidate_DBS_FUN_STL_MON_FTR_CRC_F09_PartialRefund_OutstandingAmount_IncludePaymentsRecords");
    //     }
    //     catch (Exception exception)
    //     {
    //         Assert.Fail("Error in TestValidate_DBS_FUN_STL_MON_FTR_CRC_F09_PartialRefund_OutstandingAmount_IncludePaymentsRecords \n" + exception + "\n");
    //     }
    // }
    //
    // [TestCase(Category = "FundingOperations")]
    // [Test(Description = "TestValidate_DBS_FUN_STL_MON_FTR_CRC_F09_PartialRefund_PastCharge_IncludePaymentsRecords with generated merchant"),
    //  CancelAfter(960 * 1000)]
    // public async Task TestValidate_DBS_FUN_STL_MON_FTR_CRC_F09_PartialRefund_PastCharge_IncludePaymentsRecords()
    // {
    //     try
    //     {
    //         Console.WriteLine(
    //             "\nStarting TestValidate_DBS_FUN_STL_MON_FTR_CRC_F09_PartialRefund_PastCharge_IncludePaymentsRecords");
    //         var testName = TestContext.CurrentContext.Test.Name;
    //         (_terminalApiKey, _businessUnitId) = await _testsHelper.CreateMerchantFunctionality( _requestHeader!,
    //             testName, _db!,"PaymentWizard.v3.5", "NonSecuredPlan", "Monthly", _skuDict!, "109");
    //         Assert.That(_terminalApiKey, Is.Not.EqualTo(""));
    //         var jResponse = await _list.SendGetMerchantListRequestAsync( _requestHeader!, _businessUnitId);
    //         Console.WriteLine("Done with creation of a new merchant , settings and terminal process");
    //         var createPlanDefaultValues = new CreatePlanDefaultValues();
    //         var is3DsVarsCreation = new Is3DsVarsCreation();
    //         createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
    //         Console.WriteLine("Creating 5 plans and querying mongo");
    //         await Task.Delay(5 * 1000);
    //         (_flag, _ipn1) = await _testsHelper.CreatePlanAndQueryMongoWithFailedActivities(_terminalApiKey, _businessUnitId,_db!,
    //              _requestHeader!, createPlanDefaultValues, 7, 9,
    //             "PartialRefund", "FutureInstallmentsNotAllowed");
    //         Assert.That(_flag);
    //         await Task.Delay(5 * 1000);
    //         (_flag, _ipn2) = await _testsHelper.CreatePlanAndQueryMongoWithFailedActivities(_terminalApiKey, _businessUnitId,_db!,
    //              _requestHeader!, createPlanDefaultValues, 7, 9,
    //             "PartialRefund", "FutureInstallmentsNotAllowed");
    //         Assert.That(_flag);
    //         await Task.Delay(5 * 1000);
    //         (_flag, _ipn3) = await _testsHelper.CreatePlanAndQueryMongoWithFailedActivities(_terminalApiKey, _businessUnitId,_db!,
    //              _requestHeader!, createPlanDefaultValues, 7, 9,
    //             "PartialRefund", "FutureInstallmentsNotAllowed");
    //         Assert.That(_flag);
    //         await Task.Delay(5 * 1000);
    //         (_flag, _ipn4) = await _testsHelper.CreatePlanAndQueryMongoWithFailedActivities(_terminalApiKey, _businessUnitId,_db!,
    //              _requestHeader!, createPlanDefaultValues, 7, 9,
    //             "PartialRefund", "FutureInstallmentsNotAllowed");
    //         Assert.That(_flag);
    //         await Task.Delay(5 * 1000);
    //         (_flag, _ipn5) = await _testsHelper.CreatePlanAndQueryMongoWithFailedActivities(_terminalApiKey, _businessUnitId,_db!,
    //              _requestHeader!, createPlanDefaultValues, 7, 9,
    //             "PartialRefund", "FutureInstallmentsNotAllowed");
    //         Assert.That(_flag);
    //         Console.WriteLine("Done Creating 5 plans and querying mongo\n");
    //         var requestToGetPaymentFundingOperations = await _getPaymentFundingOperations.SendPostRequestToGetPaymentFundingOperationsAsync( _requestHeader!,
    //                 jResponse!.Merchants[0].Id.ToString(), 10, 1, "merchantName");
    //         Assert.That(requestToGetPaymentFundingOperations!.paymentFundingOperationResponses[0].activitiesList.Count == 3);
    //         Assert.That(requestToGetPaymentFundingOperations.paymentFundingOperationResponses[0].collectNumberOfActivities == 10);
    //         Assert.That(requestToGetPaymentFundingOperations.paymentFundingOperationResponses[0].fundNumberOfActivities == 5);
    //         var jResponseGetFailedPaymentOperations1 =
    //             await _getFailedPaymentOperations.SendPostRequestToGetFailedPaymentOperationsAsync( _requestHeader!,
    //                 jResponse.Merchants[0].Id.ToString(), 10, 1, true, _ipn1);
    //         Assert.That(jResponseGetFailedPaymentOperations1!.totalRecords > 0);
    //         var jResponseGetFailedPaymentOperations2 =
    //             await _getFailedPaymentOperations.SendPostRequestToGetFailedPaymentOperationsAsync( _requestHeader!,
    //                 jResponse.Merchants[0].Id.ToString(), 10, 1, true, _ipn2);
    //         Assert.That(jResponseGetFailedPaymentOperations2!.totalRecords > 0);
    //         var jResponseGetFailedPaymentOperations3 =
    //             await _getFailedPaymentOperations.SendPostRequestToGetFailedPaymentOperationsAsync( _requestHeader!,
    //                 jResponse.Merchants[0].Id.ToString(), 10, 1, true, _ipn3);
    //         Assert.That(jResponseGetFailedPaymentOperations3!.totalRecords > 0);
    //         var jResponseGetFailedPaymentOperations4 =
    //             await _getFailedPaymentOperations.SendPostRequestToGetFailedPaymentOperationsAsync( _requestHeader!,
    //                 jResponse.Merchants[0].Id.ToString(), 10, 1, true, _ipn4);
    //         Assert.That(jResponseGetFailedPaymentOperations4!.totalRecords > 0);
    //         var jResponseGetFailedPaymentOperations5 =
    //             await _getFailedPaymentOperations.SendPostRequestToGetFailedPaymentOperationsAsync( _requestHeader!,
    //                 jResponse.Merchants[0].Id.ToString(), 10, 1, true, _ipn5);
    //         Assert.That(jResponseGetFailedPaymentOperations5!.totalRecords > 0);
    //         var jResponseGetPaymentRecordRequest1 =
    //             await _getPaymentRecordsFunctionality.SendGetPaymentRecordRequestAsync( _requestHeader!, _ipn1);
    //         Assert.That(jResponseGetPaymentRecordRequest1.totalRecords > 0);
    //         var jResponseGetPaymentRecordRequest2 =
    //             await _getPaymentRecordsFunctionality.SendGetPaymentRecordRequestAsync( _requestHeader!, _ipn2);
    //         Assert.That(jResponseGetPaymentRecordRequest2.totalRecords > 0);
    //         var jResponseGetPaymentRecordRequest3 =
    //             await _getPaymentRecordsFunctionality.SendGetPaymentRecordRequestAsync( _requestHeader!, _ipn3);
    //         Assert.That(jResponseGetPaymentRecordRequest3.totalRecords > 0);
    //         var jResponseGetPaymentRecordRequest4 =
    //             await _getPaymentRecordsFunctionality.SendGetPaymentRecordRequestAsync( _requestHeader!, _ipn4);
    //         Assert.That(jResponseGetPaymentRecordRequest4.totalRecords > 0);
    //         var jResponseGetPaymentRecordRequest5 =
    //             await _getPaymentRecordsFunctionality.SendGetPaymentRecordRequestAsync( _requestHeader!, _ipn5);
    //         Assert.That(jResponseGetPaymentRecordRequest5.totalRecords > 0);
    //         Console.WriteLine("Done TestValidate_DBS_FUN_STL_MON_FTR_CRC_F09_PartialRefund_PastCharge_IncludePaymentsRecords");
    //     }
    //     catch (Exception exception)
    //     {
    //         Assert.Fail(
    //             "Error in TestValidate_DBS_FUN_STL_MON_FTR_CRC_F09_PartialRefund_PastCharge_IncludePaymentsRecords \n" +
    //             exception + "\n");
    //     }
    // }
    //
    // [TestCase(Category = "FundingOperations")]
    // [Test(Description = "TestValidate_PCO_FUN_STL_MON_FTR_CRC_F12_PartialRefund_OutstandingAmount_IncludePaymentsRecords with generated merchant"),
    //  CancelAfter(960 * 1000)]
    // public async Task TestValidate_PCO_FUN_STL_MON_FTR_CRC_F12_PartialRefund_OutstandingAmount_IncludePaymentsRecords()
    // {
    //     try
    //     {
    //         Console.WriteLine(
    //             "\nStarting TestValidate_PCO_FUN_STL_MON_FTR_CRC_F12_PartialRefund_OutstandingAmount_IncludePaymentsRecords");
    //         var testName = TestContext.CurrentContext.Test.Name;
    //         (_terminalApiKey, _businessUnitId) = await _testsHelper.CreateMerchantFunctionality( _requestHeader!,
    //             testName,_db!,
    //             "PaymentWizard.v3.5", "NonSecuredPlan", "Monthly", _skuDict!, "315");
    //         Assert.That(_terminalApiKey, Is.Not.EqualTo(""));
    //         var jResponse = await _list.SendGetMerchantListRequestAsync( _requestHeader!, _businessUnitId);
    //         Console.WriteLine("Done with creation of a new merchant , settings and terminal process");
    //         var createPlanDefaultValues = new CreatePlanDefaultValues();
    //         var is3DsVarsCreation = new Is3DsVarsCreation();
    //         createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
    //         Console.WriteLine("Creating 5 plans and querying mongo");
    //         await Task.Delay(5 * 1000);
    //         (_flag, _ipn1) = await _testsHelper.CreatePlanAndQueryMongoProcessingFailedAllActivities(_terminalApiKey,
    //             _businessUnitId, _db!, _requestHeader!, createPlanDefaultValues, 10, 12,
    //             "PartialRefund", null!);
    //         Assert.That(_flag);
    //         await Task.Delay(5 * 1000);
    //         (_flag, _ipn2) = await _testsHelper.CreatePlanAndQueryMongoProcessingFailedAllActivities(_terminalApiKey,
    //             _businessUnitId, _db!, _requestHeader!, createPlanDefaultValues, 10, 12,
    //             "PartialRefund", null!);
    //         Assert.That(_flag);
    //         await Task.Delay(5 * 1000);
    //         (_flag, _ipn3) = await _testsHelper.CreatePlanAndQueryMongoProcessingFailedAllActivities(_terminalApiKey,
    //             _businessUnitId, _db!, _requestHeader!, createPlanDefaultValues, 10, 12,
    //             "PartialRefund", null!);
    //         Assert.That(_flag);
    //         await Task.Delay(5 * 1000);
    //         (_flag, _ipn4) = await _testsHelper.CreatePlanAndQueryMongoProcessingFailedAllActivities(_terminalApiKey,
    //             _businessUnitId, _db!, _requestHeader!, createPlanDefaultValues, 10, 12,
    //             "PartialRefund", null!);
    //         Assert.That(_flag);
    //         await Task.Delay(5 * 1000);
    //         (_flag, _ipn5) = await _testsHelper.CreatePlanAndQueryMongoProcessingFailedAllActivities(_terminalApiKey,
    //             _businessUnitId, _db!, _requestHeader!, createPlanDefaultValues, 10, 12,
    //             "PartialRefund", null!);
    //         Assert.That(_flag);
    //         Console.WriteLine("Done Creating 5 plans and querying mongo\n");
    //         var requestToGetPaymentFundingOperations =
    //             await _getPaymentFundingOperations.SendPostRequestToGetPaymentFundingOperationsAsync( _requestHeader!,
    //                 jResponse!.Merchants[0].Id.ToString(), 10, 1, "merchantName");
    //         Assert.That(requestToGetPaymentFundingOperations!.paymentFundingOperationResponses[0].activitiesList.Count == 2);
    //         Assert.That(requestToGetPaymentFundingOperations.paymentFundingOperationResponses[0].collectNumberOfActivities == 1);
    //         Assert.That(requestToGetPaymentFundingOperations.paymentFundingOperationResponses[0].fundNumberOfActivities == 1);
    //         var jResponseGetFailedPaymentOperations1 =
    //             await _getFailedPaymentOperations.SendPostRequestToGetFailedPaymentOperationsAsync( _requestHeader!,
    //                 jResponse.Merchants[0].Id.ToString(), 10, 1, true, _ipn1);
    //         Assert.That(jResponseGetFailedPaymentOperations1!.totalRecords > 0);
    //         var jResponseGetFailedPaymentOperations2 =
    //             await _getFailedPaymentOperations.SendPostRequestToGetFailedPaymentOperationsAsync( _requestHeader!,
    //                 jResponse.Merchants[0].Id.ToString(), 10, 1, true, _ipn2);
    //         Assert.That(jResponseGetFailedPaymentOperations2!.totalRecords > 0);
    //         var jResponseGetFailedPaymentOperations3 =
    //             await _getFailedPaymentOperations.SendPostRequestToGetFailedPaymentOperationsAsync( _requestHeader!,
    //                 jResponse.Merchants[0].Id.ToString(), 10, 1, true, _ipn3);
    //         Assert.That(jResponseGetFailedPaymentOperations3!.totalRecords > 0);
    //         var jResponseGetFailedPaymentOperations4 =
    //             await _getFailedPaymentOperations.SendPostRequestToGetFailedPaymentOperationsAsync( _requestHeader!,
    //                 jResponse.Merchants[0].Id.ToString(), 10, 1, true, _ipn4);
    //         Assert.That(jResponseGetFailedPaymentOperations4!.totalRecords > 0);
    //         var jResponseGetFailedPaymentOperations5 =
    //             await _getFailedPaymentOperations.SendPostRequestToGetFailedPaymentOperationsAsync( _requestHeader!,
    //                 jResponse.Merchants[0].Id.ToString(), 10, 1, true, _ipn5);
    //         Assert.That(jResponseGetFailedPaymentOperations5!.totalRecords > 0);
    //         var jResponseGetPaymentRecordRequest1 =
    //             await _getPaymentRecordsFunctionality.SendGetPaymentRecordRequestAsync( _requestHeader!, _ipn1);
    //         Assert.That(jResponseGetPaymentRecordRequest1.totalRecords > 0);
    //         var jResponseGetPaymentRecordRequest2 =
    //             await _getPaymentRecordsFunctionality.SendGetPaymentRecordRequestAsync( _requestHeader!, _ipn2);
    //         Assert.That(jResponseGetPaymentRecordRequest2.totalRecords > 0);
    //         var jResponseGetPaymentRecordRequest3 =
    //             await _getPaymentRecordsFunctionality.SendGetPaymentRecordRequestAsync( _requestHeader!, _ipn3);
    //         Assert.That(jResponseGetPaymentRecordRequest3.totalRecords > 0);
    //         var jResponseGetPaymentRecordRequest4 =
    //             await _getPaymentRecordsFunctionality.SendGetPaymentRecordRequestAsync( _requestHeader!, _ipn4);
    //         Assert.That(jResponseGetPaymentRecordRequest4.totalRecords > 0);
    //         var jResponseGetPaymentRecordRequest5 =
    //             await _getPaymentRecordsFunctionality.SendGetPaymentRecordRequestAsync( _requestHeader!, _ipn5);
    //         Assert.That(jResponseGetPaymentRecordRequest5.totalRecords > 0);
    //         Console.WriteLine("Done TestValidate_PCO_FUN_STL_MON_FTR_CRC_F12_PartialRefund_OutstandingAmount_IncludePaymentsRecords");
    //     }
    //     catch (Exception exception)
    //     {
    //         Assert.Fail(
    //             "Error in TestValidate_PCO_FUN_STL_MON_FTR_CRC_F12_PartialRefund_OutstandingAmount_IncludePaymentsRecords \n" +
    //             exception + "\n");
    //     }
    // }
    //
    // [TestCase(Category = "FundingOperations")]
    // [Test(Description =
    //      "TestValidate_PCO_FUN_STL_MON_FTR_CRC_F12_PartialRefund_PastCharge_IncludePaymentsRecords with generated merchant"),
    //  CancelAfter(960 * 1000)]
    // public async Task TestValidate_PCO_FUN_STL_MON_FTR_CRC_F12_PartialRefund_PastCharge_IncludePaymentsRecords()
    // {
    //     try
    //     {
    //         Console.WriteLine(
    //             "\nStarting TestValidate_PCO_FUN_STL_MON_FTR_CRC_F12_PartialRefund_PastCharge_IncludePaymentsRecords");
    //         var testName = TestContext.CurrentContext.Test.Name;
    //         (_terminalApiKey, _businessUnitId) = await _testsHelper.CreateMerchantFunctionality( _requestHeader!,
    //             testName,
    //             _db!,"PaymentWizard.v3.5", "NonSecuredPlan", "Monthly", _skuDict!, "315");
    //         Assert.That(_terminalApiKey, Is.Not.EqualTo(""));
    //         var jResponse = await _list.SendGetMerchantListRequestAsync( _requestHeader!, _businessUnitId);
    //         Console.WriteLine("Done with creation of a new merchant , settings and terminal process");
    //         var createPlanDefaultValues = new CreatePlanDefaultValues();
    //         var is3DsVarsCreation = new Is3DsVarsCreation();
    //         createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
    //         Console.WriteLine("Creating 5 plans and querying mongo");
    //         await Task.Delay(5 * 1000);
    //         (_flag, _ipn1) = await _testsHelper.CreatePlanAndQueryMongoProcessingFailedAllActivities(_terminalApiKey,
    //             _businessUnitId, _db!, _requestHeader!, createPlanDefaultValues, 10, 12,
    //             "PartialRefund", "FutureInstallmentsNotAllowed");
    //         Assert.That(_flag);
    //         await Task.Delay(5 * 1000);
    //         (_flag, _ipn2) = await _testsHelper.CreatePlanAndQueryMongoProcessingFailedAllActivities(_terminalApiKey,
    //             _businessUnitId, _db!, _requestHeader!, createPlanDefaultValues, 10, 12,
    //             "PartialRefund", "FutureInstallmentsNotAllowed");
    //         Assert.That(_flag);
    //         await Task.Delay(5 * 1000);
    //         (_flag, _ipn3) = await _testsHelper.CreatePlanAndQueryMongoProcessingFailedAllActivities(_terminalApiKey,
    //             _businessUnitId, _db!, _requestHeader!, createPlanDefaultValues, 10, 12,
    //             "PartialRefund", "FutureInstallmentsNotAllowed");
    //         Assert.That(_flag);
    //         await Task.Delay(5 * 1000);
    //         (_flag, _ipn4) = await _testsHelper.CreatePlanAndQueryMongoProcessingFailedAllActivities(_terminalApiKey,
    //             _businessUnitId, _db!, _requestHeader!, createPlanDefaultValues, 10, 12,
    //             "PartialRefund", "FutureInstallmentsNotAllowed");
    //         Assert.That(_flag);
    //         await Task.Delay(5 * 1000);
    //         (_flag, _ipn5) = await _testsHelper.CreatePlanAndQueryMongoProcessingFailedAllActivities(_terminalApiKey,
    //             _businessUnitId, _db!, _requestHeader!, createPlanDefaultValues, 10, 12,
    //             "PartialRefund", "FutureInstallmentsNotAllowed");
    //         Assert.That(_flag);
    //         Console.WriteLine("Done Creating 5 plans and querying mongo\n");
    //         var requestToGetPaymentFundingOperations =
    //             await _getPaymentFundingOperations.SendPostRequestToGetPaymentFundingOperationsAsync( _requestHeader!,
    //                 jResponse!.Merchants[0].Id.ToString(), 10, 1, "merchantName");
    //         Assert.That(
    //             requestToGetPaymentFundingOperations!.paymentFundingOperationResponses[0].activitiesList.Count == 2);
    //         Assert.That(requestToGetPaymentFundingOperations.paymentFundingOperationResponses[0]
    //             .collectNumberOfActivities == 1);
    //         Assert.That(
    //             requestToGetPaymentFundingOperations.paymentFundingOperationResponses[0].fundNumberOfActivities == 1);
    //         var jResponseGetFailedPaymentOperations1 = 
    //             await _getFailedPaymentOperations.SendPostRequestToGetFailedPaymentOperationsAsync( _requestHeader!,
    //                 jResponse.Merchants[0].Id.ToString(), 10, 1, true, _ipn1);
    //         Assert.That(jResponseGetFailedPaymentOperations1!.totalRecords == 0);
    //         var jResponseGetFailedPaymentOperations2 =
    //             await _getFailedPaymentOperations.SendPostRequestToGetFailedPaymentOperationsAsync( _requestHeader!,
    //                 jResponse.Merchants[0].Id.ToString(), 10, 1, true, _ipn2);
    //         Assert.That(jResponseGetFailedPaymentOperations2!.totalRecords == 0);
    //         var jResponseGetFailedPaymentOperations3 =
    //             await _getFailedPaymentOperations.SendPostRequestToGetFailedPaymentOperationsAsync( _requestHeader!,
    //                 jResponse.Merchants[0].Id.ToString(), 10, 1, true, _ipn3);
    //         Assert.That(jResponseGetFailedPaymentOperations3!.totalRecords == 0);
    //         var jResponseGetFailedPaymentOperations4 =
    //             await _getFailedPaymentOperations.SendPostRequestToGetFailedPaymentOperationsAsync( _requestHeader!,
    //                 jResponse.Merchants[0].Id.ToString(), 10, 1, true, _ipn4);
    //         Assert.That(jResponseGetFailedPaymentOperations4!.totalRecords == 0);
    //         var jResponseGetFailedPaymentOperations5 =
    //             await _getFailedPaymentOperations.SendPostRequestToGetFailedPaymentOperationsAsync( _requestHeader!,
    //                 jResponse.Merchants[0].Id.ToString(), 10, 1, true, _ipn5);
    //         Assert.That(jResponseGetFailedPaymentOperations5!.totalRecords == 0);
    //         Console.WriteLine("Done TestValidate_PCO_FUN_STL_MON_FTR_CRC_F12_PartialRefund_PastCharge_IncludePaymentsRecords");
    //     }
    //     catch (Exception exception)
    //     {
    //         Assert.Fail("Error in TestValidate_PCO_FUN_STL_MON_FTR_CRC_F12_PartialRefund_PastCharge_IncludePaymentsRecords \n" +
    //                     exception + "\n");
    //     }
    // }
    //
    // [TestCase(Category = "FundingOperations")]
    // [Test(Description = "TestValidate_DPL_FUN_STL_MON_FTR_CRC_F06_PartialRefund_OutstandingBalanceFirst_IncludePaymentsRecords with generated merchant"),
    //  CancelAfter(960 * 1000)]
    // public async Task TestValidate_DPL_FUN_STL_MON_FTR_CRC_F06_PartialRefund_OutstandingBalanceFirst_IncludePaymentsRecords()
    // {
    //     try
    //     {
    //         Console.WriteLine("\nStarting TestValidate_DPL_FUN_STL_MON_FTR_CRC_F06_PartialRefund_OutstandingBalanceFirst_IncludePaymentsRecords");
    //         var testName = TestContext.CurrentContext.Test.Name;
    //         (_terminalApiKey, _businessUnitId) = await _testsHelper.CreateMerchantFunctionality( _requestHeader!,
    //             testName, _db!,"PaymentWizard.v3.5", "NonSecuredPlan", "Monthly", _skuDict!, "315");
    //         Assert.That(_terminalApiKey, Is.Not.EqualTo(""));
    //         var jResponse = await _list.SendGetMerchantListRequestAsync( _requestHeader!, _businessUnitId);
    //         Console.WriteLine("Done with creation of a new merchant , settings and terminal process");
    //         var createPlanDefaultValues = new CreatePlanDefaultValues();
    //         var is3DsVarsCreation = new Is3DsVarsCreation();
    //         createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
    //         Console.WriteLine("Creating 5 plans and querying mongo");
    //         await Task.Delay(5 * 1000);
    //         (_flag, _ipn1) = await _testsHelper.CreatePlanAndQueryMongoPendingBatchToBatch(_terminalApiKey, _businessUnitId,_db!,
    //              _requestHeader!, createPlanDefaultValues, 4, 6,
    //             "PartialRefund", null!);
    //         Assert.That(_flag);
    //         await Task.Delay(5 * 1000);
    //         (_flag, _ipn2) = await _testsHelper.CreatePlanAndQueryMongoPendingBatchToBatch(_terminalApiKey, _businessUnitId,_db!,
    //              _requestHeader!, createPlanDefaultValues, 4, 6,
    //             "PartialRefund", null!);
    //         Assert.That(_flag);
    //         await Task.Delay(5 * 1000);
    //         (_flag, _ipn3) = await _testsHelper.CreatePlanAndQueryMongoPendingBatchToBatch(_terminalApiKey, _businessUnitId,_db!,
    //              _requestHeader!, createPlanDefaultValues, 4, 6,
    //             "PartialRefund", null!);
    //         Assert.That(_flag);
    //         await Task.Delay(5 * 1000);
    //         (_flag, _ipn4) = await _testsHelper.CreatePlanAndQueryMongoPendingBatchToBatch(_terminalApiKey, _businessUnitId,_db!,
    //              _requestHeader!, createPlanDefaultValues, 4, 6,
    //             "PartialRefund", null!);
    //         Assert.That(_flag);
    //         await Task.Delay(5 * 1000);
    //         (_flag, _ipn5) = await _testsHelper.CreatePlanAndQueryMongoPendingBatchToBatch(_terminalApiKey, _businessUnitId,_db!,
    //              _requestHeader!, createPlanDefaultValues, 4, 6,
    //             "PartialRefund", null!);
    //         Assert.That(_flag);
    //         Console.WriteLine("Done Creating 5 plans and querying mongo\n");
    //         var requestToGetPaymentFundingOperations =
    //             await _getPaymentFundingOperations.SendPostRequestToGetPaymentFundingOperationsAsync( _requestHeader!,
    //                 jResponse!.Merchants[0].Id.ToString(), 10, 1, "merchantName");
    //         Assert.That(requestToGetPaymentFundingOperations!.paymentFundingOperationResponses[0].activitiesList.Count == 2);
    //         Assert.That(requestToGetPaymentFundingOperations.paymentFundingOperationResponses[0].collectNumberOfActivities == 1);
    //         Assert.That(requestToGetPaymentFundingOperations.paymentFundingOperationResponses[0].fundNumberOfActivities == 1);
    //         var jResponseGetPaymentRecordRequest1 =
    //             await _getPaymentRecordsFunctionality.SendGetPaymentRecordRequestAsync( _requestHeader!, _ipn1);
    //         Assert.That(jResponseGetPaymentRecordRequest1.totalRecords > 0);
    //         var jResponseGetPaymentRecordRequest2 =
    //             await _getPaymentRecordsFunctionality.SendGetPaymentRecordRequestAsync( _requestHeader!, _ipn2);
    //         Assert.That(jResponseGetPaymentRecordRequest2.totalRecords > 0);
    //         var jResponseGetPaymentRecordRequest3 =
    //             await _getPaymentRecordsFunctionality.SendGetPaymentRecordRequestAsync( _requestHeader!, _ipn3);
    //         Assert.That(jResponseGetPaymentRecordRequest3.totalRecords > 0);
    //         var jResponseGetPaymentRecordRequest4 =
    //             await _getPaymentRecordsFunctionality.SendGetPaymentRecordRequestAsync( _requestHeader!, _ipn4);
    //         Assert.That(jResponseGetPaymentRecordRequest4.totalRecords > 0);
    //         var jResponseGetPaymentRecordRequest5 =
    //             await _getPaymentRecordsFunctionality.SendGetPaymentRecordRequestAsync( _requestHeader!, _ipn5);
    //         Assert.That(jResponseGetPaymentRecordRequest5.totalRecords > 0);
    //         Console.WriteLine("Done TestValidate_DPL_FUN_STL_MON_FTR_CRC_F06_PartialRefund_OutstandingBalanceFirst_IncludePaymentsRecords");
    //     }
    //     catch (Exception exception)
    //     {
    //         Assert.Fail("Error in TestValidate_DPL_FUN_STL_MON_FTR_CRC_F06_PartialRefund_OutstandingBalanceFirst_IncludePaymentsRecords \n" +
    //                     exception + "\n");
    //     }
    // }
    //
    // [TestCase(Category = "FundingOperations")]
    // [Test(Description = "TestValidate_DPL_FUN_STL_MON_FTR_CRC_F06_PartialRefund_PastCharge with generated merchant"), CancelAfter(960 * 1000)]
    // public async Task TestValidate_DPL_FUN_STL_MON_FTR_CRC_F06_PartialRefund_PastCharge()
    // {
    //     try
    //     {
    //         Console.WriteLine("\nStarting TestValidate_DPL_FUN_STL_MON_FTR_CRC_F06_PartialRefund_PastCharge");
    //         var testName = TestContext.CurrentContext.Test.Name;
    //         (_terminalApiKey, _businessUnitId) = await _testsHelper.CreateMerchantFunctionality( _requestHeader!,
    //             testName, _db!,"PaymentWizard.v3.5", "NonSecuredPlan", "Monthly", _skuDict!, "315");
    //         Assert.That(_terminalApiKey, Is.Not.EqualTo(""));
    //         var jResponse = await _list.SendGetMerchantListRequestAsync( _requestHeader!, _businessUnitId);
    //         Console.WriteLine("Done with creation of a new merchant , settings and terminal process");
    //         var createPlanDefaultValues = new CreatePlanDefaultValues();
    //         var is3DsVarsCreation = new Is3DsVarsCreation();
    //         createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
    //         Console.WriteLine("Creating 5 plans and querying mongo");
    //         await Task.Delay(5 * 1000);
    //         (_flag, _ipn1) = await _testsHelper.CreatePlanAndQueryMongoPendingBatchToBatch(_terminalApiKey, _businessUnitId,_db!,
    //              _requestHeader!, createPlanDefaultValues, 4, 6,
    //             "PartialRefund", "FutureInstallmentsNotAllowed");
    //         Assert.That(_flag);
    //         await Task.Delay(5 * 1000);
    //         (_flag, _ipn2) = await _testsHelper.CreatePlanAndQueryMongoPendingBatchToBatch(_terminalApiKey, _businessUnitId,_db!,
    //              _requestHeader!, createPlanDefaultValues, 4, 6,
    //             "PartialRefund", "FutureInstallmentsNotAllowed");
    //         Assert.That(_flag);
    //         await Task.Delay(5 * 1000);
    //         (_flag, _ipn3) = await _testsHelper.CreatePlanAndQueryMongoPendingBatchToBatch(_terminalApiKey, _businessUnitId,_db!,
    //              _requestHeader!, createPlanDefaultValues, 4, 6,
    //             "PartialRefund", "FutureInstallmentsNotAllowed");
    //         Assert.That(_flag);
    //         await Task.Delay(5 * 1000);
    //         (_flag, _ipn4) = await _testsHelper.CreatePlanAndQueryMongoPendingBatchToBatch(_terminalApiKey, _businessUnitId,_db!,
    //              _requestHeader!, createPlanDefaultValues, 4, 6,
    //             "PartialRefund", "FutureInstallmentsNotAllowed");
    //         Assert.That(_flag);
    //         await Task.Delay(5 * 1000);
    //         (_flag, _ipn5) = await _testsHelper.CreatePlanAndQueryMongoPendingBatchToBatch(_terminalApiKey, _businessUnitId,_db!,
    //              _requestHeader!, createPlanDefaultValues, 4, 6,
    //             "PartialRefund", "FutureInstallmentsNotAllowed");
    //         Assert.That(_flag);
    //         Console.WriteLine("Done Creating 5 plans and querying mongo\n");
    //         var requestToGetPaymentFundingOperations =
    //             await _getPaymentFundingOperations.SendPostRequestToGetPaymentFundingOperationsAsync( _requestHeader!,
    //                 jResponse!.Merchants[0].Id.ToString(), 10, 1, "merchantName");
    //         Assert.That(requestToGetPaymentFundingOperations!.paymentFundingOperationResponses[0].activitiesList.Count == 2);
    //         Assert.That(requestToGetPaymentFundingOperations.paymentFundingOperationResponses[0].collectNumberOfActivities == 1);
    //         Assert.That(requestToGetPaymentFundingOperations.paymentFundingOperationResponses[0].fundNumberOfActivities == 1);
    //         var jResponseGetPaymentRecordRequest1 =
    //             await _getPaymentRecordsFunctionality.SendGetPaymentRecordRequestAsync( _requestHeader!, _ipn1);
    //         Assert.That(jResponseGetPaymentRecordRequest1.totalRecords > 0);
    //         var jResponseGetPaymentRecordRequest2 =
    //             await _getPaymentRecordsFunctionality.SendGetPaymentRecordRequestAsync( _requestHeader!, _ipn2);
    //         Assert.That(jResponseGetPaymentRecordRequest2.totalRecords > 0);
    //         var jResponseGetPaymentRecordRequest3 =
    //             await _getPaymentRecordsFunctionality.SendGetPaymentRecordRequestAsync( _requestHeader!, _ipn3);
    //         Assert.That(jResponseGetPaymentRecordRequest3.totalRecords > 0);
    //         var jResponseGetPaymentRecordRequest4 =
    //             await _getPaymentRecordsFunctionality.SendGetPaymentRecordRequestAsync( _requestHeader!, _ipn4);
    //         Assert.That(jResponseGetPaymentRecordRequest4.totalRecords > 0);
    //         var jResponseGetPaymentRecordRequest5 =
    //             await _getPaymentRecordsFunctionality.SendGetPaymentRecordRequestAsync( _requestHeader!, _ipn5);
    //         Assert.That(jResponseGetPaymentRecordRequest5.totalRecords > 0);
    //         Console.WriteLine("Done TestValidate_DPL_FUN_STL_MON_FTR_CRC_F06_PartialRefund_PastCharge");
    //     }
    //     catch (Exception exception)
    //     {
    //         Assert.Fail("Error in TestValidate_DPL_FUN_STL_MON_FTR_CRC_F06_PartialRefund_PastCharge \n" + exception +
    //                     "\n");
    //     }
    // }
    //
    // [TestCase(Category = "FundingOperations")]
    // [Test(Description = "TestValidate_DBS_FUN_STL_MON_FTR_CRC_F06_PartialRefund_OutstandingBalanceFirst_AcknowledgedWithError with generated merchant"),
    //  CancelAfter(960 * 1000)]
    // public async Task TestValidate_DBS_FUN_STL_MON_FTR_CRC_F06_PartialRefund_OutstandingBalanceFirst_AcknowledgedWithError()
    // {
    //     try
    //     {
    //         Console.WriteLine("\nStarting TestValidate_DBS_FUN_STL_MON_FTR_CRC_F06_PartialRefund_OutstandingBalanceFirst_AcknowledgedWithError");
    //         var testName = TestContext.CurrentContext.Test.Name;
    //         (_terminalApiKey, _businessUnitId) = await _testsHelper.CreateMerchantFunctionality( _requestHeader!,
    //             testName, _db!,"PaymentWizard.v3.5", "NonSecuredPlan", "Monthly", _skuDict!, "315");
    //         Assert.That(_terminalApiKey, Is.Not.EqualTo(""));
    //         var jResponse = await _list.SendGetMerchantListRequestAsync( _requestHeader!, _businessUnitId);
    //         Console.WriteLine("Done with creation of a new merchant , settings and terminal process");
    //         var createPlanDefaultValues = new CreatePlanDefaultValues();
    //         var is3DsVarsCreation = new Is3DsVarsCreation();
    //         createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
    //         Console.WriteLine("Creating 5 plans and querying mongo");
    //         await Task.Delay(5 * 1000);
    //         (_flag, _ipn1) = await _testsHelper.CreatePlanAndQueryMongoAcknowledged(_terminalApiKey, _businessUnitId,_db!,
    //              _requestHeader!, createPlanDefaultValues, 4, 6,
    //             "PartialRefund", null!, "AcknowledgedWithError",
    //             false);
    //         Assert.That(_flag);
    //         await Task.Delay(5 * 1000);
    //         (_flag, _ipn2) = await _testsHelper.CreatePlanAndQueryMongoAcknowledged(_terminalApiKey, _businessUnitId,_db!,
    //              _requestHeader!, createPlanDefaultValues, 4, 6,
    //             "PartialRefund", null!, "AcknowledgedWithError",
    //             false);
    //         Assert.That(_flag);
    //         await Task.Delay(5 * 1000);
    //         (_flag, _ipn3) = await _testsHelper.CreatePlanAndQueryMongoAcknowledged(_terminalApiKey, _businessUnitId,_db!,
    //              _requestHeader!, createPlanDefaultValues, 4, 6,
    //             "PartialRefund", null!, "AcknowledgedWithError",
    //             false);
    //         Assert.That(_flag);
    //         await Task.Delay(5 * 1000);
    //         (_flag, _ipn4) = await _testsHelper.CreatePlanAndQueryMongoAcknowledged(_terminalApiKey, _businessUnitId,_db!,
    //              _requestHeader!, createPlanDefaultValues, 4, 6,
    //             "PartialRefund", null!, "AcknowledgedWithError",
    //             false);
    //         Assert.That(_flag);
    //         await Task.Delay(5 * 1000);
    //         (_flag, _ipn5) = await _testsHelper.CreatePlanAndQueryMongoAcknowledged(_terminalApiKey, _businessUnitId,_db!,
    //              _requestHeader!, createPlanDefaultValues, 4, 6,
    //             "PartialRefund", null!, "AcknowledgedWithError",
    //             false);
    //         Assert.That(_flag);
    //         Console.WriteLine("Done Creating 5 plans and querying mongo\n");
    //         var requestToGetPaymentFundingOperations =
    //             await _getPaymentFundingOperations.SendPostRequestToGetPaymentFundingOperationsAsync( _requestHeader!,
    //                 jResponse!.Merchants[0].Id.ToString(), 10, 1, "merchantName");
    //         Assert.That(requestToGetPaymentFundingOperations!.paymentFundingOperationResponses[0].activitiesList.Count == 2);
    //         Assert.That(requestToGetPaymentFundingOperations.paymentFundingOperationResponses[0].collectNumberOfActivities == 1);
    //         Assert.That(requestToGetPaymentFundingOperations.paymentFundingOperationResponses[0].fundNumberOfActivities == 1);
    //         var jResponseGetFailedPaymentOperations1 =
    //             await _getFailedPaymentOperations.SendPostRequestToGetFailedPaymentOperationsAsync( _requestHeader!,
    //                 jResponse.Merchants[0].Id.ToString(), 10, 1, true, _ipn1);
    //         Assert.That(jResponseGetFailedPaymentOperations1!.totalRecords == 0);
    //         var jResponseGetFailedPaymentOperations2 =
    //             await _getFailedPaymentOperations.SendPostRequestToGetFailedPaymentOperationsAsync( _requestHeader!,
    //                 jResponse.Merchants[0].Id.ToString(), 10, 1, true, _ipn2);
    //         Assert.That(jResponseGetFailedPaymentOperations2!.totalRecords == 0);
    //         var jResponseGetFailedPaymentOperations3 =
    //             await _getFailedPaymentOperations.SendPostRequestToGetFailedPaymentOperationsAsync( _requestHeader!,
    //                 jResponse.Merchants[0].Id.ToString(), 10, 1, true, _ipn3);
    //         Assert.That(jResponseGetFailedPaymentOperations3!.totalRecords == 0);
    //         var jResponseGetFailedPaymentOperations4 =
    //             await _getFailedPaymentOperations.SendPostRequestToGetFailedPaymentOperationsAsync( _requestHeader!,
    //                 jResponse.Merchants[0].Id.ToString(), 10, 1, true, _ipn4);
    //         Assert.That(jResponseGetFailedPaymentOperations4!.totalRecords == 0);
    //         var jResponseGetFailedPaymentOperations5 =
    //             await _getFailedPaymentOperations.SendPostRequestToGetFailedPaymentOperationsAsync( _requestHeader!,
    //                 jResponse.Merchants[0].Id.ToString(), 10, 1, true, _ipn5);
    //         Assert.That(jResponseGetFailedPaymentOperations5!.totalRecords == 0);
    //         Console.WriteLine("Done TestValidate_DBS_FUN_STL_MON_FTR_CRC_F06_PartialRefund_OutstandingBalanceFirst_AcknowledgedWithError");
    //     }
    //     catch (Exception exception)
    //     {
    //         Assert.Fail(
    //             "Error in TestValidate_DBS_FUN_STL_MON_FTR_CRC_F06_PartialRefund_OutstandingBalanceFirst_AcknowledgedWithError \n" +
    //             exception + "\n");
    //     }
    // }
    //
    // [TestCase(Category = "FundingOperations")]
    // [Test(Description = "TestValidate_DBS_FUN_STL_MON_FTR_CRC_F06_PartialRefund_PastCharge_AcknowledgedWithError with generated merchant"),
    //  CancelAfter(960 * 1000)]
    // public async Task TestValidate_DBS_FUN_STL_MON_FTR_CRC_F06_PartialRefund_PastCharge_AcknowledgedWithError()
    // {
    //     try
    //     {
    //         Console.WriteLine(
    //             "\nStarting TestValidate_DBS_FUN_STL_MON_FTR_CRC_F06_PartialRefund_PastCharge_AcknowledgedWithError");
    //         var testName = TestContext.CurrentContext.Test.Name;
    //         (_terminalApiKey, _businessUnitId) = await _testsHelper.CreateMerchantFunctionality( _requestHeader!,
    //             testName, _db!,"PaymentWizard.v3.5", "NonSecuredPlan", "Monthly", _skuDict!, "315");
    //         Assert.That(_terminalApiKey, Is.Not.EqualTo(""));
    //         var jResponse = await _list.SendGetMerchantListRequestAsync( _requestHeader!, _businessUnitId);
    //         Console.WriteLine("Done with creation of a new merchant , settings and terminal process");
    //         var createPlanDefaultValues = new CreatePlanDefaultValues();
    //         var is3DsVarsCreation = new Is3DsVarsCreation();
    //         createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
    //         Console.WriteLine("Creating 5 plans and querying mongo");
    //         await Task.Delay(5 * 1000);
    //         (_flag, _ipn1) = await _testsHelper.CreatePlanAndQueryMongoAcknowledged(_terminalApiKey, _businessUnitId,_db!,
    //              _requestHeader!, createPlanDefaultValues, 4, 6,
    //             "PartialRefund", "FutureInstallmentsNotAllowed", "AcknowledgedWithError",
    //             false);
    //         Assert.That(_flag);
    //         await Task.Delay(5 * 1000);
    //         (_flag, _ipn2) = await _testsHelper.CreatePlanAndQueryMongoAcknowledged(_terminalApiKey, _businessUnitId,_db!,
    //              _requestHeader!, createPlanDefaultValues, 4, 6,
    //             "PartialRefund", "FutureInstallmentsNotAllowed", "AcknowledgedWithError",
    //             false);
    //         Assert.That(_flag);
    //         await Task.Delay(5 * 1000);
    //         (_flag, _ipn3) = await _testsHelper.CreatePlanAndQueryMongoAcknowledged(_terminalApiKey, _businessUnitId,_db!,
    //              _requestHeader!, createPlanDefaultValues, 4, 6,
    //             "PartialRefund", "FutureInstallmentsNotAllowed", "AcknowledgedWithError",
    //             false);
    //         Assert.That(_flag);
    //         await Task.Delay(5 * 1000);
    //         (_flag, _ipn4) = await _testsHelper.CreatePlanAndQueryMongoAcknowledged(_terminalApiKey, _businessUnitId,_db!,
    //              _requestHeader!, createPlanDefaultValues, 4, 6,
    //             "PartialRefund", "FutureInstallmentsNotAllowed", "AcknowledgedWithError",
    //             false);
    //         Assert.That(_flag);
    //         await Task.Delay(5 * 1000);
    //         (_flag, _ipn5) = await _testsHelper.CreatePlanAndQueryMongoAcknowledged(_terminalApiKey, _businessUnitId,_db!,
    //              _requestHeader!, createPlanDefaultValues, 4, 6,
    //             "PartialRefund", "FutureInstallmentsNotAllowed", "AcknowledgedWithError",
    //             false);
    //         Assert.That(_flag);
    //         Console.WriteLine("Done Creating 5 plans and querying mongo\n");
    //         var requestToGetPaymentFundingOperations =
    //             await _getPaymentFundingOperations.SendPostRequestToGetPaymentFundingOperationsAsync( _requestHeader!,
    //                 jResponse!.Merchants[0].Id.ToString(), 10, 1, "merchantName");
    //         Assert.That(requestToGetPaymentFundingOperations!.paymentFundingOperationResponses[0].activitiesList.Count == 2);
    //         Assert.That(requestToGetPaymentFundingOperations.paymentFundingOperationResponses[0].collectNumberOfActivities == 1);
    //         Assert.That(requestToGetPaymentFundingOperations.paymentFundingOperationResponses[0].fundNumberOfActivities == 1);
    //         var jResponseGetFailedPaymentOperations1 =
    //             await _getFailedPaymentOperations.SendPostRequestToGetFailedPaymentOperationsAsync( _requestHeader!,
    //                 jResponse.Merchants[0].Id.ToString(), 10, 1, true, _ipn1);
    //         Assert.That(jResponseGetFailedPaymentOperations1!.totalRecords == 0);
    //         var jResponseGetFailedPaymentOperations2 =
    //             await _getFailedPaymentOperations.SendPostRequestToGetFailedPaymentOperationsAsync( _requestHeader!,
    //                 jResponse.Merchants[0].Id.ToString(), 10, 1, true, _ipn2);
    //         Assert.That(jResponseGetFailedPaymentOperations2!.totalRecords == 0);
    //         var jResponseGetFailedPaymentOperations3 =
    //             await _getFailedPaymentOperations.SendPostRequestToGetFailedPaymentOperationsAsync( _requestHeader!,
    //                 jResponse.Merchants[0].Id.ToString(), 10, 1, true, _ipn3);
    //         Assert.That(jResponseGetFailedPaymentOperations3!.totalRecords == 0);
    //         var jResponseGetFailedPaymentOperations4 =
    //             await _getFailedPaymentOperations.SendPostRequestToGetFailedPaymentOperationsAsync( _requestHeader!,
    //                 jResponse.Merchants[0].Id.ToString(), 10, 1, true, _ipn4);
    //         Assert.That(jResponseGetFailedPaymentOperations4!.totalRecords == 0);
    //         var jResponseGetFailedPaymentOperations5 =
    //             await _getFailedPaymentOperations.SendPostRequestToGetFailedPaymentOperationsAsync( _requestHeader!,
    //                 jResponse.Merchants[0].Id.ToString(), 10, 1, true, _ipn5);
    //         Assert.That(jResponseGetFailedPaymentOperations5!.totalRecords == 0);
    //         Console.WriteLine("Done TestValidate_DBS_FUN_STL_MON_FTR_CRC_F06_PartialRefund_PastCharge_AcknowledgedWithError");
    //     }
    //     catch (Exception exception)
    //     {
    //         Assert.Fail(
    //             "Error in TestValidate_DBS_FUN_STL_MON_FTR_CRC_F06_PartialRefund_PastCharge_AcknowledgedWithError \n" +
    //             exception + "\n");
    //     }
    // }
    //
    // [TestCase(Category = "FundingOperations")]
    // [Test(Description = "TestValidate_DBS_FUN_STL_MON_FTR_CRC_F06_PartialRefund_OutstandingBalanceFirst_Acknowledged with generated merchant"),
    //  CancelAfter(960 * 1000)]
    // public async Task TestValidate_DBS_FUN_STL_MON_FTR_CRC_F06_PartialRefund_OutstandingBalanceFirst_Acknowledged()
    // {
    //     try
    //     {
    //         Console.WriteLine("\nStarting TestValidate_DBS_FUN_STL_MON_FTR_CRC_F06_PartialRefund_OutstandingBalanceFirst_Acknowledged");
    //         var testName = TestContext.CurrentContext.Test.Name;
    //         (_terminalApiKey, _businessUnitId) = await _testsHelper.CreateMerchantFunctionality( _requestHeader!,
    //             testName, _db!,"PaymentWizard.v3.5", "NonSecuredPlan", "Monthly", _skuDict!, "315");
    //         Assert.That(_terminalApiKey, Is.Not.EqualTo(""));
    //         var jResponse = await _list.SendGetMerchantListRequestAsync( _requestHeader!, _businessUnitId);
    //         Console.WriteLine("Done with creation of a new merchant , settings and terminal process");
    //         var createPlanDefaultValues = new CreatePlanDefaultValues();
    //         var is3DsVarsCreation = new Is3DsVarsCreation();
    //         createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
    //         Console.WriteLine("Creating 5 plans and querying mongo");
    //         await Task.Delay(5 * 1000);
    //         (_flag, _ipn1) = await _testsHelper.CreatePlanAndQueryMongoAcknowledged(_terminalApiKey, _businessUnitId,_db!,
    //              _requestHeader!, createPlanDefaultValues, 4, 6,
    //             "PartialRefund", null!, "Acknowledged", true);
    //         Assert.That(_flag);
    //         await Task.Delay(5 * 1000);
    //         (_flag, _ipn2) = await _testsHelper.CreatePlanAndQueryMongoAcknowledged(_terminalApiKey, _businessUnitId,_db!,
    //              _requestHeader!, createPlanDefaultValues, 4, 6,
    //             "PartialRefund", null!, "Acknowledged", true);
    //         Assert.That(_flag);
    //         await Task.Delay(5 * 1000);
    //         (_flag, _ipn3) = await _testsHelper.CreatePlanAndQueryMongoAcknowledged(_terminalApiKey, _businessUnitId,_db!,
    //              _requestHeader!, createPlanDefaultValues, 4, 6,
    //             "PartialRefund", null!, "Acknowledged", true);
    //         Assert.That(_flag);
    //         await Task.Delay(5 * 1000);
    //         (_flag, _ipn4) = await _testsHelper.CreatePlanAndQueryMongoAcknowledged(_terminalApiKey, _businessUnitId,_db!,
    //              _requestHeader!, createPlanDefaultValues, 4, 6,
    //             "PartialRefund", null!, "Acknowledged", true);
    //         Assert.That(_flag);
    //         await Task.Delay(5 * 1000);
    //         (_flag, _ipn5) = await _testsHelper.CreatePlanAndQueryMongoAcknowledged(_terminalApiKey, _businessUnitId,_db!,
    //              _requestHeader!, createPlanDefaultValues, 4, 6,
    //             "PartialRefund", null!, "Acknowledged", true);
    //         Assert.That(_flag);
    //         Console.WriteLine("Done Creating 5 plans and querying mongo\n");
    //         var requestToGetPaymentFundingOperations =
    //             await _getPaymentFundingOperations.SendPostRequestToGetPaymentFundingOperationsAsync( _requestHeader!,
    //                 jResponse!.Merchants[0].Id.ToString(), 10, 1, "merchantName");
    //         Assert.That(requestToGetPaymentFundingOperations!.paymentFundingOperationResponses[0].activitiesList.Count == 2);
    //         Assert.That(requestToGetPaymentFundingOperations.paymentFundingOperationResponses[0].collectNumberOfActivities == 1);
    //         Assert.That(requestToGetPaymentFundingOperations.paymentFundingOperationResponses[0].fundNumberOfActivities == 1);
    //         var jResponseGetFailedPaymentOperations1 =
    //             await _getPaymentRecordsFunctionality.SendGetPaymentRecordRequestAsync( _requestHeader!, _ipn1);
    //         Assert.That(jResponseGetFailedPaymentOperations1.totalRecords > 0);
    //         var jResponseGetFailedPaymentOperations2 =
    //             await _getPaymentRecordsFunctionality.SendGetPaymentRecordRequestAsync( _requestHeader!, _ipn2);
    //         Assert.That(jResponseGetFailedPaymentOperations2.totalRecords > 0);
    //         var jResponseGetFailedPaymentOperations3 =
    //             await _getPaymentRecordsFunctionality.SendGetPaymentRecordRequestAsync( _requestHeader!, _ipn3);
    //         Assert.That(jResponseGetFailedPaymentOperations3.totalRecords > 0);
    //         var jResponseGetFailedPaymentOperations4 =
    //             await _getPaymentRecordsFunctionality.SendGetPaymentRecordRequestAsync( _requestHeader!, _ipn4);
    //         Assert.That(jResponseGetFailedPaymentOperations4.totalRecords > 0);
    //         var jResponseGetFailedPaymentOperations5 =
    //             await _getPaymentRecordsFunctionality.SendGetPaymentRecordRequestAsync( _requestHeader!, _ipn5);
    //         Assert.That(jResponseGetFailedPaymentOperations5.totalRecords > 0);
    //         Console.WriteLine("Done TestValidate_DBS_FUN_STL_MON_FTR_CRC_F06_PartialRefund_OutstandingBalanceFirst_Acknowledged");
    //     }
    //     catch (Exception exception)
    //     {
    //         Assert.Fail("Error in TestValidate_DBS_FUN_STL_MON_FTR_CRC_F06_PartialRefund_OutstandingBalanceFirst_Acknowledged \n" +
    //                     exception + "\n");
    //     }
    // }
    //
    // [TestCase(Category = "FundingOperations")]
    // [Test(Description = "TestValidate_DBS_FUN_STL_MON_FTR_CRC_F06_PartialRefund_PastCharge_Acknowledged with generated merchant"),
    //  CancelAfter(960 * 1000)]
    // public async Task TestValidate_DBS_FUN_STL_MON_FTR_CRC_F06_PartialRefund_PastCharge_Acknowledged()
    // {
    //     try
    //     {
    //         Console.WriteLine(
    //             "\nStarting TestValidate_DBS_FUN_STL_MON_FTR_CRC_F06_PartialRefund_PastCharge_Acknowledged");
    //         var testName = TestContext.CurrentContext.Test.Name;
    //         (_terminalApiKey, _businessUnitId) = await _testsHelper.CreateMerchantFunctionality( _requestHeader!,
    //             testName, _db!,"PaymentWizard.v3.5", "NonSecuredPlan", "Monthly", _skuDict!, "315");
    //         Assert.That(_terminalApiKey, Is.Not.EqualTo(""));
    //         var jResponse = await _list.SendGetMerchantListRequestAsync( _requestHeader!, _businessUnitId);
    //         Console.WriteLine("Done with creation of a new merchant , settings and terminal process");
    //         var createPlanDefaultValues = new CreatePlanDefaultValues();
    //         var is3DsVarsCreation = new Is3DsVarsCreation();
    //         createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
    //         Console.WriteLine("Creating 5 plans and querying mongo");
    //         await Task.Delay(5 * 1000);
    //         (_flag, _ipn1) = await _testsHelper.CreatePlanAndQueryMongoAcknowledged(_terminalApiKey, _businessUnitId,_db!,
    //              _requestHeader!, createPlanDefaultValues, 4, 6,
    //             "PartialRefund", "FutureInstallmentsNotAllowed", "Acknowledged",
    //             true);
    //         Assert.That(_flag);
    //         await Task.Delay(5 * 1000);
    //         (_flag, _ipn2) = await _testsHelper.CreatePlanAndQueryMongoAcknowledged(_terminalApiKey, _businessUnitId,_db!,
    //              _requestHeader!, createPlanDefaultValues, 4, 6,
    //             "PartialRefund", "FutureInstallmentsNotAllowed", "Acknowledged",
    //             true);
    //         Assert.That(_flag);
    //         await Task.Delay(5 * 1000);
    //         (_flag, _ipn3) = await _testsHelper.CreatePlanAndQueryMongoAcknowledged(_terminalApiKey, _businessUnitId,_db!,
    //              _requestHeader!, createPlanDefaultValues, 4, 6,
    //             "PartialRefund", "FutureInstallmentsNotAllowed", "Acknowledged",
    //             true);
    //         Assert.That(_flag);
    //         await Task.Delay(5 * 1000);
    //         (_flag, _ipn4) = await _testsHelper.CreatePlanAndQueryMongoAcknowledged(_terminalApiKey, _businessUnitId,_db!,
    //              _requestHeader!, createPlanDefaultValues, 4, 6,
    //             "PartialRefund", "FutureInstallmentsNotAllowed", "Acknowledged",
    //             true);
    //         Assert.That(_flag);
    //         await Task.Delay(5 * 1000);
    //         (_flag, _ipn5) = await _testsHelper.CreatePlanAndQueryMongoAcknowledged(_terminalApiKey, _businessUnitId,_db!,
    //              _requestHeader!, createPlanDefaultValues, 4, 6,
    //             "PartialRefund", "FutureInstallmentsNotAllowed", "Acknowledged",
    //             true);
    //         Assert.That(_flag);
    //         Console.WriteLine("Done Creating 5 plans and querying mongo\n");
    //         var requestToGetPaymentFundingOperations =
    //             await _getPaymentFundingOperations.SendPostRequestToGetPaymentFundingOperationsAsync( _requestHeader!,
    //                 jResponse!.Merchants[0].Id.ToString(), 10, 1, "merchantName");
    //         Assert.That(requestToGetPaymentFundingOperations!.paymentFundingOperationResponses[0].activitiesList.Count == 2);
    //         Assert.That(requestToGetPaymentFundingOperations.paymentFundingOperationResponses[0].collectNumberOfActivities == 1);
    //         Assert.That(requestToGetPaymentFundingOperations.paymentFundingOperationResponses[0].fundNumberOfActivities == 1);
    //         var jResponseGetFailedPaymentOperations1 =
    //             await _getPaymentRecordsFunctionality.SendGetPaymentRecordRequestAsync( _requestHeader!, _ipn1);
    //         Assert.That(jResponseGetFailedPaymentOperations1.totalRecords > 0);
    //         var jResponseGetFailedPaymentOperations2 =
    //             await _getPaymentRecordsFunctionality.SendGetPaymentRecordRequestAsync( _requestHeader!, _ipn2);
    //         Assert.That(jResponseGetFailedPaymentOperations2.totalRecords > 0);
    //         var jResponseGetFailedPaymentOperations3 =
    //             await _getPaymentRecordsFunctionality.SendGetPaymentRecordRequestAsync( _requestHeader!, _ipn3);
    //         Assert.That(jResponseGetFailedPaymentOperations3.totalRecords > 0);
    //         var jResponseGetFailedPaymentOperations4 =
    //             await _getPaymentRecordsFunctionality.SendGetPaymentRecordRequestAsync( _requestHeader!, _ipn4);
    //         Assert.That(jResponseGetFailedPaymentOperations4.totalRecords > 0);
    //         var jResponseGetFailedPaymentOperations5 =
    //             await _getPaymentRecordsFunctionality.SendGetPaymentRecordRequestAsync( _requestHeader!, _ipn5);
    //         Assert.That(jResponseGetFailedPaymentOperations5.totalRecords > 0);
    //         Console.WriteLine("Done TestValidate_DBS_FUN_STL_MON_FTR_CRC_F06_PartialRefund_PastCharge_Acknowledged");
    //     }
    //     catch (Exception exception)
    //     {
    //         Assert.Fail("Error in TestValidate_DBS_FUN_STL_MON_FTR_CRC_F06_PartialRefund_PastCharge_Acknowledged \n" + exception + "\n");
    //     }
    // }
    //
    // [TestCase(Category = "FundingOperations")]
    // [Test(Description = "TestValidate_DPL_FUN_STL_BIW_FTR_CRC_F03_PartialRefund_OutstandingBalanceFirst_Acknowledged with generated merchant"),
    //  CancelAfter(960 * 1000)]
    // public async Task TestValidate_DPL_FUN_STL_BIW_FTR_CRC_F03_PartialRefund_OutstandingBalanceFirst_Acknowledged()
    // {
    //     try
    //     {
    //         Console.WriteLine("\nStarting TestValidate_DPL_FUN_STL_BIW_FTR_CRC_F03_PartialRefund_OutstandingBalanceFirst_Acknowledged");
    //         var testName = TestContext.CurrentContext.Test.Name;
    //         (_terminalApiKey, _businessUnitId) = await _testsHelper.CreateMerchantFunctionality( _requestHeader!,
    //             testName, _db!, "PaymentWizard.v3.5", "NonSecuredPlan", "CustomDays", _skuDict!, "315");
    //         Assert.That(_terminalApiKey, Is.Not.EqualTo(""));
    //         var jResponse = await _list.SendGetMerchantListRequestAsync( _requestHeader!, _businessUnitId);
    //         Console.WriteLine("Done with creation of a new merchant , settings and terminal process");
    //         var createPlanDefaultValues = new CreatePlanDefaultValues();
    //         var is3DsVarsCreation = new Is3DsVarsCreation();
    //         createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
    //         Console.WriteLine("Creating 5 plans and querying mongo");
    //         await Task.Delay(5 * 1000);
    //         (_flag, _ipn1) = await _testsHelper.CreatePlanAndQueryMongoAcknowledged(_terminalApiKey, _businessUnitId, _db!,
    //              _requestHeader!, createPlanDefaultValues, 2, 3,
    //             "PartialRefund", null!, "Acknowledged", true);
    //         Assert.That(_flag);
    //         await Task.Delay(5 * 1000);
    //         (_flag, _ipn2) = await _testsHelper.CreatePlanAndQueryMongoAcknowledged(_terminalApiKey, _businessUnitId, _db!,
    //              _requestHeader!, createPlanDefaultValues, 2, 3,
    //             "PartialRefund", null!, "Acknowledged", true);
    //         Assert.That(_flag);
    //         await Task.Delay(5 * 1000);
    //         (_flag, _ipn3) = await _testsHelper.CreatePlanAndQueryMongoAcknowledged(_terminalApiKey, _businessUnitId, _db!,
    //              _requestHeader!, createPlanDefaultValues, 2, 3,
    //             "PartialRefund", null!, "Acknowledged", true);
    //         Assert.That(_flag);
    //         await Task.Delay(5 * 1000);
    //         (_flag, _ipn4) = await _testsHelper.CreatePlanAndQueryMongoAcknowledged(_terminalApiKey, _businessUnitId, _db!,
    //              _requestHeader!, createPlanDefaultValues, 2, 3,
    //             "PartialRefund", null!, "Acknowledged", true);
    //         Assert.That(_flag);
    //         await Task.Delay(5 * 1000);
    //         (_flag, _ipn5) = await _testsHelper.CreatePlanAndQueryMongoAcknowledged(_terminalApiKey, _businessUnitId, _db!,
    //              _requestHeader!, createPlanDefaultValues, 2, 3,
    //             "PartialRefund", null!, "Acknowledged", true);
    //         Assert.That(_flag);
    //         Console.WriteLine("Done Creating 5 plans and querying mongo\n");
    //         var requestToGetPaymentFundingOperations =
    //             await _getPaymentFundingOperations.SendPostRequestToGetPaymentFundingOperationsAsync( _requestHeader!,
    //                 jResponse!.Merchants[0].Id.ToString(), 10, 1, "merchantName");
    //         Assert.That(requestToGetPaymentFundingOperations!.paymentFundingOperationResponses[0].activitiesList.Count == 2);
    //         Assert.That(requestToGetPaymentFundingOperations.paymentFundingOperationResponses[0].collectNumberOfActivities == 1);
    //         Assert.That(requestToGetPaymentFundingOperations.paymentFundingOperationResponses[0].fundNumberOfActivities == 1);
    //         var jResponseGetFailedPaymentOperations1 =
    //             await _getPaymentRecordsFunctionality.SendGetPaymentRecordRequestAsync( _requestHeader!, _ipn1);
    //         Assert.That(jResponseGetFailedPaymentOperations1.totalRecords > 0);
    //         var jResponseGetFailedPaymentOperations2 =
    //             await _getPaymentRecordsFunctionality.SendGetPaymentRecordRequestAsync( _requestHeader!, _ipn2);
    //         Assert.That(jResponseGetFailedPaymentOperations2.totalRecords > 0);
    //         var jResponseGetFailedPaymentOperations3 =
    //             await _getPaymentRecordsFunctionality.SendGetPaymentRecordRequestAsync( _requestHeader!, _ipn3);
    //         Assert.That(jResponseGetFailedPaymentOperations3.totalRecords > 0);
    //         var jResponseGetFailedPaymentOperations4 =
    //             await _getPaymentRecordsFunctionality.SendGetPaymentRecordRequestAsync( _requestHeader!, _ipn4);
    //         Assert.That(jResponseGetFailedPaymentOperations4.totalRecords > 0);
    //         var jResponseGetFailedPaymentOperations5 =
    //             await _getPaymentRecordsFunctionality.SendGetPaymentRecordRequestAsync( _requestHeader!, _ipn5);
    //         Assert.That(jResponseGetFailedPaymentOperations5.totalRecords > 0);
    //         Console.WriteLine("Done TestValidate_DPL_FUN_STL_BIW_FTR_CRC_F03_PartialRefund_OutstandingBalanceFirst_Acknowledged");
    //     }
    //     catch (Exception exception)
    //     {
    //         Assert.Fail("Error in TestValidate_DPL_FUN_STL_BIW_FTR_CRC_F03_PartialRefund_OutstandingBalanceFirst_Acknowledged " +
    //                     "\n" + exception + "\n");
    //     }
    // }
    //
    // [TestCase(Category = "FundingOperations")]
    // [Test(Description = "TestValidate_DPL_FUN_STL_BIW_FTR_CRC_F03_PartialRefund_PastCharge_Acknowledged with generated merchant"),
    //  CancelAfter(960 * 1000)]
    // public async Task TestValidate_DPL_FUN_STL_BIW_FTR_CRC_F03_PartialRefund_PastCharge_Acknowledged()
    // {
    //     try
    //     {
    //         Console.WriteLine("\nStarting TestValidate_DPL_FUN_STL_BIW_FTR_CRC_F03_PartialRefund_PastCharge_Acknowledged");
    //         var testName = TestContext.CurrentContext.Test.Name;
    //         (_terminalApiKey, _businessUnitId) = await _testsHelper.CreateMerchantFunctionality( _requestHeader!,
    //             testName, _db!,"PaymentWizard.v3.5", "NonSecuredPlan", "CustomDays", _skuDict!, "315");
    //         Assert.That(_terminalApiKey, Is.Not.EqualTo(""));
    //         var jResponse = await _list.SendGetMerchantListRequestAsync( _requestHeader!, _businessUnitId);
    //         Console.WriteLine("Done with creation of a new merchant , settings and terminal process");
    //         var createPlanDefaultValues = new CreatePlanDefaultValues();
    //         var is3DsVarsCreation = new Is3DsVarsCreation();
    //         createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
    //         Console.WriteLine("Creating 5 plans and querying mongo");
    //         await Task.Delay(5 * 1000);
    //         (_flag, _ipn1) = await _testsHelper.CreatePlanAndQueryMongoAcknowledged(_terminalApiKey, _businessUnitId, _db!,
    //              _requestHeader!, createPlanDefaultValues, 2, 3,
    //             "PartialRefund", "FutureInstallmentsNotAllowed", "Acknowledged", true);
    //         Assert.That(_flag);
    //         await Task.Delay(5 * 1000);
    //         (_flag, _ipn2) = await _testsHelper.CreatePlanAndQueryMongoAcknowledged(_terminalApiKey, _businessUnitId, _db!,
    //             
    //             _requestHeader!, createPlanDefaultValues, 2, 3,
    //             "PartialRefund", "FutureInstallmentsNotAllowed", "Acknowledged", true);
    //         Assert.That(_flag);
    //         await Task.Delay(5 * 1000);
    //         (_flag, _ipn3) = await _testsHelper.CreatePlanAndQueryMongoAcknowledged(_terminalApiKey, _businessUnitId, _db!,
    //              _requestHeader!, createPlanDefaultValues, 2, 3,
    //             "PartialRefund", "FutureInstallmentsNotAllowed", "Acknowledged",
    //             true);
    //         Assert.That(_flag);
    //         await Task.Delay(5 * 1000);
    //         (_flag, _ipn4) = await _testsHelper.CreatePlanAndQueryMongoAcknowledged(_terminalApiKey, _businessUnitId, _db!,
    //              _requestHeader!, createPlanDefaultValues, 2, 3,
    //             "PartialRefund", "FutureInstallmentsNotAllowed", "Acknowledged", true);
    //         Assert.That(_flag);
    //         await Task.Delay(5 * 1000);
    //         (_flag, _ipn5) = await _testsHelper.CreatePlanAndQueryMongoAcknowledged(_terminalApiKey, _businessUnitId, _db!,
    //              _requestHeader!, createPlanDefaultValues, 2, 3,
    //             "PartialRefund", "FutureInstallmentsNotAllowed", "Acknowledged", true);
    //         Assert.That(_flag);
    //         Console.WriteLine("Done Creating 5 plans and querying mongo\n");
    //         var requestToGetPaymentFundingOperations =
    //             await _getPaymentFundingOperations.SendPostRequestToGetPaymentFundingOperationsAsync( _requestHeader!,
    //                 jResponse!.Merchants[0].Id.ToString(), 10, 1, "merchantName");
    //         Assert.That(requestToGetPaymentFundingOperations!.paymentFundingOperationResponses[0].activitiesList.Count == 2);
    //         Assert.That(requestToGetPaymentFundingOperations.paymentFundingOperationResponses[0].collectNumberOfActivities == 1);
    //         Assert.That(requestToGetPaymentFundingOperations.paymentFundingOperationResponses[0].fundNumberOfActivities == 1);
    //         var jResponseGetFailedPaymentOperations1 =
    //             await _getPaymentRecordsFunctionality.SendGetPaymentRecordRequestAsync( _requestHeader!, _ipn1);
    //         Assert.That(jResponseGetFailedPaymentOperations1.totalRecords > 0);
    //         var jResponseGetFailedPaymentOperations2 =
    //             await _getPaymentRecordsFunctionality.SendGetPaymentRecordRequestAsync( _requestHeader!, _ipn2);
    //         Assert.That(jResponseGetFailedPaymentOperations2.totalRecords > 0);
    //         var jResponseGetFailedPaymentOperations3 =
    //             await _getPaymentRecordsFunctionality.SendGetPaymentRecordRequestAsync( _requestHeader!, _ipn3);
    //         Assert.That(jResponseGetFailedPaymentOperations3.totalRecords > 0);
    //         var jResponseGetFailedPaymentOperations4 =
    //             await _getPaymentRecordsFunctionality.SendGetPaymentRecordRequestAsync( _requestHeader!, _ipn4);
    //         Assert.That(jResponseGetFailedPaymentOperations4.totalRecords > 0);
    //         var jResponseGetFailedPaymentOperations5 =
    //             await _getPaymentRecordsFunctionality.SendGetPaymentRecordRequestAsync( _requestHeader!, _ipn5);
    //         Assert.That(jResponseGetFailedPaymentOperations5.totalRecords > 0);
    //         Console.WriteLine("Done TestValidate_DPL_FUN_STL_BIW_FTR_CRC_F03_PartialRefund_PastCharge_Acknowledged");
    //     }
    //     catch (Exception exception)
    //     {
    //         Assert.Fail("Error in TestValidate_DPL_FUN_STL_BIW_FTR_CRC_F03_PartialRefund_PastCharge_Acknowledged \n" +
    //                     exception + "\n");
    //     }
    // }
    //
    // [TestCase(Category = "FundingOperations")]
    // [Test(Description = "TestValidate_DPL_FUN_STL_BIW_FTR_CRC_F03_PartialRefund_OutstandingBalanceFirst_Invoiced with generated merchant"),
    //  CancelAfter(960 * 1000)]
    // public async Task TestValidate_DPL_FUN_STL_BIW_FTR_CRC_F03_PartialRefund_OutstandingBalanceFirst_Invoiced()
    // {
    //     try
    //     {
    //         Console.WriteLine("\nStarting TestValidate_DPL_FUN_STL_BIW_FTR_CRC_F03_PartialRefund_OutstandingBalanceFirst_Invoiced");
    //         var testName = TestContext.CurrentContext.Test.Name;
    //         (_terminalApiKey, _businessUnitId) = await _testsHelper.CreateMerchantFunctionality( _requestHeader!,
    //             testName, _db!,"PaymentWizard.v3.5", "NonSecuredPlan", "CustomDays", _skuDict!, "315");
    //         Assert.That(_terminalApiKey, Is.Not.EqualTo(""));
    //         var jResponse = await _list.SendGetMerchantListRequestAsync( _requestHeader!, _businessUnitId);
    //         Console.WriteLine("Done with creation of a new merchant , settings and terminal process");
    //         var createPlanDefaultValues = new CreatePlanDefaultValues();
    //         var is3DsVarsCreation = new Is3DsVarsCreation();
    //         createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
    //         Console.WriteLine("Creating 5 plans and querying mongo");
    //         await Task.Delay(5 * 1000);
    //         (_flag, _ipn1) = await _testsHelper.CreatePlanAndQueryMongoInvoiced(_terminalApiKey, _businessUnitId, _db!,
    //             _requestHeader!, createPlanDefaultValues, 2, 3,
    //             "PartialRefund", null!, "Acknowledged", true);
    //         Assert.That(_flag);
    //         await Task.Delay(5 * 1000);
    //         (_flag, _ipn2) = await _testsHelper.CreatePlanAndQueryMongoInvoiced(_terminalApiKey, _businessUnitId, _db!,
    //             _requestHeader!, createPlanDefaultValues, 2, 3,
    //             "PartialRefund", null!, "Acknowledged", true);
    //         Assert.That(_flag);
    //         await Task.Delay(5 * 1000);
    //         (_flag, _ipn3) = await _testsHelper.CreatePlanAndQueryMongoInvoiced(_terminalApiKey, _businessUnitId, _db!,
    //             _requestHeader!, createPlanDefaultValues, 2, 3,
    //             "PartialRefund", null!, "Acknowledged", true);
    //         Assert.That(_flag);
    //         await Task.Delay(5 * 1000);
    //         (_flag, _ipn4) = await _testsHelper.CreatePlanAndQueryMongoInvoiced(_terminalApiKey, _businessUnitId, _db!,
    //             _requestHeader!, createPlanDefaultValues, 2, 3,
    //             "PartialRefund", null!, "Acknowledged", true);
    //         Assert.That(_flag);
    //         await Task.Delay(5 * 1000);
    //         (_flag, _ipn5) = await _testsHelper.CreatePlanAndQueryMongoInvoiced(_terminalApiKey, _businessUnitId, _db!,
    //             _requestHeader!, createPlanDefaultValues, 2, 3,
    //             "PartialRefund", null!, "Acknowledged", true);
    //         Assert.That(_flag);
    //         Console.WriteLine("Done Creating 5 plans and querying mongo\n");
    //         var requestToGetPaymentFundingOperations =
    //             await _getPaymentFundingOperations.SendPostRequestToGetPaymentFundingOperationsAsync( _requestHeader!,
    //                 jResponse!.Merchants[0].Id.ToString(), 10, 1, "merchantName");
    //         Assert.That(requestToGetPaymentFundingOperations!.paymentFundingOperationResponses[0].activitiesList.Count == 2);
    //         Assert.That(requestToGetPaymentFundingOperations.paymentFundingOperationResponses[0].collectNumberOfActivities == 1);
    //         Assert.That(requestToGetPaymentFundingOperations.paymentFundingOperationResponses[0].fundNumberOfActivities == 1);
    //         var jResponseGetFailedPaymentOperations1 =
    //             await _getPaymentRecordsFunctionality.SendGetPaymentRecordRequestAsync( _requestHeader!, _ipn1);
    //         Assert.That(jResponseGetFailedPaymentOperations1.totalRecords > 0);
    //         var jResponseGetFailedPaymentOperations2 =
    //             await _getPaymentRecordsFunctionality.SendGetPaymentRecordRequestAsync( _requestHeader!, _ipn2);
    //         Assert.That(jResponseGetFailedPaymentOperations2.totalRecords > 0);
    //         var jResponseGetFailedPaymentOperations3 =
    //             await _getPaymentRecordsFunctionality.SendGetPaymentRecordRequestAsync( _requestHeader!, _ipn3);
    //         Assert.That(jResponseGetFailedPaymentOperations3.totalRecords > 0);
    //         var jResponseGetFailedPaymentOperations4 =
    //             await _getPaymentRecordsFunctionality.SendGetPaymentRecordRequestAsync( _requestHeader!, _ipn4);
    //         Assert.That(jResponseGetFailedPaymentOperations4.totalRecords > 0);
    //         var jResponseGetFailedPaymentOperations5 =
    //             await _getPaymentRecordsFunctionality.SendGetPaymentRecordRequestAsync( _requestHeader!, _ipn5);
    //         Assert.That(jResponseGetFailedPaymentOperations5.totalRecords > 0);
    //         Console.WriteLine("Done TestValidate_DPL_FUN_STL_BIW_FTR_CRC_F03_PartialRefund_OutstandingBalanceFirst_Invoiced");
    //     }
    //     catch (Exception exception)
    //     {
    //         Assert.Fail("Error in TestValidate_DPL_FUN_STL_BIW_FTR_CRC_F03_PartialRefund_OutstandingBalanceFirst_Invoiced \n" +
    //                     exception + "\n");
    //     }
    // }
    //
    // [TestCase(Category = "FundingOperations")]
    // [Test(Description = "TestValidate_DPL_FUN_STL_BIW_FTR_CRC_F03_PartialRefund_PastCharge_Invoiced with generated merchant"),
    //  CancelAfter(960 * 1000)]
    // public async Task TestValidate_DPL_FUN_STL_BIW_FTR_CRC_F03_PartialRefund_PastCharge_Invoiced()
    // {
    //     try
    //     {
    //         Console.WriteLine("\nStarting TestValidate_DPL_FUN_STL_BIW_FTR_CRC_F03_PartialRefund_PastCharge_Invoiced");
    //         var testName = TestContext.CurrentContext.Test.Name;
    //         (_terminalApiKey, _businessUnitId) = await _testsHelper.CreateMerchantFunctionality( _requestHeader!,
    //             testName, _db!, "PaymentWizard.v3.5", "NonSecuredPlan", "CustomDays", _skuDict!, "315");
    //         Assert.That(_terminalApiKey, Is.Not.EqualTo(""));
    //         var jResponse = await _list.SendGetMerchantListRequestAsync( _requestHeader!, _businessUnitId);
    //         Console.WriteLine("Done with creation of a new merchant , settings and terminal process");
    //         var createPlanDefaultValues = new CreatePlanDefaultValues();
    //         var is3DsVarsCreation = new Is3DsVarsCreation();
    //         createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
    //         Console.WriteLine("Creating 5 plans and querying mongo");
    //         await Task.Delay(5 * 1000);
    //         (_flag, _ipn1) = await _testsHelper.CreatePlanAndQueryMongoInvoiced(_terminalApiKey, _businessUnitId, _db!,
    //             _requestHeader!, createPlanDefaultValues, 2, 3,
    //             "PartialRefund", "FutureInstallmentsNotAllowed", "Acknowledged", true);
    //         Assert.That(_flag);
    //         await Task.Delay(5 * 1000);
    //         (_flag, _ipn2) = await _testsHelper.CreatePlanAndQueryMongoInvoiced(_terminalApiKey, _businessUnitId, _db!,
    //             _requestHeader!, createPlanDefaultValues, 2, 3,
    //             "PartialRefund", "FutureInstallmentsNotAllowed", "Acknowledged", true);
    //         Assert.That(_flag);
    //         await Task.Delay(5 * 1000);
    //         (_flag, _ipn3) = await _testsHelper.CreatePlanAndQueryMongoInvoiced(_terminalApiKey, _businessUnitId, _db!,
    //             _requestHeader!, createPlanDefaultValues, 2, 3,
    //             "PartialRefund", "FutureInstallmentsNotAllowed", "Acknowledged", true);
    //         Assert.That(_flag);
    //         await Task.Delay(5 * 1000);
    //         (_flag, _ipn4) = await _testsHelper.CreatePlanAndQueryMongoInvoiced(_terminalApiKey, _businessUnitId, _db!,
    //             _requestHeader!, createPlanDefaultValues, 2, 3,
    //             "PartialRefund", "FutureInstallmentsNotAllowed", "Acknowledged", true);
    //         Assert.That(_flag);
    //         await Task.Delay(5 * 1000);
    //         (_flag, _ipn5) = await _testsHelper.CreatePlanAndQueryMongoInvoiced(_terminalApiKey, _businessUnitId, _db!,
    //             _requestHeader!, createPlanDefaultValues, 2, 3,
    //             "PartialRefund", "FutureInstallmentsNotAllowed", "Acknowledged", true);
    //         Assert.That(_flag);
    //         Console.WriteLine("Done Creating 5 plans and querying mongo\n");
    //         var requestToGetPaymentFundingOperations =
    //             await _getPaymentFundingOperations.SendPostRequestToGetPaymentFundingOperationsAsync( _requestHeader!,
    //                 jResponse!.Merchants[0].Id.ToString(), 10, 1, "merchantName");
    //         Assert.That(requestToGetPaymentFundingOperations!.paymentFundingOperationResponses[0].activitiesList.Count == 2);
    //         Assert.That(requestToGetPaymentFundingOperations.paymentFundingOperationResponses[0].collectNumberOfActivities == 1);
    //         Assert.That(requestToGetPaymentFundingOperations.paymentFundingOperationResponses[0].fundNumberOfActivities == 1);
    //         var jResponseGetFailedPaymentOperations1 =
    //             await _getPaymentRecordsFunctionality.SendGetPaymentRecordRequestAsync( _requestHeader!, _ipn1);
    //         Assert.That(jResponseGetFailedPaymentOperations1.totalRecords > 0);
    //         var jResponseGetFailedPaymentOperations2 =
    //             await _getPaymentRecordsFunctionality.SendGetPaymentRecordRequestAsync( _requestHeader!, _ipn2);
    //         Assert.That(jResponseGetFailedPaymentOperations2.totalRecords > 0);
    //         var jResponseGetFailedPaymentOperations3 =
    //             await _getPaymentRecordsFunctionality.SendGetPaymentRecordRequestAsync( _requestHeader!, _ipn3);
    //         Assert.That(jResponseGetFailedPaymentOperations3.totalRecords > 0);
    //         var jResponseGetFailedPaymentOperations4 =
    //             await _getPaymentRecordsFunctionality.SendGetPaymentRecordRequestAsync( _requestHeader!, _ipn4);
    //         Assert.That(jResponseGetFailedPaymentOperations4.totalRecords > 0);
    //         var jResponseGetFailedPaymentOperations5 =
    //             await _getPaymentRecordsFunctionality.SendGetPaymentRecordRequestAsync( _requestHeader!, _ipn5);
    //         Assert.That(jResponseGetFailedPaymentOperations5.totalRecords > 0);
    //         Console.WriteLine("Done TestValidate_DPL_FUN_STL_BIW_FTR_CRC_F03_PartialRefund_PastCharge_Invoiced");
    //     }
    //     catch (Exception exception)
    //     {
    //         Assert.Fail("Error in TestValidate_DPL_FUN_STL_BIW_FTR_CRC_F03_PartialRefund_PastCharge_Invoiced \n" +
    //                     exception + "\n");
    //     }
    // }
    //
    // [TestCase(Category = "FundingOperations")]
    // [Test(Description = "TestValidate_DBS_FUN_STL_MON_FTR_CRC_F06_PartialRefund_OutstandingAmount_ChargeBack with generated merchant"),
    //  CancelAfter(960 * 1000)]
    // public async Task TestValidate_DBS_FUN_STL_MON_FTR_CRC_F06_PartialRefund_OutstandingAmount_ChargeBack()
    // {
    //     try
    //     {
    //         Console.WriteLine("\nStarting TestValidate_DBS_FUN_STL_MON_FTR_CRC_F06_PartialRefund_OutstandingAmount_ChargeBack");
    //         var testName = TestContext.CurrentContext.Test.Name;
    //         (_terminalApiKey, _businessUnitId) = await _testsHelper.CreateMerchantFunctionality( _requestHeader!,
    //             testName, _db!,"PaymentWizard.v3.5", "NonSecuredPlan", "Monthly", _skuDict!, "109");
    //         Assert.That(_terminalApiKey, Is.Not.EqualTo(""));
    //         var jResponse = await _list.SendGetMerchantListRequestAsync( _requestHeader!, _businessUnitId);
    //         Console.WriteLine("Done with creation of a new merchant , settings and terminal process");
    //         var createPlanDefaultValues = new CreatePlanDefaultValues();
    //         var is3DsVarsCreation = new Is3DsVarsCreation();
    //         createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
    //         Console.WriteLine("Creating 5 plans and querying mongo");
    //         await Task.Delay(5 * 1000);
    //         (_flag, _ipn1) = await _testsHelper.CreatePlanAndQueryMongoBeforeChargeBack(_terminalApiKey, _businessUnitId, _db!,
    //              _requestHeader!, createPlanDefaultValues, 4, 6);
    //         Assert.That(_flag);
    //         await Task.Delay(5 * 1000);
    //         (_flag, _ipn2) = await _testsHelper.CreatePlanAndQueryMongoBeforeChargeBack(_terminalApiKey, _businessUnitId, _db!,
    //              _requestHeader!, createPlanDefaultValues, 4, 6);
    //         Assert.That(_flag);
    //         await Task.Delay(5 * 1000);
    //         (_flag, _ipn3) = await _testsHelper.CreatePlanAndQueryMongoBeforeChargeBack(_terminalApiKey, _businessUnitId, _db!,
    //              _requestHeader!, createPlanDefaultValues, 4, 6);
    //         Assert.That(_flag);
    //         await Task.Delay(5 * 1000);
    //         (_flag, _ipn4) = await _testsHelper.CreatePlanAndQueryMongoBeforeChargeBack(_terminalApiKey, _businessUnitId, _db!,
    //              _requestHeader!, createPlanDefaultValues, 4, 6);
    //         Assert.That(_flag);
    //         await Task.Delay(5 * 1000);
    //         (_flag, _ipn5) = await _testsHelper.CreatePlanAndQueryMongoBeforeChargeBack(_terminalApiKey, _businessUnitId, _db!,
    //              _requestHeader!, createPlanDefaultValues, 4, 6);
    //         Assert.That(_flag);
    //         Console.WriteLine("Done Creating 5 plans and querying mongo\n");
    //         Assert.That(await _testsHelper.DoingChargeBackAndQueryMongo( _requestHeader!, _ipn1, _db!,"PartialRefund", null!));
    //         var requestToGetPaymentFundingOperations =
    //             await _getPaymentFundingOperations.SendPostRequestToGetPaymentFundingOperationsAsync( _requestHeader!,
    //                 jResponse!.Merchants[0].Id.ToString(), 10, 1, "merchantName");
    //         Assert.That(requestToGetPaymentFundingOperations!.paymentFundingOperationResponses[0].activitiesList.Count == 2);
    //         Assert.That(requestToGetPaymentFundingOperations.paymentFundingOperationResponses[0].collectNumberOfActivities == 1);
    //         Assert.That(requestToGetPaymentFundingOperations.paymentFundingOperationResponses[0].fundNumberOfActivities == 1);
    //         var jResponseGetFailedPaymentOperations1 =
    //             await _getPaymentRecordsFunctionality.SendGetPaymentRecordRequestAsync( _requestHeader!, _ipn1);
    //         Assert.That(jResponseGetFailedPaymentOperations1.totalRecords > 0);
    //         var jResponseGetFailedPaymentOperations2 =
    //             await _getPaymentRecordsFunctionality.SendGetPaymentRecordRequestAsync( _requestHeader!, _ipn2);
    //         Assert.That(jResponseGetFailedPaymentOperations2.totalRecords > 0);
    //         var jResponseGetFailedPaymentOperations3 =
    //             await _getPaymentRecordsFunctionality.SendGetPaymentRecordRequestAsync( _requestHeader!, _ipn3);
    //         Assert.That(jResponseGetFailedPaymentOperations3.totalRecords > 0);
    //         var jResponseGetFailedPaymentOperations4 =
    //             await _getPaymentRecordsFunctionality.SendGetPaymentRecordRequestAsync( _requestHeader!, _ipn4);
    //         Assert.That(jResponseGetFailedPaymentOperations4.totalRecords > 0);
    //         var jResponseGetFailedPaymentOperations5 =
    //             await _getPaymentRecordsFunctionality.SendGetPaymentRecordRequestAsync( _requestHeader!, _ipn5);
    //         Assert.That(jResponseGetFailedPaymentOperations5.totalRecords > 0);
    //         Console.WriteLine("Done TestValidate_DBS_FUN_STL_MON_FTR_CRC_F06_PartialRefund_OutstandingAmount_ChargeBack");
    //     }
    //     catch (Exception exception)
    //     {
    //         Assert.Fail("Error in TestValidate_DBS_FUN_STL_MON_FTR_CRC_F06_PartialRefund_OutstandingAmount_ChargeBack \n" +
    //                     exception + "\n");
    //     }
    // }
    //
    // [TestCase(Category = "FundingOperations")]
    // [Test(Description = "TestValidate_DBS_FUN_STL_MON_FTR_CRC_F06_PartialRefund_PastCharge_ChargeBack with generated merchant"),
    //  CancelAfter(960 * 1000)]
    // public async Task TestValidate_DBS_FUN_STL_MON_FTR_CRC_F06_PartialRefund_PastCharge_ChargeBack()
    // {
    //     try
    //     {
    //         Console.WriteLine(
    //             "\nStarting TestValidate_DBS_FUN_STL_MON_FTR_CRC_F06_PartialRefund_PastCharge_ChargeBack");
    //         var testName = TestContext.CurrentContext.Test.Name;
    //         (_terminalApiKey, _businessUnitId) = await _testsHelper.CreateMerchantFunctionality( _requestHeader!,
    //             testName, _db!,"PaymentWizard.v3.5", "NonSecuredPlan", "Monthly", _skuDict!, "109");
    //         Assert.That(_terminalApiKey, Is.Not.EqualTo(""));
    //         var jResponse = await _list.SendGetMerchantListRequestAsync( _requestHeader!, _businessUnitId);
    //         Console.WriteLine("Done with creation of a new merchant , settings and terminal process");
    //         var createPlanDefaultValues = new CreatePlanDefaultValues();
    //         var is3DsVarsCreation = new Is3DsVarsCreation();
    //         createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
    //         Console.WriteLine("Creating 5 plans and querying mongo");
    //         await Task.Delay(5 * 1000);
    //         (_flag, _ipn1) = await _testsHelper.CreatePlanAndQueryMongoBeforeChargeBack(_terminalApiKey, _businessUnitId, _db!,
    //              _requestHeader!, createPlanDefaultValues, 4, 6);
    //         Assert.That(_flag);
    //         await Task.Delay(5 * 1000);
    //         (_flag, _ipn2) = await _testsHelper.CreatePlanAndQueryMongoBeforeChargeBack(_terminalApiKey, _businessUnitId, _db!,
    //              _requestHeader!, createPlanDefaultValues, 4, 6);
    //         Assert.That(_flag);
    //         await Task.Delay(5 * 1000);
    //         (_flag, _ipn3) = await _testsHelper.CreatePlanAndQueryMongoBeforeChargeBack(_terminalApiKey, _businessUnitId, _db!,
    //              _requestHeader!, createPlanDefaultValues, 4, 6);
    //         Assert.That(_flag);
    //         await Task.Delay(5 * 1000);
    //         (_flag, _ipn4) = await _testsHelper.CreatePlanAndQueryMongoBeforeChargeBack(_terminalApiKey, _businessUnitId, _db!,
    //              _requestHeader!, createPlanDefaultValues, 4, 6);
    //         Assert.That(_flag);
    //         await Task.Delay(5 * 1000);
    //         (_flag, _ipn5) = await _testsHelper.CreatePlanAndQueryMongoBeforeChargeBack(_terminalApiKey, _businessUnitId, _db!,
    //              _requestHeader!, createPlanDefaultValues, 4, 6);
    //         Assert.That(_flag);
    //         Console.WriteLine("Done Creating 5 plans and querying mongo\n");
    //         Assert.That(await _testsHelper.DoingChargeBackAndQueryMongo( _requestHeader!, _ipn1, _db!,"PartialRefund",
    //             "FutureInstallmentsNotAllowed"));
    //         var requestToGetPaymentFundingOperations =
    //             await _getPaymentFundingOperations.SendPostRequestToGetPaymentFundingOperationsAsync( _requestHeader!,
    //                 jResponse!.Merchants[0].Id.ToString(), 10, 1, "merchantName");
    //         Assert.That(requestToGetPaymentFundingOperations!.paymentFundingOperationResponses[0].activitiesList.Count == 2);
    //         Assert.That(requestToGetPaymentFundingOperations.paymentFundingOperationResponses[0].collectNumberOfActivities == 1);
    //         Assert.That(requestToGetPaymentFundingOperations.paymentFundingOperationResponses[0].fundNumberOfActivities == 1);
    //         var jResponseGetFailedPaymentOperations1 =
    //             await _getPaymentRecordsFunctionality.SendGetPaymentRecordRequestAsync( _requestHeader!, _ipn1);
    //         Assert.That(jResponseGetFailedPaymentOperations1.totalRecords > 0);
    //         var jResponseGetFailedPaymentOperations2 =
    //             await _getPaymentRecordsFunctionality.SendGetPaymentRecordRequestAsync( _requestHeader!, _ipn2);
    //         Assert.That(jResponseGetFailedPaymentOperations2.totalRecords > 0);
    //         var jResponseGetFailedPaymentOperations3 =
    //             await _getPaymentRecordsFunctionality.SendGetPaymentRecordRequestAsync( _requestHeader!, _ipn3);
    //         Assert.That(jResponseGetFailedPaymentOperations3.totalRecords > 0);
    //         var jResponseGetFailedPaymentOperations4 =
    //             await _getPaymentRecordsFunctionality.SendGetPaymentRecordRequestAsync( _requestHeader!, _ipn4);
    //         Assert.That(jResponseGetFailedPaymentOperations4.totalRecords > 0);
    //         var jResponseGetFailedPaymentOperations5 =
    //             await _getPaymentRecordsFunctionality.SendGetPaymentRecordRequestAsync( _requestHeader!, _ipn5);
    //         Assert.That(jResponseGetFailedPaymentOperations5.totalRecords > 0);
    //         Console.WriteLine("Done TestValidate_DBS_FUN_STL_MON_FTR_CRC_F06_PartialRefund_PastCharge_ChargeBack");
    //     }
    //     catch (Exception exception)
    //     {
    //         Assert.Fail("Error in TestValidate_DBS_FUN_STL_MON_FTR_CRC_F06_PartialRefund_PastCharge_ChargeBack \n" +
    //                     exception + "\n");
    //     }
    // }
    //
    // [TestCase(Category = "FundingOperations")]
    // [Test(Description = "TestValidate_DBS_FUN_STL_MON_FTR_CRC_F06_PartialRefund_OutstandingAmount_ChargeBack_WithWon with generated merchant"),
    //  CancelAfter(960 * 1000)]
    // public async Task TestValidate_DBS_FUN_STL_MON_FTR_CRC_F06_PartialRefund_OutstandingAmount_ChargeBack_WithWon()
    // {
    //     try
    //     {
    //         Console.WriteLine("\nStarting TestValidate_DBS_FUN_STL_MON_FTR_CRC_F06_PartialRefund_OutstandingAmount_ChargeBack_WithWon");
    //         var testName = TestContext.CurrentContext.Test.Name;
    //         (_terminalApiKey, _businessUnitId) = await _testsHelper.CreateMerchantFunctionality( _requestHeader!,
    //             testName, _db!,"PaymentWizard.v3.5", "NonSecuredPlan", "Monthly", _skuDict!, "109");
    //         Assert.That(_terminalApiKey, Is.Not.EqualTo(""));
    //         var jResponse = await _list.SendGetMerchantListRequestAsync( _requestHeader!, _businessUnitId);
    //         Console.WriteLine("Done with creation of a new merchant , settings and terminal process");
    //         var createPlanDefaultValues = new CreatePlanDefaultValues();
    //         var is3DsVarsCreation = new Is3DsVarsCreation();
    //         createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
    //         Console.WriteLine("Creating 5 plans and querying mongo");
    //         await Task.Delay(5 * 1000);
    //         (_flag, _ipn1) = await _testsHelper.CreatePlanAndQueryMongoBeforeChargeBack(_terminalApiKey, _businessUnitId, _db!,
    //              _requestHeader!, createPlanDefaultValues, 4, 6);
    //         Assert.That(_flag);
    //         await Task.Delay(5 * 1000);
    //         (_flag, _ipn2) = await _testsHelper.CreatePlanAndQueryMongoBeforeChargeBack(_terminalApiKey, _businessUnitId, _db!,
    //              _requestHeader!, createPlanDefaultValues, 4, 6);
    //         Assert.That(_flag);
    //         await Task.Delay(5 * 1000);
    //         (_flag, _ipn3) = await _testsHelper.CreatePlanAndQueryMongoBeforeChargeBack(_terminalApiKey, _businessUnitId, _db!,
    //              _requestHeader!, createPlanDefaultValues, 4, 6);
    //         Assert.That(_flag);
    //         await Task.Delay(5 * 1000);
    //         (_flag, _ipn4) = await _testsHelper.CreatePlanAndQueryMongoBeforeChargeBack(_terminalApiKey, _businessUnitId, _db!,
    //              _requestHeader!, createPlanDefaultValues, 4, 6);
    //         Assert.That(_flag);
    //         await Task.Delay(5 * 1000);
    //         (_flag, _ipn5) = await _testsHelper.CreatePlanAndQueryMongoBeforeChargeBack(_terminalApiKey, _businessUnitId, _db!,
    //              _requestHeader!, createPlanDefaultValues, 4, 6);
    //         Assert.That(_flag);
    //         Console.WriteLine("Done Creating 5 plans and querying mongo\n");
    //         Assert.That(await _testsHelper.DoingChargeBackAndQueryMongoWithWon( _requestHeader!, _ipn1,
    //             "PartialRefund", null!, _db!));
    //         var requestToGetPaymentFundingOperations =
    //             await _getPaymentFundingOperations.SendPostRequestToGetPaymentFundingOperationsAsync( _requestHeader!,
    //                 jResponse!.Merchants[0].Id.ToString(), 10, 1, "merchantName");
    //         Assert.That(requestToGetPaymentFundingOperations!.paymentFundingOperationResponses[0].activitiesList.Count == 2);
    //         Assert.That(requestToGetPaymentFundingOperations.paymentFundingOperationResponses[0].collectNumberOfActivities == 1);
    //         Assert.That(requestToGetPaymentFundingOperations.paymentFundingOperationResponses[0].fundNumberOfActivities == 1);
    //         var jResponseGetFailedPaymentOperations1 =
    //             await _getPaymentRecordsFunctionality.SendGetPaymentRecordRequestAsync( _requestHeader!, _ipn1);
    //         Assert.That(jResponseGetFailedPaymentOperations1.totalRecords > 0);
    //         var jResponseGetFailedPaymentOperations2 =
    //             await _getPaymentRecordsFunctionality.SendGetPaymentRecordRequestAsync( _requestHeader!, _ipn2);
    //         Assert.That(jResponseGetFailedPaymentOperations2.totalRecords > 0);
    //         var jResponseGetFailedPaymentOperations3 =
    //             await _getPaymentRecordsFunctionality.SendGetPaymentRecordRequestAsync( _requestHeader!, _ipn3);
    //         Assert.That(jResponseGetFailedPaymentOperations3.totalRecords > 0);
    //         var jResponseGetFailedPaymentOperations4 =
    //             await _getPaymentRecordsFunctionality.SendGetPaymentRecordRequestAsync( _requestHeader!, _ipn4);
    //         Assert.That(jResponseGetFailedPaymentOperations4.totalRecords > 0);
    //         var jResponseGetFailedPaymentOperations5 =
    //             await _getPaymentRecordsFunctionality.SendGetPaymentRecordRequestAsync( _requestHeader!, _ipn5);
    //         Assert.That(jResponseGetFailedPaymentOperations5.totalRecords > 0);
    //         Console.WriteLine("Done TestValidate_DBS_FUN_STL_MON_FTR_CRC_F06_PartialRefund_OutstandingAmount_ChargeBack_WithWon");
    //     }
    //     catch (Exception exception)
    //     {
    //         Assert.Fail(
    //             "Error in TestValidate_DBS_FUN_STL_MON_FTR_CRC_F06_PartialRefund_OutstandingAmount_ChargeBack_WithWon \n" +
    //             exception + "\n");
    //     }
    // }
    //
    // [TestCase(Category = "FundingOperations")]
    // [Test(Description = "TestValidate_DBS_FUN_STL_MON_FTR_CRC_F06_PartialRefund_PastCharge_ChargeBack_WithWon with generated merchant"),
    //  CancelAfter(960 * 1000)]
    // public async Task TestValidate_DBS_FUN_STL_MON_FTR_CRC_F06_PartialRefund_PastCharge_ChargeBack_WithWon()
    // {
    //     try
    //     {
    //         Console.WriteLine("\nStarting TestValidate_DBS_FUN_STL_MON_FTR_CRC_F06_PartialRefund_PastCharge_ChargeBack_WithWon");
    //         var testName = TestContext.CurrentContext.Test.Name;
    //         (_terminalApiKey, _businessUnitId) = await _testsHelper.CreateMerchantFunctionality( _requestHeader!,
    //             testName, _db!,"PaymentWizard.v3.5", "NonSecuredPlan", "Monthly", _skuDict!, "109");
    //         Assert.That(_terminalApiKey, Is.Not.EqualTo(""));
    //         var jResponse = await _list.SendGetMerchantListRequestAsync( _requestHeader!, _businessUnitId);
    //         Console.WriteLine("Done with creation of a new merchant , settings and terminal process");
    //         var createPlanDefaultValues = new CreatePlanDefaultValues();
    //         var is3DsVarsCreation = new Is3DsVarsCreation();
    //         createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
    //         Console.WriteLine("Creating 5 plans and querying mongo");
    //         await Task.Delay(5 * 1000);
    //         (_flag, _ipn1) = await _testsHelper.CreatePlanAndQueryMongoBeforeChargeBack(_terminalApiKey, _businessUnitId, _db!,
    //              _requestHeader!, createPlanDefaultValues, 4, 6);
    //         Assert.That(_flag);
    //         await Task.Delay(5 * 1000);
    //         (_flag, _ipn2) = await _testsHelper.CreatePlanAndQueryMongoBeforeChargeBack(_terminalApiKey, _businessUnitId, _db!,
    //              _requestHeader!, createPlanDefaultValues, 4, 6);
    //         Assert.That(_flag);
    //         await Task.Delay(5 * 1000);
    //         (_flag, _ipn3) = await _testsHelper.CreatePlanAndQueryMongoBeforeChargeBack(_terminalApiKey, _businessUnitId, _db!,
    //              _requestHeader!, createPlanDefaultValues, 4, 6);
    //         Assert.That(_flag);
    //         await Task.Delay(5 * 1000);
    //         (_flag, _ipn4) = await _testsHelper.CreatePlanAndQueryMongoBeforeChargeBack(_terminalApiKey, _businessUnitId, _db!,
    //              _requestHeader!, createPlanDefaultValues, 4, 6);
    //         Assert.That(_flag);
    //         await Task.Delay(5 * 1000);
    //         (_flag, _ipn5) = await _testsHelper.CreatePlanAndQueryMongoBeforeChargeBack(_terminalApiKey, _businessUnitId, _db!,
    //              _requestHeader!, createPlanDefaultValues, 4, 6);
    //         Assert.That(_flag);
    //         Console.WriteLine("Done Creating 5 plans and querying mongo\n");
    //         Assert.That(await _testsHelper.DoingChargeBackAndQueryMongoWithWon( _requestHeader!, _ipn1,
    //             "PartialRefund", "FutureInstallmentsNotAllowed", _db!));
    //         var requestToGetPaymentFundingOperations =
    //             await _getPaymentFundingOperations.SendPostRequestToGetPaymentFundingOperationsAsync( _requestHeader!,
    //                 jResponse!.Merchants[0].Id.ToString(), 10, 1, "merchantName");
    //         Assert.That(requestToGetPaymentFundingOperations!.paymentFundingOperationResponses[0].activitiesList.Count == 2);
    //         Assert.That(requestToGetPaymentFundingOperations.paymentFundingOperationResponses[0].collectNumberOfActivities == 1);
    //         Assert.That(requestToGetPaymentFundingOperations.paymentFundingOperationResponses[0].fundNumberOfActivities == 1);
    //         var jResponseGetFailedPaymentOperations1 =
    //             await _getPaymentRecordsFunctionality.SendGetPaymentRecordRequestAsync( _requestHeader!, _ipn1);
    //         Assert.That(jResponseGetFailedPaymentOperations1.totalRecords > 0);
    //         var jResponseGetFailedPaymentOperations2 =
    //             await _getPaymentRecordsFunctionality.SendGetPaymentRecordRequestAsync( _requestHeader!, _ipn2);
    //         Assert.That(jResponseGetFailedPaymentOperations2.totalRecords > 0);
    //         var jResponseGetFailedPaymentOperations3 =
    //             await _getPaymentRecordsFunctionality.SendGetPaymentRecordRequestAsync( _requestHeader!, _ipn3);
    //         Assert.That(jResponseGetFailedPaymentOperations3.totalRecords > 0);
    //         var jResponseGetFailedPaymentOperations4 =
    //             await _getPaymentRecordsFunctionality.SendGetPaymentRecordRequestAsync( _requestHeader!, _ipn4);
    //         Assert.That(jResponseGetFailedPaymentOperations4.totalRecords > 0);
    //         var jResponseGetFailedPaymentOperations5 =
    //             await _getPaymentRecordsFunctionality.SendGetPaymentRecordRequestAsync( _requestHeader!, _ipn5);
    //         Assert.That(jResponseGetFailedPaymentOperations5.totalRecords > 0);
    //         Console.WriteLine("Done TestValidate_DBS_FUN_STL_MON_FTR_CRC_F06_PartialRefund_PastCharge_ChargeBack_WithWon");
    //     }
    //     catch (Exception exception)
    //     {
    //         Assert.Fail("Error in TestValidate_DBS_FUN_STL_MON_FTR_CRC_F06_PartialRefund_PastCharge_ChargeBack_WithWon \n" +
    //                     exception + "\n");
    //     }
    // }
}