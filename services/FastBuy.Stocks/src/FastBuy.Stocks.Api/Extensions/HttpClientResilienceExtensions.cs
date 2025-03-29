using Polly.Extensions.Http;
using Polly.Timeout;
using Polly;

namespace FastBuy.Stocks.Api.Extensions
{
    public static class HttpClientResilienceExtensions
    {
        public static IHttpClientBuilder AddResiliencePolicies(this IHttpClientBuilder builder)
        {
            //Timeout Policy 
            var timeoutPolicy = Policy.TimeoutAsync<HttpResponseMessage>(TimeSpan.FromSeconds(3), TimeoutStrategy.Pessimistic);

            //Retry policy
            var randomJitter = new Random();

            var retryPolicy = HttpPolicyExtensions
                .HandleTransientHttpError()
                .Or<TimeoutRejectedException>()
                .WaitAndRetryAsync(
                    retryCount: 3,
                    sleepDurationProvider: retryAtempt =>
                        TimeSpan.FromSeconds(Math.Pow(2, retryAtempt)) +
                        TimeSpan.FromMicroseconds(randomJitter.Next(0, 100)),
                    onRetry: (outcome, timespan, retryAttempt, context) =>
                        Console.WriteLine($"[RETRY] Attempt {retryAttempt} failed, retrying in {timespan.TotalSeconds} seconds")
                );

            //CircuitBreaker Policy
            var circuitBreaker = Policy<HttpResponseMessage>
                .Handle<HttpRequestException>()
                .Or<TimeoutRejectedException>()
                .CircuitBreakerAsync(
                    handledEventsAllowedBeforeBreaking: 2,
                    durationOfBreak: TimeSpan.FromSeconds(8),
                    onBreak: (outcome, timespan) =>
                       Console.WriteLine(
                           $"[CIRCUIT BREAKER] The circuit has been opened for {timespan.Seconds} seconds, due to {outcome.Exception}"),
                    onReset: () =>
                        Console.WriteLine($"[CIRCUIT BREAKER] The circuit has close")
                );


            return builder.AddPolicyHandler(retryPolicy)
                          .AddPolicyHandler(timeoutPolicy)
                          .AddPolicyHandler(circuitBreaker);
        }
    }
}
