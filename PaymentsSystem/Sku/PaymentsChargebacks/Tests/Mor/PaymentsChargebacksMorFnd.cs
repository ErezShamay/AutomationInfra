using MongoDB.Driver;
using Allure.NUnit;
using Allure.NUnit.Attributes;
using NUnit.Framework;
using Splitit.Automation.NG.Backend.BaseActions;
using Splitit.Automation.NG.Backend.Services.AdminApi.InstallmentPlan.InstallmentsPlanFunctionality;
using Splitit.Automation.NG.Backend.Services.Chargebacks.Functionality;
using Splitit.Automation.NG.Backend.Services.DisputesV2.Dispute.Functionality;
using Splitit.Automation.NG.Backend.Services.V1.InstallmentPlan.InstallmentPlanApiEndPoints;
using Splitit.Automation.NG.Backend.Services.V3.DefaultValues;
using Splitit.Automation.NG.Backend.Services.V3.InstallmentPlan.InstallmentPlanApiEndPoint;
using Splitit.Automation.NG.Backend.Services.V3.InstallmentPlan.InstallmentPlanResponses;
using Splitit.Automation.NG.Backend.Tests.TestsSetups;
using Splitit.Automation.NG.Utilities.EnvironmentsConfig;

namespace Splitit.Automation.NG.PaymentsSystem.Sku.PaymentsChargebacks.Tests.Mor;

[TestFixture]
[AllureNUnit]
[AllureSuite("SkuFunBiwTests")]
[AllureDisplayIgnored]
[Parallelizable(ParallelScope.All)]
public class PaymentsChargebacksMorFnd
{
    private RequestHeader? _requestHeader;
    private readonly InstallmentPlans _installmentPlans = new();
    private readonly MongoHandler _mongoHandler = new();
    private IMongoDatabase? _db;
    private readonly FunctionalityUtil _functionalityUtil = new();
    private readonly GetPgtl _getPgtl = new();
    private readonly EnvConfig _envConfig = new();
    private readonly ChargeFunctionality _chargeFunctionality = new();
    private readonly FullCaptureFunctionality _fullCaptureFunctionality = new();
    private readonly PutInternalStatusDisputeIdFunctionality _putInternalStatusDisputeIdFunctionality = new();
    private int _convertedAmount;

    [OneTimeSetUp]
    public async Task InitSetUp()
    {
        Console.WriteLine("Starting PaymentsChargebacksMor InitSetUp");
        var testsSetup = new TestsSetup();
        testsSetup.Setup();
        var sendAdminLoginRequest = new SendAdminLoginRequest();
        _requestHeader = await sendAdminLoginRequest.DoAdminLogin(
            Environment.GetEnvironmentVariable("AccessTokenURI")!,
            Environment.GetEnvironmentVariable("ClientSecret")!,
            Environment.GetEnvironmentVariable("SplititMockTerminal")!,
            Environment.GetEnvironmentVariable("clientId")!);
        _db = _mongoHandler.MongoConnect(_envConfig.MongoConnection, "Splitit_Payments");
        Console.WriteLine("Done with PaymentsChargebacksMor InitSetUp");
    }
    
    [TestCase(Category = "PaymentsChargebacksMorTests")]
    [Test(Description = "TestValidate_MOR_CHB_opened_and_won"), CancelAfter(120 * 1000)]
    public async Task TestValidate_MOR_CHB_opened_and_won()
    {
        try
        {
            Console.WriteLine("\nStarting TestValidate_MOR_CHB_opened_and_won");
            var (planCreateResponse, _, chargeBackId) = await InitPlanAndOpenDispute(_requestHeader!, 
                Environment.GetEnvironmentVariable("PaymentsDbsFunMerchantApiKey")!, 4, 1000);
            var (billingAmountChb, _) = await _mongoHandler.SendMongoQueryWith3KeysReturn2StringColumnValuesAsync(_db!, "Payment_Records", 
                "InstallmentPlanNumber", planCreateResponse.InstallmentPlanNumber, "Activity", "CHB", 
                "Status","New", "BillingAmount", "_id");
            _convertedAmount = ConvertStringWithDecimalPointToInt(billingAmountChb);
            Assert.That(_convertedAmount, Is.EqualTo(250));
            var (billingAmountChp, _) = await _mongoHandler.SendMongoQueryWith3KeysReturn2StringColumnValuesAsync(_db!, "Payment_Records", 
                "InstallmentPlanNumber", planCreateResponse.InstallmentPlanNumber, "Activity", "CHP", 
                "Status","New", "BillingAmount", "_id");
            _convertedAmount = ConvertStringWithDecimalPointToInt(billingAmountChp);
            Assert.That(_convertedAmount, Is.EqualTo(500));
            var (billingAmountCrp, _) = await _mongoHandler.SendMongoQueryWith3KeysReturn2StringColumnValuesAsync(_db!, "Payment_Records", 
                "InstallmentPlanNumber", planCreateResponse.InstallmentPlanNumber, "Activity", "CRP", 
                "Status","New", "BillingAmount", "_id");
            _convertedAmount = ConvertStringWithDecimalPointToInt(billingAmountCrp);
            Assert.That(_convertedAmount, Is.EqualTo(0));
            var updateStatusResponse = await _functionalityUtil.SendUpdateRequestPutChargebacksIdStatusAsync(
                _requestHeader!, chargeBackId, "Won");
            Assert.That(updateStatusResponse!.Chargeback.Status.Equals("Won"));
            var (billingAmountRpr, _) = await _mongoHandler.SendMongoQueryWith3KeysReturn2StringColumnValuesAsync(_db!, "Payment_Records", 
                "InstallmentPlanNumber", planCreateResponse.InstallmentPlanNumber, "Activity", "RPR", 
                "Status","New", "BillingAmount", "_id");
            _convertedAmount = ConvertStringWithDecimalPointToInt(billingAmountRpr);
            Assert.That(_convertedAmount, Is.EqualTo(250));
            var (billingAmountChr, _) = await _mongoHandler.SendMongoQueryWith3KeysReturn2StringColumnValuesAsync(_db!, "Payment_Records", 
                "InstallmentPlanNumber", planCreateResponse.InstallmentPlanNumber, "Activity", "CHR", 
                "Status","New", "BillingAmount", "_id");
            _convertedAmount = ConvertStringWithDecimalPointToInt(billingAmountChr);
            Assert.That(_convertedAmount, Is.EqualTo(250));
            var docsList = await _mongoHandler.SendMongoQueryWith2KeysAndReturnListOfResultAsync(_db!, "Payment_Records",
                "InstallmentPlanNumber",
                planCreateResponse.InstallmentPlanNumber, "Activity", "COL", "Sku", "_id");
            Assert.That(_mongoHandler.ValidateValueInDocsList(docsList!, "BillingAmount", "500"));
            var (billingAmountChrAfterWon, _) = await _mongoHandler.SendMongoQueryWith3KeysReturn2StringColumnValuesAsync(_db!, "Payment_Records", 
                "InstallmentPlanNumber", planCreateResponse.InstallmentPlanNumber, "Activity", "CHR", 
                "Status","New", "BillingAmount", "_id");
            _convertedAmount = ConvertStringWithDecimalPointToInt(billingAmountChrAfterWon);
            Assert.That(_convertedAmount, Is.EqualTo(500));
            Console.WriteLine("Done TestValidate_MOR_CHB_opened_and_won");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidate_MOR_CHB_opened_and_won \n" + exception + "\n");
        }
    }
    
    [TestCase(Category = "PaymentsChargebacksMorTests")]
    [Test(Description = "TestValidate_CHB_opened"), CancelAfter(120 * 1000)]
    public async Task TestValidate_CHB_opened()
    {
        try
        {
            Console.WriteLine("\nStarting TestValidate_CHB_opened");
            var (planCreateResponse, _, _) = await InitPlanAndOpenDispute(_requestHeader!,
                Environment.GetEnvironmentVariable("PaymentsDbsFunMerchantApiKey")!, 4, 1000);
            var (billingAmountChb, _) = await _mongoHandler.SendMongoQueryWith3KeysReturn2StringColumnValuesAsync(_db!, "Payment_Records", 
                "InstallmentPlanNumber", planCreateResponse.InstallmentPlanNumber, "Activity", "CHB", 
                "Status","New", "BillingAmount", "_id");
            _convertedAmount = ConvertStringWithDecimalPointToInt(billingAmountChb);
            Assert.That(_convertedAmount, Is.EqualTo(250));
            var (billingAmountChp, _) = await _mongoHandler.SendMongoQueryWith3KeysReturn2StringColumnValuesAsync(_db!, "Payment_Records", 
                "InstallmentPlanNumber", planCreateResponse.InstallmentPlanNumber, "Activity", "CHP", 
                "Status","New", "BillingAmount", "_id");
            _convertedAmount = ConvertStringWithDecimalPointToInt(billingAmountChp);
            Assert.That(_convertedAmount, Is.EqualTo(500));
            Console.WriteLine("Done TestValidate_CHB_opened");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidate_CHB_opened \n" + exception + "\n");
        }
    }
    
    [TestCase(Category = "PaymentsChargebacksMorTests")]
    [Test(Description = "TestValidate_CHB_lost"), CancelAfter(120 * 1000)]
    public async Task TestValidate_CHB_lost()
    {
        try
        {
            Console.WriteLine("\nStarting TestValidate_CHB_lost");
            var (planCreateResponse, _, chargeBackId) = await InitPlanAndOpenDispute(_requestHeader!, 
                Environment.GetEnvironmentVariable("PaymentsDbsFunMerchantApiKey")!, 4, 1000);
            var updateStatusResponse = await _functionalityUtil.SendUpdateRequestPutChargebacksIdStatusAsync(
               _requestHeader!, chargeBackId, "Lost");
            Assert.That(updateStatusResponse!.Chargeback.Status.Equals("Lost"));
            var (billingAmountRed, _) = await _mongoHandler.SendMongoQueryWith3KeysReturn2StringColumnValuesAsync(_db!, "Payment_Records", 
                "InstallmentPlanNumber", planCreateResponse.InstallmentPlanNumber, "Activity", "RED", 
                "Status","New", "BillingAmount", "_id");
            _convertedAmount = ConvertStringWithDecimalPointToInt(billingAmountRed);
            Assert.That(_convertedAmount, Is.EqualTo(500));
            var (billingAmountChp, _) = await _mongoHandler.SendMongoQueryWith3KeysReturn2StringColumnValuesAsync(_db!, "Payment_Records", 
                "InstallmentPlanNumber", planCreateResponse.InstallmentPlanNumber, "Activity", "CHP", 
                "Status","New", "BillingAmount", "_id");
            _convertedAmount = ConvertStringWithDecimalPointToInt(billingAmountChp);
            Assert.That(_convertedAmount, Is.EqualTo(500));
            Console.WriteLine("Done TestValidate_CHB_lost");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidate_CHB_lost \n" + exception + "\n");
        }
    }
    
    [TestCase(Category = "PaymentsChargebacksMorTests")]
    [Test(Description = "TestValidate_chargeback_won_full_capture_failed_merchant_liability"), CancelAfter(120 * 1000)]
    public async Task TestValidate_chargeback_won_full_capture_failed_merchant_liability()
    {
        try
        {
            Console.WriteLine("\nStarting TestValidate_chargeback_won_full_capture_failed_merchant_liability");
            var (planCreateResponse, _, chargeBackId) = await InitPlanAndOpenDispute(_requestHeader!,
                Environment.GetEnvironmentVariable("PaymentsDbsFunMerchantApiKey")!, 4, 1000);
            var updateStatusResponse = await _functionalityUtil.SendUpdateRequestPutChargebacksIdStatusAsync(
               _requestHeader!, chargeBackId, "Won");
            Assert.That(updateStatusResponse!.Chargeback.Status.Equals("Won"));
            var (billingAmountRpr, _) = await _mongoHandler.SendMongoQueryWith3KeysReturn2StringColumnValuesAsync(_db!, "Payment_Records", 
                "InstallmentPlanNumber", planCreateResponse.InstallmentPlanNumber, "Activity", "RPR", 
                "Status","New", "BillingAmount", "_id");
            _convertedAmount = ConvertStringWithDecimalPointToInt(billingAmountRpr);
            Assert.That(_convertedAmount, Is.EqualTo(250));
            var (billingAmountChr, _) = await _mongoHandler.SendMongoQueryWith3KeysReturn2StringColumnValuesAsync(_db!, "Payment_Records", 
                "InstallmentPlanNumber", planCreateResponse.InstallmentPlanNumber, "Activity", "CHR", 
                "Status","New", "BillingAmount", "_id");
            _convertedAmount = ConvertStringWithDecimalPointToInt(billingAmountChr);
            Assert.That(_convertedAmount, Is.EqualTo(250));
            Console.WriteLine("Done TestValidate_chargeback_won_full_capture_failed_merchant_liability");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidate_chargeback_won_full_capture_failed_merchant_liability \n" + exception + "\n");
        }
    }
    
    [TestCase(Category = "PaymentsChargebacksMorTests")]
    [Test(Description = "TestValidate_chargeback_won_full_capture_failed_Splitit_liability"), CancelAfter(120 * 1000)]
    public async Task TestValidate_chargeback_won_full_capture_failed_Splitit_liability()
    {
        try
        {
            Console.WriteLine("\nStarting TestValidate_chargeback_won_full_capture_failed_Splitit_liability");
            var (planCreateResponse, _, chargeBackId) = await InitPlanAndOpenDispute(_requestHeader!, 
                Environment.GetEnvironmentVariable("PaymentsDbsFunMerchantApiKey")!, 4, 1000);
            var fullCaptureResponse =
                await _fullCaptureFunctionality.SendPostRequestFullCaptureAsync( _requestHeader!,
                    planCreateResponse.InstallmentPlanNumber, 0);
            Assert.That(fullCaptureResponse.ResponseHeader.Succeeded, Is.False);
            var updateStatusResponse = await _functionalityUtil.SendUpdateRequestPutChargebacksIdStatusAsync(
               _requestHeader!, chargeBackId, "Won");
            Assert.That(updateStatusResponse!.Chargeback.Status.Equals("Won"));
            var (billingAmountRpr, _) = await _mongoHandler.SendMongoQueryWith3KeysReturn2StringColumnValuesAsync(_db!, "Payment_Records", 
                "InstallmentPlanNumber", planCreateResponse.InstallmentPlanNumber, "Activity", "RPR", 
                "Status","New", "BillingAmount", "_id");
            _convertedAmount = ConvertStringWithDecimalPointToInt(billingAmountRpr);
            Assert.That(_convertedAmount, Is.EqualTo(250));
            var (billingAmountChr, _) = await _mongoHandler.SendMongoQueryWith3KeysReturn2StringColumnValuesAsync(_db!, "Payment_Records", 
                "InstallmentPlanNumber", planCreateResponse.InstallmentPlanNumber, "Activity", "CHR", 
                "Status","New", "BillingAmount", "_id");
            _convertedAmount = ConvertStringWithDecimalPointToInt(billingAmountChr);
            Assert.That(_convertedAmount, Is.EqualTo(250));
            var (billingAmountChrAgain, _) = await _mongoHandler.SendMongoQueryWith3KeysReturn2StringColumnValuesAsync(_db!, "Payment_Records", 
                "InstallmentPlanNumber", planCreateResponse.InstallmentPlanNumber, "Activity", "CHR", 
                "Status","New", "BillingAmount", "_id");
            _convertedAmount = ConvertStringWithDecimalPointToInt(billingAmountChrAgain);
            Assert.That(_convertedAmount, Is.EqualTo(250));
            Console.WriteLine("Done TestValidate_chargeback_won_full_capture_failed_Splitit_liability");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidate_chargeback_won_full_capture_failed_Splitit_liability \n" + exception + "\n");
        }
    }
    
    [TestCase(Category = "PaymentsChargebacksMerFndTests")]
    [Test(Description = "TestValidate_MER_CHB_On_Cleared_Plan"), CancelAfter(120 * 1000)]
    public async Task TestValidate_MER_CHB_On_Cleared_Plan()
    {
        try
        {
            Console.WriteLine("\nStarting TestValidate_MER_CHB_On_Cleared_Plan");
            var (planCreateResponse, _, _) = await InitPlanCaptureTilClearedAndOpenDispute(_requestHeader!, 
                Environment.GetEnvironmentVariable("PaymentsDbsFunMerchantApiKey")!, 4, 1000);
            var (billingAmountChb, _) = await _mongoHandler.SendMongoQueryWith3KeysReturn2StringColumnValuesAsync(
                _db!, "Payment_Records", 
                "InstallmentPlanNumber", planCreateResponse.InstallmentPlanNumber, "Activity", "CHB", 
                "Status","New", "BillingAmount", "_id");
            _convertedAmount = ConvertStringWithDecimalPointToInt(billingAmountChb);
            Assert.That(_convertedAmount, Is.EqualTo(250));
            Console.WriteLine("Done with TestValidate_MER_CHB_On_Cleared_Plan");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidate_MER_CHB_On_Cleared_Plan \n" + exception + "\n");
        }
    }
    
    private async Task<(ResponseV3.ResponseRoot installmentPlan, string? captureId, string Id)> InitPlanCaptureTilClearedAndOpenDispute(RequestHeader requestHeader, 
        string terminalApiKey, int numberOfInstallments, double totalAmount)
    {
        try
        {
            Console.WriteLine("Starting InitPlanAndOpenDispute");
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            createPlanDefaultValues.planData.totalAmount = totalAmount;
            createPlanDefaultValues.planData.numberOfInstallments = numberOfInstallments;
            var installmentPlan = await _installmentPlans.CreatePlanAsync(_envConfig.ApiV3, 
                requestHeader, "Active", numberOfInstallments, terminalApiKey, createPlanDefaultValues);
            await Task.Delay(5 * 1000);
            var captureResponse2 = await _chargeFunctionality.SendPostRequestChargeFunctionalityAsync(requestHeader,
                installmentPlan.InstallmentPlanNumber);
            Assert.That(captureResponse2.ResponseHeader.Succeeded, Is.True);
            var captureResponse3 = await _chargeFunctionality.SendPostRequestChargeFunctionalityAsync(requestHeader,
                installmentPlan.InstallmentPlanNumber);
            Assert.That(captureResponse3.ResponseHeader.Succeeded, Is.True);
            var captureResponse4 = await _chargeFunctionality.SendPostRequestChargeFunctionalityAsync(requestHeader,
                installmentPlan.InstallmentPlanNumber);
            Assert.That(captureResponse4.ResponseHeader.Succeeded, Is.True);
            var captureId = await _getPgtl.ValidatePgtlKeyValueAsync(_envConfig.StoreProcedureUrl,
                requestHeader, installmentPlan.InstallmentPlanNumber, "CaptureId");
            var openDisputeResponse = await _functionalityUtil.SendPostRequestPostChargebacksAsync(_requestHeader!,
                installmentPlan.InstallmentPlanNumber, captureId!, "USD", "Invalid Data", 
                installmentPlan.Installments[0].Amount); 
            Assert.That(openDisputeResponse!.Chargeback.Id, Is.Not.Null);
            var getChargeBacks = await _functionalityUtil.SendGetRequestGetChargebacksIdAsync(
                requestHeader, openDisputeResponse.Chargeback.Id);
            Assert.That(getChargeBacks.Chargeback.Id, Is.Not.Null);
            var putInternalStatusDisputeIdResponse = await _putInternalStatusDisputeIdFunctionality.SendPutRequestPutInternalStatusDisputeIdAsync(
                requestHeader, getChargeBacks.Chargeback.Id, "DisputeSubmitted");
            Assert.That(putInternalStatusDisputeIdResponse.Dispute.InternalStatus, Is.EqualTo("DisputeSubmitted"));
            Console.WriteLine("Done with InitPlanAndOpenDispute");
            return (installmentPlan, captureId, getChargeBacks.Chargeback.Id);
        }
        catch (Exception e)
        {
            Console.WriteLine("Error in InitPlanAndOpenDispute" + e);
            throw;
        }
    }
    
    private async Task<(ResponseV3.ResponseRoot installmentPlan, string? captureId, string Id)> InitPlanAndOpenDispute(RequestHeader requestHeader, 
        string terminalApiKey, int numberOfInstallments, double totalAmount, bool isFullCaptureFailed = false)
    {
        try
        {
            Console.WriteLine("Starting InitPlanAndOpenDispute");
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            createPlanDefaultValues.planData.totalAmount = totalAmount;
            createPlanDefaultValues.planData.numberOfInstallments = numberOfInstallments;
            if (isFullCaptureFailed)
            {
                createPlanDefaultValues.billingAddress.addressLine1 = "Automation-Charge1";
            }
            var installmentPlan = await _installmentPlans.CreatePlanAsync(_envConfig.ApiV3, 
                requestHeader, "Active", numberOfInstallments, terminalApiKey, createPlanDefaultValues);
            await Task.Delay(5 * 1000);
            var captureResponse = await _chargeFunctionality.SendPostRequestChargeFunctionalityAsync(requestHeader,
                installmentPlan.InstallmentPlanNumber);
            Assert.That(captureResponse.ResponseHeader.Succeeded, Is.True);
            await Task.Delay(5 * 1000);
            var captureId = await _getPgtl.ValidatePgtlKeyValueAsync(_envConfig.StoreProcedureUrl,
                requestHeader, installmentPlan.InstallmentPlanNumber, "CaptureId");
            var openDisputeResponse = await _functionalityUtil.SendPostRequestPostChargebacksAsync(_requestHeader!,
                installmentPlan.InstallmentPlanNumber, captureId!, "USD", "Invalid Data", 
                installmentPlan.Installments[0].Amount); 
            Assert.That(openDisputeResponse!.Chargeback.Id, Is.Not.Null);
            var getChargeBacks = await _functionalityUtil.SendGetRequestGetChargebacksIdAsync(
                requestHeader, openDisputeResponse.Chargeback.Id);
            Assert.That(getChargeBacks.Chargeback.Id, Is.Not.Null);
            var putInternalStatusDisputeIdResponse = await _putInternalStatusDisputeIdFunctionality.SendPutRequestPutInternalStatusDisputeIdAsync(
                requestHeader, getChargeBacks.Chargeback.Id, "DisputeSubmitted");
            Assert.That(putInternalStatusDisputeIdResponse.Dispute.InternalStatus, Is.EqualTo("DisputeSubmitted"));
            Console.WriteLine("Done with InitPlanAndOpenDispute");
            return (installmentPlan, captureId, getChargeBacks.Chargeback.Id);
        }
        catch (Exception e)
        {
            Console.WriteLine("Error in InitPlanAndOpenDispute" + e);
            throw;
        }
    }
    
    private int ConvertStringWithDecimalPointToInt(string stringNumber)
    {
        var parts = stringNumber.Split('.');
        var integerPart = parts[0];
        var integerNumber = int.Parse(integerPart);
        return integerNumber;
    }
}