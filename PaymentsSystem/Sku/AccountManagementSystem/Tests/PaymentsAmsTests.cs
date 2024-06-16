using Allure.NUnit;
using Allure.NUnit.Attributes;
using NUnit.Framework;
using Splitit.Automation.NG.Backend.BaseActions;
using Splitit.Automation.NG.Backend.Services.Ams.AccountManager.AccountManagerApiFunctionality;
using Splitit.Automation.NG.Backend.Services.Ams.AccountManager.AccountManagerBaseObjects;
using Splitit.Automation.NG.Backend.Services.Ams.AccountManager.AccountManagerResponses;
using Splitit.Automation.NG.Backend.Services.V3.DefaultValues;
using Splitit.Automation.NG.Backend.Tests.TestsSetups;
using Splitit.Automation.NG.Utilities.EnvironmentsConfig;

namespace Splitit.Automation.NG.PaymentsSystem.Sku.AccountManagementSystem.Tests;

[TestFixture]
[AllureNUnit]
[AllureSuite("PaymentsAmsTests")]
[AllureDisplayIgnored]
public class PaymentsAmsTests
{
    private RequestHeader? _requestHeader;
    private readonly EnvConfig _envConfig;
    private readonly GetAccountsIdFunctionality _getAccountsIdFunctionality;
    private readonly PutAccountsIdFunctionality _putAccountsIdFunctionality;

    public PaymentsAmsTests()
    {
        Console.WriteLine("Staring PaymentsAmsTests Setup");
        _envConfig = new EnvConfig();
        _getAccountsIdFunctionality = new GetAccountsIdFunctionality();
        _putAccountsIdFunctionality = new PutAccountsIdFunctionality();
        Console.WriteLine("PaymentsAmsTests Setup Succeeded\n");
    }
    
    [OneTimeSetUp]
    public async Task InitSetUp()
    {
        var testsSetup = new TestsSetup();
        testsSetup.Setup();
        var sendAdminLoginRequest = new SendAdminLoginRequest();
        _requestHeader = await sendAdminLoginRequest.DoAdminLogin(
            Environment.GetEnvironmentVariable("AccessTokenURI")!,
            Environment.GetEnvironmentVariable("ClientSecret")!,
            Environment.GetEnvironmentVariable("SplititMockTerminal")!,
            Environment.GetEnvironmentVariable("clientId")!);
    }
    
    [Category("PaymentsAmsTests")]
    [Test(Description = "TestValidateSonInheritFromFather"), CancelAfter(80*1000)]
    public async Task TestValidateSonInheritFromFather()
    {
        try
        {
            Console.WriteLine("\nStarting TestValidateSonInheritFromFather");
            var getAccountResponseFather =
                await _getAccountsIdFunctionality.SendGetRequestGetAccountsIdFunctionalityAsync(_requestHeader!,
                    _envConfig.AMS_FATHER_ACCOUNT_ID);
            var getAccountResponseSon =
                await _getAccountsIdFunctionality.SendGetRequestGetAccountsIdFunctionalityAsync(_requestHeader!,
                    _envConfig.AMS_SON_ACCOUNT_ID);
            Assert.That(getAccountResponseFather.AccountModel.FinancialInformation.FundingSetup.CreditLine,
                Is.EqualTo(getAccountResponseSon.AccountModel.FinancialInformation.FundingSetup.CreditLine));
            var newFunding = new Random().Next(40000, 400000);
            getAccountResponseFather.AccountModel.FinancialInformation.FundingSetup.CreditLine = newFunding;
            var convertedGetToPutBody = ConvertGetToPutBodyRequest(getAccountResponseFather, new PutAccountsIdBaseObjects.Root());
            var putAccountResponse =
                await _putAccountsIdFunctionality.SendPutRequestAccountIdFunctionalityAsync(
                    _requestHeader!, convertedGetToPutBody, _envConfig.AMS_FATHER_ACCOUNT_ID);
            Assert.That(putAccountResponse.AccountModel.FinancialInformation.FundingSetup.CreditLine, Is.EqualTo(newFunding));
            var getAccountResponseFatherAfterChange =
                await _getAccountsIdFunctionality.SendGetRequestGetAccountsIdFunctionalityAsync(_requestHeader!,
                    _envConfig.AMS_FATHER_ACCOUNT_ID);
            var getAccountResponseSonAfterChange =
                await _getAccountsIdFunctionality.SendGetRequestGetAccountsIdFunctionalityAsync(_requestHeader!,
                    _envConfig.AMS_SON_ACCOUNT_ID);
            Assert.That(getAccountResponseFatherAfterChange.AccountModel.FinancialInformation.FundingSetup.CreditLine,
                Is.EqualTo(getAccountResponseSonAfterChange.AccountModel.FinancialInformation.FundingSetup.CreditLine));
            Console.WriteLine("TestValidateSonInheritFromFather is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidateSonInheritFromFather\n" + exception + "\n");
        }
    }

    [Category("PaymentsAmsTests")]
    [Test(Description = "TestValidateSonDoesNotInheritFromFather"), CancelAfter(80*1000)]
    public async Task TestValidateSonDoesNotInheritFromFather()
    {
        try
        {
            Console.WriteLine("\nStarting TestValidateSonDoesNotInheritFromFather");
            var getAccountResponseSon =
                await _getAccountsIdFunctionality.SendGetRequestGetAccountsIdFunctionalityAsync(_requestHeader!,
                    _envConfig.AMS_SON_ACCOUNT_ID);
            var convertedGetToPutBody = ConvertGetToPutBodyRequest(getAccountResponseSon, 
                new PutAccountsIdBaseObjects.Root());
            var putAccountResponse =
                await _putAccountsIdFunctionality.SendPutRequestAccountIdFunctionalityAsync(
                    _requestHeader!, convertedGetToPutBody, _envConfig.AMS_SON_ACCOUNT_ID);
            Assert.That(putAccountResponse.AccountModel.FinancialInformation, Is.Not.Null);
            var newFunding = new Random().Next(500000, 600000);
            getAccountResponseSon.AccountModel.FinancialInformation.FundingSetup.CreditLine = newFunding;
            var convertedGetToPutBodyAfter = ConvertGetToPutBodyRequest(getAccountResponseSon, new PutAccountsIdBaseObjects.Root());
            var putAccountResponseAfter =
                await _putAccountsIdFunctionality.SendPutRequestAccountIdFunctionalityAsync(
                    _requestHeader!, convertedGetToPutBodyAfter, _envConfig.AMS_SON_ACCOUNT_ID);
            Assert.That(putAccountResponseAfter.AccountModel.FinancialInformation.FundingSetup.CreditLine, Is.EqualTo(newFunding));
            var getAccountResponseFather =
                await _getAccountsIdFunctionality.SendGetRequestGetAccountsIdFunctionalityAsync(_requestHeader!,
                    _envConfig.AMS_FATHER_ACCOUNT_ID);
            Assert.That(getAccountResponseFather.AccountModel.FinancialInformation.FundingSetup.CreditLine, 
                Is.Not.EqualTo(putAccountResponseAfter.AccountModel.FinancialInformation.FundingSetup.CreditLine));
            var getAccountResponseSonBackToNormal =
                await _getAccountsIdFunctionality.SendGetRequestGetAccountsIdFunctionalityAsync(_requestHeader!,
                    _envConfig.AMS_SON_ACCOUNT_ID);
            var convertedGetToPutBodyBackToNormal = ConvertGetToPutBodyRequestWithoutFinancialInfo(getAccountResponseSonBackToNormal, 
                new PutAccountsIdBaseObjects.Root());
            var putAccountResponseBackToNormal =
                await _putAccountsIdFunctionality.SendPutRequestAccountIdFunctionalityAsync(
                    _requestHeader!, convertedGetToPutBodyBackToNormal, _envConfig.AMS_SON_ACCOUNT_ID);
            Assert.That(putAccountResponseBackToNormal.AccountModel.FinancialInformation, Is.Not.Null);
            Console.WriteLine("TestValidateSonDoesNotInheritFromFather is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidateSonDoesNotInheritFromFather\n" + exception + "\n");
        }
    }
    
    [Category("PaymentsAmsTests")]
    [Test(Description = "TestValidateSonHasDifferentFather"), CancelAfter(80*1000)]
    public async Task TestValidateSonHasDifferentFather()
    {
        try
        {
            Console.WriteLine("\nStarting TestValidateSonHasDifferentFather");
            var getAccountResponseSon =
                await _getAccountsIdFunctionality.SendGetRequestGetAccountsIdFunctionalityAsync(_requestHeader!,
                    _envConfig.AMS_SON_ACCOUNT_ID);
            Assert.That(getAccountResponseSon.AccountModel.RelatedEntities.AMSParentId,
                Is.EqualTo(_envConfig.AMS_FATHER_ACCOUNT_ID));
            getAccountResponseSon.AccountModel.RelatedEntities.AMSParentId = _envConfig.AMS_DIFF_FATHER_ACCOUNT_ID;
            var convertedGetToPutBody = ConvertGetToPutBodyRequest(getAccountResponseSon, 
                new PutAccountsIdBaseObjects.Root());
            var putAccountResponseAfter =
                await _putAccountsIdFunctionality.SendPutRequestAccountIdFunctionalityAsync(
                    _requestHeader!, convertedGetToPutBody, _envConfig.AMS_SON_ACCOUNT_ID);
            Assert.That(putAccountResponseAfter.AccountModel.RelatedEntities.AMSParentId, 
                Is.EqualTo(_envConfig.AMS_DIFF_FATHER_ACCOUNT_ID));
            var getAccountResponseSonBackToNormal =
                await _getAccountsIdFunctionality.SendGetRequestGetAccountsIdFunctionalityAsync(_requestHeader!,
                    _envConfig.AMS_SON_ACCOUNT_ID);
            getAccountResponseSonBackToNormal.AccountModel.RelatedEntities.AMSParentId = _envConfig.AMS_FATHER_ACCOUNT_ID;
            var convertedGetToPutBodyBackToNormal = ConvertGetToPutBodyRequestWithoutFinancialInfo(getAccountResponseSonBackToNormal, 
                new PutAccountsIdBaseObjects.Root());
            var putAccountResponseBackToNormal =
                await _putAccountsIdFunctionality.SendPutRequestAccountIdFunctionalityAsync(
                    _requestHeader!, convertedGetToPutBodyBackToNormal, _envConfig.AMS_SON_ACCOUNT_ID);
            Assert.That(putAccountResponseBackToNormal.AccountModel.RelatedEntities.AMSParentId,
                Is.EqualTo(_envConfig.AMS_FATHER_ACCOUNT_ID));
            Console.WriteLine("TestValidateSonHasDifferentFather is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidateSonHasDifferentFather\n" + exception + "\n");
        }
    }
    
    [Category("PaymentsAmsTests")]
    [Test(Description = "TestValidateSonBecomeRoot"), CancelAfter(80*1000)]
    public async Task TestValidateSonBecomeRoot()
    {
        try
        {
            Console.WriteLine("\nStarting TestValidateSonBecomeRoot");
            var getAccountResponseSon =
                await _getAccountsIdFunctionality.SendGetRequestGetAccountsIdFunctionalityAsync(_requestHeader!,
                    _envConfig.AMS_SON_ACCOUNT_ID);
            Assert.That(getAccountResponseSon.AccountModel.RelatedEntities.AMSParentId,
                Is.EqualTo(_envConfig.AMS_FATHER_ACCOUNT_ID));
            getAccountResponseSon.AccountModel.RelatedEntities.AMSParentId = _envConfig.AMS_SPLITIT_BASE_ACCOUNT_ID;
            var convertedGetToPutBody = ConvertGetToPutBodyRequest(getAccountResponseSon, 
                new PutAccountsIdBaseObjects.Root());
            var putAccountResponseAfter =
                await _putAccountsIdFunctionality.SendPutRequestAccountIdFunctionalityAsync(
                    _requestHeader!, convertedGetToPutBody, _envConfig.AMS_SON_ACCOUNT_ID);
            Assert.That(putAccountResponseAfter.AccountModel.RelatedEntities.AMSParentId, 
                Is.EqualTo(_envConfig.AMS_SPLITIT_BASE_ACCOUNT_ID));
            var getAccountResponseSonBackToNormal =
                await _getAccountsIdFunctionality.SendGetRequestGetAccountsIdFunctionalityAsync(_requestHeader!,
                    _envConfig.AMS_SON_ACCOUNT_ID);
            getAccountResponseSonBackToNormal.AccountModel.RelatedEntities.AMSParentId = _envConfig.AMS_FATHER_ACCOUNT_ID;
            var convertedGetToPutBodyBackToNormal = ConvertGetToPutBodyRequestWithoutFinancialInfo(getAccountResponseSonBackToNormal, 
                new PutAccountsIdBaseObjects.Root());
            var putAccountResponseBackToNormal =
                await _putAccountsIdFunctionality.SendPutRequestAccountIdFunctionalityAsync(
                    _requestHeader!, convertedGetToPutBodyBackToNormal, _envConfig.AMS_SON_ACCOUNT_ID);
            Assert.That(putAccountResponseBackToNormal.AccountModel.RelatedEntities.AMSParentId,
                Is.EqualTo(_envConfig.AMS_FATHER_ACCOUNT_ID));
            Console.WriteLine("TestValidateSonBecomeRoot is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidateSonBecomeRoot\n" + exception + "\n");
        }
    }

    private PutAccountsIdBaseObjects.Root ConvertGetToPutBodyRequest(GetAccountsIdResponse.Root getAccountResponse
        , PutAccountsIdBaseObjects.Root putAccountIdBaseObjects)
    {
        try
        {
            Console.WriteLine("\nStarting ConvertGetToPutBodyRequest");
            putAccountIdBaseObjects.Company = new PutAccountsIdBaseObjects.Company
            {
                AccountEmail = getAccountResponse.AccountModel.Company.AccountEmail,
                AccountName = getAccountResponse.AccountModel.Company.AccountName,
                AccountPhone = getAccountResponse.AccountModel.Company.AccountPhone,
                CompanyAddresses = new List<PutAccountsIdBaseObjects.CompanyAddress>()
            };
            var companyAddress = new PutAccountsIdBaseObjects.CompanyAddress
            {
                City = getAccountResponse.AccountModel.Company.CompanyAddresses[0].City,
                Country = getAccountResponse.AccountModel.Company.CompanyAddresses[0].Country,
                Id = getAccountResponse.AccountModel.Company.CompanyAddresses[0].Id,
                PostalCode = getAccountResponse.AccountModel.Company.CompanyAddresses[0].PostalCode,
                State = getAccountResponse.AccountModel.Company.CompanyAddresses[0].State,
                Street = getAccountResponse.AccountModel.Company.CompanyAddresses[0].Street,
                Type = getAccountResponse.AccountModel.Company.CompanyAddresses[0].Type
            };
            putAccountIdBaseObjects.Company.CompanyAddresses.Add(companyAddress);
            putAccountIdBaseObjects.Company.Website = getAccountResponse.AccountModel.Company.Website;
            putAccountIdBaseObjects.FinancialInformation = new PutAccountsIdBaseObjects.FinancialInformation
                {
                    FundingSetup = new PutAccountsIdBaseObjects.FundingSetup
                    {
                        CreditLine = getAccountResponse.AccountModel
                            .FinancialInformation.FundingSetup.CreditLine,
                        FundingTrigger = getAccountResponse.AccountModel
                            .FinancialInformation.FundingSetup.FundingTrigger,
                        FundingOnHold = getAccountResponse.AccountModel
                            .FinancialInformation.FundingSetup.FundingOnHold,
                        FundingEndDate = getAccountResponse.AccountModel
                            .FinancialInformation.FundingSetup.FundingEndDate,
                        FundingStartDate = getAccountResponse
                            .AccountModel.FinancialInformation.FundingSetup.FundingStartDate,
                        ReservePool = getAccountResponse.AccountModel
                            .FinancialInformation.FundingSetup.ReservePool,
                        RiskRating = getAccountResponse.AccountModel
                            .FinancialInformation.FundingSetup.RiskRating,
                        DebitOnHold = getAccountResponse.AccountModel
                            .FinancialInformation.FundingSetup.DebitOnHold,
                        SettlementType = getAccountResponse.AccountModel
                            .FinancialInformation.FundingSetup.SettlementType,
                        FundNonSecuredPlans = getAccountResponse
                            .AccountModel.FinancialInformation.FundingSetup.FundNonSecuredPlans
                    },
                    BillingInformation = new PutAccountsIdBaseObjects.BillingInformation
                    {
                        BillingCurrency = getAccountResponse
                            .AccountModel.FinancialInformation.BillingInformation.BillingCurrency,
                        RegisteredNumber = getAccountResponse
                            .AccountModel.FinancialInformation.BillingInformation.RegisteredNumber,
                        LegalBusinessName = getAccountResponse
                            .AccountModel.FinancialInformation.BillingInformation.LegalBusinessName,
                        Subsidiary = getAccountResponse
                            .AccountModel.FinancialInformation.BillingInformation.Subsidiary,
                        VatGstNumber = getAccountResponse
                            .AccountModel.FinancialInformation.BillingInformation.VatGstNumber,
                        MonetaryFlow = getAccountResponse
                            .AccountModel.FinancialInformation.BillingInformation.MonetaryFlow
                    },
                    Contracts = new List<PutAccountsIdBaseObjects.Contract>()
                };

            var pricing = new PutAccountsIdBaseObjects.Pricing
            {
                Id = getAccountResponse.AccountModel.FinancialInformation.Contracts[0].Pricings[0].Id,
                SKU = getAccountResponse.AccountModel.FinancialInformation.Contracts[0].Pricings[0].SKU,
                TransactionFeePercentage = getAccountResponse.AccountModel.FinancialInformation.Contracts[0].Pricings[0].TransactionFeePercentage,
                TransactionFixedFee = getAccountResponse.AccountModel.FinancialInformation.Contracts[0].Pricings[0].TransactionFixedFee,
                ChargebackFee = getAccountResponse.AccountModel.FinancialInformation.Contracts[0].Pricings[0].ChargebackFee,
                ErpId = getAccountResponse.AccountModel.FinancialInformation.Contracts[0].Pricings[0].ErpId,
                BankRejectFee = getAccountResponse.AccountModel.FinancialInformation.Contracts[0].Pricings[0].BankRejectFee
            };

            var contract = new PutAccountsIdBaseObjects.Contract
            {
                BusinessUnitUniqueId = getAccountResponse.AccountModel.FinancialInformation.Contracts[0].BusinessUnitUniqueId,
                EndDate = getAccountResponse.AccountModel.FinancialInformation.Contracts[0].EndDate,
                Id = getAccountResponse.AccountModel.FinancialInformation.Contracts[0].Id,
                Name = getAccountResponse.AccountModel.FinancialInformation.Contracts[0].Name,
                StartDate = getAccountResponse.AccountModel.FinancialInformation.Contracts[0].StartDate,
                SubscriptionERP_Id = getAccountResponse.AccountModel.FinancialInformation.Contracts[0].SubscriptionERP_Id,
                Pricings = new List<PutAccountsIdBaseObjects.Pricing> { pricing }
            };
            putAccountIdBaseObjects.FinancialInformation.Contracts.Add(contract);

            putAccountIdBaseObjects.FinancialInformation.DebitBank = new PutAccountsIdBaseObjects.DebitBank
                {
                    BankNumber = getAccountResponse.AccountModel.FinancialInformation.DebitBank.BankNumber,
                    BankAccountNumber = getAccountResponse.AccountModel
                        .FinancialInformation.DebitBank.BankAccountNumber,
                    BankAccountName = getAccountResponse
                        .AccountModel.FinancialInformation.DebitBank.BankAccountName,
                    BankAccountType = getAccountResponse
                        .AccountModel.FinancialInformation.DebitBank.BankAccountType,
                    BSBCode = getAccountResponse.AccountModel.FinancialInformation.DebitBank.BSBCode,
                    GoCardlessCustomerId = getAccountResponse
                        .AccountModel.FinancialInformation.DebitBank.GoCardlessCustomerId,
                    GoCardlessEmail = getAccountResponse
                        .AccountModel.FinancialInformation.DebitBank.GoCardlessEmail,
                    GoCardlessFamilyName = getAccountResponse
                        .AccountModel.FinancialInformation.DebitBank.SettlementChannel,
                    GoCardlessGivenName = getAccountResponse
                        .AccountModel.FinancialInformation.DebitBank.GoCardlessMandateId,
                    GoCardlessMandateId = getAccountResponse
                        .AccountModel.FinancialInformation.DebitBank.GoCardlessCompanyName,
                    GoCardlessCompanyName = getAccountResponse
                        .AccountModel.FinancialInformation.DebitBank.SettlementChannel,
                    SettlementChannel = getAccountResponse
                        .AccountModel.FinancialInformation.DebitBank.SettlementChannel
                };

            putAccountIdBaseObjects.FinancialInformation.CreditBank = new PutAccountsIdBaseObjects.CreditBank
                {
                    SettlementChannel = getAccountResponse.AccountModel
                        .FinancialInformation.CreditBank.SettlementChannel,
                    BankAccountType = getAccountResponse
                        .AccountModel.FinancialInformation.CreditBank.BankAccountType,
                    BankAccountNumber = getAccountResponse
                        .AccountModel.FinancialInformation.CreditBank.BankAccountNumber,
                    BankAccountName = getAccountResponse
                        .AccountModel.FinancialInformation.CreditBank.BankAccountName,
                    BSBCode = getAccountResponse.AccountModel.FinancialInformation.CreditBank.BSBCode,
                    BankCode = getAccountResponse
                        .AccountModel.FinancialInformation.CreditBank.BankCode,
                    TransitNumber = getAccountResponse
                        .AccountModel.FinancialInformation.CreditBank.TransitNumber,
                    SwiftCode = getAccountResponse.AccountModel
                        .FinancialInformation.CreditBank.SwiftCode,
                    IBAN = getAccountResponse.AccountModel.FinancialInformation.CreditBank.IBAN,
                    BankNumber = getAccountResponse.AccountModel
                        .FinancialInformation.CreditBank.BankNumber
                };

            putAccountIdBaseObjects.Contacts = new List<PutAccountsIdBaseObjects.Contact>();
            var contact = new PutAccountsIdBaseObjects.Contact
            {
                Type = getAccountResponse.AccountModel.Contacts[0].Type,
                Id = getAccountResponse.AccountModel.Contacts[0].Id,
                Email = getAccountResponse.AccountModel.Contacts[0].Email,
                FullName = getAccountResponse.AccountModel.Contacts[0].FullName
            };
            putAccountIdBaseObjects.Contacts.Add(contact);
            
            putAccountIdBaseObjects.RelatedEntities = new PutAccountsIdBaseObjects.RelatedEntities
            {
                PartnerProfileId = getAccountResponse.AccountModel.RelatedEntities.PartnerProfileId,
                AMSParentId = getAccountResponse.AccountModel.RelatedEntities.AMSParentId
            };
            Console.WriteLine("Done with ConvertGetToPutBodyRequest\n");
            return putAccountIdBaseObjects;
        }
        catch (Exception e)
        {
            Console.WriteLine("Error in ConvertGetToPutBodyRequest" + e);
            throw;
        }
    }

    private PutAccountsIdBaseObjects.Root ConvertGetToPutBodyRequestWithoutFinancialInfo(GetAccountsIdResponse.Root getAccountResponse
        , PutAccountsIdBaseObjects.Root putAccountIdBaseObjects)
    {
        try
        {
            putAccountIdBaseObjects.Company = new PutAccountsIdBaseObjects.Company
            {
                AccountEmail = getAccountResponse.AccountModel.Company.AccountEmail,
                AccountName = getAccountResponse.AccountModel.Company.AccountName,
                AccountPhone = getAccountResponse.AccountModel.Company.AccountPhone,
                CompanyAddresses = new List<PutAccountsIdBaseObjects.CompanyAddress>()
            };
            var companyAddress = new PutAccountsIdBaseObjects.CompanyAddress
            {
                City = getAccountResponse.AccountModel.Company.CompanyAddresses[0].City,
                Country = getAccountResponse.AccountModel.Company.CompanyAddresses[0].Country,
                Id = getAccountResponse.AccountModel.Company.CompanyAddresses[0].Id,
                PostalCode = getAccountResponse.AccountModel.Company.CompanyAddresses[0].PostalCode,
                State = getAccountResponse.AccountModel.Company.CompanyAddresses[0].State,
                Street = getAccountResponse.AccountModel.Company.CompanyAddresses[0].Street,
                Type = getAccountResponse.AccountModel.Company.CompanyAddresses[0].Type
            };
            putAccountIdBaseObjects.Company.CompanyAddresses.Add(companyAddress);
            putAccountIdBaseObjects.Company.Website = getAccountResponse.AccountModel.Company.Website;
            putAccountIdBaseObjects.FinancialInformation = new PutAccountsIdBaseObjects.FinancialInformation();
            putAccountIdBaseObjects.FinancialInformation = null!;
            
            putAccountIdBaseObjects.Contacts = new List<PutAccountsIdBaseObjects.Contact>();
            var contact = new PutAccountsIdBaseObjects.Contact
            {
                Type = getAccountResponse.AccountModel.Contacts[0].Type,
                Id = getAccountResponse.AccountModel.Contacts[0].Id,
                Email = getAccountResponse.AccountModel.Contacts[0].Email,
                FullName = getAccountResponse.AccountModel.Contacts[0].FullName
            };
            putAccountIdBaseObjects.Contacts.Add(contact);

            putAccountIdBaseObjects.RelatedEntities = new PutAccountsIdBaseObjects.RelatedEntities
            {
                AMSParentId = getAccountResponse.AccountModel.RelatedEntities.AMSParentId,
                PartnerProfileId = getAccountResponse.AccountModel.RelatedEntities.PartnerProfileId
            };
            return putAccountIdBaseObjects;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}