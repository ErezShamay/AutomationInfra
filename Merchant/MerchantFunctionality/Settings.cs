using Newtonsoft.Json;
using NUnit.Framework;
using Splitit.Automation.NG.Backend.BaseActions;
using Splitit.Automation.NG.Backend.Services.AdminApi.BaseObjects.Merchant.response;
using Splitit.Automation.NG.Backend.Services.V3.DefaultValues;
using Splitit.Automation.NG.Utilities.EnvironmentsConfig;

namespace Splitit.Automation.NG.Backend.Services.AdminApi.Merchant.MerchantFunctionality;

public class Settings
{
    private const string SettingsEndPoint = "/api/merchant/settings";
    private readonly HttpSender _httpSender = new();
    private readonly EnvConfig _envConfig = new();

    public async Task<GetSettingsResponse.Root?> SendGetSettingsRequestAsync(
        RequestHeader requestHeader, int businessUnitId)
    {
        try
        {
            Console.WriteLine("\nStarting SendGetSettingsRequest");
            var endPoint = _envConfig.AdminUrl + SettingsEndPoint + "?BusinessUnitId=" + businessUnitId + "&IncludePaymentSettings=true";
            var response = await _httpSender.SendGetHttpsRequestAsync(endPoint, requestHeader);
            var jResponse = JsonConvert.DeserializeObject<GetSettingsResponse.Root>(response);
            Console.WriteLine("Done SendGetSettingsRequest\n");
            return jResponse;
        }
        catch (Exception exception)
        {
            Console.WriteLine("Failed SendGetSettingsRequest \n" + exception + "\n");
            return null;
        }
    }

    public bool ValidateMerchantPaymentSettings(GetSettingsResponse.Root jResponse, bool isFunded)
    {
        try
        {
            Console.WriteLine("\nStarting ValidateMerchantPaymentSettings");
            if (isFunded)
            {
                Console.WriteLine("Starting to validate CreditLine...");
                Assert.That(jResponse.MerchantSettings.PaymentSettings.CreditLine, Is.EqualTo(""));
                Console.WriteLine("Done Validating CreditLine!");
                Console.WriteLine("Starting to validate RiskRating");
                Assert.That(jResponse.MerchantSettings.PaymentSettings.RiskRating, Is.EqualTo(""));
                Console.WriteLine("Done validating RiskRating");
                Console.WriteLine("Starting to validate ReservePool");
                Assert.That(jResponse.MerchantSettings.PaymentSettings.ReservePool, Is.EqualTo(""));
                Console.WriteLine("Done validating ReservePool");
                Console.WriteLine("Starting to validate FundingTrigger");
                Assert.That(jResponse.MerchantSettings.PaymentSettings.FundingTrigger, Is.EqualTo(""));
                Console.WriteLine("Done validating FundingTrigger");
                Console.WriteLine("Starting to validate DebitOnHold");
                Assert.That(jResponse.MerchantSettings.PaymentSettings.DebitOnHold, Is.EqualTo(""));
                Console.WriteLine("Done validating DebitOnHold");
                Console.WriteLine("Starting to validate FundingOnHold");
                Assert.That(jResponse.MerchantSettings.PaymentSettings.FundingOnHold, Is.EqualTo(""));
                Console.WriteLine("Done validating FundingOnHold");
                Console.WriteLine("Starting to validate FundingEndDate");
                Assert.That(jResponse.MerchantSettings.PaymentSettings.FundingEndDate, Is.EqualTo(""));
                Console.WriteLine("Done validating FundingEndDate");
                Console.WriteLine("Starting to validate FundingStartDate");
                Assert.That(jResponse.MerchantSettings.PaymentSettings.FundingStartDate, Is.EqualTo(""));
                Console.WriteLine("Done validating FundingStartDate");
                Console.WriteLine("Starting to validate MonetaryFlow");
                Assert.That(jResponse.MerchantSettings.PaymentSettings.MonetaryFlow, Is.EqualTo(""));
                Console.WriteLine("Done validating MonetaryFlow\n");
            }
            else
            {
                Console.WriteLine("\nStarting to validate CreditLine...");
                Assert.That(jResponse.MerchantSettings.PaymentSettings.CreditLine, Is.EqualTo(0));
                Console.WriteLine("Done Validating CreditLine!");
                Console.WriteLine("Starting to validate RiskRating");
                Assert.That(jResponse.MerchantSettings.PaymentSettings.RiskRating, Is.Null);
                Console.WriteLine("Done validating RiskRating");
                Console.WriteLine("Starting to validate ReservePool");
                Assert.That(jResponse.MerchantSettings.PaymentSettings.ReservePool, Is.EqualTo(0));
                Console.WriteLine("Done validating ReservePool");
                Console.WriteLine("Starting to validate FundingTrigger");
                Assert.That(jResponse.MerchantSettings.PaymentSettings.FundingTrigger, Is.Null);
                Console.WriteLine("Done validating FundingTrigger");
                Console.WriteLine("Starting to validate DebitOnHold");
                Assert.That(jResponse.MerchantSettings.PaymentSettings.DebitOnHold, Is.EqualTo(false));
                Console.WriteLine("Done validating DebitOnHold");
                Console.WriteLine("Starting to validate FundingOnHold");
                Assert.That(jResponse.MerchantSettings.PaymentSettings.FundingOnHold, Is.EqualTo(false));
                Console.WriteLine("Done validating FundingOnHold");
                Console.WriteLine("Starting to validate FundingEndDate");
                Assert.That(jResponse.MerchantSettings.PaymentSettings.FundingEndDate, Is.Null);
                Console.WriteLine("Done validating FundingEndDate");
                Console.WriteLine("Starting to validate FundingStartDate");
                Assert.That(jResponse.MerchantSettings.PaymentSettings.FundingStartDate, Is.Null);
                Console.WriteLine("Done validating FundingStartDate");
                Console.WriteLine("Starting to validate MonetaryFlow");
                Assert.That(jResponse.MerchantSettings.PaymentSettings.MonetaryFlow, Is.Null);
                Console.WriteLine("Done validating MonetaryFlow");
            }
            Console.WriteLine("Done ValidateMerchantPaymentSettings\n");
            return true;
        }
        catch (Exception exception)
        {
            Console.WriteLine("Failed ValidateMerchantPaymentSettings \n" + exception + "\n");
            return false;
        }
    }
}