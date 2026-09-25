# Sprint 2 — Five-person task plan

The assignments below use the five GitHub contributors already present in the repository. Each owner has about **8 effort points**. Player work is split between two people as recommended by the assignment. Owners may coordinate interfaces, but each person should author and have reviewed at least one class.

Project board: [CSE 3902 Team Sprint 2](https://github.com/users/hankkyy/projects/4)

Sprint start: **September 25, 2026**

Sprint due date: **September 28, 2026**

> Sprint 开始日期：**2026 年 9 月 25 日**
>
> Sprint 截止日期：**2026 年 9 月 28 日**

| Owner | Branch | Scope (8 points each) | Acceptance criteria | Reviewer |
|---|---|---|---|---|
| `@hankkyy` | `feature/player-states` | Replace the placeholder Player action timer with explicit idle/walk/attack/damaged states (4); add sword/item-use presentation and state-safe transitions (2); add player-focused tests and docs (2) | Four directions animate; `Z/N` attacks; `E` damages and flashes; `1–3` changes the used item; invalid overlapping states are prevented; build passes | `@JojoLi132` |
| `@JojoLi132` | `feature/player-sprites` | Import/team-create a legal Link-style sprite sheet and content pipeline entry (3); implement directional animation clips in `ISprite` (3); tune frame timing and document asset attribution (2) | Walking/idle/attack/damage have visible frames for four directions; gameplay code contains no source rectangles; asset source/license is documented; build passes | `@hankkyy` |
| `@mo46-123` | `feature/enemies-npcs` | Create separate Octorok, Keese, Gel, and NPC classes (3); add characteristic movement/state animation (3); add an enemy projectile demo plus tests/docs (2) | `O/P` cycles all characters; moving enemies stay in demo bounds; Octorok fires a visible projectile; NPC remains stationary; reset is deterministic | `@Lzzz-7` |
| `@Lzzz-7` | `feature/items-blocks` | Create final block/object classes and visuals (3); create item classes with appropriate animation/motion (3); add gallery/reset tests and asset docs (2) | `T/Y` cycles at least four stationary blocks; `U/I` cycles at least four recognizable animated items; wraparound and `R` work; build passes | `@xing-gif` |
| `@xing-gif` | `feature/input-quality` | Expand typed Command classes and input tests (3); add a start/menu game state entered with Enter (2); add HUD labels/help overlay and polish layout (2); maintain CI/release checklist (1) | Upper/lowercase letter input works through XNA keys; every required key has one command; menu starts the game on Enter; controls appear on-screen; CI and formatter pass | `@mo46-123` |

Each owner must use the matching prompt in `.codex/tasks/` and follow the file-ownership table in `AGENTS.md`. Shared contracts are frozen in `docs/ARCHITECTURE_CONTRACT.md`; feature agents propose changes instead of editing those files independently.

## GitHub issues

1. [#8 — Player gameplay states and actions](https://github.com/hankkyy/CSE3902-Game-Project/issues/8) — `@hankkyy`
2. [#9 — Player sprites and four-direction animation](https://github.com/hankkyy/CSE3902-Game-Project/issues/9) — `@JojoLi132`
3. [#10 — Enemy/NPC classes, movement, and projectile](https://github.com/hankkyy/CSE3902-Game-Project/issues/10) — `@mo46-123`
4. [#11 — Block and item classes, sprites, and galleries](https://github.com/hankkyy/CSE3902-Game-Project/issues/11) — `@Lzzz-7`
5. [#12 — Commands, menu state, HUD, and CI quality](https://github.com/hankkyy/CSE3902-Game-Project/issues/12) — `@xing-gif`

All five issues are open with labels `sprint-2` and `effort-8`. Initial status: **Todo**. Reduce remaining effort at the end of each work session and set it to zero only when acceptance criteria are met.

## Integration and review

The modules can proceed in parallel. Merge small contract changes first. Recommended feature-PR order: player sprites → items/blocks → enemies → player states → input/menu/HUD. The reviewer ring ensures every member reviews and is reviewed once. First review focuses on readability; the second focuses on maintainability and records a hypothetical future change.
