# AquarumProtector

## Descrição do Projeto

O **AquarumProtector** é o backend de uma aplicação cujo principal objetivo é indicar se uma fonte de água (poço, fonte, rio, lago ou reservatório) é potável ou está contaminada. A implementação utiliza ASP.NET Core, com:

* API RESTful baseada em Controllers
* Autenticação e autorização via JWT (ASP.NET Core Identity)
* Persistência em banco Oracle através de Entity Framework Core (Code-First + Migrations)
* Documentação automática usando OpenAPI (Swagger)

### Funcionalidades Principais

* **Gerenciamento de Alertas** (`Alert`):

  * Listar, criar, atualizar e excluir alertas do sistema.
* **Autenticação de Usuários** (`Auth`):

  * Registro, login e refresh de tokens JWT.
* **Gerenciamento de Fontes de Água** (`WaterSource`):

  * CRUD (Create, Read, Update, Delete) de fontes de água.
  * Ao atualizar uma fonte, registra automaticamente um histórico de alterações (`WaterSourceUpdate`), incluindo mudança de status (Potável → Contaminada), localização e timestamp.
* **Histórico de Atualizações** (`WaterSourceUpdate`):

  * Cada vez que uma `WaterSource` é atualizada, gera um registro em `WaterSourceUpdates` para auditoria.

---

## Rotas da API

> **Observação:** todas as rotas que modificam dados (POST, PUT, DELETE) exigem um token JWT válido no cabeçalho `Authorization: Bearer <seu_token>`.

---

### 1. Alerts

* **GET** `/api/Alerts`
  Retorna todos os alertas cadastrados.

  * **Resposta 200 OK**: `IEnumerable<Alert>`

* **GET** `/api/Alerts/{id}`
  Retorna um alerta específico pelo `id`.

  * **Resposta 200 OK**: `Alert`
  * **Resposta 404 Not Found**: Alerta não encontrado

* **POST** `/api/Alerts`
  Cria um novo alerta. Envia JSON:

  ```json
  {
    "title": "string",
    "description": "string",
    "icon": "string",
    "isActive": true
  }
  ```

  * **Resposta 201 Created**: `Alert` criado
  * **Resposta 400 Bad Request**: Dados inválidos

* **PUT** `/api/Alerts/{id}`
  Atualiza um alerta existente. Envia JSON completo do `Alert` (incluindo `id`):

  ```json
  {
    "id": 1,
    "title": "novo título",
    "description": "nova descrição",
    "icon": "novo-ícone",
    "isActive": false
  }
  ```

  * **Resposta 204 No Content**: Sucesso
  * **Resposta 400 Bad Request**: `id` inconsistente ou dados inválidos
  * **Resposta 404 Not Found**: Alerta não encontrado

* **DELETE** `/api/Alerts/{id}`
  Remove um alerta pelo `id`.

  * **Resposta 204 No Content**: Sucesso
  * **Resposta 404 Not Found**: Alerta não encontrado

---

### 2. Auth

* **POST** `/api/Auth/register`
  Registra um novo usuário. Envia JSON:

  ```json
  {
    "email": "usuario@exemplo.com",
    "password": "Senha123!",
    "name": "Nome Completo"
  }
  ```

  * **Resposta 201 Created**: Objeto com dados do usuário e tokens (access + refresh)
  * **Resposta 400 Bad Request**: Dados ausentes, e-mail já cadastrado ou senha fora dos requisitos

* **POST** `/api/Auth/login`
  Autentica um usuário existente. Envia JSON:

  ```json
  {
    "email": "usuario@exemplo.com",
    "password": "Senha123!"
  }
  ```

  * **Resposta 200 OK**: `{ "token": "JWT", "refreshToken": "string", "expiresIn": 3600 }`
  * **Resposta 400 Bad Request**: Dados ausentes ou formato inválido
  * **Resposta 401 Unauthorized**: Credenciais incorretas

* **POST** `/api/Auth/refresh`
  Renova o token de acesso usando um `refreshToken`. Envia JSON:

  ```json
  {
    "token": "JWT-antigo",
    "refreshToken": "string"
  }
  ```

  * **Resposta 200 OK**: Novo `{ "token": "JWT", "refreshToken": "novo", "expiresIn": 3600 }`
  * **Resposta 400 Bad Request**: Dados ausentes ou formato inválido
  * **Resposta 401 Unauthorized**: Refresh token inválido ou expirado

---

### 3. WaterSources

> **Observação:** todos os endpoints de fonte de água requerem JWT válido.

* **GET** `/api/WaterSources`
  Retorna todas as fontes de água.

  * **Resposta 200 OK**: `IEnumerable<WaterSource>`

* **GET** `/api/WaterSources/{id}`
  Retorna uma fonte de água específica pelo `id`.

  * **Resposta 200 OK**: `WaterSource`
  * **Resposta 404 Not Found**: Fonte não encontrada

* **POST** `/api/WaterSources`
  Cria uma nova fonte de água. Envia JSON (`WaterSourceCreateDto`):

  ```jsonc
  {
    "nome": "Poço Central",
    "descricao": "Poço principal do pátio A",
    "localizacao": "Setor A, bloco 3",
    "latitude": -23.55052,
    "longitude": -46.633308,
    "type": "Poco",
    "createdById": 42,
    "lastInspected": "2025-06-04T00:00:00Z",
    "status": "Potavel"
  }
  ```

  * **Resposta 201 Created**: `WaterSource` criado
  * **Resposta 400 Bad Request**: Dados inválidos ou campos obrigatórios faltando

* **PUT** `/api/WaterSources/{id}`
  Atualiza uma fonte de água existente e registra um novo update (neste DTO, não se envia as listas de updates):

  ```jsonc
  {
    "nome": "Poço Central Atualizado",
    "descricao": "Descrição atualizada",
    "localizacao": "Setor A, bloco 3B",
    "latitude": -23.55052,
    "longitude": -46.633308,
    "type": "Poco",
    "lastInspected": "2025-06-10T00:00:00Z",
    "status": "Contaminada",
    "isActive": true,
    "updateDescricao": "Detectado aumento de contaminação após chuva"
  }
  ```

  * **Resposta 204 No Content**: Sucesso (sem conteúdo)
  * **Resposta 400 Bad Request**: Dados inválidos
  * **Resposta 404 Not Found**: Fonte não encontrada

* **DELETE** `/api/WaterSources/{id}`
  Remove uma fonte de água pelo `id`.

  * **Resposta 204 No Content**: Sucesso
  * **Resposta 404 Not Found**: Fonte não encontrada

## Instalação

### 1. Pré-requisitos

* [.NET SDK 9](https://dotnet.microsoft.com/download)
* Banco de dados **Oracle** acessível (pode ser Oracle 19c, 21c, etc.)
* Ferramenta **dotnet-ef** instalada globalmente:

  ```bash
  dotnet tool install --global dotnet-ef
  ```

### 2. Clonar o Repositório

```bash
git clone https://github.com/AquarumProtector/backend.git
cd backend
```

### 3. Configurar Conexão com Oracle e JWT

Edite o arquivo `appsettings.json` na raiz do projeto:

```jsonc
{
  "ConnectionStrings": {
    "DefaultConnection": "User Id=USUARIO;Password=SENHA;Data Source=//HOST:PORT/SERVICE_NAME"
  },
  "Jwt": {
    "Key": "UMA_CHAVE_SECRETA_LONGA_AQUI",
    "Issuer": "AquarumProtector",
    "Audience": "AquarumProtectorClient",
    "ExpireMinutes": 60
  }
}
```

* **DefaultConnection**: ajuste `USUARIO`, `SENHA`, `HOST`, `PORT` e `SERVICE_NAME` do seu Oracle.
* **Jwt**:

  * `Key`: string longa (mínimo 32 caracteres) usada para assinar tokens.
  * `Issuer`: identificador do emissor do token.
  * `Audience`: identificador dos consumidores autorizados (nome do front-end).
  * `ExpireMinutes`: tempo de expiração em minutos do token.

### 4. Aplicar Migrations

1. No terminal, na pasta do projeto (onde está o `.csproj`), execute:

   ```bash
   dotnet ef migrations add InitialCreate --context AquaContext
   ```
2. Em seguida, aplique as migrations ao banco Oracle:

   ```bash
   dotnet ef database update --context AquaContext
   ```

> Caso necessário, crie um *Design-Time DbContext Factory* implementando `IDesignTimeDbContextFactory<AquaContext>` apontando para a mesma string de conexão em `appsettings.json`.

```csharp
public class AquaContextFactory : IDesignTimeDbContextFactory<AquaContext>
{
    public AquaContext CreateDbContext(string[] args)
    {
        var config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

        var optionsBuilder = new DbContextOptionsBuilder<AquaContext>();
        optionsBuilder.UseOracle(config.GetConnectionString("DefaultConnection"));

        return new AquaContext(optionsBuilder.Options);
    }
}
```

---

## Como Executar

1. No terminal (após configurar `appsettings.json` e aplicar migrations), execute:

   ```bash
   dotnet run
   ```
2. Por padrão, a aplicação iniciará em `https://localhost:5001` (HTTPS) e `http://localhost:5000` (HTTP).
3. Acesse a interface do **Swagger UI** para testar endpoints:

   ```
   https://localhost:5001/swagger/index.html
   ```

---

## Testando Endpoints

1. **Registrar Usuário**

   ```
   POST https://localhost:5001/api/Auth/register
   Content-Type: application/json

   {
     "email": "joao@exemplo.com",
     "password": "SenhaForte@123",
     "name": "João Silva"
   }
   ```

2. **Login**

   ```
   POST https://localhost:5001/api/Auth/login
   Content-Type: application/json

   {
     "email": "joao@exemplo.com",
     "password": "SenhaForte@123"
   }
   ```

   * Na resposta, pegue o campo `token` e use no cabeçalho:

     ```
     Authorization: Bearer <seu_token_jwt_aqui>
     ```

3. **Listar Fontes de Água**

   ```
   GET https://localhost:5001/api/WaterSources
   Authorization: Bearer <token>
   ```

4. **Criar Nova Fonte de Água**

   ```
   POST https://localhost:5001/api/WaterSources
   Authorization: Bearer <token>
   Content-Type: application/json

   {
     "nome": "Poço Sul",
     "descricao": "Poço de teste do setor sul",
     "localizacao": "Bloco Sul, quadra 5",
     "latitude": -23.5587,
     "longitude": -46.6253,
     "type": "Poco",
     "createdById": 1,
     "lastInspected": "2025-06-04T00:00:00Z",
     "status": "Potavel"
   }
   ```

5. **Atualizar Fonte de Água**

   ```
   PUT https://localhost:5001/api/WaterSources/3
   Authorization: Bearer <token>
   Content-Type: application/json

   {
     "nome": "Poço Sul - Atualizado",
     "descricao": "Detectada contaminação após chuva",
     "localizacao": "Bloco Sul, quadra 5",
     "latitude": -23.5587,
     "longitude": -46.6253,
     "type": "Poco",
     "lastInspected": "2025-06-10T00:00:00Z",
     "status": "Contaminada",
     "isActive": true,
     "updateDescricao": "Teste de contaminação no dia 10/06"
   }
   ```

6. **Deletar Fonte de Água**

   ```
   DELETE https://localhost:5001/api/WaterSources/3
   Authorization: Bearer <token>
   ```

7. **Gerenciar Alertas** (sem necessidade de JWT)

   * **GET** `/api/Alerts`
   * **GET** `/api/Alerts/{id}`
   * **POST** `/api/Alerts`
   * **PUT** `/api/Alerts/{id}`
   * **DELETE** `/api/Alerts/{id}`

---
