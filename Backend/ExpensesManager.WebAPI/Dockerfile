# Build Stage
FROM mcr.microsoft.com/dotnet/sdk:6.0 as builder
WORKDIR /app

# Create a backend folder and set it as the working directory
WORKDIR /app/backend

# Copy only the necessary files for the build context
COPY Backend/ExpensesManager.WebAPI/*.sln ./ExpensesManager.WebAPI/
COPY Backend/ExpensesManager.WebAPI/*.csproj ./ExpensesManager.WebAPI/
COPY Backend/ExpensesManager.DB/*.csproj ./ExpensesManager.DB/
COPY Backend/ExpensesManager.Services/*.csproj ./ExpensesManager.Services/

# Restore dependencies
RUN dotnet restore ExpensesManager.WebAPI/ExpensesManager.WebAPI.csproj

COPY Backend/ExpensesManager.WebAPI ./ExpensesManager.WebAPI
COPY Backend/ExpensesManager.DB ./ExpensesManager.DB
COPY Backend/ExpensesManager.Services ./ExpensesManager.Services

# Build projects
RUN dotnet publish ExpensesManager.WebAPI/ExpensesManager.WebAPI.csproj -c Release -o /app/out

# Runtime Stage
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS runtime
WORKDIR /app
# Set the working directory for the published output
WORKDIR /app/out
# Copy the published output from the build stage
COPY --from=builder /app/out .

# Expose ports
EXPOSE 80
EXPOSE 443

# Command to run when the container starts
CMD ["dotnet", "ExpensesManager.WebAPI.dll"]