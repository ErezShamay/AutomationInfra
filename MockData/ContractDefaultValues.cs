using Splitit.Automation.NG.Backend.Services.Ams.AccountManager.AccountManagerBaseObjects;

namespace Splitit.Automation.NG.Backend.Services.Ams.MockData;

public class ContractDefaultValues
{
    public string Name;
    public DateTime? StartDate;
    public DateTime EndDate;
    public List<PostAccountsBaseObjects.Pricing> Pricings = new();
    public PricingDefaultValues Pricing;
    public string SubscriptionERP_Id;
    
    public ContractDefaultValues()
    {
        var faker = new Bogus.Faker();
        Name = faker.Random.String2(10);
        StartDate = DateTime.Now;
        EndDate = DateTime.Now.AddYears(1);
        SubscriptionERP_Id = null!;
        Pricing = new PricingDefaultValues();
    }
}