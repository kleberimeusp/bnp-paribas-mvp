#!/bin/bash

echo "🧼 Limpando containers e volumes..."
docker-compose down -v

echo "🚀 Subindo ambiente com build..."
docker-compose up --build
