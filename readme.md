# 12 Factor Roombooking Service
This service was implemented as part of a university homework for the software development course. It is basically a service with a HTTP API that allows the user to perform basic CRUD actions for person, building, room, and booking entities. The service was implemented with ASP.NET, split into 3 layers (service layer, business logic layer, data access layer) and containerized with Docker afterwards.

The main objective was to fulfill at least 8 of the 12 factors from http://12factor.net . As a result, the application is not really complex. The service layer receives the HTTP requests and passes the data to the Business Logic Layer. The Business Logic Layer just validates the data and passes it to the Data Access Layer, where the data handled by the Entity Framework and persisted in a SQL Server database.

## How to start the service / application

The codebase of the project is on [Github](https://github.com/kretmatt/12Factor_RoomBooking). The containerized service can be found on [Docker Hub](https://hub.docker.com/r/misterthias/tfroombooking).

In order to start the service including the database and an Nginx Loadbalancer, the following command must be executed in the root of this repository.

````
docker-compose up --scale roombooking=[Wanted number of RoomBooking APIs]
````

The command will start the containers as described in the docker-compose.yml file. Once all containers are running, the service is available under http://localhost:8080. To view the swagger page, simply visit http://localhost:8080/swagger/index.html. 

The docker-compose command for starting the service(s), load balancer and database was tested on both Windows and Linux (Ubuntu). In both cases, the command was successfully executed.

## Factors
The fulfilled factors including explanations can be found in the factors.md file.
