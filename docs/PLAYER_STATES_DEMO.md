# Player state test notes

Run the game with:

```sh
dotnet run --project GameProject/GameProject.csproj
```

The non-graphics checks can be run separately:

```sh
dotnet run --project tests/PlayerStates/PlayerStates.Tests.csproj --configuration Release
```

Check the following controls:

- Arrow keys and `WASD`: Link moves, faces the direction of travel, and stays inside the play area.
- `Z` and `N`: Link attacks in the current direction. Movement is paused until the attack ends.
- `E`: Link loses one health point and flashes for a short time. Pressing `E` again during the flash should not remove more health.
- `1`, `2`, and `3`: the selected item number changes.
- `R`: position, direction, action, health, selected item, and animation return to their starting values.

Also test reset once during an attack and once during the damaged state.
