namespace Splitit.Automation.NG.Backend.Services.V3.DefaultValues;

public class BillingAddressDefaultValues
{
    public string addressLine1;
    public string addressLine2;
    public string city;
    public string country;
    public string state;
    public string zip;

    public BillingAddressDefaultValues()
    {
        var faker = new Bogus.Faker();
        addressLine1 = faker.Address.StreetAddress();
        addressLine2 = faker.Address.SecondaryAddress();
        city = faker.Address.City();
        country = "US";
        state = faker.Address.State();
        zip = faker.Address.ZipCode();
    }
}