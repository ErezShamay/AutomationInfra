using System.Net;
using Splitit.Automation.NG.Backend.BaseActions;
using Splitit.Automation.NG.Backend.Services.V3.DefaultValues;
using Splitit.Automation.NG.Utilities.EnvironmentsConfig;

namespace Splitit.Automation.NG.Backend.Services.DisputesV2.Evidence.Functionality;

public class PostDownloadDisputeIdEvidenceIdFunctionality
{
    private const string EndPoint = "/api/v1/evidences/download/";
    private readonly EnvConfig _envConfig = new();

    public async Task<bool> SendPostRequestPostDownloadDisputeIdEvidenceIdAsync(
        RequestHeader requestHeader, string disputeId, string evidenceId, string filePathDownload)
    {
        try
        {
            Console.WriteLine("\nStarting SendPostRequestPostSendReminderEmailDisputeId");
            var response = await SendPostRequestDownloadFileAsync(
                _envConfig.DisputesV2Url + EndPoint + disputeId + "/" + evidenceId, requestHeader, filePathDownload);
            Console.WriteLine("Done with SendPostRequestPostSendReminderEmailDisputeId\n");
            return response;
        }
        catch (Exception exception)
        {
            Console.WriteLine("Error in SendPostRequestPostSendReminderEmailDisputeId \n" + exception + "\n");
            throw;
        }
    }

    private async Task<bool> SendPostRequestDownloadFileAsync(string uri, RequestHeader requestHeader, string filePathDownload)
    {
        try
        {
            var bearerToken = requestHeader.sessionId;
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + bearerToken);
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