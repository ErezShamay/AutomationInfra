namespace Splitit.Automation.NG.Backend.Tests.ToolsForCSM;

public class DeleteMerchantsOnStripe
{
    private readonly string _endPoint = "https://api.stripe.com/v1/accounts/";
    
    // [Category("DeleteMerchantsOnStripe")]
    // [Test(Description = "DeleteMerchantsOnStripe")]
    // public async Task TestValidateDeleteMerchantsOnStripe()
    // {
    //     var inputFile = "/Users/erez.shamay/Desktop/Stripe Connect - delete.txt";
    //     await SendDeleteRequest(inputFile);
    // }

    private async Task SendDeleteRequest(string inputFile)
    {
        try
        {
            var allLines = File.ReadAllLines(inputFile);
    
            foreach (var line in allLines)
            {
                var route = _endPoint + line;
                await SendTheRequest(route);
            }
            Console.WriteLine("Done TestValidateCleanUp successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }

    private async Task SendTheRequest(string route)
    {
        var client = new HttpClient();
        var request = new HttpRequestMessage(HttpMethod.Delete, route);
        request.Headers.Add("Authorization", "Basic c2tfbGl2ZV9FMTA4MGp6Q2oxODE0eHM0eDhOTUFja2YwMHdmYkhpVDNhOg==");
        var response = await client.SendAsync(request);
        Console.WriteLine(await response.Content.ReadAsStringAsync());
    }
}