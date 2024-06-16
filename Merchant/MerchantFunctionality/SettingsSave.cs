using Newtonsoft.Json;
using NUnit.Framework;
using Splitit.Automation.NG.Backend.BaseActions;
using Splitit.Automation.NG.Backend.Services.AdminApi.BaseObjects.Merchant.response;
using Splitit.Automation.NG.Backend.Services.AdminApi.Merchant.MerchantResponses;
using Splitit.Automation.NG.Backend.Services.V3.DefaultValues;
using Splitit.Automation.NG.PaymentsSystem.Sku.SkuMatching.Tests.SkuTestsData;
using Splitit.Automation.NG.Utilities.EnvironmentsConfig;

namespace Splitit.Automation.NG.Backend.Services.AdminApi.Merchant.MerchantFunctionality;

public class SettingsSave
{
    private const string SettingsSaveEndPoint = "/api/merchant/settings/save";
    private readonly HttpSender _httpSender = new();
    private readonly Settings _settings = new();
    private readonly EnvConfig _envConfig = new();

    public async Task<GetSettingsResponse.Root?> CreateMerchantSettingsUpdateAsync(
        RequestHeader requestHeader, int businessUnitId, bool allowMultipleCaptureFlag, string defaultPlanStrategy, 
        int deferPlanStartByDays, int minInstallments, int maxInstallments, string paymentFormTouchPointCode, bool attempt3DsByDefault, 
        bool isSku, bool allowInvalidCcCardBrandNotSupported, bool allowInvalidCcCardTypeNotSupported,
        double allowMoveToNonSecureForErrorsMaxAllowedAmount, int allowMoveToNonSecureForErrorsMaxAllowedInstallments,
        double nonSecurePlanMaxAllowedDebitAmount, int nonSecurePlanMaxAllowedDebitInstallments,
        string scheduleInterval = null! ,SkuTestsData.PaymentSettings paymentSettings = null!)
    {
        try
        {
            Console.WriteLine("\nStarting CreateMerchantSettingsUpdate");
            var jResponse = await _settings.SendGetSettingsRequestAsync(requestHeader, businessUnitId);
            jResponse!.MerchantSettings.AllowMultipleCapture = allowMultipleCaptureFlag;
            jResponse.MerchantSettings.DefaultPlanStrategy = defaultPlanStrategy;
            jResponse.MerchantSettings.DeferPlanStartByDays = deferPlanStartByDays;
            jResponse.MerchantSettings.MinInstallments = minInstallments; 
            jResponse.MerchantSettings.MaxInstallments = maxInstallments;
            jResponse.MerchantSettings.RunCaptureAsync = false;
            jResponse.MerchantSettings.PaymentFormTouchPointCode = paymentFormTouchPointCode;
            jResponse.MerchantSettings.Attempt3DsbyDefault = attempt3DsByDefault;
            jResponse.MerchantSettings.ScheduleInterval = scheduleInterval;
            jResponse.MerchantSettings.EnableFraudTool = false;
            jResponse.MerchantSettings.NonSecurePlanMaxAllowedDebitAmount = nonSecurePlanMaxAllowedDebitAmount;
            jResponse.MerchantSettings.NonSecurePlanMaxAllowedDebitInstallments = nonSecurePlanMaxAllowedDebitInstallments;
            jResponse.MerchantSettings.PaymentSettings = new GetSettingsResponse.PaymentSettings();
            jResponse.MerchantSettings.MoveToNonSecure = new GetSettingsResponse.MoveToNonSecure
                {
                    AllowInvalidCCCardBrandNotSupported = allowInvalidCcCardBrandNotSupported,
                    AllowInvalidCCCardTypeNotSupported = allowInvalidCcCardTypeNotSupported,
                    AllowMoveToNonSecureForErrorsMaxAllowedAmount = allowMoveToNonSecureForErrorsMaxAllowedAmount,
                    AllowMoveToNonSecureForErrorsMaxAllowedInstallments = allowMoveToNonSecureForErrorsMaxAllowedInstallments
                };
            jResponse.MerchantSettings.PaymentSettings = isSku ? SkuPopulatedPaymentSettingsObject(jResponse.MerchantSettings.PaymentSettings, paymentSettings) : PopulatedPaymentSettingsObject(jResponse.MerchantSettings.PaymentSettings);
            Console.WriteLine("Done CreateMerchantSettingsUpdate\n ");
            return jResponse;
        }
        catch (Exception exception)
        {
            Console.WriteLine("Failed CreateMerchantSettingsUpdate \n" + exception + "\n");
            return null;
        }
    }

    private GetSettingsResponse.PaymentSettings SkuPopulatedPaymentSettingsObject(
        GetSettingsResponse.PaymentSettings merchantSettingsPaymentSettings,
        SkuTestsData.PaymentSettings paymentSettingsMap)
    {
        try
        {
            Console.WriteLine("\nStarting SkuPopulatedPaymentSettingsObject");
            var paymentSettingsDict = paymentSettingsMap;
            merchantSettingsPaymentSettings.CreditLine = paymentSettingsDict.CreditLine;
            merchantSettingsPaymentSettings.RiskRating = paymentSettingsDict.RiskRating;
            merchantSettingsPaymentSettings.ReservePool = paymentSettingsDict.ReservePool;
            merchantSettingsPaymentSettings.FundingTrigger = paymentSettingsDict.FundingTrigger;
            merchantSettingsPaymentSettings.DebitOnHold = paymentSettingsDict.DebitOnHold;
            merchantSettingsPaymentSettings.FundingOnHold = paymentSettingsDict.FundingOnHold;
            merchantSettingsPaymentSettings.FundingEndDate = paymentSettingsDict.FundingEndDate;
            merchantSettingsPaymentSettings.FundingStartDate = paymentSettingsDict.FundingStartDate;
            merchantSettingsPaymentSettings.MonetaryFlow = paymentSettingsDict.MonetaryFlow;
            merchantSettingsPaymentSettings.IsActive = paymentSettingsDict.IsActive;
            merchantSettingsPaymentSettings.FundNonSecuredPlans = paymentSettingsDict.FundNonSecuredPlans;
            merchantSettingsPaymentSettings.SettlementType = paymentSettingsDict.SettlementType;
            Console.WriteLine("Done SkuPopulatedPaymentSettingsObject\n");
            return merchantSettingsPaymentSettings;
        }
        catch (Exception exception)
        {
            Console.WriteLine("Failed SkuPopulatedPaymentSettingsObject \n" + exception + "\n");
            return merchantSettingsPaymentSettings;
        }
    }

    private GetSettingsResponse.PaymentSettings PopulatedPaymentSettingsObject(
        GetSettingsResponse.PaymentSettings merchantSettingsPaymentSettings)
    {
        try
        {
            Console.WriteLine("\nStarting PopulatedPaymentSettingsObject");
            merchantSettingsPaymentSettings.CreditLine = -1;
            merchantSettingsPaymentSettings.RiskRating = null!;
            merchantSettingsPaymentSettings.ReservePool = null!;
            merchantSettingsPaymentSettings.FundingTrigger = null!;
            merchantSettingsPaymentSettings.DebitOnHold = null!;
            merchantSettingsPaymentSettings.FundingOnHold = null!;
            merchantSettingsPaymentSettings.FundingEndDate = null!;
            merchantSettingsPaymentSettings.FundingStartDate = null!;
            merchantSettingsPaymentSettings.MonetaryFlow = null!;
            Console.WriteLine("Done PopulatedPaymentSettingsObject\n");
            return merchantSettingsPaymentSettings;
        }
        catch (Exception exception)
        {
            Console.WriteLine("Error in PopulatedPaymentSettingsObject \n" + exception + "\n");
            return merchantSettingsPaymentSettings;
        }
    }

    public async Task<ResponseSettingsSave.Root> SendUpdateSettingsRequestAsync(
        RequestHeader requestHeader, object obj)
    {
        try
        {
            Console.WriteLine("\nStarting SendUpdateSettingsRequest");
            var endPoint = _envConfig.AdminUrl + SettingsSaveEndPoint;
            var response = await _httpSender.SendPostHttpsRequestAsync(endPoint, obj, requestHeader);
            var jResponse = JsonConvert.DeserializeObject<ResponseSettingsSave.Root>(response);
            Console.WriteLine("Done SendUpdateSettingsRequest\n");
            return jResponse!;
        }
        catch (Exception exception)
        {
            Console.WriteLine("Failed SendUpdateSettingsRequest \n" + exception + "\n");
            return null!;
        }
    }

    public bool ValidateSendUpdateSettingsRequest(ResponseSettingsSave.Root jResponse)
    {
        try
        {
            Console.WriteLine("\nStarting ValidateSendUpdateSettingsRequest");
            Assert.That(jResponse.MerchantSettings.AllowMultipleCapture);
            Console.WriteLine("Done ValidateSendUpdateSettingsRequest\n");
            return true;
        }
        catch (Exception exception)
        {
            Console.WriteLine("Failed ValidateSendUpdateSettingsRequest \n" + exception + "\n");
            return false;
        }
    }
}