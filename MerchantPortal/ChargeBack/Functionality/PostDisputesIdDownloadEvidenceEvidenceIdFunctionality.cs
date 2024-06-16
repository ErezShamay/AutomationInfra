using Splitit.Automation.NG.Backend.Services.V3.DefaultValues;
using Splitit.Automation.NG.Utilities.EnvironmentsConfig;

namespace Splitit.Automation.NG.Backend.Services.MerchantPortal.ChargeBack.Functionality;

public class PostDisputesIdDownloadEvidenceEvidenceIdFunctionality
{
    private const string EndPoint = "/api/v1/disputes/";
    private readonly EnvConfig _envConfig = new();

    public async Task<bool> SendPostRequestPostDisputesIdDownloadEvidenceEvidenceIdAsync(
        RequestHeader requestHeader, string disputeId, string evidenceId, string filePathDownload, int merchantId)
    {
        try
        {
            Console.WriteLine("\nStarting SendPostRequestPostDisputesIdDownloadEvidenceEvidenceId");
            var response = await SendPostRequestDownloadFileAsync(
                _envConfig.MerchantApiUrl + EndPoint + disputeId + "/download-evidence/" + evidenceId, 
                requestHeader, filePathDownload, merchantId);
            Console.WriteLine("Done with SendPostRequestPostDisputesIdDownloadEvidenceEvidenceId\n");
            return response!;
        }
        catch (Exception exception)
        {
            Console.WriteLine("Error in SendPostRequestPostDisputesIdDownloadEvidenceEvidenceId\n" + exception + "\n");
            throw;
        }
    }
    
    private async Task<bool> SendPostRequestDownloadFileAsync(string uri, RequestHeader requestHeader, string filePathDownload, int merchantId)
    {
        try
        {
            var bearerToken = requestHeader.sessionId;
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + bearerToken);
                httpClient.DefaultRequestHeaders.Add("Merchant-Id", merchantId.ToString());
                HttpContent requestContent = new StringContent("");
                var response = await httpClient.PostAsync(uri, requestContent);
                if (response.IsSuccessStatusCode)
                {
                    using (var fileStream = File.Create(filePathDownload))
                    {
                        await response.Content.CopyToAsync(fileStream);
                    }
                    Console.WriteLine("File downloaded successfully.");
                    return true;
                }
                Console.WriteLine($"Failed to download the file. Status code: {response.StatusCode}");
                return false;
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("Error in SendPostRequestDownloadFile" + e);
            throw;
        }
    }
}