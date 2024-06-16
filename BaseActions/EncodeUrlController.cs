using System.Web;

namespace Splitit.Automation.NG.Backend.BaseActions;

public class EncodeUrlController
{
    public string EncodeUrlFormatter(string originalString)
    {
        try
        {
            Console.WriteLine("Starting EncodeUrlFormatter");
            var encodedString = HttpUtility.UrlEncode(originalString);
            Console.WriteLine("Encoded String: " + encodedString);
            Console.WriteLine("Done With EncodeUrlFormatter");
            return encodedString;
        }
        catch (Exception e)
        {
            Console.WriteLine("Error in EncodeUrlFormatter" + e);
            throw;
        }
    }
}