// Copyright (c) GRCcontrol B.V. All rights reserved.

using ApiBolt.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace ApiBolt.Api.UseCases.RiskProfiles;

internal class GetRiskProfileEndpoint : IApiEndpoint
{
    [ApiEndpoint(ApiEndpointType.Get, "/piet")]
    public async Task<string> GetRiskProfile()
    {
        return $"piet";
    }
}