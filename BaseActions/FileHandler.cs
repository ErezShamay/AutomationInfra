namespace Splitit.Automation.NG.Backend.BaseActions;

public class FileHandler
{
    public string[] ReadFile()
    {
        try
        {
            Console.WriteLine("\nStarting to ReadFile");
            var filePath = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory)!.ToString();
            var filePathFixed = filePath.Substring(0, filePath.IndexOf("NG") + 2) + "/Splitit.Automation.NG/PaymentsSystem/Sku/Tests/MerchantSetUp-SKUs.csv";
            string[] lines = System.IO.File.ReadAllLines(filePathFixed);
            Console.WriteLine("Done with ReadFile\n");
            return lines;
        }
        catch (Exception exception)
        {
            Console.WriteLine("Error in ReadFile \n" + exception + "\n");
            throw;
        }
    }
}