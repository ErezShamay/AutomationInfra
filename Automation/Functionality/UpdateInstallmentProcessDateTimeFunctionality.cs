using Splitit.Automation.NG.Backend.BaseActions;
using Splitit.Automation.NG.Backend.Services.AdminApi.Automation.BaseObjects;
using Splitit.Automation.NG.Backend.Services.V3.DefaultValues;
using Splitit.Automation.NG.Utilities.EnvironmentsConfig;

namespace Splitit.Automation.NG.Backend.Services.AdminApi.Automation.Functionality;

public class UpdateInstallmentProcessDateTimeFunctionality
{
    private const string EndPoint = "/api/Automation/update-installment-processdatetime";
    private readonly HttpSender _httpSender = new();
    private readonly EnvConfig _envConfig = new();
    private readonly UpdateInstallmentProcessDateTimeBaseObjects.Root _updateInstallmentProcessDateTimeBaseObjects = new();

    public async Task<string> SendPostRequestUpdateInstallmentProcessDateTimeAsync(
        RequestHeader requestHeader, int installmentId, DateTime newProcessedDateTime)
    {
        try
        {
            Console.WriteLine("\nStarting SendPostRequestUpdateInstallmentProcessDateTime");
            _updateInstallmentProcessDateTimeBaseObjects.InstallmentId = installmentId;
            _updateInstallmentProcessDateTimeBaseObjects.newProcessedDateTime = newProcessedDateTime;
            var response = await _httpSender.SendPostHttpsRequestAsync(
                _envConfig.AdminUrl + EndPoint,
                _updateInstallmentProcessDateTimeBaseObjects, requestHeader);
            Console.WriteLine("Done with SendPostRequestForMerchantReportsInsert\n");
            return response;
        }
        catch (Exception exception)
        {
            Console.WriteLine("Error in SendPostRequestUpdateInstallmentProcessDateTime\n" + exception + "\n");
            throw;
        }
    }
}