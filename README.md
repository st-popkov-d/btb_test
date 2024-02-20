### Prerequisites

- MS SQL Server 
- MS Visual Studio (preferrably 2022)
- .NET 8 SDK
- Postman (optional)

---

### Setting up local environment

You should be able to connect to you local MS SQL Server installation. 
You can create a new login `bloggy` and database `bloggy` or use existing entries. 

1. Restore project packages with command ``` dotnet restore```.
2. Migrate database: open terminal in `src/Bloggy.Database/` and run ```dotnet ef database update -c "BloggyDbContext" -- %sql_server_connection_string%``` or run `update-database.cmd` script located under `src/Bloggy.Database/Scripts` and follow script instructions.
3. Update database connection string in `appsettings.Development.json` file within project `Bloggy.WebApi`.

---

### Launch and test application

Select `http` or `https` launch profile for project `Bloggy.WebApi` in Visual Studio and start debugging session.

For convenience, three default users should have been created in the database after migrations:
- alex
- simon
- jordano

All of them have password `password`.

#### Swagger
When application is running, you can access generated api documentation here: `https://localhost:7151/swagger` or `http://localhost:5124/swagger`

#### Request logging
Simple console request logger will print requests and responses when api is hit: `[REQ] [GET] [02/20/2024 15:12:12 +00:00]`, `[RES] [GET] [404] [02/20/2024 15:12:04 +00:00]`

#### Postman (optional)
In the root folder of repository you can find exported Postman collection `Bloggy.postman_collection.json`. After importing it, you can start sending requests to the web api. 

-or- 

Using any tool you prefer, you can send requests to:
- GET /amortization/getSchedule
- POST /auth/login
- *POST /blog/postBlog
- GET /blog/getBlogs
- GET /blog/getBlog/{id}
- *PUT /blog/updateBlog/{id}
- *DELETE /blog/deleteBlog/{id}
- *POST /blog/postComment/{blogId}
- *PUT /blog/updateComment/{id}
- *DELETE /blog/deleteComment{id}

\* - endpoints require `Authorization` header with a `Bearer %token%` value. You can retrieve token using Login endpoint and details of users mentioned above. 

### Amortization schedule (part two)

Source code for this stored procedure can be found in `src/Bloggy.Database/SqlScripts`. It is also a part of migration `AddsAmortizationScheduleSp` and should be already present in your MS SQL Server. 

You can run it with EXEC command (i.e. if using SQL Server Management Studio or sqlcmd): `EXEC [dbo].[GetAmortizationSchedule]`.

-or-

Endpoint `/amortization/getSchedule` returns result of this stored procedure execution in `json` format.

### Rationale

#### Project Structure
Given task complexity, 1 project per database, webapi and tests should be sufficient. In some places there is code repetitiveness (commented in-place) just to simplify structure.

#### WebAPI
Most straightforward approach with ControllerBase-inherited controllers is used, actions return IActionResult type, DTOs are used for data exchange between client and server.

#### JWT Authentication
Popular `Microsoft.AspNetCore.Authentication.JwtBearer` (and it's dependencies) is used for JWT token issuance and verification. Very simple and quick approach, given no extra requirements are given.

#### Validation
Since there is no requirement on endpoint validation, only a few very basic validations were introduced, so that database constraints on field lengths are not violated.

#### Unit testing
Some basic unit tests for most obvious scenarios were introduced (both positive and negative paths). 
For authentication testing, slightly different approach was taken, since it happends in a Middleware before our controller code is reached: WebApplicationFactory helper class allows us to run whole webapi in a test context with complete control over it. 

#### Stored procedure
Implemented with CET (for readability), and PMT formula (constant payments and constant interest rate). There are three temporary sets: first 12 months of payments (PS), new payments basis, recycled payments (RPS). Initial row set for PS and RPS, following rows depend on previous. Result is a union of PS and RPS.
