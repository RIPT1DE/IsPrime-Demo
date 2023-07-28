# Prime Numbers: Server/Client
A running example of client/server gRPC system for simulating high requests/second

## Running the example
### Requirements
1. .Net Core 7
2. Python
3. Windows (Not tested on linux, would require some changes to Makefile)

### Instructions
1. `make run-server` to build and run gRPC Server
2. `make run-client` to build and run gRPC Client

### Description
The client will continuously send about 10K gRpc requests per second to the server, and check the validity of response aswell as log any missed requests.

The server will check the primality of requested numbers, and log the top 10 numbers received as well as total number of requests with prime numbers

## Implementation
Throughout the implementation, the scalability and performance of the code was taken as top priority, to allow for large Req/s. This Req/s was achievable on my local machine running Release variants of both server and client. If, however, logging every request is enabled on the client, it slows down considerably.

The implementation also strives to keep the code as simple as possible. This makes it easier to track bugs and maintain in the future.

1. Created a prime number generator script in python first, to rule out prime checks as a bottleneck. Now both client and server can check primes in O(1) time. Should the range of prime numbers change, we only rerun the script
2. Created server and client with basic running example to check the base case (request/response)
3. Scaled the client upto 10K requests per second using .Net Tasks (which are essentially lightweight threads). This allows us to create many parallel requests without allocating a separate thread for each request
4. On the server, started appending all valid requests to an array (in a background Task). This ensures that any locking occurs outside of the request handler loop
5. On the server, created a background task with infinite loop which displays data on 10 second intervals
6. On the client, we need to log every sent request along with its rtt. Logging to console (stdout) is a serious bottleneck, so this slows the client down. If we disable the logging, the client and server are both easily able to send/serve 10K rps

 