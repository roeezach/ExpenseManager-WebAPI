# Build Stage
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS builder
WORKDIR /app

# Copy necessary files for the build context
COPY Backend/ExpensesManager.Automation/*.csproj ./ExpensesManager.Automation/
COPY Backend/ExpensesManager.Services/*.csproj ./ExpensesManager.Services/
COPY Backend/ExpensesManager.DB/*.csproj ./ExpensesManager.DB/

# Restore dependencies
RUN dotnet restore ExpensesManager.Automation/ExpensesManager.Automation.csproj

# Copy the rest of the files
COPY Backend/ExpensesManager.Automation ./ExpensesManager.Automation
COPY Backend/ExpensesManager.Services ./ExpensesManager.Services
COPY Backend/ExpensesManager.DB ./ExpensesManager.DB

# Build projects
RUN dotnet publish ExpensesManager.Automation/ExpensesManager.Automation.csproj -c Release -o /app/out

# Test Stage
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS test
WORKDIR /app
COPY --from=builder /app/out ./
COPY Backend/ExpensesManager.Automation ./ExpensesManager.Automation
COPY Backend/ExpensesManager.Services ./ExpensesManager.Services
COPY Backend/ExpensesManager.DB ./ExpensesManager.DB

ENV DB_PATH=/app/ExpensesManager.DB/test_database.db

RUN ls -R /app  # Print the directory structure for debugging

RUN dotnet test ExpensesManager.Automation/ExpensesManager.Automation.csproj
