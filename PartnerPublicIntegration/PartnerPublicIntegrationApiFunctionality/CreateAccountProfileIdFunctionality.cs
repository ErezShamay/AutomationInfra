using Newtonsoft.Json;
using Splitit.Automation.NG.Backend.BaseActions;
using Splitit.Automation.NG.Backend.Services.Ams.PartnerPublicIntegration.PartnerPublicIntegrationBaseObjects;
using Splitit.Automation.NG.Backend.Services.Ams.PartnerPublicIntegration.PartnerPublicIntegrationResponses;
using Splitit.Automation.NG.Backend.Services.V3.DefaultValues;
using Splitit.Automation.NG.Utilities.EnvironmentsConfig;

namespace Splitit.Automation.NG.Backend.Services.Ams.PartnerPublicIntegration.PartnerPublicIntegrationApiFunctionality;

public class CreateAccountProfileIdFunctionality
{
    private const string StructureEndPoint = "/api/v1/partner-profiles/create-account";
    private readonly HttpSender _httpSender = new();
    private readonly EnvConfig _envConfig = new();
    
    public async Task<CreateAccountProfileIdResponse.Root> SendPostRequestForCreateAccountProfileIdFunctionality(
        CreateAccountProfileIdBaseObjects.Root createAccountProfileIdBaseObjects, string profileId, RequestHeader requestHeader)
    {
        try
        {
            Console.WriteLine("\nStarting SendPostRequestForCreateAccountProfileIdFunctionality");
            var response = await _httpSender.SendPostHttpsRequestAsync(_envConfig.Ams + StructureEndPoint + "/" + profileId, 
                createAccountProfileIdBaseObjects, requestHeader);
            var jResponse = JsonConvert.DeserializeObject<CreateAccountProfileIdResponse.Root>(response);
            Console.WriteLine("Done with SendPostRequestForCreateAccountProfileIdFunctionality\n");
            return jResponse!;
        }
        catch (Exception exception)
        {
            Console.WriteLine("Error in SendPostRequestForCreateAccountProfileIdFunctionality \n" + exception + "\n");
            throw;
        }
    }
}