using System.Text;
using Org.BouncyCastle.Bcpg.OpenPgp;

namespace Splitit.Automation.NG.Backend.BaseActions;

public class DecodeBase64Pgp
{
    public string DecodeToPgp(string base64Text)
    {
        try
        {
            Console.WriteLine("Starting with DecodeToPgp");
            var data = Convert.FromBase64String(base64Text);
            var pgpText = Encoding.UTF8.GetString(data);
            Console.WriteLine(pgpText);
            Console.WriteLine("Done with DecodeToPgp");
            return pgpText;
        }
        catch (Exception e)
        {
            Console.WriteLine("Error in DecodeToPgp" + e);
            throw;
        }
    }
}