namespace Splitit.Automation.NG.Backend.Services.Ams.MockData;

public class CompanyAddressDefaultValues
{
    public string Country;
    public string Street;
    public string City;
    public string State;
    public string PostalCode;
    public string Type;
    
    public CompanyAddressDefaultValues()
    {
        var faker = new Bogus.Faker();
        Country = faker.Address.Country();
        Street = faker.Address.StreetAddress();
        City = faker.Address.City();
        State = faker.Address.State();
        PostalCode = faker.Address.ZipCode();
        Type = "Billing";
    }
}