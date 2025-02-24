using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Proxy.Shared;

public class GenericHttpClient
{
    private HttpClient _httpClient;
    private bool CheckSSL;
    string _authToken;

    public GenericHttpClient(HttpClient httpClient, bool checkSSL = false)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        CheckSSL = checkSSL;

        _httpClient.DefaultRequestHeaders.Accept.Clear();
        _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        if (checkSSL)
        {
            var handler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (message, cert, chain, sslPolicyErrors) => true
            };

            _httpClient = new HttpClient(handler);
        }
    }

    private void SetBasicAuthentication(string username, string password)
    {
        var byteArray = System.Text.Encoding.ASCII.GetBytes($"{username}:{password}");
        var base64Credentials = Convert.ToBase64String(byteArray);
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic ", base64Credentials);
    }

    public virtual void SetBaseUrl(string url)
    {
        _httpClient.BaseAddress = new Uri(url);
    }

    public virtual string AuthToken
    {
        set
        {
            _authToken = value;
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _authToken);
            _httpClient.Timeout = TimeSpan.FromSeconds(100);
        }
    }

    public void AddHeader(string name, string value)
    {
        _httpClient.DefaultRequestHeaders.Add(name, value);
    }
    public virtual async Task<TResponse> SendAsync<TRequest, TResponse>(string url, TRequest requestBody)
    {
        return await SendHttpRequestAsync<TRequest, TResponse>(HttpMethod.Post, url, requestBody);
    }

    public virtual async Task<TResponse> PostAsync<TRequest, TResponse>(string url, TRequest requestBody)
    {
        return await SendHttpRequestAsync<TRequest, TResponse>(HttpMethod.Post, url, requestBody);
    }

    public virtual async Task<TResponse> GetAsync<TResponse>(string url)
    {
        try
        {
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<TResponse>();
        }
        catch (Exception)
        {
            throw;
        }
    }

    public virtual async Task<TResponse> PutAsync<TRequest, TResponse>(string url, TRequest requestBody)
    {
        var response = await _httpClient.PutAsJsonAsync(url, requestBody);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<TResponse>();
    }

    private async Task<TResponse> SendHttpRequestAsync<TRequest, TResponse>(HttpMethod method, string url, TRequest requestBody)
    {
        using (var request = new HttpRequestMessage(method, url))
        {
            // If requestBody is not null, serialize it and set as content  
            if (requestBody != null)
            {
                request.Content = JsonContent.Create(requestBody);
            }

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<TResponse>();
        }
    }

    public virtual async Task<TResponse> Post<TRequest, TResponse>(TRequest requestBody, string relativeUrl)
    {
        string resStr = null;
        try
        {
            var jsonStr = JsonConvert.SerializeObject(requestBody);
            var buffer = System.Text.Encoding.UTF8.GetBytes(jsonStr);
            var byteContent = new ByteArrayContent(buffer);

            byteContent.Headers.Add("Content-Type", "application/json");

            var res = await _httpClient.PostAsync(relativeUrl, byteContent);
            if (res.IsSuccessStatusCode)
            {
                resStr = await res.Content.ReadAsStringAsync();
                var resultSuccess = JsonConvert.DeserializeObject<TResponse>(resStr, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                if (resultSuccess != null)
                {
                    return resultSuccess;
                }
                else
                {
                    return default;
                }
            }
            else
            {
                switch (res.StatusCode)
                {
                    case HttpStatusCode.Continue:
                        break;
                    case HttpStatusCode.SwitchingProtocols:
                        break;
                    case HttpStatusCode.Processing:
                        break;
                    case HttpStatusCode.EarlyHints:
                        break;
                    case HttpStatusCode.OK:
                        break;
                    case HttpStatusCode.Created:
                        break;
                    case HttpStatusCode.Accepted:
                        break;
                    case HttpStatusCode.NonAuthoritativeInformation:
                        break;
                    case HttpStatusCode.NoContent:
                        break;
                    case HttpStatusCode.ResetContent:
                        break;
                    case HttpStatusCode.PartialContent:
                        break;
                    case HttpStatusCode.MultiStatus:
                        break;
                    case HttpStatusCode.AlreadyReported:
                        break;
                    case HttpStatusCode.IMUsed:
                        break;
                    case HttpStatusCode.Ambiguous:
                        break;
                    case HttpStatusCode.Moved:
                        break;
                    case HttpStatusCode.Found:
                        break;
                    case HttpStatusCode.RedirectMethod:
                        break;
                    case HttpStatusCode.NotModified:
                        break;
                    case HttpStatusCode.UseProxy:
                        break;
                    case HttpStatusCode.Unused:
                        break;
                    case HttpStatusCode.RedirectKeepVerb:
                        break;
                    case HttpStatusCode.PermanentRedirect:
                        break;
                    case HttpStatusCode.BadRequest:
                        break;
                    case HttpStatusCode.Unauthorized:
                        break;
                    case HttpStatusCode.PaymentRequired:
                        break;
                    case HttpStatusCode.Forbidden:
                        break;
                    case HttpStatusCode.NotFound:
                        break;
                    case HttpStatusCode.MethodNotAllowed:
                        break;
                    case HttpStatusCode.NotAcceptable:
                        break;
                    case HttpStatusCode.ProxyAuthenticationRequired:
                        break;
                    case HttpStatusCode.RequestTimeout:
                        resStr = await res.Content.ReadAsStringAsync();
                        var result = JsonConvert.DeserializeObject<TResponse>(resStr, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                        return result;
                    case HttpStatusCode.Conflict:
                        break;
                    case HttpStatusCode.Gone:
                        break;
                    case HttpStatusCode.LengthRequired:
                        break;
                    case HttpStatusCode.PreconditionFailed:
                        break;
                    case HttpStatusCode.RequestEntityTooLarge:
                        break;
                    case HttpStatusCode.RequestUriTooLong:
                        break;
                    case HttpStatusCode.UnsupportedMediaType:
                        break;
                    case HttpStatusCode.RequestedRangeNotSatisfiable:
                        break;
                    case HttpStatusCode.ExpectationFailed:
                        break;
                    case HttpStatusCode.MisdirectedRequest:
                        break;
                    case HttpStatusCode.UnprocessableEntity:
                        break;
                    case HttpStatusCode.Locked:
                        break;
                    case HttpStatusCode.FailedDependency:
                        break;
                    case HttpStatusCode.UpgradeRequired:
                        break;
                    case HttpStatusCode.PreconditionRequired:
                        break;
                    case HttpStatusCode.TooManyRequests:
                        break;
                    case HttpStatusCode.RequestHeaderFieldsTooLarge:
                        break;
                    case HttpStatusCode.UnavailableForLegalReasons:
                        break;
                    case HttpStatusCode.InternalServerError:
                        break;
                    case HttpStatusCode.NotImplemented:
                        break;
                    case HttpStatusCode.BadGateway:
                        break;
                    case HttpStatusCode.ServiceUnavailable:
                        break;
                    case HttpStatusCode.GatewayTimeout:
                        break;
                    case HttpStatusCode.HttpVersionNotSupported:
                        break;
                    case HttpStatusCode.VariantAlsoNegotiates:
                        break;
                    case HttpStatusCode.InsufficientStorage:
                        break;
                    case HttpStatusCode.LoopDetected:
                        break;
                    case HttpStatusCode.NotExtended:
                        break;
                    case HttpStatusCode.NetworkAuthenticationRequired:
                        break;
                    default:
                        return default;
                }
                return default;
            }
        }
        catch (Exception)
        {
            return default;
        }
    }
}