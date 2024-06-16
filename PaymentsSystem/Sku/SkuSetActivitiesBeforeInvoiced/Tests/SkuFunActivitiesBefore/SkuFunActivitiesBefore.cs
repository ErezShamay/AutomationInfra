using MongoDB.Driver;
using Allure.NUnit;
using Allure.NUnit.Attributes;
using NUnit.Framework;
using Splitit.Automation.NG.Backend.BaseActions;
using Splitit.Automation.NG.Backend.Services.AdminApi.InstallmentPlan.InstallmentsPlanFunctionality;
using Splitit.Automation.NG.Backend.Services.V3.DefaultValues;
using Splitit.Automation.NG.Backend.Services.V3.InstallmentPlan.InstallmentPlanApiEndPoint;
using Splitit.Automation.NG.Backend.Tests.TestsSetups;
using Splitit.Automation.NG.PaymentsSystem.PaymentProcessing.EndPointFunctionality;
using Splitit.Automation.NG.PaymentsSystem.PaymentsOperations.EndPointFunctionality;
using Splitit.Automation.NG.PaymentsSystem.SplititOperations.EndPointsFunctionality;
using Splitit.Automation.NG.Utilities.EnvironmentsConfig;

namespace Splitit.Automation.NG.PaymentsSystem.Sku.SkuSetActivitiesBeforeInvoiced.Tests.SkuFunActivitiesBefore;

[TestFixture]
[AllureNUnit]
[AllureSuite("SkuFunActivitiesBefore")]
[AllureDisplayIgnored]
public class SkuFunActivitiesBefore
{
    private RequestHeader? _requestHeader;
    private readonly InstallmentPlans _installmentPlans;
    private readonly MongoHandler _mongoHandler;
    private string? _terminalApiKey = "";
    private readonly InstallmentPlanNumberRefund _installmentPlanNumberRefund;
    private readonly Processing _processing;
    private readonly MovePlanToStd _movePlanToStd;
    private readonly Created _created;
    private readonly Settled _settled;
    private readonly ChargeFunctionality _chargeFunctionality;
    private readonly Invoiced _invoiced;
    private IMongoDatabase? _db;
    private readonly EnvConfig _envConfig;

    public SkuFunActivitiesBefore()
    {
        Console.WriteLine("Staring SKU processing Setup");
        _installmentPlans = new InstallmentPlans();
        _mongoHandler = new MongoHandler();
        _processing = new Processing();
        _created = new Created();
        _settled = new Settled();
        _installmentPlanNumberRefund = new InstallmentPlanNumberRefund();
        _movePlanToStd = new MovePlanToStd();
        _chargeFunctionality = new ChargeFunctionality();
        _invoiced = new Invoiced();
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

    [TestCase(Category = "SkuFunActivitiesBefore")]
    [Test(Description = "TestValidate_MER_FUN_STL_MON_FTR_CRC_F01_ActivitiesBefore with generated merchant"),
     CancelAfter(480 * 1000)]
    public async Task TestValidate_MER_FUN_STL_MON_FTR_CRC_F01_ActivitiesBefore()
    {
        try
        {
            Console.WriteLine("\nStarting TestValidate_MER_FUN_STL_MON_FTR_CRC_F01_ActivitiesBefore");
            var terminalApiKey = _envConfig.MER_FUN_STL_MON_FTR_CRC_F01;
            Console.WriteLine("Done with creation of a new merchant , settings and terminal process");
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            var json = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!, _requestHeader!,
                "Active", 1, terminalApiKey, createPlanDefaultValues);
            await _mongoHandler.SendMongoQueryWith2KeysAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", json.InstallmentPlanNumber, "Activity", "COL",
                "Status", "_id");
            await _mongoHandler.SendMongoQueryWith2KeysAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", json.InstallmentPlanNumber, "Activity", "FND", 
                "Status", "_id");
            var jResponse =
                await _movePlanToStd.SendPostRequestMovePlanToStdAsync(_requestHeader!, json.InstallmentPlanNumber);
            Assert.That(jResponse.Contains(""));
            await _mongoHandler.SendMongoQueryWith2KeysAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", json.InstallmentPlanNumber, "Activity", "DEL", "Status", "_id");
            var (statusCol2, idCol2) = await _mongoHandler.SendMongoQueryWith2KeysAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", json.InstallmentPlanNumber, "Activity", "COL", "Status", "_id");
            Assert.That(statusCol2, Is.Not.Null);
            Assert.That(idCol2, Is.Not.Null);
            var (statusFnd2, idFnd2) = await _mongoHandler.SendMongoQueryWith2KeysAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", json.InstallmentPlanNumber, "Activity", "FND", "Status", "_id");
            Assert.That(statusFnd2, Is.Not.Null);
            Assert.That(idFnd2, Is.Not.Null);
            var itemList = await _mongoHandler.SendMongoQueryWith2KeysAndReturnListOfResultAsync(_db!,
                "Payment_Records", "InstallmentPlanNumber", json.InstallmentPlanNumber, 
                "Activity", "COL", "Sku", "_id");
            Assert.That(_mongoHandler.ValidateValueInDocsList(itemList!, "Sku", "UNF"));
            Console.WriteLine("Done TestValidate_MER_FUN_STL_MON_FTR_CRC_F01_ActivitiesBefore");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidate_MER_FUN_STL_MON_FTR_CRC_F01_ActivitiesBefore \n" + exception + "\n");
        }
    }

    [TestCase(Category = "SkuFunActivitiesBefore")]
    [Test(Description =
         "TestValidate_MER_FUN_STL_MON_FTR_CRC_F01_ActivitiesBefore_Last_Installment with generated merchant"),
     CancelAfter(480 * 1000)]
    public async Task TestValidate_MER_FUN_STL_MON_FTR_CRC_F01_ActivitiesBefore_Last_Installment()
    {
        try
        {
            Console.WriteLine("\nStarting TestValidate_MER_FUN_STL_MON_FTR_CRC_F01_ActivitiesBefore_Last_Installment");
            _terminalApiKey = _envConfig.MER_FUN_STL_MON_FTR;
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            var json = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!, _requestHeader!,
                "Active", 1, _terminalApiKey, createPlanDefaultValues);
            await _mongoHandler.SendMongoQueryWith2KeysAsync(_db!, "Payment_Records", 
                "InstallmentPlanNumber", json.InstallmentPlanNumber, "Activity", "COL", "Status", "_id");
            await _mongoHandler.SendMongoQueryWith2KeysAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", json.InstallmentPlanNumber, "Activity", "FND", "Status", "_id");
            Console.WriteLine("Done TestValidate_MER_FUN_STL_MON_FTR_CRC_F01_ActivitiesBefore_Last_Installment");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidate_MER_FUN_STL_MON_FTR_CRC_F01_ActivitiesBefore_Last_Installment \n" +
                        exception + "\n");
        }
    }

    [TestCase(Category = "SkuFunActivitiesBefore")]
    [TestCase(Category = "PaymentSystemSmoke")]
    [Test(Description = "TestValidate_MER_FUN_STL_MON_FTR_CRC_F03_ActivitiesBefore with generated merchant"),
     CancelAfter(480 * 1000)]
    public async Task TestValidate_MER_FUN_STL_MON_FTR_CRC_F03_ActivitiesBefore()
    {
        try
        {
            Console.WriteLine("\nStarting TestValidate_MER_FUN_STL_MON_FTR_CRC_F03_ActivitiesBefore");
            var terminalApiKey = _envConfig.MER_FUN_STL_MON_FTR_CRC_F03;
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            var json = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!, _requestHeader!,
                "Active", 3, terminalApiKey, createPlanDefaultValues);
            await _mongoHandler.SendMongoQueryWith2KeysAsync(_db!,
                "Payment_Records",
                "InstallmentPlanNumber",
                json.InstallmentPlanNumber, "Activity", "COL", "Status", "_id");
            await _mongoHandler.SendMongoQueryWith2KeysAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", json.InstallmentPlanNumber, "Activity", "FND", "Status", "_id");
            var jResponse =
                await _movePlanToStd.SendPostRequestMovePlanToStdAsync(_requestHeader!, json.InstallmentPlanNumber);
            Assert.That(jResponse.Contains(""));
            await _mongoHandler.SendMongoQueryWith2KeysAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", json.InstallmentPlanNumber, "Activity", "DEL", "Status", "_id");
            var docsList = await _mongoHandler.SendMongoQueryWith2KeysAndReturnListOfResultAsync(_db!,
                "Payment_Records", "InstallmentPlanNumber", json.InstallmentPlanNumber, "Activity", "COL", "Sku", "_id");
            Assert.That(_mongoHandler.ValidateValueInDocsList(docsList!, "Sku", "UNF"));
            Console.WriteLine("Done TestValidate_MER_FUN_STL_MON_FTR_CRC_F03_ActivitiesBefore");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidate_MER_FUN_STL_MON_FTR_CRC_F03_ActivitiesBefore \n" + exception + "\n");
        }
    }

    [TestCase(Category = "SkuFunActivitiesBefore")]
    [Test(Description =
         "TestValidate_MER_FUN_STL_MON_FTR_CRC_F09_ActivitiesBefore_Red_Del_OutstandingBalanceFirst with generated merchant"),
     CancelAfter(480 * 1000)]
    public async Task TestValidate_MER_FUN_STL_MON_FTR_CRC_F09_ActivitiesBefore_Red_Del_OutstandingBalanceFirst()
    {
        string idStatusCol = null!;
        string activityStatusColB = null!;
        string idStatusFnd = null!;
        string activityStatusFndB = null!;
        string activityStatusDelB = null!;
        string idBatchDelB = null!;
        
        try
        {
            Console.WriteLine(
                "\nStarting TestValidate_MER_FUN_STL_MON_FTR_CRC_F09_ActivitiesBefore_Red_Del_OutstandingBalanceFirst");
            _terminalApiKey = _envConfig.MER_FUN_STL_MON_FTR;
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            var json = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!, _requestHeader!,
                "Active", 9, _terminalApiKey, createPlanDefaultValues);
            await _mongoHandler.SendMongoQueryWith2KeysAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", json.InstallmentPlanNumber, "Activity", "COL", "Status", "_id");
            await _mongoHandler.SendMongoQueryWith2KeysAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", json.InstallmentPlanNumber, "Activity", "FND", "Status", "_id");
            var response = await _processing.SendGetForProcessingAsync(_requestHeader!, json.InstallmentPlanNumber);
            Assert.That(response, Is.EqualTo(""));
            (_, idStatusCol) = await _mongoHandler.SendMongoQueryWith3KeysReturn2StringColumnValuesAsync(_db!,
                "Payment_Records", "InstallmentPlanNumber", json.InstallmentPlanNumber, "Activity", "COL", "Status", "PendingBatch","Status", "_id");
            (_, idStatusFnd) = await _mongoHandler.SendMongoQueryWith3KeysReturn2StringColumnValuesAsync(_db!,
                "Payment_Records", "InstallmentPlanNumber", json.InstallmentPlanNumber, "Activity", "FND", "Status", "PendingBatch", "Status", "_id");
            await _created.SendPutRequestCreatedAsync(_requestHeader!, idStatusCol);
            await _created.SendPutRequestCreatedAsync(_requestHeader!, idStatusFnd);
            (activityStatusColB, var idBatchColB) = await _mongoHandler.SendMongoQueryWith2KeysAsync(_db!,
                "Payment_Records", "InstallmentPlanNumber", json.InstallmentPlanNumber, "Activity", "COL", "BatchId", "_id");
            Assert.That(activityStatusColB, Is.Not.Null);
            Assert.That(idBatchColB, Is.Not.Null);
            (activityStatusFndB, var idBatchFndB) = await _mongoHandler.SendMongoQueryWith2KeysAsync(_db!,
                "Payment_Records", "InstallmentPlanNumber", json.InstallmentPlanNumber, "Activity", "FND", "BatchId", "_id");
            Assert.That(activityStatusFndB, Is.Not.Null);
            Assert.That(idBatchFndB, Is.Not.Null);
            await _settled.SendPutRequestSettledAsync(_requestHeader!, activityStatusColB, idStatusCol, true);
            await _settled.SendPutRequestSettledAsync(_requestHeader!, activityStatusFndB, idStatusFnd, true);
            await _mongoHandler.SendMongoQueryWith3KeysReturn2StringColumnValuesAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", json.InstallmentPlanNumber, "Activity", "COL", 
                "Status","Acknowledged", "Status", "_id");
            await _mongoHandler.SendMongoQueryWith3KeysReturn2StringColumnValuesAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", json.InstallmentPlanNumber, "Activity", "FND", 
                "Status","Acknowledged", "Status", "_id");
            await _installmentPlanNumberRefund.SendRefundRequestAsync(_requestHeader!, json.InstallmentPlanNumber,
                "PartialRefund");
            await _mongoHandler.SendMongoQueryWith2KeysAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", json.InstallmentPlanNumber, "Activity", "RED", "Status", "_id");
            var responseGetForProcessing = await _processing.SendGetForProcessingAsync(_requestHeader!, json.InstallmentPlanNumber);
            Assert.That(responseGetForProcessing, Is.EqualTo("")); 
            await _mongoHandler.SendMongoQueryWith3KeysReturn2StringColumnValuesAsync(_db!,
                "Payment_Records", "InstallmentPlanNumber", json.InstallmentPlanNumber, "Activity", "DEL", "Status", "PendingBatch","Status", "_id");
            var docsListCol = await _mongoHandler.SendMongoQueryWith2KeysAndReturnListOfResultAsync(_db!,
                "Payment_Records", "InstallmentPlanNumber", json.InstallmentPlanNumber, "Activity", "COL", "Sku", "_id");
            Assert.That(_mongoHandler.ValidateValueInDocsList(docsListCol!, "Status", "New"));
            var docsListFnd = await _mongoHandler.SendMongoQueryWith2KeysAndReturnListOfResultAsync(_db!,
                "Payment_Records", "InstallmentPlanNumber", json.InstallmentPlanNumber, "Activity", "FND", "Sku", "_id");
            Assert.That(_mongoHandler.ValidateValueInDocsList(docsListFnd!, "Status", "New"));
            var idStatusColNew = _mongoHandler.ReturnValueFromDocsList(docsListCol!, "Status", "New", "_id");
            var idStatusFndNew = _mongoHandler.ReturnValueFromDocsList(docsListFnd!, "Status", "New", "_id");
            Assert.That(idStatusColNew, Is.Not.Null);
            Assert.That(idStatusFndNew, Is.Not.Null); 
            await _mongoHandler.SendMongoQueryWith2KeysAsync(_db!, "Plan_PaymentSettings", "InstallmentPlanNumber", json.InstallmentPlanNumber,
                "PendingDeletion", "true", "Sku", "_id");
            var (_, idStatusDel) = await _mongoHandler.SendMongoQueryWith2KeysAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", json.InstallmentPlanNumber, "Activity", "DEL", "Status", "_id");
            await _created.SendPutRequestCreatedAsync(_requestHeader!, idStatusDel);
            await _mongoHandler.SendMongoQueryWith2KeysAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", json.InstallmentPlanNumber, "Activity", "DEL", "BatchId", "_id");
            await _settled.SendPutRequestSettledAsync(_requestHeader!, activityStatusDelB, idBatchDelB, true);
            await _mongoHandler.SendMongoQueryWith3KeysReturn2StringColumnValuesAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", json.InstallmentPlanNumber, "Activity", "DEL",
                "Status","Acknowledged","Status", "_id");
            var docsListCol1 = await _mongoHandler.SendMongoQueryWith2KeysAndReturnListOfResultAsync(_db!,
                "Payment_Records", "InstallmentPlanNumber",
                json.InstallmentPlanNumber, "Activity", "COL", "Sku", "_id");
            var idStatusColNew1 = _mongoHandler.ReturnValueFromDocsList(docsListCol1!, "Status", "New", "IsDeleted");
            Assert.That(idStatusColNew1, Is.EqualTo("false"));
            var docsListFnd1 = await _mongoHandler.SendMongoQueryWith2KeysAndReturnListOfResultAsync(_db!,
                "Payment_Records", "InstallmentPlanNumber",
                json.InstallmentPlanNumber, "Activity", "FND", "Sku", "_id");
            var idStatusFndNew1 = _mongoHandler.ReturnValueFromDocsList(docsListFnd1!, "Status", "New", "IsDeleted");
            Assert.That(idStatusFndNew1, Is.EqualTo("false"));
            var docsListDel = await _mongoHandler.SendMongoQueryWith2KeysAndReturnListOfResultAsync(_db!,
                "Payment_Records", "InstallmentPlanNumber",
                json.InstallmentPlanNumber, "Activity", "DEL", "IsDeleted", "_id");
            var statusDelAck = _mongoHandler.ReturnValueFromDocsList(docsListDel!, "Status", "Acknowledged", "IsDeleted");
            Assert.That(statusDelAck, Is.EqualTo("false"));
            var responseGetForProcessingAfter = await _processing.SendGetForProcessingAsync(_requestHeader!, json.InstallmentPlanNumber);
            Assert.That(responseGetForProcessingAfter, Is.EqualTo(""));
            var docsListColAfter = await _mongoHandler.SendMongoQueryWith2KeysAndReturnListOfResultAsync(_db!,
                "Payment_Records", "InstallmentPlanNumber",
                json.InstallmentPlanNumber, "Activity", "COL", "Status", "_id");
            var statusColAfter =
                _mongoHandler.ReturnValueFromDocsList(docsListColAfter!, "Status", "PendingBatch", "Status");
            Assert.That(statusColAfter, Is.EqualTo("PendingBatch"));
            var docsListFndAfter = await _mongoHandler.SendMongoQueryWith2KeysAndReturnListOfResultAsync(_db!,
                "Payment_Records", "InstallmentPlanNumber",
                json.InstallmentPlanNumber, "Activity", "FND", "Status", "_id");
            var statusFndAfter = _mongoHandler.ReturnValueFromDocsList(docsListFndAfter!, "Status", "PendingBatch", "Status");
            Assert.That(statusFndAfter, Is.EqualTo("PendingBatch"));
            var jResponse = await _chargeFunctionality.SendPostRequestChargeFunctionalityAsync(
                _requestHeader!, json.InstallmentPlanNumber);
            Assert.That(jResponse.ResponseHeader.Succeeded);
            Console.WriteLine("Done TestValidate_MER_FUN_STL_MON_FTR_CRC_F09_ActivitiesBefore_Red_Del_OutstandingBalanceFirst");
        }
        catch (Exception exception)
        {
            await Task.Delay(10 * 1000);
            await _settled.SendPutRequestSettledAsync(_requestHeader!, activityStatusColB, idStatusCol, false);
            await _settled.SendPutRequestSettledAsync(_requestHeader!, activityStatusFndB, idStatusFnd, false);
            await _settled.SendPutRequestSettledAsync(_requestHeader!, activityStatusDelB, idBatchDelB, false);
            Assert.Fail("Error in TestValidate_MER_FUN_STL_MON_FTR_CRC_F09_ActivitiesBefore_Red_Del_OutstandingBalanceFirst \n" +
                        exception + "\n");
        }
    }

    [TestCase(Category = "SkuFunActivitiesBefore")]
    [Test(Description = "TestValidate_MER_FUN_STL_MON_FTR_CRC_F09_ActivitiesBefore_Red_Del_LastInstallment with generated merchant"),
     CancelAfter(480 * 1000)]
    public async Task TestValidate_MER_FUN_STL_MON_FTR_CRC_F09_ActivitiesBefore_Red_Del_LastInstallment()
    {
        string idStatusCol = null!;
        string activityStatusColB = null!;
        string idStatusFnd = null!;
        string activityStatusFndB = null!;
        string activityStatusDelB = null!;
        string idBatchDelB = null!;
        
        try
        {
            Console.WriteLine(
                "\nStarting TestValidate_MER_FUN_STL_MON_FTR_CRC_F09_ActivitiesBefore_Red_Del_LastInstallment");
            _terminalApiKey = _envConfig.MER_FUN_STL_MON_FTR;
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            var json = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!, _requestHeader!,
                "Active", 9, _terminalApiKey, createPlanDefaultValues);
            var (_, idStatusCol1Before) = await _mongoHandler.SendMongoQueryWith2KeysAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", json.InstallmentPlanNumber, "Activity", "COL", "Status", "_id");
            Assert.That(idStatusCol1Before, Is.Not.Null);
            await _mongoHandler.SendMongoQueryWith2KeysAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", json.InstallmentPlanNumber, "Activity", "FND", "Status", "_id");
            var response = await _processing.SendGetForProcessingAsync(_requestHeader!, json.InstallmentPlanNumber);
            Assert.That(response, Is.EqualTo(""));
            (var _, idStatusCol) = await _mongoHandler.SendMongoQueryWith3KeysReturn2StringColumnValuesAsync(_db!,
                "Payment_Records", "InstallmentPlanNumber", json.InstallmentPlanNumber, "Activity", "COL", "Status", "PendingBatch","Status", "_id");
            (var _, idStatusFnd) = await _mongoHandler.SendMongoQueryWith3KeysReturn2StringColumnValuesAsync(_db!,
                "Payment_Records", "InstallmentPlanNumber",
                json.InstallmentPlanNumber, "Activity", "FND", "Status", "PendingBatch", "Status", "_id");
            await _created.SendPutRequestCreatedAsync(_requestHeader!, idStatusCol);
            await _created.SendPutRequestCreatedAsync(_requestHeader!, idStatusFnd);
            (activityStatusColB, var _) = await _mongoHandler.SendMongoQueryWith2KeysAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", json.InstallmentPlanNumber, "Activity", "COL", "BatchId", "_id");
            (activityStatusFndB, var _) = await _mongoHandler.SendMongoQueryWith2KeysAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", json.InstallmentPlanNumber, "Activity", "FND", "BatchId", "_id");
            await _settled.SendPutRequestSettledAsync(_requestHeader!, activityStatusColB, idStatusCol, true);
            await _settled.SendPutRequestSettledAsync(_requestHeader!, activityStatusFndB, idStatusFnd, true);
            await _mongoHandler.SendMongoQueryWith3KeysReturn2StringColumnValuesAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", json.InstallmentPlanNumber, "Activity", "COL", 
                "Status","Acknowledged", "Status", "_id");
            await _mongoHandler.SendMongoQueryWith3KeysReturn2StringColumnValuesAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", json.InstallmentPlanNumber, "Activity", "FND", 
                "Status","Acknowledged", "Status", "_id");
            await _installmentPlanNumberRefund.SendRefundRequestAsync(_requestHeader!, json.InstallmentPlanNumber,
                "PartialRefund", "ReduceFromLastInstallment");
            await _mongoHandler.SendMongoQueryWith2KeysAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", json.InstallmentPlanNumber, "Activity", "RED", "Status", "_id");
            var responseGetForProcessing = await _processing.SendGetForProcessingAsync(_requestHeader!, json.InstallmentPlanNumber);
            Assert.That(responseGetForProcessing, Is.EqualTo(""));
            await _mongoHandler.SendMongoQueryWith3KeysReturn2StringColumnValuesAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", json.InstallmentPlanNumber, "Activity", "DEL", "Status", "PendingBatch","Status", "_id");
            var docsListCol = await _mongoHandler.SendMongoQueryWith2KeysAndReturnListOfResultAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", json.InstallmentPlanNumber, "Activity", "COL", "Sku", "_id");
            Assert.That(_mongoHandler.ValidateValueInDocsList(docsListCol!, "Status", "New"));
            var docsListFnd = await _mongoHandler.SendMongoQueryWith2KeysAndReturnListOfResultAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", json.InstallmentPlanNumber, "Activity", "FND", "Sku", "_id");
            Assert.That(_mongoHandler.ValidateValueInDocsList(docsListFnd!, "Status", "New"));
            _mongoHandler.ReturnValueFromDocsList(docsListCol!, "Status", "New", "_id");
            _mongoHandler.ReturnValueFromDocsList(docsListFnd!, "Status", "New", "_id");
            await _mongoHandler.SendMongoQueryWith2KeysAsync(_db!,
                "Plan_PaymentSettings", "InstallmentPlanNumber", json.InstallmentPlanNumber,
                "PendingDeletion", "true", "Sku", "_id");
            var (_, idStatusDel) = await _mongoHandler.SendMongoQueryWith2KeysAsync(_db!, "Payment_Records", 
                "InstallmentPlanNumber", json.InstallmentPlanNumber, "Activity", "DEL", "Status", "_id");
            await _created.SendPutRequestCreatedAsync(_requestHeader!, idStatusDel);
            (activityStatusDelB, idBatchDelB) = await _mongoHandler.SendMongoQueryWith2KeysAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", json.InstallmentPlanNumber, "Activity", "DEL", "BatchId", "_id");
            await _settled.SendPutRequestSettledAsync(_requestHeader!, activityStatusDelB, idBatchDelB, true);
            await _mongoHandler.SendMongoQueryWith3KeysReturn2StringColumnValuesAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", json.InstallmentPlanNumber, "Activity", "DEL", "Status", "Acknowledged", "Status", "_id");
            var docsListCol1 = await _mongoHandler.SendMongoQueryWith2KeysAndReturnListOfResultAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", json.InstallmentPlanNumber, "Activity", "COL", "Sku", "_id");
            var idStatusColNew1 = _mongoHandler.ReturnValueFromDocsList(docsListCol1!, "Status", "New", "IsDeleted");
            Assert.That(idStatusColNew1, Is.EqualTo("false"));
            var docsListFnd1 = await _mongoHandler.SendMongoQueryWith2KeysAndReturnListOfResultAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", json.InstallmentPlanNumber, "Activity", "FND", "Sku", "_id");
            var idStatusFndNew1 = _mongoHandler.ReturnValueFromDocsList(docsListFnd1!, "Status", "New", "IsDeleted");
            Assert.That(idStatusFndNew1, Is.EqualTo("false"));
            var docsListDel = await _mongoHandler.SendMongoQueryWith2KeysAndReturnListOfResultAsync(_db!, "Payment_Records", 
                "InstallmentPlanNumber", json.InstallmentPlanNumber, "Activity", "DEL", "IsDeleted", "_id");
            var statusDelAck = _mongoHandler.ReturnValueFromDocsList(docsListDel!, "Status", "Acknowledged", "IsDeleted");
            Assert.That(statusDelAck, Is.EqualTo("false"));
            var responseGetForProcessingAfter = await _processing.SendGetForProcessingAsync(_requestHeader!, json.InstallmentPlanNumber);
            Assert.That(responseGetForProcessingAfter, Is.EqualTo(""));
            var docsListColAfter = await _mongoHandler.SendMongoQueryWith2KeysAndReturnListOfResultAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", json.InstallmentPlanNumber, "Activity", "COL", "Status", "_id");
            var statusColAfter = _mongoHandler.ReturnValueFromDocsList(docsListColAfter!, "Status", "PendingBatch", "Status");
            Assert.That(statusColAfter, Is.EqualTo("PendingBatch"));
            var docsListFndAfter = await _mongoHandler.SendMongoQueryWith2KeysAndReturnListOfResultAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", json.InstallmentPlanNumber, "Activity", "FND", "Status", "_id");
            var statusFndAfter = _mongoHandler.ReturnValueFromDocsList(docsListFndAfter!, "Status", "PendingBatch", "Status");
            Assert.That(statusFndAfter, Is.EqualTo("PendingBatch"));
            Console.WriteLine("Done TestValidate_MER_FUN_STL_MON_FTR_CRC_F09_ActivitiesBefore_Red_Del_LastInstallment");
        }
        catch (Exception exception)
        {
            await Task.Delay(10 * 1000);
            await _settled.SendPutRequestSettledAsync(_requestHeader!, activityStatusColB, idStatusCol, false);
            await _settled.SendPutRequestSettledAsync(_requestHeader!, activityStatusFndB, idStatusFnd, false);
            await _settled.SendPutRequestSettledAsync(_requestHeader!, activityStatusDelB, idBatchDelB, false);
            Assert.Fail(
                "Error in TestValidate_MER_FUN_STL_MON_FTR_CRC_F09_ActivitiesBefore_Red_Del_LastInstallment \n" +
                exception + "\n");
        }
    }

    [TestCase(Category = "SkuFunActivitiesBefore")]
    [Test(Description =
         "TestValidate_MER_FUN_STL_MON_FTR_CRC_F09_ActivitiesBefore_Red with generated merchant"),
     CancelAfter(480 * 1000)]
    public async Task TestValidate_MER_FUN_STL_MON_FTR_CRC_F09_ActivitiesBefore_Red()
    {
        try
        {
            Console.WriteLine(
                "\nStarting TestValidate_MER_FUN_STL_MON_FTR_CRC_F09_ActivitiesBefore_Red");
            _terminalApiKey = _envConfig.MER_FUN_STL_MON_FTR;
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            var json = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!, _requestHeader!,
                "Active", 9, _terminalApiKey, createPlanDefaultValues);
            await _mongoHandler.SendMongoQueryWith2KeysAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", json.InstallmentPlanNumber, "Activity", "COL", "Status", "_id");
            await _mongoHandler.SendMongoQueryWith2KeysAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", json.InstallmentPlanNumber, "Activity", "FND", "Status", "_id");
            await _installmentPlanNumberRefund.SendRefundRequestAsync(_requestHeader!, json.InstallmentPlanNumber,
                "PartialRefund");
            await _mongoHandler.SendMongoQueryWith2KeysAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", json.InstallmentPlanNumber, "Activity", "RED", "Status", "_id");
            Console.WriteLine("Done TestValidate_MER_FUN_STL_MON_FTR_CRC_F09_ActivitiesBefore_Red");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidate_MER_FUN_STL_MON_FTR_CRC_F09_ActivitiesBefore_Red \n" + exception + "\n");
        }
    }

    [TestCase(Category = "SkuFunActivitiesBefore")]
    [Test(Description =
         "TestValidate_MER_FUN_STL_MON_FTR_DRC_F12_ActivitiesBefore_PartialRefund_OutstandingAmount with generated merchant"),
     CancelAfter(480 * 1000)]
    public async Task TestValidate_MER_FUN_STL_MON_FTR_DRC_F12_ActivitiesBefore_PartialRefund_OutstandingAmount()
    {
        string idStatusCol = null!;
        string activityStatusColB = null!;
        string idStatusFnd = null!;
        string activityStatusFndB = null!;
        
        try
        {
            Console.WriteLine(
                "\nStarting TestValidate_MER_FUN_STL_MON_FTR_DRC_F12_ActivitiesBefore_PartialRefund_OutstandingAmount");
            _terminalApiKey = _envConfig.MER_FUN_STL_MON_FTR;
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            var json = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!, _requestHeader!,
                "Active", 12, _terminalApiKey, createPlanDefaultValues);
            await _mongoHandler.SendMongoQueryWith2KeysAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", json.InstallmentPlanNumber, "Activity", "COL", "Status", "_id");
            await _mongoHandler.SendMongoQueryWith2KeysAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", json.InstallmentPlanNumber, "Activity", "FND", "Status", "_id");
            var responseGetForProcessingProcessing = await _processing.SendGetForProcessingAsync(_requestHeader!, json.InstallmentPlanNumber);
            Assert.That(responseGetForProcessingProcessing, Is.Not.Null);
            (_, idStatusCol) = await _mongoHandler.SendMongoQueryWith3KeysReturn2StringColumnValuesAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", json.InstallmentPlanNumber, "Activity", "COL", "Status", "PendingBatch","Status", "_id");
            (_, idStatusFnd) = await _mongoHandler.SendMongoQueryWith3KeysReturn2StringColumnValuesAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", json.InstallmentPlanNumber, "Activity", "FND", "Status", "PendingBatch", "Status", "_id");
            await _created.SendPutRequestCreatedAsync(_requestHeader!, idStatusCol);
            await _created.SendPutRequestCreatedAsync(_requestHeader!, idStatusFnd);
            (activityStatusColB, _) = await _mongoHandler.SendMongoQueryWith2KeysAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", json.InstallmentPlanNumber, "Activity", "COL", "BatchId", "_id");
            (activityStatusFndB, _) = await _mongoHandler.SendMongoQueryWith2KeysAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", json.InstallmentPlanNumber, "Activity", "FND", "BatchId", "_id");
            await _settled.SendPutRequestSettledAsync(_requestHeader!, activityStatusColB, idStatusCol, true);
            await _settled.SendPutRequestSettledAsync(_requestHeader!, activityStatusFndB, idStatusFnd, true);
            await _mongoHandler.SendMongoQueryWith3KeysReturn2StringColumnValuesAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", json.InstallmentPlanNumber, "Activity", "COL", 
                "Status","Acknowledged", "Status", "_id");
            await _mongoHandler.SendMongoQueryWith3KeysReturn2StringColumnValuesAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", json.InstallmentPlanNumber, "Activity", "FND", 
                "Status","Acknowledged", "Status", "_id");
            await _installmentPlanNumberRefund.SendRefundRequestAsync(_requestHeader!, json.InstallmentPlanNumber,
                "PartialRefund");
            await _mongoHandler.SendMongoQueryWith2KeysAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", json.InstallmentPlanNumber, "Activity", "RED", "Status", "_id");
            var response = await _processing.SendGetForProcessingAsync(_requestHeader!, json.InstallmentPlanNumber);
            Assert.That(response, Is.Not.Null);
            await _mongoHandler.SendMongoQueryWith3KeysReturn2StringColumnValuesAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", json.InstallmentPlanNumber, "Activity", "DEL", "Status", "PendingBatch","Status", "_id");
            var docsListColNew = await _mongoHandler.SendMongoQueryWith2KeysAndReturnListOfResultAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", json.InstallmentPlanNumber, "Activity", "COL", "Status", "_id");
            Assert.That(_mongoHandler.ValidateValueInDocsList(docsListColNew!, "Status", "New"));
            Assert.That(_mongoHandler.ValidateValueInDocsList(docsListColNew!, "IsDeleted", "true"));
            var docsListFndNew = await _mongoHandler.SendMongoQueryWith2KeysAndReturnListOfResultAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", json.InstallmentPlanNumber, "Activity", "FND", "Status", "_id");
            Assert.That(_mongoHandler.ValidateValueInDocsList(docsListFndNew!, "Status", "New"));
            Assert.That(_mongoHandler.ValidateValueInDocsList(docsListFndNew!, "IsDeleted", "true"));
            await _mongoHandler.SendMongoQueryWith2KeysAsync(_db!, "Plan_PaymentSettings",
                "InstallmentPlanNumber", json.InstallmentPlanNumber, "PendingDeletion", "true", "IsDeleted", "_id");
            var docFound = _mongoHandler.ReturnBsonDocumentAfterQueryAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", json.InstallmentPlanNumber, "Activity", "DEL", "Status", "PendingBatch");
            Assert.That(_mongoHandler.SetValueForGivenKeyInMongoDocument(_db!, "Payment_Records", await docFound,
                "AcknowledgedWithError", "true"));
            var docsListColNewAfterSetError = await _mongoHandler.SendMongoQueryWith2KeysAndReturnListOfResultAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", json.InstallmentPlanNumber, "Activity", "COL", "Status", "_id");
            Assert.That(_mongoHandler.ValidateValueInDocsList(docsListColNewAfterSetError!, "Status", "New"));
            Assert.That(_mongoHandler.ValidateValueInDocsList(docsListColNewAfterSetError!, "IsDeleted", "true"));
            var docsListFndNewAfterSetError = await _mongoHandler.SendMongoQueryWith2KeysAndReturnListOfResultAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", json.InstallmentPlanNumber, "Activity", "FND", "Status", "_id");
            Assert.That(_mongoHandler.ValidateValueInDocsList(docsListFndNewAfterSetError!, "Status", "New"));
            Assert.That(_mongoHandler.ValidateValueInDocsList(docsListFndNewAfterSetError!, "IsDeleted", "true"));
            await _mongoHandler.SendMongoQueryWith2KeysAsync(_db!, "Plan_PaymentSettings",
                    "InstallmentPlanNumber", json.InstallmentPlanNumber, "PendingDeletion", "true", "IsDeleted", "_id");
            var responseGetForProcessing = await _processing.SendGetForProcessingAsync(_requestHeader!, json.InstallmentPlanNumber);
            Assert.That(responseGetForProcessing, Is.Not.Null);
            await _mongoHandler.SendMongoQueryWith2KeysAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", json.InstallmentPlanNumber, "Activity", "RED", "Status", "_id");
            var jResponse = await _chargeFunctionality.SendPostRequestChargeFunctionalityAsync(_requestHeader!,
                    json.InstallmentPlanNumber);
            Assert.That(jResponse.ResponseHeader.Succeeded);
            var docsListColNewAfterProcessing = await _mongoHandler.SendMongoQueryWith2KeysAndReturnListOfResultAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", json.InstallmentPlanNumber, "Activity", "COL", "Status", "_id");
            Assert.That(_mongoHandler.ValidateValueInDocsList(docsListColNewAfterProcessing!, "Status", "New"));
            Assert.That(_mongoHandler.ValidateValueInDocsList(docsListFndNewAfterSetError!, "IsDeleted", "false"));
            Console.WriteLine(
                "Done TestValidate_MER_FUN_STL_MON_FTR_DRC_F12_ActivitiesBefore_PartialRefund_OutstandingAmount");
        }
        catch (Exception exception)
        {
            await Task.Delay(10 * 1000);
            await _settled.SendPutRequestSettledAsync(_requestHeader!, activityStatusColB, idStatusCol, false);
            await _settled.SendPutRequestSettledAsync(_requestHeader!, activityStatusFndB, idStatusFnd, false);
            Assert.Fail(
                "Error in TestValidate_MER_FUN_STL_MON_FTR_DRC_F12_ActivitiesBefore_PartialRefund_OutstandingAmount \n" +
                exception + "\n");
        }
    }


    [TestCase(Category = "SkuFunActivitiesBefore")]
    [Test(Description =
         "TestValidate_MER_FUN_STL_MON_FTR_DRC_F12_ActivitiesBefore_PartialRefund_LastInstallment with generated merchant"),
     CancelAfter(480 * 1000)]
    public async Task TestValidate_MER_FUN_STL_MON_FTR_DRC_F12_ActivitiesBefore_PartialRefund_LastInstallment()
    {
        string idStatusCol = null!;
        string activityStatusColB = null!;
        string idStatusFnd = null!;
        string activityStatusFndB = null!;
        
        try
        {
            Console.WriteLine(
                "\nStarting TestValidate_MER_FUN_STL_MON_FTR_DRC_F12_ActivitiesBefore_PartialRefund_LastInstallment");
            _terminalApiKey = _envConfig.MER_FUN_STL_MON_FTR;
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            var json = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!, _requestHeader!,
                "Active", 12, _terminalApiKey, createPlanDefaultValues);
            await _mongoHandler.SendMongoQueryWith2KeysAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", json.InstallmentPlanNumber, "Activity", "COL", "Status", "_id");
            await _mongoHandler.SendMongoQueryWith2KeysAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", json.InstallmentPlanNumber, "Activity", "FND", "Status", "_id");
            var responseGetForProcessingProcessing = await _processing.SendGetForProcessingAsync(_requestHeader!, json.InstallmentPlanNumber);
            Assert.That(responseGetForProcessingProcessing, Is.Not.Null);
            (_, idStatusCol) = await _mongoHandler.SendMongoQueryWith3KeysReturn2StringColumnValuesAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", json.InstallmentPlanNumber, "Activity", "COL", "Status", "PendingBatch","Status", "_id");
            (_, idStatusFnd) = await _mongoHandler.SendMongoQueryWith3KeysReturn2StringColumnValuesAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", json.InstallmentPlanNumber, "Activity", "FND", "Status", "PendingBatch","Status", "_id");
            await _created.SendPutRequestCreatedAsync(_requestHeader!, idStatusCol);
            await _created.SendPutRequestCreatedAsync(_requestHeader!, idStatusFnd);
            (activityStatusColB, var idBatchColB) = await _mongoHandler.SendMongoQueryWith2KeysAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", json.InstallmentPlanNumber, "Activity", "COL", "BatchId", "_id");
            Assert.That(activityStatusColB, Is.Not.Null);
            Assert.That(idBatchColB, Is.Not.Null);
            (activityStatusFndB, var idBatchFndB) = await _mongoHandler.SendMongoQueryWith2KeysAsync(_db!, "Payment_Records", 
                "InstallmentPlanNumber", json.InstallmentPlanNumber, "Activity", "FND", "BatchId", "_id");
            Assert.That(activityStatusFndB, Is.Not.Null);
            Assert.That(idBatchFndB, Is.Not.Null);
            await _settled.SendPutRequestSettledAsync(_requestHeader!, activityStatusColB, idStatusCol, true);
            await _settled.SendPutRequestSettledAsync(_requestHeader!, activityStatusFndB, idStatusFnd, true);
            await _mongoHandler.SendMongoQueryWith3KeysReturn2StringColumnValuesAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", json.InstallmentPlanNumber, "Activity", "COL", 
                "Status","Acknowledged", "Status", "_id");
            await _mongoHandler.SendMongoQueryWith3KeysReturn2StringColumnValuesAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", json.InstallmentPlanNumber, "Activity", "FND", 
                "Status","Acknowledged", "Status", "_id");
            await _installmentPlanNumberRefund.SendRefundRequestAsync(_requestHeader!, json.InstallmentPlanNumber,
                "PartialRefund", "ReduceFromLastInstallment");
            await _mongoHandler.SendMongoQueryWith2KeysAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", json.InstallmentPlanNumber, "Activity", "RED", "Status", "_id");
            var response = await _processing.SendGetForProcessingAsync(_requestHeader!, json.InstallmentPlanNumber);
            Assert.That(response, Is.Not.Null);
            await _mongoHandler.SendMongoQueryWith3KeysReturn2StringColumnValuesAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", json.InstallmentPlanNumber, "Activity", "DEL", "Status", "PendingBatch","Status", "_id");
            var docsListColNew = await _mongoHandler.SendMongoQueryWith2KeysAndReturnListOfResultAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", json.InstallmentPlanNumber, "Activity", "COL", "Status", "_id");
            Assert.That(_mongoHandler.ValidateValueInDocsList(docsListColNew!, "Status", "New"));
            Assert.That(_mongoHandler.ValidateValueInDocsList(docsListColNew!, "IsDeleted", "true"));
            var docsListFndNew = await _mongoHandler.SendMongoQueryWith2KeysAndReturnListOfResultAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", json.InstallmentPlanNumber, "Activity", "FND", "Status", "_id");
            Assert.That(_mongoHandler.ValidateValueInDocsList(docsListFndNew!, "Status", "New"));
            Assert.That(_mongoHandler.ValidateValueInDocsList(docsListFndNew!, "IsDeleted", "true"));
            await _mongoHandler.SendMongoQueryWith2KeysAsync(_db!, "Plan_PaymentSettings",
                "InstallmentPlanNumber", json.InstallmentPlanNumber,
                "PendingDeletion", "true", "IsDeleted", "_id");
            var docFound = await _mongoHandler.ReturnBsonDocumentAfterQueryAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", json.InstallmentPlanNumber, "Activity", "DEL", "Status", "PendingBatch");
            Assert.That(_mongoHandler.SetValueForGivenKeyInMongoDocument(_db!, "Payment_Records", docFound,
                "AcknowledgedWithError", "true"));
            var docsListColNewAfterSetError = await _mongoHandler.SendMongoQueryWith2KeysAndReturnListOfResultAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", json.InstallmentPlanNumber, "Activity", "COL", "Status", "_id");
            Assert.That(_mongoHandler.ValidateValueInDocsList(docsListColNewAfterSetError!, "Status", "New"));
            Assert.That(_mongoHandler.ValidateValueInDocsList(docsListColNewAfterSetError!, "IsDeleted", "true"));
            var docsListFndNewAfterSetError = await _mongoHandler.SendMongoQueryWith2KeysAndReturnListOfResultAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", json.InstallmentPlanNumber, "Activity", "FND", "Status", "_id");
            Assert.That(_mongoHandler.ValidateValueInDocsList(docsListFndNewAfterSetError!, "Status", "New"));
            Assert.That(_mongoHandler.ValidateValueInDocsList(docsListFndNewAfterSetError!, "IsDeleted", "true"));
            await _mongoHandler.SendMongoQueryWith2KeysAsync(_db!, "Plan_PaymentSettings",
                    "InstallmentPlanNumber", json.InstallmentPlanNumber, "PendingDeletion", "true", "IsDeleted", "_id");
            var responseGetForProcessing = await _processing.SendGetForProcessingAsync(_requestHeader!, json.InstallmentPlanNumber);
            Assert.That(responseGetForProcessing, Is.Not.Null);
            await _mongoHandler.SendMongoQueryWith2KeysAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", json.InstallmentPlanNumber, "Activity", "RED", "Status", "_id");
            var jResponse =
                await _chargeFunctionality.SendPostRequestChargeFunctionalityAsync(_requestHeader!,
                    json.InstallmentPlanNumber);
            Assert.That(jResponse.ResponseHeader.Succeeded);
            var docsListColNewAfterProcessing = await _mongoHandler.SendMongoQueryWith2KeysAndReturnListOfResultAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", json.InstallmentPlanNumber, "Activity", "COL", "Status", "_id");
            Assert.That(_mongoHandler.ValidateValueInDocsList(docsListColNewAfterProcessing!, "Status", "New"));
            Assert.That(_mongoHandler.ValidateValueInDocsList(docsListFndNewAfterSetError!, "IsDeleted", "false"));
            Console.WriteLine("Done TestValidate_MER_FUN_STL_MON_FTR_DRC_F12_ActivitiesBefore_PartialRefund_LastInstallment");
        }
        catch (Exception exception)
        {
            await Task.Delay(10 * 1000);
            await _settled.SendPutRequestSettledAsync(_requestHeader!, activityStatusColB, idStatusCol, false);
            await _settled.SendPutRequestSettledAsync(_requestHeader!, activityStatusFndB, idStatusFnd, false);
            Assert.Fail("Error in TestValidate_MER_FUN_STL_MON_FTR_DRC_F12_ActivitiesBefore_PartialRefund_LastInstallment \n" +
                        exception + "\n");
        }
    }

    [TestCase(Category = "SkuFunActivitiesBefore")]
    [Test(Description = "TestValidate_MER_FUN_STL_MON_FTR_DRC_F12_ActivitiesBefore_FullRefund with generated merchant"),
     CancelAfter(480 * 1000)]
    public async Task TestValidate_MER_FUN_STL_MON_FTR_DRC_F12_ActivitiesBefore_FullRefund()
    {
        try
        {
            Console.WriteLine("\nStarting TestValidate_MER_FUN_STL_MON_FTR_DRC_F12_ActivitiesBefore_FullRefund");
            _terminalApiKey = _envConfig.MER_FUN_STL_MON_FTR;
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            var json = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!, _requestHeader!,
                "Active", 12, _terminalApiKey, createPlanDefaultValues);
            var (_, idStatusColBefore) = await _mongoHandler.SendMongoQueryWith2KeysAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", json.InstallmentPlanNumber, "Activity", "COL", "Status", "_id");
            var (_, idStatusFndBefore) = await _mongoHandler.SendMongoQueryWith2KeysAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", json.InstallmentPlanNumber, "Activity", "FND", "Status", "_id");
            var docFoundCol = _mongoHandler.ReturnBsonDocumentAfterQueryAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", json.InstallmentPlanNumber, "Activity", "COL", "Status", "New");
            Assert.That(_mongoHandler.SetValueForGivenKeyInMongoDocument(_db!, "Payment_Records", await docFoundCol,
                "Status", "Acknowledged"));
            var docFoundFnd = _mongoHandler.ReturnBsonDocumentAfterQueryAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", json.InstallmentPlanNumber, "Activity", "FND", "Status", "New");
            Assert.That(_mongoHandler.SetValueForGivenKeyInMongoDocument(_db!, "Payment_Records", await docFoundFnd,
                "Status", "Acknowledged"));
            await _mongoHandler.SendMongoQueryWith2KeysAsync(_db!, "Plan_PaymentSettings",
                    "InstallmentPlanNumber", json.InstallmentPlanNumber, "PendingDeletion", "false", "IsDeleted", "_id");
            await _installmentPlanNumberRefund.SendRefundRequestAsync(_requestHeader!, json.InstallmentPlanNumber,
                "FullRefund");
            var (_, idStatusRedAfter) = await _mongoHandler.SendMongoQueryWith2KeysAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", json.InstallmentPlanNumber, "Activity", "RED", "Status", "_id");
            var response = await _processing.SendGetForProcessingAsync(_requestHeader!, json.InstallmentPlanNumber);
            Assert.That(response, Is.Not.Null);
            var docsListFnd = await _mongoHandler.SendMongoQueryWith2KeysAndReturnListOfResultAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", json.InstallmentPlanNumber, "Activity", "FND", "Status", "_id");
            if (docsListFnd![0]["Status"].ToString()!.Equals("PendingBatch") ||
                docsListFnd[0]["Status"].ToString()!.Equals("Acknowledged"))
            {
                Console.WriteLine("FND is in the right status");
            }
            else
            {
                Assert.Fail("FND is NOT in the right status");
            }
            Assert.That(_mongoHandler.ValidateValueInDocsList(docsListFnd, "IsDeleted", "false"));
            var docsListCol = await _mongoHandler.SendMongoQueryWith2KeysAndReturnListOfResultAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", json.InstallmentPlanNumber, "Activity", "COL", "Status", "_id");
            Assert.That(_mongoHandler.ValidateValueInDocsList(docsListCol!, "Status", "New"));
            Assert.That(_mongoHandler.ValidateValueInDocsList(docsListCol!, "IsDeleted", "true"));
            Assert.That(_mongoHandler.ValidateValueInDocsList(docsListCol!, "Sku", "MER-UNF-STL-MON-FTR-CRC-U12"));
            var (_, idStatusDel) = await _mongoHandler.SendMongoQueryWith2KeysAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", json.InstallmentPlanNumber, "Activity", "DEL", "Status", "_id");
            var jResponseSettled = await _settled.SendPutRequestSettledAsync(_requestHeader!, "AutomationRun", idStatusDel, true);
            Assert.That(jResponseSettled, Is.Not.Null);
            await _mongoHandler.SendMongoQueryWith2KeysAsync(_db!, "Plan_PaymentSettings",
                "InstallmentPlanNumber", json.InstallmentPlanNumber, "PendingDeletion", "false", "IsDeleted", "_id");
            var docCol = _mongoHandler.QueryMongoWithDocId(_db!, "Payment_Records", idStatusColBefore);
            Assert.That(docCol!["IsDeleted"].ToString()!.Equals("true"));
            var docFnd = _mongoHandler.QueryMongoWithDocId(_db!, "Payment_Records", idStatusFndBefore);
            Assert.That(docFnd!["IsDeleted"].ToString()!.Equals("true"));
            var docRed = _mongoHandler.QueryMongoWithDocId(_db!, "Payment_Records", idStatusRedAfter);
            Assert.That(docRed!["IsDeleted"].ToString()!.Equals("true"));
            await _mongoHandler.SendMongoQueryWith2KeysAsync(_db!, "Plan_PaymentSettings",
                "InstallmentPlanNumber", json.InstallmentPlanNumber, "PendingDeletion", "false", "IsDeleted", "_id");
            var docCol1 = _mongoHandler.QueryMongoWithDocId(_db!, "Payment_Records", idStatusColBefore);
            Assert.That(docCol1!["IsDeleted"].ToString()!.Equals("true"));
            var docFnd1 = _mongoHandler.QueryMongoWithDocId(_db!, "Payment_Records", idStatusFndBefore);
            Assert.That(docFnd1!["IsDeleted"].ToString()!.Equals("true"));
            var docRed1 = _mongoHandler.QueryMongoWithDocId(_db!, "Payment_Records", idStatusRedAfter);
            Assert.That(docRed1!["IsDeleted"].ToString()!.Equals("true"));
            var docsListCol1 = await _mongoHandler.SendMongoQueryWith2KeysAndReturnListOfResultAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", json.InstallmentPlanNumber, "Activity", "COL", "Status", "_id");
            Assert.That(_mongoHandler.ValidateValueInDocsList(docsListCol1!, "Status", "New"));
            Assert.That(_mongoHandler.ValidateValueInDocsList(docsListCol1!, "IsDeleted", "false"));
            Assert.That(_mongoHandler.ValidateValueInDocsList(docsListCol1!, "Sku", "MER-UNF-STL-MON-FTR-CRC-U12"));
            var responseProcessing = await _processing.SendGetForProcessingAsync(_requestHeader!, json.InstallmentPlanNumber);
            Assert.That(responseProcessing, Is.Not.Null); 
            await _mongoHandler.SendMongoQueryWith3KeysReturn2StringColumnValuesAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", json.InstallmentPlanNumber, "Activity", "COL", "Status", "PendingBatch", "Status", "_id");
            Console.WriteLine("Done TestValidate_MER_FUN_STL_MON_FTR_DRC_F12_ActivitiesBefore_FullRefund");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidate_MER_FUN_STL_MON_FTR_DRC_F12_ActivitiesBefore_FullRefund \n" + exception + "\n");
        }
    }

    [TestCase(Category = "SkuFunActivitiesBefore")]
    [Test(Description = "TestValidate_DBS_FUN_STL_MON_FTR_CRC_F01_ActivitiesBefore with generated merchant"),
     CancelAfter(480 * 1000)]
    public async Task TestValidate_DBS_FUN_STL_MON_FTR_CRC_F01_ActivitiesBefore()
    {
        try
        {
            Console.WriteLine("\nStarting TestValidate_DBS_FUN_STL_MON_FTR_CRC_F01_ActivitiesBefore");
            _terminalApiKey = _envConfig.DBS_FUN_STL_MON_FTR;
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            var json = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!, _requestHeader!,
                "Active", 1, _terminalApiKey, createPlanDefaultValues);
            await _mongoHandler.SendMongoQueryWith2KeysAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", json.InstallmentPlanNumber, "Activity", "COL", "Status", "_id"); 
            await _mongoHandler.SendMongoQueryWith2KeysAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", json.InstallmentPlanNumber, "Activity", "FND", "Status", "_id");
            await _installmentPlanNumberRefund.SendRefundRequestAsync(_requestHeader!, json.InstallmentPlanNumber, "FullRefund");
            await _mongoHandler.SendMongoQueryWith2KeysAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", json.InstallmentPlanNumber, "Activity", "REF", "Status", "_id");
            Console.WriteLine("Done TestValidate_DBS_FUN_STL_MON_FTR_CRC_F01_ActivitiesBefore");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidate_DBS_FUN_STL_MON_FTR_CRC_F01_ActivitiesBefore \n" + exception + "\n");
        }
    }

    [TestCase(Category = "SkuFunActivitiesBefore")]
    [Test(Description = "TestValidate_DBS_FUN_STL_MON_FTR_CRC_F01_ActivitiesBefore_Past_Charge_First with generated merchant"),
     CancelAfter(480 * 1000)]
    public async Task TestValidate_DBS_FUN_STL_MON_FTR_CRC_F01_ActivitiesBefore_Past_Charge_First()
    {
        try
        {
            Console.WriteLine("\nStarting TestValidate_DBS_FUN_STL_MON_FTR_CRC_F01_ActivitiesBefore_Past_Charge_First");
            _terminalApiKey = _envConfig.DBS_FUN_STL_MON_FTR;
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            var json = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!, _requestHeader!,
                "Active", 1, _terminalApiKey, createPlanDefaultValues);
            await _mongoHandler.SendMongoQueryWith2KeysAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", json.InstallmentPlanNumber, "Activity", "COL", "Status", "_id");
            await _mongoHandler.SendMongoQueryWith2KeysAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", json.InstallmentPlanNumber, "Activity", "FND", "Status", "_id");
            await _installmentPlanNumberRefund.SendRefundRequestAsync(_requestHeader!, json.InstallmentPlanNumber,
                "PartialRefund", "FutureInstallmentsNotAllowed");
            await _mongoHandler.SendMongoQueryWith2KeysAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", json.InstallmentPlanNumber, "Activity", "REF", "Status", "_id");
            Console.WriteLine("Done TestValidate_DBS_FUN_STL_MON_FTR_CRC_F01_ActivitiesBefore_Past_Charge_First");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidate_DBS_FUN_STL_MON_FTR_CRC_F01_ActivitiesBefore_Past_Charge_First \n" + exception + "\n");
        }
    }

    [TestCase(Category = "SkuFunActivitiesBefore")]
    [Test(Description = "TestValidate_DBS_FUN_STL_MON_FTR_CRC_F03_ActivitiesBefore_Outstanding_Balance_First with generated merchant"),
     CancelAfter(480 * 1000)]
    public async Task TestValidate_DBS_FUN_STL_MON_FTR_CRC_F03_ActivitiesBefore_Outstanding_Balance_First()
    {
        try
        {
            Console.WriteLine(
                "\nStarting TestValidate_DBS_FUN_STL_MON_FTR_CRC_F03_ActivitiesBefore_Outstanding_Balance_First");
            _terminalApiKey = _envConfig.DBS_FUN_STL_MON_FTR;
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            var json = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!, _requestHeader!,
                "Active", 3, _terminalApiKey, createPlanDefaultValues);
            await _mongoHandler.SendMongoQueryWith2KeysAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", json.InstallmentPlanNumber, "Activity", "COL", "Status", "_id");
            await _mongoHandler.SendMongoQueryWith2KeysAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", json.InstallmentPlanNumber, "Activity", "FND", "Status", "_id");
            await _installmentPlanNumberRefund.SendRefundRequestAsync(_requestHeader!, json.InstallmentPlanNumber,
                "PartialRefund");
            await _mongoHandler.SendMongoQueryWith2KeysAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", json.InstallmentPlanNumber, "Activity", "RED", "Status", "_id");
            var response = await _processing.SendGetForProcessingAsync(_requestHeader!, json.InstallmentPlanNumber);
            Assert.That(response, Is.EqualTo(""));
            await _mongoHandler.SendMongoQueryWith2KeysAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", json.InstallmentPlanNumber, "Activity", "DEL", "Status", "_id");
            Console.WriteLine("Done TestValidate_DBS_FUN_STL_MON_FTR_CRC_F03_ActivitiesBefore_Outstanding_Balance_First");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidate_DBS_FUN_STL_MON_FTR_CRC_F03_ActivitiesBefore_Outstanding_Balance_First \n" + exception + "\n");
        }
    }

    [TestCase(Category = "SkuFunActivitiesBefore")]
    [Test(Description =
         "TestValidate_DBS_FUN_STL_MON_FTR_CRC_F03_ActivitiesBefore_Last_Installment with generated merchant"),
     CancelAfter(480 * 1000)]
    public async Task TestValidate_DBS_FUN_STL_MON_FTR_CRC_F03_ActivitiesBefore_Last_Installment()
    {
        try
        {
            Console.WriteLine("\nStarting TestValidate_DBS_FUN_STL_MON_FTR_CRC_F03_ActivitiesBefore_Last_Installment");
            _terminalApiKey = _envConfig.DBS_FUN_STL_MON_FTR;
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            var json = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!, _requestHeader!,
                "Active", 3, _terminalApiKey, createPlanDefaultValues);
            await _mongoHandler.SendMongoQueryWith2KeysAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", json.InstallmentPlanNumber, "Activity", "COL", "Status", "_id");
            await _mongoHandler.SendMongoQueryWith2KeysAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", json.InstallmentPlanNumber, "Activity", "FND", "Status", "_id");
            await _installmentPlanNumberRefund.SendRefundRequestAsync(_requestHeader!, json.InstallmentPlanNumber,
                "PartialRefund", "ReduceFromLastInstallment");
            await _mongoHandler.SendMongoQueryWith2KeysAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", json.InstallmentPlanNumber, "Activity", "RED", "Status", "_id");
            var response = await _processing.SendGetForProcessingAsync(_requestHeader!, json.InstallmentPlanNumber);
            Assert.That(response, Is.EqualTo(""));
            await _mongoHandler.SendMongoQueryWith2KeysAsync(_db!, "Payment_Records", 
                "InstallmentPlanNumber", json.InstallmentPlanNumber, "Activity", "DEL", "Status", "_id");
            Console.WriteLine("Done TestValidate_DBS_FUN_STL_MON_FTR_CRC_F03_ActivitiesBefore_Last_Installment");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidate_DBS_FUN_STL_MON_FTR_CRC_F03_ActivitiesBefore_Last_Installment \n" + exception + "\n");
        }
    }

    [TestCase(Category = "SkuFunActivitiesBefore")]
    [Test(Description = "TestValidate_DBS_FUN_STL_MON_FTR_CRC_F12_ActivitiesBefore with generated merchant"),
     CancelAfter(480 * 1000)]
    public async Task TestValidate_DBS_FUN_STL_MON_FTR_CRC_F12_ActivitiesBefore()
    {
        try
        {
            Console.WriteLine("\nStarting TestValidate_DBS_FUN_STL_MON_FTR_CRC_F12_ActivitiesBefore");
            var terminalApiKey = _envConfig.DBS_FUN_STL_MON_FTR_CRC_F12;
            Console.WriteLine("Done with creation of a new merchant , settings and terminal process");
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            var json = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!, _requestHeader!,
                "Active", 12, terminalApiKey, createPlanDefaultValues);
            await _mongoHandler.SendMongoQueryWith2KeysAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", json.InstallmentPlanNumber, "Activity", "COL", "Status", "_id");
            await _mongoHandler.SendMongoQueryWith2KeysAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", json.InstallmentPlanNumber, "Activity", "FND", "Status", "_id");
            await _installmentPlanNumberRefund.SendRefundRequestAsync(_requestHeader!, json.InstallmentPlanNumber,
                "FullRefund");
            await _mongoHandler.SendMongoQueryWith2KeysAsync(_db!, "Payment_Records", "InstallmentPlanNumber",
                json.InstallmentPlanNumber, "Activity", "REF", "Status", "_id");
            await _mongoHandler.SendMongoQueryWith2KeysAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", json.InstallmentPlanNumber, "Activity", "RED", "Status", "_id");
            var response = await _processing.SendGetForProcessingAsync(_requestHeader!, json.InstallmentPlanNumber);
            Assert.That(response, Is.EqualTo(""));
            await _mongoHandler.SendMongoQueryWith2KeysAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", json.InstallmentPlanNumber, "Activity", "DEL", "Status", "_id");
            Console.WriteLine("Done TestValidate_DBS_FUN_STL_MON_FTR_CRC_F12_ActivitiesBefore");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidate_DBS_FUN_STL_MON_FTR_CRC_F12_ActivitiesBefore \n" + exception + "\n");
        }
    }

    [TestCase(Category = "SkuFunActivitiesBefore")]
    [Test(Description = "TestValidate_PCO_FUN_STL_MON_FTR_CRC_F01_ActivitiesBefore with generated merchant"),
     CancelAfter(480 * 1000)]
    public async Task TestValidate_PCO_FUN_STL_MON_FTR_CRC_F01_ActivitiesBefore()
    {
        try
        {
            Console.WriteLine("\nStarting TestValidate_PCO_FUN_STL_MON_FTR_CRC_F01_ActivitiesBefore");
            _terminalApiKey = _envConfig.PCO_FUN_STL_MON_FTR;
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            var json = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!, _requestHeader!,
                "Active", 1, _terminalApiKey, createPlanDefaultValues);
            await _mongoHandler.SendMongoQueryWith2KeysAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", json.InstallmentPlanNumber, "Activity", "COL", "Status", "_id");
            await _mongoHandler.SendMongoQueryWith2KeysAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", json.InstallmentPlanNumber, "Activity", "FND", "Status", "_id");
            await _installmentPlanNumberRefund.SendRefundRequestAsync(_requestHeader!, json.InstallmentPlanNumber,
                "FullRefund");
            await _mongoHandler.SendMongoQueryWith2KeysAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber",
                json.InstallmentPlanNumber, "Activity", "REF", "Status", "_id");
            Console.WriteLine("Done TestValidate_PCO_FUN_STL_MON_FTR_CRC_F01_ActivitiesBefore");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidate_PCO_FUN_STL_MON_FTR_CRC_F01_ActivitiesBefore \n" + exception + "\n");
        }
    }

    [TestCase(Category = "SkuFunActivitiesBefore")]
    [Test(Description =
         "TestValidate_PCO_FUN_STL_MON_FTR_CRC_F01_ActivitiesBefore_PastChargeFirst with generated merchant"),
     CancelAfter(480 * 1000)]
    public async Task TestValidate_PCO_FUN_STL_MON_FTR_CRC_F01_ActivitiesBefore_PastChargeFirst()
    {
        try
        {
            Console.WriteLine("\nStarting TestValidate_PCO_FUN_STL_MON_FTR_CRC_F01_ActivitiesBefore_PastChargeFirst");
            _terminalApiKey = _envConfig.PCO_FUN_STL_MON_FTR;
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            var json = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!, _requestHeader!,
                "Active", 1, _terminalApiKey, createPlanDefaultValues);
            await _mongoHandler.SendMongoQueryWith2KeysAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", json.InstallmentPlanNumber, "Activity", "COL", "Status", "_id");
            await _mongoHandler.SendMongoQueryWith2KeysAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", json.InstallmentPlanNumber, "Activity", "FND", "Status", "_id");
            await _installmentPlanNumberRefund.SendRefundRequestAsync(_requestHeader!, json.InstallmentPlanNumber,
                "PartialRefund", "FutureInstallmentsNotAllowed"); 
            await _mongoHandler.SendMongoQueryWith2KeysAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", json.InstallmentPlanNumber, "Activity", "REF", "Status", "_id");
            Console.WriteLine("Done TestValidate_PCO_FUN_STL_MON_FTR_CRC_F01_ActivitiesBefore_PastChargeFirst");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidate_PCO_FUN_STL_MON_FTR_CRC_F01_ActivitiesBefore_PastChargeFirst \n" +
                        exception + "\n");
        }
    }

    [TestCase(Category = "SkuFunActivitiesBefore")]
    [Test(Description =
         "TestValidate_PCO_FUN_STL_MON_FTR_CRC_F06_ActivitiesBefore_Red_Del_OutstandingBalanceFirst with generated merchant"),
     CancelAfter(480 * 1000)]
    public async Task TestValidate_PCO_FUN_STL_MON_FTR_CRC_F06_ActivitiesBefore_Red_Del_OutstandingBalanceFirst()
    {
        string idStatusCol = null!;
        string activityStatusColB = null!;
        string idStatusFnd = null!;
        string activityStatusFndB = null!;
        string activityStatusDelB = null!;
        string idBatchDelB = null!;
        
        try
        {
            Console.WriteLine(
                "\nStarting TestValidate_PCO_FUN_STL_MON_FTR_CRC_F06_ActivitiesBefore_Red_Del_OutstandingBalanceFirst");
            _terminalApiKey = _envConfig.PCO_FUN_STL_MON_FTR;
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            var json = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!, _requestHeader!,
                "Active", 6, _terminalApiKey, createPlanDefaultValues);
            await _mongoHandler.SendMongoQueryWith2KeysAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", json.InstallmentPlanNumber, "Activity", "COL", "Status", "_id");
            await _mongoHandler.SendMongoQueryWith2KeysAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", json.InstallmentPlanNumber, "Activity", "FND", "Status", "_id");
            var response = await _processing.SendGetForProcessingAsync(_requestHeader!, json.InstallmentPlanNumber);
            Assert.That(response, Is.EqualTo(""));
            (_, idStatusCol) = await _mongoHandler.SendMongoQueryWith3KeysReturn2StringColumnValuesAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", json.InstallmentPlanNumber, "Activity", "COL", "Status", "PendingBatch", "Status", "_id");
            (_, idStatusFnd) = await _mongoHandler.SendMongoQueryWith3KeysReturn2StringColumnValuesAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", json.InstallmentPlanNumber, "Activity", "FND", "Status", "PendingBatch","Status", "_id");
            await _created.SendPutRequestCreatedAsync(_requestHeader!, idStatusCol);
            await _created.SendPutRequestCreatedAsync(_requestHeader!, idStatusFnd);
            (activityStatusColB, _) = await _mongoHandler.SendMongoQueryWith2KeysAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", json.InstallmentPlanNumber, "Activity", "COL", "BatchId", "_id");
            (activityStatusFndB, _) = await _mongoHandler.SendMongoQueryWith2KeysAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", json.InstallmentPlanNumber, "Activity", "FND", "BatchId", "_id");
            await _settled.SendPutRequestSettledAsync(_requestHeader!, activityStatusColB, idStatusCol, true);
            await _settled.SendPutRequestSettledAsync(_requestHeader!, activityStatusFndB, idStatusFnd, true);
            await _mongoHandler.SendMongoQueryWith3KeysReturn2StringColumnValuesAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", json.InstallmentPlanNumber, "Activity", "COL", 
                "Status","Acknowledged", "Status", "_id");
            await _mongoHandler.SendMongoQueryWith3KeysReturn2StringColumnValuesAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", json.InstallmentPlanNumber, "Activity", "FND", 
                "Status","Acknowledged", "Status", "_id");
            await _installmentPlanNumberRefund.SendRefundRequestAsync(_requestHeader!, json.InstallmentPlanNumber,
                "PartialRefund");
            await _mongoHandler.SendMongoQueryWith2KeysAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", json.InstallmentPlanNumber, "Activity", "RED", "Status", "_id");
            var responseGetForProcessing = await _processing.SendGetForProcessingAsync(_requestHeader!, json.InstallmentPlanNumber);
            Assert.That(responseGetForProcessing, Is.EqualTo(""));
            await _mongoHandler.SendMongoQueryWith3KeysReturn2StringColumnValuesAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", json.InstallmentPlanNumber, "Activity", "DEL", "Status", "PendingBatch","Status", "_id");
            var docsListCol = await _mongoHandler.SendMongoQueryWith2KeysAndReturnListOfResultAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", json.InstallmentPlanNumber, "Activity", "COL", "Sku", "_id");
            Assert.That(_mongoHandler.ValidateValueInDocsList(docsListCol!, "Status", "New"));
            var docsListFnd = await _mongoHandler.SendMongoQueryWith2KeysAndReturnListOfResultAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", json.InstallmentPlanNumber, "Activity", "FND", "Sku", "_id");
            Assert.That(_mongoHandler.ValidateValueInDocsList(docsListFnd!, "Status", "New"));
            var idStatusColNew = _mongoHandler.ReturnValueFromDocsList(docsListCol!, "Status", "New", "_id");
            var idStatusFndNew = _mongoHandler.ReturnValueFromDocsList(docsListFnd!, "Status", "New", "_id");
            Assert.That(idStatusColNew, Is.Not.Null);
            Assert.That(idStatusFndNew, Is.Not.Null);
            await _mongoHandler.SendMongoQueryWith2KeysAsync(_db!, "Plan_PaymentSettings", "InstallmentPlanNumber", json.InstallmentPlanNumber,
                "PendingDeletion", "true", "Sku", "_id");
            var (_, idStatusDel) = await _mongoHandler.SendMongoQueryWith2KeysAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", json.InstallmentPlanNumber, "Activity", "DEL", "Status", "_id");
            await _created.SendPutRequestCreatedAsync(_requestHeader!, idStatusDel);
            (activityStatusDelB, idBatchDelB) = await _mongoHandler.SendMongoQueryWith2KeysAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", json.InstallmentPlanNumber, "Activity", "DEL", "BatchId", "_id");
            await _settled.SendPutRequestSettledAsync(_requestHeader!, activityStatusDelB, idBatchDelB, true);
            await _mongoHandler.SendMongoQueryWith3KeysReturn2StringColumnValuesAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", json.InstallmentPlanNumber, "Activity", "DEL", "Status", "Acknowledged","Status", "_id");
            var docsListCol1 = await _mongoHandler.SendMongoQueryWith2KeysAndReturnListOfResultAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", json.InstallmentPlanNumber, "Activity", "COL", "Sku", "_id");
            var idStatusColNew1 = _mongoHandler.ReturnValueFromDocsList(docsListCol1!, "Status", "New", "IsDeleted");
            Assert.That(idStatusColNew1, Is.EqualTo("false"));
            var docsListFnd1 = await _mongoHandler.SendMongoQueryWith2KeysAndReturnListOfResultAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", json.InstallmentPlanNumber, "Activity", "FND", "Sku", "_id");
            var idStatusFndNew1 = _mongoHandler.ReturnValueFromDocsList(docsListFnd1!, "Status", "New", "IsDeleted");
            Assert.That(idStatusFndNew1, Is.EqualTo("false"));
            var docsListDel = await _mongoHandler.SendMongoQueryWith2KeysAndReturnListOfResultAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", json.InstallmentPlanNumber, "Activity", "DEL", "IsDeleted", "_id");
            var statusDelAck = _mongoHandler.ReturnValueFromDocsList(docsListDel!, "Status", "Acknowledged", "IsDeleted");
            Assert.That(statusDelAck, Is.EqualTo("false"));
            var responseGetForProcessingAfter = await _processing.SendGetForProcessingAsync(_requestHeader!, json.InstallmentPlanNumber);
            Assert.That(responseGetForProcessingAfter, Is.EqualTo(""));
            var docsListColAfter = await _mongoHandler.SendMongoQueryWith2KeysAndReturnListOfResultAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", json.InstallmentPlanNumber, "Activity", "COL", "Status", "_id");
            var statusColAfter = _mongoHandler.ReturnValueFromDocsList(docsListColAfter!, "Status", "PendingBatch", "Status");
            Assert.That(statusColAfter, Is.EqualTo("PendingBatch"));
            var docsListFndAfter = await _mongoHandler.SendMongoQueryWith2KeysAndReturnListOfResultAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", json.InstallmentPlanNumber, "Activity", "FND", "Status", "_id");
            var statusFndAfter = _mongoHandler.ReturnValueFromDocsList(docsListFndAfter!, "Status", "PendingBatch", "Status");
            Assert.That(statusFndAfter, Is.EqualTo("PendingBatch"));
            var jResponse = await _chargeFunctionality.SendPostRequestChargeFunctionalityAsync(_requestHeader!, json.InstallmentPlanNumber);
            Assert.That(jResponse.ResponseHeader.Succeeded);
            Console.WriteLine("Done TestValidate_PCO_FUN_STL_MON_FTR_CRC_F06_ActivitiesBefore_Red_Del_OutstandingBalanceFirst");
        }
        catch (Exception exception)
        {
            await Task.Delay(10 * 1000);
            await _settled.SendPutRequestSettledAsync(_requestHeader!, activityStatusColB, idStatusCol, false);
            await _settled.SendPutRequestSettledAsync(_requestHeader!, activityStatusFndB, idStatusFnd, false);
            await _settled.SendPutRequestSettledAsync(_requestHeader!, activityStatusDelB, idBatchDelB, false);
            Assert.Fail("Error in TestValidate_PCO_FUN_STL_MON_FTR_CRC_F06_ActivitiesBefore_Red_Del_OutstandingBalanceFirst \n" + exception + "\n");
        }
    }

    [TestCase(Category = "SkuFunActivitiesBefore")]
    [Test(Description = "TestValidate_PCO_FUN_STL_MON_FTR_CRC_F06_ActivitiesBefore_Red_Del_LastInstallment with generated merchant"),
     CancelAfter(480 * 1000)]
    public async Task TestValidate_PCO_FUN_STL_MON_FTR_CRC_F06_ActivitiesBefore_Red_Del_LastInstallment()
    {
        string idStatusCol = null!;
        string activityStatusColB = null!;
        string idStatusFnd = null!;
        string activityStatusFndB = null!;
        string activityStatusDelB = null!;
        string idBatchDelB = null!;
        
        try
        {
            Console.WriteLine("Starting TestValidate_PCO_FUN_STL_MON_FTR_CRC_F06_ActivitiesBefore_Red_Del_LastInstallment");
            _terminalApiKey = _envConfig.PCO_FUN_STL_MON_FTR;
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            var json = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!, _requestHeader!,
                "Active", 6, _terminalApiKey, createPlanDefaultValues);
            await _mongoHandler.SendMongoQueryWith2KeysAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", json.InstallmentPlanNumber, "Activity", "COL", "Status", "_id");
            await _mongoHandler.SendMongoQueryWith2KeysAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", json.InstallmentPlanNumber, "Activity", "FND", "Status", "_id");
            var response = await _processing.SendGetForProcessingAsync(_requestHeader!, json.InstallmentPlanNumber);
            Assert.That(response, Is.EqualTo(""));
            (_, idStatusCol) = await _mongoHandler.SendMongoQueryWith3KeysReturn2StringColumnValuesAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", json.InstallmentPlanNumber, "Activity", "COL", "Status", "PendingBatch", "Status", "_id");
            (_, idStatusFnd) = await _mongoHandler.SendMongoQueryWith3KeysReturn2StringColumnValuesAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", json.InstallmentPlanNumber, "Activity", "FND", "Status", "PendingBatch", "Status", "_id");
            await _created.SendPutRequestCreatedAsync(_requestHeader!, idStatusCol);
            await _created.SendPutRequestCreatedAsync(_requestHeader!, idStatusFnd);
            (activityStatusColB, _) = await _mongoHandler.SendMongoQueryWith2KeysAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", json.InstallmentPlanNumber, "Activity", "COL", "BatchId", "_id");
            (activityStatusFndB, _) = await _mongoHandler.SendMongoQueryWith2KeysAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", json.InstallmentPlanNumber, "Activity", "FND", "BatchId", "_id");
            await _settled.SendPutRequestSettledAsync(_requestHeader!, activityStatusColB, idStatusCol, true);
            await _settled.SendPutRequestSettledAsync(_requestHeader!, activityStatusFndB, idStatusFnd, true);
            await _mongoHandler.SendMongoQueryWith3KeysReturn2StringColumnValuesAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", json.InstallmentPlanNumber, "Activity", "COL", "Status", "Acknowledged","Status", "_id");
            await _mongoHandler.SendMongoQueryWith3KeysReturn2StringColumnValuesAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", json.InstallmentPlanNumber, "Activity", "FND", "Status", "Acknowledged", "Status", "_id");
            await _installmentPlanNumberRefund.SendRefundRequestAsync(_requestHeader!, json.InstallmentPlanNumber,
                "PartialRefund", "ReduceFromLastInstallment");
            await _mongoHandler.SendMongoQueryWith2KeysAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", json.InstallmentPlanNumber, "Activity", "RED", "Status", "_id");
            var responseGetForProcessing = await _processing.SendGetForProcessingAsync(_requestHeader!, json.InstallmentPlanNumber);
            Assert.That(responseGetForProcessing, Is.EqualTo(""));
            await _mongoHandler.SendMongoQueryWith3KeysReturn2StringColumnValuesAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", json.InstallmentPlanNumber, "Activity", "DEL", "Status", "PendingBatch","Status", "_id");
            var docsListCol = await _mongoHandler.SendMongoQueryWith2KeysAndReturnListOfResultAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", json.InstallmentPlanNumber, "Activity", "COL", "Sku", "_id");
            Assert.That(_mongoHandler.ValidateValueInDocsList(docsListCol!, "Status", "New"));
            var docsListFnd = await _mongoHandler.SendMongoQueryWith2KeysAndReturnListOfResultAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", json.InstallmentPlanNumber, "Activity", "FND", "Sku", "_id");
            Assert.That(_mongoHandler.ValidateValueInDocsList(docsListFnd!, "Status", "New"));
            _mongoHandler.ReturnValueFromDocsList(docsListCol!, "Status", "New", "_id");
            _mongoHandler.ReturnValueFromDocsList(docsListFnd!, "Status", "New", "_id");
            await _mongoHandler.SendMongoQueryWith2KeysAsync(_db!, "Plan_PaymentSettings", 
                "InstallmentPlanNumber", json.InstallmentPlanNumber, "PendingDeletion", "true", "Sku", "_id");
            var (_, idStatusDel) = await _mongoHandler.SendMongoQueryWith2KeysAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", json.InstallmentPlanNumber, "Activity", "DEL", "Status", "_id");
            await _created.SendPutRequestCreatedAsync(_requestHeader!, idStatusDel);
            (activityStatusDelB, idBatchDelB) = await _mongoHandler.SendMongoQueryWith2KeysAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", json.InstallmentPlanNumber, "Activity", "DEL", "BatchId", "_id");
            await _settled.SendPutRequestSettledAsync(_requestHeader!, activityStatusDelB, idBatchDelB, true);
            await _mongoHandler.SendMongoQueryWith3KeysReturn2StringColumnValuesAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", json.InstallmentPlanNumber, "Activity", "DEL", "Status", "Acknowledged","Status", "_id");
            var docsListCol1 = await _mongoHandler.SendMongoQueryWith2KeysAndReturnListOfResultAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", json.InstallmentPlanNumber, "Activity", "COL", "Sku", "_id");
            var idStatusColNew1 = _mongoHandler.ReturnValueFromDocsList(docsListCol1!, "Status", "New", "IsDeleted");
            Assert.That(idStatusColNew1, Is.EqualTo("false"));
            var docsListFnd1 = await _mongoHandler.SendMongoQueryWith2KeysAndReturnListOfResultAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", json.InstallmentPlanNumber, "Activity", "FND", "Sku", "_id");
            var idStatusFndNew1 = _mongoHandler.ReturnValueFromDocsList(docsListFnd1!, "Status", "New", "IsDeleted");
            Assert.That(idStatusFndNew1, Is.EqualTo("false"));
            var docsListDel = await _mongoHandler.SendMongoQueryWith2KeysAndReturnListOfResultAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", json.InstallmentPlanNumber, "Activity", "DEL", "IsDeleted", "_id");
            var statusDelAck = _mongoHandler.ReturnValueFromDocsList(docsListDel!, "Status", "Acknowledged", "IsDeleted");
            Assert.That(statusDelAck, Is.EqualTo("false"));
            var responseGetForProcessingAfter = await _processing.SendGetForProcessingAsync(_requestHeader!, json.InstallmentPlanNumber);
            Assert.That(responseGetForProcessingAfter, Is.EqualTo(""));
            var docsListColAfter = await _mongoHandler.SendMongoQueryWith2KeysAndReturnListOfResultAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", json.InstallmentPlanNumber, "Activity", "COL", "Status", "_id");
            var statusColAfter = _mongoHandler.ReturnValueFromDocsList(docsListColAfter!, "Status", "PendingBatch", "Status");
            Assert.That(statusColAfter, Is.EqualTo("PendingBatch"));
            var docsListFndAfter = await _mongoHandler.SendMongoQueryWith2KeysAndReturnListOfResultAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", json.InstallmentPlanNumber, "Activity", "FND", "Status", "_id");
            var statusFndAfter = _mongoHandler.ReturnValueFromDocsList(docsListFndAfter!, "Status", "PendingBatch", "Status");
            Assert.That(statusFndAfter, Is.EqualTo("PendingBatch"));
            Console.WriteLine("Done TestValidate_PCO_FUN_STL_MON_FTR_CRC_F06_ActivitiesBefore_Red_Del_LastInstallment");
        }
        catch (Exception exception)
        {
            await Task.Delay(10 * 1000);
            await _settled.SendPutRequestSettledAsync(_requestHeader!, activityStatusDelB, idBatchDelB, false);
            await _settled.SendPutRequestSettledAsync(_requestHeader!, activityStatusColB, idStatusCol, false);
            await _settled.SendPutRequestSettledAsync(_requestHeader!, activityStatusFndB, idStatusFnd, false);
            Assert.Fail(
                "Error in TestValidate_PCO_FUN_STL_MON_FTR_CRC_F06_ActivitiesBefore_Red_Del_LastInstallment \n" +
                exception + "\n");
        }
    }

    [TestCase(Category = "SkuFunActivitiesBefore")]
    [Test(Description = "TestValidate_PCO_FUN_STL_MON_FTR_CRC_F06_ActivitiesBefore_FullRefund with generated merchant"),
     CancelAfter(480 * 1000)]
    public async Task TestValidate_PCO_FUN_STL_MON_FTR_CRC_F06_ActivitiesBefore_FullRefund()
    {
        string idStatusDel = null!;
        string idStatusColBefore = null!;
        string idStatusFndBefore = null!;
        
        try
        {
            Console.WriteLine("\nStarting TestValidate_PCO_FUN_STL_MON_FTR_CRC_F06_ActivitiesBefore_FullRefund");
            _terminalApiKey = _envConfig.PCO_FUN_STL_MON_FTR;
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            var json = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!, _requestHeader!,
                "Active", 6, _terminalApiKey, createPlanDefaultValues);
            (_, idStatusColBefore) = await _mongoHandler.SendMongoQueryWith2KeysAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", json.InstallmentPlanNumber, "Activity", "COL", "Status", "_id");
            (_, idStatusFndBefore) = await _mongoHandler.SendMongoQueryWith2KeysAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", json.InstallmentPlanNumber, "Activity", "FND", "Status", "_id");
            await _settled.SendPutRequestSettledAsync(_requestHeader!, "AutomationRunCol", idStatusColBefore, true);
            await _settled.SendPutRequestSettledAsync(_requestHeader!, "AutomationRunFnd", idStatusFndBefore, true);
            await _mongoHandler.SendMongoQueryWith3KeysReturn2StringColumnValuesAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", json.InstallmentPlanNumber, "Activity", "COL", 
                "Status","Acknowledged", "Status", "_id");
            await _mongoHandler.SendMongoQueryWith3KeysReturn2StringColumnValuesAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", json.InstallmentPlanNumber, "Activity", "FND", 
                "Status","Acknowledged", "Status", "_id");
            await _mongoHandler.SendMongoQueryWith2KeysAsync(_db!, "Plan_PaymentSettings",
                "InstallmentPlanNumber", json.InstallmentPlanNumber, "PendingDeletion", "false", "IsDeleted", "_id");
            var jResponse = await _installmentPlanNumberRefund.SendRefundRequestAsync(_requestHeader!,
                json.InstallmentPlanNumber, "FullRefund");
            Assert.That(jResponse.Error, Is.Null);
            await _mongoHandler.SendMongoQueryWith2KeysAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", json.InstallmentPlanNumber, "Activity", "RED", "Status", "_id");
            await _mongoHandler.SendMongoQueryWith2KeysAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", json.InstallmentPlanNumber, "Activity", "REF", "Status", "_id");
            var response = await _processing.SendGetForProcessingAsync(_requestHeader!, json.InstallmentPlanNumber);
            Assert.That(response, Is.EqualTo(""));
            (_, idStatusDel) = await _mongoHandler.SendMongoQueryWith3KeysReturn2StringColumnValuesAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", json.InstallmentPlanNumber, "Activity", "DEL", "Status", "PendingBatch", "Status", "_id");
            await _mongoHandler.SendMongoQueryWith2KeysAsync(_db!, "Plan_PaymentSettings",
                "InstallmentPlanNumber", json.InstallmentPlanNumber, "PendingDeletion", "true", "IsDeleted", "_id");
            await _mongoHandler.SendMongoQueryWith3KeysReturn2StringColumnValuesAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", json.InstallmentPlanNumber, "Activity", "COL", "IsDeleted", "false", "Status", "_id");
            await _mongoHandler.SendMongoQueryWith3KeysReturn2StringColumnValuesAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", json.InstallmentPlanNumber, "Activity", "FND", "IsDeleted", "false", "Status", "_id");
            await _mongoHandler.SendMongoQueryWith3KeysReturn2StringColumnValuesAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", json.InstallmentPlanNumber, "Activity", "RED", "IsDeleted", "false", "Status", "_id");
            await _mongoHandler.SendMongoQueryWith3KeysReturn2StringColumnValuesAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", json.InstallmentPlanNumber, "Activity", "REF", "IsDeleted", "false", "Status", "_id");
            await _settled.SendPutRequestSettledAsync(_requestHeader!, "AutomationRunDel", idStatusDel, true);
            await _mongoHandler.SendMongoQueryWith3KeysReturn2StringColumnValuesAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", json.InstallmentPlanNumber, "Activity", "DEL", "Status", "Acknowledged","Status", "_id");
            await _mongoHandler.SendMongoQueryWith2KeysAsync(_db!, "Plan_PaymentSettings",
                "InstallmentPlanNumber", json.InstallmentPlanNumber, "PendingDeletion", "false", "IsDeleted", "_id");
            await _mongoHandler.SendMongoQueryWith3KeysReturn2StringColumnValuesAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", json.InstallmentPlanNumber, "Activity", "COL", "IsDeleted", "true", "Status", "_id");
            await _mongoHandler.SendMongoQueryWith3KeysReturn2StringColumnValuesAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", json.InstallmentPlanNumber, "Activity", "FND", "IsDeleted", "true", "Status", "_id");
            await _mongoHandler.SendMongoQueryWith3KeysReturn2StringColumnValuesAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", json.InstallmentPlanNumber, "Activity", "RED", "IsDeleted", "true", "Status", "_id");
            Console.WriteLine("Starting TestValidate_PCO_FUN_STL_MON_FTR_CRC_F06_ActivitiesBefore_FullRefund\n");
        }
        catch (Exception exception)
        {
            await Task.Delay(10 * 1000);
            await _settled.SendPutRequestSettledAsync(_requestHeader!, "AutomationRunDel", idStatusDel, false);
            await _settled.SendPutRequestSettledAsync(_requestHeader!, "AutomationRunCol", idStatusColBefore, false);
            await _settled.SendPutRequestSettledAsync(_requestHeader!, "AutomationRunFnd", idStatusFndBefore, false);
            Assert.Fail("Error in TestValidate_PCO_FUN_STL_MON_FTR_CRC_F06_ActivitiesBefore_FullRefund \n" + exception + "\n");
        }
    }

    [TestCase(Category = "SkuFunActivitiesBefore")]
    [Test(Description =
         "TestValidate_PCO_FUN_STL_MON_FTR_CRC_F06_ActivitiesBefore_FullRefund_Del with generated merchant"),
     CancelAfter(480 * 1000)]
    public async Task TestValidate_PCO_FUN_STL_MON_FTR_CRC_F06_ActivitiesBefore_FullRefund_Del()
    {
        string idStatusCol = null!;
        string idBatchColB = null!;
        string idStatusFnd = null!;
        string idBatchFndB = null!;
        
        try
        {
            Console.WriteLine("\nStarting TestValidate_PCO_FUN_STL_MON_FTR_CRC_F06_ActivitiesBefore_FullRefund_Del");
            _terminalApiKey = _envConfig.PCO_FUN_STL_MON_FTR;
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            var json = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!, _requestHeader!,
                "Active", 6, _terminalApiKey, createPlanDefaultValues);
            await _mongoHandler.SendMongoQueryWith2KeysAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", json.InstallmentPlanNumber, "Activity", "COL", "Status", "_id");
            await _mongoHandler.SendMongoQueryWith2KeysAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", json.InstallmentPlanNumber, "Activity", "FND", "Status", "_id");
            var response = await _processing.SendGetForProcessingAsync(_requestHeader!, json.InstallmentPlanNumber);
            Assert.That(response, Is.Not.Null);
            (_, idStatusCol) = await _mongoHandler.SendMongoQueryWith3KeysReturn2StringColumnValuesAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", json.InstallmentPlanNumber, "Activity", "COL", "Status","PendingBatch", "Status", "_id");
            (_, idStatusFnd) = await _mongoHandler.SendMongoQueryWith3KeysReturn2StringColumnValuesAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", json.InstallmentPlanNumber, "Activity", "FND","Status","PendingBatch", "Status", "_id");
            await _created.SendPutRequestCreatedAsync(_requestHeader!, idStatusCol);
            await _created.SendPutRequestCreatedAsync(_requestHeader!, idStatusFnd);
            (_, idBatchColB) = await _mongoHandler.SendMongoQueryWith2KeysAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", json.InstallmentPlanNumber, "Activity", "COL", "BatchId", "_id");
            (_, idBatchFndB) = await _mongoHandler.SendMongoQueryWith2KeysAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", json.InstallmentPlanNumber, "Activity", "FND", "BatchId", "_id");
            await _settled.SendPutRequestSettledAsync(_requestHeader!, idBatchColB, idStatusCol, true);
            await _settled.SendPutRequestSettledAsync(_requestHeader!, idBatchFndB, idStatusFnd, true);
            await _mongoHandler.SendMongoQueryWith3KeysReturn2StringColumnValuesAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", json.InstallmentPlanNumber, "Activity", "COL", 
                "Status","Acknowledged", "Status", "_id");
            await _mongoHandler.SendMongoQueryWith3KeysReturn2StringColumnValuesAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", json.InstallmentPlanNumber, "Activity", "FND", 
                "Status","Acknowledged", "Status", "_id");
            await _mongoHandler.SendMongoQueryWith2KeysAsync(_db!, "Plan_PaymentSettings",
                "InstallmentPlanNumber", json.InstallmentPlanNumber, "PendingDeletion", "false", "IsDeleted", "_id");
            var jResponse = await _installmentPlanNumberRefund.SendRefundRequestAsync(_requestHeader!,
                json.InstallmentPlanNumber, "FullRefund");
            Assert.That(jResponse.RefundId, Is.Not.Null);
            await _mongoHandler.SendMongoQueryWith2KeysAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", json.InstallmentPlanNumber, "Activity", "REF", "Status", "_id");
            await _mongoHandler.SendMongoQueryWith2KeysAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", json.InstallmentPlanNumber, "Activity", "RED", "Status", "_id");
            await _mongoHandler.SendMongoQueryWith3KeysReturn2StringColumnValuesAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", json.InstallmentPlanNumber, "Activity", "REF", "IsDeleted", "false", "Status", "_id");
            await _mongoHandler.SendMongoQueryWith3KeysReturn2StringColumnValuesAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", json.InstallmentPlanNumber, "Activity", "RED", "IsDeleted", "false", "Status", "_id");
            var jResponseProcessing = await _processing.SendGetForProcessingAsync(_requestHeader!, json.InstallmentPlanNumber);
            Assert.That(jResponseProcessing, Is.Not.Null);
            await _mongoHandler.SendMongoQueryWith2KeysAsync(_db!, "Plan_PaymentSettings",
                "InstallmentPlanNumber", json.InstallmentPlanNumber, "PendingDeletion", "true", "IsDeleted", "_id");
            await _mongoHandler.SendMongoQueryWith3KeysReturn2StringColumnValuesAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", json.InstallmentPlanNumber, "Activity", "COL", "IsDeleted", "false", "Status", "_id");
            await _mongoHandler.SendMongoQueryWith3KeysReturn2StringColumnValuesAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", json.InstallmentPlanNumber, "Activity", "FND", "IsDeleted", "false", "Status", "_id");
            await _mongoHandler.SendMongoQueryWith3KeysReturn2StringColumnValuesAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", json.InstallmentPlanNumber, "Activity", "REF", "IsDeleted", "false", "Status", "_id");
            await _mongoHandler.SendMongoQueryWith3KeysReturn2StringColumnValuesAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", json.InstallmentPlanNumber, "Activity", "RED", "IsDeleted", "false", "Status", "_id");
            await _created.SendPutRequestCreatedAsync(_requestHeader!, idStatusCol);
            await Task.Delay(5 * 1000);
            await _settled.SendPutRequestSettledAsync(_requestHeader!, idBatchColB, idStatusCol, true);
            await _mongoHandler.SendMongoQueryWith2KeysAsync(_db!, "Plan_PaymentSettings", "InstallmentPlanNumber", json.InstallmentPlanNumber,
                "PendingDeletion", "true", "IsDeleted", "_id");
            await _mongoHandler.SendMongoQueryWith3KeysReturn2StringColumnValuesAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", json.InstallmentPlanNumber, "Activity", "COL", "IsDeleted", "false", "Status", "_id");
            await _mongoHandler.SendMongoQueryWith3KeysReturn2StringColumnValuesAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", json.InstallmentPlanNumber, "Activity", "FND", "IsDeleted", "false", "Status", "_id");
            await _mongoHandler.SendMongoQueryWith3KeysReturn2StringColumnValuesAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", json.InstallmentPlanNumber, "Activity", "REF", "IsDeleted", "false", "Status", "_id");
            await _mongoHandler.SendMongoQueryWith3KeysReturn2StringColumnValuesAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", json.InstallmentPlanNumber, "Activity", "RED", "IsDeleted", "false", "Status", "_id");
            await _invoiced.SendPutRequestInvoicedAsync(_requestHeader!, idStatusCol, "AutomationRun");
            await _invoiced.SendPutRequestInvoicedAsync(_requestHeader!, idStatusFnd, "AutomationRun");
            Console.WriteLine("Starting TestValidate_PCO_FUN_STL_MON_FTR_CRC_F06_ActivitiesBefore_FullRefund_Del\n");
        }
        catch (Exception exception)
        {
            await Task.Delay(10 * 1000);
            await _settled.SendPutRequestSettledAsync(_requestHeader!, idBatchColB, idStatusCol, false);
            await _settled.SendPutRequestSettledAsync(_requestHeader!, idBatchFndB, idStatusFnd, false);
            Assert.Fail("Error in TestValidate_PCO_FUN_STL_MON_FTR_CRC_F06_ActivitiesBefore_FullRefund_Del \n" +
                        exception + "\n");
        }
    }

    [TestCase(Category = "SkuFunActivitiesBefore")]
    [Test(Description = "TestValidate_PCO_FUN_STL_MON_FTR_CRC_F12_ActivitiesBefore_FullRefund with generated merchant"),
     CancelAfter(480 * 1000)]
    public async Task TestValidate_PCO_FUN_STL_MON_FTR_CRC_F12_ActivitiesBefore_FullRefund()
    {
        try
        {
            Console.WriteLine("\nStarting TestValidate_PCO_FUN_STL_MON_FTR_CRC_F12_ActivitiesBefore_FullRefund");
            _terminalApiKey = _envConfig.PCO_FUN_STL_MON_FTR;
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            var json = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!, _requestHeader!,
                "Active", 12, _terminalApiKey, createPlanDefaultValues);
            await _mongoHandler.SendMongoQueryWith2KeysAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", json.InstallmentPlanNumber, "Activity", "COL", "Status", "_id");
            await _mongoHandler.SendMongoQueryWith2KeysAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", json.InstallmentPlanNumber, "Activity", "FND", "Status", "_id");
            await _installmentPlanNumberRefund.SendRefundRequestAsync(_requestHeader!, json.InstallmentPlanNumber,
                "FullRefund");
            await _mongoHandler.SendMongoQueryWith2KeysAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", json.InstallmentPlanNumber, "Activity", "REF", "Status", "_id");
            var response = await _processing.SendGetForProcessingAsync(_requestHeader!, json.InstallmentPlanNumber);
            Assert.That(response, Is.Not.Null);
            await _mongoHandler.SendMongoQueryWith3KeysReturn2StringColumnValuesAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", json.InstallmentPlanNumber, "Activity", "Status", "Skipped","DEL", "Status", "_id");
            Console.WriteLine("Starting TestValidate_PCO_FUN_STL_MON_FTR_CRC_F12_ActivitiesBefore_FullRefund\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidate_PCO_FUN_STL_MON_FTR_CRC_F12_ActivitiesBefore_FullRefund \n" + exception +
                        "\n");
        }
    }
}