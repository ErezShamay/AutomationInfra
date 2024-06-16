using Newtonsoft.Json;
using Splitit.Automation.NG.Backend.BaseActions;
using Splitit.Automation.NG.Backend.Services.DisputesV2.AuditLogs.Responses;
using Splitit.Automation.NG.Backend.Services.V3.DefaultValues;
using Splitit.Automation.NG.Utilities.EnvironmentsConfig;

namespace Splitit.Automation.NG.Backend.Services.DisputesV2.AuditLogs.Functionality;

public class GetAuditLogsFunctionality
{
    private const string EndPoint = "/api/v1/audit-logs";
    private readonly HttpSender _httpSender = new();
    private readonly EnvConfig _envConfig = new();

    public async Task<GetAuditLogsResponse.Root> SendGetRequestGetAuditLogsAsync(
        RequestHeader requestHeader, string disputeId, string pagingRequestSkip, string pagingRequestTake)
    {
        try
        {
            Console.WriteLine("\nStarting SendGetRequestGetAuditLogs");
            var response = await _httpSender.SendGetHttpsRequestAsync(
                _envConfig.DisputesV2Url + EndPoint +
                "?DisputeId=" + disputeId + "&PagingRequest.Skip=" + pagingRequestSkip + "&PagingRequest.Take=" +
                pagingRequestTake + "&",
                requestHeader);
            var jResponse = JsonConvert.DeserializeObject<GetAuditLogsResponse.Root>(response);
            if (jResponse!.AuditLogs.Count == 0)
            {
                for (var i = 0; i < 10; i++)
                {
                    response = await _httpSender.SendGetHttpsRequestAsync(
                        _envConfig.DisputesV2Url + EndPoint +
                        "?DisputeId=" + disputeId + "&PagingRequest.Skip=" + pagingRequestSkip + "&PagingRequest.Take=" +
                        pagingRequestTake + "&",
                        requestHeader);
                    jResponse = JsonConvert.DeserializeObject<GetAuditLogsResponse.Root>(response);

                    if (jResponse!.AuditLogs.Count > 0)
                    {
                        return jResponse;
                    }
                    else
                    {
                        await Task.Delay(2 * 1000);
                    }
                }

            }
            
            Console.WriteLine("Done with SendGetRequestGetAuditLogs\n");
            return jResponse!;
        }
        catch (Exception exception)
        {
            Console.WriteLine("Error in SendGetRequestGetAuditLogs \n" + exception + "\n");
            throw;
        }
    }

    public bool ValidateAuditLogStatus(GetAuditLogsResponse.Root jResponse, List<string> expectedLogs)
    {
        var verifyList = new List<bool>();
        var flag = false;
        try
        {
            Console.WriteLine("Starting ValidateAuditLogStatus");
            foreach (var expectedLog in expectedLogs)
            {
                foreach (var actualLog in jResponse.AuditLogs)
                {
                    if (actualLog.GatewayPayload != null!)
                    {
                        if (actualLog.GatewayPayload.Contains(expectedLog))
                        {
                            verifyList.Add(true);
                            flag = true;
                        }
                    }
                }

                if (!flag)
                {
                    verifyList.Add(false);
                }
            }

            if (verifyList.Contains(false))
            {
                Console.WriteLine("Done ValidateAuditLogStatus return false");
                return false;
            }

            Console.WriteLine("Done ValidateAuditLogStatus return true");
            return true;
        }
        catch (Exception exception)
        {
            Console.WriteLine("Error in ValidateAuditLogStatus \n" + exception + "\n");
            throw;
        }
    }

    public bool ValidateAuditLogsActions(GetAuditLogsResponse.Root jResponse, List<string> expectedLogsActions)
    {
        var verifyList = new List<bool>();
        var flag = false;
        try
        {
            Console.WriteLine("\n\nStarting ValidateAuditLogStatus");
            foreach (var expectedLogAction in expectedLogsActions)
            {
                Console.WriteLine("expectedLogAction is -> " + expectedLogAction);
                foreach (var actualLog in jResponse.AuditLogs)
                {
                    Console.WriteLine("actualLog is -> " + actualLog.Action);
                    if (actualLog.Action.Contains(expectedLogAction))
                    {
                        verifyList.Add(true);
                        flag = true;
                    }
                }

                if (!flag)
                {
                    verifyList.Add(false);
                }
            }

            if (verifyList.Contains(false))
            {
                Console.WriteLine("Done ValidateAuditLogStatus return false\n");
                return false;
            }

            Console.WriteLine("Done ValidateAuditLogStatus return true\n");
            return true;
        }
        catch (Exception exception)
        {
            Console.WriteLine("Error in ValidateAuditLogStatus \n" + exception + "\n");
            throw;
        }
    }
}