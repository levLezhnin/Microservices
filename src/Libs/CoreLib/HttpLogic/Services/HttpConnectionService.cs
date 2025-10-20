using CoreLib.HttpLogic.Entities;
using CoreLib.HttpLogic.Services.Interfaces;
using Polly;
using System.Net;

namespace CoreLib.HttpLogic.Services;

/// <inheritdoc />
internal class HttpConnectionService : IHttpConnectionService
{
    private readonly IHttpClientFactory _httpClientFactory;

    ///
    public HttpConnectionService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    /// <inheritdoc />
    public HttpClient CreateHttpClient(HttpConnectionData httpConnectionData)
    {
        var httpClient = string.IsNullOrWhiteSpace(httpConnectionData.ClientName)
            ? _httpClientFactory.CreateClient()
            : _httpClientFactory.CreateClient(httpConnectionData.ClientName);
            
        if (httpConnectionData.Timeout != null)
        {
            httpClient.Timeout = httpConnectionData.Timeout.Value;
        }
        
        return httpClient;
    }

    /// <inheritdoc />
    public async Task<HttpResponseMessage> SendRequestAsync(HttpRequestMessage httpRequestMessage, HttpClient httpClient, CancellationToken cancellationToken, HttpCompletionOption httpCompletionOption = HttpCompletionOption.ResponseContentRead)
    {
        var response = await Policy
            .Handle<HttpRequestException>()
            .WaitAndRetryAsync(
                10, 
                retryAttempt => TimeSpan.FromSeconds(5 + retryAttempt),
                onRetry: (result, timespan, retryCount, context) =>
                {
                    Console.WriteLine($"Начало {retryCount} Попытки повтора");
                }
            )
            .ExecuteAsync(
                async () => await httpClient.SendAsync(httpRequestMessage, httpCompletionOption, cancellationToken)
            );

        return response;
    }
}