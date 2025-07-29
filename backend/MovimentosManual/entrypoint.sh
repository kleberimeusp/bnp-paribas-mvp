#!/bin/bash

# Remove possíveis caracteres \r caso o arquivo tenha sido criado no Windows
sed -i 's/\r$//' /scripts/init.sql 2>/dev/null || true

/opt/mssql/bin/sqlservr &

echo "Aguardando SQL Server iniciar..."
until /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P "$SA_PASSWORD" -Q "SELECT 1" > /dev/null 2>&1
do
    sleep 5
done

echo "Executando script de inicialização..."
/opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P "$SA_PASSWORD" -i /scripts/init.sql

wait
