# Use the official .NET 6 SDK image as a build environment
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build

# Set the working directory
WORKDIR /app

# Copy the csproj files and restore dependencies
COPY ExpensesManager.Automation/*.csproj ./ExpensesManager.Automation/
COPY ExpensesManager.DB/*.csproj ./ExpensesManager.DB/
COPY ExpensesManager.Services/*.csproj ./ExpensesManager.Services/
COPY ExpensesManager.WebAPI/*.csproj ./ExpensesManager.WebAPI/

RUN dotnet restore ./ExpensesManager.Automation/ExpensesManager.Automation.csproj
RUN dotnet restore ./ExpensesManager.DB/ExpensesManager.DB.csproj
RUN dotnet restore ./ExpensesManager.Services/ExpensesManager.Services.csproj
RUN dotnet restore ./ExpensesManager.WebAPI/ExpensesManager.WebAPI.csproj

# Copy the rest of the files and build the project
COPY ExpensesManager.Automation/. ./ExpensesManager.Automation/
COPY ExpensesManager.DB/. ./ExpensesManager.DB/
COPY ExpensesManager.Services/. ./ExpensesManager.Services/
COPY ExpensesManager.WebAPI/. ./ExpensesManager.WebAPI/

RUN dotnet publish ./ExpensesManager.WebAPI/ExpensesManager.WebAPI.csproj -c Release -o /app/publish

# Use the official ASP.NET Core runtime image as a runtime environment
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS runtime

# Set the working directory
WORKDIR /app

# Copy the build output to the runtime environment
COPY --from=build /app/publish .

# Expose the port the app runs on
# EXPOSE 51634

# Set the entry point for the application
ENTRYPOINT ["dotnet", "ExpensesManager.WebAPI.dll"]
