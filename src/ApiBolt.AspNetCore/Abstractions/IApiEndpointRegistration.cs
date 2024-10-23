// Copyright (c) GRCcontrol B.V. All rights reserved.

using Microsoft.AspNetCore.Routing;

namespace ApiBolt.AspNetCore.Abstractions;

public interface IApiEndpointRegistration
{
    void MapEndpoint(IEndpointRouteBuilder app);
}