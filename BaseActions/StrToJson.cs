using Newtonsoft.Json.Linq;

namespace Splitit.Automation.NG.Backend.BaseActions;

public abstract class StrToJson
{
    public static JObject ConvertStrToJson(string str)
    {
        try
        {
            var jsonStr = JObject.Parse(str);
            Console.WriteLine("\nConvertStrToJson Succeeded");
            return jsonStr;
        }
        catch(Exception exception)
        {
            Console.WriteLine("Error in ConvertStrToJson \n" + exception + "\n");
            throw;
        }
    }
}