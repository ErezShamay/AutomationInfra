using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Net.Security;
using System.Text;
using Newtonsoft.Json;
using Splitit.Automation.NG.Backend.Services.V3.DefaultValues;

namespace Splitit.Automation.NG.Backend.BaseActions;

public class HttpSender
{
    private readonly HttpClient _httpClient;
    public HttpSender()
    {
        _httpClient = new HttpClient();
        ServicePointManager.ServerCertificateValidationCallback += new RemoteCertificateValidationCallback((sender, certificate, chain, policyErrors) => true);
    }

    public async Task<string> SendPostHttpRequestStringBody(string endPoint, string bodyToSend)
    {
        try
        {
            //var jsonContent = JsonContent.Create(bodyToSend);
            using (var req = new HttpRequestMessage(HttpMethod.Post, new Uri(endPoint)))
            {
                req.Content = new StringContent(bodyToSend, Encoding.UTF8, "application/x-www-form-urlencoded");
                //req.Headers.Add("Accept", "application/x-www-form-urlencoded");

                var res = await _httpClient.SendAsync(req);
                return await res.Content.ReadAsStringAsync();
            }

        }
        catch (Exception exception)
        {
            Console.WriteLine("did not managed to send https POST request to: " + endPoint + " \n" + exception);
            throw;
        }
    }

    public async Task<string> SendPostRequestStringBody(string endPoint, RequestHeader requestHeader, 
        string requestBody, string kesFlag = default!, string requestSignature = default!,
        string requestSignatureKeyId = default!, string requestSignatureClientId = default!)
    {
        using (var client = new HttpClient())
        {
            try
            {
                var content = new StringContent(requestBody);
                Console.WriteLine("String request: \n" + content);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", requestHeader.sessionId);
                if (kesFlag != null!)
                {
                    client.DefaultRequestHeaders.Add("X-Splitit-RequestSignature", requestSignature);
                    client.DefaultRequestHeaders.Add("X-Splitit-RequestSignature-KeyId", requestSignatureKeyId);
                    client.DefaultRequestHeaders.Add("X-Splitit-RequestSignature-ClientId", requestSignatureClientId);
                }
                content.Headers.ContentType = new MediaTypeHeaderValue("application/jose+json");
                var response = await client.PostAsync(endPoint, content);
                string responseContent = null!;
                if (response.IsSuccessStatusCode)
                {
                    responseContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine("Response from server: " + responseContent);
                }
                else
                {
                    Console.WriteLine("Failed to make a successful request. Status code: " + response.StatusCode);
                }
                return responseContent;
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("Exception Caught!");
                Console.WriteLine("Message :{0} ", e.Message);
                throw;
            }
        }
    }
    
    public async Task<string> SendPostHttpsRequestAsync(string endPoint, object obj, RequestHeader requestHeader,
        string? googleFlag = default!, string? idempotencyKey = default!, string? merchantPortalFlag = default!, 
        string merchantId = default!, string kesFlag = default!, string requestSignature = default!,
        string requestSignatureKeyId = default!, string requestSignatureClientId = default!, 
        string tokenFlag = default!, string auth = default!, string spreedlyTokenFlag = default!,
        string testModeFlag = default!, string testModeValue = default!)
    {
        try
        {
            using var httpClient = new HttpClient();
            var jsonString = JsonConvert.SerializeObject(obj, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            });
            Console.WriteLine("String request: \n" + jsonString);
            HttpContent content = new StringContent(jsonString, Encoding.UTF8, "application/json");
            var request = new HttpRequestMessage()
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri(endPoint)
            };
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            if (merchantPortalFlag != null)
            {
                httpClient.DefaultRequestHeaders.Add("Merchant-Id", merchantId);
            }
            if (googleFlag != null)
            {
                httpClient.DefaultRequestHeaders.Add("X-Splitit-IdempotencyKey", idempotencyKey);
            }
            if (kesFlag != null!)
            {
                httpClient.DefaultRequestHeaders.Add("X-Splitit-RequestSignature", requestSignature);
                httpClient.DefaultRequestHeaders.Add("X-Splitit-RequestSignature-KeyId", requestSignatureKeyId);
                httpClient.DefaultRequestHeaders.Add("X-Splitit-RequestSignature-ClientId", requestSignatureClientId);
            }

            if (testModeFlag != null!)
            {
                httpClient.DefaultRequestHeaders.Add("X-Splitit-TestMode", testModeValue);
            }
            if (tokenFlag != null!)
            {
                httpClient.DefaultRequestHeaders.Add("Authorization", "Basic " + auth);
            }
            if (spreedlyTokenFlag != null!)
            {
                httpClient.DefaultRequestHeaders.Add("Authorization", auth);
            }
            if (kesFlag == null! && tokenFlag == null! && spreedlyTokenFlag == null!)
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", requestHeader.sessionId);
            }
            httpClient.BaseAddress = request.RequestUri;
            request.Content = content;
            var stringResult = await ExecuteSendAsyncRequestAsync(httpClient, request);
            Console.WriteLine("String result: \n" + stringResult);
            return stringResult;
        }
        catch (Exception exception)
        {
            await Console.Error.WriteLineAsync("did not managed to send https POST request to: " + endPoint + " \n" + exception);
            throw;
        }
    }

    private async Task<string> ExecuteSendAsyncRequestAsync(HttpClient httpClient, HttpRequestMessage request)
    {
        var response = await httpClient.SendAsync(request);
        return await response.Content.ReadAsStringAsync();
    }

    public async Task<string> SendGetHttpsRequestAsync(string url, RequestHeader requestHeader, 
        string? merchantPortalFlag = default!, string merchantId = default!, 
        string xSplititSkip = default!, string xSplititTake = default!)
    {
        try
        {
            using var httpClient = new HttpClient();
            var request = new HttpRequestMessage()
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(url)
            };
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", requestHeader.sessionId);
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            if (merchantPortalFlag != null)
            {
                httpClient.DefaultRequestHeaders.Add("Merchant-Id", merchantId);
            }
            if (xSplititSkip != null!)
            {
                httpClient.DefaultRequestHeaders.Add("X-Splitit-Skip", xSplititSkip);
                httpClient.DefaultRequestHeaders.Add("X-Splitit-Take", xSplititTake);
            }
            httpClient.BaseAddress = request.RequestUri;
            Console.WriteLine("request uri: \n" + request.RequestUri);
            var stringResult = await ExecuteSendAsyncRequestAsync(httpClient, request);
            Console.WriteLine("String result: \n" + stringResult);
            return stringResult;
        }
        catch (Exception exception)
        {
            Console.WriteLine("did not managed to send https GET request to -> " + url + " \n" + exception);
            throw;
        }
    }
    
    public async Task<string> SendGetWithBodyHttpsRequestAsync(string url, RequestHeader requestHeader, string stringBody)
    {
        try
        {
            using var httpClient = new HttpClient();
            var request = new HttpRequestMessage()
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(url)
            };
            httpClient.BaseAddress = request.RequestUri;
            var content = new StringContent(stringBody, null, "application/json");
            request.Content = content;
            Console.WriteLine("String request: \n" + stringBody);
            var stringResult = await ExecuteSendAsyncRequestAsync(httpClient, request);
            Console.WriteLine("String result: \n" + stringResult);
            return stringResult;
        }
        catch (Exception exception)
        {
            Console.WriteLine("did not managed to send https GET request to -> " + url + " \n" + exception);
            throw;
        }
    }

    public async Task<string> SendPutHttpsRequestAsync(string endPoint, object obj, RequestHeader requestHeader,
        string? merchantPortalFlag = default!, string merchantId = default!)
    {
        try
        {
            using var httpClient = new HttpClient();
            var jsonString = JsonConvert.SerializeObject(obj);
            Console.WriteLine("String body: \n" + jsonString);
            HttpContent content = new StringContent(jsonString, Encoding.UTF8, "application/json");
            var request = new HttpRequestMessage()
            {
                Method = HttpMethod.Put,
                RequestUri = new Uri(endPoint)
            };
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", requestHeader.sessionId);
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            if (merchantPortalFlag != null)
            {
                httpClient.DefaultRequestHeaders.Add("Merchant-Id", merchantId);
            }
            httpClient.BaseAddress = request.RequestUri;
            request.Content = content;
            var stringResult = await ExecuteSendAsyncRequestAsync(httpClient, request);
            Console.WriteLine("String result: \n" + stringResult);
            return stringResult;
        }
        catch (Exception exception)
        {
            await Console.Error.WriteLineAsync("did not managed to send https PUT request to: " + endPoint + " \n" + exception);
            throw;
        }
    }

    public async Task<string> SendDeleteHttpsRequestAsync(string url, RequestHeader requestHeader,
        string? merchantPortalFlag = default!, string merchantId = default!)
    {
        try
        {
            using var httpClient = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Delete,
                RequestUri = new Uri(url)
            };
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", requestHeader.sessionId);
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            if (merchantPortalFlag != null)
            {
                httpClient.DefaultRequestHeaders.Add("Merchant-Id", merchantId);
            }
            httpClient.BaseAddress = request.RequestUri;
            Console.WriteLine("String uri \n" + request.RequestUri);
            var stringResult = await ExecuteSendAsyncRequestAsync(httpClient, request);
            Console.WriteLine("String result: \n" + stringResult);
            return stringResult;
        }
        catch (Exception exception)
        {
            Console.WriteLine("did not managed to send Delete request to -> " + url + " \n" + exception);
            throw;
        }
    }

    public async Task<(string responseBody, HttpResponseMessage response)> SendPostRequestReturnResponseStringAndHeaders(string url, Object obj, string bearerToken)
    {
        using (var client = new HttpClient())
        {
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", bearerToken);
            var jsonString = JsonConvert.SerializeObject(obj, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            });
            Console.WriteLine("String request: \n" + jsonString);
            var content = new StringContent(jsonString, System.Text.Encoding.UTF8, "application/json");
            var response = await client.PostAsync(url, content);
            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("Response Headers:");
                foreach (var header in response.Headers)
                {
                    Console.WriteLine($"{header.Key}: {string.Join(",", header.Value)}");
                }
                var responseBody = await response.Content.ReadAsStringAsync();
                Console.WriteLine("Response Body:");
                Console.WriteLine(responseBody);
                return (responseBody, response);
            }
            Console.WriteLine($"Request failed with status code: {response.StatusCode}");
            return ("", null)!;
        }
    }

    public async Task<string> SendDeleteRequestAsync(string endPoint, object obj, RequestHeader requestHeader)
    {
        try
        {
            using var httpClient = new HttpClient();
            var jsonString = JsonConvert.SerializeObject(obj, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            });
            Console.WriteLine("String request: \n" + jsonString);
            HttpContent content = new StringContent(jsonString, Encoding.UTF8, "application/json");
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Delete,
                RequestUri = new Uri(endPoint)
            };
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", requestHeader.sessionId);
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            httpClient.BaseAddress = request.RequestUri;
            request.Content = content;
            var stringResult = await ExecuteSendAsyncRequestAsync(httpClient, request);
            Console.WriteLine("String result: \n" + stringResult);
            return stringResult;
        }
        catch (Exception exception)
        {
            await Console.Error.WriteLineAsync("did not managed to send https POST request to: " + endPoint + " \n" +
                                               exception);
            throw;
        }
    }
    
    public async Task<string> SendPostRequestConnectTokenAsync(string endPoint, RequestHeader requestHeader, 
        string grantType, string scope, string clientId, string clientSecret)
    {
        try
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                "Bearer", requestHeader.sessionId);
            var request = new HttpRequestMessage(HttpMethod.Post, endPoint);
            var collection = new List<KeyValuePair<string, string>>
            {
                new("grant_type", grantType),
                new("scope", scope),
                new("client_id", clientId),
                new("client_secret", clientSecret)
            };
            var content = new FormUrlEncodedContent(collection);
            request.Content = content;
            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            var responseContent = await response.Content.ReadAsStringAsync();
            Console.WriteLine(responseContent);
            return responseContent;
        }
        catch (Exception exception)
        {
            await Console.Error.WriteLineAsync("did not managed to send https POST request to: " + endPoint + " \n" + exception);
            throw;
        }
    }
}