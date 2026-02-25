# Kairudev — État du projet

> Ce fichier est mis à jour après chaque itération.
> Il est lu par Claude au démarrage de chaque session.

---

## Itérations

| # | Contenu | Statut | Date |
|---|---|---|---|
| ~~#1~~ | ~~BC Tasks — Domain, Application, Adapters, Infrastructure, SQLite, 23 tests~~ | ~~✅ Livré~~ | ~~2026-02-24~~ |
| ~~#2~~ | ~~API REST (ASP.NET Core) + UI Blazor WebAssembly~~ | ~~✅ Livré~~ | ~~2026-02-24~~ |
| ~~#2b~~ | ~~Réécriture spec.md (use cases + diagrammes Mermaid) + prompts agents~~ | ~~✅ Livré~~ | ~~2026-02-24~~ |
| ~~#3~~ | ~~BC Pomodoro — sessions de focus, chrono circulaire, lien avec Tasks~~ | ~~✅ Livré~~ | ~~2026-02-25~~ |
| ~~#3b~~ | ~~.NET Aspire 13.1.1 — AppHost + ServiceDefaults~~ | ~~✅ Livré~~ | ~~2026-02-25~~ |
| ~~#4~~ | ~~Tests d'intégration SQLite (`Kairudev.Infrastructure.Tests`)~~ | ~~✅ Livré~~ | ~~2026-02-25~~ |
| ~~#5~~ | ~~Configuration externalisée — URL API via `appsettings.json`~~ | ~~✅ Livré~~ | ~~2026-02-25~~ |
| #6 | BC Journal — log d'activité quotidien alimenté par les sprints | 📋 Planifié | — |
| #7 | BC Tickets — intégration Jira / Linear / GitHub Issues | 📋 Planifié | — |
| #8 | .NET MAUI — application desktop/mobile | 📋 Planifié | — |

---

## Dernière itération livrée

**#4 + #5 — Tests intégration SQLite + Config externalisée** — Livré le 2026-02-25

### Ce qui a été livré

#### #4 — Tests d'intégration SQLite
- **`InfrastructureTestBase`** : helper SQLite in-memory (`DataSource=:memory:`), schéma créé via `EnsureCreated()`
- **`SqliteTaskRepositoryTests`** : 6 tests (Add, GetById, GetAll vide, GetAll multiple, Update, Delete)
- **`SqlitePomodoroSessionRepositoryTests`** : 7 tests (Add, GetById NotFound, GetActive, GetActive None, Update, CompletedTodayCount, LinkedTask)
- **`SqlitePomodoroSettingsRepositoryTests`** : 3 tests (Defaults, Save, Update)
- **`Kairudev.Infrastructure.csproj`** : `InternalsVisibleTo` pour Kairudev.Infrastructure.Tests
- **Total : 72 tests** (35 Domain + 20 Application + 17 Infrastructure), 0 échec

#### #5 — Configuration externalisée URL API
- **`src/Kairudev.Web/wwwroot/appsettings.json`** : clé `ApiBaseUrl` (valeur dev par défaut)
- **`src/Kairudev.Web/Program.cs`** : lit `builder.Configuration["ApiBaseUrl"]` (fallback `https://localhost:7056`)
- L'URL n'est plus hardcodée dans le code source

### Dette technique héritée et courante
- `TaskStatus` : alias `DomainTaskStatus` nécessaire dans les tests (conflit namespace)
- `DomainErrors` : alias `PomodoroErrors` nécessaire quand Tasks et Pomodoro sont tous deux importés
- `DeveloperTask.StartProgress()` codé dans le domaine, pas exposé en UC ni en endpoint
- `Kairudev.Adapters` : projet toujours présent dans la solution mais vide de sens (suppression à planifier)

---

## Stack technique
- .NET 10 GA (SDK 10.0.200-preview = SDK .NET 10.1 preview, runtime 10 GA)
- SQLite + EF Core 10.0.3 (fichier local `kairudev.db`, hors git)
- ASP.NET Core Web API (`Kairudev.Api`)
- Blazor WebAssembly (`Kairudev.Web`)
- .NET Aspire 13.1.1 (`Kairudev.AppHost` + `Kairudev.ServiceDefaults`)
- .NET MAUI — itération future
- xUnit pour les tests
- Solution : `Kairudev.slnx`

## Structure du projet
```
src/
├── Kairudev.Domain/
│   ├── Tasks/
│   └── Pomodoro/
├── Kairudev.Application/
│   ├── Tasks/
│   └── Pomodoro/
├── Kairudev.Adapters/          ← à supprimer (ADR-007)
├── Kairudev.Infrastructure/    ← migrations dans Persistence/Migrations/
├── Kairudev.Api/               ← Tasks/ + Pomodoro/
├── Kairudev.AppHost/           ← orchestration Aspire
├── Kairudev.ServiceDefaults/   ← OTEL, health checks, service discovery
└── Kairudev.Web/               ← Services/ + Pages/ + wwwroot/appsettings.json
tests/
├── Kairudev.Domain.Tests/      ← Tasks/ + Pomodoro/
├── Kairudev.Application.Tests/ ← Tasks/ + Pomodoro/
└── Kairudev.Infrastructure.Tests/  ← Tasks/ + Pomodoro/ (17 tests intégration SQLite)
docs/
├── spec.md
└── project-state.md
```
