

using Microsoft.Extensions.Logging;
using Polly.Retry;
using Polly;
using Polly.CircuitBreaker;
using Polly.Timeout;
using Polly.Wrap;

namespace OrdersService.Business.Policies
{
    public class UsersMicroservicePolicies : IUsersMicroservicePolicies
    {
        private readonly ILogger<UsersMicroservicePolicies> _logger;
        private readonly IPollyPolicies _policies;
        public UsersMicroservicePolicies(
            ILogger<UsersMicroservicePolicies> logger,
            IPollyPolicies policies)
        {
            _logger = logger;
            _policies = policies;
        }

        public IAsyncPolicy<HttpResponseMessage> GetCombinedPolicy()
        {
            var retryPolicy = _policies.GetRetryPolicy(5);
            var circuitBreakerPolicy = _policies.GetCircuitBreakerPolicy(3, TimeSpan.FromMinutes(2));
            var timeoutPolicy = _policies.GetTimeoutPolicy(TimeSpan.FromSeconds(5));

            AsyncPolicyWrap<HttpResponseMessage> wrappedPolicy = Policy.WrapAsync(retryPolicy, circuitBreakerPolicy, timeoutPolicy);
            return wrappedPolicy;
        }
    }
}
