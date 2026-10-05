# Sprint 2 integration record

The integration branch now combines all five submitted feature scopes, including `feature/items-blocks`, while preserving contributor commits and the frozen public contracts. Integration does not mean every course requirement or manual check is complete.

## Integrated functionality

- Explicit idle, walking, attacking, and damaged player states.
- Original CC0 four-direction player atlas wired to gameplay state.
- Dedicated Octorok, Keese, Gel, and NPC behaviors plus a projectile demonstration.
- Typed commands, key-edge handling, start menu, gameplay HUD, and reset/quit behavior.
- Dedicated Stone, Push, Water, and Statue block classes; Heart, Rupee, Key, and Bomb item classes. Existing constructors remain compatible. Blocks are stationary; items use time-based in-place preview animation. Gallery cycling and deterministic reset include hidden items.

## Working-copy roster expansion

Ten new objects add Crystal Pillar, Rune Tile, five item previews, and three distinct enemy behaviors. Lantern Keeper is the display name of the existing NPC. The start-menu subtitle and window title identify Starlight Ruins. No collision, collection, room navigation, or damage between objects is introduced. See the roster document for future room roles and manual checks.

## Automated verification

Run `bash scripts/verify.sh` from the Git repository root. It checks repository hygiene, formatting, analyzers, the Release build, and these six test projects:

1. `tests/PlayerStates/PlayerStates.Tests.csproj`
2. `tests/PlayerSprites/PlayerSprites.Tests.csproj` (79 headless animation checks)
3. `tests/Enemies/Enemies.Tests.csproj`
4. `tests/InputTests/InputTests.csproj` (25 input/menu checks)
5. `tests/ItemsBlocks/ItemsBlocks.csproj`
6. `tests/PlayerItems/PlayerItems.Tests.csproj` (arrow, boomerang, bomb)

The existing GitHub Actions workflow invokes the same script for PRs targeting `main` and pushes to `main`. A push to the integration branch alone does not trigger that workflow. Do not report a remote CI pass unless a run for the intended commit actually completed successfully.

## Remaining functionality and visual checks

- Older gallery sprites remain placeholders; ten added roster objects use original procedural pixel motifs. Final recognizable artwork and object-specific animation still require visual review.
- Number keys now demonstrate arrow, boomerang, and bomb use; dedicated player item-use poses remain future visual work.
- Run the full keyboard/visual walkthrough on the integrated game, including four-direction attacks, damage, all galleries, reset, and quit. Confirm placeholder limitations honestly in the demo.
- R resets gameplay objects and gallery selections while staying in gameplay. Confirm this interpretation of initial state with the course expectations.
- The working copy now uses the designed [Starlight Ruins roster](STARLIGHT_RUINS_ROSTER.md): 6 blocks, 9 gallery items, 7 enemies/NPCs. This is a defined original mini-dungeon scope, not proof of grading acceptance or a complete playable dungeon.

## Human work still required

Automated checks do not replace requested code reviews, manual demonstrations, project-board updates, or course submission. Record the required readability and maintainability reviews, update remaining effort, and complete the reflection at the appropriate sprint stage. Follow the course Sprint 0 packaging instructions, test the extracted submission, and have one team member upload it to the correct Carmen assignment. No review, packaging, or submission completion is asserted here.
