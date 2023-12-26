#region Usings

using Polly;
using Polly.CircuitBreaker;
using Polly.Extensions.Http;
using Polly.Retry;
using Polly.Timeout;

#endregion

namespace Records.Shared.Infra.Http.Policies;

/// <summary>
/// Represents a helper class to provive commons http policies that reads settings from the
/// appsettings.json file.
/// </summary>
public static class PolicyProvider
{
    #region Public methods

    /// <summary>
    /// Gets a Retry policy (to call it as Func).
    /// </summary>
    /// <param name="settings">The settings mapped from the appsettings.json file.</param>
    /// <param name="serviceProvider">The service provider.</param>
    /// <param name="request">The request to to get the uri (for logging).</param>
    /// <returns>An <see cref="IAsyncPolicy"/>.</returns>
    public static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy(
        IPolicySettings settings,
        IServiceProvider serviceProvider,
        HttpRequestMessage request)
    {
        ArgumentNullException.ThrowIfNull(settings);

        AsyncRetryPolicy<HttpResponseMessage> policy = HttpPolicyExtensions
            .HandleTransientHttpError()
            .Or<TimeoutRejectedException>() // HttpError or Polly.TimeoutRejectedException.
            .WaitAndRetryAsync(
                settings.RetryCount, // 3 times.
                (attempt) =>
                {
                    // First time wait 500ms, second 1000...
                    TimeSpan timeToRetry = TimeSpan.FromMilliseconds(settings.TimeToRetry * attempt); // 500ms.
                    Console.WriteLine($"[Policy:WaitAndRetry] Delaying for {timeToRetry.TotalMilliseconds}ms, then making retry {attempt}. URI: {request.RequestUri}");

                    return timeToRetry;
                },
                onRetry: (_, _, _, _) => // (result, timespan, retryNo, context).
                {
                    ////Console.WriteLine($"{context.OperationKey}: Retry number {retryNo} within {timespan.TotalMilliseconds}ms.");
                });

        ////AsyncRetryPolicy<HttpResponseMessage> policy = HttpPolicyExtensions
        ////    .HandleTransientHttpError()
        ////    .RetryAsync(3);

        return policy;
    }

    /// <summary>
    /// Gets a CircuitBreaker policy.
    /// </summary>
    /// <param name="settings">The settings mapped from the appsettings.json file.</param>
    /// <returns>An <see cref="IAsyncPolicy"/>.</returns>
    public static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy(IPolicySettings settings)
    {
        ArgumentNullException.ThrowIfNull(settings);

        AsyncCircuitBreakerPolicy<HttpResponseMessage> policy = HttpPolicyExtensions
            .HandleTransientHttpError()
            .Or<TimeoutRejectedException>() // HttpError or Polly.TimeoutRejectedException.
            .CircuitBreakerAsync(
                settings.HandledEventsAllowedBeforeBreaking, // 3 times.
                TimeSpan.FromMilliseconds(settings.DurationOfBreak), // 10000ms.
                onBreak: (outcome, timespam) =>
                {
                    string requestUri = outcome?.Result?.RequestMessage?.RequestUri?.ToString() ?? "(unknown)";

                    ////System.Diagnostics.Debug.WriteLine($"Opening the circuit for {timespam.TotalMilliseconds}ms. URI: {request.RequestUri}");
                    Console.WriteLine($"[Policy:CircuitBreaker] Opening the circuit for {timespam.TotalMilliseconds}ms. URI: {requestUri}");
                    ////_logger.LogInformation($"[Policy:CircuitBreaker] Opening the circuit for {timespam.TotalMilliseconds}ms. URI: {request.RequestUri}");
                },
                onReset: () =>
                {
                    ////System.Diagnostics.Debug.WriteLine("Closing the circuit. URI: {request.RequestUri}");
                    Console.WriteLine($"[Policy:CircuitBreaker] Closing the circuit. URI: ?");
                    ////_logger.LogInformation($"[Policy:CircuitBreaker] Closing the circuit. URI: {request.RequestUri}");
                });

        return policy;
    }

    // [!] Not working when call it as Func<> to send the arguments.
    // See: https://stackoverflow.com/a/53996441/15711876
    // Like this: .AddPolicyHandler((provider, request) => GetCircuitBreakerPolicy(provider, request));
    //  ||
    //  \/

    /// <summary>
    /// Gets a CircuitBreaker policy (to call it as Func).
    /// </summary>
    /// <param name="settings">The settings mapped from the appsettings.json file.</param>
    /// <param name="serviceProvider">The service provider.</param>
    /// <param name="request">The request to to get the uri (for logging).</param>
    /// <returns>An <see cref="IAsyncPolicy"/>.</returns>
    // ReSharper disable once UnusedMember.Global
    public static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy(
        IPolicySettings settings,
        IServiceProvider serviceProvider,
        HttpRequestMessage request)
    {
        ArgumentNullException.ThrowIfNull(settings);

        AsyncCircuitBreakerPolicy<HttpResponseMessage> policy = HttpPolicyExtensions
            .HandleTransientHttpError()
            .Or<TimeoutRejectedException>() // HttpError or Polly.TimeoutRejectedException.
            .CircuitBreakerAsync(
                settings.HandledEventsAllowedBeforeBreaking, // 3 times.
                TimeSpan.FromMilliseconds(settings.DurationOfBreak), // 10000ms.
                onBreak: (_, timespam) => // (outcome, timespan).
                {
                    ////System.Diagnostics.Debug.WriteLine($"Opening the circuit for {timespam.TotalMilliseconds}ms. URI: {request.RequestUri}");
                    Console.WriteLine($"[Policy:CircuitBreaker] Opening the circuit for {timespam.TotalMilliseconds}ms. URI: {request.RequestUri}");
                    ////_logger.LogInformation($"[Policy:CircuitBreaker] Opening the circuit for {timespam.TotalMilliseconds}ms. URI: {request.RequestUri}");
                },
                onReset: () =>
                {
                    ////System.Diagnostics.Debug.WriteLine("Closing the circuit. URI: {request.RequestUri}");
                    Console.WriteLine($"[Policy:CircuitBreaker] Closing the circuit. URI: {request.RequestUri}");
                    ////_logger.LogInformation($"[Policy:CircuitBreaker] Closing the circuit. URI: {request.RequestUri}");
                });

        return policy;
    }

    /// <summary>
    /// Gets a TimeOut policy (to call it as Func).
    /// </summary>
    /// <param name="settings">The settings mapped from the appsettings.json file.</param>
    /// <param name="serviceProvider">The service provider.</param>
    /// <param name="request">The request to to get the uri (for logging).</param>
    /// <returns>An <see cref="IAsyncPolicy"/>.</returns>
    public static IAsyncPolicy<HttpResponseMessage> GetTimeOutPolicy(
        IPolicySettings settings,
        IServiceProvider serviceProvider,
        HttpRequestMessage request)
    {
        ArgumentNullException.ThrowIfNull(settings);

        // Throws Polly.Timeout.TimeoutRejectedException.

        double timeoutMilliSecs = settings.TimeoutTime; // Time out the request after 4000 ms.

        AsyncTimeoutPolicy<HttpResponseMessage> policy = Policy.TimeoutAsync<HttpResponseMessage>(
            TimeSpan.FromMilliseconds(timeoutMilliSecs),
            TimeoutStrategy.Pessimistic,
            onTimeoutAsync: (_, timespan, _, _) => // (context, timespan, task, exception)
            {
                Console.WriteLine(
                    $"[Policy:Timeout] Execution timed out after {timespan.TotalSeconds} seconds. URI: {request.RequestUri}");
                return Task.CompletedTask;
            });

        return policy;
    }

    /*
    public static IAsyncPolicy GetTimeOutPolicy()
    {
        int timeoutInMilliSecs = 15000; // Time out the request after 15000 ms.

        AsyncTimeoutPolicy? policy = Policy.TimeoutAsync(
            TimeSpan.FromMilliseconds(timeoutInMilliSecs),
            TimeoutStrategy.Pessimistic);

        return policy;
    }
    */

    #endregion
}
