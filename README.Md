# Amazing Store

To execute the project, the following step is necessary: 

## Installation

- Make sure the docker is running and ready on your machine 

- Run the shell script with the name .RUN (located at the root of the repository), it will create the database and web API containers. 

- The api documentation will open in your default browser 

- The tests will be run automatically by the same script that will create the containers, but it is also possible to run them with the command:

```bash
dotnet test AmazingStore.Tests
``` 

## What was implemented? 

- Clean architecture 
- .NET 5
- Authentication with Auth0
- Entity Framework Core 5
- Tests
- Docker

