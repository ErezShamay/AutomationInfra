using Newtonsoft.Json;
using NUnit.Framework;
using Splitit.Automation.NG.Backend.BaseActions;
using Splitit.Automation.NG.Backend.Services.DisputesV2.Dispute.BaseObjects;
using Splitit.Automation.NG.Backend.Services.DisputesV2.Dispute.Responses;
using Splitit.Automation.NG.Backend.Services.V3.DefaultValues;
using Splitit.Automation.NG.Utilities.EnvironmentsConfig;

namespace Splitit.Automation.NG.Backend.Services.DisputesV2.Dispute.Functionality;

public class PostDisputesFunctionality
{
    private const string EndPoint = "/api/v1/disputes";
    private readonly HttpSender _httpSender = new();
    private readonly PostDisputesBaseObjects.Root _postDisputesBaseObjects = new();
    private readonly EnvConfig _envConfig = new();

    public async Task<PostDisputesResponse.Root> SendPostRequestPostDisputesAsync(
        RequestHeader requestHeader, string ipn, int merchantId, string status, int index = 0, int counter = 0)
    {
        try
        {
            Console.WriteLine("\nStarting SendPostRequestPostDisputes");
            _postDisputesBaseObjects.InstallmentPlanNumbers = new List<string> { ipn };
            _postDisputesBaseObjects.MerchantId = merchantId;
            _postDisputesBaseObjects.PagingRequest = new PostDisputesBaseObjects.PagingRequest
            {
                Skip = 0,
                Take = 10
            };
            PostDisputesResponse.Root? jResponse = null;


            var response = await _httpSender.SendPostHttpsRequestAsync(_envConfig.DisputesV2Url + EndPoint,
                _postDisputesBaseObjects, requestHeader);
            jResponse = JsonConvert.DeserializeObject<PostDisputesResponse.Root>(response);
            

            if (jResponse!.Disputes.Count > 0)
            {
                return jResponse;
            }    
            else if (counter < 5)
            {
                await Task.Delay(5 * 1000);
                return await SendPostRequestPostDisputesAsync(requestHeader, ipn, merchantId, status, 0, ++counter);
            }
            else 
            {
                Assert.Fail("\n\nNo Disputes were found");
                return jResponse;
            }
        }
        catch (Exception exception)
        {
            Console.WriteLine("Error in SendPostRequestPostDisputes \n" + exception + "\n");
            throw;
        }
    }

    public bool ValidateDisputeStatus(PostDisputesResponse.Root jResponseDisputesResponse, string status)
    {
        try
        {
            Console.WriteLine("Starting ValidateDisputeStatus");
            foreach (var dispute in jResponseDisputesResponse.Disputes)
            {
                Console.WriteLine("Dispute status is: " + dispute.Status);
                if (dispute.Status.Equals(status))
                {
                    Console.WriteLine("Done with ValidateDisputeStatus");
                    return true;
                }
            }
            Console.WriteLine("Error in ValidateDisputeStatus");
            return false;
        }
        catch (Exception e)
        {
            Console.WriteLine("Error in ValidateDisputeStatus" + e);
            throw;
        }
    }
}