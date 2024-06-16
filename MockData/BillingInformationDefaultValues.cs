namespace Splitit.Automation.NG.Backend.Services.Ams.MockData;

public class BillingInformationDefaultValues
{
    public string LegalBusinessName;
    public string RegisteredNumber;
    public string Subsidiary;
    public string VatGstNumber;
    public string BillingCurrency;
    public string MonetaryFlow;
    
    public BillingInformationDefaultValues()
    {
        var faker = new Bogus.Faker();
        LegalBusinessName = faker.Company.CompanyName();
        RegisteredNumber = faker.Random.String2(10);
        Subsidiary = "SplititUSAInc";
        VatGstNumber = faker.Random.String2(10);
        BillingCurrency = "AUD";
        MonetaryFlow = "MER";
    }
}