using Newtonsoft.Json;
using NUnit.Framework;
using Splitit.Automation.NG.Backend.BaseActions;
using Splitit.Automation.NG.Backend.Services.AdminApi.BaseObjects.Merchant.requests;
using Splitit.Automation.NG.Backend.Services.AdminApi.BaseObjects.Merchant.response;
using Splitit.Automation.NG.Backend.Services.AdminApi.Terminal;
using Splitit.Automation.NG.Backend.Services.IdServer.Functionality;
using Splitit.Automation.NG.Backend.Services.V3.DefaultValues;
using Splitit.Automation.NG.PaymentsSystem.Sku.SkuMatching.Tests.SkuTestsData;
using Splitit.Automation.NG.Utilities.EnvironmentsConfig;

namespace Splitit.Automation.NG.Backend.Services.AdminApi.Merchant.MerchantFunctionality
{
    public class Create
    {
        private const string CreateMerchantEndPoint = "/api/merchant/create";
        private readonly HttpSender _httpSender = new();
        private readonly Save _saveTerminal = new();
        private readonly SettingsSave _settingsSave = new();
        private readonly ClientsSaveFunctionality _clientsSaveFunctionality = new();
        private readonly ApiCredentials _apiCredentials = new();
        private readonly EnvConfig _envConfig = new ();

        public async Task<(string, int, int, string, string)> CreateMerchantAndInfoAsync(RequestHeader requestHeader,
            string businessCode, bool isSku, int deferPlanStartByDays, int minInstallments, int maxInstallments, 
            string paymentFormTouchPointCode, bool allowMultipleCaptureFlag, string defaultPlanStrategy, bool attempt3DsByDefault,
            bool allowInvalidCcCardBrandNotSupported, bool allowInvalidCcCardTypeNotSupported, 
            double allowMoveToNonSecureForErrorsMaxAllowedAmount, int allowMoveToNonSecureForErrorsMaxAllowedInstallments,
            double nonSecurePlanMaxAllowedDebitAmount, int nonSecurePlanMaxAllowedDebitInstallments, 
            string scheduleInterval = null!, SkuTestsData.PaymentSettings paymentSettings = null!, 
            string gatewayId = null!, string? kesFlag = null!, string adminFlag = default!)
        {
            try
            {
                Console.WriteLine("\nStarting creation of a new merchant , settings and terminal process");
                Console.WriteLine("Starting creating merchant");
                var createMerchantObject = CreateMerchantObject(businessCode);
                var jCreateMerchantResponse = await SendCreateMerchantRequestAsync(requestHeader, createMerchantObject!);
                Console.WriteLine("MerchantId that was generated is: " + jCreateMerchantResponse!.MerchantId);
                var businessUnitId = int.Parse(jCreateMerchantResponse.BusinessUnitId);
                var merchantId = jCreateMerchantResponse.MerchantId;
                Assert.That(businessUnitId, Is.Not.EqualTo(0));
                Console.WriteLine("Done creating merchant, MerchantId -> " + merchantId);
                Console.WriteLine("Starting creating merchant settings");
                var merchantSettingsObject = await _settingsSave.CreateMerchantSettingsUpdateAsync(requestHeader, businessUnitId, allowMultipleCaptureFlag,
                    defaultPlanStrategy, deferPlanStartByDays, minInstallments, maxInstallments, paymentFormTouchPointCode, 
                    attempt3DsByDefault, isSku, allowInvalidCcCardBrandNotSupported, allowInvalidCcCardTypeNotSupported,
                    allowMoveToNonSecureForErrorsMaxAllowedAmount, allowMoveToNonSecureForErrorsMaxAllowedInstallments, 
                    nonSecurePlanMaxAllowedDebitAmount, nonSecurePlanMaxAllowedDebitInstallments,
                    scheduleInterval, paymentSettings);
                var jResponse = await _settingsSave.SendUpdateSettingsRequestAsync(requestHeader, merchantSettingsObject!);
                Assert.That(_settingsSave.ValidateSendUpdateSettingsRequest(jResponse), Is.True);
                Console.WriteLine("Done creating merchant settings");
                Console.WriteLine("Getting ClientId from api credentials");
                var jResponseApiCredentials = await _apiCredentials.SendGetRequestForApiCredentialsAsync(requestHeader, merchantId);
                var clientId = jResponseApiCredentials!.ApiUsers[0].ClientId;
                Console.WriteLine("Done Getting ClientId from api credentials");
                Console.WriteLine("Add permissions to merchant scope");
                var clientSaveObject = _clientsSaveFunctionality.CreateClientsSaveObject(clientId, kesFlag, adminFlag);
                var jClientsSaveResponse = await _clientsSaveFunctionality.SendClientsSaveRequestAsync(requestHeader, clientSaveObject);
                Assert.That(jClientsSaveResponse.Client.AllowedScopes, Does.Contain("api.v3"));
                Console.WriteLine("Done permissions to merchant scope");
                Console.WriteLine("Starting creating merchant terminal");
                var terminalApiKey = await _saveTerminal.CreateTerminalAndCredentialSettingsAsync(requestHeader, businessUnitId, int.Parse(gatewayId));
                Console.WriteLine("Done creation of new merchant , settings and terminal process\n");
                return (terminalApiKey, businessUnitId, int.Parse(merchantId), createMerchantObject!.BusinessLegalName, clientId);
            }
            catch (Exception exception)
            {
                Console.WriteLine("Error in CreateMerchantAndInfo \n" + exception + "\n");
                return ("", 0, 0, "", "");
            }
        }

        private CreateNewMerchant.Root? CreateMerchantObject(string businessCode)
        {
            try
            {
                Console.WriteLine("\nStarting creation of create merchant object");
                var createNewMerchantBaseObject = new CreateNewMerchant.Root();
                var guidName = GuidGenerator.GenerateNewGuid();
                createNewMerchantBaseObject.BusinessDBAName = "Automation-" + guidName;
                createNewMerchantBaseObject.BusinessLegalName = "Automation-" + guidName;
                createNewMerchantBaseObject.RegisteredCountryOfBusinessCode = businessCode;
                Console.WriteLine("Done Creation of merchant object\n");
                return createNewMerchantBaseObject;
            }
            catch (Exception exception)
            {
                Console.WriteLine("Error in CreateMerchantObject \n" + exception + "\n");
                return null;
            }
        }

        private async Task<CreateMerchantResponse.Root?> SendCreateMerchantRequestAsync(RequestHeader requestHeader, object obj)
        {
            try
            {
                Console.WriteLine("\nStarting sending https post for merchant creation");
                var endPoint = _envConfig.AdminUrl + CreateMerchantEndPoint;
                var response = await _httpSender.SendPostHttpsRequestAsync(endPoint, obj, requestHeader);
                var jResponse = JsonConvert.DeserializeObject<CreateMerchantResponse.Root>(response);
                Console.WriteLine("Done sending https post for merchant creation\n");
                return jResponse;
            }
            catch (Exception exception)
            {
                Console.WriteLine("Error in SendCreateMerchantRequest \n" + exception + "\n");
                return null;
            }
        }
    }
}