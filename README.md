# APTastify Web API

A .NET 8 Web API project with AWS integration, PostgreSQL database, and email services.

## 🐳 Docker Build & Deployment

### Platform Considerations

**Important**: This project must be built for Linux/AMD64 architecture when deploying to AWS ECS, even when developing on macOS (Apple Silicon).

### Prerequisites

- Docker Desktop
- AWS CLI configured with SSO
- AWS ECR repository access

### Build Commands

#### For Local Development (macOS)
```bash
# Build for local testing (ARM64 on Apple Silicon)
docker build -f AP.WebAPI/Dockerfile -t aptastify-be .
```

#### For Production Deployment (AWS ECS)
```bash
# Build for Linux/AMD64 platform (required for ECS)
docker build --platform linux/amd64 -f AP.WebAPI/Dockerfile -t aptastify-be .
```

### AWS ECR Deployment

#### 1. Authenticate with AWS
```bash
# Login to AWS SSO
aws sso login --profile AdministratorAccess-325066841340

# Authenticate Docker with ECR
aws ecr get-login-password --region us-east-2 --profile AdministratorAccess-325066841340 | docker login --username AWS --password-stdin 325066841340.dkr.ecr.us-east-2.amazonaws.com
```

#### 2. Build and Push Image
```bash
# Build for ECS (Linux/AMD64)
docker build --platform linux/amd64 -f AP.WebAPI/Dockerfile -t aptastify-be .

# Tag for ECR
docker tag aptastify-be:latest 325066841340.dkr.ecr.us-east-2.amazonaws.com/aptastify-be:latest

# Push to ECR
docker push 325066841340.dkr.ecr.us-east-2.amazonaws.com/aptastify-be:latest
```

### Complete Deployment Script

```bash
#!/bin/bash

# 1. AWS Authentication
echo "Authenticating with AWS..."
aws sso login --profile AdministratorAccess-325066841340
aws ecr get-login-password --region us-east-2 --profile AdministratorAccess-325066841340 | docker login --username AWS --password-stdin 325066841340.dkr.ecr.us-east-2.amazonaws.com

# 2. Build for ECS
echo "Building Docker image for Linux/AMD64..."
docker build --platform linux/amd64 -f AP.WebAPI/Dockerfile -t aptastify-be .

# 3. Tag and Push
echo "Tagging and pushing to ECR..."
docker tag aptastify-be:latest 325066841340.dkr.ecr.us-east-2.amazonaws.com/aptastify-be:latest
docker push 325066841340.dkr.ecr.us-east-2.amazonaws.com/aptastify-be:latest

echo "Deployment complete!"
```

## 🏗️ Project Structure

```
APTastifyWebAPI/
├── AP.BusinessInterfaces/     # Data models and interfaces
├── AP.BusinessLogic/          # Business logic and services
├── AP.WebAPI/                 # Web API project
│   ├── Controllers/           # API controllers
│   ├── DependencyInjection/   # DI configuration
│   ├── Managers/              # Business logic managers
│   ├── Middleware/            # Custom middleware
│   └── Dockerfile            # Docker configuration
└── AP.AWS/                    # AWS integration services
```

## 🔧 Configuration

### Environment Variables

The application supports multiple configuration sources:

1. **Local Development**: `appsettings.json` and `appsettings.Development.json`
2. **Production**: AWS Secrets Manager
3. **Environment Variables**: For containerized deployments

### AWS Secrets Manager

The application automatically retrieves configuration from AWS Secrets Manager:

- **Connection Strings**: `connection-strings`
- **Email Settings**: `email-settings`
- **AWS Credentials**: Configured via appsettings or environment variables

### Database

- **Type**: PostgreSQL
- **Connection**: Configured via AWS Secrets Manager or local appsettings
- **Migrations**: Entity Framework migrations included

### Email Service

- **Provider**: Configurable (Gmail SMTP, AWS SES, etc.)
- **Templates**: HTML email templates with variable substitution
- **Settings**: Retrieved from AWS Secrets Manager

## 🚀 Local Development

### Prerequisites

- .NET 8 SDK
- PostgreSQL (local or remote)
- Docker Desktop

### Running Locally

```bash
# Navigate to Web API project
cd AP.WebAPI

# Restore dependencies
dotnet restore

# Run the application
dotnet run
```

### Database Migrations

```bash
# Create initial migration
dotnet ef migrations add initial

# Apply migrations to database
dotnet ef database update
```

## 🔍 Troubleshooting

### Common Issues

#### 1. Platform Architecture Error
```
Error: "image Manifest does not contain descriptor matching platform 'linux/amd64'"
```

**Solution**: Always use `--platform linux/amd64` when building for ECS deployment.

#### 2. ECR Authentication Error
```
Error: "403 Forbidden" when pushing to ECR
```

**Solution**: Ensure AWS SSO is logged in and ECR authentication is current.

#### 3. Database Connection Error
```
Error: "Unable to connect to database"
```

**Solution**: Verify connection string in AWS Secrets Manager or local appsettings.

### Docker Commands Reference

| Command | Purpose | Platform |
|---------|---------|----------|
| `docker build -f AP.WebAPI/Dockerfile -t aptastify-be .` | Local build | Native |
| `docker build --platform linux/amd64 -f AP.WebAPI/Dockerfile -t aptastify-be .` | ECS build | Linux/AMD64 |
| `docker tag aptastify-be:latest 325066841340.dkr.ecr.us-east-2.amazonaws.com/aptastify-be:latest` | Tag for ECR | Any |
| `docker push 325066841340.dkr.ecr.us-east-2.amazonaws.com/aptastify-be:latest` | Push to ECR | Any |

## 📝 Notes

- **Platform Specificity**: Always build with `--platform linux/amd64` for ECS deployment
- **AWS Integration**: The application is designed to work with AWS services (ECS, RDS, Secrets Manager)
- **Configuration**: Supports both local development and production AWS configurations
- **Email Service**: Includes templated email functionality with AWS Secrets Manager integration

## 🤝 Contributing

1. Ensure Docker builds work for both local development and ECS deployment
2. Test configuration changes with both local and AWS settings
3. Update this README for any new deployment requirements 