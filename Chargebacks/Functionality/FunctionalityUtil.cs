using System.Net.Http.Headers;
using Newtonsoft.Json;
using Splitit.Automation.NG.Backend.BaseActions;
using Splitit.Automation.NG.Backend.Services.Chargebacks.BaseObjects;
using Splitit.Automation.NG.Backend.Services.Chargebacks.Responses;
using Splitit.Automation.NG.Backend.Services.V3.DefaultValues;
using Splitit.Automation.NG.Utilities.EnvironmentsConfig;

namespace Splitit.Automation.NG.Backend.Services.Chargebacks.Functionality;

public class FunctionalityUtil
{
    private readonly HttpSender _httpSender = new();
    private readonly EnvConfig _envConfig = new();
    private const string EndPoint = "/api/chargebacks/";
    private readonly DeleteChargebacksIdDeleteCommentCommentIdBaseObjects.Root _deleteChargebacksIdDeleteCommentCommentIdBaseObjects = new();
    private readonly PostChargebacksIdAddCommentBaseObjects.Root _postCommentsDisputeIdBaseObjects = new();
    private readonly PutChargebacksIdAcceptBaseObjects.Root _putChargebacksIdAcceptBaseObjects = new();
    private readonly PutChargebackStatusBaseObjects.Root _putChargebackStatusBaseObjects = new();
    private readonly PostChargebacksBaseObjects.Root _postChargebacksBaseObjects = new();
    
    public async Task<string> SendDeleteChargebacksIdDeleteCommentCommentIdAsync(
        RequestHeader requestHeader, string disputeId, string commentId)
    {
        try
        {
            Console.WriteLine("\nStarting SendDeleteChargebacksIdDeleteCommentCommentIdAsync");
            var response = await _httpSender.SendDeleteHttpsRequestAsync(
                _envConfig.ChargebacksUrl + EndPoint + disputeId + "/delete-comment/" + commentId, requestHeader);
            Console.WriteLine("Done with SendDeleteChargebacksIdDeleteCommentCommentIdAsync\n");
            return response;
        }
        catch (Exception exception)
        {
            Console.WriteLine("Error in SendDeleteChargebacksIdDeleteCommentCommentIdAsync \n" + exception + "\n");
            throw;
        }
    }
    
    public async Task<DeleteChargebacksIdDeleteEvidenceResponse.Root> SendDeleteRequestDeleteChargebacksIdDeleteEvidence(
        RequestHeader requestHeader, string disputeId, string evidenceId)
    {
        try
        {
            Console.WriteLine("\nStarting SendDeleteRequestDeleteChargebacksIdDeleteEvidence");
            _deleteChargebacksIdDeleteCommentCommentIdBaseObjects.Ids = new List<string> { evidenceId };
            var route = _envConfig.ChargebacksUrl + EndPoint + disputeId + "/delete-evidence";
            var response = await _httpSender.SendDeleteRequestAsync(
                route, _deleteChargebacksIdDeleteCommentCommentIdBaseObjects, requestHeader);
            var jResponse = JsonConvert.DeserializeObject<DeleteChargebacksIdDeleteEvidenceResponse.Root>(response);
            Console.WriteLine("Done with SendDeleteRequestDeleteChargebacksIdDeleteEvidence\n");
            return jResponse!;
        }
        catch (Exception exception)
        {
            Console.WriteLine("Error in SendDeleteRequestDeleteChargebacksIdDeleteEvidence \n" + exception + "\n");
            throw;
        }
    }
    
    public async Task<GetChargebacksResponse.Root> SendGetRequestGetChargebacksAsync(
        RequestHeader requestHeader, string status, string from, string to, 
        string xSplititSkip, string xSplititTake)
    {
        try
        {
            Console.WriteLine("\nStarting SendGetRequestGetChargebacksAsync");
            var route = _envConfig.ChargebacksUrl + EndPoint + "?status=" + status + "&from=" + from + "&to=" + to;
            var response = await _httpSender.SendGetHttpsRequestAsync(
                route, requestHeader, null!, 
                null!, xSplititSkip, xSplititTake);
            var jResponse = JsonConvert.DeserializeObject<GetChargebacksResponse.Root>(response);
            Console.WriteLine("Done with SendGetRequestGetChargebacksAsync\n");
            return jResponse!;
        }
        catch (Exception exception)
        {
            Console.WriteLine("Error in SendGetRequestGetChargebacksAsync \n" + exception + "\n");
            throw;
        }
    }
    
    public async Task<GetChargebacksIdResponse.Root> SendGetRequestGetChargebacksIdAsync(
        RequestHeader requestHeader, string disputeId)
    {
        try
        {
            Console.WriteLine("\nStarting SendGetRequestGetChargebacksIdAsync");
            await Task.Delay(5*1000);
            var response = await _httpSender.SendGetHttpsRequestAsync(
                _envConfig.ChargebacksUrl + EndPoint + disputeId, requestHeader);
            var jResponse = JsonConvert.DeserializeObject<GetChargebacksIdResponse.Root>(response);
            Console.WriteLine("Done with SendGetRequestGetChargebacksIdAsync\n");
            return jResponse!;
        }
        catch (Exception exception)
        {
            Console.WriteLine("Error in SendGetRequestGetChargebacksIdAsync \n" + exception + "\n");
            throw;
        }
    }
    
    public async Task<PostChargebacksIdAddCommentResponse.Root> SendPostRequestCommentsAsync(
        RequestHeader requestHeader, string disputeId, string text, string createdBy, string? commentId, bool internalComment)
    {
        try
        {
            Console.WriteLine("\nStarting SendPostRequestCommentsAsync");
            await Task.Delay(10 * 1000);
            _postCommentsDisputeIdBaseObjects.CommentId = commentId!;
            _postCommentsDisputeIdBaseObjects.Text = text;
            _postCommentsDisputeIdBaseObjects.CreatedBy = createdBy;
            var response = await _httpSender.SendPostHttpsRequestAsync(
                _envConfig.ChargebacksUrl + EndPoint + disputeId + "/add-comment",
                _postCommentsDisputeIdBaseObjects, requestHeader);
            var jResponse = JsonConvert.DeserializeObject<PostChargebacksIdAddCommentResponse.Root>(response);
            Console.WriteLine("Done with SendPostRequestCommentsAsync\n");
            return jResponse!;
        }
        catch (Exception exception)
        {
            Console.WriteLine("Error in SendPostCommentsDisputeIdRequestAsync \n" + exception + "\n");
            throw;
        }
    }
    
    public async Task<string> SendPostRequestPostUploadAsync(
        RequestHeader requestHeader, string disputeId, bool internalEvidence, 
        string filePath, string name, string fileName)
    {
        try
        {
            Console.WriteLine("Starting SendPostRequestPostUpload");
            var route = _envConfig.ChargebacksUrl + EndPoint + disputeId+ "/upload-evidence";
            var response = await SendPostRequestWithUploadFileAsync(
                route, requestHeader, disputeId, internalEvidence, filePath);
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
    
    public async Task<PutChargebacksIdAcceptResponse.Root?> SendUpdateRequestPutChargebacksIdAcceptAsync(
        RequestHeader requestHeader, string disputeId, bool accept)
    {
        try
        {
            Console.WriteLine("\nStarting SendUpdateRequest");
            _putChargebacksIdAcceptBaseObjects.Accept = accept;
            var route = _envConfig.ChargebacksUrl + EndPoint + disputeId + "/accept";
            var response = await _httpSender.SendPutHttpsRequestAsync(
                route, _putChargebacksIdAcceptBaseObjects, requestHeader);
            var jResponse = JsonConvert.DeserializeObject<PutChargebacksIdAcceptResponse.Root>(response);
            Console.WriteLine("SendUpdateRequest Succeeded\n");
            return jResponse;
        }
        catch (Exception exception)
        {
            Console.WriteLine("SendUpdateRequest Failed \n" + exception);
            throw;
        }
    }

    public async Task<PostChargebacksResponse.Root?> SendPostRequestPostChargebacksAsync(RequestHeader requestHeader,
        string ipn, string captureId, string currency, string reasonCode, double chargeBackAmount)
    {
        try
        {
            Console.WriteLine("Starting SendPostRequestPostChargebacksAsync");
            _postChargebacksBaseObjects.ChargebackCreationDate = DateTime.Now;
            _postChargebacksBaseObjects.RefDisputeId = GuidGenerator.GenerateNewGuid();
            _postChargebacksBaseObjects.DueDateForEvidence = DateTime.Now.AddMonths(1);
            _postChargebacksBaseObjects.InstallmentPlanNumber = ipn;
            _postChargebacksBaseObjects.Amount = chargeBackAmount;
            _postChargebacksBaseObjects.Currency = currency;
            _postChargebacksBaseObjects.ReasonCode = reasonCode;
            _postChargebacksBaseObjects.TransactionSplititReference = captureId;
            var response = await _httpSender.SendPostHttpsRequestAsync(
                _envConfig.ChargebacksUrl + EndPoint,
                _postChargebacksBaseObjects, requestHeader);
            var jResponse = JsonConvert.DeserializeObject<PostChargebacksResponse.Root>(response);
            Console.WriteLine("SendPostRequestPostChargebacksAsync Done");
            return jResponse;
        }
        catch (Exception e)
        {
            Console.WriteLine("Error in SendPostRequestPostChargebacksAsync" + e);
            throw;
        }
    }
    
    public async Task<PutChargebacksIdStatusResponse.Root?> SendUpdateRequestPutChargebacksIdStatusAsync(
        RequestHeader requestHeader, string disputeId, string status)
    {
        try
        {
            Console.WriteLine("\nStarting SendUpdateRequestPutChargebacksIdStatusAsync");
            _putChargebackStatusBaseObjects.Status = status;
            await Task.Delay(5 * 1000);
            var route = _envConfig.ChargebacksUrl + EndPoint + disputeId + "/status";
            var response = await _httpSender.SendPutHttpsRequestAsync(
                route, _putChargebackStatusBaseObjects, requestHeader);
            var jResponse = JsonConvert.DeserializeObject<PutChargebacksIdStatusResponse.Root>(response);
            Console.WriteLine("SendUpdateRequestPutChargebacksIdStatusAsync Succeeded\n");
            return jResponse;
        }
        catch (Exception exception)
        {
            Console.WriteLine("SendUpdateRequestPutChargebacksIdStatusAsync Failed \n" + exception);
            throw;
        }
    }
}