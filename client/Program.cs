// See https://aka.ms/new-console-template for more information

using System.Threading.Tasks;
using Grpc.Net.Client;
using client;

// The port number must match the port of the gRPC server.
using var channel = GrpcChannel.ForAddress("https://localhost:7201");
var client = new Primes.PrimesClient(channel);
var reply = await client.IsPrimeAsync(
                  new PrimeNumber { Id = 1, Timestamp = 2, Number = 4 });
Console.WriteLine("Greeting: " + reply.IsPrime);
Console.WriteLine("Press any key to exit...");
Console.ReadKey();

