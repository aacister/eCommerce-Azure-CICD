using Polly;

namespace OrdersService.Business.Policies
{
    public interface IProductsMicroservicePolicies
    {
        IAsyncPolicy<HttpResponseMessage> GetFallbackPolicy();
        IAsyncPolicy<HttpResponseMessage> GetBulkheadIsolationPolicy();
    }
}
