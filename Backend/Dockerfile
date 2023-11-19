# Build Stage
FROM mcr.microsoft.com/dotnet/sdk:6.0 as builder
WORKDIR /app

# Copy only the necessary files for the build context
COPY ./ExpensesManager.WebAPI/*.csproj ./ExpensesManager.WebAPI/
COPY ./ExpensesManager.DB/*.csproj ./ExpensesManager.DB/
COPY ./ExpensesManger.Services/*.csproj ./ExpensesManger.Services/
COPY ./ExpenseManger.Automation/*.csproj ./ExpenseManger.Automation/

# Restore dependencies
RUN dotnet restore ExpensesManager.WebAPI/ExpensesManager.WebAPI.csproj

COPY . ./

# Build projects
RUN dotnet publish ExpensesManager.WebAPI/ExpensesManager.WebAPI.csproj -c Release -o /app/out

# Runtime Stage
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS runtime
WORKDIR /app

# Set the working directory for the published output
WORKDIR /app/out

# Copy the published output from the build stage
COPY --from=builder /app/out .

# Create a directory for the database
RUN mkdir /app/data

# Copy SQLite database into the container
COPY ./ExpensesManager.DB/ExpensesManagerApp.db /app/data/

# Expose ports
EXPOSE 80
EXPOSE 443

# Command to run when the container starts
CMD ["dotnet", "ExpensesManager.WebAPI.dll"]
