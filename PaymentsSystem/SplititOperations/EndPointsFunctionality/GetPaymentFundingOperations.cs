using Newtonsoft.Json;
using Splitit.Automation.NG.Backend.BaseActions;
using Splitit.Automation.NG.Backend.Services.V3.DefaultValues;
using Splitit.Automation.NG.PaymentsSystem.SplititOperations.BaseObjects;
using Splitit.Automation.NG.PaymentsSystem.SplititOperations.Responses;
using Splitit.Automation.NG.Utilities.EnvironmentsConfig;

namespace Splitit.Automation.NG.PaymentsSystem.SplititOperations.EndPointsFunctionality;

public class GetPaymentFundingOperations
{
    private const string GetPaymentFundingOperationStdEndPoint = "/api/splitit-operations/get-payment-funding-operations";
    private readonly GetPaymentFundingOperationsBaseObjects.Root _getPaymentFundingOperations = new();
    private readonly HttpSender _httpSender = new();
    private readonly EnvConfig _envConfig = new();
    
    public async Task<GetPaymentFundingOperationsResponse.Root?> SendPostRequestToGetPaymentFundingOperationsAsync(
        RequestHeader requestHeader, string merchantId, int numberOfRowsInPage, 
        int pageNumber, string sortParam, string? ipn = null!)
    {
        try
        {
            Console.WriteLine("\nStarting SendPostRequestToGetPaymentFundingOperations");
            if (ipn != null)
            {
                _getPaymentFundingOperations.installmentPlanNumber = ipn;
            }
            else
            {
                _getPaymentFundingOperations.merchantId = merchantId;
            }
            _getPaymentFundingOperations.numberOfRowsInPage = numberOfRowsInPage;
            _getPaymentFundingOperations.pageNumber = pageNumber;
            _getPaymentFundingOperations.sortParam = sortParam;
            _getPaymentFundingOperations.fromDate = DateTime.Today;
            _getPaymentFundingOperations.toDate = DateTime.Today.AddDays(2);
            var response = await _httpSender.SendPostHttpsRequestAsync(_envConfig.PaymentsUrl + GetPaymentFundingOperationStdEndPoint,
                _getPaymentFundingOperations, requestHeader);
            var jResponse = JsonConvert.DeserializeObject<GetPaymentFundingOperationsResponse.Root>(response);
            Console.WriteLine("Done with SendPostRequestToGetPaymentFundingOperations\n");
            return jResponse;
        }
        catch (Exception exception)
        {
            Console.WriteLine("Error in SendPostRequestToGetPaymentFundingOperations\n" + exception + "\n");
            throw;
        }
    }   
}