namespace Splitit.Automation.NG.Backend.BaseActions;

public class ExtractHeadersController
{
    public (string?, string?, string?) ExtractHeaders(HttpResponseMessage response)
    {
        try
        {
            Console.WriteLine("Starting ExtractHeaders");
            string responseSignature = null!;
            string? responseSignatureKeyId = null!;
            string? responseSignatureToken = null!;
            foreach (var header in response.Headers)
            {
                switch (header.Key)
                {
                    case "x-splitit-responsesignature":
                        responseSignature = header.Value.ToList()[0];
                        break;
                    case "x-splitit-responsesignature-keyid":
                        responseSignatureKeyId = header.Value.ToList()[0];
                        break;
                    case "x-splitit-responsesignature-token":
                        responseSignatureToken = header.Value.ToList()[0];
                        break;
                }
            }
            Console.WriteLine("Done With ExtractHeaders");
            return (responseSignature, responseSignatureKeyId, responseSignatureToken);
        }
        catch (Exception e)
        {
            Console.WriteLine("Error in ExtractHeaders" + e);
            throw;
        }
    }
}