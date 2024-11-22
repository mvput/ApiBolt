using Microsoft.AspNetCore.Builder;

namespace ApiBolt.AspNetCore.Abstractions;

public interface IApiEndpointConvention
{
   static abstract void Configure(IEndpointConventionBuilder builder);
}