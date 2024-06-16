using MongoDB.Driver;
using Allure.NUnit;
using Allure.NUnit.Attributes;
using NUnit.Framework;
using Splitit.Automation.NG.Backend.BaseActions;
using Splitit.Automation.NG.Backend.Services.V3.DefaultValues;
using Splitit.Automation.NG.Backend.Services.V3.InstallmentPlan.InstallmentPlanApiEndPoint;
using Splitit.Automation.NG.Backend.Tests.TestsSetups;
using Splitit.Automation.NG.PaymentsSystem.PaymentProcessing.EndPointFunctionality;
using Splitit.Automation.NG.PaymentsSystem.PaymentsOperations.EndPointFunctionality;
using Splitit.Automation.NG.Utilities.EnvironmentsConfig;

namespace Splitit.Automation.NG.PaymentsSystem.Sku.SkuSetActivitiesAfterInvoiced.Tests.SkuFunActivitiesAfter;

[TestFixture]
[AllureNUnit]
[AllureSuite("SkuFunActivitiesAfter")]
[AllureDisplayIgnored]
public class SkuFunActivitiesAfter
{
    private RequestHeader? _requestHeader;
    private readonly InstallmentPlans _installmentPlans;
    private readonly MongoHandler _mongoHandler;
    private readonly Processing _processing;
    private string? _terminalApiKey;
    private readonly Created _created;
    private readonly Settled _settled;
    private readonly CreatePaymentInvoice _createPaymentInvoice;
    private readonly Invoiced _invoiced;
    private readonly InstallmentPlanNumberRefund _installmentPlanNumberRefund;
    private IMongoDatabase? _db;
    private readonly EnvConfig _envConfig;


    public SkuFunActivitiesAfter()
    {
        Console.WriteLine("Staring SKU Setup");
        _installmentPlans = new InstallmentPlans();
        _mongoHandler = new MongoHandler();
        _processing = new Processing();
        _created = new Created();
        _settled = new Settled();
        _createPaymentInvoice = new CreatePaymentInvoice();
        _invoiced = new Invoiced();
        _installmentPlanNumberRefund = new InstallmentPlanNumberRefund();
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

    [TestCase(Category = "SkuFunActivitiesAfter")]
    [Test(Description = "TestValidate_MER_FUN_STL_MON_FTR_CRC_F06_ActivitiesAfter with generated merchant"),
     CancelAfter(360 * 1000)]
    public async Task TestValidate_MER_FUN_STL_MON_FTR_CRC_F06_ActivitiesAfter()
    {
        try
        {
            Console.WriteLine("\nStarting TestValidate_MER_FUN_STL_MON_FTR_CRC_F06_ActivitiesAfter");
            var terminalApiKey = _envConfig.MER_FUN_STL_MON_FTR_CRC_F06;
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            var planCreateResponse = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!, _requestHeader!,
                "PendingCapture", 6, terminalApiKey, createPlanDefaultValues);
            await _mongoHandler.SendMongoQueryWith2KeysAsync(_db!,
                "Payment_Records", "InstallmentPlanNumber", planCreateResponse.InstallmentPlanNumber,
                "Activity", "COL", "Status", "_id");
            Console.WriteLine("Done TestValidate_MER_FUN_STL_MON_FTR_CRC_F06_ActivitiesAfter");
        }
        catch (Exception exception)
        {
            Assert.Fail("Failed -> TestValidate_MER_FUN_STL_MON_FTR_CRC_F06_ActivitiesAfter \n" + exception + "\n");
        }
    }

    [TestCase(Category = "SkuFunActivitiesAfter")]
    [Test(Description = "TestValidate_MER_FUN_STL_MON_FTR_CRC_F09_ActivitiesAfter with generated merchant"),
     CancelAfter(360 * 1000)]
    public async Task TestValidate_MER_FUN_STL_MON_FTR_CRC_F09_ActivitiesAfter()
    {
        try
        {
            Console.WriteLine("\nStarting TestValidate_MER_FUN_STL_MON_FTR_CRC_F09_ActivitiesAfter");
            _terminalApiKey = _envConfig.MER_FUN_STL_MON_FTR;
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            var planCreateResponse = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!, _requestHeader!,
                "PendingCapture", 9, _terminalApiKey, createPlanDefaultValues);
            await _installmentPlanNumberRefund.SendRefundRequestAsync(_requestHeader!, planCreateResponse.InstallmentPlanNumber,
                "PartialRefund");
            await _mongoHandler.SendMongoQueryWith2KeysAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", planCreateResponse.InstallmentPlanNumber, "Activity", "RED", "Status", "_id");
            Console.WriteLine("Done with TestValidate_MER_FUN_STL_MON_FTR_CRC_F09_ActivitiesAfter");
        }
        catch (Exception exception)
        {
            Assert.Fail("Failed -> TestValidate_MER_FUN_STL_MON_FTR_CRC_F09_ActivitiesAfter \n" + exception + "\n");
        }
    }

    [TestCase(Category = "SkuFunActivitiesAfter")]
    [Test(Description =
         "TestValidate_MER_FUN_STL_MON_FTR_DRC_F12_ActivitiesAfter_partial_Refund_Outstanding with generated merchant"),
     CancelAfter(360 * 1000)]
    public async Task TestValidate_MER_FUN_STL_MON_FTR_DRC_F12_ActivitiesAfter_partial_Refund_Outstanding()
    {
        try
        {
            Console.WriteLine(
                "\nStarting TestValidate_MER_FUN_STL_MON_FTR_DRC_F12_ActivitiesAfter_partial_Refund_Outstanding");
            _terminalApiKey = _envConfig.MER_FUN_STL_MON_FTR;
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            createPlanDefaultValues.paymentMethod.card.cardNumber = Environment.GetEnvironmentVariable("DebitCard")!;
            var planCreateResponse = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!, _requestHeader!,
                "PendingCapture",
                12, _terminalApiKey, createPlanDefaultValues);
            var response = await _processing.SendGetForProcessingAsync(_requestHeader!, planCreateResponse.InstallmentPlanNumber);
            Assert.That(response, Is.EqualTo(""));
            await _mongoHandler.SendMongoQueryForStatusAndIdAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", planCreateResponse.InstallmentPlanNumber, "Status", "_id");
            var jResponse = await _createPaymentInvoice.SendPostRequestCreatePaymentInvoiceAsync(_requestHeader!);
            Assert.That(jResponse, Is.Not.Null);
            var (_, idStatus) = await _mongoHandler.SendMongoQueryWith2KeysAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", planCreateResponse.InstallmentPlanNumber, "Activity", "FND", 
                "Status", "_id");
            var jResponseInvoiced =
                await _invoiced.SendPutRequestInvoicedAsync(_requestHeader!, idStatus, "AutomationRun");
            Assert.That(jResponseInvoiced, Is.Not.Null);
            await _mongoHandler.SendMongoQueryWith3KeysReturn2StringColumnValuesAsync(
                _db!, "Payment_Records",
                "InstallmentPlanNumber", planCreateResponse.InstallmentPlanNumber, "Activity", "FND", 
                "Status", "Invoiced", "Status", "_id");
            await _installmentPlanNumberRefund.SendRefundRequestAsync(_requestHeader!, planCreateResponse.InstallmentPlanNumber,
                "PartialRefund"); 
            await _mongoHandler.SendMongoQueryWith2KeysAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", planCreateResponse.InstallmentPlanNumber, "Activity", "RED", "Status", "_id");
            Console.WriteLine("Done TestValidate_MER_FUN_STL_MON_FTR_DRC_F12_ActivitiesAfter_partial_Refund_Outstanding");
        }
        catch (Exception exception)
        {
            Assert.Fail(
                "Failed -> TestValidate_MER_FUN_STL_MON_FTR_DRC_F12_ActivitiesAfter_partial_Refund_Outstanding \n" +
                exception + "\n");
        }
    }

    [TestCase(Category = "SkuFunActivitiesAfter")]
    [Test(Description =
         "TestValidate_MER_FUN_STL_MON_FTR_DRC_F12_ActivitiesAfter_Refund_Last_Installment with generated merchant"),
     CancelAfter(360 * 1000)]
    public async Task TestValidate_MER_FUN_STL_MON_FTR_DRC_F12_ActivitiesAfter_Refund_Last_Installment()
    {
        try
        {
            Console.WriteLine(
                "\nStarting TestValidate_MER_FUN_STL_MON_FTR_DRC_F12_ActivitiesAfter_Refund_Last_Installment");
            _terminalApiKey = _envConfig.MER_FUN_STL_MON_FTR;
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            createPlanDefaultValues.paymentMethod.card.cardNumber = Environment.GetEnvironmentVariable("DebitCard")!;
            var planCreateResponse = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!, _requestHeader!,
                "PendingCapture",
                12, _terminalApiKey, createPlanDefaultValues);
            var response = await _processing.SendGetForProcessingAsync(_requestHeader!, planCreateResponse.InstallmentPlanNumber);
            Assert.That(response, Is.EqualTo(""));
            await _mongoHandler.SendMongoQueryForStatusAndIdAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", planCreateResponse.InstallmentPlanNumber, "Status", "_id");
            var jResponse = await _createPaymentInvoice.SendPostRequestCreatePaymentInvoiceAsync(_requestHeader!);
            Assert.That(jResponse, Is.Not.Null);
            var (_, idStatus) = await _mongoHandler.SendMongoQueryWith2KeysAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", planCreateResponse.InstallmentPlanNumber, "Activity", "FND",
                "Status", "_id");
            var jResponseInvoiced =
                await _invoiced.SendPutRequestInvoicedAsync(_requestHeader!, idStatus, "AutomationRun");
            Assert.That(jResponseInvoiced, Is.Not.Null);
            await _mongoHandler.SendMongoQueryWith3KeysReturn2StringColumnValuesAsync(
                _db!, "Payment_Records",
                "InstallmentPlanNumber", planCreateResponse.InstallmentPlanNumber, "Activity", "FND"
                , "Status", "Invoiced", "Status", "_id");
            await _installmentPlanNumberRefund.SendRefundRequestAsync(_requestHeader!, planCreateResponse.InstallmentPlanNumber,
                "PartialRefund", "ReduceFromLastInstallment");
            await _mongoHandler.SendMongoQueryWith2KeysAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", planCreateResponse.InstallmentPlanNumber, "Activity", "RED", "Status", "_id");
            Console.WriteLine("Done TestValidate_MER_FUN_STL_MON_FTR_DRC_F12_ActivitiesAfter_Refund_Last_Installment");
        }
        catch (Exception exception)
        {
            Assert.Fail(
                "Failed -> TestValidate_MER_FUN_STL_MON_FTR_DRC_F12_ActivitiesAfter_Refund_Last_Installment \n" +
                exception + "\n");
        }
    }

    [TestCase(Category = "SkuFunActivitiesAfter")]
    [Test(Description =
         "TestValidate_MER_FUN_STL_MON_FTR_DRC_F12_ActivitiesAfter_DEL_OutstandingAmount with generated merchant"),
     CancelAfter(360 * 1000)]
    public async Task TestValidate_MER_FUN_STL_MON_FTR_DRC_F12_ActivitiesAfter_DEL_OutstandingAmount()
    {
        string idStatusCol = null!;
        string batchIdColB = null!;
        string idStatusFnd = null!;
        string batchIdFndB = null!;
        
        try
        {
            Console.WriteLine(
                "\nStarting TestValidate_MER_FUN_STL_MON_FTR_DRC_F12_ActivitiesAfter_DEL_OutstandingAmount");
            _terminalApiKey = _envConfig.MER_FUN_STL_MON_FTR;
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            createPlanDefaultValues.paymentMethod.card.cardNumber = Environment.GetEnvironmentVariable("DebitCard")!;
            var planCreateResponse = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!, _requestHeader!,
                "PendingCapture",
                12, _terminalApiKey, createPlanDefaultValues);
            var response = await _processing.SendGetForProcessingAsync(_requestHeader!, planCreateResponse.InstallmentPlanNumber);
            Assert.That(response, Is.EqualTo(""));
            await _mongoHandler.SendMongoQueryWith2KeysAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", planCreateResponse.InstallmentPlanNumber, "Activity", "COL", 
                "Status", "_id");
            (_, idStatusFnd) = await _mongoHandler.SendMongoQueryWith2KeysAsync(
                _db!, "Payment_Records",
                "InstallmentPlanNumber", planCreateResponse.InstallmentPlanNumber, "Activity", "FND",
                "Status", "_id");
            Assert.That(idStatusFnd, Is.Not.Null);
            await _created.SendPutRequestCreatedAsync(_requestHeader!, idStatusCol);
            await _created.SendPutRequestCreatedAsync(_requestHeader!, idStatusFnd);
            (batchIdColB, var idBatchColB) = await _mongoHandler.SendMongoQueryWith2KeysAsync(
                _db!, "Payment_Records",
                "InstallmentPlanNumber", planCreateResponse.InstallmentPlanNumber, "Activity", "COL",
                "BatchId", "_id");
            Assert.That(batchIdColB, Is.Not.Null);
            Assert.That(idBatchColB, Is.Not.Null);
            (batchIdFndB, var idBatchFndB) = await _mongoHandler.SendMongoQueryWith2KeysAsync(
                _db!, "Payment_Records",
                "InstallmentPlanNumber", planCreateResponse.InstallmentPlanNumber, "Activity", "FND", 
                "BatchId", "_id");
            Assert.That(batchIdFndB, Is.Not.Null);
            Assert.That(idBatchFndB, Is.Not.Null);
            await _settled.SendPutRequestSettledAsync(_requestHeader!, batchIdColB, idStatusCol, true);
            await _settled.SendPutRequestSettledAsync(_requestHeader!, batchIdFndB, idStatusFnd, true);
            await _mongoHandler.SendMongoQueryWith3KeysReturn2StringColumnValuesAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", planCreateResponse.InstallmentPlanNumber, "Activity", "COL", 
                "Status","Acknowledged", "Status", "_id");
            await _mongoHandler.SendMongoQueryWith3KeysReturn2StringColumnValuesAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", planCreateResponse.InstallmentPlanNumber, "Activity", "FND", 
                "Status","Acknowledged", "Status", "_id");
            await _createPaymentInvoice.SendPostRequestCreatePaymentInvoiceAsync(_requestHeader!);
            await _invoiced.SendPutRequestInvoicedAsync(_requestHeader!, idStatusCol, "AutomationRun");
            await _invoiced.SendPutRequestInvoicedAsync(_requestHeader!, idStatusFnd, "AutomationRun");
            await _installmentPlanNumberRefund.SendRefundRequestAsync(_requestHeader!, planCreateResponse.InstallmentPlanNumber,
                "PartialRefund");
            await _mongoHandler.SendMongoQueryWith2KeysAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", planCreateResponse.InstallmentPlanNumber, "Activity", "RED", "Status", "_id");
            Console.WriteLine("TestValidate_MER_FUN_STL_MON_FTR_DRC_F12_ActivitiesAfter_DEL_OutstandingAmount\n");
        }
        catch (Exception exception)
        {
            await Task.Delay(10 * 1000);
            await _settled.SendPutRequestSettledAsync(_requestHeader!, batchIdColB, idStatusCol, false);
            await _settled.SendPutRequestSettledAsync(_requestHeader!, batchIdFndB, idStatusFnd, false);
            Assert.Fail(
                "Failed -> TestValidate_MER_FUN_STL_MON_FTR_DRC_F12_ActivitiesAfter_DEL_OutstandingAmount\n" +
                        exception + "\n");
        }
    }

    [TestCase(Category = "SkuFunActivitiesAfter")]
    [Test(Description = "TestValidate_MER_FUN_STL_MON_FTR_DRC_F12_ActivitiesAfter_DEL_LastInstallment with generated merchant"),
     CancelAfter(360 * 1000)]
    public async Task TestValidate_MER_FUN_STL_MON_FTR_DRC_F12_ActivitiesAfter_DEL_LastInstallment()
    {
        string idStatusCol = null!;
        string batchIdColB = null!;
        string idStatusFnd = null!;
        string batchIdFndB = null!;
        
        try
        {
            Console.WriteLine("\nStarting TestValidate_MER_FUN_STL_MON_FTR_DRC_F12_ActivitiesAfter_DEL_LastInstallment");
            var terminalApiKey = _envConfig.MER_FUN_STL_MON_FTR_DRC_F12;
            Assert.That(_terminalApiKey, Is.Not.EqualTo(""));
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            createPlanDefaultValues.paymentMethod.card.cardNumber = Environment.GetEnvironmentVariable("DebitCard")!;
            var planCreateResponse = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!, _requestHeader!,
                "PendingCapture", 12, terminalApiKey, createPlanDefaultValues);
            var response = await _processing.SendGetForProcessingAsync(_requestHeader!, planCreateResponse.InstallmentPlanNumber);
            Assert.That(response, Is.EqualTo(""));
            (_ , idStatusCol) = await _mongoHandler.SendMongoQueryWith2KeysAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", planCreateResponse.InstallmentPlanNumber, "Activity", "COL",
                "Status", "_id");
            (_, idStatusFnd) = await _mongoHandler.SendMongoQueryWith2KeysAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", planCreateResponse.InstallmentPlanNumber, "Activity", "FND",
                "Status", "_id");
            await _created.SendPutRequestCreatedAsync(_requestHeader!, idStatusCol);
            await _created.SendPutRequestCreatedAsync(_requestHeader!, idStatusFnd);
            (batchIdColB, var idBatchColB) = await _mongoHandler.SendMongoQueryWith2KeysAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", planCreateResponse.InstallmentPlanNumber, "Activity", "COL", "BatchId", "_id");
            Assert.That(batchIdColB, Is.Not.Null);
            Assert.That(idBatchColB, Is.Not.Null);
            (batchIdFndB, var idBatchFndB) = await _mongoHandler.SendMongoQueryWith2KeysAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", planCreateResponse.InstallmentPlanNumber, "Activity", "FND", "BatchId", "_id");
            Assert.That(batchIdFndB, Is.Not.Null);
            Assert.That(idBatchFndB, Is.Not.Null);
            await _settled.SendPutRequestSettledAsync(_requestHeader!, batchIdColB, idStatusCol, true);
            await _settled.SendPutRequestSettledAsync(_requestHeader!, batchIdFndB, idStatusFnd, true);
            await _mongoHandler.SendMongoQueryWith3KeysReturn2StringColumnValuesAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", planCreateResponse.InstallmentPlanNumber, "Activity", "COL", 
                "Status","Acknowledged", "Status", "_id");
            await _mongoHandler.SendMongoQueryWith3KeysReturn2StringColumnValuesAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", planCreateResponse.InstallmentPlanNumber, "Activity", "FND", 
                "Status","Acknowledged", "Status", "_id");
            await _createPaymentInvoice.SendPostRequestCreatePaymentInvoiceAsync(_requestHeader!);
            await _invoiced.SendPutRequestInvoicedAsync(_requestHeader!, idStatusCol, "AutomationRun");
            await _invoiced.SendPutRequestInvoicedAsync(_requestHeader!, idStatusFnd, "AutomationRun");
            await _installmentPlanNumberRefund.SendRefundRequestAsync(_requestHeader!, planCreateResponse.InstallmentPlanNumber,
                "PartialRefund", "ReduceFromLastInstallment");
            await _mongoHandler.SendMongoQueryWith2KeysAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", planCreateResponse.InstallmentPlanNumber, "Activity", "RED", "Status", "_id");
            Console.WriteLine("TestValidate_MER_FUN_STL_MON_FTR_DRC_F12_ActivitiesAfter_DEL_LastInstallment\n");
        }
        catch (Exception exception)
        {
            await Task.Delay(10 * 1000);
            await _settled.SendPutRequestSettledAsync(_requestHeader!, batchIdColB, idStatusCol, false);
            await _settled.SendPutRequestSettledAsync(_requestHeader!, batchIdFndB, idStatusFnd, false);
            Assert.Fail("Failed -> TestValidate_MER_FUN_STL_MON_FTR_DRC_F12_ActivitiesAfter_DEL_LastInstallment\n" +
                        exception + "\n");
        }
    }

    [TestCase(Category = "SkuFunActivitiesAfter")]
    [Test(Description = "TestValidate_DBS_FUN_STL_MON_FTR_CRC_F01_ActivitiesAfter with generated merchant"),
     CancelAfter(360 * 1000)]
    public async Task TestValidate_DBS_FUN_STL_MON_FTR_CRC_F01_ActivitiesAfter()
    {
        string idStatusCol = null!;
        string batchIdColB = null!;
        string idStatusFnd = null!;
        string batchIdFndB = null!;
        try
        {
            Console.WriteLine("\nStarting TestValidate_DBS_FUN_STL_MON_FTR_CRC_F01_ActivitiesAfter");
            _terminalApiKey = _envConfig.DBS_FUN_STL_MON_FTR;
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            var planCreateResponse = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!, _requestHeader!,
                "PendingCapture", 1, _terminalApiKey, createPlanDefaultValues);
            var response = await _processing.SendGetForProcessingAsync(_requestHeader!, planCreateResponse.InstallmentPlanNumber);
            Assert.That(response, Is.EqualTo(""));
            await Task.Delay(5 * 1000);
            (_, idStatusCol) = await _mongoHandler.SendMongoQueryWith2KeysAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", planCreateResponse.InstallmentPlanNumber, "Activity", "COL", "Status", "_id");
            (_, idStatusFnd) = await _mongoHandler.SendMongoQueryWith2KeysAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", planCreateResponse.InstallmentPlanNumber, "Activity", "FND", "Status", "_id");
            await _created.SendPutRequestCreatedAsync(_requestHeader!, idStatusCol);
            await _created.SendPutRequestCreatedAsync(_requestHeader!, idStatusFnd);
            (batchIdColB, var idBatchColB) = await _mongoHandler.SendMongoQueryWith2KeysAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", planCreateResponse.InstallmentPlanNumber, "Activity", "COL", "BatchId", "_id");
            Assert.That(batchIdColB, Is.Not.Null);
            Assert.That(idBatchColB, Is.Not.Null);
            (batchIdFndB, var idBatchFndB) = await _mongoHandler.SendMongoQueryWith2KeysAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", planCreateResponse.InstallmentPlanNumber, "Activity", "FND", "BatchId", "_id");
            Assert.That(batchIdFndB, Is.Not.Null);
            Assert.That(idBatchFndB, Is.Not.Null);
            await _settled.SendPutRequestSettledAsync(_requestHeader!, batchIdColB, idStatusCol, true);
            await _settled.SendPutRequestSettledAsync(_requestHeader!, batchIdFndB, idStatusFnd, true);
            await _mongoHandler.SendMongoQueryWith3KeysReturn2StringColumnValuesAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", planCreateResponse.InstallmentPlanNumber, "Activity", "COL", 
                "Status","Acknowledged", "Status", "_id");
            await _mongoHandler.SendMongoQueryWith3KeysReturn2StringColumnValuesAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", planCreateResponse.InstallmentPlanNumber, "Activity", "FND", 
                "Status","Acknowledged", "Status", "_id");
            await _createPaymentInvoice.SendPostRequestCreatePaymentInvoiceAsync(_requestHeader!);
            await _invoiced.SendPutRequestInvoicedAsync(_requestHeader!, idStatusCol, "AutomationRun");
            await _invoiced.SendPutRequestInvoicedAsync(_requestHeader!, idStatusFnd, "AutomationRun");
            await _installmentPlanNumberRefund.SendRefundRequestAsync(_requestHeader!, planCreateResponse.InstallmentPlanNumber,
                "PartialRefund", "FutureInstallmentsLast");
            await _mongoHandler.SendMongoQueryWith2KeysAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", planCreateResponse.InstallmentPlanNumber, "Activity", "REF", "Status", "_id");
            Console.WriteLine("Done TestValidate_DBS_FUN_STL_MON_FTR_CRC_F01_ActivitiesAfter");
        }
        catch (Exception exception)
        {
            await Task.Delay(10 * 1000);
            await _settled.SendPutRequestSettledAsync(_requestHeader!, batchIdColB, idStatusCol, false);
            await _settled.SendPutRequestSettledAsync(_requestHeader!, batchIdFndB, idStatusFnd, false);
            Assert.Fail("Failed -> TestValidate_DBS_FUN_STL_MON_FTR_CRC_F01_ActivitiesAfter \n" + exception + "\n");
        }
    }

    [TestCase(Category = "SkuFunActivitiesAfter")]
    [Test(Description =
         "TestValidate_DBS_FUN_STL_MON_FTR_CRC_F01_ActivitiesAfter_PartialRefund with generated merchant"),
     CancelAfter(360 * 1000)]
    public async Task TestValidate_DBS_FUN_STL_MON_FTR_CRC_F01_ActivitiesAfter_PartialRefund()
    {
        string idStatusCol = null!;
        string batchIdColB = null!;
        string idStatusFnd = null!;
        string batchIdFndB = null!;
        
        try
        {
            Console.WriteLine("\nStarting TestValidate_DBS_FUN_STL_MON_FTR_CRC_F01_ActivitiesAfter_PartialRefund");
            _terminalApiKey = _envConfig.DBS_FUN_STL_MON_FTR;
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            var planCreateResponse = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!, _requestHeader!,
                "PendingCapture", 1, _terminalApiKey, createPlanDefaultValues);
            var response = await _processing.SendGetForProcessingAsync(_requestHeader!, planCreateResponse.InstallmentPlanNumber);
            Assert.That(response, Is.EqualTo(""));
            (_, idStatusCol) = await _mongoHandler.SendMongoQueryWith2KeysAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", planCreateResponse.InstallmentPlanNumber, "Activity", "COL", "Status", "_id");
            (_, idStatusFnd) = await _mongoHandler.SendMongoQueryWith2KeysAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", planCreateResponse.InstallmentPlanNumber, "Activity", "FND", "Status", "_id");
            await _created.SendPutRequestCreatedAsync(_requestHeader!, idStatusCol);
            await _created.SendPutRequestCreatedAsync(_requestHeader!, idStatusFnd);
            (batchIdColB, var idBatchColB) = await _mongoHandler.SendMongoQueryWith2KeysAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", planCreateResponse.InstallmentPlanNumber, "Activity", "COL", "BatchId", "_id");
            Assert.That(batchIdColB, Is.Not.Null);
            Assert.That(idBatchColB, Is.Not.Null);
            (batchIdFndB, var idBatchFndB) = await _mongoHandler.SendMongoQueryWith2KeysAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", planCreateResponse.InstallmentPlanNumber, "Activity", "FND", "BatchId", "_id");
            Assert.That(batchIdFndB, Is.Not.Null);
            Assert.That(idBatchFndB, Is.Not.Null);
            await _settled.SendPutRequestSettledAsync(_requestHeader!, batchIdColB, idStatusCol, true);
            await _settled.SendPutRequestSettledAsync(_requestHeader!, batchIdFndB, idStatusFnd, true);
            await _mongoHandler.SendMongoQueryWith3KeysReturn2StringColumnValuesAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", planCreateResponse.InstallmentPlanNumber, "Activity", "COL", 
                "Status","Acknowledged", "Status", "_id");
            await _mongoHandler.SendMongoQueryWith3KeysReturn2StringColumnValuesAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", planCreateResponse.InstallmentPlanNumber, "Activity", "FND", 
                "Status","Acknowledged", "Status", "_id");
            await _createPaymentInvoice.SendPostRequestCreatePaymentInvoiceAsync(_requestHeader!);
            await _invoiced.SendPutRequestInvoicedAsync(_requestHeader!, idStatusCol, "AutomationRun");
            await _invoiced.SendPutRequestInvoicedAsync(_requestHeader!, idStatusFnd, "AutomationRun");
            await _installmentPlanNumberRefund.SendRefundRequestAsync(_requestHeader!, planCreateResponse.InstallmentPlanNumber,
                "PartialRefund");
            await _mongoHandler.SendMongoQueryWith2KeysAsync(_db!, "Payment_Records", 
                "InstallmentPlanNumber", planCreateResponse.InstallmentPlanNumber, "Activity", "REF", "Status", "_id");
            Console.WriteLine("Done TestValidate_DBS_FUN_STL_MON_FTR_CRC_F01_ActivitiesAfter_PartialRefund");
        }
        catch (Exception exception)
        {
            await Task.Delay(10 * 1000);
            await _settled.SendPutRequestSettledAsync(_requestHeader!, batchIdColB, idStatusCol, false);
            await _settled.SendPutRequestSettledAsync(_requestHeader!, batchIdFndB, idStatusFnd, false);
            Assert.Fail("Failed -> TestValidate_DBS_FUN_STL_MON_FTR_CRC_F01_ActivitiesAfter_PartialRefund \n" +
                        exception + "\n");
        }
    }

    [TestCase(Category = "SkuFunActivitiesAfter")]
    [Test(Description = "TestValidate_DBS_FUN_STL_MON_FTR_CRC_F03_ActivitiesAfter_Past_Charge with generated merchant"),
     CancelAfter(360 * 1000)]
    public async Task TestValidate_DBS_FUN_STL_MON_FTR_CRC_F03_ActivitiesAfter_Past_Charge()
    {
        try
        {
            Console.WriteLine("\nStarting TestValidate_DBS_FUN_STL_MON_FTR_CRC_F03_ActivitiesAfter_Past_Charge");
            var terminalApiKey = _envConfig.DBS_FUN_STL_MON_FTR_CRC_F03;
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            var planCreateResponse = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!, _requestHeader!,
                "PendingCapture",
                3, terminalApiKey, createPlanDefaultValues);
            var response = await _processing.SendGetForProcessingAsync(_requestHeader!, planCreateResponse.InstallmentPlanNumber);
            Assert.That(response, Is.EqualTo(""));
            await _mongoHandler.SendMongoQueryWith2KeysAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", planCreateResponse.InstallmentPlanNumber, "Activity", "COL", "Status", "_id");
            var (_, idStatusFnd) = await _mongoHandler.SendMongoQueryWith2KeysAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", planCreateResponse.InstallmentPlanNumber, "Activity", "FND", "Status", "_id");
            var responseCreatePaymentInvoice =
                await _createPaymentInvoice.SendPostRequestCreatePaymentInvoiceAsync(_requestHeader!);
            Assert.That(responseCreatePaymentInvoice, Is.Not.Null);
            var invoicedResponse =
                await _invoiced.SendPutRequestInvoicedAsync(_requestHeader!, idStatusFnd, "AutomationRun");
            Assert.That(invoicedResponse, Is.Not.Null);
            await _mongoHandler.SendMongoQueryWith3KeysReturn2StringColumnValuesAsync(
                _db!, "Payment_Records", "InstallmentPlanNumber", planCreateResponse.InstallmentPlanNumber, 
                "Activity", "FND", "Status", "Invoiced","Status", "_id");
            await _installmentPlanNumberRefund.SendRefundRequestAsync(_requestHeader!, planCreateResponse.InstallmentPlanNumber,
                "PartialRefund", "FutureInstallmentsNotAllowed");
            await _mongoHandler.SendMongoQueryWith2KeysAsync(_db!, "Payment_Records", 
                "InstallmentPlanNumber", planCreateResponse.InstallmentPlanNumber, "Activity", "REF", 
                "Status", "_id");
            Console.WriteLine("Done TestValidate_DBS_FUN_STL_MON_FTR_CRC_F03_ActivitiesAfter_Past_Charge");
        }
        catch (Exception exception)
        {
            Assert.Fail("Failed -> TestValidate_DBS_FUN_STL_MON_FTR_CRC_F03_ActivitiesAfter_Past_Charge \n" +
                        exception + "\n");
        }
    }

    [TestCase(Category = "SkuFunActivitiesAfter")]
    [Test(Description =
         "TestValidate_DBS_FUN_STL_MON_FTR_CRC_F03_ActivitiesAfter_Past_Charge_First with generated merchant"),
     CancelAfter(360 * 1000)]
    public async Task TestValidate_DBS_FUN_STL_MON_FTR_CRC_F03_ActivitiesAfter_Past_Charge_First()
    {
        string idStatusCol = null!;
        string batchIdColB = null!;
        string idStatusFnd = null!;
        string batchIdFndB = null!;
        
        try
        {
            Console.WriteLine("\nStarting TestValidate_DBS_FUN_STL_MON_FTR_CRC_F03_ActivitiesAfter_Past_Charge_First");
            _terminalApiKey = _envConfig.DBS_FUN_STL_MON_FTR;
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            var planCreateResponse = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!, _requestHeader!,
                "PendingCapture",
                3, _terminalApiKey, createPlanDefaultValues);
            var response = await _processing.SendGetForProcessingAsync(_requestHeader!, planCreateResponse.InstallmentPlanNumber);
            Assert.That(response, Is.EqualTo(""));
            (_, idStatusCol) = await _mongoHandler.SendMongoQueryWith2KeysAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", planCreateResponse.InstallmentPlanNumber, "Activity", "COL", 
                "Status", "_id");
            (_, idStatusFnd) = await _mongoHandler.SendMongoQueryWith2KeysAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", planCreateResponse.InstallmentPlanNumber, "Activity", "FND",
                "Status", "_id");
            await _created.SendPutRequestCreatedAsync(_requestHeader!, idStatusCol);
            await _created.SendPutRequestCreatedAsync(_requestHeader!, idStatusFnd);
            (batchIdColB, var idBatchColB) = await _mongoHandler.SendMongoQueryWith2KeysAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", planCreateResponse.InstallmentPlanNumber, "Activity", "COL", "BatchId", "_id");
            Assert.That(batchIdColB, Is.Not.Null);
            Assert.That(idBatchColB, Is.Not.Null);
            (batchIdFndB, var idBatchFndB) = await _mongoHandler.SendMongoQueryWith2KeysAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", planCreateResponse.InstallmentPlanNumber, "Activity", "FND", "BatchId", "_id");
            Assert.That(batchIdFndB, Is.Not.Null);
            Assert.That(idBatchFndB, Is.Not.Null);
            await _settled.SendPutRequestSettledAsync(_requestHeader!, batchIdColB, idStatusCol, true);
            await _settled.SendPutRequestSettledAsync(_requestHeader!, batchIdFndB, idStatusFnd, true);
            await _mongoHandler.SendMongoQueryWith3KeysReturn2StringColumnValuesAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", planCreateResponse.InstallmentPlanNumber, "Activity", "COL", 
                "Status","Acknowledged", "Status", "_id");
            await _mongoHandler.SendMongoQueryWith3KeysReturn2StringColumnValuesAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", planCreateResponse.InstallmentPlanNumber, "Activity", "FND", 
                "Status","Acknowledged", "Status", "_id");
            await _createPaymentInvoice.SendPostRequestCreatePaymentInvoiceAsync(_requestHeader!);
            await _invoiced.SendPutRequestInvoicedAsync(_requestHeader!, idStatusCol, "AutomationRun");
            await _invoiced.SendPutRequestInvoicedAsync(_requestHeader!, idStatusFnd, "AutomationRun");
            await _installmentPlanNumberRefund.SendRefundRequestAsync(_requestHeader!, planCreateResponse.InstallmentPlanNumber,
                "PartialRefund", "FutureInstallmentsNotAllowed");
            await _mongoHandler.SendMongoQueryWith2KeysAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", planCreateResponse.InstallmentPlanNumber, "Activity", "REF",
                "Status", "_id");
            Console.WriteLine("Done TestValidate_DBS_FUN_STL_MON_FTR_CRC_F03_ActivitiesAfter_Past_Charge_First");
        }
        catch (Exception exception)
        {
            await Task.Delay(10 * 1000);
            await _settled.SendPutRequestSettledAsync(_requestHeader!, batchIdColB, idStatusCol, false);
            await _settled.SendPutRequestSettledAsync(_requestHeader!, batchIdFndB, idStatusFnd, false);
            Assert.Fail("Failed -> TestValidate_DBS_FUN_STL_MON_FTR_CRC_F03_ActivitiesAfter_Past_Charge_First \n" +
                        exception + "\n");
        }
    }

    [TestCase(Category = "SkuFunActivitiesAfter")]
    [Test(Description =
         "TestValidate_DBS_FUN_STL_MON_FTR_CRC_F03_ActivitiesAfter_Last_Installment with generated merchant"),
     CancelAfter(360 * 1000)]
    public async Task TestValidate_DBS_FUN_STL_MON_FTR_CRC_F03_ActivitiesAfter_Last_Installment()
    {
        try
        {
            Console.WriteLine("\nStarting TestValidate_DBS_FUN_STL_MON_FTR_CRC_F03_ActivitiesAfter_Last_Installment");
            var terminalApiKey = _envConfig.DBS_FUN_STL_MON_FTR_CRC_F03;
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            var planCreateResponse = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!, _requestHeader!,
                "PendingCapture",
                3, terminalApiKey, createPlanDefaultValues);
            var response = await _processing.SendGetForProcessingAsync(_requestHeader!, planCreateResponse.InstallmentPlanNumber);
            Assert.That(response, Is.EqualTo(""));
            await _mongoHandler.SendMongoQueryWith2KeysAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", planCreateResponse.InstallmentPlanNumber, "Activity", "COL", "Status", "_id");
            var (_, idStatusFnd) = await _mongoHandler.SendMongoQueryWith2KeysAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", planCreateResponse.InstallmentPlanNumber, "Activity", "FND", "Status", "_id");
            var responseCreate = await _createPaymentInvoice.SendPostRequestCreatePaymentInvoiceAsync(_requestHeader!);
            Assert.That(responseCreate, Is.Not.Null);
            var invoicedResponse =
                await _invoiced.SendPutRequestInvoicedAsync(_requestHeader!, idStatusFnd, "AutomationRun");
            Assert.That(invoicedResponse, Is.Not.Null);
            await _mongoHandler.SendMongoQueryWith3KeysReturn2StringColumnValuesAsync(_db!, "Payment_Records", 
                "InstallmentPlanNumber", planCreateResponse.InstallmentPlanNumber, "Activity", "FND", "Status", "Invoiced","Status", "_id");
            await _installmentPlanNumberRefund.SendRefundRequestAsync(_requestHeader!, planCreateResponse.InstallmentPlanNumber,
                "PartialRefund", "ReduceFromLastInstallment");
            await _mongoHandler.SendMongoQueryWith2KeysAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", planCreateResponse.InstallmentPlanNumber, "Activity", "RED", "Status", "_id");
            Console.WriteLine("Done TestValidate_DBS_FUN_STL_MON_FTR_CRC_F03_ActivitiesAfter_Last_Installment");
        }
        catch (Exception exception)
        {
            Assert.Fail("Failed -> TestValidate_DBS_FUN_STL_MON_FTR_CRC_F03_ActivitiesAfter_Last_Installment \n" +
                        exception + "\n");
        }
    }

    [TestCase(Category = "SkuFunActivitiesAfter")]
    [Test(Description = "TestValidate_DBS_FUN_STL_MON_FTR_CRC_F09_ActivitiesAfter_Ref_Red with generated merchant"),
     CancelAfter(360 * 1000)]
    public async Task TestValidate_DBS_FUN_STL_MON_FTR_CRC_F09_ActivitiesAfter_Ref_Red()
    {
        string idStatusCol = null!;
        string batchIdColB = null!;
        string idStatusFnd = null!;
        string batchIdFndB = null!;
        
        try
        {
            Console.WriteLine("\nStarting TestValidate_DBS_FUN_STL_MON_FTR_CRC_F09_ActivitiesAfter_Ref_Red");
            var terminalApiKey = _envConfig.DBS_FUN_STL_MON_FTR_CRC_F09;
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            var planCreateResponse = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!, _requestHeader!,
                "PendingCapture",
                9, terminalApiKey, createPlanDefaultValues);
            var response = await _processing.SendGetForProcessingAsync(_requestHeader!, planCreateResponse.InstallmentPlanNumber);
            Assert.That(response, Is.Not.Null);
            (_, idStatusCol) = await _mongoHandler.SendMongoQueryWith2KeysAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", planCreateResponse.InstallmentPlanNumber, "Activity", "COL", "Status", "_id");
            (_, idStatusFnd) = await _mongoHandler.SendMongoQueryWith2KeysAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", planCreateResponse.InstallmentPlanNumber, "Activity", "FND", "Status", "_id");
            await _created.SendPutRequestCreatedAsync(_requestHeader!, idStatusCol);
            await _created.SendPutRequestCreatedAsync(_requestHeader!, idStatusFnd);
            (batchIdColB, var idBatchColB) = await _mongoHandler.SendMongoQueryWith2KeysAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", planCreateResponse.InstallmentPlanNumber, "Activity", "COL", "BatchId", "_id");
            Assert.That(batchIdColB, Is.Not.Null);
            Assert.That(idBatchColB, Is.Not.Null);
            (batchIdFndB, var idBatchFndB) = await _mongoHandler.SendMongoQueryWith2KeysAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", planCreateResponse.InstallmentPlanNumber, "Activity", "FND", "BatchId", "_id");
            Assert.That(batchIdFndB, Is.Not.Null);
            Assert.That(idBatchFndB, Is.Not.Null);
            await _settled.SendPutRequestSettledAsync(_requestHeader!, batchIdColB, idStatusCol, true);
            await _settled.SendPutRequestSettledAsync(_requestHeader!, batchIdFndB, idStatusFnd, true);
            await _mongoHandler.SendMongoQueryWith3KeysReturn2StringColumnValuesAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", planCreateResponse.InstallmentPlanNumber, "Activity", "COL", 
                "Status","Acknowledged", "Status", "_id");
            await _mongoHandler.SendMongoQueryWith3KeysReturn2StringColumnValuesAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", planCreateResponse.InstallmentPlanNumber, "Activity", "FND", 
                "Status","Acknowledged", "Status", "_id");
            await _createPaymentInvoice.SendPostRequestCreatePaymentInvoiceAsync(_requestHeader!);
            await _invoiced.SendPutRequestInvoicedAsync(_requestHeader!, idStatusCol, "AutomationRun");
            await _invoiced.SendPutRequestInvoicedAsync(_requestHeader!, idStatusFnd, "AutomationRun");
            await _installmentPlanNumberRefund.SendRefundRequestAsync(_requestHeader!, planCreateResponse.InstallmentPlanNumber,
                "FullRefund");
            await _mongoHandler.SendMongoQueryWith2KeysAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", planCreateResponse.InstallmentPlanNumber, "Activity", "REF", "Status", "_id");
            await _mongoHandler.SendMongoQueryWith2KeysAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", planCreateResponse.InstallmentPlanNumber, "Activity", "RED", "Status", "_id");
            Console.WriteLine("Done with TestValidate_DBS_FUN_STL_MON_FTR_CRC_F09_ActivitiesAfter_Ref_Red\n");
        }
        catch (Exception exception)
        {
            await Task.Delay(10 * 1000);
            await _settled.SendPutRequestSettledAsync(_requestHeader!, batchIdColB, idStatusCol, false);
            await _settled.SendPutRequestSettledAsync(_requestHeader!, batchIdFndB, idStatusFnd, false);
            Assert.Fail("Failed -> TestValidate_DBS_FUN_STL_MON_FTR_CRC_F09_ActivitiesAfter_Ref_Red\n" + exception + "\n");
        }
    }

    [TestCase(Category = "SkuFunActivitiesAfter")]
    [Test(Description = "TestValidate_DBS_FUN_STL_MON_FTR_CRC_F09_ActivitiesAfter_Ref with generated merchant"),
     CancelAfter(360 * 1000)]
    public async Task TestValidate_DBS_FUN_STL_MON_FTR_CRC_F09_ActivitiesAfter_Ref()
    {
        string idStatusCol = null!;
        string batchIdColB = null!;
        string idStatusFnd = null!;
        string batchIdFndB = null!;
        
        try
        {
            Console.WriteLine("\nStarting TestValidate_DBS_FUN_STL_MON_FTR_CRC_F09_ActivitiesAfter_Ref");
            _terminalApiKey = _envConfig.DBS_FUN_STL_MON_FTR;
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            var planCreateResponse = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!, _requestHeader!,
                "PendingCapture",
                9, _terminalApiKey, createPlanDefaultValues);
            var response = await _processing.SendGetForProcessingAsync(_requestHeader!, planCreateResponse.InstallmentPlanNumber);
            Assert.That(response, Is.Not.Null);
            (_, idStatusCol) = await _mongoHandler.SendMongoQueryWith2KeysAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", planCreateResponse.InstallmentPlanNumber, "Activity", "COL", "Status", "_id");
            (_, idStatusFnd) = await _mongoHandler.SendMongoQueryWith2KeysAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", planCreateResponse.InstallmentPlanNumber, "Activity", "FND", "Status", "_id");
            await _created.SendPutRequestCreatedAsync(_requestHeader!, idStatusCol);
            await _created.SendPutRequestCreatedAsync(_requestHeader!, idStatusFnd);
            (batchIdColB, var idBatchColB) = await _mongoHandler.SendMongoQueryWith2KeysAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", planCreateResponse.InstallmentPlanNumber, "Activity", "COL", "BatchId", "_id");
            Assert.That(batchIdColB, Is.Not.Null);
            Assert.That(idBatchColB, Is.Not.Null);
            (batchIdFndB, var idBatchFndB) = await _mongoHandler.SendMongoQueryWith2KeysAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", planCreateResponse.InstallmentPlanNumber, "Activity", "FND", "BatchId", "_id");
            Assert.That(batchIdFndB, Is.Not.Null);
            Assert.That(idBatchFndB, Is.Not.Null);
            await _settled.SendPutRequestSettledAsync(_requestHeader!, batchIdColB, idStatusCol, true);
            await _settled.SendPutRequestSettledAsync(_requestHeader!, batchIdFndB, idStatusFnd, true);
            await _mongoHandler.SendMongoQueryWith3KeysReturn2StringColumnValuesAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", planCreateResponse.InstallmentPlanNumber, "Activity", "COL", 
                "Status","Acknowledged", "Status", "_id");
            await _mongoHandler.SendMongoQueryWith3KeysReturn2StringColumnValuesAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", planCreateResponse.InstallmentPlanNumber, "Activity", "FND", 
                "Status","Acknowledged", "Status", "_id");
            await _createPaymentInvoice.SendPostRequestCreatePaymentInvoiceAsync(_requestHeader!);
            await _invoiced.SendPutRequestInvoicedAsync(_requestHeader!, idStatusCol, "AutomationRun");
            await _invoiced.SendPutRequestInvoicedAsync(_requestHeader!, idStatusFnd, "AutomationRun");
            await _installmentPlanNumberRefund.SendRefundRequestAsync(_requestHeader!, planCreateResponse.InstallmentPlanNumber,
                "PartialRefund");
            await _mongoHandler.SendMongoQueryWith2KeysAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", planCreateResponse.InstallmentPlanNumber, "Activity", "RED", "Status", "_id");
            Console.WriteLine("Done with TestValidate_DBS_FUN_STL_MON_FTR_CRC_F09_ActivitiesAfter_Ref\n");
        }
        catch (Exception exception)
        {
            await Task.Delay(10 * 1000);
            await _settled.SendPutRequestSettledAsync(_requestHeader!, batchIdColB, idStatusCol, false);
            await _settled.SendPutRequestSettledAsync(_requestHeader!, batchIdFndB, idStatusFnd, false);
            Assert.Fail("Failed -> TestValidate_DBS_FUN_STL_MON_FTR_CRC_F09_ActivitiesAfter_Ref\n" + exception + "\n");
        }
    }

    [TestCase(Category = "SkuFunActivitiesAfter")]
    [Test(Description = "TestValidate_PCO_FUN_STL_MON_FTR_CRC_F01_ActivitiesAfter with generated merchant"),
     CancelAfter(360 * 1000)]
    public async Task TestValidate_PCO_FUN_STL_MON_FTR_CRC_F01_ActivitiesAfter()
    {
        string idStatusCol = null!;
        string batchIdColB = null!;
        string idStatusFnd = null!;
        string batchIdFndB = null!;
        
        try
        {
            Console.WriteLine("\nStarting TestValidate_PCO_FUN_STL_MON_FTR_CRC_F01_ActivitiesAfter");
            _terminalApiKey = _envConfig.PCO_FUN_STL_MON_FTR;
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            var planCreateResponse = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!, _requestHeader!,
                "PendingCapture", 1, _terminalApiKey, createPlanDefaultValues);
            var response = await _processing.SendGetForProcessingAsync(_requestHeader!, planCreateResponse.InstallmentPlanNumber);
            Assert.That(response, Is.EqualTo(""));
            (_, idStatusCol) = await _mongoHandler.SendMongoQueryWith2KeysAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", planCreateResponse.InstallmentPlanNumber, "Activity", "COL", "Status", "_id");
            (_, idStatusFnd) = await _mongoHandler.SendMongoQueryWith2KeysAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", planCreateResponse.InstallmentPlanNumber, "Activity", "FND", "Status", "_id");
            await _created.SendPutRequestCreatedAsync(_requestHeader!, idStatusCol);
            await _created.SendPutRequestCreatedAsync(_requestHeader!, idStatusFnd);
            (batchIdColB, var idBatchColB) = await _mongoHandler.SendMongoQueryWith2KeysAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", planCreateResponse.InstallmentPlanNumber, "Activity", "COL", "BatchId", "_id");
            Assert.That(batchIdColB, Is.Not.Null);
            Assert.That(idBatchColB, Is.Not.Null);
            (batchIdFndB, var idBatchFndB) = await _mongoHandler.SendMongoQueryWith2KeysAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", planCreateResponse.InstallmentPlanNumber, "Activity", "FND", "BatchId", "_id");
            Assert.That(batchIdFndB, Is.Not.Null);
            Assert.That(idBatchFndB, Is.Not.Null);
            await _settled.SendPutRequestSettledAsync(_requestHeader!, batchIdColB, idStatusCol, true);
            await _settled.SendPutRequestSettledAsync(_requestHeader!, batchIdFndB, idStatusFnd, true);
            await _mongoHandler.SendMongoQueryWith3KeysReturn2StringColumnValuesAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", planCreateResponse.InstallmentPlanNumber, "Activity", "COL", 
                "Status","Acknowledged", "Status", "_id");
            await _mongoHandler.SendMongoQueryWith3KeysReturn2StringColumnValuesAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", planCreateResponse.InstallmentPlanNumber, "Activity", "FND", 
                "Status","Acknowledged", "Status", "_id");
            await _createPaymentInvoice.SendPostRequestCreatePaymentInvoiceAsync(_requestHeader!);
            await _invoiced.SendPutRequestInvoicedAsync(_requestHeader!, idStatusCol, "AutomationRun");
            await _invoiced.SendPutRequestInvoicedAsync(_requestHeader!, idStatusFnd, "AutomationRun");
            await _installmentPlanNumberRefund.SendRefundRequestAsync(_requestHeader!, planCreateResponse.InstallmentPlanNumber,
                "FullRefund");
            await _mongoHandler.SendMongoQueryWith2KeysAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", planCreateResponse.InstallmentPlanNumber, "Activity", "REF", "Status", "_id");
            Console.WriteLine("Done TestValidate_PCO_FUN_STL_MON_FTR_CRC_F01_ActivitiesAfter");
        }
        catch (Exception exception)
        {
            await Task.Delay(10 * 1000);
            await _settled.SendPutRequestSettledAsync(_requestHeader!, batchIdColB, idStatusCol, false);
            await _settled.SendPutRequestSettledAsync(_requestHeader!, batchIdFndB, idStatusFnd, false);
            Assert.Fail("Failed -> TestValidate_PCO_FUN_STL_MON_FTR_CRC_F01_ActivitiesAfter \n" + exception + "\n");
        }
    }

    [TestCase(Category = "SkuFunActivitiesAfter")]
    [Test(Description =
         "TestValidate_PCO_FUN_STL_MON_FTR_CRC_F01_ActivitiesAfter_ReduceFromLastInstallment with generated merchant"),
     CancelAfter(360 * 1000)]
    public async Task TestValidate_PCO_FUN_STL_MON_FTR_CRC_F01_ActivitiesAfter_ReduceFromLastInstallment()
    {
        string idStatusCol = null!;
        string batchIdColB = null!;
        string idStatusFnd = null!;
        string batchIdFndB = null!;
        
        try
        {
            Console.WriteLine(
                "\nStarting TestValidate_PCO_FUN_STL_MON_FTR_CRC_F01_ActivitiesAfter_ReduceFromLastInstallment");
            _terminalApiKey = _envConfig.PCO_FUN_STL_MON_FTR;
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            var planCreateResponse = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!, _requestHeader!,
                "PendingCapture", 1, _terminalApiKey, createPlanDefaultValues);
            var response = await _processing.SendGetForProcessingAsync(_requestHeader!, planCreateResponse.InstallmentPlanNumber);
            Assert.That(response, Is.EqualTo(""));
            (_, idStatusCol) = await _mongoHandler.SendMongoQueryWith2KeysAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", planCreateResponse.InstallmentPlanNumber, "Activity", "COL", "Status", "_id");
            (_, idStatusFnd) = await _mongoHandler.SendMongoQueryWith2KeysAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", planCreateResponse.InstallmentPlanNumber, "Activity", "FND", "Status", "_id");
            await _created.SendPutRequestCreatedAsync(_requestHeader!, idStatusCol);
            await _created.SendPutRequestCreatedAsync(_requestHeader!, idStatusFnd);
            (batchIdColB, var idBatchColB) = await _mongoHandler.SendMongoQueryWith2KeysAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", planCreateResponse.InstallmentPlanNumber, "Activity", "COL", "BatchId", "_id");
            Assert.That(batchIdColB, Is.Not.Null);
            Assert.That(idBatchColB, Is.Not.Null);
            (batchIdFndB, var idBatchFndB) = await _mongoHandler.SendMongoQueryWith2KeysAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", planCreateResponse.InstallmentPlanNumber, "Activity", "FND", "BatchId", "_id");
            Assert.That(batchIdFndB, Is.Not.Null);
            Assert.That(idBatchFndB, Is.Not.Null);
            await _settled.SendPutRequestSettledAsync(_requestHeader!, batchIdColB, idStatusCol, true);
            await _settled.SendPutRequestSettledAsync(_requestHeader!, batchIdFndB, idStatusFnd, true);
            await _mongoHandler.SendMongoQueryWith3KeysReturn2StringColumnValuesAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", planCreateResponse.InstallmentPlanNumber, "Activity", "COL", 
                "Status","Acknowledged", "Status", "_id");
            await _mongoHandler.SendMongoQueryWith3KeysReturn2StringColumnValuesAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", planCreateResponse.InstallmentPlanNumber, "Activity", "FND", 
                "Status","Acknowledged", "Status", "_id");
            await _createPaymentInvoice.SendPostRequestCreatePaymentInvoiceAsync(_requestHeader!);
            await _invoiced.SendPutRequestInvoicedAsync(_requestHeader!, idStatusCol, "AutomationRun");
            await _invoiced.SendPutRequestInvoicedAsync(_requestHeader!, idStatusFnd, "AutomationRun");
            await _installmentPlanNumberRefund.SendRefundRequestAsync(_requestHeader!, planCreateResponse.InstallmentPlanNumber,
                "PartialRefund", "ReduceFromLastInstallment");
            await _mongoHandler.SendMongoQueryWith2KeysAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", planCreateResponse.InstallmentPlanNumber, "Activity", "REF", "Status", "_id");
            Console.WriteLine("Done TestValidate_PCO_FUN_STL_MON_FTR_CRC_F01_ActivitiesAfter_ReduceFromLastInstallment");
        }
        catch (Exception exception)
        {
            await Task.Delay(10 * 1000);
            await _settled.SendPutRequestSettledAsync(_requestHeader!, batchIdColB, idStatusCol, false);
            await _settled.SendPutRequestSettledAsync(_requestHeader!, batchIdFndB, idStatusFnd, false);
            Assert.Fail("Failed -> TestValidate_PCO_FUN_STL_MON_FTR_CRC_F01_ActivitiesAfter_ReduceFromLastInstallment \n" + exception + "\n");
        }
    }

    [TestCase(Category = "SkuFunActivitiesAfter")]
    [Test(Description = "TestValidate_PCO_FUN_STL_MON_FTR_CRC_F01_ActivitiesAfter_FutureInstallmentsLast with generated merchant"),
     CancelAfter(360 * 1000)]
    public async Task TestValidate_PCO_FUN_STL_MON_FTR_CRC_F01_ActivitiesAfter_FutureInstallmentsLast()
    {
        string idStatusCol = null!;
        string batchIdColB = null!;
        string idStatusFnd = null!;
        string batchIdFndB = null!;
        
        try
        {
            Console.WriteLine(
                "\nStarting TestValidate_PCO_FUN_STL_MON_FTR_CRC_F01_ActivitiesAfter_FutureInstallmentsLast");
            _terminalApiKey = _envConfig.PCO_FUN_STL_MON_FTR;
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            var planCreateResponse = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!, _requestHeader!,
                "PendingCapture", 1, _terminalApiKey, createPlanDefaultValues);
            var response = await _processing.SendGetForProcessingAsync(_requestHeader!, planCreateResponse.InstallmentPlanNumber);
            Assert.That(response, Is.EqualTo(""));
            (_, idStatusCol) = await _mongoHandler.SendMongoQueryWith2KeysAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", planCreateResponse.InstallmentPlanNumber, "Activity", "COL", "Status", "_id");
            (_, idStatusFnd) = await _mongoHandler.SendMongoQueryWith2KeysAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", planCreateResponse.InstallmentPlanNumber, "Activity", "FND", "Status", "_id");
            await _created.SendPutRequestCreatedAsync(_requestHeader!, idStatusCol);
            await _created.SendPutRequestCreatedAsync(_requestHeader!, idStatusFnd);
            (batchIdColB, var idBatchColB) = await _mongoHandler.SendMongoQueryWith2KeysAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", planCreateResponse.InstallmentPlanNumber, "Activity", "COL", "BatchId", "_id");
            Assert.That(batchIdColB, Is.Not.Null);
            Assert.That(idBatchColB, Is.Not.Null);
            (batchIdFndB, var idBatchFndB) = await _mongoHandler.SendMongoQueryWith2KeysAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", planCreateResponse.InstallmentPlanNumber, "Activity", "FND", "BatchId", "_id");
            Assert.That(batchIdFndB, Is.Not.Null);
            Assert.That(idBatchFndB, Is.Not.Null);
            await _settled.SendPutRequestSettledAsync(_requestHeader!, batchIdColB, idStatusCol, true);
            await _settled.SendPutRequestSettledAsync(_requestHeader!, batchIdFndB, idStatusFnd, true);
            await _mongoHandler.SendMongoQueryWith3KeysReturn2StringColumnValuesAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", planCreateResponse.InstallmentPlanNumber, "Activity", "COL", 
                "Status","Acknowledged", "Status", "_id");
            await _mongoHandler.SendMongoQueryWith3KeysReturn2StringColumnValuesAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", planCreateResponse.InstallmentPlanNumber, "Activity", "FND", 
                "Status","Acknowledged", "Status", "_id");
            await _createPaymentInvoice.SendPostRequestCreatePaymentInvoiceAsync(_requestHeader!);
            await _invoiced.SendPutRequestInvoicedAsync(_requestHeader!, idStatusCol, "AutomationRun");
            await _invoiced.SendPutRequestInvoicedAsync(_requestHeader!, idStatusFnd, "AutomationRun");
            await _installmentPlanNumberRefund.SendRefundRequestAsync(_requestHeader!, planCreateResponse.InstallmentPlanNumber,
                "PartialRefund", "FutureInstallmentsLast");
            await _mongoHandler.SendMongoQueryWith2KeysAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", planCreateResponse.InstallmentPlanNumber, "Activity", "REF", "Status", "_id");
            Console.WriteLine("Done TestValidate_PCO_FUN_STL_MON_FTR_CRC_F01_ActivitiesAfter_FutureInstallmentsLast");
        }
        catch (Exception exception)
        {
            await Task.Delay(10 * 1000);
            await _settled.SendPutRequestSettledAsync(_requestHeader!, batchIdColB, idStatusCol, false);
            await _settled.SendPutRequestSettledAsync(_requestHeader!, batchIdFndB, idStatusFnd, false);
            Assert.Fail("Failed -> TestValidate_PCO_FUN_STL_MON_FTR_CRC_F01_ActivitiesAfter_FutureInstallmentsLast \n" +
                        exception + "\n");
        }
    }

    [TestCase(Category = "SkuFunActivitiesAfter")]
    [Test(Description =
         "TestValidate_PCO_FUN_STL_MON_FTR_CRC_F01_ActivitiesAfter_PartialRefund with generated merchant"),
     CancelAfter(360 * 1000)]
    public async Task TestValidate_PCO_FUN_STL_MON_FTR_CRC_F01_ActivitiesAfter_PartialRefund()
    {
        string idStatusCol = null!;
        string batchIdColB = null!;
        string idStatusFnd = null!;
        string batchIdFndB = null!;
        
        try
        {
            Console.WriteLine("\nStarting TestValidate_PCO_FUN_STL_MON_FTR_CRC_F01_ActivitiesAfter_PartialRefund");
            _terminalApiKey = _envConfig.PCO_FUN_STL_MON_FTR;
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            var planCreateResponse = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!, _requestHeader!,
                "PendingCapture",
                1, _terminalApiKey, createPlanDefaultValues);
            var response = await _processing.SendGetForProcessingAsync(_requestHeader!, planCreateResponse.InstallmentPlanNumber);
            Assert.That(response, Is.EqualTo(""));
            (_, idStatusCol) = await _mongoHandler.SendMongoQueryWith2KeysAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", planCreateResponse.InstallmentPlanNumber, "Activity", "COL", "Status", "_id");
            (_, idStatusFnd) = await _mongoHandler.SendMongoQueryWith2KeysAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", planCreateResponse.InstallmentPlanNumber, "Activity", "FND", "Status", "_id");
            await _created.SendPutRequestCreatedAsync(_requestHeader!, idStatusCol);
            await _created.SendPutRequestCreatedAsync(_requestHeader!, idStatusFnd);
            (batchIdColB, var idBatchColB) = await _mongoHandler.SendMongoQueryWith2KeysAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", planCreateResponse.InstallmentPlanNumber, "Activity", "COL", "BatchId", "_id");
            Assert.That(batchIdColB, Is.Not.Null);
            Assert.That(idBatchColB, Is.Not.Null);
            (batchIdFndB, var idBatchFndB) = await _mongoHandler.SendMongoQueryWith2KeysAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", planCreateResponse.InstallmentPlanNumber, "Activity", "FND", "BatchId", "_id");
            Assert.That(batchIdFndB, Is.Not.Null);
            Assert.That(idBatchFndB, Is.Not.Null);
            await _settled.SendPutRequestSettledAsync(_requestHeader!, batchIdColB, idStatusCol, true);
            await _settled.SendPutRequestSettledAsync(_requestHeader!, batchIdFndB, idStatusFnd, true);
            await _mongoHandler.SendMongoQueryWith3KeysReturn2StringColumnValuesAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", planCreateResponse.InstallmentPlanNumber, "Activity", "COL", 
                "Status","Acknowledged", "Status", "_id");
            await _mongoHandler.SendMongoQueryWith3KeysReturn2StringColumnValuesAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", planCreateResponse.InstallmentPlanNumber, "Activity", "FND", 
                "Status","Acknowledged", "Status", "_id");
            await _createPaymentInvoice.SendPostRequestCreatePaymentInvoiceAsync(_requestHeader!);
            await _invoiced.SendPutRequestInvoicedAsync(_requestHeader!, idStatusCol, "AutomationRun");
            await _invoiced.SendPutRequestInvoicedAsync(_requestHeader!, idStatusFnd, "AutomationRun");
            await _installmentPlanNumberRefund.SendRefundRequestAsync(_requestHeader!, planCreateResponse.InstallmentPlanNumber,
                "PartialRefund");
            await _mongoHandler.SendMongoQueryWith2KeysAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", planCreateResponse.InstallmentPlanNumber, "Activity", "REF", "Status", "_id");
            Console.WriteLine("Done TestValidate_PCO_FUN_STL_MON_FTR_CRC_F01_ActivitiesAfter_PartialRefund");
        }
        catch (Exception exception)
        {
            await Task.Delay(10 * 1000);
            await _settled.SendPutRequestSettledAsync(_requestHeader!, batchIdColB, idStatusCol, false);
            await _settled.SendPutRequestSettledAsync(_requestHeader!, batchIdFndB, idStatusFnd, false);
            Assert.Fail("Failed -> TestValidate_PCO_FUN_STL_MON_FTR_CRC_F01_ActivitiesAfter_PartialRefund \n" +
                        exception + "\n");
        }
    }

    [TestCase(Category = "SkuFunActivitiesAfter")]
    [Test(Description = "TestValidate_PCO_FUN_STL_MON_FTR_CRC_F06_ActivitiesAfter with generated merchant"),
     CancelAfter(360 * 1000)]
    public async Task TestValidate_PCO_FUN_STL_MON_FTR_CRC_F06_ActivitiesAfter()
    {
        string idStatusCol = null!;
        string batchIdColB = null!;
        string idStatusFnd = null!;
        string batchIdFndB = null!;
        
        try
        {
            Console.WriteLine("\nStarting TestValidate_PCO_FUN_STL_MON_FTR_CRC_F06_ActivitiesAfter");
            _terminalApiKey = _envConfig.PCO_FUN_STL_MON_FTR;
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            var planCreateResponse = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!, _requestHeader!,
                "PendingCapture",
                6, _terminalApiKey, createPlanDefaultValues);
            var response = await _processing.SendGetForProcessingAsync(_requestHeader!, planCreateResponse.InstallmentPlanNumber);
            Assert.That(response, Is.EqualTo(""));
            (_, idStatusCol) = await _mongoHandler.SendMongoQueryWith2KeysAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", planCreateResponse.InstallmentPlanNumber, "Activity", "COL", "Status", "_id");
            (_, idStatusFnd) = await _mongoHandler.SendMongoQueryWith2KeysAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", planCreateResponse.InstallmentPlanNumber, "Activity", "FND", "Status", "_id");
            await _created.SendPutRequestCreatedAsync(_requestHeader!, idStatusCol);
            await _created.SendPutRequestCreatedAsync(_requestHeader!, idStatusFnd);
            (batchIdColB, var idBatchColB) = await _mongoHandler.SendMongoQueryWith2KeysAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", planCreateResponse.InstallmentPlanNumber, "Activity", "COL", "BatchId", "_id");
            Assert.That(batchIdColB, Is.Not.Null);
            Assert.That(idBatchColB, Is.Not.Null);
            (batchIdFndB, var idBatchFndB) = await _mongoHandler.SendMongoQueryWith2KeysAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", planCreateResponse.InstallmentPlanNumber, "Activity", "FND", "BatchId", "_id");
            Assert.That(batchIdFndB, Is.Not.Null);
            Assert.That(idBatchFndB, Is.Not.Null);
            await _settled.SendPutRequestSettledAsync(_requestHeader!, batchIdColB, idStatusCol, true);
            await _settled.SendPutRequestSettledAsync(_requestHeader!, batchIdFndB, idStatusFnd, true);
            await _mongoHandler.SendMongoQueryWith3KeysReturn2StringColumnValuesAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", planCreateResponse.InstallmentPlanNumber, "Activity", "COL", 
                "Status","Acknowledged", "Status", "_id");
            await _mongoHandler.SendMongoQueryWith3KeysReturn2StringColumnValuesAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", planCreateResponse.InstallmentPlanNumber, "Activity", "FND", 
                "Status","Acknowledged", "Status", "_id");
            await _createPaymentInvoice.SendPostRequestCreatePaymentInvoiceAsync(_requestHeader!);
            await _invoiced.SendPutRequestInvoicedAsync(_requestHeader!, idStatusCol, "AutomationRun");
            await _invoiced.SendPutRequestInvoicedAsync(_requestHeader!, idStatusFnd, "AutomationRun");
            await _installmentPlanNumberRefund.SendRefundRequestAsync(_requestHeader!, planCreateResponse.InstallmentPlanNumber,
                "FullRefund");
            await _mongoHandler.SendMongoQueryWith2KeysAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", planCreateResponse.InstallmentPlanNumber, "Activity", "REF", "Status", "_id");
            await _mongoHandler.SendMongoQueryWith2KeysAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", planCreateResponse.InstallmentPlanNumber, "Activity", "RED", "Status", "_id");
            Console.WriteLine("Done TestValidate_PCO_FUN_STL_MON_FTR_CRC_F06_ActivitiesAfter");
        }
        catch (Exception exception)
        {
            await Task.Delay(10 * 1000);
            await _settled.SendPutRequestSettledAsync(_requestHeader!, batchIdColB, idStatusCol, false);
            await _settled.SendPutRequestSettledAsync(_requestHeader!, batchIdFndB, idStatusFnd, false);
            Assert.Fail("Failed -> TestValidate_PCO_FUN_STL_MON_FTR_CRC_F06_ActivitiesAfter \n" + exception + "\n");
        }
    }

    [TestCase(Category = "SkuFunActivitiesAfter")]
    [Test(Description = "TestValidate_PCO_FUN_STL_MON_FTR_CRC_F12_ActivitiesAfter_Outstanding_Balance_First with generated merchant"),
     CancelAfter(360 * 1000)]
    public async Task TestValidate_PCO_FUN_STL_MON_FTR_CRC_F12_ActivitiesAfter_Outstanding_Balance_First()
    {
        try
        {
            Console.WriteLine("\nStarting TestValidate_PCO_FUN_STL_MON_FTR_CRC_F12_ActivitiesAfter_Outstanding_Balance_First");
            var terminalApiKey = _envConfig.PCO_FUN_STL_MON_FTR_CRC_F12;
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            var planCreateResponse = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!, _requestHeader!,
                "PendingCapture",
                12, terminalApiKey, createPlanDefaultValues);
            var response = await _processing.SendGetForProcessingAsync(_requestHeader!, planCreateResponse.InstallmentPlanNumber);
            Assert.That(response, Is.EqualTo(""));
            var (_, idStatusFndPending) = await _mongoHandler.SendMongoQueryWith3KeysReturn2StringColumnValuesAsync(_db!, "Payment_Records", 
                "InstallmentPlanNumber", planCreateResponse.InstallmentPlanNumber, "Activity", "FND", "Status", "PendingBatch","Status", "_id");
            var responseCreate = await _createPaymentInvoice.SendPostRequestCreatePaymentInvoiceAsync(_requestHeader!);
            Assert.That(responseCreate, Is.Not.Null);
            var invoicedResponse =
                await _invoiced.SendPutRequestInvoicedAsync(_requestHeader!, idStatusFndPending, "AutomationRun");
            Assert.That(invoicedResponse, Is.Not.Null);
            await _mongoHandler.SendMongoQueryWith3KeysReturn2StringColumnValuesAsync(
                _db!, "Payment_Records", "InstallmentPlanNumber", planCreateResponse.InstallmentPlanNumber, 
                "Activity", "FND", "Status","Invoiced","Status", "_id");
            await _installmentPlanNumberRefund.SendRefundRequestAsync(_requestHeader!, planCreateResponse.InstallmentPlanNumber,
                "PartialRefund");
            await _mongoHandler.SendMongoQueryWith2KeysAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", planCreateResponse.InstallmentPlanNumber, "Activity", "RED", "Status", "_id");
            Console.WriteLine(
                "Done TestValidate_PCO_FUN_STL_MON_FTR_CRC_F12_ActivitiesAfter_Outstanding_Balance_First");
        }
        catch (Exception exception)
        {
            Assert.Fail("Failed -> TestValidate_PCO_FUN_STL_MON_FTR_CRC_F12_ActivitiesAfter_Outstanding_Balance_First \n" + exception + "\n");
        }
    }

    [TestCase(Category = "SkuFunActivitiesAfter")]
    [Test(Description =
         "TestValidate_PCO_FUN_STL_MON_FTR_CRC_F12_ActivitiesAfter_Last_Installment with generated merchant"),
     CancelAfter(360 * 1000)]
    public async Task TestValidate_PCO_FUN_STL_MON_FTR_CRC_F12_ActivitiesAfter_Last_Installment()
    {
        try
        {
            Console.WriteLine("\nStarting TestValidate_PCO_FUN_STL_MON_FTR_CRC_F12_ActivitiesAfter_Last_Installment");
            var terminalApiKey = _envConfig.PCO_FUN_STL_MON_FTR_CRC_F12;
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            var planCreateResponse = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!, _requestHeader!,
                "PendingCapture", 12, terminalApiKey, createPlanDefaultValues);
            var response = await _processing.SendGetForProcessingAsync(_requestHeader!, planCreateResponse.InstallmentPlanNumber);
            Assert.That(response, Is.EqualTo(""));
            var (_, idStatusFndPending) = await _mongoHandler.SendMongoQueryWith3KeysReturn2StringColumnValuesAsync(_db!, "Payment_Records", 
                "InstallmentPlanNumber", planCreateResponse.InstallmentPlanNumber, "Activity", "FND", "Status", "PendingBatch","Status", "_id");
            var responseCreate = await _createPaymentInvoice.SendPostRequestCreatePaymentInvoiceAsync(_requestHeader!);
            Assert.That(responseCreate, Is.Not.Null);
            var invoicedResponse =
                await _invoiced.SendPutRequestInvoicedAsync(_requestHeader!, idStatusFndPending, "AutomationRun");
            Assert.That(invoicedResponse, Is.Not.Null);
            await _mongoHandler.SendMongoQueryWith3KeysReturn2StringColumnValuesAsync(
                _db!, "Payment_Records", "InstallmentPlanNumber", planCreateResponse.InstallmentPlanNumber, 
                "Activity", "FND", "Status","Invoiced","Status", "_id");
            await _installmentPlanNumberRefund.SendRefundRequestAsync(_requestHeader!, planCreateResponse.InstallmentPlanNumber,
                "PartialRefund", "ReduceFromLastInstallment");
            await _mongoHandler.SendMongoQueryWith2KeysAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", planCreateResponse.InstallmentPlanNumber, "Activity", "RED", "Status", "_id");
            Console.WriteLine("Done TestValidate_PCO_FUN_STL_MON_FTR_CRC_F12_ActivitiesAfter_Last_Installment");
        }
        catch (Exception exception)
        {
            Assert.Fail("Failed -> TestValidate_PCO_FUN_STL_MON_FTR_CRC_F12_ActivitiesAfter_Last_Installment \n" +
                        exception + "\n");
        }
    }

    [TestCase(Category = "SkuFunActivitiesAfter")]
    [Test(Description =
         "TestValidate_PCO_FUN_STL_MON_FTR_CRC_F09_ActivitiesAfter_Partial_Refund with generated merchant"),
     CancelAfter(360 * 1000)]
    public async Task TestValidate_PCO_FUN_STL_MON_FTR_CRC_F09_ActivitiesAfter_Partial_Refund()
    {
        string idStatusCol = null!;
        string batchIdColB = null!;
        string idStatusFnd = null!;
        string batchIdFndB = null!;
        
        try
        {
            Console.WriteLine("\nStarting TestValidate_PCO_FUN_STL_MON_FTR_CRC_F09_ActivitiesAfter_Partial_Refund");
            _terminalApiKey = _envConfig.PCO_FUN_STL_MON_FTR;
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            var planCreateResponse = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!, _requestHeader!,
                "PendingCapture", 1, _terminalApiKey, createPlanDefaultValues);
            var response = await _processing.SendGetForProcessingAsync(_requestHeader!, planCreateResponse.InstallmentPlanNumber);
            Assert.That(response, Is.EqualTo(""));
            (_, idStatusCol) = await _mongoHandler.SendMongoQueryWith2KeysAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", planCreateResponse.InstallmentPlanNumber, "Activity", "COL", "Status", "_id");
            (var _, idStatusFnd) = await _mongoHandler.SendMongoQueryWith2KeysAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", planCreateResponse.InstallmentPlanNumber, "Activity", "FND", "Status", "_id");
            await _created.SendPutRequestCreatedAsync(_requestHeader!, idStatusCol);
            await _created.SendPutRequestCreatedAsync(_requestHeader!, idStatusFnd);
            (batchIdColB, var idBatchColB) = await _mongoHandler.SendMongoQueryWith2KeysAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", planCreateResponse.InstallmentPlanNumber, "Activity", "COL", "BatchId", "_id");
            Assert.That(batchIdColB, Is.Not.Null);
            Assert.That(idBatchColB, Is.Not.Null);
            (batchIdFndB, var idBatchFndB) = await _mongoHandler.SendMongoQueryWith2KeysAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", planCreateResponse.InstallmentPlanNumber, "Activity", "FND", "BatchId", "_id");
            Assert.That(batchIdFndB, Is.Not.Null);
            Assert.That(idBatchFndB, Is.Not.Null);
            await _settled.SendPutRequestSettledAsync(_requestHeader!, batchIdColB, idStatusCol, true);
            await _settled.SendPutRequestSettledAsync(_requestHeader!, batchIdFndB, idStatusFnd, true);
            await _mongoHandler.SendMongoQueryWith3KeysReturn2StringColumnValuesAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", planCreateResponse.InstallmentPlanNumber, "Activity", "COL", 
                "Status","Acknowledged", "Status", "_id");
            await _mongoHandler.SendMongoQueryWith3KeysReturn2StringColumnValuesAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", planCreateResponse.InstallmentPlanNumber, "Activity", "FND", 
                "Status","Acknowledged", "Status", "_id");
            await _createPaymentInvoice.SendPostRequestCreatePaymentInvoiceAsync(_requestHeader!);
            await _invoiced.SendPutRequestInvoicedAsync(_requestHeader!, idStatusCol, "AutomationRun");
            await _invoiced.SendPutRequestInvoicedAsync(_requestHeader!, idStatusFnd, "AutomationRun");
            await _installmentPlanNumberRefund.SendRefundRequestAsync(_requestHeader!, planCreateResponse.InstallmentPlanNumber,
                "PartialRefund");
            await _mongoHandler.SendMongoQueryWith2KeysAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber", planCreateResponse.InstallmentPlanNumber, "Activity", "REF", "Status", "_id");
            Console.WriteLine("Done TestValidate_PCO_FUN_STL_MON_FTR_CRC_F09_ActivitiesAfter_Partial_Refund");
        }
        catch (Exception exception)
        {
            await Task.Delay(10 * 1000);
            await _settled.SendPutRequestSettledAsync(_requestHeader!, batchIdColB, idStatusCol, false);
            await _settled.SendPutRequestSettledAsync(_requestHeader!, batchIdFndB, idStatusFnd, false);
            Assert.Fail("Failed -> TestValidate_PCO_FUN_STL_MON_FTR_CRC_F09_ActivitiesAfter_Partial_Refund \n" +
                        exception + "\n");
        }
    }
}