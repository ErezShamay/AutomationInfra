using Newtonsoft.Json;
using Splitit.Automation.NG.Backend.BaseActions;
using Splitit.Automation.NG.Backend.Services.AdminApi.Merchant.MerchantBaseObjects;
using Splitit.Automation.NG.Backend.Services.AdminApi.Merchant.MerchantResponses;
using Splitit.Automation.NG.Backend.Services.V3.DefaultValues;
using Splitit.Automation.NG.Utilities.EnvironmentsConfig;

namespace Splitit.Automation.NG.Backend.Services.AdminApi.Merchant.MerchantFunctionality;

public class UsersCreateFunctionality
{
    private const string EndPoint = "/api/merchant/users/create";
    private readonly HttpSender _httpSender = new();
    private UsersCreate.Root _root = new();
    private readonly EnvConfig _envConfig = new();

    public async Task<(ResponseUsersCreate.Root, UsersCreate.Root _root)> SendPostRequestForUsersCreateAsync(
        RequestHeader requestHeader)
    {
        try
        {
            Console.WriteLine("\nStarting SendPostRequestForUsersCreateAsync");
            _root = PopulateUsersCreateBaseObjects(_root);
            var response = await _httpSender.SendPostHttpsRequestAsync(
                _envConfig.AdminUrl + EndPoint,
                _root, requestHeader);
            var jResponse = JsonConvert.DeserializeObject<ResponseUsersCreate.Root>(response);
            Console.WriteLine("Done with SendPostRequestForUsersCreateAsync\n");
            return (jResponse!, _root);
        }
        catch (Exception exception)
        {
            Console.WriteLine("Error in SendPostRequestForUsersCreateAsync\n" + exception);
            throw;
        }
    }

    private UsersCreate.Root PopulateUsersCreateBaseObjects(UsersCreate.Root root)
    {
        var faker = new Bogus.Faker();
        root.MerchantId = int.Parse(_envConfig.MerchantId);
        root.BusinessUnitId = int.Parse(_envConfig.MerchantBusinessUnitId);
        root.Roles = new List<string> { "Merchant.AccountOwner" };
        root.User = new UsersCreate.User
        {
            Id = null!,
            UserName = faker.Internet.Email(),
            FullName = faker.Name.FullName()
        };
        root.User.Email = root.User.UserName;
        root.User.PhoneNumber = faker.Phone.PhoneNumber();
        root.User.CultureName = "en-US";
        root.User.IsLocked = false;
        root.User.IsDataPrivateRestricted = false;
        return root;
    }
}