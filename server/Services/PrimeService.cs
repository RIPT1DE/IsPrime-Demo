using Grpc.Core;
using server;
using server.Util;

namespace server.Services;

public class PrimeService : Primes.PrimesBase
{
  private readonly ILogger<PrimeService> _logger;
  public PrimeService(ILogger<PrimeService> logger)
  {
    _logger = logger;
  }

  public override Task<PrimeNumberResponse> IsPrime(PrimeNumber request, ServerCallContext context)
  {
    var response = new PrimeNumberResponse
    {
      IsPrime = PrimeUtils.IsPrime(request.Number)
    };

    LogRequest.Log(request, response);

    return Task.FromResult(response);
  }
}
