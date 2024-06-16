using Newtonsoft.Json;
using NUnit.Framework;
using Splitit.Automation.NG.Backend.BaseActions;
using Splitit.Automation.NG.Backend.Services.AdminApi.InstallmentPlan.InstallmentPlanResponses;
using Splitit.Automation.NG.Backend.Services.V3.DefaultValues;
using Splitit.Automation.NG.Utilities.EnvironmentsConfig;

namespace Splitit.Automation.NG.Backend.Services.AdminApi.InstallmentPlan.InstallmentPlanBaseObjects;

public class PlanJobFutureInformation
{
    private const string FutureInformationEndPoint = "/api/installment-plan/plan-job-future-information?InstallmentPlanId=&InstallmentPlanNumber=";
    private readonly HttpSender _httpSender = new();
    private readonly EnvConfig _envConfig = new();

    public async Task<ResponsePlanJobFutureInformation.Root> SendGetRequestForFutureJobsAsync( RequestHeader requestHeader,
        string ipn)
    {
        try
        {
            Console.WriteLine("\nStarting SendGetRequestForFutureJobs");
            var requestUrl = _envConfig.AdminUrl + FutureInformationEndPoint + ipn;
            var response = await _httpSender.SendGetHttpsRequestAsync(requestUrl, requestHeader);
            var jResponse = JsonConvert.DeserializeObject<ResponsePlanJobFutureInformation.Root>(response);
            if (jResponse!.Errors != null!)
            {
                Console.WriteLine("Error in SendGetRequestForFutureJobs \n" + jResponse.Errors[0].Message + "\n");
                return null!;
            }
            Console.WriteLine("SendGetRequestForFutureJobs Done\n");
            return jResponse;
        }
        catch (Exception exception)
        {
            Console.WriteLine("Error in SendGetRequestForFutureJobs \n" + exception + "\n");
            return null!;
        }
    }

    public bool ValidateJobName(ResponsePlanJobFutureInformation.Root jResponsePlanJobFutureInfo, string jobName)
    {
        try
        {
            Console.WriteLine("\nStarting ValidateJobName");
            foreach (var job in jResponsePlanJobFutureInfo.FutureRuns)
            {
                if (!job.JobName.Equals(jobName)) continue;
                var currentDate = DateTime.Now;
                Assert.That(currentDate.Day.Equals(job.LastExecution.Day));
                Assert.That(currentDate.Month.Equals(job.LastExecution.Month));
                Assert.That(currentDate.Year.Equals(job.LastExecution.Year));
            }

            Console.WriteLine("Done with ValidateJobName\n");
            return true;
        }
        catch (Exception exception)
        {
            Console.WriteLine("Error in ValidateJobName \n" + exception + "\n");
            return false;
        }
    }

    public bool ValidateJobExecutionDate(ResponsePlanJobFutureInformation.Root planJobFutureInfo, string jobName)
    {
        try
        {
            var listResult = new List<bool>();
            Console.WriteLine("Starting ValidateJobExecutionDate");
            foreach (var job in planJobFutureInfo.FutureRuns)
            {
                if (!job.JobName.Equals(jobName)) continue;
                var currentDate = DateTime.Now;
                var currentDateDay = currentDate.Day;
                var currentDateMonth = currentDate.Month;
                var currentDateYear = currentDate.Year;

                if (currentDateDay == job.LastExecution.Day && currentDateMonth == job.LastExecution.Month &&
                    currentDateYear == job.LastExecution.Year)
                {
                    listResult.Add(true);
                }
                else
                {
                    listResult.Add(false);
                }
            }

            if (listResult.Contains(true))
            {
                return true;
            }
            Console.WriteLine("Done with ValidateJobExecutionDate");
            return false;
        }
        catch (Exception e)
        {
            Console.WriteLine("Error in ValidateJobExecutionDate" + e);
            throw;
        }
    }
}