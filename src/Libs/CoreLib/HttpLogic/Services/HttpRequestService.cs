using System.Net;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Text;
using CoreLib.HttpServiceV2.Services.Interfaces;
using CoreLib.HttpLogic.Services.Interfaces;
using CoreLib.TraceLogic.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using CoreLib.HttpLogic.Entities;
using System.Net.Http.Json;

namespace CoreLib.HttpLogic.Services;

/// <inheritdoc />
internal class HttpRequestService : IHttpRequestService
{
    private readonly IHttpConnectionService _httpConnectionService;
    private readonly IEnumerable<ITraceWriter> _traceWriterList;

    ///
    public HttpRequestService(
        IHttpConnectionService httpConnectionService,
        IEnumerable<ITraceWriter> traceWriterList)
    {
        _httpConnectionService = httpConnectionService;
        _traceWriterList = traceWriterList;
    }

    /// <inheritdoc />
    public async Task<HttpResponse<TResponse>> SendRequestAsync<TResponse>(HttpRequestData requestData,
        HttpConnectionData connectionData)
    {
        var client = _httpConnectionService.CreateHttpClient(connectionData);

        HttpContent content = PrepairContent(requestData.Body, requestData.ContentType);

        StringBuilder sbUri = new StringBuilder();

        sbUri.Append(requestData.Uri.ToString());
        sbUri.Append('?');
        foreach (var pair in requestData.QueryParameterList)
        {
            sbUri.Append(pair.Key);
            sbUri.Append('=');
            sbUri.Append(pair.Value);
            sbUri.Append('&');
        }
        sbUri.Remove(sbUri.Length-1, 1);

        Uri resultUri = new Uri(sbUri.ToString());

        var httpRequestMessage = new HttpRequestMessage()
        {
            Method = requestData.Method,
            RequestUri = resultUri,
            Content = content
        };

        foreach (var header in requestData.HeaderDictionary)
        {
            httpRequestMessage.Headers.Add(header.Key, header.Value);
        }

        foreach (var traceWriter in _traceWriterList)
        {
            httpRequestMessage.Headers.Add(traceWriter.Name, traceWriter.GetValue());
        }

        var res = await _httpConnectionService.SendRequestAsync(httpRequestMessage, client, default);
        Console.WriteLine(res.Content.ToString());
        return new HttpResponse<TResponse>
        {
            StatusCode = res.StatusCode,
            Headers = res.Headers,
            ContentHeaders = res.Content.Headers,
            Body = await res.Content.ReadFromJsonAsync<TResponse>()
        };
    }

    private HttpContent PrepairContent(object body, Entities.ContentType contentType)
    {
        switch (contentType)
        {
            case Entities.ContentType.ApplicationJson:
            {
                if (body is string stringBody)
                {
                    body = JToken.Parse(stringBody);
                }

                var serializeSettings = new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    NullValueHandling = NullValueHandling.Ignore
                };
                var serializedBody = JsonConvert.SerializeObject(body, serializeSettings);
                var content = new StringContent(serializedBody, Encoding.UTF8, MediaTypeNames.Application.Json);
                return content;
            }

            case Entities.ContentType.XWwwFormUrlEncoded:
            {
                if (body is not IEnumerable<KeyValuePair<string, string>> list)
                {
                    throw new Exception(
                        $"Body for content type {contentType} must be {typeof(IEnumerable<KeyValuePair<string, string>>).Name}");
                }

                return new FormUrlEncodedContent(list);
            }
            case Entities.ContentType.ApplicationXml:
            {
                if (body is not string s)
                {
                    throw new Exception($"Body for content type {contentType} must be XML string");
                }

                return new StringContent(s, Encoding.UTF8, MediaTypeNames.Application.Xml);
            }
            case Entities.ContentType.Binary:
            {
                if (body.GetType() != typeof(byte[]))
                {
                    throw new Exception($"Body for content type {contentType} must be {typeof(byte[]).Name}");
                }

                return new ByteArrayContent((byte[])body);
            }
            case Entities.ContentType.TextXml:
            {
                if (body is not string s)
                {
                    throw new Exception($"Body for content type {contentType} must be XML string");
                }

                return new StringContent(s, Encoding.UTF8, MediaTypeNames.Text.Xml);
            }
            default:
                throw new ArgumentOutOfRangeException(nameof(contentType), contentType, null);
        }
    }
}