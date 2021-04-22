# UsersApi
This repository is a sample project for fetching user details from https://jsonplaceholder.typicode.com
It is built in .NET Core 3.1. I have intergated following:
- Mediatr
- Lamar as DI
- Serilog with custome SourceContext
- Polly for HTTP retries
- Swagger

The project exposes following endpoints:
/api/User/{userId} - Search user by ID
/api/User/search - Search by Ids
/api/User/searchbygeo - Search by lat long
/summary - Summary of users
