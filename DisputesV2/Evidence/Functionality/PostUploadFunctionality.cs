using System.Net.Http.Headers;
using Splitit.Automation.NG.Backend.Services.V3.DefaultValues;
using Splitit.Automation.NG.Utilities.EnvironmentsConfig;

namespace Splitit.Automation.NG.Backend.Services.DisputesV2.Evidence.Functionality;

public class PostUploadFunctionality
{
    private const string EndPoint = "/api/v1/evidences/upload";
    private readonly EnvConfig _envConfig = new();

    public async Task<string> SendPostRequestPostUploadAsync(
        RequestHeader requestHeader, string disputeId, bool internalEvidence, 
        string filePath, string name, string fileName)
    {
        try
        {
            Console.WriteLine("Starting SendPostRequestPostUpload");
            var response = await SendPostRequestWithUploadFileAsync(
                _envConfig.DisputesV2Url + EndPoint, requestHeader, disputeId, internalEvidence, filePath);
            Console.WriteLine("Done with SendPostRequestPostUpload");
            return response;
        }
        catch (Exception exception)
        {
            Console.WriteLine("Error in SendPostRequestPostUpload -> " + exception);
            throw;
        }
    }
    
    private async Task<string> SendPostRequestWithUploadFileAsync(string uri, RequestHeader requestHeader, string disputeId
        , bool internalEvidence ,string filePath)
    {
        try
        {
            Console.WriteLine("Starting SendPostRequestWithUploadFile");
            using var client = new HttpClient();
            using var request = new HttpRequestMessage(HttpMethod.Post, uri);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", requestHeader.sessionId);
            request.Headers.Add("accept", "text/plain");
            using MemoryStream memoryStream = new(await File.ReadAllBytesAsync(filePath));
            var content = new MultipartFormDataContent
            {
                { new StringContent(disputeId), "DisputeId" },
                { new StringContent(internalEvidence.ToString()), "InternalEvidence" }
            };
            var contentFile = new StreamContent(memoryStream);
            contentFile.Headers.ContentType = MediaTypeHeaderValue.Parse("application/pdf");
            content.Add(contentFile, "File", "File");
            request.Content = content;
            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            Console.WriteLine("Done with SendPostRequestWithUploadFile");
            return response.ToString();
        }
        catch (Exception exception)
        {
            Console.WriteLine("Error in  SendPostRequestWithUploadFile\n" + exception);
            throw;
        }
    }
}