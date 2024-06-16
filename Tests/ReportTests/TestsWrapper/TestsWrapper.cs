using MongoDB.Driver;
using NUnit.Framework;
using Splitit.Automation.NG.Backend.BaseActions;
using Splitit.Automation.NG.Backend.Services.AdminApi.BusinessUnit.Functionality;
using Splitit.Automation.NG.Backend.Services.ReportSystem.Reports.ReportsApiEndPoint;
using Splitit.Automation.NG.Backend.Services.V3.DefaultValues;
using Splitit.Automation.NG.Backend.Services.V3.InstallmentPlan.InstallmentPlanApiEndPoint;
using Splitit.Automation.NG.Backend.Services.V3.InstallmentPlan.InstallmentPlanResponses;
using Splitit.Automation.NG.PaymentsSystem.PaymentProcessing.EndPointFunctionality;
using Splitit.Automation.NG.PaymentsSystem.PaymentsOperations.EndPointFunctionality;
using Splitit.Automation.NG.PaymentsSystem.Sku.SkuMatching.Tests.SkuTestsData;

namespace Splitit.Automation.NG.Backend.Tests.ReportTests.TestsWrapper;

public class TestsWrapper
{
    private readonly InstallmentPlans _installmentPlans = new();
    private readonly MongoHandler _mongoHandler = new();
    private readonly Processing _processing = new();
    private readonly CreatePaymentInvoice _createPaymentInvoice = new();
    private readonly Invoiced _invoiced = new();
    private readonly BashFileController _bashFileController = new();
    private readonly TestsHelper.TestsHelper _testsHelper = new();
    private readonly GetListFunctionality _getListFunctionality = new();
    private readonly GenerateFile _generateFile = new();
    private readonly S3Controller _s3Controller = new();
    private readonly InstallmentPlanNumberRefund _installmentPlanNumberRefund = new();

    public async Task<ResponseV3.ResponseRoot> InitPlanAndQueryMongo(string testName, string terminalApiKey,
        RequestHeader requestHeader,
        IMongoDatabase db, Dictionary<string, SkuTestsData.FeesSettings> feesDict, int numOfInstallments)
    {
        try
        {
            Console.WriteLine("Starting InitPlanAndQueryMongo");
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            var planCreateResponse = await _installmentPlans.CreatePlanAsync(
                Environment.GetEnvironmentVariable("ApiV3")!, requestHeader, "PendingCapture",
                numOfInstallments, terminalApiKey, createPlanDefaultValues);
            var (statusCol, idCol) = await _mongoHandler.SendMongoQueryWith3KeysReturn2StringColumnValuesAsync(db, "Payment_Records",
                "InstallmentPlanNumber", planCreateResponse.InstallmentPlanNumber, 
                "Activity", "COL", 
                "Status", "Invoiced",
                "Status", "_id");
            if (!statusCol.Equals("New") && !statusCol.Equals("PendingBatch"))
            {
                Assert.Fail("COL Is not in the correct status");
            }

            Assert.That(idCol, Is.Not.Null);
            var response = await _processing.SendGetForProcessingAsync(requestHeader, planCreateResponse.InstallmentPlanNumber);
            Assert.That(response, Is.EqualTo(""));
            var invoiceId = GuidGenerator.GenerateNewGuid();
            var jResponse =
                await _createPaymentInvoice.SendPostRequestCreatePaymentInvoiceAsync(requestHeader, invoiceId);
            Assert.That(jResponse, Is.Not.Null);
            var jResponseInvoiced =
                await _invoiced.SendPutRequestInvoicedAsync(requestHeader, idCol, invoiceId,
                    feesDict[testName].VariableFee, feesDict[testName].FixedFee);
            Assert.That(jResponseInvoiced, Is.Not.Null);
            var (activityStatusCol, idStatusCol) = await _mongoHandler.SendMongoQueryWith2KeysAsync(db,
                "Payment_Records",
                "InstallmentPlanNumber",
                planCreateResponse.InstallmentPlanNumber, "Activity", "COL", "Status", "_id");
            Assert.That(activityStatusCol, Is.EqualTo("Invoiced"));
            Assert.That(idStatusCol, Is.Not.Null);
            Console.WriteLine("Done with InitPlanAndQueryMongo");
            return planCreateResponse;
        }
        catch (Exception e)
        {
            Console.WriteLine("Error in InitPlanAndQueryMongo" + e);
            throw;
        }
    }

    public async Task GenerateReport()
    {
        try
        {
            Console.WriteLine("Starting GenerateReport");
            await _bashFileController.RunBashFile();
            await Task.Delay(10 * 1000);
            Console.WriteLine("Done with GenerateReport");
        }
        catch (Exception e)
        {
            Console.WriteLine("Error in GenerateReport" + e);
            throw;
        }
    }

    public async Task<string> DownloadReport(RequestHeader requestHeader, string merchantId, string reportType)
    {
        var columnsList = await _testsHelper.ReturnSettlementReportColumns(reportType);
        var jResponseBu =
            await _getListFunctionality.SendGetRequestGetListAsync(requestHeader, merchantId);
        var jResponseGenerateReport = await _generateFile.SendPostRequestForGenerateFileAsync(
            requestHeader, Environment.GetEnvironmentVariable("SettlementReport")!, DateTime.Now.AddDays(-1),
            DateTime.Now.AddDays(1), 0, int.Parse(merchantId), columnsList,
            jResponseBu.BusinessUnits[0].Id);
        Assert.That(jResponseGenerateReport.IsSuccess);
        var filePathDownload = await _s3Controller.DownloadFileFromS3(jResponseGenerateReport.FileUrl,
            "ReportToTest", ".csv");
        return filePathDownload;
    }

    public async Task<bool> ValidateReport(string testName, int expectedListCount,
        List<TestsHelper.TestsHelper.SettlementReportValues> settlementReportValuesList,
        Dictionary<string, string> splititActivityMerchantFacingList, ResponseV3.ResponseRoot planCreateResponse,
        Dictionary<string, Dictionary<string, string>> signDict, List<string> activitiesList)
    {
        try
        {
            Console.WriteLine("Starting ValidateReport");
            Assert.That(settlementReportValuesList.Count, Is.EqualTo(expectedListCount));
            Assert.That(await _testsHelper.ValidateActivityValues(settlementReportValuesList,
                splititActivityMerchantFacingList, activitiesList), Is.True);
            Assert.That(await _testsHelper.ValidateAmount(planCreateResponse.Amount, settlementReportValuesList), Is.True);
            Assert.That(await _testsHelper.ValidateGrossSettlementAmount(settlementReportValuesList,
                splititActivityMerchantFacingList, signDict[testName]), Is.True);
            Assert.That(await _testsHelper.ValidateFeesCalculationAmounts(settlementReportValuesList), Is.True);
            Console.WriteLine("Done with ValidateReport");
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine("Error in ValidateReport" + e);
            throw;
        }
    }

    public async Task<(ResponseV3.ResponseRoot planCreateResponse, string invoiceId)> InitPlanAndQueryMongoFnd(
        string testName, string terminalApiKey, RequestHeader requestHeader, IMongoDatabase db, 
        Dictionary<string, SkuTestsData.FeesSettings> feesDict, int numOfInstallments)
    {
        try
        {
            Console.WriteLine("Starting InitPlanAndQueryMongoFnd");
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            var planCreateResponse = await _installmentPlans.CreatePlanAsync(
                Environment.GetEnvironmentVariable("ApiV3")!, requestHeader, "PendingCapture",
                numOfInstallments, terminalApiKey, createPlanDefaultValues);
            var (statusCol, idCol) = await _mongoHandler.SendMongoQueryWith2KeysAsync(db, "Payment_Records",
                "InstallmentPlanNumber",
                planCreateResponse.InstallmentPlanNumber, "Activity", "COL", "Status", "_id");
            if (!statusCol.Equals("New") && !statusCol.Equals("PendingBatch"))
            {
                Assert.Fail("COL Is not in the correct status");
            }

            Assert.That(idCol, Is.Not.Null);
            var (statusFnd, idFnd) = await _mongoHandler.SendMongoQueryWith2KeysAsync(db, "Payment_Records",
                "InstallmentPlanNumber",
                planCreateResponse.InstallmentPlanNumber, "Activity", "FND", "Status", "_id");
            if (!statusFnd.Equals("New") && !statusFnd.Equals("PendingBatch"))
            {
                Assert.Fail("FND Is not in the correct status");
            }

            Assert.That(idFnd, Is.Not.Null);
            var response = await _processing.SendGetForProcessingAsync(requestHeader, planCreateResponse.InstallmentPlanNumber);
            Assert.That(response, Is.EqualTo(""));
            var invoiceId = GuidGenerator.GenerateNewGuid();
            var jResponse =
                await _createPaymentInvoice.SendPostRequestCreatePaymentInvoiceAsync(requestHeader,
                    invoiceId);
            Assert.That(jResponse, Is.Not.Null);
            var jResponseInvoicedCol =
                await _invoiced.SendPutRequestInvoicedAsync(requestHeader, idCol, invoiceId,
                    feesDict[testName].VariableFee, feesDict[testName].FixedFee);
            Assert.That(jResponseInvoicedCol, Is.Not.Null);
            var jResponseInvoicedFnd =
                await _invoiced.SendPutRequestInvoicedAsync(requestHeader, idFnd, invoiceId,
                    feesDict[testName].VariableFee, feesDict[testName].FixedFee);
            Assert.That(jResponseInvoicedFnd, Is.Not.Null);
            var (activityStatusCol, idStatusCol) = await _mongoHandler.SendMongoQueryWith2KeysAsync(db,
                "Payment_Records",
                "InstallmentPlanNumber",
                planCreateResponse.InstallmentPlanNumber, "Activity", "COL", "Status", "_id");
            Assert.That(activityStatusCol, Is.EqualTo("Invoiced"));
            Assert.That(idStatusCol, Is.Not.Null);
            var (activityStatusFnd, idStatusFnd) = await _mongoHandler.SendMongoQueryWith2KeysAsync(db,
                "Payment_Records",
                "InstallmentPlanNumber",
                planCreateResponse.InstallmentPlanNumber, "Activity", "FND", "Status", "_id");
            Assert.That(activityStatusFnd, Is.EqualTo("Invoiced"));
            Assert.That(idStatusFnd, Is.Not.Null);
            Console.WriteLine("Done with InitPlanAndQueryMongoFnd");
            return (planCreateResponse, invoiceId);
        }
        catch (Exception e)
        {
            Console.WriteLine("Error in InitPlanAndQueryMongoFnd" + e);
            throw;
        }
    }

    public async Task DoRefundAndQueryIt(ResponseV3.ResponseRoot planCreateResponse, RequestHeader requestHeader,
        IMongoDatabase db, string testName, string invoiceId, Dictionary<string, SkuTestsData.FeesSettings> feesDict)
    {
        try
        {
            Console.WriteLine("Starting DoRefundAndQueryIt");
            await _installmentPlanNumberRefund.SendRefundRequestAsync(requestHeader,
                planCreateResponse.InstallmentPlanNumber,
                "PartialRefund", "ReduceFromLastInstallment");
            var (statusRed, idRed) = await _mongoHandler.SendMongoQueryWith2KeysAsync(db, "Payment_Records",
                "InstallmentPlanNumber",
                planCreateResponse.InstallmentPlanNumber, "Activity", "RED", "Status", "_id");
            if (!statusRed.Equals("New") && !statusRed.Equals("PendingBatch"))
            {
                Assert.Fail("RED Is not in the correct status");
            }
            var responseAfter = await _processing.SendGetForProcessingAsync(requestHeader, planCreateResponse.InstallmentPlanNumber);
            Assert.That(responseAfter, Is.EqualTo(""));
            var jResponseInvoice =
                await _createPaymentInvoice.SendPostRequestCreatePaymentInvoiceAsync(requestHeader,
                    invoiceId);
            Assert.That(jResponseInvoice, Is.Not.Null);
            var jResponseInvoicedRed =
                await _invoiced.SendPutRequestInvoicedAsync(requestHeader, idRed, invoiceId, 
                    feesDict[testName].VariableFee, feesDict[testName].FixedFee);
            Assert.That(jResponseInvoicedRed, Is.Not.Null);
            var (statusRedAfter, idRedAfter) = await _mongoHandler.SendMongoQueryWith2KeysAsync(db, "Payment_Records",
                "InstallmentPlanNumber",
                planCreateResponse.InstallmentPlanNumber, "Activity", "RED", "Status", "_id");
            Assert.That(statusRedAfter, Is.EqualTo("Invoiced"));
            Assert.That(idRedAfter, Is.Not.Null);
            Console.WriteLine("Done with DoRefundAndQueryIt");
        }
        catch (Exception e)
        {
            Console.WriteLine("Error in DoRefundAndQueryIt" + e);
            throw;
        }
    }

    public async Task<bool> ValidateReportFnd(string testName, int expectedListCount,
        List<TestsHelper.TestsHelper.SettlementReportValues> settlementReportValuesList,
        Dictionary<string, string> splititActivityMerchantFacingList, ResponseV3.ResponseRoot planCreateResponse,
        Dictionary<string, Dictionary<string, string>> signDict, List<string> activitiesList)
    {
        try
        {
            Console.WriteLine("Starting ValidateReportFnd");
            Assert.That(settlementReportValuesList.Count, Is.EqualTo(expectedListCount));
            Assert.That(await _testsHelper.ValidateActivityValues(settlementReportValuesList,
                splititActivityMerchantFacingList, activitiesList), Is.True);
            Assert.That(await _testsHelper.ValidateAmount(planCreateResponse.Amount, settlementReportValuesList), Is.True);
            Assert.That(await _testsHelper.ValidateGrossSettlementAmount(settlementReportValuesList, 
                splititActivityMerchantFacingList, signDict[testName]), Is.True);
            Assert.That(await _testsHelper.ValidateRefundAmountInReport(settlementReportValuesList, 1,
                splititActivityMerchantFacingList, signDict[testName]), Is.True);
            Assert.That(await _testsHelper.ValidateFeesCalculationAmounts(settlementReportValuesList), Is.True);
            Console.WriteLine("Done with ValidateReportFnd");
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine("Error in ValidateReportFnd" + e);
            throw;
        }
    }
}