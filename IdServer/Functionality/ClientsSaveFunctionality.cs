using Newtonsoft.Json;
using Splitit.Automation.NG.Backend.BaseActions;
using Splitit.Automation.NG.Backend.Services.IdServer.requests;
using Splitit.Automation.NG.Backend.Services.IdServer.responses;
using Splitit.Automation.NG.Backend.Services.V3.DefaultValues;
using Splitit.Automation.NG.Utilities.EnvironmentsConfig;

namespace Splitit.Automation.NG.Backend.Services.IdServer.Functionality;

public class ClientsSaveFunctionality
{
    private const string ClientsSaveEndPoint = "/api/clients/save";
    private readonly HttpSender _httpSender = new();
    private readonly EnvConfig _envConfig = new();

    public async Task<ResponseClientSave.Root> SendClientsSaveRequestAsync(RequestHeader requestHeader, object obj)
    {
        try
        {
            Console.WriteLine("\nStarting SendClientsSaveRequest");
            var endPoint = _envConfig.AccessTokenURI + ClientsSaveEndPoint;
            var response = await _httpSender.SendPostHttpsRequestAsync(endPoint, obj, requestHeader);
            var jResponse = JsonConvert.DeserializeObject<ResponseClientSave.Root>(response);
            Console.WriteLine("Done SendClientsSaveRequest\n");
            return jResponse!;
        }
        catch (Exception exception)
        {
            Console.WriteLine("Error in SendClientsSaveRequest \n" + exception + "\n");
            await SendClientsSaveRequestAsync(requestHeader, obj);
            throw;
        }
    }

    public RequestClientsSave.Root CreateClientsSaveObject(string clientId, string? kesFlag, string adminFlag)
    {
        try
        {
            Console.WriteLine("\nStarting CreateClientsSaveObject");
            var createClientSaveObject = new RequestClientsSave.Root
            {
                Client = new RequestClientsSave.Client
                {
                    AllowedGrantTypes = new List<string>(),
                    AllowedScopes = new List<string>(),
                    Permissions = new List<string>(),
                    Roles = new List<string>(),
                    RedirectUris = new List<string>(),
                    AccessTokenLifetime = 3600,
                    AccessTokenType = "Reference",
                    AllowAccessTokenSlidingExpiration = true,
                    AllowOfflineAccess = false
                }
            };
            if (kesFlag != null)
            {
                createClientSaveObject.Client.AllowedGrantTypes.Add("client_key_pair_auth");
                createClientSaveObject.Client.RequireClientSecret = false;
            }
            else
            {
                createClientSaveObject.Client.AllowedGrantTypes.Add("client_credentials");
                createClientSaveObject.Client.RequireClientSecret = true;
            }
            createClientSaveObject.Client.AllowedScopes.Add("api.v2");
            createClientSaveObject.Client.AllowedScopes.Add("api.v1");
            createClientSaveObject.Client.AllowedScopes.Add("api.v3");
            createClientSaveObject.Client.AllowedScopes.Add("keyexchange.api");
            createClientSaveObject.Client.AllowedScopes.Add("disputes.api");
            createClientSaveObject.Client.ClientId = clientId;
            createClientSaveObject.Client.IsBehalfOfAllowed = false;
            createClientSaveObject.Client.Permissions.Add("Plan.MerchantOps");
            createClientSaveObject.Client.Permissions.Add("KeyExchangeServer.Manage");
            createClientSaveObject.Client.Permissions.Add("KeyExchangeServer.Manage.All");
            createClientSaveObject.Client.Permissions.Add("Plan.MerchantOps");
            createClientSaveObject.Client.Permissions.Add("Disputes.Get");
            createClientSaveObject.Client.Permissions.Add("Disputes.Manage");
            if (adminFlag == null!)
            {
                createClientSaveObject.Client.Roles.Add("Admin");
            }
            createClientSaveObject.Client.SlidingAccessTokenExtension = 1800;
            Console.WriteLine("Done CreateClientsSaveObject\n");
            return createClientSaveObject;
        }
        catch (Exception exception)
        {
            Console.WriteLine("CreateClientsSaveObject \n" + exception + "\n");
            throw;
        }
    }
}