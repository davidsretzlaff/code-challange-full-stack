## Code Challenge Full Stack — Employee Management
<img width="2548" height="972" alt="image" src="https://github.com/user-attachments/assets/b7057dbc-04f5-4d0a-a7f3-c5ed58af701d" />

Quick guide to run the Backend (.NET 8 + EF InMemory) and the Frontend (Next.js 16 + React 19) in development.

### Prerequisites
- Node.js 20 LTS or newer (recommended)
- .NET SDK 8.0+
- NPM 10+ (or PNPM/Yarn if you prefer)

### Structure
```
Backend/
  src/
    Employee.API/              # ASP.NET Core API (default port: 5058)
    Employee.Application/      # Use cases (Application)
    Employee.Domain/           # Domain entities (Domain)
    Employee.Infrastructure/   # EF Core + Repositories (Infra)
Frontend/                      # Next.js (UI)
```

---

## Backend (ASP.NET Core)

### How to run
1. Terminal at the repository root:
   ```bash
   cd Backend
   dotnet build
   dotnet run --project src/Employee.API/EmployeeManagement.API.csproj
   ```
2. The API starts (by default) at `http://localhost:5058` with Swagger at `http://localhost:5058/swagger`.

### Authentication
- Login endpoint: `POST /api/auth/login`
- Default credentials (mock):
  - email: `admin@admin.com`
  - password: `123456`
- The response includes `accessToken` (JWT) and `expiresInSeconds`.

### Main endpoints
- `GET /api/employees` — list employees
- `GET /api/employees/{id}` — detail
- `POST /api/employees` — create
- `PUT /api/employees/{id}` — update
- `DELETE /api/employees/{id}` — delete

Notes:
- The project uses EF Core InMemory for development (no external database required).
- CORS is allowed only in Development.

### Variables and configuration
- `appsettings.Development.json` already contains the `Jwt` section (Issuer, Audience, Secret) for development.
- To change the port, you can:
  - Edit `Backend/src/Employee.API/Properties/launchSettings.json` (`applicationUrl` field), or
  - Run with: `dotnet run --project src/Employee.API/EmployeeManagement.API.csproj --urls http://localhost:5058`

---

## Frontend (Next.js)

### Environment variables
Create `Frontend/.env.local` with the API base URL (including `/api`):
```bash
NEXT_PUBLIC_API_URL=http://localhost:5058/api
```

### How to run
1. Terminal at the repository root:
   ```bash
   cd Frontend
   npm install
   npm run dev
   ```
2. The app starts (by default) at `http://localhost:3000`.

### Login flow (UI)
1. Go to `http://localhost:3000/login`.
2. Use the credentials: `admin@admin.com / 123456`.
3. After logging in, the token is saved in `localStorage` and requests start sending `Authorization: Bearer <token>`.

---

## End-to-end walkthrough
1. Start the Backend on port `5058`.
2. Set `Frontend/.env.local` with `NEXT_PUBLIC_API_URL=http://localhost:5058/api`.
3. Start the Frontend at `http://localhost:3000`.
4. Log in via the UI (`/login`).
5. Navigate through the list and CRUD of Employees.

---

## Useful scripts
Backend:
```bash
dotnet build                             # build
dotnet run --project src/Employee.API/... # run the API
```

Frontend:
```bash
npm run dev    # development
npm run build  # production build
npm run start  # start production build
```

---

## Troubleshooting
- Frontend cannot connect to the Backend:
  - Check `Frontend/.env.local` — the URL must include `/api` (e.g., `http://localhost:5058/api`).
  - Confirm the API is running at `http://localhost:5058/swagger`.
- 401 after some time:
  - The token expires. Log in again or clear `localStorage` (`auth_token`, `auth_expires_at`).
- Port conflict (5058 or 3000):
  - Adjust the Backend port via `launchSettings.json` or `--urls`.
  - Run the Frontend with `PORT=3001 npm run dev`.
- CORS in non-dev environments:
  - In production, configure CORS explicitly and set `NEXT_PUBLIC_API_URL` to the correct host (always including `/api`).

---

## Architecture notes
- Layers follow DDD (Domain, Application, Infrastructure) and SOLID principles.
- API exposes thin controllers; use cases concentrate the application logic.
- Infra uses EF Core InMemory in dev; switch to a relational provider when needed.