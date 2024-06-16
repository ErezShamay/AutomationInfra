namespace Splitit.Automation.NG.Backend.Services.V3.DefaultValues;

public class ShopperDefaultValues
{
    public string culture;
    public string email;
    public string fullName;
    public string phoneNumber;

    public ShopperDefaultValues()
    {
        var random = new Random();
        var faker = new Bogus.Faker();
        var num = random.NextInt64(2, 10);
        fullName = faker.Name.FullName();
        email = "Splitit.Automation+" + num + "@splitit.com";
        phoneNumber = "0501234567";
        culture = "en-US";
    }
}