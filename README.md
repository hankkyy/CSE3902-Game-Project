# CSE 3902 Team 6 — Sprint 3 Check-In

This check-in build loads six rooms from JSON files. It includes a room with every current object type, normal doorway transitions, mouse room switching, reset, and a room HUD.

## Run the game

Install the .NET 8 SDK, open a terminal in this folder, and run:

```sh
dotnet restore GameProject/GameProject.csproj
dotnet run --project GameProject/GameProject.csproj
```

The game opens at a start screen. Press Enter to begin.

## Controls

| Input | Action |
| --- | --- |
| Enter | Start the game |
| Arrow keys or W/A/S/D | Move |
| Z or N | Sword attack |
| 1 | Fire an arrow |
| 2 | Throw the boomerang |
| 3 | Place a bomb |
| Left mouse button | Previous room |
| Right mouse button | Next room |
| R | Reset |
| Q or Escape | Quit |

## Check-in limits

This version is for the Sprint 3 functionality check-in. Room loading and navigation are working. Full collision and inventory integration will be completed for the final Sprint 3 submission.
