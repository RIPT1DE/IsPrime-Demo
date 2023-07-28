
.PHONY: server client all clean run-client run-server

all: server client 


run-server: server-bin
	cd ./server && dotnet run --configuration Release server/bin/Release/server.dll

run-client: client-bin
	cd ./client && dotnet run --configuration Release client/bin/Release/client.dll


server-bin: server/bin/Release/server.dll
client-bin: client/bin/Release/client.dll

server/bin/Release/server.dll: server
	cd ./server && dotnet build --configuration Release 
	
client/bin/Release/client.dll: client
	cd ./client && dotnet build --configuration Release 
	

primes.txt: genPrimes.py
	python -m genPrimes

clean:
	rm -rf bin
