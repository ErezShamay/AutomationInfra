namespace Splitit.Automation.NG.Backend.Services.Ams.MockData;

public class PricingDefaultValues
{
    public string SKU;
    public int TransactionFeePercentage;
    public int TransactionFixedFee;
    public int ChargebackFee;
    
    public PricingDefaultValues()
    {
        var faker = new Bogus.Faker();
        var randomString = ReturnRandomString();
        SKU = "MER-"+randomString+"-STL-MON-FTR-CRC-U09";
        TransactionFeePercentage = faker.Random.Int();
        TransactionFixedFee = faker.Random.Int();
        ChargebackFee = faker.Random.Int();
    }

    private static string ReturnRandomString()
    {
        var skuList = new List<string>
        {
            "UNF",
            "FND"
        };
        var r = new Random( );
        var index = r.Next( skuList.Count );
        var randomString = skuList[ index ];
        return randomString;
    }
}