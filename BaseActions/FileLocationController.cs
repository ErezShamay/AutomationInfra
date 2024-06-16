namespace Splitit.Automation.NG.Backend.BaseActions;

public class FileLocationController
{
    public string ReturnFileLocation(string inBackendTestsFolderLocation, string fileName)
    {
        string filePathFixed;
        var filePath = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).ToString();
        var forFilePathJenkins = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.Parent;
        var jobName = Environment.GetEnvironmentVariable("JOB_NAME");
        if (jobName != null)
        {
            Console.WriteLine("This is a Jenkins run");
            filePathFixed = forFilePathJenkins + "/Backend/Tests/"+inBackendTestsFolderLocation+"/" + fileName;
            Console.WriteLine("created filePathFixed -> " + filePathFixed);
        }
        else
        {
            Console.WriteLine("This is a local run");
            filePathFixed = filePath.Substring(0, filePath.IndexOf("NG") + 2) +
                            "/Splitit.Automation.NG/Backend/Tests/"+inBackendTestsFolderLocation+"/" + fileName;
            Console.WriteLine("created filePathFixed -> " + filePathFixed);
        }

        return filePathFixed;
    }
}