# CSE 3902 Game Project — Sprint 2

A MonoGame functionality gallery for Sprint 2 (Game Objects and Sprites). The integrated build includes an original CC0 player atlas, dedicated player/enemy behaviors, a start menu, and an on-screen gameplay HUD. Dedicated item/block implementations and their gallery/reset tests are also integrated; their visuals now use simple original pixel motifs.

## Sprint 2 implementation status

The current build uses an original extension theme, **Starlight Ruins**, while retaining the existing course scaffold and player controls. The selected mini-dungeon roster contains **6 blocks, 9 gallery items, and 7 enemies/NPCs**. See [the roster and planned room roles](docs/STARLIGHT_RUINS_ROSTER.md). These counts are our design choices, not a numeric grading threshold.

The current build provides:

- Player movement with arrow keys or `WASD`, four facing directions, and walking animation.

- Attack state with `Z` or `N`, damage state with `E`, three usable items: `1` fires an arrow, `2` throws a returning boomerang, and `3` places a timed bomb.

- Six dedicated block classes: Stone, Push, Water, Statue, Crystal Pillar, and Rune Tile. Cycle with `T`/`Y`; all remain stationary.

- Nine gallery items: Heart, Rupee, Key, Bomb, Bow, Boomerang, Ruins Map, Star Compass, and Star Shard. Dedicated classes use elapsed-time preview animation and `U`/`I` cycling. Pickup previews are separate from using items with `1`/`2`/`3`.

- Seven enemies/NPCs cycled with `O`/`P`: Octorok, Keese, Gel, Lantern Keeper (existing NPC), Rune Wisp, Clockwork Beetle, and Prism Sentinel. New behaviors include floating, a four-sided patrol, and stationary charging/light shots.

- Reset with `R`; quit with `Q` or `Escape`.

- A start menu entered with `Enter`, with `Q`/`Escape` available in both the menu and gameplay.

- On-screen health, item slot, action, facing direction, gallery names/indexes, and control instructions.

- Typed discrete commands and 25 headless input/menu regression checks.

- Command, Factory Method, and object/sprite interfaces for separation of concerns.

This integrates all five submitted feature scopes into a Sprint 2 functionality gallery. Remaining functionality and visual limitations are listed below; it is not a finished dungeon. The five assignments and their integration boundaries are recorded in [the Sprint 2 task plan](docs/SPRINT2_TASKS.md).

## Requirements and setup

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)

- A Windows, macOS, or Linux graphics environment supported by MonoGame DesktopGL.

- Git.

Run these commands from the repository root:

```sh
dotnet restore GameProject/GameProject.csproj
dotnet build GameProject/GameProject.csproj
dotnet run --project GameProject/GameProject.csproj
```

Visual Studio users can open `GameProject/GameProject.csproj` directly. VS Code and Rider users can open the repository root and run the same commands.

## Controls

### `Enter`

Start gameplay from the menu. Gameplay controls are inactive until the game starts; a discrete key held in the menu must be released and pressed again to trigger in gameplay.

### Arrow keys or `WASD`

Move Link and change facing direction.

### `Z` or `N`

Attack with the sword. The HUD briefly shows `ATTACKING`, and the directional attack clip is selected from the player atlas.

### `1`, `2`, or `3`

Press `1` to fire an arrow, `2` to throw a boomerang that returns to Link's current position, or `3` to place a bomb that explodes after 1.2 seconds. Each key selects and uses that item. One effect of each type may be active at a time; release and press again after it ends. New uses are blocked during sword attacks or damage. Item effects do not collide with or damage other objects. See [arrow checks](docs/PLAYER_ARROW.md) and [boomerang/bomb checks](docs/PLAYER_SECONDARY_ITEMS.md).

### `E`

Damage Link.

### `T` or `Y`

Show the previous or next block.

### `U` or `I`

Show the previous or next item.

### `O` or `P`

Show the previous or next enemy/NPC.

### `R`

Reset all objects during gameplay, including health, selected item, positions, timers, gallery indexes, and all active player arrows, boomerangs, bombs, and explosions. Gameplay stays active.

### `Q` or `Escape`

Quit from either the start menu or gameplay.

The top HUD shows health, selected item slot, action, and facing direction. The red and gold bars visualize health and item selection. The right panel shows each gallery's name, current selection, count, and previous/next keys. The bottom row lists the gameplay controls.

## Architecture

```text
GameProject/
├── Commands/       Command interface and keyboard actions
├── Controllers/    Input mapping and key-edge detection
├── Core/           Shared object contracts, direction, gallery
├── Objects/        Player, blocks, items, enemies/NPCs
├── Sprites/        Drawing contracts, player animation clips, sprite factory
├── States/         Menu-to-gameplay session state
├── UI/             Start menu, pixel text, status HUD and help
├── Game1.cs        Composition root and game loop
└── Program.cs      Application entry point
```

`Game1` composes the systems. `KeyboardController` translates input into commands. Objects own behavior and state but draw through `ISprite`; `SpriteFactory` owns visual construction. This lets art, player state, enemies, items, and controls evolve in separate branches with fewer merge conflicts.

## Team workflow

1. Pick only your assigned task in [docs/SPRINT2_TASKS.md](docs/SPRINT2_TASKS.md).

2. Check out the pre-created branch listed for you.

3. Move the matching issue to **In Progress** and update remaining effort.

4. Commit small, buildable changes; do not commit `bin/` or `obj/`.

5. Run `./scripts/verify.sh` and complete manual acceptance checks before opening a PR.

6. Use the PR template and request the assigned reviewer; merge only after review and green CI.

7. Move the issue to **Done** and set remaining effort to zero.

See [CONTRIBUTING.md](CONTRIBUTING.md) for exact Git commands and review rules.

### Codex consistency

Every Codex session is governed by the repository-level [AGENTS.md](AGENTS.md). It freezes shared contracts, assigns file ownership, prohibits cross-feature rewrites, and defines one verification command. Each branch also has a focused prompt under `.codex/tasks/`. Do not ask Codex to “finish Sprint 2”; use only the task prompt matching the current branch.

Recommended first prompt for every teammate:

```text
Read AGENTS.md, docs/ARCHITECTURE_CONTRACT.md, and the task file matching this branch. Implement only that assignment, stay inside owned paths, run ./scripts/verify.sh, and summarize any integration needs.
```

## Known limitations

- The player uses the existing original CC0 atlas. The ten new roster objects use original procedural pixel motifs; the twelve older gallery sprites now also use simple original motifs, including two-frame enemy/item details. See [new roster art provenance](docs/assets/STARLIGHT_RUINS.md) and [baseline gallery art and manual checks](docs/assets/SIMPLE_GALLERY_ART.md). This is not final visual acceptance.

- Object galleries demonstrate behaviors independently. Collision, room transitions, inventory UI, audio, and a complete dungeon are outside this Sprint 2 functionality demonstration.

- Enemy movement and item animation are deterministic; Octorok projectiles are demonstrations and do not interact with other objects during Sprint 2.

- Numbers `1`/`2`/`3` demonstrate an arrow, boomerang, and bomb using original procedural sprites. They preserve the existing player poses; a dedicated item-use pose/state and inventory/ammunition rules are not implemented. This is not a declaration that the complete target-dungeon object roster is finished. Simple recognizable gallery motifs and two-frame details are now provided; final in-game visual acceptance remains to be performed. See [items/blocks notes](docs/ITEMS_BLOCKS.md).

- Menu and HUD text use a small built-in pixel alphabet. It supports English letters, digits, and the punctuation used by the controls; it is not a general-purpose localized font.

## Documentation

- [Sprint 2 requirements checklist](docs/SPRINT2_CHECKLIST.md)

- [Five-person task plan](docs/SPRINT2_TASKS.md)

- [Architecture and integration contract](docs/ARCHITECTURE_CONTRACT.md)

- [Code review template](docs/code-reviews/TEMPLATE.md)

- [Sprint reflection template](docs/SPRINT2_REFLECTION_TEMPLATE.md)

- [Input/menu/HUD acceptance and release checklist](docs/INPUT_QUALITY_CHECKLIST.md)

## Current verification

Run the integrated verification command from the repository root before submitting:

```sh
bash scripts/verify.sh
```

The verification script checks repository hygiene, formatting, analyzers, the Release build, and every submitted headless feature suite: player states, player items (arrows/boomerangs/bombs), sprites, enemies/NPCs/projectiles, input/menu behavior, and items/blocks/gallery reset.

GitHub Actions runs the same `./scripts/verify.sh` command for pull requests targeting `main` and pushes to `main`. Actual keyboard bindings and visual layout still require the manual checks in the acceptance checklists.
