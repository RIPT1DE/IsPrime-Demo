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
Random rnd = new Random((DateTime.UtcNow - DateTime.UnixEpoch).Milliseconds);

while (true)
{
  // Unix timestamp in milliseconds
  var currentEpoch = (DateTime.UtcNow - DateTime.UnixEpoch).Milliseconds;

  //If server fails to respond to any request within a second,
  //We timeout and handle later
  var deadline = DateTime.UtcNow.AddSeconds(1);

  var successfulRequests = 0;
  var timedOutRequests = 0;
  var errorRequests = 0;

  for (var i = 0; i < REQUESTS_PER_SECOND; i++)
  {
    requests[i] = Task.Run(async () =>
    {
      try
      {
        var requestNumber = rnd.Next(0, 1000);
        var requestData = new PrimeNumber { Id = ++currentRequestId, Timestamp = currentEpoch, Number = requestNumber };
        var reply = await client.IsPrimeAsync(
                    requestData,
                    deadline: deadline);

        successfulRequests++;

        var rtt = (DateTime.UtcNow - DateTimeOffset.FromUnixTimeMilliseconds(requestData.Timestamp).DateTime).Milliseconds;

        Util.Print($"Request Id {requestData.Id}, RTT: {rtt}, isPrime: {reply.IsPrime}");
      }
      catch (RpcException ex) when (ex.StatusCode == StatusCode.DeadlineExceeded)
      {
        Util.Print("Request timed out!", ConsoleColor.Red);
        timedOutRequests++;
      }
      catch (Exception ex)
      {
        Util.Print("Request error" + ex.Message, ConsoleColor.Red);
        errorRequests++;
      }
    });
  }
  await Task.WhenAll(requests);

  Util.Print("Successful Requests: " + successfulRequests);
  Util.Print("Timed out Requests: " + timedOutRequests);
  Util.Print("Error Requests: " + errorRequests);

  // There is still time until the next interval, so we wait
  if (DateTime.UtcNow < deadline)
  {
    await Task.Delay(deadline - DateTime.UtcNow);
  }

}