using Newtonsoft.Json;
using NUnit.Framework;
using Splitit.Automation.NG.Backend.BaseActions;
using Splitit.Automation.NG.Backend.Services.AdminApi.InstallmentPlan.InstallmentPlanBaseObjects;
using Splitit.Automation.NG.Backend.Services.AdminApi.InstallmentPlan.InstallmentPlanResponses;
using Splitit.Automation.NG.Backend.Services.V3.DefaultValues;
using Splitit.Automation.NG.Utilities.EnvironmentsConfig;

namespace Splitit.Automation.NG.Backend.Services.AdminApi.InstallmentPlan.InstallmentsPlanFunctionality;

public class StartInstallmentsFunctionality
{
    private const string StartInstallmentsEndPoint = "/api/Installment-Plan/Start-Installments";
    private readonly HttpSender _httpSender = new();
    private int _counter;
    private readonly EnvConfig _envConfig = new();
    
    public async Task<ResponseStartInstallments.Root> SendStartInstallmentsRequestAsync(
        RequestHeader requestHeader, string installmentPlanNumber)
    {
        try
        {
            Console.WriteLine("\nStarting SendStartInstallmentsRequest");
            await Task.Delay(5 * 1000);
            var sendStartInstallmentsRequest = BuildStartInstallmentsObject(installmentPlanNumber);
            Console.WriteLine("Starting Sending the request for -> StartInstallmentsRequest");
            var endPoint = _envConfig.AdminUrl + StartInstallmentsEndPoint;
            var response = await _httpSender.SendPostHttpsRequestAsync(endPoint, sendStartInstallmentsRequest, requestHeader);
            var responseStartInstallments = JsonConvert.DeserializeObject<ResponseStartInstallments.Root>(response);
            Console.WriteLine("Done with Sending the request for -> StartInstallmentsRequest");
            if (responseStartInstallments is not null 
                && responseStartInstallments.InstallmentPlan is not null 
                && responseStartInstallments.ResponseHeader.Succeeded)
            {
                if (!responseStartInstallments.InstallmentPlan.InstallmentPlanStatus.Code.Equals("Cleared"))
                {
                    if (!responseStartInstallments.InstallmentPlan.InstallmentPlanStatus.Code.Equals("InProgress"))
                    {
                        await Task.Delay(2 * 1000);
                        await SendStartInstallmentsRequestAsync(requestHeader, installmentPlanNumber);
                    }
                    Assert.That(responseStartInstallments.InstallmentPlan.InstallmentPlanStatus.Code, Is.EqualTo("InProgress"));
                }
            }
            else
            {
                await Task.Delay(2 * 1000);
               await SendStartInstallmentsRequestAsync(requestHeader, installmentPlanNumber);
            }

            Console.WriteLine("Done SendStartInstallmentsRequest\n");
            return responseStartInstallments!;
        }
        catch (Exception exception)
        {
            Console.WriteLine("Error in SendStartInstallmentsRequest \n" + exception + "\n");
            throw;
        }
    }

    private StartInstallments.Root BuildStartInstallmentsObject(string installmentPlanNumber)
    {
        try
        {
            Console.WriteLine("\nStarting BuildStartInstallmentsObject");
            var startInstallmentsObject = new StartInstallments.Root
            {
                InstallmentPlanNumber = installmentPlanNumber
            };
            Console.WriteLine("Done BuildStartInstallmentsObject\n");
            return startInstallmentsObject;
        }
        catch (Exception exception)
        {
            Console.WriteLine("Error in BuildStartInstallmentsObject \n" + exception + "\n");
            throw;
        }
    }
}