# MeCorp.Y
This solution contains a .NET Core Web API project designed to handle authentication, user management and reporting while following Clean Architecture.

It also contains a simple a Web Frontend to consume this api.

## MeCorp.Y Solution Structure:

- MeCorp.Y.Domain: 
        
    Core business logic and domain entities. 

    Other projects can depend on this project, but it should never depend on the other projects.

- MeCorp.Y.Application: 
    
    Use cases, DTOs, services, and service extensions.
    
    It uses Infrastructure and Domain layers to fullfill the use cases.

- MeCorp.Y.Infrastructure.Data:

    Persistence Entities, repositories, service extensions and other data related concerns.

- MeCorp.Y.Infrastructure.Security:

    Security related services, service extensions.

- MeCorp.Y.Shared: 
    
    Cross-cutting utilities, classes.

- MeCorp.Y.Api: 

    Entry point for the API.

- MeCorp.Y.Api.Tests: 
    
    Unit tests for Web Api.

- MeCorp.Y.Web: 
    
    A simple .net web application that uses MeCorp.Y.Api.


## Getting Started

### Prerequisites
- .NET SDK 9.0
- PostgreSQL

### Setup Instructions
1. Run Terminal

2. Clone the repository.
   ```bash
   git clone git@github.com:mehmetxyx/me-corp-y.git

3. Go into the solution folder
    ```bash
    cd me-corp-y

4. Restore the solution
    ```bash
    dotnet restore

5. Build the solution
    ```bash
    dotnet build

6. Set database connection

    Configure the database connection string in MeCorp.Y\MeCorp.Y.Api\appsettings.json
    ```json
    {
      "Logging": {
        "LogLevel": {
          "Default": "Information",
          "Microsoft.AspNetCore": "Warning"
        }
      },
      "AuthenticationSettings": {
        "SecretKey": "MySuperStrongKeyForAuthorization12345678901234567890qwertyuiopasdfasdfsadfdf",
        "HashKey": "MySuperStrongKeyForHashGenerating!asdskuwioouwqroweriweriweieieiieieieieieie",
        "Audience": "MeCorp.Y.Api",
        "ValidateAudience": "true",
        "Issuer": "MeCorp.Y",
        "ValidateIssuer": "true",
        "ValidateIssuerSigningKey": "true",
        "ExpirationTimeSpan": "3.00:00:00"
      },
      "ConnectionStrings": {
        "PostgresqlDb": "Host=localhost;Port=5432;Database=MeCorpY;Username=postgres;Password=1234"
      },
      "AllowedHosts": "*"
    }
    ```

    **Note:** If you don't have ready postgresql server, you can create it docker like this:

        ```Bash
            docker run --name my_postgresql -e POSTGRES_PASSWORD=1234 -d -p 5432:5432 --memory="756m" postgres

7. Go to WebApi
    ```bash
    cd .\MeCorp.Y.Api\

8. Run api
    ```bash
    dotnet run
    ```

    **Note:** The first run will take a few minutes.
    **Note:** It will run the migrations when the api runs.
    This will make sure a simple run.

9. Wait untill you see following messages on the console
    ```bash
    info: Microsoft.Hosting.Lifetime[14]
          Now listening on: http://localhost:5280
    info: Microsoft.Hosting.Lifetime[0]
          Application started. Press Ctrl+C to shut down.
    info: Microsoft.Hosting.Lifetime[0]
          Hosting environment: Development
    info: Microsoft.Hosting.Lifetime[0]
          Content root path: C:\Users\mryil\source\repos\test-github\me-corp-y\MeCorp.Y.Api
    ```

    **Remark** it shows that our api runs on `http://localhost:5280`

10. Open another terminal

11. Go to MeCorp.Y.Web frontend project
    ```bash
     cd .\MeCorp.Y.Web\

12 Run Web project

    ```bash
    dotnet run

13 Wait until you see the following messages:

    ```bash
    info: Microsoft.Hosting.Lifetime[14]
          Now listening on: http://localhost:5218
    info: Microsoft.Hosting.Lifetime[0]
          Application started. Press Ctrl+C to shut down.
    info: Microsoft.Hosting.Lifetime[0]
          Hosting environment: Development
    info: Microsoft.Hosting.Lifetime[0]
          Content root path: C:\Users\mryil\source\repos\test-github\me-corp-y\MeCorp.Y.Web
 
 
   **Remark:** The web project will run on `http://localhost:5218`.


## Login into Web

- We can login into web project on 
  ```url
  http://localhost:5218/index.html

- Database includes two `Admin` users `mehmet` and `mecorp` with passwords `1234`.

- After 10 subsequent failed logins, IP will be blocked for one minute.

## Registering users
- We can register new users on 
  ```url
  http://localhost:5218/register.html

- We can register new users with `Manager` role by a referral code.
  ```url
  http://localhost:5218/register.html?referralCode=CreateAsManager
  ```

  **Note** these codes are in the database.

## Dashboard

  - We can see the dashboard after loging in.
      ```url
      http://localhost:5218/dashboard.html?userId=1

## Using Api
   
   - We can use api with scalar ui (a swagger ui alternative)

   - After the api run, it will be available on
       ```url
        http://localhost:5280/scalar/v1

## Rate limiting

   - Too many requests will be prevented by RateLimiter middleware configured in Security layer.


