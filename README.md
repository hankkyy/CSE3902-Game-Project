# CSE 3902 Game Project — Sprint 2

A MonoGame functionality gallery for Sprint 2 (Game Objects and Sprites). The current build is deliberately asset-free: colored placeholder sprites make every required state visible while the team works in parallel on final art and object behavior.

## Functionality check-in status

The starter currently provides:

- player movement with arrow keys or `WASD`, four facing directions, and walking animation;
- sword attack with `Z` or `N`, damage state with `E`, and item selection with `1`–`3`;
- stationary block gallery cycled with `T` / `Y`;
- animated item gallery cycled with `U` / `I`;
- independently moving/animated enemy and NPC gallery cycled with `O` / `P`;
- reset with `R`, quit with `Q` or `Escape`;
- Command, Factory Method, and object/sprite interfaces to separate concerns.

This is a functional foundation, not the final dungeon. Placeholder visuals and simplified motion are explicitly intended to be replaced by the five assigned feature branches in [the Sprint 2 task plan](docs/SPRINT2_TASKS.md).

## Requirements and setup

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- Windows, macOS, or Linux graphics environment supported by MonoGame DesktopGL
- Git

From the repository root:

```sh
dotnet restore GameProject/GameProject.csproj
dotnet build GameProject/GameProject.csproj
dotnet run --project GameProject/GameProject.csproj
```

Visual Studio users can open `GameProject/GameProject.csproj` directly. VS Code/Rider users can open the repository root and run the same commands.

## Controls

| Keys | Action |
|---|---|
| Arrow keys / `WASD` | Move Link and change facing direction |
| `Z` / `N` | Sword attack |
| `1`, `2`, `3` | Select secondary item slot |
| `E` | Damage Link |
| `T` / `Y` | Previous / next block |
| `U` / `I` | Previous / next item |
| `O` / `P` | Previous / next enemy or NPC |
| `R` | Reset all objects |
| `Q` / `Escape` | Quit |

The top red bar is player health. The gold bar represents the selected item slot. Dots in the right panel show the selected block, item, and enemy.

## Architecture

```text
GameProject/
├── Commands/       Command interface and keyboard actions
├── Controllers/    Input mapping and key-edge detection
├── Core/           Shared object contracts, direction, gallery
├── Objects/        Player, blocks, items, enemies/NPCs
├── Sprites/        Drawing contract, placeholders, sprite factory
├── Game1.cs        Composition root and game loop
└── Program.cs      Application entry point
```

`Game1` composes the systems. `KeyboardController` translates input into commands. Objects own behavior and state but draw through `ISprite`; `SpriteFactory` owns visual construction. This lets art, player state, enemies, items, and controls evolve in separate branches with fewer merge conflicts.

## Team workflow

1. Pick only your assigned task in [docs/SPRINT2_TASKS.md](docs/SPRINT2_TASKS.md).
2. Create the listed branch from an updated `main`.
3. Move the matching issue to **In Progress** and update remaining effort.
4. Commit small, buildable changes. Do not commit `bin/` or `obj/`.
5. Before opening a PR, run `dotnet build GameProject/GameProject.csproj` and complete the manual acceptance checks.
6. Open a PR using the template; request the assigned reviewer. Merge only after review and green CI.
7. Move the issue to **Done** and set remaining effort to zero.

See [CONTRIBUTING.md](CONTRIBUTING.md) for exact Git commands and review rules.

## Known limitations

- Sprites are geometric placeholders; no copyrighted game assets are committed.
- Object galleries demonstrate behaviors independently. Collision, room transitions, inventory UI, audio, and a complete dungeon are outside this check-in scaffold.
- Enemy motion is deterministic and projectiles are planned in the enemy task.
- There is no on-screen font yet; the README is the control reference.

## Documentation

- [Sprint 2 requirements checklist](docs/SPRINT2_CHECKLIST.md)
- [Five-person task plan](docs/SPRINT2_TASKS.md)
- [Code review template](docs/code-reviews/TEMPLATE.md)
- [Sprint reflection template](docs/SPRINT2_REFLECTION_TEMPLATE.md)

## Current verification

`dotnet restore` and `dotnet build` succeed with zero warnings and zero errors. GitHub Actions repeats the build on every pull request and push to `main`.
