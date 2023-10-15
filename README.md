# BOVinc

##Farms API
A simple RESTful API with a single entity and CRUD endpoints.

Implementations details:
- Data is stored in an Azure SQL Database
- Uses code-first approach to database design
- Uses distributed caching with Redis cache in production
- Deployed to Azure Container App
- Runs as a multi-container in development environment (with app and seq images)
- Github actions are triggered upon repository push:
  - Builds Docker image
  - Pushes image to Docker Hub
  - Deploys a new revision in Azure Container App
- Structured logging using Serilog
- Logs are saved to Azure Application Insights (production) or Seq (development)
- API is documented using Swagger
- Endpoints return responses compliant with RFC 7231 standards
- Secret data is securely stored in Azure Key Vault and sensitive data is masked

Environment:
- .NET 7
- Visual Studio 2022 Community Edition
- Docker Desktop 4
- Entity Framework 7
- Serilog 7
- AutoMapper 12

To do:
- Implement authentication and authorization
- Change the Serilog sinks configurations (from appsettings to services)?
- Use in memory SQL database for development
