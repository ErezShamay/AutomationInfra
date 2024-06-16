namespace Splitit.Automation.NG.Backend.Services.Ams.MockData;

public class ContactDefaultValues
{
    public string Type;
    public string Email;
    public string FullName;
    
    public ContactDefaultValues()
    {
        var faker = new Bogus.Faker();
        Type = "Financial";
        Email = faker.Internet.Email();
        FullName = faker.Name.FullName();
    }
}