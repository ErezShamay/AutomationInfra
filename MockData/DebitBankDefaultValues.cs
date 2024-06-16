namespace Splitit.Automation.NG.Backend.Services.Ams.MockData;

public class DebitBankDefaultValues
{
    public string SettlementChannel;
    public string BankAccountType;
    public string BankAccountNumber;
    public string BankAccountName;
    public string BSBCode;
    public string GoCardlessMandateId;
    public string GoCardlessCustomerId;
    public string GoCardlessGivenName;
    public string GoCardlessFamilyName;
    public string GoCardlessCompanyName;
    public string GoCardlessEmail;
    public string BankNumber;
    
    public DebitBankDefaultValues()
    {
        var faker = new Bogus.Faker();
        SettlementChannel = "ABA";
        BankAccountType = "Savings";
        BankAccountNumber = faker.Random.String2(10);
        BankAccountName = faker.Name.FullName();
        BSBCode = faker.Random.String2(10);
        GoCardlessMandateId = faker.Random.String2(10);
        GoCardlessCustomerId = faker.Random.String2(10);
        GoCardlessGivenName = faker.Name.FirstName();
        GoCardlessFamilyName = faker.Name.LastName();
        GoCardlessCompanyName = faker.Company.CompanyName();
        GoCardlessEmail = faker.Internet.Email();
        BankNumber = faker.Random.String2(10);
    }
}