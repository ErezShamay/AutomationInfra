using System.Diagnostics;
using Newtonsoft.Json;
using NUnit.Framework;
using Splitit.Automation.NG.Backend.BaseActions;
using Splitit.Automation.NG.Backend.Services.V3.DefaultValues;
using Splitit.Automation.NG.PaymentsSystem.SplititOperations.BaseObjects;
using Splitit.Automation.NG.PaymentsSystem.SplititOperations.Responses;
using Splitit.Automation.NG.Utilities.EnvironmentsConfig;

namespace Splitit.Automation.NG.PaymentsSystem.SplititOperations.EndPointsFunctionality;

public class GetPaymentRecordsFunctionality
{
    private const string GetPaymentRecordsEndPoint = "/api/splitit-operations/get-payment-records";
    private readonly HttpSender _httpSender = new();
    private readonly EnvConfig _envConfig = new();

    private GetPaymentRecordsBaseObjects.Root BuildGetPaymentRecordsObject(string? installmentPlanNumber)
    {
        try
        {
            Console.WriteLine("\nStarting BuildGetPaymentRecordsObject");
            var getPaymentRecordsRequest = new GetPaymentRecordsBaseObjects.Root
            {
                installmentPlanNumber = installmentPlanNumber,
                pageNumber = 1,
                numberOfRowsInPage = 10,
                activities = new List<int>()
            };
            for (var i = 0 ; i < 9 ; i ++)
            {
                getPaymentRecordsRequest.activities.Add(i);
            }
            getPaymentRecordsRequest.paymentRecordStatus = new List<int>();
            for (var i = 0 ; i < 15 ; i ++)
            {
                getPaymentRecordsRequest.paymentRecordStatus.Add(i);
            }
            Console.WriteLine("Done BuildGetPaymentRecordsObject\n");
            return getPaymentRecordsRequest;
        }
        catch (Exception exception)
        {
            Console.WriteLine("Error in BuildGetPaymentRecordsObject \n" + exception + "\n");
            return null!;
        }
    }

    public async Task<GetPaymentRecordsResponse.Root> SendGetPaymentRecordRequestAsync(RequestHeader requestHeader,
        string? installmentPlanNumber)
    {
        try
        {
            Console.WriteLine("\nStarting SendGetPaymentRecordRequest");
            var getPaymentRecordsObject = BuildGetPaymentRecordsObject(installmentPlanNumber);
            var endPoint = _envConfig.PaymentsUrl + GetPaymentRecordsEndPoint;
            var response = await _httpSender.SendPostHttpsRequestAsync(endPoint, getPaymentRecordsObject, requestHeader);
            var responseGetPaymentRecords = JsonConvert.DeserializeObject<GetPaymentRecordsResponse.Root>(response);
            if (responseGetPaymentRecords!.totalRecords == 0)
            {
                for (var i = 0; i < 10; i++)
                {
                    response = await _httpSender.SendPostHttpsRequestAsync(endPoint, getPaymentRecordsObject, requestHeader);
                    responseGetPaymentRecords = JsonConvert.DeserializeObject<GetPaymentRecordsResponse.Root>(response);
                    if (responseGetPaymentRecords!.totalRecords > 0)
                    {
                        Console.WriteLine("Found in mongo");
                        return responseGetPaymentRecords;
                    }
                }
            }
            Console.WriteLine("Checking after 5 retry (each has time out of 30 sec) if total records is grater then 0");
            Assert.That(responseGetPaymentRecords.totalRecords, Is.GreaterThan(0));
            Console.WriteLine("Total records is grater then 0");
            Debug.Assert(responseGetPaymentRecords != null, nameof(responseGetPaymentRecords) + " != null");
            Console.WriteLine("Done with SendGetPaymentRecordRequest\n");
            return responseGetPaymentRecords;
        }
        catch (Exception exception)
        {
            Console.WriteLine("Error in SendGetPaymentRecordRequest \n" + exception + "\n");
            return null!;
        }
    }

    public bool ValidateSkuValue(GetPaymentRecordsResponse.Root json, string testName)
    {
        try
        {
            Console.WriteLine("\nStarting ValidateSkuValue");
            var expectedResultSplitList = testName.Split("_");
            var actualResultSplitList = json.getPaymentRecordResponses[0].sku.Split("-");
            Assert.That(actualResultSplitList, Has.Length.EqualTo(7));
            foreach (var variable in actualResultSplitList)
            {
                Console.WriteLine("Searching Segment -> " + variable);
                Assert.That(expectedResultSplitList.Contains(variable));
                Console.WriteLine("Segment -> " + variable + " Found");
            }
            Console.WriteLine("Done ValidateSkuValue\n");
            return true;
        }
        catch (Exception exception)
        {
            Console.WriteLine("Error in ValidateSkuValue \n" + exception + "\n");
            return false;
        }
    }
}