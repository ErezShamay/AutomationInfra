using Newtonsoft.Json;
using Splitit.Automation.NG.Backend.BaseActions;
using Splitit.Automation.NG.Backend.Services.AdminApi.InstallmentPlan.InstallmentPlanBaseObjects;
using Splitit.Automation.NG.Backend.Services.AdminApi.InstallmentPlan.InstallmentPlanResponses;
using Splitit.Automation.NG.Backend.Services.V3.DefaultValues;
using Splitit.Automation.NG.Utilities.EnvironmentsConfig;

namespace Splitit.Automation.NG.Backend.Services.AdminApi.InstallmentPlan.InstallmentsPlanFunctionality;

public class InstallmentPlanNumberCancel
{
    private const string CancelPlanEndPoint = "/api/installment-plan/cancel";
    private readonly HttpSender _httpSender = new();
    private readonly EnvConfig _envConfig = new();

    private static Cancel.Root BuildCancelPlanObject(
        string installmentPlanNumber, int cancellationReason, string refundUnderCancellation)
    {
        try
        {
            Console.WriteLine("ֿ\nStarting BuildCancelPlanObject");
            var cancelRequest = new Cancel.Root
            {
                InstallmentPlanNumber = installmentPlanNumber,
                RefundUnderCancelation = refundUnderCancellation,
                CancelationReason = cancellationReason
            };
            Console.WriteLine("BuildCancelPlanObject done\n");
            return cancelRequest;
        }
        catch (Exception exception)
        {
            Console.WriteLine("BuildCancelPlanObject Failed\n" + exception + "\n");
            throw;
        }
    }
    
    public async Task<ResponseCancel.Root> SendCancelPlanRequestAsync(RequestHeader requestHeader,
        string installmentPlanNumber, int cancellationReason, string refundUnderCancellation)
    {
        try
        {
            Console.WriteLine("\nStarting SendCancelPlanRequest");
            await Task.Delay(10 * 1000);
            var cancelObject = BuildCancelPlanObject(installmentPlanNumber, cancellationReason, refundUnderCancellation);
            var endPoint = _envConfig.AdminUrl + CancelPlanEndPoint;
            var response = await _httpSender.SendPostHttpsRequestAsync(endPoint, cancelObject, requestHeader);
            var responseCancel = JsonConvert.DeserializeObject<ResponseCancel.Root>(response);
            Console.WriteLine("SendCancelPlanRequest succeeded\n");
            return responseCancel!;
        }
        catch (Exception exception)
        {
            Console.WriteLine("SendExtendedParamsRequest Failed\n" + exception + "\n");
            throw;
        }
    }
}