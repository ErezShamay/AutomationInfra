using Newtonsoft.Json;
using NUnit.Framework;
using Splitit.Automation.NG.Backend.BaseActions;
using Splitit.Automation.NG.Backend.Services.AdminApi.InstallmentPlan.InstallmentPlanResponses;
using Splitit.Automation.NG.Backend.Services.AdminApi.InstallmentPlan.InstallmentsPlanFunctionality;
using Splitit.Automation.NG.Backend.Services.V1.InstallmentPlan.InstallmentPlanApiEndPoints;
using Splitit.Automation.NG.Backend.Services.V3.DefaultValues;
using Splitit.Automation.NG.Backend.Services.V3.InstallmentPlan.InstallmentPlanResponses;
using Splitit.Automation.NG.Utilities.EnvironmentsConfig;
using GetExtended = Splitit.Automation.NG.Backend.Services.AdminApi.InstallmentPlan.InstallmentPlanBaseObjects.GetExtended;

namespace Splitit.Automation.NG.Backend.Services.V3.InstallmentPlan.InstallmentPlanApiEndPoint;

public class InstallmentPlans
{
    private readonly HttpSender _httpSender = new();
    private readonly StatusToFlagInit _statusToFlagInit = new();
    private const string CreatePlanEndpoint = "/api/installmentplans";
    private const string CreatePlanInitiateEndpoint = "/api/installmentplans/initiate";
    private const string SearchInstallmentPlan = "/api/installmentplans/search";
    private const string VerifyAuthorizationEndpoint = "/verifyauthorization";
    private const string ExtendedParamsEndPoint = "/api/installment-plan/get-extended";
    private readonly EnvConfig _envConfig = new();
    private readonly InstallmentPlanNumberCancel _installmentPlanNumberCancel = new();
    private readonly GetPgtl _getPgtl = new();

    public async Task<ResponseV3.ResponseRoot> CreatePlanAsync(string baseUrl, RequestHeader requestHeader,
        string status, int numberOfInstallments, string terminal, 
        CreatePlanDefaultValues createPlanDefaultValues, string isNegativeTest = null!, 
        string kesFlag = default!, string requestSignature = default!, 
        string requestSignatureKeyId = default!, string requestSignatureClientId = default!,
        string testModeFlag = default!, string testModeValue = default!, bool shouldCancel = true)
    {
        try
        {
            Console.WriteLine("\nStarting CreatePlan");
            createPlanDefaultValues.planData.terminalId = terminal;
            createPlanDefaultValues.autoCapture = true;
            createPlanDefaultValues.planData.numberOfInstallments = numberOfInstallments;
            string response;
            if (kesFlag != null!)
            {
                response = await _httpSender.SendPostHttpsRequestAsync(
                    baseUrl + CreatePlanEndpoint, createPlanDefaultValues, requestHeader,
                    kesFlag: kesFlag, requestSignature: requestSignature, requestSignatureKeyId: requestSignatureKeyId,
                    requestSignatureClientId: requestSignatureClientId);
            }
            else
            {
                response = await _httpSender.SendPostHttpsRequestAsync(
                    baseUrl + CreatePlanEndpoint, createPlanDefaultValues, requestHeader, 
                    testModeFlag:testModeFlag, testModeValue: testModeValue);
            }
            
            var jResponse = JsonConvert.DeserializeObject<ResponseV3.ResponseRoot>(response);
            if (isNegativeTest != null!){
                Console.WriteLine("\nThis is a negative test, validating Error object is not null");
                Assert.That(jResponse!.Error, Is.Not.Null);
                Console.WriteLine("\nError object is not null\n");
            }
            else
            {
                Assert.That(jResponse!.Error, Is.Null);
            }
            Console.WriteLine("CreatePlan Succeeded! IPN -> " + jResponse.InstallmentPlanNumber + " With Status -> " + jResponse.Status + "\n");
            if (shouldCancel)
            {
                Task.Delay(new Random().Next(180000, 210000)).ContinueWith(t =>
                {
                    _installmentPlanNumberCancel.SendCancelPlanRequestAsync(requestHeader, jResponse.InstallmentPlanNumber,
                        0, "NoRefunds");
                });
            }
            var pgtlCaptureId = await _getPgtl.ValidatePgtlKeyValueInnerAsync(Environment.GetEnvironmentVariable("StoreProcedureUrl")!,
                requestHeader, jResponse.InstallmentPlanNumber, "Type", "Capture","Id");
            if (pgtlCaptureId!.Equals("") || pgtlCaptureId == null!)
            {
                Console.WriteLine("pgtlCaptureId is null or empty");
            }
            return jResponse;
        }
        catch (Exception exception)
        {
            Console.WriteLine("Error in CreatePlan\nֿ" +exception+ "\n");
            throw;
        }
    }
    
    public async Task<ResponseV3Initiate.Root> CreatePlanInitiateAsync(string baseUrl, RequestHeader requestHeader,
        string status, int numberOfInstallments, string terminal,
        CreatePlanDefaultValues createPlanDefaultValues, bool shouldCancel = true)
    {
        try
        {
            Console.WriteLine("\nStarting CreatePlanInitiate");
            createPlanDefaultValues = _statusToFlagInit.StatusToFlagsInit(status, createPlanDefaultValues);
            createPlanDefaultValues.planData.terminalId = terminal;
            createPlanDefaultValues.planData.numberOfInstallments = numberOfInstallments;
            var response = await _httpSender.SendPostHttpsRequestAsync(baseUrl + CreatePlanInitiateEndpoint, createPlanDefaultValues, requestHeader);
            var jResponse = JsonConvert.DeserializeObject<ResponseV3Initiate.Root>(response);
            Console.WriteLine("CreatePlan Succeeded! IPN -> " + jResponse!.InstallmentPlanNumber + " With Status -> " + jResponse.Status + "\n");
            if (shouldCancel)
            {
                Task.Delay(new Random().Next(180000, 240000)).ContinueWith(t =>
                {
                    _installmentPlanNumberCancel.SendCancelPlanRequestAsync(requestHeader, jResponse.InstallmentPlanNumber,
                        0, "NoRefunds");
                });
            }
            return jResponse;
        }
        catch (Exception exception)
        {
            Console.WriteLine("Error in CreatePlan\n" + exception + "\n");
            throw;
        }
    }

    public bool ValidateInstallmentPlanCreation(ResponseV3.ResponseRoot response, string expectedStatus)
    {
        try
        {
            Console.WriteLine("\nStarting ValidateInstallmentPlanCreation");
            Console.WriteLine("Trying to Validate Create Plan With 3DS");
            Assert.That(response.InstallmentPlanNumber, Is.Not.Null);
            Console.WriteLine("IPN is not null");
            Assert.That(response.Status, Is.EqualTo(expectedStatus));
            Console.WriteLine("Plan status is as expected -> " + response.Status);
            Console.WriteLine("ValidateInstallmentPlanCreation succeeded\n");
            return true;
        }
        catch (Exception exception)
        {
            Console.WriteLine("Error in ValidateInstallmentPlanCreation\n" +exception+ "\n");
            return false;
        }
    }

    public async Task<ResponseV3Search.Root> SendSearchInstallmentPlanByRefNumberAsync(RequestHeader requestHeader, string refOrderNumber)
    {
        try
        {
            Console.WriteLine("\nStarting SendSearchInstallmentPlanByRefNumber");
            var requestUrl = Environment.GetEnvironmentVariable("ApiV3")! + SearchInstallmentPlan + "?RefOrderNumber="+refOrderNumber;
            var response = await _httpSender.SendGetHttpsRequestAsync(requestUrl, requestHeader);
            var jResponseSearch = JsonConvert.DeserializeObject<ResponseV3Search.Root>(response);
            Console.WriteLine("SendSearchInstallmentPlanByRefNumber succeeded\n");
            return jResponseSearch!;
        }
        catch (Exception exception)
        {
            Console.WriteLine("SendSearchInstallmentPlanByRefNumber Failed\n" +exception+ "\n");
            throw;
        }
    }

    public bool ValidateRedirectionAfterChallenge(ResponseV3Search.Root jResponseSearch)
    {
        try
        {
            Console.WriteLine("\nStarting ValidateRedirectionAfterChallenge");
            Assert.That(jResponseSearch.PlanList[0].InstallmentPlanNumber, Is.Not.Null);
            Console.WriteLine("ValidateRedirectionAfterChallenge Succeeded\n");
            return true;
        }
        catch (Exception exception)
        {
            Console.WriteLine("ValidateRedirectionAfterChallenge Failed\n" +exception+ "\n");
            return false;
        }
    }

    public async Task<ResponseV3GetPlanByIpn.ResponseRoot> GetInstallmentPlanByIpnAsync(
        RequestHeader requestHeader, string installmentPlanNumber)
    {
        try
        {
            Console.WriteLine("\nStarting GetInstallmentPlanByIpn");
            var requestUrl = Environment.GetEnvironmentVariable("ApiV3")! + CreatePlanEndpoint + "/"+installmentPlanNumber;
            var response = await _httpSender.SendGetHttpsRequestAsync(requestUrl, requestHeader);
            var jsonResponse = JsonConvert.DeserializeObject<ResponseV3GetPlanByIpn.ResponseRoot>(response);
            Console.WriteLine("GetInstallmentPlanByIpn Succeeded\n");
            return jsonResponse!;
        }
        catch (Exception exception)
        {
            Console.WriteLine("GetInstallmentPlanByIpn Failed\n" +exception+ "\n");
            throw;
        }
    }

    public bool ValidateAuthorizationObjectAfter3DsChallenge(ResponseV3GetPlanByIpn.ResponseRoot jsonResponse)
    {
        try
        {
            Console.WriteLine("\nStaring ValidateAuthorizationObjectAfter3DsChallenge");
            Console.WriteLine("Validating Plan Status is PendingCapture / Active");
            Assert.That(jsonResponse.Status, Is.Not.EqualTo("Initialized"), "Plan Status Error");
            Console.WriteLine("Validated Plan Status");
            Console.WriteLine("Validating Authorization Status is Succeeded");
            Assert.That(jsonResponse.Authorization.Status, Is.EqualTo("Succeeded"));
            Console.WriteLine("Validated Authorization Status");
            Console.WriteLine("Validating GatewayTransactionID exist");
            Assert.That(jsonResponse.Authorization.GatewayTransactionID, Is.Not.Null, "GatewayTransactionID Error");
            Console.WriteLine("Validated GatewayTransactionID exist");
            Console.WriteLine("Validating GatewayResultCode is Authorized");
            Assert.That(jsonResponse.Authorization.GatewayResultCode.Equals("Authorized"), Is.True);
            Console.WriteLine("Validated GatewayResultCode is Authorized");
            Console.WriteLine("Validating CAVV is not Null");
            Assert.That(jsonResponse.Authorization.CAVV, Is.Not.Null, "CAVV is Null");
            Console.WriteLine("Validated CAVV is not Null");
            Console.WriteLine("Validating ECI is not Null");
            Assert.That(jsonResponse.Authorization.ECI, Is.Not.Null, "ECI is Null");
            Console.WriteLine("Validated ECI is not Null");
            Console.WriteLine("ValidateAuthorizationObjectAfter3DsChallenge Succeeded\n");
            return true;
        }
        catch (Exception exception)
        {
            Console.WriteLine("ValidateAuthorizationObjectAfter3DsChallenge Failed\n" +exception+ "\n");
            return false;
        }
    }

    public bool Validate3DsFailure(ResponseV3.ResponseRoot jResponse)
    {
        try
        {
            Console.WriteLine("\nStarting Validate3DsFailure");
            Console.WriteLine("Validate Plan Status is Initialized | Actual Status: " + jResponse.Status);
            Assert.That(jResponse.Status, Is.EqualTo("Initialized"), "Plan Status Error");
            Console.WriteLine("Validate Authorization status on 'Pending3DS' | Actual Status: " +jResponse.Authorization.Status );
            Assert.That(jResponse.Authorization.Status, Is.EqualTo("Pending3DS"), "Authorization Status Error");
            Console.WriteLine("Validate GatewayResultCode should be 'ENROLL3D' | Actual Code: " + jResponse.Authorization.GatewayResultCode);
            Assert.That(jResponse.Authorization.GatewayResultCode.ToLower(), Is.EqualTo("enroll3d"), "GatewayResultCode Error");
            Console.WriteLine("Validate splititErrorResultCode is populated -> Actual value: " + jResponse.Authorization.SplititErrorResultCode);
            Assert.That(jResponse.Authorization.SplititErrorResultCode, Is.Not.Null, "splititErrorResultCode is NOT populated");
            Console.WriteLine("Validate3DsFailure Succeeded\n");
            return true;
        }
        catch (Exception exception)
        {
            Console.WriteLine("ValidateAuthorizationObjectAfter3DsChallenge Failed\n" +exception+ "\n");
            return false;
        }
    }

    public async Task<ResponseVerifyAuthorization.Root> SendVerifyAuthorizationRequestAsync(RequestHeader requestHeader, string ipn)
    {
        try
        {
            Console.WriteLine("\nStarting SendVerifyAuthorizationRequest");
            var requestUrl = Environment.GetEnvironmentVariable("ApiV3")! + CreatePlanEndpoint +"/" + ipn + VerifyAuthorizationEndpoint;
            var response = await _httpSender.SendGetHttpsRequestAsync(requestUrl, requestHeader);
            var responseVerifyAuthorization = JsonConvert.DeserializeObject<ResponseVerifyAuthorization.Root>(response);
            Console.WriteLine("SendVerifyAuthorizationRequest Succeeded\n");
            return responseVerifyAuthorization!;
        }
        catch (Exception exception)
        {
            Console.WriteLine("ValidateAuthorizationObjectAfter3DsChallenge Failed\n" +exception+ "\n");
            throw;
        }
    }

    public bool ValidateVerifyAuthorizationRequest(ResponseVerifyAuthorization.Root jResponseVerify, bool isAuthorized)
    {
        try
        {
            Console.WriteLine("\nStarting ValidateVerifyAuthorizationRequest");
            Assert.That(jResponseVerify.IsAuthorized, Is.EqualTo(isAuthorized));
            if(isAuthorized) {
                Console.WriteLine("Plan should be authorized");
                Assert.That(jResponseVerify.Authorization.Status, Is.EqualTo("Succeeded"), "Authorization Status Error");
                
            } else {
                Console.WriteLine("Validate that no Authorization applied - Authorization object should be null");
                Assert.That(jResponseVerify.Authorization, Is.Null, "Authorization Object Error");
            }
            Console.WriteLine("ValidateVerifyAuthorizationRequest Succeeded\n");
            return true;
        }
        catch (Exception exception)
        {
            Console.WriteLine("ValidateVerifyAuthorizationRequest Failed\n" +exception+ "\n");
            return false;
        }
    }

    public async Task<GetExtendedResponse.Root> SendExtendedParamsRequestAsync(RequestHeader requestHeader, string ipn)
    {
        try
        {
            Console.WriteLine("\nStarting SendExtendedParamsRequest");
            var queryCriteria = new GetExtended.QueryCriteria
            {
                InstallmentPlanNumber = ipn
            };
            var root = new GetExtended.Root(queryCriteria);
            var requestedUrl = _envConfig.AdminUrl + ExtendedParamsEndPoint;
            var sResponse = await _httpSender.SendPostHttpsRequestAsync(requestedUrl, root, requestHeader);
            var jResponseExtendedParams = JsonConvert.DeserializeObject<GetExtendedResponse.Root>(sResponse);
            if (jResponseExtendedParams!.PlansList.Count > 0)
            {
                for (var i = 0; i < 10; i++)
                {
                    await Task.Delay(5 * 1000);
                    sResponse = await _httpSender.SendPostHttpsRequestAsync(requestedUrl, root, requestHeader);
                    jResponseExtendedParams = JsonConvert.DeserializeObject<GetExtendedResponse.Root>(sResponse);
                    if (jResponseExtendedParams!.PlansList.Count == 0)
                    {
                        return jResponseExtendedParams;
                    }
                }
            }
            Console.WriteLine("SendExtendedParamsRequest Succeeded\n");
            return jResponseExtendedParams!;
        }
        catch (Exception exception)
        {
            Console.WriteLine("SendExtendedParamsRequest Failed\n" +exception+ "\n");
            throw;
        }
    }

    public bool ValidateFirstInstallmentAmount(ResponseV3.ResponseRoot json, double firstInstallmentAmount)
    {
        try
        {
            Console.WriteLine("\nStarting ValidateFirstInstallmentAmount");
            Console.WriteLine("Validate First Installment Amount is as expected...");
            Assert.That(json.Installments[0].Amount, Is.EqualTo(firstInstallmentAmount));
            Console.WriteLine("Validated, Actual First Installment is -> " + json.Installments[0].Amount + " Expected first installment amount is ->" +firstInstallmentAmount);
            var amount = json.Amount - json.Installments[0].Amount;
            Console.WriteLine("Validating -> Full Amount minus first installment equals to the next installment amount");
            Assert.That(amount, Is.EqualTo(json.Installments[1].Amount));
            Console.WriteLine("Validated -> Full Amount minus first installment equals to the next installment amount");
            Console.WriteLine("ValidateFirstInstallmentAmount Succeeded\n");
            return true;
        }
        catch (Exception exception)
        {
            Console.WriteLine("ValidateFirstInstallmentAmount Failed\n" +exception+ "\n");
            return false;
        }
    }

    public bool ValidateAllInstallmentsAmountsEquals(ResponseV3.ResponseRoot json)
    {
        try
        {
            Console.WriteLine("\nStarting ValidateAllInstallmentsAmountsEquals");
            var rangeEnd = 0.01;
            var firstAmount = json.Installments[0].Amount;
            Console.WriteLine("Starting Validating All Installments Are Equal: First Installment amount = " + firstAmount);
            foreach (var amount in json.Installments)
            {
                Console.WriteLine("Actual Amount = " + amount.Amount);
                var result = amount.Amount - firstAmount;
                if (!(result <= rangeEnd) && !(result >= rangeEnd))
                {
                    Assert.Fail("Error in ValidateAllInstallmentsAmountsEquals");
                }
                Console.WriteLine("Same value");
            }
            Console.WriteLine("Done ValidateAllInstallmentsAmountsEquals\n");
            return true;
        }
        catch (Exception exception)
        {
            Console.WriteLine("Error in ValidateAllInstallmentsAmountsEquals\n" +exception+ "\n");
            return false;
        }
    }
}