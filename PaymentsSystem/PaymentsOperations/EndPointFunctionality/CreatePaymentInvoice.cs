using Splitit.Automation.NG.Backend.BaseActions;
using Splitit.Automation.NG.Backend.Services.V3.DefaultValues;
using Splitit.Automation.NG.PaymentsSystem.PaymentsOperations.BaseObjects;
using Splitit.Automation.NG.Utilities.EnvironmentsConfig;

namespace Splitit.Automation.NG.PaymentsSystem.PaymentsOperations.EndPointFunctionality;

public class CreatePaymentInvoice
{
    private const string CreatePaymentInvoiceEndPoint = "/api/payments/create-payment-invoice";
    private readonly CreatePaymentsInvoiceBaseObjects.Root _createPaymentsInvoiceBaseObjects = new();
    private readonly CreatePaymentsInvoiceBaseObjects.InvoiceDetail _invoiceDetail = new();
    private readonly HttpSender _httpSender = new();
    private readonly EnvConfig _envConfig = new();

    public async Task<string> SendPostRequestCreatePaymentInvoiceAsync( RequestHeader requestHeader, string invoiceId = default!)
    {
        try
        {
            Console.WriteLine("\nStarting SendPostRequestCreatePaymentInvoice");
            _createPaymentsInvoiceBaseObjects.invoiceDetails = new List<CreatePaymentsInvoiceBaseObjects.InvoiceDetail>();
            if (invoiceId == null!)
            {
                _invoiceDetail.externalInvoiceId = "AutomationRun";
            }
            else
            {
                _invoiceDetail.externalInvoiceId = invoiceId;
            }
            _createPaymentsInvoiceBaseObjects.invoiceDetails.Add(_invoiceDetail);
            var response = await _httpSender.SendPostHttpsRequestAsync(_envConfig.PaymentsUrl + CreatePaymentInvoiceEndPoint,
                _createPaymentsInvoiceBaseObjects, requestHeader);
            Console.WriteLine("Done with SendPostRequestCreatePaymentInvoice\n");
            return response;
        }
        catch (Exception exception)
        {
            Console.WriteLine("Error in SendPostRequestCreatePaymentInvoice\n" + exception + "\n");
            throw;
        }
    }
}