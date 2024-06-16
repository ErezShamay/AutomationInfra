using Newtonsoft.Json;
using Splitit.Automation.NG.Backend.Services.AdminApi.InstallmentPlan.InstallmentPlanResponses;
using Splitit.Automation.NG.Backend.Services.V3.DefaultValues;
using Splitit.Automation.NG.Utilities.EnvironmentsConfig;

namespace Splitit.Automation.NG.Backend.BaseActions;

public class AuditLogController
{
    private const string AuditLogEndPoint = "/api/installment-plan/audit-log?InstallmentPlanNumber=";
    private readonly HttpSender _httpSender = new();
    private readonly EnvConfig _envConfig = new();

    private class AuditLogRoot
    {
        public AuditLogRoot(RequestHeader requestHeader, string installmentPlanNumber)
        {
            RequestHeader = requestHeader;
            InstallmentPlanNumber = installmentPlanNumber;
            PlanApprovalEvidenceDefaultValues = new PlanApprovalEvidenceDefaultValues();
        }

        private RequestHeader RequestHeader { get; set; }
        private string InstallmentPlanNumber { get; set; }
        private PlanApprovalEvidenceDefaultValues PlanApprovalEvidenceDefaultValues { get; set; }
    }
    
    public bool ValidateAuditLogLogs(IEnumerable<string> expectedActions, ResponseAuditLog.Root jAuditLogResponse)
    {
        try
        {
            Console.WriteLine("\nStaring ValidateAuditLogLogs");
            var ans = true;
            var actualResults = jAuditLogResponse.AuditLogs.Select(log => log.Activity).ToList();
            foreach (var expectedAction in expectedActions)
            {
                if (actualResults.IndexOf(expectedAction) != -1) continue;
                Console.WriteLine(expectedAction + " Was not found");
                ans = false;
            }

            return ans;
        }
        catch (Exception exception)
        {
            Console.WriteLine("ValidateAuditLogLogs Failed \n" + exception + "\n");
            throw;
        }
    }

    public async Task<ResponseAuditLog.Root?> SendRetrieveAuditLogRequestAsync(RequestHeader requestHeader, string ipn)
    {
        try
        {
            Console.WriteLine("\nStaring SendRetrieveAuditLogRequest");
            await Task.Delay(10*1000);
            var updatedAuditLogEndPoint = AuditLogEndPoint + ipn;
            var response = await _httpSender.SendGetHttpsRequestAsync(
                _envConfig.AdminUrl + updatedAuditLogEndPoint, requestHeader);
            var jResponse = JsonConvert.DeserializeObject<ResponseAuditLog.Root>(response);
            Console.WriteLine("SendRetrieveAuditLogRequest Succeeded\n");
            return jResponse;
        }
        catch(Exception exception)
        {
            Console.WriteLine("ValidateAuditLogLogs Failed \n" + exception + "\n");
            throw;
        }
    }
    
    public bool ValidateAuditLogLogsWithCounter(string ipn, string[] logActions, ResponseAuditLog.Root jAuditLogResponse)
    {
        var counter = 0;
        try
        {
            Console.WriteLine("\nStaring ValidateAuditLogLogs");
            Console.WriteLine("Validating audit log actions for IPN: " + ipn);
            foreach (var action in logActions)
            {
                Console.WriteLine("Searching action: " + action + " in Audit log");
                foreach (var log in jAuditLogResponse.AuditLogs.Where(log => action.Equals(log.Activity)))
                {
                    Console.WriteLine("Action: " + action + " was found in audit log: " + log.Activity);
                    counter++;
                }
            }

            if (counter/2 != logActions.Length)
            {
                return false;
            }
            Console.WriteLine("ValidateAuditLogLogs Succeeded\n");
            return true;
        }
        catch (Exception exception)
        {
            Console.WriteLine("ValidateAuditLogLogs Failed \n" + exception + "\n");
            throw;
        }
    }

    public async Task PrintAllAuditLogResultMessagesAsync(RequestHeader requestHeader, string ipn)
    {
        try
        {
            Console.WriteLine("\nStarting PrintAllAuditLogResultMessages");
            Console.WriteLine("Getting Audit log message");
            var jAuditLogResponse = await SendRetrieveAuditLogRequestAsync(requestHeader, ipn);
            Console.WriteLine("GOT Audit log message");
            foreach (var mAuditLog in jAuditLogResponse!.AuditLogs)
            {
                Console.WriteLine("The Audit log ResultMessage is --> " + mAuditLog.ResultMessage);
                Console.WriteLine("The Audit log AdditionalInfo is --> " + mAuditLog.AdditionalInfo);
            }
            Console.WriteLine("Done with PrintAllAuditLogResultMessages\n");
        }
        catch(Exception exception)
        {
            Console.WriteLine("Error in PrintAllAuditLogResultMessages \n" + exception + "\n");
        }
    }

    public async Task<bool> WaitForActivityToBeLogged(
        RequestHeader requestHeader, string ipn, string[] activities)
    {
        var counter = 0;
        try
        {
            Console.WriteLine("Starting WaitForActivityToBeLogged");
            while (counter < 8)
            {
                var jAuditLogResponse = await SendRetrieveAuditLogRequestAsync(requestHeader, ipn);
                if (ValidateAuditLogLogs(activities, jAuditLogResponse!))
                {
                    return true;
                }
                counter += 1;
            }
            Console.WriteLine("Done with WaitForActivityToBeLogged");
            return false;
        }
        catch (Exception e)
        {
            Console.WriteLine("Error in WaitForActivityToBeLogged" + e);
            throw;
        }
    }
}