using Polly;
using Polly.Extensions.Http;
namespace StarterKit.Infrastructure.External.Polly;
public static class PollyPolicies 
{
    public static IAsyncPolicy<HttpResponseMessage> RetryPolicy()
         =>HttpPolicyExtensions.HandleTransientHttpError()
        .WaitAndRetryAsync(3, r=>TimeSpan.FromSeconds(r));
}
