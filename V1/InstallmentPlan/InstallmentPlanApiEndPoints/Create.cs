using Newtonsoft.Json;
using NUnit.Framework;
using Splitit.Automation.NG.Backend.BaseActions;
using Splitit.Automation.NG.Backend.Services.AdminApi.InstallmentPlan.InstallmentsPlanFunctionality;
using Splitit.Automation.NG.Backend.Services.V1.DefaultValues;
using Splitit.Automation.NG.Backend.Services.V1.InstallmentPlan.InstallmentPlanResponses;
using Splitit.Automation.NG.Backend.Services.V3.DefaultValues;
using Splitit.Automation.NG.Utilities.EnvironmentsConfig;

namespace Splitit.Automation.NG.Backend.Services.V1.InstallmentPlan.InstallmentPlanApiEndPoints;

public class Create
{
    private const string CreatePlanEndPoint = "/api/InstallmentPlan/Create";
    private const string CreatePlanInitiateEndPoint = "/api/InstallmentPlan/Initiate";
    private readonly HttpSender _httpSender = new();
    private readonly EnvConfig _envConfig = new();
    private readonly InstallmentPlanNumberCancel _installmentPlanNumberCancel = new();
    private readonly GetPgtl _getPgtl = new();

    public async Task<CreateResponse.Root> CreatePlanAsync(
        RequestHeader requestHeader, V1InitiateDefaultValues v1InitiateDefaultValues, 
        string terminal = default!, string tokenFlag = default!, string token = default!, 
        string type = default!, string last4Digit = default!, bool shouldCancel = true)
    {
        try
        {
            Console.WriteLine("\nStarting CreatePlan");
            v1InitiateDefaultValues.RequestHeader.sessionId = requestHeader.sessionId;
            v1InitiateDefaultValues.RequestHeader.apiKey = terminal != null! ? terminal : requestHeader.apiKey;
            if (tokenFlag != null!)
            {
                switch (tokenFlag)
                {
                    case "AuthNetV2":
                        v1InitiateDefaultValues.paymentToken.token = token;
                        v1InitiateDefaultValues.paymentToken.type = type;
                        v1InitiateDefaultValues.paymentToken.last4Digits = last4Digit;
                        break;
                    case "PaySafe":
                        v1InitiateDefaultValues.paymentToken.token = token;
                        v1InitiateDefaultValues.paymentToken.type = type;
                        break;
                    case "BluesnapDirect":
                        v1InitiateDefaultValues.paymentToken.token = token;
                        v1InitiateDefaultValues.paymentToken.type = type;
                        v1InitiateDefaultValues.paymentToken.last4Digits = last4Digit;
                        break;
                }
            }
            var response = await _httpSender.SendPostHttpsRequestAsync(
                _envConfig.V1BaseUrl + CreatePlanEndPoint, 
                v1InitiateDefaultValues, requestHeader);
            var jResponse = JsonConvert.DeserializeObject<CreateResponse.Root>(response);
            if (jResponse!.InstallmentPlan == null)
            {
                Assert.Fail("Error in CreatePlan");
            }
            if (shouldCancel)
            {
                Task.Delay(new Random().Next(180000, 210000)).ContinueWith(t =>
                {
                    _installmentPlanNumberCancel.SendCancelPlanRequestAsync(requestHeader, jResponse.InstallmentPlan!.InstallmentPlanNumber,
                        0, "NoRefunds");
                });
            }
            var pgtlCaptureId = await _getPgtl.ValidatePgtlKeyValueInnerAsync(Environment.GetEnvironmentVariable("StoreProcedureUrl")!,
                requestHeader, jResponse.InstallmentPlan!.InstallmentPlanNumber, "Type", "Capture","Id");
            if (pgtlCaptureId!.Equals("") || pgtlCaptureId == null!)
            {
                Console.WriteLine("pgtlCaptureId is null or empty");
            }
            Console.WriteLine("Done with CreatePlan");
            Console.WriteLine("Plan was created IPN -> "+ jResponse.InstallmentPlan!.InstallmentPlanNumber +" with status -> " + jResponse.InstallmentPlan.InstallmentPlanStatus.Code);
            return jResponse;
        }
        catch(Exception exception)
        {
            Console.WriteLine("Error in CreatePlan" +exception+ "\n");
            throw;
        }
    }
    
    public async Task<InitiateResponse.Root> CreatePlanInitiateAsync( RequestHeader requestHeader, V1InitiateDefaultValues v1InitiateDefaultValues)
    {
        try
        {
            Console.WriteLine("\nStarting CreatePlanInitiate");
            v1InitiateDefaultValues.RequestHeader.apiKey = requestHeader.apiKey;
            v1InitiateDefaultValues.RequestHeader.sessionId = requestHeader.sessionId;
            var response = await _httpSender.SendPostHttpsRequestAsync(_envConfig.V1BaseUrl + CreatePlanInitiateEndPoint, v1InitiateDefaultValues, requestHeader);
            var jResponse = JsonConvert.DeserializeObject<InitiateResponse.Root>(response);
            if (jResponse!.InstallmentPlan == null)
            {
                Assert.Fail("Error in CreatePlanInitiate");
            }
            Console.WriteLine("Done with CreatePlanInitiate");
            Console.WriteLine("Plan was created IPN -> "+ jResponse.InstallmentPlan!.InstallmentPlanNumber +" with status -> " + jResponse.InstallmentPlan.InstallmentPlanStatus.Code);
            return jResponse;
        }
        catch(Exception exception)
        {
            Console.WriteLine("Error in CreatePlanInitiate" +exception+ "\n");
            throw;
        }
    }
}