namespace Splitit.Automation.NG.SharedResources.Tests.TestsHelper;

public class VaultedShopperResponse
{
    public class FraudResultInfo
    {
    }

    public class PaymentSources
    {
    }

    public class Root
    {
        public int vaultedShopperId { get; set; }
        public string shopperCurrency { get; set; }
        public PaymentSources paymentSources { get; set; }
        public FraudResultInfo fraudResultInfo { get; set; }
        public string dateCreated { get; set; }
        public string timeCreated { get; set; }
    }
}