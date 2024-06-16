using Newtonsoft.Json;
using NUnit.Framework;
using Splitit.Automation.NG.Backend.BaseActions;
using Splitit.Automation.NG.Backend.Services.AdminApi.InstallmentPlan.InstallmentPlanResponses;
using Splitit.Automation.NG.Backend.Services.V3.DefaultValues;
using Splitit.Automation.NG.Utilities.EnvironmentsConfig;

namespace Splitit.Automation.NG.Backend.Services.AdminApi.InstallmentPlan.InstallmentsPlanFunctionality;

public class GetAuditLog
{
    private const string EndPoint = "/api/installment-plan/audit-log?InstallmentPlanNumber=";
    private readonly HttpSender _httpSender = new();
    private readonly EnvConfig _envConfig = new();
    private int _counter;

    public async Task<ResponseAuditLog.Root> SendGetRequestForGetAuditLogAsync(RequestHeader requestHeader, string ipn)
    {
        try
        {
            Console.WriteLine("\nStarting SendGetRequestForGetAuditLogAsync");
            await Task.Delay(10 * 1000);
            var route = _envConfig.AdminUrl + EndPoint + ipn + "&ExcludeActivity[0]=Get&";
            var response = await _httpSender.SendGetHttpsRequestAsync(route, requestHeader);
            var jResponse = JsonConvert.DeserializeObject<ResponseAuditLog.Root>(response);
            if (jResponse!.AuditLogs.Count == 0)
            {
                Assert.Fail("Audit logs was not found in the response.");
            }
            Console.WriteLine("Audit logs found in the response");
            return jResponse;
        }
        catch (Exception exception)
        {
            Console.WriteLine("Error in SendGetRequestForGetAuditLogAsync\n" + exception + "\n");
            throw;
        }
    }

    public async Task<bool> ValidateAuditLogExpectedActivityWithPolling(ResponseAuditLog.Root? jResponse, string expectedActivity, 
        RequestHeader requestHeader, string ipn)
    {
        try
        {
            Console.WriteLine("Starting ValidateExpectedActivity");
            if (jResponse!.AuditLogs.Any(log => log.Activity.Equals(expectedActivity)))
            {
                return true;
            }

            while (_counter < 10)
            {
                _counter++;
                var auditLogResponse = await SendGetRequestForGetAuditLogAsync(requestHeader, ipn);
                if (await ValidateAuditLogExpectedActivityWithPolling(auditLogResponse, expectedActivity, 
                        requestHeader, ipn))
                {
                    return true;
                }
            }
            
            Console.WriteLine("ExpectedActivity was not found");
            return false;
        }
        catch (Exception e)
        {
            Console.WriteLine("Error in ValidateExpectedActivity" + e);
            throw;
        }
    }
}