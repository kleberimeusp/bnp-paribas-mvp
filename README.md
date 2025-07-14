# bnp-paribas-mvp
Initial structure for the BNP Paribas &amp; Antlia challenge.
# 💼 Projeto MVP - Movimentos Manuais

## 📄 Descrição

Este projeto MVP foi desenvolvido para gerenciar **Movimentos Manuais**, incluindo cadastro de **Produtos**, **Cosif** (ProdutoCosif), e os próprios movimentos, utilizando Angular (frontend) e ASP.NET Core (backend).

---

## 🚀 Tecnologias

* **Frontend**: Angular 17+
* **Backend**: ASP.NET Core 7 ou superior
* **Banco de dados**: SQL Server
* **Docker**: Containers opcionais para facilitar setup

---

## 🗂️ Estrutura principal

### 🔥 Frontend

* `/src/app/home` → Tela inicial com links para movimentos, produtos e cosifs.
* `/src/app/movimentos` → CRUD de Movimentos Manuais.
* `/src/app/produtos` → CRUD de Produtos.
* `/src/app/cosifs` → CRUD de Cosifs.

### ⚙️ Backend

* `Controllers`

  * `ProdutosController`
  * `CosifsController`
  * `MovimentosController`
* `Models`

  * `Produto.cs`
  * `ProdutoCosif.cs`
  * `MovimentoManual.cs`
* `Services`

  * ProdutoService
  * CosifService
  * MovimentoService

---

## ✅ Passos para configurar e executar

### 💻 Backend local

1️⃣ Configure a string de conexão no `appsettings.json`:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Database=SeuDb;User Id=sa;Password=SuaSenha;"
}
```

2️⃣ Execute as migrations:

```bash
dotnet ef migrations add InitialCreate
dotnet ef database update
```

3️⃣ Rode a aplicação backend localmente:

```bash
dotnet run
```

A API ficará disponível em: `http://localhost:5000/api`

### 🐳 Backend com Docker (opcional)

1️⃣ Crie um arquivo `Dockerfile`:

```dockerfile
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY . .
RUN dotnet restore
RUN dotnet publish -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "SeuProjeto.dll"]
```

2️⃣ Build e run:

```bash
docker build -t movimentos-backend .
docker run -d -p 5000:80 movimentos-backend
```

### 🌐 Frontend

1️⃣ Instale as dependências:

```bash
npm install
```

2️⃣ Rode a aplicação:

```bash
ng serve --open
```

Acesse: `http://localhost:4200`

---

## 📑 Funcionalidades

### ✏️ Movimentos Manuais

* Incluir novo movimento
* Atualizar movimento
* Excluir movimento
* Seleção dinâmica de produto e cosif
* Visualização em tabela com formatação

### 📦 Produtos

* Cadastro de novos produtos
* Edição de produtos existentes
* Exclusão de produtos

### 🔧 Cosifs

* Cadastro de novos cosifs
* Edição de cosifs
* Exclusão de cosifs

---

## 🎨 Estilo CSS (SASS/SCSS)

* Paleta principal: Verde BNP (#006341)
* Botões personalizados com hover
* Tabelas com sombra e destaque
* Botão Voltar presente em todas as telas

---

## 🛠️ Observações importantes

* Certifique-se que o backend esteja rodando antes de iniciar o frontend.
* Valide todas as rotas e endpoints (`api/produtos`, `api/cosifs`, `api/movimentos`).
* Caso receba erro 404, revise os controllers e endpoints no backend.
* Utilize `FormsModule` no Angular para evitar erros de binding com `ngModel`.

---

## 🤝 Contribuição

Contribuições são bem-vindas! Para sugerir melhorias, crie uma issue ou envie um pull request.

---

## 📬 Contato

Se precisar de suporte ou tiver dúvidas:

* 💬 Email: [gmail.com](mailto:kleber.ime.usp@gmail.com)
* 💼 LinkedIn: [https://www.linkedin.com/in/kleber-augusto/](https://www.linkedin.com/in/kleber-augusto/)

---

### 🚩 MVP finalizado com sucesso!

> *Desenvolvido com dedicação 💚 e foco em escalabilidade!*
