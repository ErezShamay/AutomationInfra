using Newtonsoft.Json;
using Splitit.Automation.NG.Backend.BaseActions;
using Splitit.Automation.NG.Backend.Services.V3.DefaultValues;
using Splitit.Automation.NG.PaymentsSystem.SplititOperations.BaseObjects;
using Splitit.Automation.NG.PaymentsSystem.SplititOperations.Responses;
using Splitit.Automation.NG.Utilities.EnvironmentsConfig;

namespace Splitit.Automation.NG.PaymentsSystem.SplititOperations.EndPointsFunctionality;

public class GetFailedPaymentOperations
{
    private const string GetFailedPaymentOperationsEndPoint = "/api/splitit-operations/get-failed-payment-operations";
    private readonly GetFailedPaymentOperationsBaseObjects.Root _getFailedPaymentOperations = new();
    private readonly HttpSender _httpSender = new();
    private readonly EnvConfig _envConfig = new();
    
    public async Task<GetFailedPaymentOperationsResponse.Root?> SendPostRequestToGetFailedPaymentOperationsAsync(
        RequestHeader requestHeader, string merchantId,
        int numberOfRowsInPage, int pageNumber, bool isBusinessEx, string ipn = null!)
    {
        try
        {
            Console.WriteLine("\nStarting SendPostRequestToGetPaymentFundingOperations");
            if (ipn != null!)
            {
                _getFailedPaymentOperations.installmentPlanNumber = ipn;
            }
            else
            {
                _getFailedPaymentOperations.merchantId = merchantId;
            }
            _getFailedPaymentOperations.numberOfRowsInPage = numberOfRowsInPage;
            _getFailedPaymentOperations.pageNumber = pageNumber;
            _getFailedPaymentOperations.fromDate = DateTime.Today;
            _getFailedPaymentOperations.toDate = DateTime.Today.AddDays(2);
            _getFailedPaymentOperations.isBusinessEx = isBusinessEx;
            var response = await _httpSender.SendPostHttpsRequestAsync(_envConfig.PaymentsUrl + GetFailedPaymentOperationsEndPoint,
                _getFailedPaymentOperations, requestHeader);
            var jResponse = JsonConvert.DeserializeObject<GetFailedPaymentOperationsResponse.Root>(response);
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