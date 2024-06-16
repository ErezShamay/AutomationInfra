using LumenWorks.Framework.IO.Csv;

namespace Splitit.Automation.NG.Backend.BaseActions;

public class ExcelFileController
{
    public CachedCsvReader ReadExcelFile(string suiteName)
    {
        try
        {
            Console.WriteLine("\nStarting ReadExcelFile");
            var filePath = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).ToString();
            var jenkinsFilePath = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.Parent;
            var jobName = Environment.GetEnvironmentVariable("JOB_NAME");
            var nameOfFile = suiteName switch
            {
                "matching" => "/PaymentsSystem/Sku/SkuMatching/Tests/MerchantSetUp-SKUs.csv",
                "processing" => "/PaymentsSystem/Sku/SkuProcessing/Tests/MerchantSetUp-SKUs-Processing.csv",
                "activityBefore" => "/PaymentsSystem/Sku/SkuSetActivitiesBeforeInvoiced/Tests/MerchantSetUp-SKUs-Set-Activities-Before-Invoiced.csv",
                "activityAfter" => "/PaymentsSystem/Sku/SkuSetActivitiesAfterInvoiced/Tests/MerchantSetUp-SKUs-Set-Activities-After-Invoiced.csv",
                "nonSecure" => "/PaymentsSystem/Sku/FundNonSecuredPlans/Tests/MerchantSetUp-SKUs-Fund-Non-Secured-Plans.csv",
                "fundingOperations" => "/PaymentsSystem/Sku/FundingOperations/Tests/MerchantSetUp-SKUs-Funding-Operations.csv",
                "settlementReport" => "/Backend/Tests/ReportTests/MerchantSetUp-SKUs-SettlementReport.csv",
                _ => null!
            };
            string filePathFixed;
            if (jobName != null)
            {
                filePathFixed = jenkinsFilePath!.ToString();
                filePathFixed += nameOfFile;
            }
            else
            {
                filePathFixed = filePath.Substring(0, filePath.IndexOf("NG") + 2);
                filePathFixed += "/Splitit.Automation.NG" + nameOfFile;
            }
            Console.WriteLine("File path fixed is: ----> " + filePathFixed);
            var csv = new CachedCsvReader(new StreamReader(filePathFixed), true);
            Console.WriteLine("Done ReadExcelFile\nֿ");
            return csv;
        }
        catch (Exception exception)
        {
            Console.WriteLine("Error in ReadExcelFile\n" + exception + "\n");
            throw;
        }
    }
}