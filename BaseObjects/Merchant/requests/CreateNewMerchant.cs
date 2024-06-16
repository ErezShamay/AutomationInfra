namespace Splitit.Automation.NG.Backend.Services.AdminApi.BaseObjects.Merchant.requests;

public class CreateNewMerchant
{
    public class Root
    {
        public string BusinessLegalName { get; set; }
        public string BusinessDBAName { get; set; }
        public string RegisteredCountryOfBusinessCode { get; set; }
    }
}