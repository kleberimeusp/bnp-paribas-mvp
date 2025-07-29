# run-backend.ps1
param(
    [string]$Environment = "dev"
)

Clear-Host

function Write-Log {
    param (
        [string]$Message,
        [string]$Color = "White"
    )
    Write-Host $Message -ForegroundColor $Color
}

function Convert-ToUnixLineEndings {
    param ([string]$FilePath)
    if (Test-Path $FilePath) {
        Write-Log "🔧 Convertendo '$FilePath' para formato Unix (LF)..." "Cyan"
        (Get-Content $FilePath) | Set-Content -Encoding Ascii $FilePath
        Write-Log "✅ Conversão concluída com sucesso." "Green"
    } else {
        Write-Log "❌ Arquivo '$FilePath' não encontrado." "Red"
        exit 1
    }
}

function Load-DotEnv {
    param ([string]$FilePath)
    if (-not (Test-Path $FilePath)) {
        Write-Log "❌ Arquivo .env '$FilePath' não encontrado." "Red"
        exit 1
    }

    Write-Log "📦 Carregando variáveis do .env: $FilePath" "DarkCyan"
    Get-Content $FilePath | ForEach-Object {
        $_ = $_.Trim()
        if (-not $_.StartsWith("#") -and $_ -match "=") {
            $parts = $_ -split '=', 2
            $key = $parts[0].Trim()
            $value = $parts[1].Trim()
            $env:$key = $value
            Write-Log "    ✓ $key=$value" "Gray"
        }
    }
}

Write-Log "`n🚀 Iniciando deploy do backend .NET + SQL Server com Docker" "Yellow"

# Carrega .env correto
$envFile = ".env.$Environment"
Load-DotEnv $envFile

# Converter os arquivos
Convert-ToUnixLineEndings "./entrypoint.sh"
Convert-ToUnixLineEndings "./script/init.sql"

# Parar containers
Write-Log "`n🧼 Parando containers antigos e limpando volumes..." "DarkGray"
docker-compose down -v

# Subir containers
Write-Log "`n🔨 Build e inicialização do ambiente Docker..." "Magenta"
docker-compose up --build
