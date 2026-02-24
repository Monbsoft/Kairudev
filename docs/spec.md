# Kairudev — Spécification

## Vision produit
Application de gestion d'activité quotidienne pour développeurs : todo list de micro-tâches,
journal de bord, intégration tickets (Jira/Linear/GitHub Issues), sessions Pomodoro.

---

## Bounded Contexts identifiés
- **Tasks** — micro-tâches quotidiennes ✅ itération #1
- **Journal** — notes de bord (à venir)
- **Tickets** — intégration externe (à venir)
- **Pomodoro** — sessions de focus avec lien aux tâches (à venir)

---

## Use Cases identifiés

### Tasks
- [x] AddTask
- [x] ListTasks
- [x] CompleteTask
- [x] DeleteTask

---

## Fonctionnalités livrées

- [x] **Bounded Context Tasks — CRUD de base** — itération #1 — 2026-02-24
- [x] **API REST + UI Blazor WebAssembly** — itération #2 — 2026-02-24

---

## ADR (Architecture Decision Records)

### ADR-001 — Clean Architecture + DDD
- **Contexte :** Besoin d'une base solide, évolutive, testable, multi-UI (Web + MAUI).
- **Décision :** Clean Architecture (Uncle Bob) avec patterns DDD. Boundary pattern complet
  (InputBoundary / Interactor / OutputBoundary). Dépendances pointant uniquement vers l'intérieur.
- **Conséquences :** Les Use Cases sont indépendants de l'UI et de la persistance.
  L'ajout de Web ou MAUI ne touche pas au Domain ni à l'Application.

### ADR-002 — SQLite via EF Core (fichier local)
- **Contexte :** Première itération, besoin simple, zéro infra.
- **Décision :** SQLite + EF Core 10, fichier `kairudev.db` exclu du git.
  Interface `ITaskRepository` dans Domain, implémentation dans Infrastructure.
- **Conséquences :** Swap PostgreSQL = nouvelle implémentation `PostgresTaskRepository`,
  aucun impact sur Domain/Application.

### ADR-003 — .NET 10 preview
- **Contexte :** Choix utilisateur.
- **Décision :** Cible `net10.0`. À surveiller : stabilité SDK preview.
- **Conséquences :** Warning NETSDK1057 non bloquant.

### ADR-004 — Controllers composent les Interactors
- **Contexte :** Le presenter HTTP est spécifique à chaque requête, pas injectable globalement.
- **Décision :** Le Controller instancie le presenter et l'Interactor à chaque action,
  avec `ITaskRepository` injecté via DI.
- **Conséquences :** Composition explicite et lisible, sans factory ni abstraction supplémentaire.

### ADR-005 — Blazor WebAssembly standalone
- **Contexte :** Cible multi-UI (Web + MAUI). Partage de composants via Blazor Hybrid futur.
- **Décision :** Blazor WASM standalone, communique uniquement via l'API REST.
  `TaskDto` défini dans le projet Web (pas de référence vers le Domain).
- **Conséquences :** Le projet Web est totalement découplé du backend.

---

## Ports de développement
- API : `https://localhost:7056`
- Web : `https://localhost:7204`

---

## Dette technique
- `Kairudev.Infrastructure.Tests` : aucun test d'intégration SQLite.
- `TaskStatus` conflit de nom avec `System.Threading.Tasks.TaskStatus` → alias `DomainTaskStatus` dans les tests.
- URL de l'API hardcodée dans `Program.cs` du Web → à externaliser en configuration.

---

## Prochaine itération
Options à prioriser :
1. **Sessions Pomodoro** — Bounded Context Pomodoro, lien avec Tasks
2. **Tests d'intégration SQLite** — compléter `Kairudev.Infrastructure.Tests`
3. **Configuration externalisée** — URL API via `appsettings.json` dans Blazor WASM
