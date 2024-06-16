namespace Splitit.Automation.NG.Backend.Services.V2.InstallmentPlan.InstallmentPlanBaseObjects;

public class GetValidNumberOfInstallmentsBaseObjects
{
    public class Address
    {
        public string AddressLine { get; set; }
        public string AddressLine2 { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string FullAddressLine { get; set; }
    }

    public class CardBrand
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
    }

    public class CardType
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
    }

    public class CreditCardDetails
    {
        public string CardId { get; set; }
        public string CardNumber { get; set; }
        public string CardExpMonth { get; set; }
        public string CardExpYear { get; set; }
        public CardBrand CardBrand { get; set; }
        public CardType CardType { get; set; }
        public string Bin { get; set; }
        public string CardHolderFullName { get; set; }
        public string CardCvv { get; set; }
        public Address Address { get; set; }
        public string Token { get; set; }
    }

    public class Root
    {
        public string InstallmentPlanNumber { get; set; }
        public CreditCardDetails CreditCardDetails { get; set; }
    }
}