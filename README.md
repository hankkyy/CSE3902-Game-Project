# CSE 3902 — Sprint 2

Team: 6

## 1. Project Overview

Starlight Ruins is a C# and MonoGame project for Sprint 2:
Game Objects and Sprites.

The current program is a functionality demonstration with a controllable
player and separate galleries for blocks, items, and enemies/NPCs.
It demonstrates object drawing, movement, animation, player state changes,
keyboard input, and reset behavior.

The project uses an original visual theme. The current roster contains
6 block types, 9 item previews, and 7 enemy/NPC types. These counts describe
our implementation; they are not numeric requirements from the assignment.

## 2. Team Contributions

Our team divided the main tasks as follows. We also updated the project
documentation as the implementation changed.

| Member | Contributions |
| --- | --- |
| Hank Zhang (@hankkyy) | Worked on player movement and the idle, walking, attacking, and damaged states. Added state transitions, damage feedback, reset behavior, and player-state tests. |
| Xuanzhe Li (@JojoLi132) | Created the player sprite sheet and animations for all four directions. Worked on animation timing, sprite loading, sprite tests, and asset documentation. |
| Michael Xu (@mo46-123) | Implemented the Octorok, Keese, Gel, and NPC behaviors. Added enemy movement, animations, projectile behavior, reset handling, and enemy tests. |
| Leo Zhuang (@Lzzz-7) | Implemented the basic block and item classes. Added item preview animations and tests for gallery cycling, wraparound, and reset behavior. |
| Ashley Zhang (@xing-gif) | Worked on keyboard input, command classes, the start menu, and the HUD. Added input/menu tests and maintained the verification workflow. Also reorganized the README and updated the controls, feature descriptions, limitations, and team contribution information. |

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

## 6. Sprint 2 Scope

This build focuses on object movement, sprite animation, and state changes.
As required for Sprint 2, blocks remain stationary, while items and
enemies demonstrate their individual behaviors without interacting
with other objects.

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
- Six automated test projects that run without a game window.
- A GitHub Actions workflow for pushes to `main` and pull requests
  targeting `main`.

A push to the integration branch alone does not trigger that workflow.

### Automated verification

From the project root, using Bash:

```sh
bash scripts/verify.sh
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

## 10. Assets and Attribution

The player uses the original Moss Scout atlas. Gallery objects and player
item effects use original procedural pixel artwork.

Asset provenance and applicable license information are documented in:

- [Player sprite attribution](docs/assets/PLAYER_SPRITES.md)
- [Starlight Ruins artwork](docs/assets/STARLIGHT_RUINS.md)
- [Baseline gallery artwork](docs/assets/SIMPLE_GALLERY_ART.md)

These asset-specific statements do not relicense the entire repository
or its dependencies.
