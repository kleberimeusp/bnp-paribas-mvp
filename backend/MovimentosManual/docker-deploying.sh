#!/bin/bash

# Nome do compose file
COMPOSE_FILE="docker-compose.yml"

# Parar e remover containers, redes e volumes existentes
echo "[INFO] Removendo containers, imagens e volumes antigos..."
docker-compose -f $COMPOSE_FILE down --volumes --remove-orphans

# Remover volumes pendentes (opcional, cuidado)
docker volume prune -f

echo "[INFO] Construindo e subindo os containers..."
docker-compose -f $COMPOSE_FILE up --build -d

# Aguardar até o SQL Server estar saudável
echo "[INFO] Aguardando SQL Server ficar pronto..."
until [ "$(docker inspect -f '{{.State.Health.Status}}' sqlserver)" == "healthy" ]; do
  echo "[INFO] SQL Server ainda inicializando..."
  sleep 5
  if [ "$(docker inspect -f '{{.State.Status}}' sqlserver)" != "running" ]; then
    echo "[ERRO] Container SQL Server parou inesperadamente."
    exit 1
  fi
done

echo "[INFO] SQL Server está pronto."
echo "[INFO] Backend acessível em: http://localhost:5000"
echo "[INFO] Swagger: http://localhost:5000/swagger"
