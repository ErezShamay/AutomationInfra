using Newtonsoft.Json;
using Splitit.Automation.NG.Backend.BaseActions;
using Splitit.Automation.NG.Backend.Services.V3.DefaultValues;
using Splitit.Automation.NG.SharedResources.Tests.TestsHelper;
using Splitit.Automation.NG.Utilities.EnvironmentsConfig;

namespace Splitit.Automation.NG.SharedResources.BaseActions;

public class BluesnapVaultedShopperController
{
    private readonly HttpSender _httpSender;
    private readonly EnvConfig _envConfig;

    public BluesnapVaultedShopperController()
    {
        _httpSender = new HttpSender();
        _envConfig = new EnvConfig();
    }

    public class CreateVaultedShopperRequest
    {
        public string FirstName;
        public string LastName;
        public string Phone;
        public PaymentSources PaymentSources;
	
        public CreateVaultedShopperRequest() {
            var faker = new Bogus.Faker();
            FirstName = faker.Name.FirstName();
            LastName = faker.Name.LastName();
            Phone = faker.Phone.PhoneNumber();
            PaymentSources = new PaymentSources();
        }
    }

    public class PaymentSources
    {
        public List<CreditCardInfo> CreditCardInfo;
        
        public PaymentSources()
        {
            CreditCardInfo = new List<CreditCardInfo> { new CreditCardInfo() };
        }
    }
    
    public class CreditCardInfo
    {
        public BillingContactInfo BillingContactInfo;
        public CreditCard CreditCard;
	
        public CreditCardInfo() {
            BillingContactInfo = new BillingContactInfo();
            CreditCard = new CreditCard();
        }
    }
    
    public class BillingContactInfo {
        public string FirstName;
        public string LastName;
        public string Email;
        public string Phone;
        public string Country;
        public string State;
        public string Address;
        public string City;
        public string Zip;
	
        public BillingContactInfo() {
            var faker = new Bogus.Faker();
            FirstName = faker.Name.FirstName();
            LastName = faker.Name.LastName();
            Email = "Splitit.automation+"+new Random().Next(1, 9)+"@gmail.com";
            Phone = faker.Phone.PhoneNumber();
            Country = "US";
            State = faker.Address.State();
            Address = faker.Address.StreetAddress();
            City = faker.Address.City();
            Zip = faker.Address.ZipCode();
        }
    }
    
    public class CreditCard {
        public string CardNumber;
        public string SecurityCode;
        public string ExpirationMonth;
        public string ExpirationYear;
	
        public CreditCard() {
            CardNumber = "4111111111111111";
            SecurityCode = "123";
            ExpirationMonth = "03";
            ExpirationYear = "26";
        }
    }
    
    string EncodingBlueSnapCredentials(string userName, string password) {
        var encodedBytes = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(userName + ":" + password));
        return new string(encodedBytes);
    }
    
    public async Task<VaultedShopperResponse.Root?> GetNewBluesnapToken(RequestHeader requestHeader)
    {
        try
        {
            Console.WriteLine("Starting GetNewBluesnapToken");
            var vaultedShopperRequest = new CreateVaultedShopperRequest();
            var auth = EncodingBlueSnapCredentials(
                _envConfig.MORBlueSnapUser, _envConfig.MORBlueSnapPassword);
            var response = await _httpSender.SendPostHttpsRequestAsync(
                _envConfig.BlueSnapValuetedShopper,
                vaultedShopperRequest, requestHeader, null, null,
                null, null!, null!, null!,
                null!, null!, "yes", auth);
            var jResponse = JsonConvert.DeserializeObject<VaultedShopperResponse.Root>(response);
            Console.WriteLine("Done with GetNewBluesnapToken");
            return jResponse;
        }
        catch (Exception e)
        {
            Console.WriteLine("Error in GetNewBluesnapToken" + e);
            throw;
        }
    }
}