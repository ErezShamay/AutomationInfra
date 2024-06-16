using Splitit.Automation.NG.Backend.BaseActions;
using Splitit.Automation.NG.Backend.Services.V3.DefaultValues;
using Splitit.Automation.NG.PaymentsSystem.PaymentsOperations.BaseObjects;
using Splitit.Automation.NG.Utilities.EnvironmentsConfig;

namespace Splitit.Automation.NG.PaymentsSystem.PaymentsOperations.EndPointFunctionality;

public class Created
{
    private const string CreatedEndPoint= "/api/payments/created";
    private readonly CreatedBaseObjects.Root _createdBaseObjects = new();
    private readonly CreatedBaseObjects.Record _record = new();
    private readonly HttpSender _httpSender = new();
    private readonly EnvConfig _envConfig = new();

    public async Task<string> SendPutRequestCreatedAsync(RequestHeader requestHeader, string id)
    {
        try
        {
            Console.WriteLine("\nStarting SendPutRequestCreated");
            var batchId = new Random().Next(10000, 1000000000);
            _createdBaseObjects.batch_id = batchId.ToString();
            _createdBaseObjects.records = new List<CreatedBaseObjects.Record>();
            _record.success = true;
            _record.internal_id = id;
            _createdBaseObjects.records.Add(_record);
            var response = await _httpSender.SendPutHttpsRequestAsync(_envConfig.PaymentsUrl + CreatedEndPoint,
                _createdBaseObjects, requestHeader);
            Console.WriteLine("Done with SendPutRequestCreated\n");
            await Task.Delay(3 * 1000);   
            return response;
        }
        catch (Exception exception)
        {
            Console.WriteLine("Error in SendPutRequestCreated\n" + exception + "\n");
            throw;
        }
    }
}