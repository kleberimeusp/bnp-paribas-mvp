#!/bin/bash

/opt/mssql/bin/sqlservr &

echo "Aguardando SQL Server iniciar..."
until /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P SuaSenhaForteAqui123! -Q "SELECT 1" > /dev/null 2>&1
do
    sleep 5
done

echo "Executando script de inicialização..."
/opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P SuaSenhaForteAqui123! -i /scripts/init.sql

wait
