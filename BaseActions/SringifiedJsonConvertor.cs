using Newtonsoft.Json;

namespace Splitit.Automation.NG.Backend.BaseActions;

public class SringifiedJsonConvertor
{
    public string ConvertStringToStringified(object obj)
    {
        try
        {
            Console.WriteLine("starting ConvertStringToStringified");
            var jsonString = JsonConvert.SerializeObject(obj, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            });
            Console.WriteLine("Done with ConvertStringToStringified");
            return jsonString;
        }
        catch (Exception e)
        {
            Console.WriteLine("Error in ConvertStringToStringified" + e);
            throw;
        }
    }
}