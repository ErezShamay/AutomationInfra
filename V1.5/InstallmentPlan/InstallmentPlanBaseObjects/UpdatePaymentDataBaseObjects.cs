namespace Splitit.Automation.NG.Backend.Services.V1._5.InstallmentPlan.InstallmentPlanBaseObjects;

public class UpdatePaymentDataBaseObjects
{
    public class Address
    {
        public string addressLine { get; set; }
        public string addressLine2 { get; set; }
        public string city { get; set; }
        public string country { get; set; }
        public string state { get; set; }
        public string zip { get; set; }
        public string fullAddressLine { get; set; }
    }

    public class CardBrand
    {
        public int id { get; set; }
        public string code { get; set; }
        public string description { get; set; }
    }

    public class CardType
    {
        public int id { get; set; }
        public string code { get; set; }
        public string description { get; set; }
    }

    public class CreditCardDetails
    {
        public string cardId { get; set; }
        public string cardNumber { get; set; }
        public string cardExpMonth { get; set; }
        public string cardExpYear { get; set; }
        public CardBrand cardBrand { get; set; }
        public CardType cardType { get; set; }
        public string bin { get; set; }
        public string cardHolderFullName { get; set; }
        public string cardCvv { get; set; }
        public Address address { get; set; }
        public string token { get; set; }
    }

    public class ExtendedParams
    {
        public string additionalProp1 { get; set; }
        public string additionalProp2 { get; set; }
        public string additionalProp3 { get; set; }
    }

    public class RequestHeader
    {
        public TouchPoint touchPoint { get; set; }
        public string sessionId { get; set; }
        public string apiKey { get; set; }
        public string cultureName { get; set; }
        public int authenticationType { get; set; }
    }

    public class Root
    {
        public string installmentPlanNumber { get; set; }
        public CreditCardDetails creditCardDetails { get; set; }
        public ExtendedParams extendedParams { get; set; }
        public RequestHeader requestHeader { get; set; }
    }

    public class TouchPoint
    {
        public string code { get; set; }
        public string version { get; set; }
        public string subVersion { get; set; }
        public int versionedTouchpointId { get; set; }
    }
}