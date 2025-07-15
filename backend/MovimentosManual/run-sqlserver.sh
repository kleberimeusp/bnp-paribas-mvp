#!/bin/bash

# Torna os scripts executáveis, se necessário
chmod +x entrypoint.sh

# Para container já existente, remova-o primeiro
if [ $(docker ps -a -q -f name=sql2022) ]; then
  echo "Removendo container existente: sql2022"
  docker stop sql2022 && docker rm sql2022
fi

# Executa o container com SQL Server 2022 + script de inicialização

docker run -e "ACCEPT_EULA=Y" \
  -e "SA_PASSWORD=SuaSenhaForteAqui123!" \
  -p 1433:1433 \
  --name sql2022 \
  -v $(pwd)/entrypoint.sh:/scripts/entrypoint.sh \
  -v $(pwd)/init.sql:/scripts/init.sql \
  -d mcr.microsoft.com/mssql/server:2022-latest \
  /bin/bash /scripts/entrypoint.sh
