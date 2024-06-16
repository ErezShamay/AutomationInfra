
using Splitit.Automation.NG.Backend.Services.Ams.AccountManager.AccountManagerBaseObjects;

namespace Splitit.Automation.NG.Backend.Services.Ams.MockData;

public class FinancialInformationDefaultValues
{
    public BillingInformationDefaultValues BillingInformation;
    public DebitBankDefaultValues DebitBank;
    public CreditBankDefaultValues CreditBank;
    public FundingSetupDefaultValues FundingSetup;
    public List<PostAccountsBaseObjects.Contract> Contracts = new();


    public FinancialInformationDefaultValues()
    {
        var faker = new Bogus.Faker();
        BillingInformation = new BillingInformationDefaultValues();
        DebitBank = new DebitBankDefaultValues();
        CreditBank = new CreditBankDefaultValues();
        FundingSetup = new FundingSetupDefaultValues();
    }
}