# Use an appropriate base image
FROM alpine:latest

# Ensure the database directory exists
RUN mkdir -p /app/data

# Copy the database file into the container
COPY ExpensesManagerApp.db /app/data/ExpensesManagerApp.db

# Install SQLite for any required operations
RUN apk add --no-cache sqlite

# Expose any necessary ports (if needed for the database service, optional)
EXPOSE 5432

# Default command (optional, usually for running a database server)
CMD ["sqlite3", "/app/data/ExpensesManagerApp.db"]
