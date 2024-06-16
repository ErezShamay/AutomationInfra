namespace Splitit.Automation.NG.Backend.Services.V3.DefaultValues;

public class CardDefaultValues
{
    public string cardCvv;
    public int cardExpMonth;
    public int cardExpYear;
    public string cardHolderFullName;
    public string cardNumber;

    public CardDefaultValues()
    {
        var faker = new Bogus.Faker();
        cardHolderFullName = faker.Name.FullName();
        cardNumber = "4111111111111111";
        cardExpYear = 2030;
        cardExpMonth = 03;
        cardCvv = "737";
    }
}