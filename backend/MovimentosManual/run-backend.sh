#!/bin/bash

ENVIRONMENT=${1:-dev}
ENV_FILE=".env.$ENVIRONMENT"

log() {
  COLOR=$1
  shift
  echo -e "\033[${COLOR}m$@\033[0m"
}

convert_to_unix() {
  FILE=$1
  if [ -f "$FILE" ]; then
    log "36" "🔧 Convertendo '$FILE' para formato Unix (LF)..."
    sed -i 's/\r$//' "$FILE"
    log "32" "✅ Conversão concluída."
  else
    log "31" "❌ Arquivo '$FILE' não encontrado."
    exit 1
  fi
}

load_dotenv() {
  if [ ! -f "$ENV_FILE" ]; then
    log "31" "❌ Arquivo $ENV_FILE não encontrado."
    exit 1
  fi
  log "36" "📦 Carregando variáveis do $ENV_FILE..."
  export $(grep -v '^#' $ENV_FILE | xargs)
}

log "33" "\n🚀 Iniciando deploy do backend .NET + SQL Server com Docker"

load_dotenv

convert_to_unix "./entrypoint.sh"
convert_to_unix "./script/init.sql"

log "90" "\n🧼 Parando containers antigos e limpando volumes..."
docker-compose down -v

log "35" "\n🔨 Build e inicialização do ambiente Docker..."
docker-compose up --build