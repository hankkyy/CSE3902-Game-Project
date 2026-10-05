# Sprint 2 requirements checklist

Legend: **Done** is implemented in the scaffold; **Assigned** has an owner in the task plan.

## Functionality

- [x] **Done:** interfaces for game objects and sprites
- [x] **Done:** drawing separated from gameplay behavior
- [x] **Done:** Command-based keyboard input and Factory-based sprite creation
- [x] **Done:** player movement, four directions, attack, damage, item selection, and animation
- [x] **Done:** block gallery with `T/Y`, stationary behavior, and wraparound
- [x] **Done:** item gallery with `U/I`, animation, and wraparound
- [x] **Done:** enemy/NPC gallery with `O/P`, distinct movement, and animation
- [x] **Done:** `Q` quit and `R` reset
- [x] **Done:** original attributed player sprite sheet with four-direction idle/walk/attack/damage clips
- [x] **Done:** dedicated block and item classes with gallery/reset tests integrated from `feature/items-blocks`
- [ ] **Remaining:** final recognizable item/block/enemy artwork and item-specific animation
- [x] **Implemented:** `1` arrow, `2` returning boomerang, `3` timed bomb, with reset/menu/input tests
- [x] **Implemented:** Starlight Ruins design roster: 6 blocks, 9 gallery items, 7 enemies/NPCs
- [ ] **Remaining:** manually demonstrate every expanded roster entry and review visual fidelity
- [x] **Done:** dedicated player-state, enemy, NPC, and enemy-projectile classes
- [x] **Done:** start/menu state; `Enter` starts gameplay and `Q`/`Escape` exit in either mode
- [x] **Done:** typed discrete commands, on-screen controls/status HUD, and input/menu regression checks

Input/menu/HUD release checks are tracked separately in [INPUT_QUALITY_CHECKLIST.md](INPUT_QUALITY_CHECKLIST.md).
Enemy and player-sprite checks are documented in their feature guides. Item/block behavior and manual checks are documented in [ITEMS_BLOCKS.md](ITEMS_BLOCKS.md). The integration script runs all six headless test projects.

## Process and documentation

- [x] README with setup, controls, architecture, limitations, and status
- [x] equal-effort task plan for all five contributors
- [x] PR, issue, code-review, and reflection templates
- [x] Roslyn/.NET analyzers enabled and CI build added
- [x] Create and assign the five Sprint 2 issues with effort labels
- [x] Add the five issues to the [Sprint 2 GitHub Project board](https://github.com/users/hankkyy/projects/4)
- [x] Set Sprint dates to September 15–28, 2026 on all five project items
- [x] Enter the official Sprint 2 start/end dates on the board
- [ ] Each member completes readability and maintainability review records
- [ ] Record weekly analyzer results, complete reflection, zip, and submit on Carmen

## Final-submission demo walkthrough

1. Build and run the game; confirm the start menu appears and press `Enter`.
2. Move with arrows and `WASD`; show four directions and animation.
3. Press `Z`/`N`, then `E`; show `ATTACKING`/`DAMAGED` in the HUD and the health change.
4. Press `1`, `2`, `3`; demonstrate arrow, returning boomerang, and bomb fuse/explosion.
5. Cycle blocks with `T/Y`, items with `U/I`, and enemies with `O/P` in both directions.
6. Leave an enemy/item visible long enough to show autonomous movement/animation.
7. Press `R` and verify all state resets; press `Q` to quit.
