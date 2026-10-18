# CSE 3902 Sprint 3 — working agreement

This is the authoritative instruction set for contributors and coding agents. Read it together with `docs/SPRINT3_ARCHITECTURE.md`, `docs/COLLISION_MATRIX.md`, and the task file matching the current branch before changing code.

## Start every task this way

1. Run `git branch --show-current` and `git status --short`.
2. Read this file, `docs/SPRINT3_ARCHITECTURE.md`, `docs/COLLISION_MATRIX.md`, and `.codex/tasks/<branch-suffix>.md`.
3. Confirm the requested work belongs to the current branch.
4. Run `./scripts/verify.sh` before editing.
5. Preserve existing work. Never reset, rewrite, or delete another contributor's changes.

If the branch has no matching task file, stop and ask the integrator which Sprint 3 assignment it belongs to.

## Branches, ownership, and review

| Branch | Owner | Owned production paths | Required reviewer |
|---|---|---|---|
| `feature/s3-collision-core` | `@hankkyy` | `GameProject/Collision/**`, collision contracts approved in `Core`, player solid-collision integration | `@JojoLi132` |
| `feature/s3-room-loading` | `@JojoLi132` | `GameProject/Rooms/Definitions/**`, `GameProject/Rooms/Loading/**`, `GameProject/Content/Rooms/**`, room visuals | `@mo46-123` |
| `feature/s3-combat-collisions` | `@mo46-123` | `GameProject/Combat/**`, `GameProject/Objects/Enemies/**`, enemy/projectile collision integration | `@Lzzz-7` |
| `feature/s3-items-blocks-collisions` | `@Lzzz-7` | `GameProject/Inventory/**`, `GameProject/Objects/Items/**`, `GameProject/Objects/Blocks/**`, pickup/push/door handlers | `@xing-gif` |
| `feature/s3-room-navigation-integration` | `@xing-gif` | `GameProject/Rooms/Navigation/**`, `Controllers/MouseController.cs`, `UI/**`, `Game1.cs`, integration tests and CI | `@hankkyy` |
| `integration/sprint3` | integrator only | shared contracts, task records, integration fixes approved by affected owners | reviewer for affected feature |

Tests and feature-specific documentation may be added under a matching subfolder. Do not edit another owner's production paths. Shared files (`Core/**`, project files, `AGENTS.md`, architecture documents, and build scripts) require a small integrator-owned contract PR before feature implementations depend on the change.

## Required implementation rules

- Target .NET 8 and MonoGame DesktopGL. Do not add a package without team approval.
- `Game1` remains a composition root; collision rules, room parsing, and gameplay state do not belong there.
- Rooms are created from external data files. Do not hard-code room object lists in `Game1`.
- Input belongs in `Controllers`; discrete input is edge-triggered and invokes commands or navigation services.
- Every collidable object exposes an explicit hitbox and collision category through the approved contract.
- Collision detection and collision response are separate. Responses are dispatched by the pair types in `docs/COLLISION_MATRIX.md`.
- Never modify a collection while enumerating it. Queue removals and room transitions until collision processing ends.
- Use `gameTime.ElapsedGameTime`; do not use frame counts or wall-clock time for gameplay.
- `Update` changes state and `Draw` only renders.
- Room transitions, collection, enemy death, and reset must be deterministic.
- Preserve the existing required controls: arrows/WASD, Z/N, 1–3, R, Q/Escape, and Enter.
- Mouse room navigation is required for Sprint 3. Gamepad support is optional.
- Sound and smooth camera transitions are out of Sprint 3 scope.

## Task tracking requirements

- Each feature starts at 12 effort points and an estimated 15 hours.
- Move the issue to In Progress when work starts.
- At the end of every work session, record actual time and reduce Remaining Hours.
- Set Remaining Hours to zero only after acceptance criteria, tests, documentation, and review are complete.
- Keep one feature per PR and link its issue with `Closes #...`.
- Each owner must provide repeatable manual test steps and automated headless tests where possible.

## Reviews and reflection

Each reviewer examines at least two named classes: one for readability and one for maintainability. Record reviewer, date, minutes, files, specific comments, a hypothetical future change, and the PR link. Every member must also contribute a signed paragraph to the Sprint 3 reflection.

## Definition of done

Before requesting review:

```sh
./scripts/verify.sh
./scripts/analyze-strict.sh
```

The normal verification script must pass. New strict-analysis warnings in changed code must be fixed or documented individually; blanket suppressions are not accepted. The PR must satisfy its branch task, stay inside owned paths, include tests/docs, identify its reviewer, and include exact manual test steps.
