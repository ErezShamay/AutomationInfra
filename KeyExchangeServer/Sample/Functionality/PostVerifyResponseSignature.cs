using System.Security.Cryptography;
using System.Text;

namespace Splitit.Automation.NG.Backend.Services.KeyExchangeServer.Sample.Functionality;

public class PostVerifyResponseSignature
{
    public bool VerifyResponseSignature(string stringBodyResponse , string responseSignature, 
        string responseSignatureKey, string responseSignatureToken )
    {
        try
        {
            Console.WriteLine("Starting VerifyResponseSignature");
            var rsa = RSA.Create();
            rsa.ImportSubjectPublicKeyInfo(Convert.FromBase64String(responseSignatureKey), out _);
            var toVerify = $"{stringBodyResponse};{200};{responseSignatureToken}";
            var signatureBytes = Convert.FromBase64String(responseSignature);
            var x = rsa.VerifyData(Encoding.UTF8.GetBytes(toVerify), signatureBytes, HashAlgorithmName.SHA384,
                RSASignaturePadding.Pss);
            Console.WriteLine("Done with VerifyResponseSignature");
            return x;
        }
        catch (Exception exception)
        {
            Console.WriteLine("Error in VerifyResponseSignature" + exception);
            throw;
        }
    }
}