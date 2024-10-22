// Copyright (c) GRCcontrol B.V. All rights reserved.

using ApiBolt.Abstractions;

namespace ApiBolt.Api.UseCases.RiskProfiles;

internal class GetRiskProfileEndpoint : IApiEndpoint
{
    [ApiEndpoint(ApiEndpointType.Get, "/riskprofile")]
    public async Task<string> GetRiskProfile()
    {
        return "piet";
    }
}