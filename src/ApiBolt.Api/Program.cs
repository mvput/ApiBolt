// Copyright (c) GRCcontrol B.V. All rights reserved.

using ApiBolt;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddApiBolt(typeof(Program).Assembly);

var app = builder.Build();
app.MapApiBolt();

await app.RunAsync();