namespace Splitit.Automation.NG.PaymentsSystem.Sku.MainFunctionality;

public class SkuCartesianProduct
{
    private readonly string[] _merchantSetup = { "MER", "DBS", "PCO", "DPL" };
    private readonly string[] _isFunded = { "FUN", "UNF" };
    private readonly string[] _setFirstInstallment = { "STL", "F25", "F50" };
    private readonly string[] _setCollectionCadence = { "MON", "BIW" };
    private readonly string[] _future = { "FTR" };
    private readonly string[] _setCardType = { "DRC", "CRC" };
    private readonly string[] _setInstallments = {"U01", "U03", "U06", "U09", "U12", "U15", "U18", "U21", "U24","U36", "F03", "F06", "F09", "F12", "F15", "F18", "F21", "F24"};

    public SkuCartesianProduct()
    {
        Console.WriteLine("Starting RunCartesianProduct\n");
        Console.Write("\nGenerate a cartesian product of given sets : ");
        Console.Write("\n----------------------------------------------------\n");
        var cartesianProduct =
            from merchant in _merchantSetup
            from funded in _isFunded
            from firstInstallment in _setFirstInstallment
            from collectionCadence in _setCollectionCadence
            from future in _future
            from cardType in _setCardType
            from installment in _setInstallments
            select new { merchant, funded, firstInstallment, collectionCadence, future, cardType, installment };
        Console.Write("The Cartesian Product are : \n");
        foreach (var productList in cartesianProduct)
        {
            Console.WriteLine(productList);
        }
        Console.WriteLine("\nDone RunCartesianProduct");
    }
}