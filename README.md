# CSE 3902 — Sprint 3

Team: 6

## 1. Project Overview

Starlight Ruins is a C# and MonoGame project. Sprint 3 adds data-driven
dungeon rooms, room navigation, and complete collision handling to the
working Sprint 2 object demonstration.

The Sprint 3 starting baseline is a functionality demonstration with a controllable
player and separate galleries for blocks, items, and enemies/NPCs.
It demonstrates object drawing, movement, animation, player state changes,
keyboard input, and reset behavior.

The project uses an original visual theme. The current roster contains
6 block types, 9 item previews, and 7 enemy/NPC types. These counts describe
our implementation; they are not numeric requirements from the assignment.

### Sprint 3 status

Planning and task setup are complete. Room loading, room navigation, pickups,
and object-to-object collisions are assigned work and must not be described as
implemented until their feature PRs merge. The exact requirements, check-in
gate, architecture, collision matrix, and individual assignments are linked in
[Supporting Documentation](#9-supporting-documentation).

The `sprint2-final` tag is the verified Sprint 2 baseline. Sprint 3 work is
integrated through `integration/sprint3`; contributors do not push feature
work directly to `main`.

## 2. Team assignments

The five Sprint 3 assignments each start at 12 effort points and an estimated
15 hours. Update Remaining Hours on the GitHub project after every work session.

| Member | Contributions |
| --- | --- |
| Hank Zhang (@hankkyy) | Collision contracts/detection/dispatch and player-solid collision. Branch: `feature/s3-collision-core`. |
| Xuanzhe Li (@JojoLi132) | External room schema/loader, five rooms, and all-object test room. Branch: `feature/s3-room-loading`. |
| Michael Xu (@mo46-123) | Enemy health, combat collision responses, and projectile/environment interaction. Branch: `feature/s3-combat-collisions`. |
| Leo Zhuang (@Lzzz-7) | Item pickup/inventory, push blocks, locked doors, persistence, and reset. Branch: `feature/s3-items-blocks-collisions`. |
| Ashley Zhang (@xing-gif) | RoomManager, mouse navigation, HUD, integration, and strict-analysis records. Branch: `feature/s3-room-navigation-integration`. |

## 3. Requirements and Running

### Requirements

- .NET 8 SDK
- A graphical environment supported by MonoGame DesktopGL
- Internet access for the initial NuGet package restore

The project targets `net8.0` and references
`MonoGame.Framework.DesktopGL` version `3.8.5.1`.

### Run from the project root

```sh
dotnet restore GameProject/GameProject.csproj
dotnet build GameProject/GameProject.csproj
dotnet run --project GameProject/GameProject.csproj
```

Visual Studio users can open `GameProject/GameProject.csproj`.

The application starts at a menu. Press Enter to begin the demonstration.

## 4. Controls

| Key | Action |
| --- | --- |
| Enter | Start gameplay from the menu |
| Arrow keys / W, A, S, D | Move the player and change facing direction |
| Z / N | Perform a sword attack |
| 1 | Select and fire an arrow |
| 2 | Select and throw a returning boomerang |
| 3 | Select and place a timed bomb |
| E | Trigger the player's damaged state |
| T / Y | Show the previous / next block |
| U / I | Show the previous / next item preview |
| O / P | Show the previous / next enemy or NPC |
| R | Reset gameplay objects and gallery selections |
| Q / Escape | Quit from either the menu or gameplay |
| Mouse left/right | Sprint 3 room navigation after the navigation PR merges |

Letter controls work regardless of letter case.

Movement responds while a movement key is held. Attacks, item use, damage,
gallery changes, and reset trigger on a new key press; release and press
the key again to repeat an action.

Use the number keys above the letter keys for items.

During gameplay, R restores the player's initial position, health, action,
and selected item; resets gallery selections and object timers; and clears
active player item effects and enemy shots. It keeps the application in
gameplay rather than returning to the start menu.

## 5. Implemented Features

### Player

- Four-direction movement and facing.
- Idle, walking, attacking, and damaged states.
- Directional sprite animations.
- Sword attack and damage feedback.
- Arrow, returning boomerang, and timed bomb demonstrations.
- HUD indicators for health, selected item, action, and direction.

### Blocks

The block gallery includes:

- Stone Block
- Push Block
- Water Tile
- Statue
- Crystal Pillar
- Rune Tile

Blocks remain stationary and do not interact with other objects.
T and Y cycle through the gallery with wraparound.

### Items

The item gallery includes:

- Heart
- Rupee
- Key
- Bomb
- Bow
- Boomerang
- Ruins Map
- Star Compass
- Star Shard

Item previews use time-based visual animation and do not collect, equip,
or activate items. U and I cycle through the gallery with wraparound.

The item gallery is separate from the usable player items on keys 1–3.

### Enemies and NPCs

| Character | Demonstrated behavior |
| --- | --- |
| Octorok | Horizontal patrol and periodic projectiles |
| Keese | Figure-eight flight |
| Gel | Alternating rest and hop behavior |
| Lantern Keeper | Stationary NPC |
| Rune Wisp | Elliptical floating movement |
| Clockwork Beetle | Rectangular patrol with direction changes |
| Prism Sentinel | Charging and alternating left/right light shots |

O and P cycle through the gallery with wraparound.
The selected character appears in both the preview and the play area.

### Menu and HUD

- Start menu with Enter to begin.
- Gameplay input is disabled while the menu is active.
- On-screen player status, gallery names, selection indexes, and controls.

## 6. Sprint 3 Scope

The final Sprint 3 build must load each room's objects from external data,
support normal and mouse-driven room transitions, include an artificial room
with every first-dungeon object type, and implement the responses listed in
`docs/COLLISION_MATRIX.md`. Sound, smooth camera transitions, and gamepad
support are not required. Until feature PRs merge, the baseline still has the
Sprint 2 limitations described above.

## 7. Design and Organization

- **Command pattern:** keyboard actions are represented by command classes.
- **State pattern:** player behavior is separated into idle, walking,
  attacking, and damaged states.
- **Sprite factory:** sprite creation is centralized in `SpriteFactory`.
- **Object/sprite separation:** game objects own behavior and state;
  sprites handle visual representation.
- **Time-based updates:** movement and animation use elapsed game time.

```text
GameProject/
├── Commands/       Input action commands
├── Controllers/    Keyboard mapping and press detection
├── Core/           Shared interfaces and object galleries
├── Objects/        Player, items, blocks, enemies, and behaviors
├── Sprites/        Sprite creation, drawing, and animation
├── States/         Menu and gameplay session state
├── UI/             Menu, HUD, and control instructions
├── Game1.cs        System setup and game loop
└── Program.cs      Application entry point
```

## 8. Tools and Verification

### Tools and processes

The repository provides:

- Git and GitHub collaboration conventions.
- .NET/Roslyn analyzers enabled through `Directory.Build.props`.
- `dotnet format` checks.
- A shared verification script.
- A separate strict-analysis script and warning baseline for early cleanup.
- Six automated test projects that run without a game window.
- A GitHub Actions workflow for pushes to `main` and pull requests
  targeting `main`.

A push to the integration branch alone does not trigger that workflow.

### Automated verification

From the project root, using Bash:

```sh
bash scripts/verify.sh
./scripts/analyze-strict.sh
```

On Windows, use a Bash environment such as Git Bash for this script.

The script restores dependencies, checks formatting, builds the game in
Release configuration with analyzers enabled, and runs tests for:

1. Player states.
2. Player items.
3. Player sprites.
4. Enemies and NPCs.
5. Input and menu behavior.
6. Items, blocks, and gallery reset.

When run inside a Git working tree, it also performs repository hygiene
and whitespace checks.

### Manual demonstration checklist

- Start the program and enter gameplay.
- Demonstrate movement and animation in all four directions.
- Demonstrate sword attack and damage feedback.
- Use the arrow, boomerang, and bomb.
- Cycle every gallery forward and backward, including wraparound.
- Observe enemy movement, projectiles, and item animation.
- Change selections and activate effects, then verify reset.
- Verify quitting from both the menu and gameplay.

## 9. Supporting Documentation

Existing project guides include:

- [Sprint 3 instructor requirements and team interpretation](docs/SPRINT3_REQUIREMENTS.md)
- [Sprint 3 five-person task plan](docs/SPRINT3_TASKS.md)
- [Sprint 3 functionality and process checklist](docs/SPRINT3_CHECKLIST.md)
- [Sprint 3 architecture contract](docs/SPRINT3_ARCHITECTURE.md)
- [Required collision matrix](docs/COLLISION_MATRIX.md)
- [Sprint 3 review record](docs/code-reviews/SPRINT3_REVIEWS.md)
- [Sprint 3 reflection template](docs/SPRINT3_REFLECTION_TEMPLATE.md)
- [Object roster and planned roles](docs/STARLIGHT_RUINS_ROSTER.md)
- [Sprint 2 checklist](docs/SPRINT2_CHECKLIST.md)
- [Arrow behavior and checks](docs/PLAYER_ARROW.md)
- [Boomerang and bomb behavior](docs/PLAYER_SECONDARY_ITEMS.md)
- [Enemy behavior and checks](docs/ENEMIES_NPCS.md)
- [Items and blocks](docs/ITEMS_BLOCKS.md)
- [Development and review conventions](CONTRIBUTING.md)

Project records:

- [Code quality analysis](docs/CODE_ANALYSIS.md)
- [Sprint 2 code review records](docs/code-reviews/SPRINT2_REVIEWS.md)
- [Sprint 2 reflection](docs/SPRINT2_REFLECTION.md)
- [Project task board](https://github.com/users/hankkyy/projects/4)

## 10. Game Artwork

We use the Moss Scout sprite sheet for the player.
Other objects and item effects use simple pixel art drawn through code.

More details about the artwork and licenses:

- [Player sprites](docs/assets/PLAYER_SPRITES.md)
- [Starlight Ruins artwork](docs/assets/STARLIGHT_RUINS.md)
- [Basic gallery artwork](docs/assets/SIMPLE_GALLERY_ART.md)
