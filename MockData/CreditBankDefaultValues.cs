namespace Splitit.Automation.NG.Backend.Services.Ams.MockData;

public class CreditBankDefaultValues
{
    public string SettlementChannel;
    public string BankAccountType;
    public string BankAccountNumber;
    public string BankAccountName;
    public string BSBCode;
    public string TransitNumber;
    public string BankCode;
    public string SwiftCode;
    public string IBAN;
    public string BankNumber;
    
    public CreditBankDefaultValues()
    {
        var faker = new Bogus.Faker();
        SettlementChannel = "ABA";
        BankAccountType = "Savings";
        BankAccountNumber = faker.Random.String2(10);
        BankAccountName = faker.Name.FullName();
        BSBCode = faker.Random.String2(10);
        TransitNumber = faker.Random.String2(10);
        BankCode = faker.Random.String2(10);
        SwiftCode = faker.Random.String2(10);
        IBAN = faker.Random.String2(10);
        BankNumber = faker.Random.String2(10);
    }
}