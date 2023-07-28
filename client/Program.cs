using System.Threading.Tasks;
using Grpc.Net.Client;
using Grpc.Core;
using client;

const int REQUESTS_PER_SECOND = 10_000;

// The port number must match the port of the gRPC server.
using var channel = GrpcChannel.ForAddress("http://localhost:5000");
var client = new Primes.PrimesClient(channel);

//Pre-allocate the array of tasks so we don't waste time on allocations
var requests = new Task[REQUESTS_PER_SECOND];

//Request id
var currentRequestId = 0;

//Random number generator
Random rnd = new Random();

while (true)
{
  // Unix timestamp in milliseconds
  var currentEpoch = (DateTime.UtcNow - DateTime.UnixEpoch).Milliseconds;

  //If server fails to respond to any request within a second,
  //We timeout and handle later
  var deadline = DateTime.UtcNow.AddSeconds(1);

  var successfulRequests = 0;
  var timedOutRequests = 0;

  for (var i = 0; i < REQUESTS_PER_SECOND; i++)
  {
    requests[i] = Task.Run(async () =>
    {
      try
      {
        var requestNumber = rnd.Next(0, 1000);
        var reply = await client.IsPrimeAsync(
                    new PrimeNumber { Id = currentRequestId++, Timestamp = currentEpoch, Number = requestNumber },
                    deadline: deadline);

        successfulRequests++;
      }
      catch (RpcException ex) when (ex.StatusCode == StatusCode.DeadlineExceeded)
      {
        Console.WriteLine("Request timed out!");
        timedOutRequests++;
      }
    });
  }
  await Task.WhenAll(requests);

  Console.WriteLine("Successful Requests: " + successfulRequests);
  Console.WriteLine("Failed Requests: " + timedOutRequests);

}

