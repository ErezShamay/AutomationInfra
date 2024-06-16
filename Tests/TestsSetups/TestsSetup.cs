using System.Xml.Linq;
using static Splitit.Automation.NG.Backend.BaseActions.EnvVars;

namespace Splitit.Automation.NG.Backend.Tests.TestsSetups;

public class TestsSetup
{
    private readonly string _env = Environment.GetEnvironmentVariable("ENVIRONMENT")!;
    
    public void Setup()
    {
        try
        {
            Console.WriteLine("\nenv var is -> " + _env);
            if (_env != null!)
            {
                Console.WriteLine();
                Console.WriteLine("Starting to set env vars for -> " + _env);
                SetEnvVars(_env.ToLower());
                Console.WriteLine("SetEnvVars for " + _env + " finished successfully");
            }
            else
            {
                Console.WriteLine("Starting to set env vars for default value sandbox");
                SetEnvVars("sandbox");
                Console.WriteLine("SetEnvVars for sandbox finished successfully");
            }

            Console.WriteLine("Done with Setup\n");
            Console.WriteLine();
        }
        catch (Exception exception)
        {
            Console.Error.WriteLineAsync("\nError in setup of Setting " + _env + " EnvVars \n" + exception + "\n");
        }
    }

    public Dictionary<string, string>? ReadXmlAndConvertToDict()
    {
        try
        {
            Console.WriteLine();
            Console.WriteLine("\nStarting ReadXmlAndConvertToDict");
            string filePathFixed;
            var filePath = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory)!.ToString();
            var forFilePathJenkins = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory)!.Parent!.Parent!.Parent;
            var jobName = Environment.GetEnvironmentVariable("JOB_NAME");
            if (jobName != null)
            {
                Console.WriteLine("This is a Jenkins run");
                filePathFixed = forFilePathJenkins + "/Utilities/EnvironmentsConfig/" + _env.ToLower() + ".xml";
                Console.WriteLine("created filePathFixed -> " + filePathFixed);
            }
            else
            {
                Console.WriteLine("This is a local run");
                filePathFixed = filePath.Substring(0, filePath.IndexOf("NG") + 2) +
                                "/Splitit.Automation.NG/Utilities/EnvironmentsConfig/sandbox.xml";
                Console.WriteLine("created filePathFixed -> " + filePathFixed);
            }

            Console.WriteLine("Starting to read file");
            var xmlString = File.ReadAllText(filePathFixed);
            Console.WriteLine("Done with read all text");
            Console.WriteLine("Starting to parse the file");
            var doc = XDocument.Parse(xmlString);
            Console.WriteLine("Done with parsing the file");
            var dataDictionary = new Dictionary<string, string>();
            Console.WriteLine("Starting foreach loop");
            foreach (var element in doc.Descendants().Where(p => p.HasElements == false))
            {
                var keyInt = 0;
                var keyName = element.Name.LocalName;
                var parent = element.Parent;
                while (parent != null)
                {
                    keyName = parent.Name.LocalName + "." + keyName;
                    parent = parent.Parent;
                }

                while (dataDictionary.ContainsKey(keyName))
                {
                    keyName = keyName + "_" + keyInt++;
                }

                dataDictionary.Add(keyName, element.Value);
            }

            Console.WriteLine("Done with foreach loop\n");
            return dataDictionary;
        }
        catch (Exception exception)
        {
            Console.WriteLine("\nError in ReadXmlAndConvertToDict\n" + exception + "\n");
            throw;
        }
    }
}