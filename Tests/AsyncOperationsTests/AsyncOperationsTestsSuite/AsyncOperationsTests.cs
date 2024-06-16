using Nito.AsyncEx;
using Allure.NUnit;
using Allure.NUnit.Attributes;
using NUnit.Framework;
using Splitit.Automation.NG.Backend.BaseActions;
using Splitit.Automation.NG.Backend.Services.AdminApi.Automation.Functionality;
using Splitit.Automation.NG.Backend.Services.AdminApi.InstallmentPlan.InstallmentPlanResponses;
using Splitit.Automation.NG.Backend.Services.AdminApi.InstallmentPlan.InstallmentsPlanFunctionality;
using Splitit.Automation.NG.Backend.Services.V1.InstallmentPlan.InstallmentPlanApiEndPoints;
using Splitit.Automation.NG.Backend.Services.V3.DefaultValues;
using Splitit.Automation.NG.Backend.Services.V3.InstallmentPlan.InstallmentPlanApiEndPoint;
using Splitit.Automation.NG.Backend.Tests.TestsSetups;
using Splitit.Automation.NG.Utilities.CommonUtilities;
using FullPlanInfoIpn = Splitit.Automation.NG.Backend.Services.AdminApi.InstallmentPlan.InstallmentsPlanFunctionality.FullPlanInfoIpn;

namespace Splitit.Automation.NG.Backend.Tests.AsyncOperationsTests.AsyncOperationsTestsSuite;

[TestFixture]
[AllureNUnit]
[AllureSuite("AsyncOperationsTestsSuite")]
[AllureDisplayIgnored]
public class AsyncOperationsTests
{
    private readonly InstallmentPlans _installmentPlans;
    private readonly RefundFunctionality _refundFunctionality;
    private RequestHeader? _requestHeader;
    private readonly FullPlanInfoIpn _fullPlanInfoIpn;
    private readonly FullCaptureFunctionality _fullCaptureFunctionality;
    private readonly ReAuthFunctionality _reAuthFunctionality;
    private readonly GetPgtl _getPgtl;
    private readonly ChargeFunctionality _chargeFunctionality;
    private readonly InstallmentPlanNumberCancel _installmentPlanNumberCancel;
    private readonly UpdateInstallmentProcessDateTimeFunctionality _updateInstallmentProcessDateTimeFunctionality;
    private readonly RefundUtilities _refundUtilities;

    public AsyncOperationsTests()
    {
        Console.WriteLine("Staring Setup");
        _refundUtilities = new RefundUtilities();
        _installmentPlans = new InstallmentPlans();
        _refundFunctionality = new RefundFunctionality();
        _fullPlanInfoIpn = new FullPlanInfoIpn();
        _fullCaptureFunctionality = new FullCaptureFunctionality();
        _reAuthFunctionality = new ReAuthFunctionality();
        _getPgtl = new GetPgtl();
        _chargeFunctionality = new ChargeFunctionality();
        _installmentPlanNumberCancel = new InstallmentPlanNumberCancel();
        _updateInstallmentProcessDateTimeFunctionality = new UpdateInstallmentProcessDateTimeFunctionality();
        Console.WriteLine("Setup Succeeded\n");
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
    }

    [Category("AsyncOperations")]
    [Test(Description = "TestValidate_FullRefund"), CancelAfter(80 * 1000)]
    public async Task TestValidate_FullRefund()
    {
        try
        {
            Console.WriteLine("\nStarting TestValidate_FullRefund");
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");


            createPlanDefaultValues.planData.totalAmount = 9;
            var planCreateResponse = await _installmentPlans.CreatePlanAsync(
                Environment.GetEnvironmentVariable("ApiV3")!, _requestHeader!,"Active", 3, 
                Environment.GetEnvironmentVariable("SplititMockTerminal")!, createPlanDefaultValues);

            var jResponseRefund = await _refundFunctionality.SendRefundRequestAsync( _requestHeader!, planCreateResponse.InstallmentPlanNumber, "FullRefund");
            Assert.That(jResponseRefund.InstallmentPlan.RefundAmount, Is.Not.Null);

            var jResponseFullPlanInfoAfter = await _fullPlanInfoIpn.SendGetFullPlanInfoIpnAsync(_requestHeader!, planCreateResponse.InstallmentPlanNumber);

            // Plan
            var plan = new RefundAssertionsObject(planCreateResponse.InstallmentPlanNumber, "Cleared",
                false, 3, 3, 9, 0, 0);
            _refundUtilities.AfterRefundPlanValidation(jResponseFullPlanInfoAfter, plan);

            // Installment 1
            var firstInstallment = new RefundAssertionsObject(planCreateResponse.InstallmentPlanNumber, "Finished",
                true, 3, 3, 3, 0, 0);
            _refundUtilities.AfterRefundInstallmentValidation(jResponseFullPlanInfoAfter, firstInstallment);

            // Installment 2
            var secondInstallment = new RefundAssertionsObject(planCreateResponse.InstallmentPlanNumber, "Canceled",
                false, 0, 0, 3, 0, 1);
            _refundUtilities.AfterRefundInstallmentValidation(jResponseFullPlanInfoAfter, secondInstallment);

            // Installment 3
            var thirdInstallment = new RefundAssertionsObject(planCreateResponse.InstallmentPlanNumber, "Canceled",
                false, 0, 0, 3, 0, 2);
            _refundUtilities.AfterRefundInstallmentValidation(jResponseFullPlanInfoAfter, thirdInstallment);

            Console.WriteLine("TestValidate_FullRefund is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidate_FullRefund\n" + exception + "\n");
        }
    }

    [Category("AsyncOperations")]
    [Test(Description = "TestValidate_Success_AfterFullCapturePlanAmountAndOutstandingAmountNotEffected"), CancelAfter(80 * 1000)]
    public async Task TestValidate_Success_AfterFullCapturePlanAmountAndOutstandingAmountNotEffected()
    {
        try
        {
            Console.WriteLine(
                "Starting with TestValidate_Success_AfterFullCapturePlanAmountAndOutstandingAmountNotEffected");
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");

            createPlanDefaultValues.planData.totalAmount = 9;
            var planCreateResponse = await _installmentPlans.CreatePlanAsync(
                Environment.GetEnvironmentVariable("ApiV3")!, _requestHeader!, "Active", 3, 
                Environment.GetEnvironmentVariable("SplititMockTerminal")!, createPlanDefaultValues);

            var jResponseFullCapture = await _fullCaptureFunctionality.SendPostRequestFullCaptureAsync(_requestHeader!, planCreateResponse.InstallmentPlanNumber, 0);
            Assert.That(jResponseFullCapture.ResponseHeader.Succeeded);

            var jResponseFullRefund = await _refundFunctionality.SendRefundRequestAsync(_requestHeader!, planCreateResponse.InstallmentPlanNumber, "FullRefund");

            //Uniq asserts
            Assert.That(jResponseFullCapture.InstallmentPlan.Amount.Value.Equals(jResponseFullRefund.InstallmentPlan.Amount.Value));
            Assert.That(jResponseFullCapture.InstallmentPlan.OutstandingAmount.Value.Equals(jResponseFullRefund.InstallmentPlan.OutstandingAmount.Value));
            Assert.That(jResponseFullCapture.InstallmentPlan.Installments[^1].Amount.Value.Equals(jResponseFullRefund.InstallmentPlan.Installments[^1].RefundAmount.Value));
            Assert.That(jResponseFullCapture.InstallmentPlan.RefundAmount.Value.Equals(jResponseFullRefund.InstallmentPlan.RefundAmount.Value), Is.False);

            await Task.Delay(5 * 1000);

            var jResponseFullPlanInfoAfter = await _fullPlanInfoIpn.SendGetFullPlanInfoIpnAsync(_requestHeader!, planCreateResponse.InstallmentPlanNumber);

            // Plan
            var plan = new RefundAssertionsObject(planCreateResponse.InstallmentPlanNumber, "Cleared",
                false, 9, 9, 9, 0, 0);
            _refundUtilities.AfterRefundPlanValidation(jResponseFullPlanInfoAfter, plan);

            // Installment 1
            var firstInstallment = new RefundAssertionsObject(planCreateResponse.InstallmentPlanNumber, "Finished",
                true, 3, 3, 3, 0, 0);
            _refundUtilities.AfterRefundInstallmentValidation(jResponseFullPlanInfoAfter, firstInstallment);

            // Installment 2
            var secondInstallment = new RefundAssertionsObject(planCreateResponse.InstallmentPlanNumber, "Deleted",
                false, 3, 0, 3, 0, 1);
            _refundUtilities.AfterRefundInstallmentValidation(jResponseFullPlanInfoAfter, secondInstallment);

            // Installment 3
            var thirdInstallment = new RefundAssertionsObject(planCreateResponse.InstallmentPlanNumber, "Deleted",
                false, 3, 0, 3, 0, 2);
            _refundUtilities.AfterRefundInstallmentValidation(jResponseFullPlanInfoAfter, thirdInstallment);

            // Installment 4 (FullCapture)
            var fourthInstallment = new RefundAssertionsObject(planCreateResponse.InstallmentPlanNumber, "Finished",
                true, 6, 6, 6, 0, 3);
            _refundUtilities.AfterRefundInstallmentValidation(jResponseFullPlanInfoAfter, fourthInstallment);

            Console.WriteLine("Done with TestValidate_Success_AfterFullCapturePlanAmountAndOutstandingAmountNotEffected");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidate_Success_AfterFullCapturePlanAmountAndOutstandingAmountNotEffected" +
                        exception);
        }
    }

    [Category("AsyncOperations")]
    [Test(Description = "TestValidate_Success_AfterDelayed_PlanStaysOnDelayed"), CancelAfter(80 * 1000)]
    public async Task TestValidate_Success_AfterDelayed_PlanStaysOnDelayed()
    {
        try
        {
            Console.WriteLine("Starting with TestValidate_Success_AfterDelayed_PlanStaysOnDelayed");
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");

            createPlanDefaultValues.planData.totalAmount = 9;
            createPlanDefaultValues.billingAddress.addressLine1 = "Automation-Auth2";
            var planCreateResponse = await _installmentPlans.CreatePlanAsync(
                Environment.GetEnvironmentVariable("ApiV3")!, _requestHeader!, "Active", 3, 
                Environment.GetEnvironmentVariable("SplititMockTerminal")!, createPlanDefaultValues);

            var jResponseAuth = await _reAuthFunctionality.SendPostRequestReAuthAsync( _requestHeader!, planCreateResponse.InstallmentPlanNumber, "yes");
            Assert.That(jResponseAuth.Errors, Is.Not.Null);

            await _refundFunctionality.SendRefundRequestAsync( 
                _requestHeader!, planCreateResponse.InstallmentPlanNumber, "AmountToRefund", "FutureInstallmentsLast", null!, 2);

            var jResponseFullPlanInfoAfter = await _fullPlanInfoIpn.SendGetFullPlanInfoIpnAsync(_requestHeader!, planCreateResponse.InstallmentPlanNumber);

            // Plan
            var plan = new RefundAssertionsObject(planCreateResponse.InstallmentPlanNumber, "PendingPaymentUpdate",
                false, 9, 2, 9, 6, 0);
            _refundUtilities.AfterRefundPlanValidation(jResponseFullPlanInfoAfter, plan);

            // Installment 1
            var firstInstallment = new RefundAssertionsObject(planCreateResponse.InstallmentPlanNumber, "Finished",
                true, 3, 2, 3, 0, 0);
            _refundUtilities.AfterRefundInstallmentValidation(jResponseFullPlanInfoAfter, firstInstallment);

            // Installment 2
            var secondInstallment = new RefundAssertionsObject(planCreateResponse.InstallmentPlanNumber, "WaitingForProcessDate",
                false, 3, 0, 3, 0, 1);
            _refundUtilities.AfterRefundInstallmentValidation(jResponseFullPlanInfoAfter, secondInstallment);

            // Installment 3
            var thirdInstallment = new RefundAssertionsObject(planCreateResponse.InstallmentPlanNumber, "WaitingForProcessDate",
                false, 3, 0, 3, 0, 2);
            _refundUtilities.AfterRefundInstallmentValidation(jResponseFullPlanInfoAfter, thirdInstallment);

            Console.WriteLine("Done with TestValidate_Success_AfterDelayed_PlanStaysOnDelayed");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidate_Success_AfterDelayed_PlanStaysOnDelayed" + exception);
        }
    }

    [Category("AsyncOperations")]
    [Test(Description = "TestValidate_Success_OnPartialAmount_PriorityToCharged_WhenValueBelowChargedInstallments"), CancelAfter(80 * 1000)]
    public async Task TestValidate_Success_OnPartialAmount_PriorityToCharged_WhenValueBelowChargedInstallments()
    {
        try
        {
            Console.WriteLine(
                "Starting with TestValidate_Success_OnPartialAmount_PriorityToCharged_WhenValueBelowChargedInstallments");
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");

            createPlanDefaultValues.planData.totalAmount = 9;
            var planCreateResponse = await _installmentPlans.CreatePlanAsync(
                Environment.GetEnvironmentVariable("ApiV3")!, _requestHeader!, "Active", 3, 
                Environment.GetEnvironmentVariable("SplititMockTerminal")!, createPlanDefaultValues);

            var jResponseRefund = await _refundFunctionality.SendRefundRequestAsync( 
                _requestHeader!, planCreateResponse.InstallmentPlanNumber, "AmountToRefund", "FutureInstallmentsLast", null!, 2);
            Assert.That(jResponseRefund.ResponseHeader.Succeeded!.Value);

            await Task.Delay(5 * 1000);

            var jResponseFullPlanInfoAfter = await _fullPlanInfoIpn.SendGetFullPlanInfoIpnAsync( _requestHeader!,planCreateResponse.InstallmentPlanNumber);

            // Plan
            var plan = new RefundAssertionsObject(planCreateResponse.InstallmentPlanNumber, "InProgress",
                false, 9, 2, 9, 6, 0);
            _refundUtilities.AfterRefundPlanValidation(jResponseFullPlanInfoAfter, plan);

            // Installment 1
            var firstInstallment = new RefundAssertionsObject(planCreateResponse.InstallmentPlanNumber, "Finished",
                true, 3, 2, 3, 0, 0);
            _refundUtilities.AfterRefundInstallmentValidation(jResponseFullPlanInfoAfter, firstInstallment);

            // Installment 2
            var secondInstallment = new RefundAssertionsObject(planCreateResponse.InstallmentPlanNumber, "WaitingForProcessDate",
                false, 3, 0, 3, 0, 1);
            _refundUtilities.AfterRefundInstallmentValidation(jResponseFullPlanInfoAfter, secondInstallment);

            // Installment 3
            var thirdInstallment = new RefundAssertionsObject(planCreateResponse.InstallmentPlanNumber, "WaitingForProcessDate",
                false, 3, 0, 3, 0, 2);
            _refundUtilities.AfterRefundInstallmentValidation(jResponseFullPlanInfoAfter, thirdInstallment);

            Console.WriteLine(
                "Done with TestValidate_Success_OnPartialAmount_PriorityToCharged_WhenValueBelowChargedInstallments");
        }
        catch (Exception exception)
        {
            Assert.Fail(
                "Error in TestValidate_Success_OnPartialAmount_PriorityToCharged_WhenValueBelowChargedInstallments" +
                exception);
        }
    }

    [Category("AsyncOperations")]
    [Test(Description = "TestValidate_Success_OnPartialAmount_PriorityToCharged_WhenValueExceedsChargedInstallments"), CancelAfter(80 * 1000)]
    public async Task TestValidate_Success_OnPartialAmount_PriorityToCharged_WhenValueExceedsChargedInstallments()
    {
        try
        {
            Console.WriteLine(
                "Starting with TestValidate_Success_OnPartialAmount_PriorityToCharged_WhenValueExceedsChargedInstallments");
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");

            createPlanDefaultValues.planData.totalAmount = 9;
            var planCreateResponse = await _installmentPlans.CreatePlanAsync(
                Environment.GetEnvironmentVariable("ApiV3")!, _requestHeader!, "Active", 3, 
                Environment.GetEnvironmentVariable("SplititMockTerminal")!,createPlanDefaultValues);

            var jResponseRefund = await _refundFunctionality.SendRefundRequestAsync( _requestHeader!,
                planCreateResponse.InstallmentPlanNumber, "AmountToRefund", "FutureInstallmentsLast", null!, 5);
            Assert.That(jResponseRefund.ResponseHeader.Succeeded!.Value);

            await Task.Delay(5 * 1000);

            var jResponseFullPlanInfoAfter = await _fullPlanInfoIpn.SendGetFullPlanInfoIpnAsync(_requestHeader!, planCreateResponse.InstallmentPlanNumber);

            // Plan
            var plan = new RefundAssertionsObject(planCreateResponse.InstallmentPlanNumber, "InProgress",
                false, 7, 3, 9, 4, 0);
            _refundUtilities.AfterRefundPlanValidation(jResponseFullPlanInfoAfter, plan);

            // Installment 1
            var firstInstallment = new RefundAssertionsObject(planCreateResponse.InstallmentPlanNumber, "Finished",
                true, 3, 3, 3, 0, 0);
            _refundUtilities.AfterRefundInstallmentValidation(jResponseFullPlanInfoAfter, firstInstallment);

            // Installment 2
            var secondInstallment = new RefundAssertionsObject(planCreateResponse.InstallmentPlanNumber, "WaitingForProcessDate",
                false, 2, 0, 3, 0, 1);
            _refundUtilities.AfterRefundInstallmentValidation(jResponseFullPlanInfoAfter, secondInstallment);

            // Installment 3
            var thirdInstallment = new RefundAssertionsObject(planCreateResponse.InstallmentPlanNumber, "WaitingForProcessDate",
                false, 2, 0, 3, 0, 2);
            _refundUtilities.AfterRefundInstallmentValidation(jResponseFullPlanInfoAfter, thirdInstallment);

            Console.WriteLine("Done with TestValidate_Success_OnPartialAmount_PriorityToCharged_WhenValueExceedsChargedInstallments");
        }
        catch (Exception exception)
        {
            Assert.Fail(
                "Error in TestValidate_Success_OnPartialAmount_PriorityToCharged_WhenValueExceedsChargedInstallments" +
                exception);
        }
    }

    [Category("AsyncOperations")]
    [Test(Description =
         "TestValidate_Success_OnPartialAmount_PriorityToNonCharged_WhenValueBelowNonChargedInstallments"), CancelAfter(80 * 1000)]
    public async Task TestValidate_Success_OnPartialAmount_PriorityToNonCharged_WhenValueBelowNonChargedInstallments()
    {
        try
        {
            Console.WriteLine(
                "Starting with TestValidate_Success_OnPartialAmount_PriorityToNonCharged_WhenValueBelowNonChargedInstallments");
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");

            createPlanDefaultValues.planData.totalAmount = 9;
            var planCreateResponse = await _installmentPlans.CreatePlanAsync(
                Environment.GetEnvironmentVariable("ApiV3")!, _requestHeader!,"Active", 3, 
                Environment.GetEnvironmentVariable("SplititMockTerminal")!, createPlanDefaultValues);

            var jResponseRefund = await _refundFunctionality.SendRefundRequestAsync( 
                _requestHeader!,planCreateResponse.InstallmentPlanNumber, "AmountToRefund", "FutureInstallmentsFirst", null!, 2);
            Assert.That(jResponseRefund.ResponseHeader.Succeeded!.Value);

            await Task.Delay(5 * 1000);

            var jResponseFullPlanInfoAfter = await _fullPlanInfoIpn.SendGetFullPlanInfoIpnAsync(_requestHeader!, planCreateResponse.InstallmentPlanNumber);

            // Plan
            var plan = new RefundAssertionsObject(planCreateResponse.InstallmentPlanNumber, "InProgress",
                false, 7, 0, 9, 4, 0);
            _refundUtilities.AfterRefundPlanValidation(jResponseFullPlanInfoAfter, plan);

            // Installment 1
            var firstInstallment = new RefundAssertionsObject(planCreateResponse.InstallmentPlanNumber, "Finished",
                false, 3, 0, 3, 0, 0);
            _refundUtilities.AfterRefundInstallmentValidation(jResponseFullPlanInfoAfter, firstInstallment);

            // Installment 2
            var secondInstallment = new RefundAssertionsObject(planCreateResponse.InstallmentPlanNumber, "WaitingForProcessDate",
                false, 2, 0, 3, 0, 1);
            _refundUtilities.AfterRefundInstallmentValidation(jResponseFullPlanInfoAfter, secondInstallment);

            // Installment 3
            var thirdInstallment = new RefundAssertionsObject(planCreateResponse.InstallmentPlanNumber, "WaitingForProcessDate",
                false, 2, 0, 3, 0, 2);
            _refundUtilities.AfterRefundInstallmentValidation(jResponseFullPlanInfoAfter, thirdInstallment);

            Console.WriteLine(
                "Done with TestValidate_Success_OnPartialAmount_PriorityToNonCharged_WhenValueBelowNonChargedInstallments");
        }
        catch (Exception exception)
        {
            Assert.Fail(
                "Error in TestValidate_Success_OnPartialAmount_PriorityToNonCharged_WhenValueBelowNonChargedInstallments" +
                exception);
        }
    }

    [Category("AsyncOperations")]
    [Test(Description = "TestValidate_Success_OnPartialAmount_PriorityToNonCharged_WhenValueExceedsNonChargedInstallments"), CancelAfter(80 * 1000)]
    public async Task TestValidate_Success_OnPartialAmount_PriorityToNonCharged_WhenValueExceedsNonChargedInstallments()
    {
        try
        {
            Console.WriteLine(
                "Starting with TestValidate_Success_OnPartialAmount_PriorityToNonCharged_WhenValueExceedsNonChargedInstallments");
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");

            createPlanDefaultValues.planData.totalAmount = 9;
            var planCreateResponse = await _installmentPlans.CreatePlanAsync(
                Environment.GetEnvironmentVariable("ApiV3")!, _requestHeader!, "Active", 3, 
                Environment.GetEnvironmentVariable("SplititMockTerminal")!, createPlanDefaultValues);

            await _refundFunctionality.SendRefundRequestAsync( 
                _requestHeader!, planCreateResponse.InstallmentPlanNumber, "AmountToRefund", "FutureInstallmentsFirst", null!, 8);

            await Task.Delay(5 * 1000);

            var jResponseFullPlanInfoAfter = await _fullPlanInfoIpn.SendGetFullPlanInfoIpnAsync(_requestHeader!, planCreateResponse.InstallmentPlanNumber);

            // Plan
            var plan = new RefundAssertionsObject(planCreateResponse.InstallmentPlanNumber, "Cleared",
                false, 3, 2, 9,0, 0);
            _refundUtilities.AfterRefundPlanValidation(jResponseFullPlanInfoAfter, plan);

            // Installment 1
            var firstInstallment = new RefundAssertionsObject(planCreateResponse.InstallmentPlanNumber, "Finished",
                true, 3, 2, 3, 0, 0);
            _refundUtilities.AfterRefundInstallmentValidation(jResponseFullPlanInfoAfter, firstInstallment);

            // Installment 2
            var secondInstallment = new RefundAssertionsObject(planCreateResponse.InstallmentPlanNumber, "Canceled",
                false, 0, 0, 3, 0, 1);
            _refundUtilities.AfterRefundInstallmentValidation(jResponseFullPlanInfoAfter, secondInstallment);

            // Installment 3
            var thirdInstallment = new RefundAssertionsObject(planCreateResponse.InstallmentPlanNumber, "Canceled",
                false, 0, 0, 3, 0, 2);
            _refundUtilities.AfterRefundInstallmentValidation(jResponseFullPlanInfoAfter, thirdInstallment);

            Console.WriteLine(
                "Done with TestValidate_Success_OnPartialAmount_PriorityToNonCharged_WhenValueExceedsNonChargedInstallments");
        }
        catch (Exception exception)
        {
            Assert.Fail(
                "Error in TestValidate_Success_OnPartialAmount_PriorityToNonCharged_WhenValueExceedsNonChargedInstallments" +
                exception);
        }
    }

    [Category("AsyncOperations")]
    [Test(Description = "TestValidate_Success_OnFullAmountIndicationRefundAllRefundableInstallmentsAndPutPlanOnCleared"), CancelAfter(80 * 1000)]
    public async Task TestValidate_Success_OnFullAmountIndicationRefundAllRefundableInstallmentsAndPutPlanOnCleared()
    {
        try
        {
            Console.WriteLine(
                "Starting with TestValidate_Success_OnFullAmountIndicationRefundAllRefundableInstallmentsAndPutPlanOnCleared");
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");

            createPlanDefaultValues.planData.totalAmount = 10;
            var planCreateResponse = await _installmentPlans.CreatePlanAsync(
                Environment.GetEnvironmentVariable("ApiV3")!, _requestHeader!,"Active", 3, 
                Environment.GetEnvironmentVariable("SplititMockTerminal")!, createPlanDefaultValues);
            
            var jResponseChargeNext = await _chargeFunctionality.SendPostRequestChargeFunctionalityAsync( 
                _requestHeader!, planCreateResponse.InstallmentPlanNumber);
            Assert.That(jResponseChargeNext.ResponseHeader.Succeeded);

            var jResponseRefund = await _refundFunctionality.SendRefundRequestAsync( 
                _requestHeader!, planCreateResponse.InstallmentPlanNumber, "FullRefund");
            Assert.That(jResponseRefund.ResponseHeader.Succeeded!.Value);

            await Task.Delay(15 * 1000);

            var jResponseFullPlanInfoAfter = await _fullPlanInfoIpn.SendGetFullPlanInfoIpnAsync( 
                _requestHeader!, planCreateResponse.InstallmentPlanNumber);

            // Plan
            var plan = new RefundAssertionsObject(planCreateResponse.InstallmentPlanNumber, "Cleared",
                false, 6.66, 6.66, 10, 0, 0);
            _refundUtilities.AfterRefundPlanValidation(jResponseFullPlanInfoAfter, plan);

            // Installment 1
            var firstInstallment = new RefundAssertionsObject(planCreateResponse.InstallmentPlanNumber, "Finished",
                true, 3.33, 3.33, 3.33, 0, 0);
            _refundUtilities.AfterRefundInstallmentValidation(jResponseFullPlanInfoAfter, firstInstallment);

            // Installment 2
            var secondInstallment = new RefundAssertionsObject(planCreateResponse.InstallmentPlanNumber, "Finished",
                true, 3.33, 3.33, 3.33, 0, 1);
            _refundUtilities.AfterRefundInstallmentValidation(jResponseFullPlanInfoAfter, secondInstallment);

            // Installment 3
            var thirdInstallment = new RefundAssertionsObject(planCreateResponse.InstallmentPlanNumber, "Canceled",
                false, 0, 0, 3.34, 0, 2);
            _refundUtilities.AfterRefundInstallmentValidation(jResponseFullPlanInfoAfter, thirdInstallment);

            Console.WriteLine(
                "Done with TestValidate_Success_OnFullAmountIndicationRefundAllRefundableInstallmentsAndPutPlanOnCleared");
        }
        catch (Exception exception)
        {
            Assert.Fail(
                "Error in TestValidate_Success_OnFullAmountIndicationRefundAllRefundableInstallmentsAndPutPlanOnCleared" +
                exception);
        }
    }

    [Category("AsyncOperations")]
    [Test(Description = "TestValidate_Failure_OnFullAmountBothRefundActionAndVoidActionFailedForChargedInstallments_ReturnRefundPartiallyFailedError"), CancelAfter(80 * 1000)]
    public async Task
        TestValidate_Failure_OnFullAmountBothRefundActionAndVoidActionFailedForChargedInstallments_ReturnRefundPartiallyFailedError()
    {
        try
        {
            Console.WriteLine(
                "Starting with TestValidate_Failure_OnFullAmountBothRefundActionAndVoidActionFailedForChargedInstallments_ReturnRefundPartiallyFailedError");
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");

            createPlanDefaultValues.planData.totalAmount = 10;
            createPlanDefaultValues.billingAddress.addressLine1 = "Configuration-2";
            var planCreateResponse = await _installmentPlans.CreatePlanAsync(
                Environment.GetEnvironmentVariable("ApiV3")!, _requestHeader!, "Active", 3, 
                Environment.GetEnvironmentVariable("SplititMockTerminal")!,createPlanDefaultValues);

            var jResponseChargeNext = await _chargeFunctionality.SendPostRequestChargeFunctionalityAsync( 
                _requestHeader!,planCreateResponse.InstallmentPlanNumber);
            Assert.That(jResponseChargeNext.ResponseHeader.Succeeded);

            var jResponseRefund = await _refundFunctionality.SendRefundRequestAsync( 
                _requestHeader!,planCreateResponse.InstallmentPlanNumber, "AmountToRefund", "FutureInstallmentsLast", null!, 7);
            Assert.That(jResponseRefund.ResponseHeader.Succeeded!.Value);

            await Task.Delay(5 * 1000);

            var jResponseFullPlanInfoAfter = await _fullPlanInfoIpn.SendGetFullPlanInfoIpnAsync(
                _requestHeader!, planCreateResponse.InstallmentPlanNumber);

            // Plan
            var plan = new RefundAssertionsObject(planCreateResponse.InstallmentPlanNumber, "InProgress",
                false, 9.66, 0, 10, 3, 0);
            _refundUtilities.AfterRefundPlanValidation(jResponseFullPlanInfoAfter, plan);

            // Installment 1
            var firstInstallment = new RefundAssertionsObject(planCreateResponse.InstallmentPlanNumber, "Finished",
                false, 3.33, 0, 3.33, 0, 0);
            _refundUtilities.AfterRefundInstallmentValidation(jResponseFullPlanInfoAfter, firstInstallment);

            // Installment 2
            var secondInstallment = new RefundAssertionsObject(planCreateResponse.InstallmentPlanNumber, "Finished",
                false, 3.33, 0, 3.33, 0, 1);
            _refundUtilities.AfterRefundInstallmentValidation(jResponseFullPlanInfoAfter, secondInstallment);

            // Installment 3
            var thirdInstallment = new RefundAssertionsObject(planCreateResponse.InstallmentPlanNumber, "WaitingForProcessDate",
                false, 3, 0, 3.34, 0, 2);
            _refundUtilities.AfterRefundInstallmentValidation(jResponseFullPlanInfoAfter, thirdInstallment);

            var typeCounter = await _getPgtl.CountTypeValuesInPgtlAsync(
                Environment.GetEnvironmentVariable("StoreProcedureUrl")!, _requestHeader!,planCreateResponse.InstallmentPlanNumber
                , "Type", "Refund", false, "Result", "False");

            Assert.That(2 == typeCounter!);

            Console.WriteLine(
                "Done with TestValidate_Failure_OnFullAmountBothRefundActionAndVoidActionFailedForChargedInstallments_ReturnRefundPartiallyFailedError");
        }
        catch (Exception exception)
        {
            Assert.Fail(
                "Error in TestValidate_Failure_OnFullAmountBothRefundActionAndVoidActionFailedForChargedInstallments_ReturnRefundPartiallyFailedError" +
                exception);
        }
    }

    [Category("AsyncOperations")]
    [Test(Description = "Failure_WithRefund_OnRefundPeriodPassed_WhenRefundPeriodPastForAnyChargedInstallmentPutStatusAsDelayed"), CancelAfter(80 * 1000)]
    public async Task Failure_WithRefund_OnRefundPeriodPassed_WhenRefundPeriodPastForAnyChargedInstallmentPutStatusAsDelayed()
    {
        try
        {
            Console.WriteLine("Starting with Failure_WithRefund_OnRefundPeriodPassed_WhenRefundPeriodPastForAnyChargedInstallmentPutStatusAsDelayed");
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");

            var planCreateResponse = await _installmentPlans.CreatePlanAsync(
                Environment.GetEnvironmentVariable("ApiV3")!, _requestHeader!, "Active", 3, 
                Environment.GetEnvironmentVariable("SplititMockTerminal")!,createPlanDefaultValues);

            var jResponseChargeNext = await _chargeFunctionality.SendPostRequestChargeFunctionalityAsync( 
                _requestHeader!,planCreateResponse.InstallmentPlanNumber);
            Assert.That(jResponseChargeNext.ResponseHeader.Succeeded);

            var jResponseUpdateProcess = await _updateInstallmentProcessDateTimeFunctionality.SendPostRequestUpdateInstallmentProcessDateTimeAsync( 
                _requestHeader!, int.Parse(jResponseChargeNext.InstallmentPlan.Installments[1].InstallmentId), DateTime.Now.AddDays(-400));
            Assert.That(jResponseUpdateProcess.Equals(""));

            var jResponseCancel = await _installmentPlanNumberCancel.SendCancelPlanRequestAsync(
                _requestHeader!,planCreateResponse.InstallmentPlanNumber, 0, "OnlyIfAFullRefundIsPossible");
            Assert.That(jResponseCancel.Succeeded, Is.False);

            await Task.Delay(5 * 1000);

            var jResponseFullPlanInfoAfter = await _fullPlanInfoIpn.SendGetFullPlanInfoIpnAsync( 
                _requestHeader!,planCreateResponse.InstallmentPlanNumber);

            Assert.That(jResponseFullPlanInfoAfter.InstallmentPlan.InstallmentPlanStatus.Code.Equals("InProgress"));

            Console.WriteLine("Done with Failure_WithRefund_OnRefundPeriodPassed_WhenRefundPeriodPastForAnyChargedInstallmentPutStatusAsDelayed");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in Failure_WithRefund_OnRefundPeriodPassed_WhenRefundPeriodPastForAnyChargedInstallmentPutStatusAsDelayed" + exception);
        }
    }

    [Category("AsyncOperations")]
    [Test(Description = "Success_CancelPlanWithFullRefundStrategy"), CancelAfter(80 * 1000)]
    public async Task Success_CancelPlanWithFullRefundStrategy()
    {
        try
        {
            Console.WriteLine("Starting with Success_CancelPlanWithFullRefundStrategy");
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");

            createPlanDefaultValues.planData.totalAmount = 60.10;
            var planCreateResponse = await _installmentPlans.CreatePlanAsync(
                Environment.GetEnvironmentVariable("ApiV3")!, _requestHeader!, "Active", 3, 
                Environment.GetEnvironmentVariable("SplititMockTerminal")!, createPlanDefaultValues);

            var jResponseChargeNext = await _chargeFunctionality.SendPostRequestChargeFunctionalityAsync(_requestHeader!, planCreateResponse.InstallmentPlanNumber);
            Assert.That(jResponseChargeNext.ResponseHeader.Succeeded);

            var jResponseCancel = await _installmentPlanNumberCancel.SendCancelPlanRequestAsync(
                _requestHeader!, planCreateResponse.InstallmentPlanNumber, 0, "OnlyIfAFullRefundIsPossible");
            Assert.That(jResponseCancel.ResponseHeader.Succeeded);

            await Task.Delay(5 * 1000);

            var jResponseFullPlanInfoAfter = await _fullPlanInfoIpn.SendGetFullPlanInfoIpnAsync(_requestHeader!, planCreateResponse.InstallmentPlanNumber);
            Assert.That(jResponseFullPlanInfoAfter.ResponseHeader.Succeeded);

            // Plan
            var plan = new RefundAssertionsObject(planCreateResponse.InstallmentPlanNumber, "Cleared",
                false, 40.06, 40.06, 60.1, 0, 0);
            _refundUtilities.AfterRefundPlanValidation(jResponseFullPlanInfoAfter, plan);

            // Installment 1
            var firstInstallment = new RefundAssertionsObject(planCreateResponse.InstallmentPlanNumber, "Finished",
                true, 20.03, 20.03, 20.03, 0, 0);
            _refundUtilities.AfterRefundInstallmentValidation(jResponseFullPlanInfoAfter, firstInstallment);

            //// Installment 2
            var secondInstallment = new RefundAssertionsObject(planCreateResponse.InstallmentPlanNumber, "Finished",
                true, 20.03, 20.03, 20.03, 0, 1);
            _refundUtilities.AfterRefundInstallmentValidation(jResponseFullPlanInfoAfter, secondInstallment);

            //// Installment 3
            var thirdInstallment = new RefundAssertionsObject(planCreateResponse.InstallmentPlanNumber, "Canceled",
                false, 0, 0, 20.04, 0, 2);
            _refundUtilities.AfterRefundInstallmentValidation(jResponseFullPlanInfoAfter, thirdInstallment);

            //Plan auditLog
            var responsePgtl = await _getPgtl.ValidatePgtlKeyValueInnerAsync(
                Environment.GetEnvironmentVariable("StoreProcedureUrl")!, _requestHeader!, planCreateResponse.InstallmentPlanNumber, "Type", "Void", "ResultMessageMessageText");
            Assert.That(responsePgtl!.Contains("Void Succeeded"));

            Console.WriteLine("Done with Success_CancelPlanWithFullRefundStrategy");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in Success_CancelPlanWithFullRefundStrategy" + exception);
        }
    }
    
    [Category("AsyncOperations")]
    [Test(Description = "Success_CancelPlanWithDeductionRefundStrategy"), CancelAfter(80 * 1000)]
    public async Task Success_CancelPlanWithDeductionRefundStrategy()
    {
        try
        {
            Console.WriteLine("Starting with Success_CancelPlanWithDeductionRefundStrategy");
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");

            createPlanDefaultValues.planData.totalAmount = 60.10;
            var planCreateResponse = await _installmentPlans.CreatePlanAsync(
                Environment.GetEnvironmentVariable("ApiV3")!, _requestHeader!, "Active", 3, 
                Environment.GetEnvironmentVariable("SplititMockTerminal")!, createPlanDefaultValues);

            var jResponseChargeNext = await _chargeFunctionality.SendPostRequestChargeFunctionalityAsync(_requestHeader!, planCreateResponse.InstallmentPlanNumber);
            Assert.That(jResponseChargeNext.ResponseHeader.Succeeded);

            var jResponseCancel = await _installmentPlanNumberCancel.SendCancelPlanRequestAsync(
                _requestHeader!, planCreateResponse.InstallmentPlanNumber, 0, "NoRefunds");
            Assert.That(jResponseCancel.ResponseHeader.Succeeded);

            await Task.Delay(5 * 1000);

            var jResponseFullPlanInfoAfter = await _fullPlanInfoIpn.SendGetFullPlanInfoIpnAsync(_requestHeader!, planCreateResponse.InstallmentPlanNumber);
            Assert.That(jResponseFullPlanInfoAfter.ResponseHeader.Succeeded);

            // Plan
            var plan = new RefundAssertionsObject(planCreateResponse.InstallmentPlanNumber, "Canceled",
                false, 40.06, 0, 60.1, 0, 0);
            _refundUtilities.AfterRefundPlanValidation(jResponseFullPlanInfoAfter, plan);

            // Installment 1
            var firstInstallment = new RefundAssertionsObject(planCreateResponse.InstallmentPlanNumber, "Finished",
                false, 20.03, 0, 20.03, 0, 0);
            _refundUtilities.AfterRefundInstallmentValidation(jResponseFullPlanInfoAfter, firstInstallment);

            //// Installment 2
            var secondInstallment = new RefundAssertionsObject(planCreateResponse.InstallmentPlanNumber, "Finished",
                false, 20.03, 0, 20.03, 0, 1);
            _refundUtilities.AfterRefundInstallmentValidation(jResponseFullPlanInfoAfter, secondInstallment);

            //// Installment 3
            var thirdInstallment = new RefundAssertionsObject(planCreateResponse.InstallmentPlanNumber, "Canceled",
                false, 0, 0, 20.04, 0, 2);
            _refundUtilities.AfterRefundInstallmentValidation(jResponseFullPlanInfoAfter, thirdInstallment);

            //Plan auditLog
            var responsePgtl = await _getPgtl.ValidatePgtlKeyValueInnerAsync(
                Environment.GetEnvironmentVariable("StoreProcedureUrl")!, _requestHeader!, planCreateResponse.InstallmentPlanNumber, "Type", "Void", "ResultMessageMessageText");
            Assert.That(responsePgtl!.Contains("Void Succeeded"));

            Console.WriteLine("Done with Success_CancelPlanWithDeductionRefundStrategy");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in Success_CancelPlanWithDeductionRefundStrategy" + exception);
        }
    }

    [Category("AsyncOperations")]
    [Test(Description = "TestValidate_Success_DoRefundInParallel"), CancelAfter(80 * 1000)]
    public async Task TestValidate_Success_DoRefundInParallel()
    {
        try
        {
            Console.WriteLine("Starting TestValidate_Success_DoRefundInParallel");
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");

            createPlanDefaultValues.planData.totalAmount = 100;
            var planCreateResponse = await _installmentPlans.CreatePlanAsync(
                Environment.GetEnvironmentVariable("ApiV3")!, _requestHeader!,"Active",4, 
                Environment.GetEnvironmentVariable("SplititMockTerminal")!, createPlanDefaultValues);

            var jResponseChargeNext = await _chargeFunctionality.SendPostRequestChargeFunctionalityAsync(_requestHeader!,planCreateResponse.InstallmentPlanNumber);
            Assert.That(jResponseChargeNext.ResponseHeader.Succeeded);

            List<Task<ResponseRefund.Root>> refunds = new List<Task<ResponseRefund.Root>>();

            for(var i = 0; i < 10; i++) 
            {
                refunds.Add(_refundFunctionality.SendRefundRequestAsync( 
                    _requestHeader!, planCreateResponse.InstallmentPlanNumber, "AmountToRefund", null!, null!, 10));
            }

            await refunds.WhenAll();

            var jResponseFullPlanInfoAfter = await _fullPlanInfoIpn.SendGetFullPlanInfoIpnAsync(_requestHeader!,planCreateResponse.InstallmentPlanNumber);

            // Plan
            var plan = new RefundAssertionsObject(planCreateResponse.InstallmentPlanNumber, "Cleared",
                true, 50, 50, 100, 0, 0);
            _refundUtilities.AfterRefundPlanValidation(jResponseFullPlanInfoAfter, plan);

            // Installment 1
            var firstInstallment = new RefundAssertionsObject(planCreateResponse.InstallmentPlanNumber, "Finished",
                true, 25, 25, 25, 0, 0);
            _refundUtilities.AfterRefundInstallmentValidation(jResponseFullPlanInfoAfter, firstInstallment);

            // Installment 2
            var secondInstallment = new RefundAssertionsObject(planCreateResponse.InstallmentPlanNumber, "Finished",
                true, 25, 25, 25, 0, 1);
            _refundUtilities.AfterRefundInstallmentValidation(jResponseFullPlanInfoAfter, secondInstallment);

            // Installment 3
            var thirdInstallment = new RefundAssertionsObject(planCreateResponse.InstallmentPlanNumber, "Canceled",
                false, 0, 0, 25, 0, 2);
            _refundUtilities.AfterRefundInstallmentValidation(jResponseFullPlanInfoAfter, thirdInstallment);

            // Installment 4
            var fourthInstallment = new RefundAssertionsObject(planCreateResponse.InstallmentPlanNumber, "Canceled",
                false, 0, 0, 25, 0, 3);
            _refundUtilities.AfterRefundInstallmentValidation(jResponseFullPlanInfoAfter, fourthInstallment);

            Console.WriteLine("Done with TestValidate_Success_DoRefundInParallel");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidate_Success_DoRefundInParallel" + exception);
        }
    }
}