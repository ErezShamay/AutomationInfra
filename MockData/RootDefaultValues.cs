using Splitit.Automation.NG.Backend.Services.Ams.AccountManager.AccountManagerBaseObjects;

namespace Splitit.Automation.NG.Backend.Services.Ams.MockData;

public class RootDefaultValues
{
    public readonly CompanyDefaultValues Company;
    public FinancialInformationDefaultValues FinancialInformation;
    public List<PostAccountsBaseObjects.Contact> Contacts;
    public RelatedEntitiesDefaultValues RelatedEntitiesDefaultValues;
    public CustomerErpIdDefaultValues CustomerErpIdDefaultValues;

    public RootDefaultValues()
    {
        Company = new CompanyDefaultValues();
        FinancialInformation = new FinancialInformationDefaultValues();
        Contacts = new List<PostAccountsBaseObjects.Contact>();
        RelatedEntitiesDefaultValues = new RelatedEntitiesDefaultValues();
        CustomerErpIdDefaultValues = new CustomerErpIdDefaultValues();
    }
}