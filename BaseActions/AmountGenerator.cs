using NUnit.Framework;

namespace Splitit.Automation.NG.Backend.BaseActions;

public abstract class AmountGenerator
{
    public static double GenerateAmount()
    {
        var random = new Random();
        const decimal minValue = 15.00m;
        const decimal maxValue = 9999.00m;
        var randomDecimal = (decimal)random.NextDouble() * (maxValue - minValue) + minValue;
        randomDecimal = Math.Round(randomDecimal, 2);
        var formattedRandomDecimal = randomDecimal.ToString("N2");
        if (decimal.TryParse(formattedRandomDecimal, out var resultDecimal))
        {
            return Decimal.ToDouble(resultDecimal);
        }
        Assert.Fail("Error in generating double amount");
        return 0;
    }
    
    public static double GenerateAmountWithMinMaxValues(decimal minValue, decimal maxValue)
    {
        var random = new Random();
        var randomDecimal = (decimal)random.NextDouble() * (maxValue - minValue) + minValue;
        randomDecimal = Math.Round(randomDecimal, 2);
        var formattedRandomDecimal = randomDecimal.ToString("N2");
        if (decimal.TryParse(formattedRandomDecimal, out var resultDecimal))
        {
            return Decimal.ToDouble(resultDecimal);
        }
        Assert.Fail("Error in generating double amount");
        return 0;
    }
}