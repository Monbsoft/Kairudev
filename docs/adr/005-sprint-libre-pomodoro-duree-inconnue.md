# ADR-005 — Sprint libre : Pomodoro à durée inconnue

**Statut :** Accepté  
**Date :** 2026-03-25

---

## Contexte et problème

Le BC Sprint a été introduit en itération #17 comme un bounded context séparé pour le "sprint libre" (chronomètre count-up). La question s'est posée lors de l'itération #22 : faut-il fusionner `SprintSession` avec `PomodoroSession` ?

## Décision

**Le sprint libre est conceptuellement un Pomodoro à durée inconnue.** Exemple d'usage : une réunion ou un daily standup dont la durée n'est pas connue à l'avance.

Les deux entités partagent le même objectif métier (session de focus d'un développeur), les mêmes événements journal (`SprintStarted`, `SprintCompleted`, `SprintInterrupted`), et la même relation avec les tâches.

**Pour l'itération #22**, les deux bounded contexts restent séparés — la fusion est inscrite comme dette technique à traiter en itération #23.

## Conséquences

**Avantages du statu quo (séparation) :**
- Aucune migration ou refactoring risqué dans le périmètre d'un bugfix
- `SprintSession` reste simple (Record a posteriori, timer client-side)

**Inconvénients connus :**
- `JournalEntryMapper` ne résout pas les tâches liées aux sprints libres (bug latent : le mapper tente une lookup `PomodoroSessionId` sur un `SprintSessionId`)
- Duplication de logique entre les deux BCs

## Alternatives considérées

**Option A (retenue pour itération #23) — Fusionner dans `PomodoroSession` :**
- Ajouter `PomodoroSessionType.Free` avec `PlannedDurationMinutes = null`
- Supprimer `SprintSession`, `ISprintSessionRepository`, le BC Sprint
- Le `JournalEntryMapper` bénéficierait automatiquement de l'enrichissement tâches

**Option B (statu quo itération #22) :**
- Garder les deux séparés, corriger le bug de note, reporter la fusion

## Références

- Itération #17 : introduction du BC Sprint libre
- Itération #22 : correction note + identification du besoin de fusion
