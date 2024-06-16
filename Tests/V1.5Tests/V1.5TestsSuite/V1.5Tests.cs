using Allure.NUnit;
using Allure.NUnit.Attributes;
using NUnit.Framework;
using Splitit.Automation.NG.Backend.BaseActions;
using Splitit.Automation.NG.Backend.Services.V1._5.DefaultValues;
using Splitit.Automation.NG.Backend.Services.V1._5.InstallmentPlan.InstallmentPlanApiEndPoints;
using Splitit.Automation.NG.Backend.Services.V1._5.InstallmentPlan.InstallmentPlanBaseObjects;
using Splitit.Automation.NG.Backend.Services.V1._5.Login.LoginApiEndPoints;
using Splitit.Automation.NG.Backend.Services.V3.DefaultValues;
using Splitit.Automation.NG.Backend.Tests.TestsSetups;
using Cancel = Splitit.Automation.NG.Backend.Services.V1._5.InstallmentPlan.InstallmentPlanApiEndPoints.Cancel;
using Create = Splitit.Automation.NG.Backend.Services.V1._5.InstallmentPlan.InstallmentPlanApiEndPoints.Create;
using Refund = Splitit.Automation.NG.Backend.Services.V1._5.InstallmentPlan.InstallmentPlanApiEndPoints.Refund;

namespace Splitit.Automation.NG.Backend.Tests.V1._5Tests.V1._5TestsSuite;

[TestFixture]
[AllureNUnit]
[AllureSuite("V1.5Tests")]
[AllureDisplayIgnored]
public class V1Point5Tests
{
    private RequestHeader? _requestHeader;
    private RequestHeader? _requestHeaderMerchant;
    private readonly Create _create;
    private readonly Login _login;
    private readonly CreatePlanV1Point5DefaultValues _createPlanV1Point5DefaultValues;
    private readonly Refund _refund;
    private readonly UpdatePaymentData _updatePaymentData;
    private readonly Cancel _cancel;
    private readonly AuditLogController _auditLogController;


    public V1Point5Tests()
    {
        Console.WriteLine("Staring V1Point5Tests setup");
        _create = new Create();
        _login = new Login();
        _createPlanV1Point5DefaultValues = new CreatePlanV1Point5DefaultValues();
        _refund = new Refund();
        _updatePaymentData = new UpdatePaymentData();
        _cancel = new Cancel();
        _auditLogController = new AuditLogController();
        Console.WriteLine("Done with V1Point5Tests setup");
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
        _requestHeaderMerchant = await _login.LoginAsMerchantAsync(Environment.GetEnvironmentVariable("GoogleBaseUri")!,
            new RequestHeader(), Environment.GetEnvironmentVariable("GoogleMock")!, 
            Environment.GetEnvironmentVariable("UserGoogleAutomationTestUserName")!,
            Environment.GetEnvironmentVariable("UserGoogleAutomationTestPassword")!);
    }
    
    [Category("V1.5Tests")]
    [Test(Description = "TestValidateGoogleApiLoginAsMerchantCreateNewPlan"), CancelAfter(80*1000)]
    public async Task TestValidateGoogleApiLoginAsMerchantCreateNewPlan()
    {
        try
        {
            Console.WriteLine("Starting TestValidateGoogleApiLoginAsMerchantCreateNewPlan");
            _createPlanV1Point5DefaultValues.planData.amount.currencyCode = "JPY";
            _createPlanV1Point5DefaultValues.planData.amount.value = new Random().Next(10, 100);
            var planCreateResponse = await _create.CreatePlanV1Point5Async( _requestHeaderMerchant!,
                new Random().Next(2, 6), _createPlanV1Point5DefaultValues);
            Assert.That(planCreateResponse.InstallmentPlan.InstallmentPlanStatus.Code, Is.EqualTo("InProgress"));
            Console.WriteLine("Done with TestValidateGoogleApiLoginAsMerchantCreateNewPlan");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidateGoogleApiLoginAsMerchantCreateNewPlan \n" + exception + "\n");
        }
    }
    
    [Category("V1.5Tests")]
    [Test(Description = "TestValidateGoogleApiLoginAsAdminCreateNewPlan"), CancelAfter(80*1000)]
    public async Task TestValidateGoogleApiLoginAsAdminCreateNewPlan()
    {
        try
        {
            Console.WriteLine("Starting TestValidateGoogleApiLoginAsAdminCreateNewPlan");
            _createPlanV1Point5DefaultValues.planData.amount.currencyCode = "JPY";
            _createPlanV1Point5DefaultValues.planData.amount.value = new Random().Next(10, 100);
            var planCreateResponse = await _create.CreatePlanV1Point5Async( _requestHeader!,
                new Random().Next(2, 6), _createPlanV1Point5DefaultValues);
            Assert.That(planCreateResponse.InstallmentPlan.InstallmentPlanStatus.Code, Is.EqualTo("InProgress"));
            Console.WriteLine("Done with TestValidateGoogleApiLoginAsAdminCreateNewPlan");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidateGoogleApiLoginAsMerchantCreateNewPlan \n" + exception + "\n");
        }
    }
    
    [Category("V1.5Tests")]
    [Test(Description = "TestValidateGoogleApiCreateResponseValidateConsumerData"), CancelAfter(80*1000)]
    public async Task TestValidateGoogleApiCreateResponseValidateConsumerData()
    {
        try
        {
            Console.WriteLine("Starting TestValidateGoogleApiCreateResponseValidateConsumerData");
            _createPlanV1Point5DefaultValues.planData.amount.currencyCode = "JPY";
            _createPlanV1Point5DefaultValues.planData.amount.value = new Random().Next(10, 100);
            var planCreateResponse = await _create.CreatePlanV1Point5Async( _requestHeader!,
                new Random().Next(2, 6), _createPlanV1Point5DefaultValues);
            Assert.That(planCreateResponse.InstallmentPlan.InstallmentPlanStatus.Code, Is.EqualTo("InProgress"));
            Assert.That(planCreateResponse.InstallmentPlan.Consumer.Email.Equals(_createPlanV1Point5DefaultValues.ConsumerData.email));
            Assert.That(planCreateResponse.InstallmentPlan.Consumer.FullName.Equals(_createPlanV1Point5DefaultValues.ConsumerData.fullName));
            Console.WriteLine("Done with TestValidateGoogleApiCreateResponseValidateConsumerData");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidateGoogleApiCreateResponseValidateConsumerData \n" + exception + "\n");
        }
    }
    
    [Category("V1.5Tests")]
    [Test(Description = "TestValidateGoogleApiCreateResponseValidateBillingAddress"), CancelAfter(80*1000)]
    public async Task TestValidateGoogleApiCreateResponseValidateBillingAddress()
    {
        try
        {
            Console.WriteLine("Starting TestValidateGoogleApiCreateResponseValidateBillingAddress");
            _createPlanV1Point5DefaultValues.planData.amount.currencyCode = "JPY";
            _createPlanV1Point5DefaultValues.planData.amount.value = new Random().Next(10, 100);
            var planCreateResponse = await _create.CreatePlanV1Point5Async( _requestHeader!,
                new Random().Next(2, 6), _createPlanV1Point5DefaultValues);
            Assert.That(planCreateResponse.InstallmentPlan.InstallmentPlanStatus.Code, Is.EqualTo("InProgress"));
            Assert.That(planCreateResponse.InstallmentPlan.ActiveCard.Address.AddressLine.Equals(_createPlanV1Point5DefaultValues.BillingAddress.addressLine));
            Assert.That(planCreateResponse.InstallmentPlan.ActiveCard.Address.AddressLine2.Equals(_createPlanV1Point5DefaultValues.BillingAddress.addressLine2));
            Assert.That(planCreateResponse.InstallmentPlan.ActiveCard.Address.City.Equals(_createPlanV1Point5DefaultValues.BillingAddress.city));
            Assert.That(planCreateResponse.InstallmentPlan.ActiveCard.Address.Country.Equals(_createPlanV1Point5DefaultValues.BillingAddress.country));
            Assert.That(planCreateResponse.InstallmentPlan.ActiveCard.Address.Zip.Equals(_createPlanV1Point5DefaultValues.BillingAddress.zip));
            Console.WriteLine("Done with TestValidateGoogleApiCreateResponseValidateBillingAddress");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidateGoogleApiCreateResponseValidateBillingAddress \n" + exception + "\n");
        }
    }
    
    [Category("V1.5Tests")]
    [Test(Description = "TestValidateGoogleApiCreateResponseValidatePlanData"), CancelAfter(80*1000)]
    public async Task TestValidateGoogleApiCreateResponseValidatePlanData()
    {
        try
        {
            Console.WriteLine("Starting TestValidateGoogleApiCreateResponseValidatePlanData");
            _createPlanV1Point5DefaultValues.planData.amount.currencyCode = "JPY";
            _createPlanV1Point5DefaultValues.planData.amount.value = new Random().Next(10, 100);
            var planCreateResponse = await _create.CreatePlanV1Point5Async( _requestHeader!,
                new Random().Next(2, 6), _createPlanV1Point5DefaultValues);
            Assert.That(planCreateResponse.InstallmentPlan.InstallmentPlanStatus.Code, Is.EqualTo("InProgress"));
            Assert.That(planCreateResponse.InstallmentPlan.Amount.Value.Equals(Convert.ToDecimal(_createPlanV1Point5DefaultValues.planData.amount.value)));
            Assert.That(planCreateResponse.InstallmentPlan.NumberOfInstallments.Equals(_createPlanV1Point5DefaultValues.planData.numberOfInstallments));
            Assert.That(planCreateResponse.InstallmentPlan.Amount.Currency.Code.Equals(_createPlanV1Point5DefaultValues.planData.amount.currencyCode));
            Console.WriteLine("Done with TestValidateGoogleApiCreateResponseValidatePlanData");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidateGoogleApiCreateResponseValidatePlanData \n" + exception + "\n");
        }
    }
    
    [Category("V1.5Tests")]
    [Test(Description = "TestValidateGoogleApiCreateResponseValidateGatewayTransactionResults"), CancelAfter(80*1000)]
    public async Task TestValidateGoogleApiCreateResponseValidateGatewayTransactionResults()
    {
        try
        {
            Console.WriteLine("Starting TestValidateGoogleApiCreateResponseValidateGatewayTransactionResults");
            _createPlanV1Point5DefaultValues.planData.amount.currencyCode = "JPY";
            _createPlanV1Point5DefaultValues.planData.amount.value = new Random().Next(10, 100);
            var planCreateResponse = await _create.CreatePlanV1Point5Async( _requestHeader!,
                new Random().Next(2, 6), _createPlanV1Point5DefaultValues);
            Assert.That(planCreateResponse.InstallmentPlan.InstallmentPlanStatus.Code, Is.EqualTo("InProgress"));
            Assert.That(planCreateResponse.GatewayTransactionResults[0].GatewayTransactionId, Is.Not.Null);
            Assert.That(planCreateResponse.GatewayTransactionResults[0].OperationType.Code.Equals("Authorize"));
            Assert.That(planCreateResponse.GatewayTransactionResults[1].OperationType.Code.Equals("Authorize"));
            Assert.That(planCreateResponse.GatewayTransactionResults[2].OperationType.Code.Equals("Capture"));
            Console.WriteLine("Done with TestValidateGoogleApiCreateResponseValidateGatewayTransactionResults");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidateGoogleApiCreateResponseValidateGatewayTransactionResults \n" + exception + "\n");
        }
    }
    
    [Category("V1.5Tests")]
    [Test(Description = "TestValidateGoogleApiRefundResponseValidateStrategyOne"), CancelAfter(80*1000)]
    public async Task TestValidateGoogleApiRefundResponseValidateStrategyOne()
    {
        try
        {
            Console.WriteLine("Starting TestValidateGoogleApiRefundResponseValidateStrategyOne");
            _createPlanV1Point5DefaultValues.planData.amount.currencyCode = "JPY";
            _createPlanV1Point5DefaultValues.planData.amount.value = 80;
            var planCreateResponse = await _create.CreatePlanV1Point5Async( _requestHeader!,
                new Random().Next(2, 6), _createPlanV1Point5DefaultValues);
            Assert.That(planCreateResponse.InstallmentPlan.InstallmentPlanStatus.Code, Is.EqualTo("InProgress"));
            var jResponseRefund = await _refund.SendRefundRequestAsync( _requestHeader!, 10, 1, planCreateResponse.InstallmentPlan.InstallmentPlanNumber);
            Console.WriteLine("Validating jResponse First refund");
            Assert.That(jResponseRefund.CurrentRefundAmount.Value == 10);
            Assert.That(jResponseRefund.InstallmentPlan.RefundAmount.Value == 0);
            var jResponseAuditLog = await _auditLogController.SendRetrieveAuditLogRequestAsync(
                _requestHeader!, planCreateResponse.InstallmentPlan.InstallmentPlanNumber);
            Assert.That(_auditLogController.ValidateAuditLogLogs( 
                new[] {"Create", "RefundReceived", "RefundCompleted"}, jResponseAuditLog!));
            Console.WriteLine("Done with Validating jResponse First refund");
            var jResponseRefund1 = await _refund.SendRefundRequestAsync( _requestHeader!, 10, 1, 
                planCreateResponse.InstallmentPlan.InstallmentPlanNumber);
            Console.WriteLine("Validating jResponse Second refund");
            Assert.That(jResponseRefund1.CurrentRefundAmount.Value == 10);
            await _auditLogController.SendRetrieveAuditLogRequestAsync(
                _requestHeader!, planCreateResponse.InstallmentPlan.InstallmentPlanNumber);
            Assert.That(_auditLogController.ValidateAuditLogLogs(
                new[] {"Create", "RefundReceived", "RefundCompleted"}, jResponseAuditLog!));
            Console.WriteLine("Done with Validating jResponse Second refund");
            Console.WriteLine("Done with TestValidateGoogleApiRefundResponseValidateStrategyOne");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidateGoogleApiRefundResponseValidateStrategyOne \n" + exception + "\n");
        }
    }
    
    [Category("V1.5Tests")]
    [Test(Description = "TestValidateGoogleApiRefundResponseValidateStrategyTwo"), CancelAfter(80*1000)]
    public async Task TestValidateGoogleApiRefundResponseValidateStrategyTwo()
    {
        try
        {
            Console.WriteLine("Starting TestValidateGoogleApiRefundResponseValidateStrategyTwo");
            _createPlanV1Point5DefaultValues.planData.amount.currencyCode = "JPY";
            _createPlanV1Point5DefaultValues.planData.amount.value = 70;
            var planCreateResponse = await _create.CreatePlanV1Point5Async( _requestHeader!,
                new Random().Next(2, 6), _createPlanV1Point5DefaultValues);
            Assert.That(planCreateResponse.InstallmentPlan.InstallmentPlanStatus.Code, Is.EqualTo("InProgress"));
            var jResponseRefund = await _refund.SendRefundRequestAsync( _requestHeader!, 10, 1, 
                planCreateResponse.InstallmentPlan.InstallmentPlanNumber);
            Assert.That(jResponseRefund.CurrentRefundAmount.Value == 10);
            Assert.That(jResponseRefund.InstallmentPlan.RefundAmount.Value == 0);
            var jResponseAuditLog = await _auditLogController.SendRetrieveAuditLogRequestAsync(
                _requestHeader!, planCreateResponse.InstallmentPlan.InstallmentPlanNumber);
            Assert.That(_auditLogController.ValidateAuditLogLogs( 
                new[] {"Create", "RefundReceived", "RefundCompleted"}, jResponseAuditLog!));
            var jResponseRefund1 = await _refund.SendRefundRequestAsync( _requestHeader!, 10, 1, 
                planCreateResponse.InstallmentPlan.InstallmentPlanNumber);
            Assert.That(jResponseRefund1.CurrentRefundAmount.Value == 10);
            Assert.That(jResponseRefund1.InstallmentPlan.RefundAmount.Value == 0);
            var jResponseAuditLog1 = await _auditLogController.SendRetrieveAuditLogRequestAsync(
                _requestHeader!, planCreateResponse.InstallmentPlan.InstallmentPlanNumber);
            Assert.That(_auditLogController.ValidateAuditLogLogs(
                new[] {"Create", "RefundReceived", "RefundCompleted"}, jResponseAuditLog1!));
            Console.WriteLine("Done with TestValidateGoogleApiRefundResponseValidateStrategyTwo");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidateGoogleApiRefundResponseValidateStrategyTwo \n" + exception + "\n");
        }
    }
    
    [Category("V1.5Tests")]
    [Test(Description = "TestValidateGoogleApiUseIdempotencyKeyUseSameCreateCall"), CancelAfter(80*1000)]
    public async Task TestValidateGoogleApiUseIdempotencyKeyUseSameCreateCall()
    {
        try
        {
            Console.WriteLine("Starting TestValidateGoogleApiUseIdempotencyKeyUseSameCreateCall");
            _createPlanV1Point5DefaultValues.planData.amount.currencyCode = "JPY";
            _createPlanV1Point5DefaultValues.planData.amount.value = new Random().Next(10, 100);
            var now = DateTime.Now;
            const string customFormat = "MM/dd/yy HH:mm:ss";
            var formattedDateTime = now.ToString(customFormat);
            var planCreateResponse = await _create.CreatePlanV1Point5Async( _requestHeader!,
                5, _createPlanV1Point5DefaultValues, "yes", formattedDateTime);
            Assert.That(planCreateResponse.InstallmentPlan.InstallmentPlanStatus.Code, Is.EqualTo("InProgress"));
            var ipn = planCreateResponse.InstallmentPlan.InstallmentPlanNumber;
            Assert.That(ipn, Is.Not.Null);
            await Task.Delay(1*1000);
            _createPlanV1Point5DefaultValues.planData.amount.value = Decimal.ToDouble(planCreateResponse.InstallmentPlan.Amount.Value);
            _createPlanV1Point5DefaultValues.planData.amount.currencyCode = "JPY";
            var json1 = await _create.CreatePlanV1Point5Async( _requestHeader!,
                5, _createPlanV1Point5DefaultValues, "yes", formattedDateTime);
            Assert.That(json1.InstallmentPlan, Is.Not.Null);
            Assert.That(planCreateResponse.InstallmentPlan.InstallmentPlanNumber.Equals(json1.InstallmentPlan.InstallmentPlanNumber));
            Console.WriteLine("Done with TestValidateGoogleApiUseIdempotencyKeyUseSameCreateCall");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidateGoogleApiUseIdempotencyKeyUseSameCreateCall \n" + exception + "\n");
        }
    }
    
    [Category("V1.5Tests")]
    [Test(Description = "TestValidateGoogleApiUseIdempotencyKeyUseDifferentCreateCall"), CancelAfter(80*1000)]
    public async Task TestValidateGoogleApiUseIdempotencyKeyUseDifferentCreateCall()
    {
        try
        {
            Console.WriteLine("Starting TestValidateGoogleApiUseIdempotencyKeyUseDifferentCreateCall");
            _createPlanV1Point5DefaultValues.planData.amount.currencyCode = "JPY";
            _createPlanV1Point5DefaultValues.planData.amount.value = new Random().Next(10, 100);
            var now = DateTime.Now;
            const string customFormat = "MM/dd/yy HH:mm:ss";
            var formattedDateTime = now.ToString(customFormat);
            var planCreateResponse = await _create.CreatePlanV1Point5Async( _requestHeader!,
                new Random().Next(2, 6), _createPlanV1Point5DefaultValues, "yes", formattedDateTime);
            Assert.That(planCreateResponse.InstallmentPlan.InstallmentPlanStatus.Code, Is.EqualTo("InProgress"));
            var ipn = planCreateResponse.InstallmentPlan.InstallmentPlanNumber;
            await Task.Delay(5*1000);
            var json1 = await _create.CreatePlanV1Point5Async( _requestHeader!,
                new Random().Next(2, 6), _createPlanV1Point5DefaultValues, "yes", formattedDateTime);
            Assert.That(ipn, Is.Not.Null);
            Assert.That(json1.InstallmentPlan, Is.Null);
            Console.WriteLine("Done with TestValidateGoogleApiUseIdempotencyKeyUseDifferentCreateCall");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidateGoogleApiUseIdempotencyKeyUseDifferentCreateCall \n" + exception + "\n");
        }
    }
    
    [Category("V1.5Tests")]
    [Test(Description = "TestValidateGoogleApiValidateUpdatePaymentData"), CancelAfter(80*1000)]
    public async Task TestValidateGoogleApiValidateUpdatePaymentData()
    {
        try
        {
            Console.WriteLine("Starting TestValidateGoogleApiValidateUpdatePaymentData");
            _createPlanV1Point5DefaultValues.planData.amount.currencyCode = "JPY";
            _createPlanV1Point5DefaultValues.planData.amount.value = new Random().Next(10, 100);
            var planCreateResponse = await _create.CreatePlanV1Point5Async( _requestHeaderMerchant!,
                new Random().Next(2, 6), _createPlanV1Point5DefaultValues);
            Assert.That(planCreateResponse.InstallmentPlan.InstallmentPlanStatus.Code, Is.EqualTo("InProgress"));
            var updatePaymentDataBaseObjects = new UpdatePaymentDataBaseObjects.Root();
            var creditCard = new UpdatePaymentDataBaseObjects.CreditCardDetails
            {
                cardNumber = "545454545454545454",
                cardExpMonth = "04",
                cardExpYear = "40",
                cardCvv = "747"
            };
            updatePaymentDataBaseObjects.creditCardDetails = creditCard;
            updatePaymentDataBaseObjects.installmentPlanNumber = planCreateResponse.InstallmentPlan.InstallmentPlanNumber;
            var jResponseUpdateCard = await _updatePaymentData.SendUpdatePaymentDataRequestAsync( _requestHeader!, updatePaymentDataBaseObjects);
            Assert.That(jResponseUpdateCard.ResponseHeader.Succeeded);
            Assert.That(jResponseUpdateCard.InstallmentPlan.ActiveCard.CardNumber == "**************5454");
            Console.WriteLine("Done with TestValidateGoogleApiValidateUpdatePaymentData");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidateGoogleApiValidateUpdatePaymentData \n" + exception + "\n");
        }
    }
    
    [Category("V1.5Tests")]
    [Test(Description = "TestValidateGoogleApiValidateCancelInstallmentPlan"), CancelAfter(80*1000)]
    public async Task TestValidateGoogleApiValidateCancelInstallmentPlan()
    {
        try
        {
            Console.WriteLine("Starting TestValidateGoogleApiValidateCancelInstallmentPlan");
            _createPlanV1Point5DefaultValues.planData.amount.currencyCode = "JPY";
            _createPlanV1Point5DefaultValues.planData.amount.value = new Random().Next(10, 100);
            var planCreateResponse = await _create.CreatePlanV1Point5Async( _requestHeaderMerchant!,
                new Random().Next(2, 6), _createPlanV1Point5DefaultValues);
            Assert.That(planCreateResponse.InstallmentPlan.InstallmentPlanStatus.Code, Is.EqualTo("InProgress"));
            var cancelBaseObjects = new CancelBaseObjects.Root
            {
                installmentPlanNumber = planCreateResponse.InstallmentPlan.InstallmentPlanNumber,
                cancelationReason = 0,
                refundUnderCancelation = 1
            };
            var jResponse = await _cancel.SendCancelRequestAsync( _requestHeader!, cancelBaseObjects);
            Assert.That(jResponse.ResponseHeader.Succeeded);
            Console.WriteLine("Done with TestValidateGoogleApiValidateCancelInstallmentPlan");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidateGoogleApiValidateCancelInstallmentPlan \n" + exception + "\n");
        }
    }
}

