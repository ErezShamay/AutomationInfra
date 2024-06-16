using System.Diagnostics;

namespace Splitit.Automation.NG.Backend.BaseActions;

public class BashFileController
{
    public async Task RunBashFile()
    {
        var filePath = ReturnBashFileLocation("ReportTrigger.sh");
        var startInfo = new ProcessStartInfo
        {
            FileName = "/bin/bash",
            Arguments = $"-c \"{filePath}\"",
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        var process = new Process();
        process.StartInfo = startInfo;
        process.OutputDataReceived += (sender, e) => Console.WriteLine(e.Data);
        process.ErrorDataReceived += (sender, e) => Console.WriteLine(e.Data);
        process.Start();
        process.BeginOutputReadLine();
        process.BeginErrorReadLine();
        await process.WaitForExitAsync();
        var exitCode = process.ExitCode;
        Console.WriteLine(exitCode == 0
            ? "Bash script executed successfully."
            : $"Bash script execution failed with exit code: {exitCode}");
    }
    
    private string ReturnBashFileLocation(string fileName)
    {
        string filePathFixed;
        var filePath = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).ToString();
        var forFilePathJenkins = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.Parent;
        var jobName = Environment.GetEnvironmentVariable("JOB_NAME");
        if (jobName != null)
        {
            Console.WriteLine("This is a Jenkins run");
            filePathFixed = forFilePathJenkins + "/Backend/Tests/ReportTests/" + fileName;
            Console.WriteLine("created filePathFixed -> " + filePathFixed);
        }
        else
        {
            Console.WriteLine("This is a local run");
            filePathFixed = filePath.Substring(0, filePath.IndexOf("NG") + 2) +
                            "/Splitit.Automation.NG/Backend/Tests/ReportTests/" + fileName;
            Console.WriteLine("created filePathFixed -> " + filePathFixed);
        }

        return filePathFixed;
    }
}