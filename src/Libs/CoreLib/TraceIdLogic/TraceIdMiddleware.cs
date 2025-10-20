using CoreLib.TraceLogic.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace CoreLib.TraceIdLogic
{
    public class TraceIdMiddleware
    {
        private readonly RequestDelegate _requestDelegate;

        public TraceIdMiddleware(RequestDelegate requestDelegate)
        {
            _requestDelegate = requestDelegate;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var serviceProvider = context.RequestServices;
            var traceReaders = serviceProvider.GetRequiredService<IEnumerable<ITraceReader>>();

            foreach (var traceReader in traceReaders)
            {
                if (traceReader.Name == "TraceId" && 
                    context.Request.Headers.TryGetValue("TraceId", out var traceIdValue))
                {
                    traceReader.WriteValue(traceIdValue);
                    break;
                }
            }

            await _requestDelegate(context);
        }
    }
}
