using Splitit.Automation.NG.Backend.Services.Ams.AccountManager.AccountManagerBaseObjects;

namespace Splitit.Automation.NG.Backend.Services.Ams.MockData;

public class MockDataHandler
{
    private readonly RootDefaultValues _rootDefaultValues = new();
    private readonly PostAccountsBaseObjects.CompanyAddress _compAdd = new();
    private readonly PostAccountsBaseObjects.Pricing _pricing = new();
    private readonly PostAccountsBaseObjects.Contact _contact = new();
    private readonly ContactDefaultValues _contactDefaultValues = new();
    private readonly ContractDefaultValues _contractDefaultValues = new();
    private readonly PostAccountsBaseObjects.Contract _conrtact = new();

    public RootDefaultValues GetMockData()
    {
        _compAdd.Country = _rootDefaultValues.Company.CompanyAddres.Country;
        _compAdd.Street = _rootDefaultValues.Company.CompanyAddres.Street;
        _compAdd.City = _rootDefaultValues.Company.CompanyAddres.City;
        _compAdd.State = _rootDefaultValues.Company.CompanyAddres.State;
        _compAdd.PostalCode = _rootDefaultValues.Company.CompanyAddres.PostalCode;
        _compAdd.Type = _rootDefaultValues.Company.CompanyAddres.Country;
        
        _rootDefaultValues.Company.CompanyAddress.Add(_compAdd);

        _pricing.SKU = _contractDefaultValues.Pricing.SKU;
        _pricing.TransactionFeePercentage = _contractDefaultValues.Pricing.TransactionFeePercentage;
        _pricing.TransactionFixedFee = _contractDefaultValues.Pricing.TransactionFixedFee;
        _pricing.ChargebackFee = _contractDefaultValues.Pricing.ChargebackFee;

        _conrtact.Name = _contractDefaultValues.Name;
        _conrtact.StartDate = _contractDefaultValues.StartDate;
        _conrtact.EndDate = _contractDefaultValues.EndDate;
        _conrtact.SubscriptionERP_Id = _contractDefaultValues.SubscriptionERP_Id;
        _conrtact.Pricings = _contractDefaultValues.Pricings;
        _conrtact.Pricings.Add(_pricing);
        
        _rootDefaultValues.FinancialInformation.Contracts.Add(_conrtact);

        _contact.Type = _contactDefaultValues.Type;
        _contact.Email = _contactDefaultValues.Email;
        _contact.FullName = _contactDefaultValues.FullName;
        
        _rootDefaultValues.Contacts.Add(_contact);
        
        return _rootDefaultValues;
    }
}