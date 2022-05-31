using Polly;
using Polly.Registry;
using Polly.Wrap;

namespace CleanArchitecture.DDD.Core.Polly;

public interface IPolicyHolder
{
    PolicyRegistry Registry { get; }

    IAsyncPolicy<HttpResponseMessage> GetPolicy(HttpPolicyNames httpPolicyNames);

    IAsyncPolicy GetPolicy(PolicyNames policyName);

    AsyncPolicyWrap<HttpResponseMessage> GetPolicy(WrappedPolicyNames wrappedPolicyNames);

    IAsyncPolicy<HttpResponseMessage> PolicySelector(HttpRequestMessage httpRequestMessage);
}