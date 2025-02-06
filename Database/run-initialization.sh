#!/bin/bash
sleep 5s

# Function to check if SQL Server is ready
is_sql_server_ready() {
    /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P password123! -Q "SELECT 1" &> /dev/null
    return $?
}

# Wait to be sure that SQL Server came up
echo "Waiting for SQL Server to be ready..."
until is_sql_server_ready; do
	echo -n "."
    sleep 10
done
echo "SQL Server is Ready"

echo "Running create-database.sql"
/opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P password123! -d master -i create-database.sql
echo "Running create-schema.sql"
/opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P password123! -d SolutionTemplateDb -i create-schema.sql
echo "Running init-database.sql"
/opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P password123! -d SolutionTemplateDb -i init-database.sql
echo "Database setup completed"