using Splitit.Automation.NG.SharedResources.Tests.TestsHelper;

namespace Splitit.Automation.NG.Backend.Services.V3.DefaultValues;

public class PaymentMethodDefaultValues
{
    public CardDefaultValues card;
    public string type;
    public SpreedlyTokenDefaultValues SpreedlyToken;
    public readonly BluesnapVaultedShopperToken BluesnapVaultedShopperToken;

    public PaymentMethodDefaultValues()
    {
        type = "Card";
        card = new CardDefaultValues();
        SpreedlyToken = new SpreedlyTokenDefaultValues();
        BluesnapVaultedShopperToken = new BluesnapVaultedShopperToken();
    }
}