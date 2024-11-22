// Copyright (c) GRCcontrol B.V. All rights reserved.

using ApiBolt;
using ApiBolt.Api.UseCases.Weather;
using ApiBolt.AspNetCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddApiBolt();

var app = builder.Build();
app.MapApiBolt();


await app.RunAsync();