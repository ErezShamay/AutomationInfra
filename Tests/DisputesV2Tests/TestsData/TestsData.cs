namespace Splitit.Automation.NG.Backend.Tests.DisputesV2Tests.TestsData;

public class TestsData
{
    public string ReturnEvidenceFileLocation(string fileName)
    {
        string filePathFixed;
        var filePath = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).ToString();
        var forFilePathJenkins = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.Parent;
        var jobName = Environment.GetEnvironmentVariable("JOB_NAME");
        if (jobName != null)
        {
            Console.WriteLine("This is a Jenkins run");
            filePathFixed = forFilePathJenkins + "/Backend/Tests/DisputesV2Tests/TestsData/" + fileName;
            Console.WriteLine("created filePathFixed -> " + filePathFixed);
        }
        else
        {
            Console.WriteLine("This is a local run");
            filePathFixed = filePath.Substring(0, filePath.IndexOf("NG") + 2) +
                            "/Splitit.Automation.NG/Backend/Tests/DisputesV2Tests/TestsData/" + fileName;
            Console.WriteLine("created filePathFixed -> " + filePathFixed);
        }

        return filePathFixed;
    }
}