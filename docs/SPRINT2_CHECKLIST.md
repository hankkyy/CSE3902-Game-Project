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
- [ ] **Assigned:** real sprite sheets and complete per-object animation clips
- [ ] **Assigned:** dedicated class for each final dungeon object and enemy projectile behavior
- [ ] **Assigned:** start/menu state (optional for five-person teams, included as polish)

## Process and documentation

- [x] README with setup, controls, architecture, limitations, and status
- [x] equal-effort task plan for all five contributors
- [x] PR, issue, code-review, and reflection templates
- [x] Roslyn/.NET analyzers enabled and CI build added
- [x] Create and assign the five Sprint 2 issues with effort labels
- [ ] Add the five issues to the GitHub Project board
- [ ] Enter the official Sprint 2 start/end dates on the board
- [ ] Each member completes readability and maintainability review records
- [ ] Record weekly analyzer results, complete reflection, zip, and submit on Carmen

## Check-in demo script

1. Build and run the game.
2. Move with arrows and `WASD`; show four directions and animation.
3. Press `Z`/`N`, then `E`; show attack and damage/health.
4. Press `1`, `2`, `3`; show the selected-item bar change.
5. Cycle blocks with `T/Y`, items with `U/I`, and enemies with `O/P` in both directions.
6. Leave an enemy/item visible long enough to show autonomous movement/animation.
7. Press `R` and verify all state resets; press `Q` to quit.
