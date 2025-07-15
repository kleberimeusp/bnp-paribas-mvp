#!/bin/bash

# Aviso
echo "[INFO] Limpando todos os containers, imagens, volumes e redes Docker..."

# Parar todos os containers
containers=$(docker ps -aq)
if [ -n "$containers" ]; then
    echo "[INFO] Parando todos os containers..."
    docker stop $containers
fi

# Remover todos os containers
if [ -n "$containers" ]; then
    echo "[INFO] Removendo todos os containers..."
    docker rm $containers
fi

# Remover todas as imagens
images=$(docker images -q)
if [ -n "$images" ]; then
    echo "[INFO] Removendo todas as imagens..."
    docker rmi -f $images
fi

# Remover todos os volumes
volumes=$(docker volume ls -q)
if [ -n "$volumes" ]; then
    echo "[INFO] Removendo todos os volumes..."
    docker volume rm $volumes
fi

# Remover todas as redes que não são default
networks=$(docker network ls | grep -vE 'bridge|host|none' | awk '{print $1}' | tail -n +2)
if [ -n "$networks" ]; then
    echo "[INFO] Removendo todas as redes customizadas..."
    docker network rm $networks
fi

echo "[INFO] Docker limpo com sucesso."
