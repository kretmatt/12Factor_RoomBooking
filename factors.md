# 12 Factors
This file describes the implemented factors from http://12factor.net. The goal was to explicitly implement at least 8 out of 12 factors. All in all, I have implement 11 factors.
## Codebase - DONE
There is only one codebase of this service and it can be found on [Github](https://github.com/kretmatt/12Factor_RoomBooking). There can be several deploys of this app, for example when it is executed locally or when the container is deployed on a server.
## Dependencies - DONE
This app was implemented in C#. The dependencies of this app are managed with NuGet, the package manager for .NET. At first, only the main packages were installed (e.g Entity Framework) and after the implementation, all implicitly referenced packages were also included as in the .csproj file as PackageReferences (the implicit dependencies were "discovered" by the Rider IDE, I simply installed them afterwards in the corresponding projects of the solution).
## Config - DONE
For this service, the connection string for the SQL Server database can be configured in the docker-compose.yml file. It is then passed into the running containers, where they are used to establish connections with the database. There are no special configurations aside from that.
## Backing services - DONE
For this service, the database is treated as an attached resource. The database is accessed with a connection string, which can be configured in the docker-compose.yml file. Therefore, it is possible to swap out the database by changing the connection string.
## Build, release, run - DONE
As soon as there is a new commit / merged pull request on the main branch, the service is built on Github and released on [Docker Hub](https://hub.docker.com/r/misterthias/tfroombooking). There is  no dedicated release stage in the pipeline on Github, but the execution of the docker compose file on a device or server could be interpreted as the run stage.
## Processes - DONE
The app is executed in the execution environment as one or more processes, depending on how many containers are created from the RoomBooking API image. Every piece of data that needs to persist is stored in the SQL Server database. Additionally, no session data is being saved. The database is not shared with other services, as there is only one kind of service (that can be duplicated).
## Port binding - DONE
The containerized service itself can be completely self-contained and could export HTTP as a service by binding to a port and listening to the requests coming in on that port. However, as a result of "implementing" a load balancer with a nginx container that passes traffic to one of the API instances, one could argue that this is not the case anymore.

Aside from that, the database is only accessible by the service itself. The load balancer passes traffic on the 8080 port (exported via port binding) to one of the API instances, which expose their functionality internally via the 80 port. The responses are then sent back through the load balancer to the user.
## Concurrency - DONE
The service can be scaled out by defining a number of needed API instances in the docker-compose up command. The traffic is handled by the nginx load balancer, which assigns the incoming traffic to one of the several instances of the API.
## Disposability - DONE
The services are running within a few seconds after issuing the command. Furthermore, controllers, repositories, business logic classes, etc. are injected for every HTTP request the service receives (with AddScoped) and "cease to exist" once the request is fulfilled. 

Most of the classes also implement the IDisposable interface, to ensure that the resources are freed correctly.
## Dev/prod parity - DONE
Due to the pipeline that publishes the containerized service as an image on Docker Hub, the time, personnel, and tool gaps are smaller than before. Development and Production could be as similar as possible (production environment does not really exist in the scope of the exercise), the time to deploy & release is reduced, and devs are more involved in the deployment / release process.

Development and production are also similar, as the docker-compose.yml file guarantees that the same backing services and versions are used.
## Logs - DONE
In every layer, the different controller, business logic classes, and repositories log the events that are occurring. The logs / event streams can be viewed and observed in the terminal.
## Admin processes - NOT IMPLEMENTED
The only factor that was not implemented was the "Admin processes" factor.
If I had tried to implement it, I would have implemented database migrations / seeders. Those migration and seeding processes for the database would be one-off processes to ensure that basic data is in the database.
