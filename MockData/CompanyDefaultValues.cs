using Splitit.Automation.NG.Backend.Services.Ams.AccountManager.AccountManagerBaseObjects;

namespace Splitit.Automation.NG.Backend.Services.Ams.MockData;

public class CompanyDefaultValues
{
    public string AccountName;
    public string AccountPhone;
    public string AccountEmail;
    public List<PostAccountsBaseObjects.CompanyAddress> CompanyAddress = new();
    public CompanyAddressDefaultValues CompanyAddres;
    public string Website;
    
    public CompanyDefaultValues()
    {
        var faker = new Bogus.Faker();
        {
            AccountName = faker.Company.CompanyName();
            AccountPhone = faker.Phone.PhoneNumber();
            AccountEmail = faker.Internet.Email();
            Website = faker.Internet.Url();
        }
        CompanyAddres = new CompanyAddressDefaultValues();
    }
}