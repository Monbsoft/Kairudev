# Kairudev — État du projet

> Ce fichier est mis à jour après chaque itération.
> Il est lu par Claude au démarrage de chaque session.

---

## Dernière itération livrée
**#2** — Livrée le 2026-02-24

### Ce qui a été livré
- `Kairudev.Api` : API REST ASP.NET Core (4 endpoints Tasks)
- `Kairudev.Web` : UI Blazor WebAssembly (liste, ajout, complétion, suppression)
- Migration EF Core InitialCreate générée
- `IDesignTimeDbContextFactory` dans Infrastructure
- 23 tests passants (inchangés)

### Dette technique
- URL API hardcodée dans `Program.cs` du Web (`https://localhost:7056`)
- Pas de tests d'intégration SQLite
- `TaskStatus` : alias `DomainTaskStatus` nécessaire dans les tests (conflit namespace)

---

## Itération suivante — options
1. **Sessions Pomodoro** — Bounded Context Pomodoro, lien avec Tasks
2. **Tests d'intégration SQLite**
3. **Configuration externalisée** — URL API via `appsettings.json`

---

## Stack technique
- .NET 10 (preview)
- SQLite + EF Core 10 (fichier local `kairudev.db`, hors git)
- ASP.NET Core Web API (`Kairudev.Api`) — `https://localhost:7056`
- Blazor WebAssembly (`Kairudev.Web`) — `https://localhost:7204`
- .NET MAUI — itération future
- xUnit pour les tests

## Structure du projet
```
src/
├── Kairudev.Domain/
├── Kairudev.Application/
├── Kairudev.Adapters/
├── Kairudev.Infrastructure/    ← migrations dans Persistence/Migrations/
├── Kairudev.Api/               ← Tasks/TasksController + Tasks/Presenters/
└── Kairudev.Web/               ← Services/TaskApiClient + Pages/Tasks.razor
tests/
├── Kairudev.Domain.Tests/
├── Kairudev.Application.Tests/
└── Kairudev.Infrastructure.Tests/  ← vide, à compléter
docs/
├── spec.md
└── project-state.md
```
