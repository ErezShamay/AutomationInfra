using Splitit.Automation.NG.Backend.BaseActions;
using Splitit.Automation.NG.Backend.Services.V3.DefaultValues;
using Splitit.Automation.NG.PaymentsSystem.PaymentsOperations.BaseObjects;
using Splitit.Automation.NG.Utilities.EnvironmentsConfig;

namespace Splitit.Automation.NG.PaymentsSystem.PaymentsOperations.EndPointFunctionality;

public class Invoiced
{
    private const string InvoicedEndPoint= "/api/payments/invoiced";
    private readonly HttpSender _httpSender = new();
    private readonly InvoicedBaseObjects.Root _invoicedBaseObjects = new();
    private readonly InvoicedBaseObjects.Payment _payment = new();
    private readonly EnvConfig _envConfig = new();

    public async Task<string> SendPutRequestInvoicedAsync(
        RequestHeader requestHeader, string paymentRecordId, string invoicedId
        , string variableFee = default!, string fixedFee = default!)
    {
        try
        {
            Console.WriteLine("\nStarting SendPutRequestInvoiced");
            _invoicedBaseObjects.payments = new List<InvoicedBaseObjects.Payment>();
            _payment.paymentRecordId = paymentRecordId;
            _payment.inoviceId = invoicedId;
            if (fixedFee != null!)
            {
                var invoiceFees = new InvoicedBaseObjects.InvoiceFees
                {
                    variableFee = double.Parse(variableFee),
                    fixedFee = double.Parse(fixedFee)
                };
                _payment.invoiceFees = invoiceFees;
            }
            
            _invoicedBaseObjects.payments.Add(_payment);
            var response = await _httpSender.SendPutHttpsRequestAsync(_envConfig.PaymentsUrl + InvoicedEndPoint,
                _invoicedBaseObjects, requestHeader);
            Console.WriteLine("Done with SendPutRequestInvoiced\n");
            return response;
        }
        catch (Exception exception)
        {
            Console.WriteLine("Error in SendPutRequestInvoiced\n" + exception + "\n");
            throw;
        }
    }
}