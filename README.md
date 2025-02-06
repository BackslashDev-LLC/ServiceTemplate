# BackslashDev Service Template

![NuGet Version](https://img.shields.io/nuget/v/BackslashDev.Service.Template)

This repository contains a starter template for building a .NET 8 Service utilizing preferred patterns and practices.

This template follows a rough implementation of "Clean Architecture" with a Domain library at the center, wrapped by an Application. The Application is then implemented in the Infrastructure project. The application and infrastructure are then utilized by the presentation layers, which in this case are the API and BackgroundProcessor.

## Components

### SQL Server 2017

This solution contains a dockerized SQL Server 2017 instance, with example scripts that can be used to initialize the database into a desired state

### RabbitMQ

The docker-compose file specifies a RabbitMQ instance used for messaging

### SolutionTemplate.Api

This API is used as the external gateway for users or other applications to communicate with the service. The API makes use of Swagger to produce OpenAPI documentation and specifications for consumption by API consumers.

The API has a very simple JWT authentication mechanism example, but is not production-ready in its current state.

### SolutionTemplate.BackgroundProcessor

The background processor is configured as a consumer of messages enqueued in RabbitMQ. This hosted service could also be used to host other background services as needed.

## Utilization

Install the template in your local machine from our nuget repository using the following command:

```sh
dotnet new install BackslashDev.Service.Template
```

To create your new project, execute the following:

```sh
mkdir NewFolder
cd NewFolder
dotnet new backslashdevservice --name CoolNewProject
```

Ensure the `docker-compose` project is set as the startup project, and then start debugging.
