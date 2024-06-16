using Splitit.Automation.NG.Backend.BaseActions;
using Splitit.Automation.NG.Backend.Services.V3.DefaultValues;

namespace Splitit.Automation.NG.Backend.Services.KeyExchangeServer.Sample.Functionality;

public class CreatePlanV3Kes
{
    private readonly HttpSender _httpSender = new();
    private readonly StatusToFlagInit _statusToFlagInit = new();
    private const string CreatePlanEndpoint = "/api/installmentplans";

    public async Task<(string stringBodyResponse, HttpResponseMessage response)> CreatePlanKesAsync(
        string baseUrl, RequestHeader requestHeader, string status, 
        int numberOfInstallments, string terminal, CreatePlanDefaultValues createPlanDefaultValues)
    {
        try
        {
            Console.WriteLine("Starting CreatePlanAsync");
            createPlanDefaultValues = _statusToFlagInit.StatusToFlagsInit(status, createPlanDefaultValues);
            createPlanDefaultValues.planData.terminalId = terminal;
            createPlanDefaultValues.planData.numberOfInstallments = numberOfInstallments;
            var (stringBodyResponse, response )=
                await _httpSender.SendPostRequestReturnResponseStringAndHeaders(baseUrl + CreatePlanEndpoint, createPlanDefaultValues,
                    requestHeader.sessionId);
            Console.WriteLine("Done with CreatePlanAsync");
            return (stringBodyResponse, response);
        }
        catch (Exception e)
        {
            Console.WriteLine("Error in CreatePlanAsync" + e);
            throw;
        }
    }

    public async Task<string> CreatePlanKesAsyncReturnString(
        string baseUrl, RequestHeader requestHeader, string status,
        int numberOfInstallments, string terminal, CreatePlanDefaultValues createPlanDefaultValues)
    {
        try
        {
            Console.WriteLine("\nStarting CreatePlanKesAsyncReturnString");
            createPlanDefaultValues = _statusToFlagInit.StatusToFlagsInit(status, createPlanDefaultValues);
            createPlanDefaultValues.planData.terminalId = terminal;
            createPlanDefaultValues.planData.numberOfInstallments = numberOfInstallments;
            var response = await _httpSender.SendPostHttpsRequestAsync(baseUrl + CreatePlanEndpoint, createPlanDefaultValues, requestHeader);
            Console.WriteLine("Done with CreatePlanKesAsyncReturnString");
            return response;
        }
        catch (Exception e)
        {
            Console.WriteLine("Error in CreatePlanKesAsyncReturnString" + e);
            throw;
        }
    }
}