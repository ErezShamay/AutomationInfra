using MongoDB.Driver;
using NUnit.Framework;
using Splitit.Automation.NG.Backend.BaseActions;
using Splitit.Automation.NG.Backend.Services.AdminApi.Disputes.DisputesFunctionality;
using Splitit.Automation.NG.Backend.Services.Notifications.Bluesnap.BluesnapNotificationsFunctionality;
using Splitit.Automation.NG.Backend.Services.V1.InstallmentPlan.InstallmentPlanApiEndPoints;
using Splitit.Automation.NG.Backend.Services.V3.DefaultValues;
using Splitit.Automation.NG.Backend.Services.V3.InstallmentPlan.InstallmentPlanApiEndPoint;
using Splitit.Automation.NG.PaymentsSystem.PaymentProcessing.EndPointFunctionality;
using Splitit.Automation.NG.PaymentsSystem.PaymentsOperations.EndPointFunctionality;
using Splitit.Automation.NG.PaymentsSystem.Sku.SkuMatching.Tests.SkuTestsData;
using Splitit.Automation.NG.Utilities.EnvironmentsConfig;
using Create = Splitit.Automation.NG.Backend.Services.AdminApi.Merchant.MerchantFunctionality.Create;

namespace Splitit.Automation.NG.PaymentsSystem.Sku.FundingOperations.TestsHelper;

public class TestsHelper
{
    private readonly InstallmentPlans _installmentPlans;
    private readonly MongoHandler _mongoHandler;
    private readonly InstallmentPlanNumberRefund _installmentPlanNumberRefund;
    private readonly Processing _processing;
    private readonly Create _createMerchant;
    private readonly Created _created;
    private readonly Settled _settled;
    private readonly CreatePaymentInvoice _createPaymentInvoice;
    private readonly Invoiced _invoiced;
    private readonly GetPgtl _getPgtl;
    private readonly SetDisputeLiabilityId _setDisputeLiabilityId;
    private readonly DisputeCharges _disputeCharges;
    private readonly BluesnapFunctionality _bluesnapFunctionality;
    private int _businessUnitId;
    private string? _terminalApiKey;
    private readonly EnvConfig _envConfig;

    public TestsHelper()
    {
        Console.WriteLine("Starting Tests Helper");
        _installmentPlans = new InstallmentPlans();
        _mongoHandler = new MongoHandler();
        _installmentPlanNumberRefund = new InstallmentPlanNumberRefund();
        _processing = new Processing();
        _createMerchant = new Create();
        _created = new Created();
        _settled = new Settled();
        _createPaymentInvoice = new CreatePaymentInvoice();
        _invoiced = new Invoiced();
        _getPgtl = new GetPgtl();
        _setDisputeLiabilityId = new SetDisputeLiabilityId();
        _disputeCharges = new DisputeCharges();
        _bluesnapFunctionality = new BluesnapFunctionality();
        _envConfig = new EnvConfig();
        Console.WriteLine("Starting Tests Helper");
    }

    public async Task<(string, int)> CreateMerchantFunctionality(RequestHeader requestHeader, string testName, IMongoDatabase db,
        string paymentForm, string strategy, string scheduledInterval, Dictionary<string, SkuTestsData.PaymentSettings> skuDict, string gatewayId)
    {
        try
        {
            (_terminalApiKey, _businessUnitId, _, _, _) = await _createMerchant.CreateMerchantAndInfoAsync( requestHeader, "USA", true,
                75, 1, 36, paymentForm, 
                true, strategy, true, 
                true, true, 
                20000, 24, 
                20000, 24,
                scheduledInterval, skuDict[testName.Substring(0, 40)], gatewayId);
            return (_terminalApiKey, _businessUnitId);
        }
        catch (Exception exception)
        {
            Console.WriteLine("Error in CreateMerchantFunctionality -> " + exception);
            throw;
        }
    }
    public async Task<(bool, string)> CreatePlanAndQueryMongo(string terminalApiKey, int businessUnitId, IMongoDatabase db,
        RequestHeader requestHeader, CreatePlanDefaultValues createPlanDefaultValues, int fromInstallment, 
        int toInstallment, string refundType, string refundStrategy)
    {
        try
        {
            Console.WriteLine("Starting CreatePlan");
            var json1 = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!,  requestHeader,
                "PendingCapture", new Random().Next(fromInstallment, toInstallment), terminalApiKey,
                createPlanDefaultValues);
            Console.WriteLine("Done CreatePlan\n");
            Console.WriteLine("Starting query mongo");
            var (activityStatusCol1Before, idStatusCol1Before) = await _mongoHandler.SendMongoQueryWith2KeysAsync(db,
                "Payment_Records",
                "InstallmentPlanNumber",
                json1.InstallmentPlanNumber, "Activity", "COL", "Status", "_id");
            if (activityStatusCol1Before.Equals("New") || activityStatusCol1Before.Equals("PendingBatch"))
            {
                Console.WriteLine("activityStatusCol is in the right state");
            }
            else
            {
                Assert.Fail("activityStatusCol is not in the right state");
            }
            Assert.That(idStatusCol1Before, Is.Not.Null);
            var (activityStatusFnd, idStatusFnd) = await _mongoHandler.SendMongoQueryWith2KeysAsync(db, "Payment_Records",
                "InstallmentPlanNumber",
                json1.InstallmentPlanNumber, "Activity", "FND", "Status", "_id");
            if (activityStatusFnd.Equals("New") || activityStatusFnd.Equals("PendingBatch"))
            {
                Console.WriteLine("activityStatusFnd is in the right state");
            }
            else
            {
                Assert.Fail("activityStatusFnd is not in the right state");
            }
            Assert.That(idStatusFnd, Is.Not.Null);
            Console.WriteLine("Done querying mongo");
            Console.WriteLine("Starting refund");
            var jResponseRefund1 = await _installmentPlanNumberRefund.SendRefundRequestAsync( requestHeader,
                json1.InstallmentPlanNumber,
                refundType, refundStrategy);
            Assert.That(jResponseRefund1.RefundId, Is.Not.Null);
            Console.WriteLine("Done refund\n");
            Console.WriteLine("Continuing querying mongo");
            var (activityStatusRed1Before, idStatusRed1Before) = await _mongoHandler.SendMongoQueryWith2KeysAsync(db,
                "Payment_Records",
                "InstallmentPlanNumber",
                json1.InstallmentPlanNumber, "Activity", "RED", "Status", "_id");
            Assert.That(activityStatusRed1Before, Is.EqualTo("New"));
            Assert.That(idStatusRed1Before, Is.Not.Null);
            var responseProcessing = await _processing.SendGetForProcessingAsync(requestHeader, json1.InstallmentPlanNumber);
            Assert.That(responseProcessing, Is.Not.Null);
            var responseProcessing1 = await _processing.SendGetForProcessingAsync( requestHeader, json1.InstallmentPlanNumber);
            Assert.That(responseProcessing1, Is.Not.Null);
            var (activityStatusDelAfter, idStatusDelAfter) = await _mongoHandler.SendMongoQueryWith2KeysAsync(db,
                "Payment_Records",
                "InstallmentPlanNumber",
                json1.InstallmentPlanNumber, "Activity", "DEL", "Status", "_id");
            if (!activityStatusDelAfter.Equals("Skipped"))
            {
                var responseProcessing2 = await _processing.SendGetForProcessingAsync( requestHeader, json1.InstallmentPlanNumber);
                Assert.That(responseProcessing2, Is.Not.Null);
            }
            Assert.That(idStatusDelAfter, Is.Not.Null);
            var docsListCol = await _mongoHandler.SendMongoQueryWith2KeysAndReturnListOfResultAsync(db, "Payment_Records",
                "InstallmentPlanNumber",
                json1.InstallmentPlanNumber, "Activity", "COL", "Sku", "_id");
            Assert.That(_mongoHandler.ValidateValueInDocsList(docsListCol!, "Status", "PendingBatch"));
            var docsListFnd = await _mongoHandler.SendMongoQueryWith2KeysAndReturnListOfResultAsync(db, "Payment_Records",
                "InstallmentPlanNumber",
                json1.InstallmentPlanNumber, "Activity", "FND", "Sku", "_id");
            Assert.That(_mongoHandler.ValidateValueInDocsList(docsListFnd!, "Status", "PendingBatch"));
            var (activityStatusRed1After, idStatusRed1After) = await _mongoHandler.SendMongoQueryWith2KeysAsync(db,
                "Payment_Records",
                "InstallmentPlanNumber",
                json1.InstallmentPlanNumber, "Activity", "RED", "Status", "_id");
            Assert.That(activityStatusRed1After, Is.EqualTo("New"));
            Assert.That(idStatusRed1After, Is.Not.Null);
            Console.WriteLine("Done querying mongo\n");
            Console.WriteLine("Done with CreatePlanAndQueryMongo and Succeeded");
            Console.WriteLine("Done creating a plan - ipn is -> " + json1.InstallmentPlanNumber);
            return (true, json1.InstallmentPlanNumber);
        }
        catch (Exception exception)
        {
            Console.WriteLine("Error in CreatePlanAndQueryMongo -> " + exception + "\n");
            throw;
        }
    }

    public async Task<(bool, string)> CreatePlanAndQueryMongoWithFailed(string terminalApiKey, int businessUnitId, IMongoDatabase db,
        RequestHeader requestHeader,
        CreatePlanDefaultValues createPlanDefaultValues, int fromInstallment, int toInstallment, string refundType,
        string refundStrategy)
    {
        try
        {
            Console.WriteLine("Starting CreatePlanAndQueryMongoWithFailed\n");
            Console.WriteLine("Staring Create Plan");
            var json1 = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!,  requestHeader,
                "PendingCapture", new Random().Next(fromInstallment, toInstallment), terminalApiKey,
                createPlanDefaultValues);
            Console.WriteLine("Done with Create Plan\n");
            Console.WriteLine("Starting query mongo");
            var (activityStatusCol1Before, idStatusCol1Before) = await _mongoHandler.SendMongoQueryWith2KeysAsync(db,
                "Payment_Records",
                "InstallmentPlanNumber",
                json1.InstallmentPlanNumber, "Activity", "COL", "Status", "_id");
            if (activityStatusCol1Before.Equals("New") || activityStatusCol1Before.Equals("PendingBatch") || activityStatusCol1Before.Equals("ProcessingFailed"))
            {
                Console.WriteLine("activityStatusCol is in the right state");
            }
            else
            {
                Assert.Fail("activityStatusCol is not in the right state");
            }
            Assert.That(idStatusCol1Before, Is.Not.Null);
            Console.WriteLine("Done querying mongo\n");
            Console.WriteLine("Starting refund");
            var jResponseRefund1 = await _installmentPlanNumberRefund.SendRefundRequestAsync( requestHeader,
                json1.InstallmentPlanNumber,
                refundType, refundStrategy);
            Assert.That(jResponseRefund1.RefundId, Is.Not.Null);
            Console.WriteLine("Done refund\n");
            Console.WriteLine("Continuing querying mongo");
            var (activityStatusRef1Before, idStatusRef1Before) = await _mongoHandler.SendMongoQueryWith2KeysAsync(db,
                "Payment_Records",
                "InstallmentPlanNumber",
                json1.InstallmentPlanNumber, "Activity", "REF", "Status", "_id");
            Assert.That(activityStatusRef1Before, Is.EqualTo("New"));
            Assert.That(idStatusRef1Before, Is.Not.Null);
            Console.WriteLine("Done with query mongo\n");
            Console.WriteLine("Running Processing");
            var responseProcessing = await _processing.SendGetForProcessingAsync( requestHeader, json1.InstallmentPlanNumber);
            Assert.That(responseProcessing, Is.Not.Null);
            var responseProcessing1 = await _processing.SendGetForProcessingAsync( requestHeader, json1.InstallmentPlanNumber);
            Assert.That(responseProcessing1, Is.Not.Null);
            Console.WriteLine("Done Running Processing");
            var (activityStatusCol1After, idStatusCol1After) = await _mongoHandler.SendMongoQueryWith2KeysAsync(db,
                "Payment_Records",
                "InstallmentPlanNumber",
                json1.InstallmentPlanNumber, "Activity", "COL", "Status", "_id");
            Assert.That(activityStatusCol1After, Is.EqualTo("PendingBatch"));
            Assert.That(idStatusCol1After, Is.Not.Null);
            var (activityStatusRef1After, idStatusRef1After) = await _mongoHandler.SendMongoQueryWith2KeysAsync(db,
                "Payment_Records",
                "InstallmentPlanNumber",
                json1.InstallmentPlanNumber, "Activity", "REF", "Status", "_id");
            Assert.That(activityStatusRef1After, Is.EqualTo("PendingBatch"));
            Assert.That(idStatusRef1After, Is.Not.Null);
            Console.WriteLine("Done with CreatePlanAndQueryMongoWithFailed");
            Console.WriteLine("Done creating a plan - ipn is -> " + json1.InstallmentPlanNumber);
            return (true, json1.InstallmentPlanNumber);
        }
        catch (Exception exception)
        {
            Console.WriteLine("Error in CreatePlanAndQueryMongoWithFailed -> " +exception+ "\n");
            throw;
        }
    }
    
    public async Task<(bool, string)> CreatePlanAndQueryMongoWithFailedActivities(string terminalApiKey, int businessUnitId, IMongoDatabase db,
        RequestHeader requestHeader,
        CreatePlanDefaultValues createPlanDefaultValues, int fromInstallment, int toInstallment, string refundType,
        string refundStrategy)
    {
        try
        {
            Console.WriteLine("Starting CreatePlanAndQueryMongoWithFailedActivities\n");
            Console.WriteLine("Staring Create Plan");
            var json1 = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!,  requestHeader,
                "PendingCapture", new Random().Next(fromInstallment, toInstallment), terminalApiKey,
                createPlanDefaultValues);
            Console.WriteLine("Done with Create Plan\n");
            Console.WriteLine("Starting query mongo");
            var (activityStatusCol1Before, idStatusCol1Before) = await _mongoHandler.SendMongoQueryWith2KeysAsync(db,
                "Payment_Records",
                "InstallmentPlanNumber",
                json1.InstallmentPlanNumber, "Activity", "COL", "Status", "_id");
            if (activityStatusCol1Before.Equals("New") || activityStatusCol1Before.Equals("PendingBatch"))
            {
                Console.WriteLine("activityStatusCol is in the right state");
            }
            else
            {
                Assert.Fail("activityStatusCol is not in the right state");
            }
            Assert.That(idStatusCol1Before, Is.Not.Null);
            var (activityStatusFnd, idStatusFnd) = await _mongoHandler.SendMongoQueryWith2KeysAsync(db, "Payment_Records",
                "InstallmentPlanNumber",
                json1.InstallmentPlanNumber, "Activity", "FND", "Status", "_id");
            if (activityStatusFnd.Equals("New") || activityStatusFnd.Equals("PendingBatch"))
            {
                Console.WriteLine("activityStatusFnd is in the right state");
            }
            else
            {
                Assert.Fail("activityStatusFnd is not in the right state");
            }
            Assert.That(idStatusFnd, Is.Not.Null);
            Console.WriteLine("Done querying mongo\n");
            Console.WriteLine("Starting refund");
            var jResponseRefund1 = await _installmentPlanNumberRefund.SendRefundRequestAsync( requestHeader,
                json1.InstallmentPlanNumber,
                refundType, refundStrategy);
            Assert.That(jResponseRefund1.RefundId, Is.Not.Null);
            Console.WriteLine("Done refund\n");
            Console.WriteLine("Running Processing");
            var responseProcessing = await _processing.SendGetForProcessingAsync( requestHeader, json1.InstallmentPlanNumber);
            Assert.That(responseProcessing, Is.Not.Null);
            var responseProcessing1 = await _processing.SendGetForProcessingAsync( requestHeader, json1.InstallmentPlanNumber);
            Assert.That(responseProcessing1, Is.Not.Null);
            Console.WriteLine("Done Running Processing");
            Console.WriteLine("Continuing querying mongo");
            var (activityStatusFndAfter, idStatusFndAfter) = await _mongoHandler.SendMongoQueryWith3KeysReturn2StringColumnValuesAsync(db,
                "Payment_Records",
                "InstallmentPlanNumber",
                json1.InstallmentPlanNumber, "Activity", "FND", "Status", "PendingBatch", "Status", "_id");
            Assert.That(activityStatusFndAfter, Is.EqualTo("PendingBatch"));
            Assert.That(idStatusFndAfter, Is.Not.Null);
            var (activityStatusColAfter, idStatusColAfter) = await _mongoHandler.SendMongoQueryWith3KeysReturn2StringColumnValuesAsync(db,
                "Payment_Records",
                "InstallmentPlanNumber",
                json1.InstallmentPlanNumber, "Activity", "COL", "Status", "ProcessingFailed", "Status", "_id");
            Assert.That(activityStatusColAfter, Is.EqualTo("ProcessingFailed"));
            Assert.That(idStatusColAfter, Is.Not.Null);
            Console.WriteLine("Done with query mongo\n");
            Console.WriteLine("Done with CreatePlanAndQueryMongoWithFailedActivities");
            Console.WriteLine("Done creating a plan - ipn is -> " + json1.InstallmentPlanNumber);
            return (true, json1.InstallmentPlanNumber);
        }
        catch (Exception exception)
        {
            Console.WriteLine("Error in CreatePlanAndQueryMongoWithFailedActivities -> " +exception+ "\n");
            throw;
        }
    } 
    
    public async Task<(bool, string)> CreatePlanAndQueryMongoProcessingFailedAllActivities(string terminalApiKey, int businessUnitId, IMongoDatabase db,
        RequestHeader requestHeader,
        CreatePlanDefaultValues createPlanDefaultValues, int fromInstallment, int toInstallment, string refundType, string refundStrategy)
    {
        try
        {
            Console.WriteLine("Starting CreatePlan");
            var json1 = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!,  requestHeader,
                "PendingCapture", new Random().Next(fromInstallment, toInstallment), terminalApiKey,
                createPlanDefaultValues);
            Console.WriteLine("Done CreatePlan\n");
            Console.WriteLine("Starting query mongo");
            
            var (activityStatusCol1Before, idStatusCol1Before) = await _mongoHandler.SendMongoQueryWith2KeysAsync(db,
                "Payment_Records",
                "InstallmentPlanNumber",
                json1.InstallmentPlanNumber, "Activity", "COL", "Status", "_id");
            if (activityStatusCol1Before.Equals("New") || activityStatusCol1Before.Equals("PendingBatch"))
            {
                Console.WriteLine("activityStatusCol is in the right state");
            }
            else
            {
                Assert.Fail("activityStatusCol is not in the right state");
            }
            Assert.That(idStatusCol1Before, Is.Not.Null);
            var (activityStatusFnd, idStatusFnd) = await _mongoHandler.SendMongoQueryWith2KeysAsync(db, "Payment_Records",
                "InstallmentPlanNumber",
                json1.InstallmentPlanNumber, "Activity", "FND", "Status", "_id");
            if (activityStatusFnd.Equals("New") || activityStatusFnd.Equals("PendingBatch"))
            {
                Console.WriteLine("activityStatusFnd is in the right state");
            }
            else
            {
                Assert.Fail("activityStatusFnd is not in the right state");
            }
            Assert.That(idStatusFnd, Is.Not.Null);
            Console.WriteLine("Done querying mongo");
            Console.WriteLine("Starting refund");
            var jResponseRefund1 = await _installmentPlanNumberRefund.SendRefundRequestAsync( requestHeader,
                json1.InstallmentPlanNumber,
                refundType, refundStrategy);
            Assert.That(jResponseRefund1.RefundId, Is.Not.Null);
            Console.WriteLine("Done refund\n");
            Console.WriteLine("Continuing querying mongo");
            var (activityStatusRed1Before, idStatusRed1Before) = await _mongoHandler.SendMongoQueryWith2KeysAsync(db,
                "Payment_Records",
                "InstallmentPlanNumber",
                json1.InstallmentPlanNumber, "Activity", "RED", "Status", "_id");
            Assert.That(activityStatusRed1Before, Is.EqualTo("New"));
            Assert.That(idStatusRed1Before, Is.Not.Null);
            var (activityStatusRef1Before, idStatusRef1Before) = await _mongoHandler.SendMongoQueryWith2KeysAsync(db,
                "Payment_Records",
                "InstallmentPlanNumber",
                json1.InstallmentPlanNumber, "Activity", "REF", "Status", "_id");
            Assert.That(activityStatusRef1Before, Is.EqualTo("New"));
            Assert.That(idStatusRef1Before, Is.Not.Null);
            var responseProcessing = await _processing.SendGetForProcessingAsync( requestHeader, json1.InstallmentPlanNumber);
            Assert.That(responseProcessing, Is.Not.Null);
            var responseProcessing1 = await _processing.SendGetForProcessingAsync( requestHeader, json1.InstallmentPlanNumber);
            Assert.That(responseProcessing1, Is.Not.Null);
            var (activityStatusCol1After, idStatusCol1After) = await _mongoHandler.SendMongoQueryWith2KeysAsync(db,
                "Payment_Records",
                "InstallmentPlanNumber",
                json1.InstallmentPlanNumber, "Activity", "COL", "Status", "_id");
            Assert.That(activityStatusCol1After, Is.EqualTo("ProcessingFailed"));
            Assert.That(idStatusCol1After, Is.Not.Null);
            var (activityStatusFnd1After, idStatusFnd1After) = await _mongoHandler.SendMongoQueryWith2KeysAsync(db,
                "Payment_Records",
                "InstallmentPlanNumber",
                json1.InstallmentPlanNumber, "Activity", "FND", "Status", "_id");
            Assert.That(activityStatusFnd1After, Is.EqualTo("ProcessingFailed"));
            Assert.That(idStatusFnd1After, Is.Not.Null);
            var (activityStatusRed1After, idStatusRed1After) = await _mongoHandler.SendMongoQueryWith2KeysAsync(db,
                "Payment_Records",
                "InstallmentPlanNumber",
                json1.InstallmentPlanNumber, "Activity", "RED", "Status", "_id");
            Assert.That(activityStatusRed1After, Is.EqualTo("ProcessingFailed"));
            Assert.That(idStatusRed1After, Is.Not.Null);
            var (activityStatusRef1After, idStatusRef1After) = await _mongoHandler.SendMongoQueryWith2KeysAsync(db,
                "Payment_Records",
                "InstallmentPlanNumber",
                json1.InstallmentPlanNumber, "Activity", "REF", "Status", "_id");
            Assert.That(activityStatusRef1After, Is.EqualTo("ProcessingFailed"));
            Assert.That(idStatusRef1After, Is.Not.Null);
            Console.WriteLine("Done querying mongo\n");
            Console.WriteLine("Done with CreatePlanAndQueryMongo and Succeeded");
            Console.WriteLine("Done creating a plan - ipn is -> " + json1.InstallmentPlanNumber);
            return (true, json1.InstallmentPlanNumber);
        }
        catch (Exception exception)
        {
            Console.WriteLine("Error in CreatePlanAndQueryMongo -> " + exception + "\n");
            throw;
        }
    }

    public async Task<(bool, string)> CreatePlanAndQueryMongoPendingBatchToBatch(string terminalApiKey, int businessUnitId, IMongoDatabase db,
        RequestHeader requestHeader, CreatePlanDefaultValues createPlanDefaultValues, 
        int fromInstallment, int toInstallment, string refundType, string refundStrategy)
    {
        try
        {
            Console.WriteLine("Starting CreatePlan");
            var json1 = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!,  requestHeader,
                "PendingCapture", new Random().Next(fromInstallment, toInstallment), terminalApiKey,
                createPlanDefaultValues);
            Console.WriteLine("Done CreatePlan\n");
            Console.WriteLine("Starting query mongo");
            
            var (activityStatusCol1Before, idStatusCol1Before) = await _mongoHandler.SendMongoQueryWith2KeysAsync(db,
                "Payment_Records",
                "InstallmentPlanNumber",
                json1.InstallmentPlanNumber, "Activity", "COL", "Status", "_id");
            if (activityStatusCol1Before.Equals("New") || activityStatusCol1Before.Equals("PendingBatch"))
            {
                Console.WriteLine("activityStatusCol is in the right state");
            }
            else
            {
                Assert.Fail("activityStatusCol is not in the right state");
            }

            Assert.That(idStatusCol1Before, Is.Not.Null);
            var (activityStatusFnd, idStatusFnd) = await _mongoHandler.SendMongoQueryWith2KeysAsync(db, "Payment_Records",
                "InstallmentPlanNumber",
                json1.InstallmentPlanNumber, "Activity", "FND", "Status", "_id");
            if (activityStatusFnd.Equals("New") || activityStatusFnd.Equals("PendingBatch"))
            {
                Console.WriteLine("activityStatusFnd is in the right state");
            }
            else
            {
                Assert.Fail("activityStatusFnd is not in the right state");
            }

            Assert.That(idStatusFnd, Is.Not.Null);
            Console.WriteLine("Done querying mongo");
            Console.WriteLine("Starting refund");
            var jResponseRefund1 = await _installmentPlanNumberRefund.SendRefundRequestAsync( requestHeader,
                json1.InstallmentPlanNumber,
                refundType, refundStrategy);
            Assert.That(jResponseRefund1.RefundId, Is.Not.Null);
            Console.WriteLine("Done refund\n");
            Console.WriteLine("Continuing querying mongo");
            var (activityStatusRed1Before, idStatusRed1Before) = await _mongoHandler.SendMongoQueryWith2KeysAsync(db,
                "Payment_Records",
                "InstallmentPlanNumber",
                json1.InstallmentPlanNumber, "Activity", "RED", "Status", "_id");
            Assert.That(activityStatusRed1Before, Is.EqualTo("New"));
            Assert.That(idStatusRed1Before, Is.Not.Null);
            var (activityStatusRef1Before, idStatusRef1Before) = await _mongoHandler.SendMongoQueryWith2KeysAsync(db,
                "Payment_Records",
                "InstallmentPlanNumber",
                json1.InstallmentPlanNumber, "Activity", "REF", "Status", "_id");
            Assert.That(activityStatusRef1Before, Is.EqualTo("New"));
            Assert.That(idStatusRef1Before, Is.Not.Null);
            var responseProcessing = await _processing.SendGetForProcessingAsync( requestHeader, json1.InstallmentPlanNumber);
            Assert.That(responseProcessing, Is.Not.Null);
            var responseProcessing1 = await _processing.SendGetForProcessingAsync( requestHeader, json1.InstallmentPlanNumber);
            Assert.That(responseProcessing1, Is.Not.Null);
            var (activityStatusCol1After, idStatusCol1After) = await _mongoHandler.SendMongoQueryWith2KeysAsync(db,
                "Payment_Records",
                "InstallmentPlanNumber",
                json1.InstallmentPlanNumber, "Activity", "COL", "Status", "_id");
            Assert.That(activityStatusCol1After, Is.EqualTo("PendingBatch"));
            Assert.That(idStatusCol1After, Is.Not.Null);
            var (activityStatusFnd1After, idStatusFnd1After) = await _mongoHandler.SendMongoQueryWith2KeysAsync(db,
                "Payment_Records",
                "InstallmentPlanNumber",
                json1.InstallmentPlanNumber, "Activity", "FND", "Status", "_id");
            Assert.That(activityStatusFnd1After, Is.EqualTo("PendingBatch"));
            Assert.That(idStatusFnd1After, Is.Not.Null);
            var (activityStatusRed1After, idStatusRed1After) = await _mongoHandler.SendMongoQueryWith2KeysAsync(db,
                "Payment_Records",
                "InstallmentPlanNumber",
                json1.InstallmentPlanNumber, "Activity", "RED", "Status", "_id");
            Assert.That(activityStatusRed1After, Is.EqualTo("PendingBatch"));
            Assert.That(idStatusRed1After, Is.Not.Null);
            var (activityStatusRef1After, idStatusRef1After) = await _mongoHandler.SendMongoQueryWith2KeysAsync(db,
                "Payment_Records",
                "InstallmentPlanNumber",
                json1.InstallmentPlanNumber, "Activity", "REF", "Status", "_id");
            Assert.That(activityStatusRef1After, Is.EqualTo("PendingBatch"));
            Assert.That(idStatusRef1After, Is.Not.Null);
            var (activityStatusDel1After, idStatusDel1After) = await _mongoHandler.SendMongoQueryWith2KeysAsync(db,
                "Payment_Records",
                "InstallmentPlanNumber",
                json1.InstallmentPlanNumber, "Activity", "DEL", "Status", "_id");
            Assert.That(activityStatusDel1After, Is.EqualTo("Skipped"));
            Assert.That(idStatusDel1After, Is.Not.Null);
            var jResponseCreatedCol = await _created.SendPutRequestCreatedAsync( requestHeader, idStatusCol1After);
            Assert.That(jResponseCreatedCol, Is.Not.Null);
            var jResponseCreatedFnd = await _created.SendPutRequestCreatedAsync( requestHeader, idStatusFnd1After);
            Assert.That(jResponseCreatedFnd, Is.Not.Null);
            var jResponseCreatedRed = await _created.SendPutRequestCreatedAsync( requestHeader, idStatusRed1After);
            Assert.That(jResponseCreatedRed, Is.Not.Null);
            var jResponseCreatedRef = await _created.SendPutRequestCreatedAsync( requestHeader, idStatusRef1After);
            Assert.That(jResponseCreatedRef, Is.Not.Null);
            var (activityStatusCol1AfterBatched, idStatusCol1AfterBatched) = await _mongoHandler.SendMongoQueryWith2KeysAsync(db,
                "Payment_Records",
                "InstallmentPlanNumber",
                json1.InstallmentPlanNumber, "Activity", "COL", "Status", "_id");
            Assert.That(activityStatusCol1AfterBatched, Is.EqualTo("Batched"));
            Assert.That(idStatusCol1AfterBatched, Is.Not.Null);
            var (activityStatusFnd1AfterBatched, idStatusFnd1AfterBatched) = await _mongoHandler.SendMongoQueryWith2KeysAsync(db,
                "Payment_Records",
                "InstallmentPlanNumber",
                json1.InstallmentPlanNumber, "Activity", "FND", "Status", "_id");
            Assert.That(activityStatusFnd1AfterBatched, Is.EqualTo("Batched"));
            Assert.That(idStatusFnd1AfterBatched, Is.Not.Null);
            var (activityStatusRed1AfterBatched, idStatusRed1AfterBatched) = await _mongoHandler.SendMongoQueryWith2KeysAsync(db,
                "Payment_Records",
                "InstallmentPlanNumber",
                json1.InstallmentPlanNumber, "Activity", "RED", "Status", "_id");
            Assert.That(activityStatusRed1AfterBatched, Is.EqualTo("Batched"));
            Assert.That(idStatusRed1AfterBatched, Is.Not.Null);
            var (activityStatusRef1AfterBatched, idStatusRef1AfterBatched) = await _mongoHandler.SendMongoQueryWith2KeysAsync(db,
                "Payment_Records",
                "InstallmentPlanNumber",
                json1.InstallmentPlanNumber, "Activity", "REF", "Status", "_id");
            Assert.That(activityStatusRef1AfterBatched, Is.EqualTo("Batched"));
            Assert.That(idStatusRef1AfterBatched, Is.Not.Null);
            Console.WriteLine("Done creating a plan - ipn is -> " + json1.InstallmentPlanNumber);
            return (true, json1.InstallmentPlanNumber);
        }
        catch (Exception exception)
        {
            Console.WriteLine("Error in CreatePlanAndQueryMongoPendingBatchToBatch -> " + exception);
            throw;
        }
    }
    
    public async Task<(bool, string)> CreatePlanAndQueryMongoAcknowledged(string terminalApiKey, int businessUnitId, IMongoDatabase db,
        RequestHeader requestHeader, CreatePlanDefaultValues createPlanDefaultValues, 
        int fromInstallment, int toInstallment, string refundType, string refundStrategy, string ackValue, bool ackSuccessesFlag)
    {
        try
        {
            Console.WriteLine("Starting CreatePlan");
            var json1 = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!,  requestHeader,
                "PendingCapture", new Random().Next(fromInstallment, toInstallment), terminalApiKey,
                createPlanDefaultValues);
            Console.WriteLine("Done CreatePlan\n");
            Console.WriteLine("Starting query mongo");
            
            var (activityStatusCol1Before, idStatusCol1Before) = await _mongoHandler.SendMongoQueryWith2KeysAsync(db,
                "Payment_Records",
                "InstallmentPlanNumber",
                json1.InstallmentPlanNumber, "Activity", "COL", "Status", "_id");
            if (activityStatusCol1Before.Equals("New") || activityStatusCol1Before.Equals("PendingBatch"))
            {
                Console.WriteLine("activityStatusCol is in the right state");
            }
            else
            {
                Assert.Fail("activityStatusCol is not in the right state");
            }

            Assert.That(idStatusCol1Before, Is.Not.Null);
            var (activityStatusFnd, idStatusFnd) = await _mongoHandler.SendMongoQueryWith2KeysAsync(db, "Payment_Records",
                "InstallmentPlanNumber",
                json1.InstallmentPlanNumber, "Activity", "FND", "Status", "_id");
            if (activityStatusFnd.Equals("New") || activityStatusFnd.Equals("PendingBatch"))
            {
                Console.WriteLine("activityStatusFnd is in the right state");
            }
            else
            {
                Assert.Fail("activityStatusFnd is not in the right state");
            }

            Assert.That(idStatusFnd, Is.Not.Null);
            Console.WriteLine("Done querying mongo");
            Console.WriteLine("Starting refund");
            var jResponseRefund1 = await _installmentPlanNumberRefund.SendRefundRequestAsync( requestHeader,
                json1.InstallmentPlanNumber,
                refundType, refundStrategy);
            Assert.That(jResponseRefund1.RefundId, Is.Not.Null);
            Console.WriteLine("Done refund\n");
            Console.WriteLine("Continuing querying mongo");
            var (activityStatusRed1Before, idStatusRed1Before) = await _mongoHandler.SendMongoQueryWith2KeysAsync(db,
                "Payment_Records",
                "InstallmentPlanNumber",
                json1.InstallmentPlanNumber, "Activity", "RED", "Status", "_id");
            Assert.That(activityStatusRed1Before, Is.EqualTo("New"));
            Assert.That(idStatusRed1Before, Is.Not.Null);
            var (activityStatusRef1Before, idStatusRef1Before) = await _mongoHandler.SendMongoQueryWith2KeysAsync(db,
                "Payment_Records",
                "InstallmentPlanNumber",
                json1.InstallmentPlanNumber, "Activity", "REF", "Status", "_id");
            Assert.That(activityStatusRef1Before, Is.EqualTo("New"));
            Assert.That(idStatusRef1Before, Is.Not.Null);
            var responseProcessing = await _processing.SendGetForProcessingAsync( requestHeader, json1.InstallmentPlanNumber);
            Assert.That(responseProcessing, Is.Not.Null);
            var responseProcessing1 = await _processing.SendGetForProcessingAsync( requestHeader, json1.InstallmentPlanNumber);
            Assert.That(responseProcessing1, Is.Not.Null);
            var (activityStatusCol1After, idStatusCol1After) = await _mongoHandler.SendMongoQueryWith2KeysAsync(db,
                "Payment_Records",
                "InstallmentPlanNumber",
                json1.InstallmentPlanNumber, "Activity", "COL", "Status", "_id");
            Assert.That(activityStatusCol1After, Is.EqualTo("PendingBatch"));
            Assert.That(idStatusCol1After, Is.Not.Null);
            var (activityStatusFnd1After, idStatusFnd1After) = await _mongoHandler.SendMongoQueryWith2KeysAsync(db,
                "Payment_Records",
                "InstallmentPlanNumber",
                json1.InstallmentPlanNumber, "Activity", "FND", "Status", "_id");
            Assert.That(activityStatusFnd1After, Is.EqualTo("PendingBatch"));
            Assert.That(idStatusFnd1After, Is.Not.Null);
            var (activityStatusRed1After, idStatusRed1After) = await _mongoHandler.SendMongoQueryWith2KeysAsync(db,
                "Payment_Records",
                "InstallmentPlanNumber",
                json1.InstallmentPlanNumber, "Activity", "RED", "Status", "_id");
            Assert.That(activityStatusRed1After, Is.EqualTo("PendingBatch"));
            Assert.That(idStatusRed1After, Is.Not.Null);
            var (activityStatusRef1After, idStatusRef1After) = await _mongoHandler.SendMongoQueryWith2KeysAsync(db,
                "Payment_Records",
                "InstallmentPlanNumber",
                json1.InstallmentPlanNumber, "Activity", "REF", "Status", "_id");
            Assert.That(activityStatusRef1After, Is.EqualTo("PendingBatch"));
            Assert.That(idStatusRef1After, Is.Not.Null);
            var (activityStatusDel1After, idStatusDel1After) = await _mongoHandler.SendMongoQueryWith2KeysAsync(db,
                "Payment_Records",
                "InstallmentPlanNumber",
                json1.InstallmentPlanNumber, "Activity", "DEL", "Status", "_id");
            Assert.That(activityStatusDel1After, Is.EqualTo("Skipped"));
            Assert.That(idStatusDel1After, Is.Not.Null);
            var jResponseCreatedCol = await _created.SendPutRequestCreatedAsync( requestHeader, idStatusCol1After);
            Assert.That(jResponseCreatedCol, Is.Not.Null);
            var jResponseCreatedFnd = await _created.SendPutRequestCreatedAsync( requestHeader, idStatusFnd1After);
            Assert.That(jResponseCreatedFnd, Is.Not.Null);
            var jResponseCreatedRed = await _created.SendPutRequestCreatedAsync( requestHeader, idStatusRed1After);
            Assert.That(jResponseCreatedRed, Is.Not.Null);
            var jResponseCreatedRef = await _created.SendPutRequestCreatedAsync( requestHeader, idStatusRef1After);
            Assert.That(jResponseCreatedRef, Is.Not.Null);
            var (activityStatusCol1AfterBatched, idStatusCol1AfterBatched) = await _mongoHandler.SendMongoQueryWith2KeysAsync(db,
                "Payment_Records",
                "InstallmentPlanNumber",
                json1.InstallmentPlanNumber, "Activity", "COL", "Status", "_id");
            Assert.That(activityStatusCol1AfterBatched, Is.EqualTo("Batched"));
            Assert.That(idStatusCol1AfterBatched, Is.Not.Null);
            var (activityStatusFnd1AfterBatched, idStatusFnd1AfterBatched) = await _mongoHandler.SendMongoQueryWith2KeysAsync(db,
                "Payment_Records",
                "InstallmentPlanNumber",
                json1.InstallmentPlanNumber, "Activity", "FND", "Status", "_id");
            Assert.That(activityStatusFnd1AfterBatched, Is.EqualTo("Batched"));
            Assert.That(idStatusFnd1AfterBatched, Is.Not.Null);
            var (activityStatusRed1AfterBatched, idStatusRed1AfterBatched) = await _mongoHandler.SendMongoQueryWith2KeysAsync(db,
                "Payment_Records",
                "InstallmentPlanNumber",
                json1.InstallmentPlanNumber, "Activity", "RED", "Status", "_id");
            Assert.That(activityStatusRed1AfterBatched, Is.EqualTo("Batched"));
            Assert.That(idStatusRed1AfterBatched, Is.Not.Null);
            var (activityStatusRef1AfterBatched, idStatusRef1AfterBatched) = await _mongoHandler.SendMongoQueryWith2KeysAsync(db,
                "Payment_Records",
                "InstallmentPlanNumber",
                json1.InstallmentPlanNumber, "Activity", "REF", "Status", "_id");
            Assert.That(activityStatusRef1AfterBatched, Is.EqualTo("Batched"));
            Assert.That(idStatusRef1AfterBatched, Is.Not.Null);
            var jResponseSettledCol = await _settled.SendPutRequestSettledAsync( requestHeader, idStatusCol1After, idStatusCol1AfterBatched, ackSuccessesFlag);
            Assert.That(jResponseSettledCol, Is.Not.Null);
            var jResponseSettledFnd = await _settled.SendPutRequestSettledAsync( requestHeader, idStatusFnd1After, idStatusFnd1AfterBatched, ackSuccessesFlag);
            Assert.That(jResponseSettledFnd, Is.Not.Null);
            var jResponseSettledRed = await _settled.SendPutRequestSettledAsync( requestHeader, idStatusRed1After, idStatusRed1AfterBatched, ackSuccessesFlag);
            Assert.That(jResponseSettledRed, Is.Not.Null);
            var jResponseSettledRef = await _settled.SendPutRequestSettledAsync( requestHeader, idStatusRef1After, idStatusRef1AfterBatched, ackSuccessesFlag);
            Assert.That(jResponseSettledRef, Is.Not.Null);
            var (activityStatusCol1AfterAck, idStatusCol1AfterAck) = await _mongoHandler.SendMongoQueryWith2KeysAsync(db,
                "Payment_Records",
                "InstallmentPlanNumber",
                json1.InstallmentPlanNumber, "Activity", "COL", "Status", "_id");
            Assert.That(activityStatusCol1AfterAck, Is.EqualTo(ackValue));
            Assert.That(idStatusCol1AfterAck, Is.Not.Null);
            var (activityStatusFnd1AfterAck, idStatusFnd1AfterAck) = await _mongoHandler.SendMongoQueryWith2KeysAsync(db,
                "Payment_Records",
                "InstallmentPlanNumber",
                json1.InstallmentPlanNumber, "Activity", "FND", "Status", "_id");
            Assert.That(activityStatusFnd1AfterAck, Is.EqualTo(ackValue));
            Assert.That(idStatusFnd1AfterAck, Is.Not.Null);
            var (activityStatusRed1AfterAck, idStatusRed1AfterAck) = await _mongoHandler.SendMongoQueryWith2KeysAsync(db,
                "Payment_Records",
                "InstallmentPlanNumber",
                json1.InstallmentPlanNumber, "Activity", "RED", "Status", "_id");
            Assert.That(activityStatusRed1AfterAck, Is.EqualTo(ackValue));
            Assert.That(idStatusRed1AfterAck, Is.Not.Null);
            var (activityStatusRef1AfterAck, idStatusRef1AfterAck) = await _mongoHandler.SendMongoQueryWith2KeysAsync(db,
                "Payment_Records",
                "InstallmentPlanNumber",
                json1.InstallmentPlanNumber, "Activity", "REF", "Status", "_id");
            Assert.That(activityStatusRef1AfterAck, Is.EqualTo(ackValue));
            Assert.That(idStatusRef1AfterAck, Is.Not.Null);
            Console.WriteLine("Done creating a plan - ipn is -> " + json1.InstallmentPlanNumber);
            return (true, json1.InstallmentPlanNumber);
        }
        catch (Exception exception)
        {
            Console.WriteLine("Error in CreatePlanAndQueryMongoAcknowledgedWithError -> " + exception);
            throw;
        }
    }
    
    public async Task<(bool, string)> CreatePlanAndQueryMongoInvoiced(string terminalApiKey, int businessUnitId, IMongoDatabase db,
        RequestHeader requestHeader, CreatePlanDefaultValues createPlanDefaultValues, 
        int fromInstallment, int toInstallment, string refundType, string refundStrategy, string ackValue, bool ackSuccessesFlag)
    {
        try
        {
            Console.WriteLine("Starting CreatePlan");
            var json1 = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!,  requestHeader,
                "PendingCapture", new Random().Next(fromInstallment, toInstallment), terminalApiKey,
                createPlanDefaultValues);
            Console.WriteLine("Done CreatePlan\n");
            Console.WriteLine("Starting query mongo");
            
            var (activityStatusCol1Before, idStatusCol1Before) = await _mongoHandler.SendMongoQueryWith2KeysAsync(db,
                "Payment_Records",
                "InstallmentPlanNumber",
                json1.InstallmentPlanNumber, "Activity", "COL", "Status", "_id");
            if (activityStatusCol1Before.Equals("New") || activityStatusCol1Before.Equals("PendingBatch"))
            {
                Console.WriteLine("activityStatusCol is in the right state");
            }
            else
            {
                Assert.Fail("activityStatusCol is not in the right state");
            }

            Assert.That(idStatusCol1Before, Is.Not.Null);
            var (activityStatusFnd, idStatusFnd) = await _mongoHandler.SendMongoQueryWith2KeysAsync(db, "Payment_Records",
                "InstallmentPlanNumber",
                json1.InstallmentPlanNumber, "Activity", "FND", "Status", "_id");
            if (activityStatusFnd.Equals("New") || activityStatusFnd.Equals("PendingBatch"))
            {
                Console.WriteLine("activityStatusFnd is in the right state");
            }
            else
            {
                Assert.Fail("activityStatusFnd is not in the right state");
            }

            Assert.That(idStatusFnd, Is.Not.Null);
            Console.WriteLine("Done querying mongo");
            Console.WriteLine("Starting refund");
            var jResponseRefund1 = await _installmentPlanNumberRefund.SendRefundRequestAsync( requestHeader,
                json1.InstallmentPlanNumber,
                refundType, refundStrategy);
            Assert.That(jResponseRefund1.RefundId, Is.Not.Null);
            Console.WriteLine("Done refund\n");
            Console.WriteLine("Continuing querying mongo");
            var (activityStatusRed1Before, idStatusRed1Before) = await _mongoHandler.SendMongoQueryWith2KeysAsync(db,
                "Payment_Records",
                "InstallmentPlanNumber",
                json1.InstallmentPlanNumber, "Activity", "RED", "Status", "_id");
            Assert.That(activityStatusRed1Before, Is.EqualTo("New"));
            Assert.That(idStatusRed1Before, Is.Not.Null);
            var (activityStatusRef1Before, idStatusRef1Before) = await _mongoHandler.SendMongoQueryWith2KeysAsync(db,
                "Payment_Records",
                "InstallmentPlanNumber",
                json1.InstallmentPlanNumber, "Activity", "REF", "Status", "_id");
            Assert.That(activityStatusRef1Before, Is.EqualTo("New"));
            Assert.That(idStatusRef1Before, Is.Not.Null);
            var responseProcessing = await _processing.SendGetForProcessingAsync( requestHeader, json1.InstallmentPlanNumber);
            Assert.That(responseProcessing, Is.Not.Null);
            var responseProcessing1 = await _processing.SendGetForProcessingAsync( requestHeader, json1.InstallmentPlanNumber);
            Assert.That(responseProcessing1, Is.Not.Null);
            var (activityStatusCol1After, idStatusCol1After) = await _mongoHandler.SendMongoQueryWith2KeysAsync(db,
                "Payment_Records",
                "InstallmentPlanNumber",
                json1.InstallmentPlanNumber, "Activity", "COL", "Status", "_id");
            Assert.That(activityStatusCol1After, Is.EqualTo("PendingBatch"));
            Assert.That(idStatusCol1After, Is.Not.Null);
            var (activityStatusFnd1After, idStatusFnd1After) = await _mongoHandler.SendMongoQueryWith2KeysAsync(db,
                "Payment_Records",
                "InstallmentPlanNumber",
                json1.InstallmentPlanNumber, "Activity", "FND", "Status", "_id");
            Assert.That(activityStatusFnd1After, Is.EqualTo("PendingBatch"));
            Assert.That(idStatusFnd1After, Is.Not.Null);
            var (activityStatusRed1After, idStatusRed1After) = await _mongoHandler.SendMongoQueryWith2KeysAsync(db,
                "Payment_Records",
                "InstallmentPlanNumber",
                json1.InstallmentPlanNumber, "Activity", "RED", "Status", "_id");
            Assert.That(activityStatusRed1After, Is.EqualTo("PendingBatch"));
            Assert.That(idStatusRed1After, Is.Not.Null);
            var (activityStatusRef1After, idStatusRef1After) = await _mongoHandler.SendMongoQueryWith2KeysAsync(db,
                "Payment_Records",
                "InstallmentPlanNumber",
                json1.InstallmentPlanNumber, "Activity", "REF", "Status", "_id");
            Assert.That(activityStatusRef1After, Is.EqualTo("PendingBatch"));
            Assert.That(idStatusRef1After, Is.Not.Null);
            var (activityStatusDel1After, idStatusDel1After) = await _mongoHandler.SendMongoQueryWith2KeysAsync(db,
                "Payment_Records",
                "InstallmentPlanNumber",
                json1.InstallmentPlanNumber, "Activity", "DEL", "Status", "_id");
            Assert.That(activityStatusDel1After, Is.EqualTo("Skipped"));
            Assert.That(idStatusDel1After, Is.Not.Null);
            var jResponseCreatedCol = await _created.SendPutRequestCreatedAsync( requestHeader, idStatusCol1After);
            Assert.That(jResponseCreatedCol, Is.Not.Null);
            var jResponseCreatedFnd = await _created.SendPutRequestCreatedAsync( requestHeader, idStatusFnd1After);
            Assert.That(jResponseCreatedFnd, Is.Not.Null);
            var jResponseCreatedRed = await _created.SendPutRequestCreatedAsync( requestHeader, idStatusRed1After);
            Assert.That(jResponseCreatedRed, Is.Not.Null);
            var jResponseCreatedRef = await _created.SendPutRequestCreatedAsync( requestHeader, idStatusRef1After);
            Assert.That(jResponseCreatedRef, Is.Not.Null);
            var (activityStatusCol1AfterBatched, idStatusCol1AfterBatched) = await _mongoHandler.SendMongoQueryWith2KeysAsync(db,
                "Payment_Records",
                "InstallmentPlanNumber",
                json1.InstallmentPlanNumber, "Activity", "COL", "Status", "_id");
            Assert.That(activityStatusCol1AfterBatched, Is.EqualTo("Batched"));
            Assert.That(idStatusCol1AfterBatched, Is.Not.Null);
            var (activityStatusFnd1AfterBatched, idStatusFnd1AfterBatched) = await _mongoHandler.SendMongoQueryWith2KeysAsync(db,
                "Payment_Records",
                "InstallmentPlanNumber",
                json1.InstallmentPlanNumber, "Activity", "FND", "Status", "_id");
            Assert.That(activityStatusFnd1AfterBatched, Is.EqualTo("Batched"));
            Assert.That(idStatusFnd1AfterBatched, Is.Not.Null);
            var (activityStatusRed1AfterBatched, idStatusRed1AfterBatched) = await _mongoHandler.SendMongoQueryWith2KeysAsync(db,
                "Payment_Records",
                "InstallmentPlanNumber",
                json1.InstallmentPlanNumber, "Activity", "RED", "Status", "_id");
            Assert.That(activityStatusRed1AfterBatched, Is.EqualTo("Batched"));
            Assert.That(idStatusRed1AfterBatched, Is.Not.Null);
            var (activityStatusRef1AfterBatched, idStatusRef1AfterBatched) = await _mongoHandler.SendMongoQueryWith2KeysAsync(db,
                "Payment_Records",
                "InstallmentPlanNumber",
                json1.InstallmentPlanNumber, "Activity", "REF", "Status", "_id");
            Assert.That(activityStatusRef1AfterBatched, Is.EqualTo("Batched"));
            Assert.That(idStatusRef1AfterBatched, Is.Not.Null);
            var jResponseSettledCol = await _settled.SendPutRequestSettledAsync( requestHeader, idStatusCol1After, idStatusCol1AfterBatched, ackSuccessesFlag);
            Assert.That(jResponseSettledCol, Is.Not.Null);
            var jResponseSettledFnd = await _settled.SendPutRequestSettledAsync( requestHeader, idStatusFnd1After, idStatusFnd1AfterBatched, ackSuccessesFlag);
            Assert.That(jResponseSettledFnd, Is.Not.Null);
            var jResponseSettledRed = await _settled.SendPutRequestSettledAsync( requestHeader, idStatusRed1After, idStatusRed1AfterBatched, ackSuccessesFlag);
            Assert.That(jResponseSettledRed, Is.Not.Null);
            var jResponseSettledRef = await _settled.SendPutRequestSettledAsync( requestHeader, idStatusRef1After, idStatusRef1AfterBatched, ackSuccessesFlag);
            Assert.That(jResponseSettledRef, Is.Not.Null);
            var (activityStatusCol1AfterAck, idStatusCol1AfterAck) = await _mongoHandler.SendMongoQueryWith2KeysAsync(db,
                "Payment_Records",
                "InstallmentPlanNumber",
                json1.InstallmentPlanNumber, "Activity", "COL", "Status", "_id");
            Assert.That(activityStatusCol1AfterAck, Is.EqualTo(ackValue));
            Assert.That(idStatusCol1AfterAck, Is.Not.Null);
            var (activityStatusFnd1AfterAck, idStatusFnd1AfterAck) = await _mongoHandler.SendMongoQueryWith2KeysAsync(db,
                "Payment_Records",
                "InstallmentPlanNumber",
                json1.InstallmentPlanNumber, "Activity", "FND", "Status", "_id");
            Assert.That(activityStatusFnd1AfterAck, Is.EqualTo(ackValue));
            Assert.That(idStatusFnd1AfterAck, Is.Not.Null);
            var (activityStatusRed1AfterAck, idStatusRed1AfterAck) = await _mongoHandler.SendMongoQueryWith2KeysAsync(db,
                "Payment_Records",
                "InstallmentPlanNumber",
                json1.InstallmentPlanNumber, "Activity", "RED", "Status", "_id");
            Assert.That(activityStatusRed1AfterAck, Is.EqualTo(ackValue));
            Assert.That(idStatusRed1AfterAck, Is.Not.Null);
            var (activityStatusRef1AfterAck, idStatusRef1AfterAck) = await _mongoHandler.SendMongoQueryWith2KeysAsync(db,
                "Payment_Records",
                "InstallmentPlanNumber",
                json1.InstallmentPlanNumber, "Activity", "REF", "Status", "_id");
            Assert.That(activityStatusRef1AfterAck, Is.EqualTo(ackValue));
            Assert.That(idStatusRef1AfterAck, Is.Not.Null);
            var jResponse = _createPaymentInvoice.SendPostRequestCreatePaymentInvoiceAsync( requestHeader);
            Assert.That(jResponse, Is.Not.Null);
            var jResponseInvoicedCol =
                _invoiced.SendPutRequestInvoicedAsync( requestHeader, idStatusCol1AfterAck, "AutomationRun");
            Assert.That(jResponseInvoicedCol, Is.Not.Null);
            var jResponseInvoicedFnd =
                _invoiced.SendPutRequestInvoicedAsync( requestHeader, idStatusFnd1AfterAck, "AutomationRun");
            Assert.That(jResponseInvoicedFnd, Is.Not.Null);
            var jResponseInvoicedRed =
                _invoiced.SendPutRequestInvoicedAsync( requestHeader, idStatusRed1AfterAck, "AutomationRun");
            Assert.That(jResponseInvoicedRed, Is.Not.Null);
            var jResponseInvoicedRef =
                _invoiced.SendPutRequestInvoicedAsync( requestHeader, activityStatusRef1AfterAck, "AutomationRun");
            Assert.That(jResponseInvoicedRef, Is.Not.Null);
            Console.WriteLine("Done creating a plan - ipn is -> " + json1.InstallmentPlanNumber);
            return (true, json1.InstallmentPlanNumber);
        }
        catch (Exception exception)
        {
            Console.WriteLine("Error in CreatePlanAndQueryMongoInvoiced -> " + exception);
            throw;
        }
    }
    
    public async Task<(bool, string)> CreatePlanAndQueryMongoBeforeChargeBack(string terminalApiKey, int businessUnitId, IMongoDatabase db,
        RequestHeader requestHeader, CreatePlanDefaultValues createPlanDefaultValues, 
        int fromInstallment, int toInstallment)
    {
        try
        {
            Console.WriteLine("Starting CreatePlan");
            var json1 = await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!,  requestHeader,
                "PendingCapture", new Random().Next(fromInstallment, toInstallment), terminalApiKey,
                createPlanDefaultValues);
            Console.WriteLine("Done CreatePlan\n");
            Console.WriteLine("Starting query mongo");
            
            var (activityStatusCol1Before, idStatusCol1Before) = await _mongoHandler.SendMongoQueryWith2KeysAsync(db,
                "Payment_Records",
                "InstallmentPlanNumber",
                json1.InstallmentPlanNumber, "Activity", "COL", "Status", "_id");
            if (activityStatusCol1Before.Equals("New") || activityStatusCol1Before.Equals("PendingBatch"))
            {
                Console.WriteLine("activityStatusCol is in the right state");
            }
            else
            {
                Assert.Fail("activityStatusCol is not in the right state");
            }

            Assert.That(idStatusCol1Before, Is.Not.Null);
            var (activityStatusFnd, idStatusFnd) = await _mongoHandler.SendMongoQueryWith2KeysAsync(db, "Payment_Records",
                "InstallmentPlanNumber",
                json1.InstallmentPlanNumber, "Activity", "FND", "Status", "_id");
            if (activityStatusFnd.Equals("New") || activityStatusFnd.Equals("PendingBatch"))
            {
                Console.WriteLine("activityStatusFnd is in the right state");
            }
            else
            {
                Assert.Fail("activityStatusFnd is not in the right state");
            }

            Assert.That(idStatusFnd, Is.Not.Null);
            Console.WriteLine("Done querying mongo");
            Console.WriteLine("Starting refund");
            var responseProcessing = await _processing.SendGetForProcessingAsync( requestHeader, json1.InstallmentPlanNumber);
            Assert.That(responseProcessing, Is.Not.Null);
            var responseProcessing1 = await _processing.SendGetForProcessingAsync( requestHeader, json1.InstallmentPlanNumber);
            Assert.That(responseProcessing1, Is.Not.Null);
            var (activityStatusCol1After, idStatusCol1After) = await _mongoHandler.SendMongoQueryWith2KeysAsync(db,
                "Payment_Records",
                "InstallmentPlanNumber",
                json1.InstallmentPlanNumber, "Activity", "COL", "Status", "_id");
            Assert.That(activityStatusCol1After, Is.EqualTo("PendingBatch"));
            Assert.That(idStatusCol1After, Is.Not.Null);
            var (activityStatusFnd1After, idStatusFnd1After) = await _mongoHandler.SendMongoQueryWith2KeysAsync(db,
                "Payment_Records",
                "InstallmentPlanNumber",
                json1.InstallmentPlanNumber, "Activity", "FND", "Status", "_id");
            Assert.That(activityStatusFnd1After, Is.EqualTo("PendingBatch"));
            Assert.That(idStatusFnd1After, Is.Not.Null);
            Console.WriteLine("Done creating a plan - ipn is -> " + json1.InstallmentPlanNumber);
            return (true, json1.InstallmentPlanNumber);
        }
        catch (Exception exception)
        {
            Console.WriteLine("Error in CreatePlanAndQueryMongoBeforeChargeBack -> " + exception);
            throw;
        }
    }

    public async Task<bool> DoingChargeBackAndQueryMongo(RequestHeader requestHeader, string ipn, IMongoDatabase db,
        string refundType, string refundStrategy)
    {
        try
        {
            Console.WriteLine("Starting DoingChargeBackAndQueryMongo");
            
            var captureId = await _getPgtl.ValidatePgtlKeyValueAsync(_envConfig.StoreProcedureUrl,
                requestHeader, ipn, "CaptureId");
            var notificationResponse = await _bluesnapFunctionality.SendPostRequestBluesnapNotifications(
                _envConfig.NotificationUrl, captureId!, "CHARGEBACK", "NEW", "Set-Liabilty-Merchant");
            Assert.That(notificationResponse.Contains("accepted"));
            var responseDisputesCharges =
                await _disputeCharges.SendGetDisputeChargesAsync( requestHeader, ipn);
            var responseDispute = await _setDisputeLiabilityId.SendPutRequestSetDisputeLiabilityIdAsync( requestHeader,
                responseDisputesCharges.Disputes[0].DisputedChargesMerchantInfoId.ToString());
            Assert.That(responseDispute!.Status.Equals("Open"));
            var (activityStatus, idStatus) = await _mongoHandler.SendMongoQueryWith2KeysAsync(db, "Payment_Records",
                "InstallmentPlanNumber",
                ipn, "Activity", "CHB", "Status", "_id");
            Assert.That(activityStatus, Is.EqualTo("New"));
            Assert.That(idStatus, Is.Not.Null);
            var jResponseRefund1 = await _installmentPlanNumberRefund.SendRefundRequestAsync( requestHeader,
                ipn, refundType, refundStrategy);
            Assert.That(jResponseRefund1.RefundId, Is.Not.Null);
            Console.WriteLine("Done refund\n");
            Console.WriteLine("Continuing querying mongo");
            var (activityStatusRed1Before, idStatusRed1Before) = await _mongoHandler.SendMongoQueryWith2KeysAsync(db,
                "Payment_Records",
                "InstallmentPlanNumber",
                ipn, "Activity", "RED", "Status", "_id");
            Assert.That(activityStatusRed1Before, Is.EqualTo("New"));
            Assert.That(idStatusRed1Before, Is.Not.Null);
            var (activityStatusRef1Before, idStatusRef1Before) = await _mongoHandler.SendMongoQueryWith2KeysAsync(db,
                "Payment_Records",
                "InstallmentPlanNumber",
                ipn, "Activity", "REF", "Status", "_id");
            Assert.That(activityStatusRef1Before, Is.EqualTo("New"));
            Assert.That(idStatusRef1Before, Is.Not.Null);
            Console.WriteLine("Done DoingChargeBackAndQueryMongo");
            return true;
        }
        catch (Exception exception)
        {
            Console.WriteLine("Error in DoingChargeBackAndQueryMongo -> " +exception);
            throw;
        }
    }
    
    public async Task<bool> DoingChargeBackAndQueryMongoWithWon(RequestHeader requestHeader, string ipn, 
        string refundType, string refundStrategy, IMongoDatabase db)
    {
        try
        {
            Console.WriteLine("Starting DoingChargeBackAndQueryMongoWithWon");
            
            var captureId = await _getPgtl.ValidatePgtlKeyValueAsync(_envConfig.StoreProcedureUrl,
                requestHeader, ipn, "CaptureId");
            var notificationResponse = await _bluesnapFunctionality.SendPostRequestBluesnapNotifications(
                _envConfig.NotificationUrl, captureId!, "CHARGEBACK", "NEW", "Set-Liabilty-Merchant");
            Assert.That(notificationResponse.Contains("accepted"));
            var responseDisputesCharges =
                await _disputeCharges.SendGetDisputeChargesAsync( requestHeader, ipn);
            var responseDispute = await _setDisputeLiabilityId.SendPutRequestSetDisputeLiabilityIdAsync( requestHeader,
                responseDisputesCharges.Disputes[0].DisputedChargesMerchantInfoId.ToString());
            Assert.That(responseDispute!.Status.Equals("Open"));
            var (activityStatus, idStatus) = await _mongoHandler.SendMongoQueryWith2KeysAsync(db, "Payment_Records",
                "InstallmentPlanNumber",
                ipn, "Activity", "CHB", "Status", "_id");
            Assert.That(activityStatus, Is.EqualTo("New"));
            Assert.That(idStatus, Is.Not.Null);
            await Task.Delay(120*1000);
            var notificationResponseAfter = await _bluesnapFunctionality.SendPostRequestBluesnapNotifications(
                _envConfig.NotificationUrl, captureId!, "CHARGEBACK", "Completed_Won", "Set-Liabilty-Merchant");
            Assert.That(notificationResponseAfter.Contains("accepted"));
            Console.WriteLine("Continuing querying mongo");
            var (statusValue, idStatus1) = await _mongoHandler.SendMongoQueryWith2KeysAsync(db, "Payment_Records",
                "InstallmentPlanNumber",
                ipn, "Activity", "PRP", "Status", "_id");
            Assert.That(statusValue, Is.Not.Null);
            Assert.That(idStatus1, Is.Not.Null);
            var jResponseRefund1 = await _installmentPlanNumberRefund.SendRefundRequestAsync( requestHeader,
                ipn, refundType, refundStrategy);
            Assert.That(jResponseRefund1.RefundId, Is.Not.Null);
            Console.WriteLine("Done refund\n");
            var (activityStatusRed1Before, idStatusRed1Before) = await _mongoHandler.SendMongoQueryWith2KeysAsync(db,
                "Payment_Records",
                "InstallmentPlanNumber",
                ipn, "Activity", "RED", "Status", "_id");
            Assert.That(activityStatusRed1Before, Is.EqualTo("New"));
            Assert.That(idStatusRed1Before, Is.Not.Null);
            var (activityStatusRef1Before, idStatusRef1Before) = await _mongoHandler.SendMongoQueryWith2KeysAsync(db,
                "Payment_Records",
                "InstallmentPlanNumber",
                ipn, "Activity", "REF", "Status", "_id");
            Assert.That(activityStatusRef1Before, Is.EqualTo("New"));
            Assert.That(idStatusRef1Before, Is.Not.Null);
            Console.WriteLine("Done DoingChargeBackAndQueryMongoWithWon");
            return true;
        }
        catch (Exception exception)
        {
            Console.WriteLine("Error in DoingChargeBackAndQueryMongoWithWon -> " +exception);
            throw;
        }
    }
}