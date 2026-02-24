using Polly;

namespace OrdersService.Business.Policies;

public interface IUsersMicroservicePolicies
{
    IAsyncPolicy<HttpResponseMessage> GetCombinedPolicy();
}