# CSE 3902 Sprint 2 — Codex working agreement

This file is the authoritative instruction set for every Codex agent working in this repository. Read it together with `docs/ARCHITECTURE_CONTRACT.md` and the task file matching the current Git branch before changing code.

## Start every task this way

1. Run `git branch --show-current` and `git status --short`.
2. Read this entire file, `docs/ARCHITECTURE_CONTRACT.md`, and `.codex/tasks/<current-branch-suffix>.md`.
3. Confirm that the requested work belongs to the current branch. Do not implement another teammate's scope.
4. Run `./scripts/verify.sh` before editing to establish a clean baseline.
5. Preserve user changes and never rewrite, reset, or delete another contributor's work.

If the branch has no matching task file, stop and ask which assignment it belongs to.

## Non-negotiable architecture rules

- Target .NET 8 and MonoGame DesktopGL. Do not change framework or add a package without an approved architecture proposal.
- `Game1` is the composition root only. It may create objects and call `Update`/`Draw`; gameplay rules do not belong there.
- Input belongs in `Controllers`; a key invokes an `ICommand`, never mutates unrelated game objects directly from `Game1`.
- Gameplay state belongs in `Objects`; objects draw only through `ISprite`.
- Texture creation, animation frames, source rectangles, and colors belong in `Sprites` or `Content`, never in gameplay objects.
- All moving behavior must use `gameTime.ElapsedGameTime`; do not make movement depend on frame rate.
- New object families implement `IGameObject`. Galleries use `ObjectGallery<T>` and must wrap in both directions.
- Keep reset deterministic: `R` must restore positions, health, timers, selections, and gallery indexes.
- Do not rename or change public members listed as frozen in `docs/ARCHITECTURE_CONTRACT.md`.
- Prefer adding a focused class over expanding a switch statement in `Game1`.
- No service locators, global mutable state, static game objects, duplicated key polling, or direct dependencies between sibling feature modules.

## Ownership and merge-conflict rules

Edit only the paths owned by the current branch. Reading any file is allowed.

| Branch | Owned paths |
|---|---|
| `feature/player-states` | `GameProject/Objects/Player.cs`, `GameProject/Objects/PlayerStates/**`, player tests/docs |
| `feature/player-sprites` | `GameProject/Sprites/**`, `GameProject/Content/**`, asset attribution docs |
| `feature/enemies-npcs` | `GameProject/Objects/EnemyObject.cs`, `GameProject/Objects/Enemies/**`, enemy tests/docs |
| `feature/items-blocks` | `GameProject/Objects/ItemObject.cs`, `GameProject/Objects/BlockObject.cs`, `GameProject/Objects/Items/**`, `GameProject/Objects/Blocks/**`, item/block tests/docs |
| `feature/input-quality` | `GameProject/Commands/**`, `GameProject/Controllers/**`, `GameProject/States/**`, `GameProject/UI/**`, `GameProject/Game1.cs`, CI/release docs |

Shared/frozen files include `GameProject/Core/**`, `GameProject/GameProject.csproj`, `Directory.Build.props`, `AGENTS.md`, and `docs/ARCHITECTURE_CONTRACT.md`. Only the team integrator changes them.

If your task truly requires a frozen-file or another owner's change, do not edit it. Add `docs/architecture-proposals/<branch-name>.md` describing the smallest API change, why it is needed, and affected owners; then call it out in the PR for the integrator.

## Code standard

- Use file-scoped namespaces, nullable reference types, four-space indentation, and one public type per file.
- Use descriptive English identifiers. Public types and members use PascalCase; private fields use camelCase.
- Keep methods small and single-purpose. Avoid unexplained numbers; use named constants.
- Public classes and non-obvious state transitions need concise XML documentation.
- Do not suppress analyzer warnings without a written explanation in the PR.
- Do not commit `bin/`, `obj/`, IDE settings, secrets, generated packages, or unapproved/copyrighted assets.
- Add or update tests for behavior that can be tested without a graphics device. For visual behavior, document repeatable manual test steps and attach a screenshot/GIF to the PR.

## Compatibility requirements

These controls may not change: arrows/WASD move, Z/N attack, 1–3 select items, E damages, T/Y cycle blocks, U/I cycle items, O/P cycle enemies/NPCs, R resets, Q exits. Letter keys are case-insensitive through MonoGame's `Keys` enum.

Do not break the existing constructor calls in `Game1`. New constructor parameters must be optional or introduced through an additive overload until the input-quality owner performs integration.

## Definition of done

Before committing, run:

```sh
./scripts/verify.sh
```

The task is done only when formatting, Release build, analyzers, and repository hygiene checks pass; the assigned acceptance criteria are demonstrated; the README/docs reflect user-visible changes; and the PR changes only owned paths (plus task-specific tests/docs). Every PR must identify its reviewer and include exact manual test steps.
