// Copyright (c) GRCcontrol B.V. All rights reserved.

using Microsoft.AspNetCore.Routing;

namespace ApiBolt.Abstractions;

public interface IApiEndpointRegistration
{
    void MapEndpoint(IEndpointRouteBuilder app);
}