using Newtonsoft.Json;
using Splitit.Automation.NG.Backend.BaseActions;
using Splitit.Automation.NG.Backend.Services.Ams.PartnerPublicIntegration.PartnerPublicIntegrationResponses;
using Splitit.Automation.NG.Backend.Services.V3.DefaultValues;
using Splitit.Automation.NG.Utilities.EnvironmentsConfig;

namespace Splitit.Automation.NG.Backend.Services.Ams.PartnerPublicIntegration.PartnerPublicIntegrationApiFunctionality;

public class StructureFunctionality
{
    private const string StructureEndPoint = "/api/v1/partner-profiles/structure";
    private readonly HttpSender _httpSender = new();
    private readonly EnvConfig _envConfig = new();
    
    public async Task<StructureResponse.Root> SendGetRequestForStructureFunctionality(RequestHeader requestHeader)
    {
        try
        {
            Console.WriteLine("\nStarting SendGetRequestForStructureFunctionality");
            var response = await _httpSender.SendGetHttpsRequestAsync(_envConfig.Ams + StructureEndPoint, requestHeader);
            var jResponse = JsonConvert.DeserializeObject<StructureResponse.Root>(response);
            Console.WriteLine("Done with SendGetRequestForStructureFunctionality\n");
            return jResponse!;
        }
        catch (Exception exception)
        {
            Console.WriteLine("Error in SendGetRequestForStructureFunctionality \n" + exception + "\n");
            throw;
        }
    }
}