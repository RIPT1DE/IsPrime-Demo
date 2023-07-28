using server.Services;
using server.Util;
using Microsoft.AspNetCore.Server.Kestrel.Core;

LogRequest.DisplayData();

var builder = WebApplication.CreateBuilder(args);


// Add grpc service to the container.
builder.Services.AddGrpc();

builder.WebHost.ConfigureKestrel(options =>
{
  // Setup a HTTP/2 endpoint without TLS.
  options.ListenLocalhost(5000, o => o.Protocols = HttpProtocols.Http2);

});

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapGrpcService<PrimeService>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");


app.Run();
