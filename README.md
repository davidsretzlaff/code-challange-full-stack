## Code Challenge Full Stack — Employee Management

Guia rápido para rodar o Backend (.NET 8 + EF InMemory) e o Frontend (Next.js 16 + React 19) em desenvolvimento.

### Pré‑requisitos
- Node.js 20 LTS ou superior (recomendado)
- .NET SDK 8.0+
- NPM 10+ (ou PNPM/Yarn se preferir)

### Estrutura
```
Backend/
  src/
    Employee.API/              # API ASP.NET Core (porta padrão: 5058)
    Employee.Application/      # Casos de uso (Application)
    Employee.Domain/           # Entidades de domínio (Domain)
    Employee.Infrastructure/   # EF Core + Repositórios (Infra)
Frontend/                      # Next.js (UI)
```

---

## Backend (ASP.NET Core)

### Como rodar
1. Terminal na raiz do repositório:
   ```bash
   cd Backend
   dotnet build
   dotnet run --project src/Employee.API/EmployeeManagement.API.csproj
   ```
2. A API sobe (por padrão) em `http://localhost:5058` com Swagger em `http://localhost:5058/swagger`.

### Autenticação
- Endpoint de login: `POST /api/auth/login`
- Credenciais padrão (mock):
  - email: `admin@admin.com`
  - password: `123456`
- A resposta inclui `accessToken` (JWT) e `expiresInSeconds`.

### Recursos principais
- `GET /api/employees` — lista funcionários
- `GET /api/employees/{id}` — detalhe
- `POST /api/employees` — cria
- `PUT /api/employees/{id}` — atualiza
- `DELETE /api/employees/{id}` — remove

Observações:
- O projeto usa EF Core InMemory para desenvolvimento (nenhum banco externo necessário).
- CORS está liberado apenas em Development.

### Variáveis e configuração
- `appsettings.Development.json` já contém a seção `Jwt` (Issuer, Audience, Secret) para desenvolvimento.
- Para alterar a porta, você pode:
  - Editar `Backend/src/Employee.API/Properties/launchSettings.json` (campo `applicationUrl`), ou
  - Executar com: `dotnet run --project src/Employee.API/EmployeeManagement.API.csproj --urls http://localhost:5058`

---

## Frontend (Next.js)

### Variáveis de ambiente
Crie `Frontend/.env.local` com a URL base da API (incluindo `/api`):
```bash
NEXT_PUBLIC_API_URL=http://localhost:5058/api
```

### Como rodar
1. Terminal na raiz do repositório:
   ```bash
   cd Frontend
   npm install
   npm run dev
   ```
2. A aplicação sobe (por padrão) em `http://localhost:3000`.

### Fluxo de login (UI)
1. Acesse `http://localhost:3000/login`.
2. Use as credenciais: `admin@admin.com / 123456`.
3. Após logar, o token é salvo no `localStorage` e as chamadas passam a enviar `Authorization: Bearer <token>`.

---

## Passo a passo end‑to‑end
1. Suba o Backend na porta `5058`.
2. Configure `Frontend/.env.local` com `NEXT_PUBLIC_API_URL=http://localhost:5058/api`.
3. Suba o Frontend em `http://localhost:3000`.
4. Faça login na UI (`/login`).
5. Navegue pela lista e CRUD de Employees.

---

## Scripts úteis
Backend:
```bash
dotnet build                             # compila
dotnet run --project src/Employee.API/... # roda a API
```

Frontend:
```bash
npm run dev    # desenvolvimento
npm run build  # build de produção
npm run start  # inicia build de produção
```

---

## Troubleshooting
- Frontend não conecta ao Backend:
  - Verifique `Frontend/.env.local` — a URL deve incluir `/api` (ex.: `http://localhost:5058/api`).
  - Confirme se a API está ativa em `http://localhost:5058/swagger`.
- 401 após algum tempo:
  - O token expira. Faça login novamente ou limpe `localStorage` (`auth_token`, `auth_expires_at`).
- Conflito de porta (5058 ou 3000):
  - Ajuste a porta do Backend via `launchSettings.json` ou `--urls`.
  - Rode o Frontend com `PORT=3001 npm run dev`.
- CORS em ambientes não‑dev:
  - Em produção, configure CORS explicitamente e defina `NEXT_PUBLIC_API_URL` para o host correto (sempre com `/api`).

---

## Notas de arquitetura
- Camadas seguem DDD (Domain, Application, Infrastructure) e princípios SOLID.
- API expõe controllers finos; casos de uso concentram a lógica de aplicação.
- Infra usa EF Core InMemory em dev; troque por provider relacional quando necessário.


