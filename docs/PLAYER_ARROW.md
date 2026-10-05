# Player arrow — first item-use step

This incremental change addresses the missing player projectile demonstration. It does not complete all Sprint 2 item, object-roster, or artwork requirements.

## Behavior

- In gameplay, the top-row `1` key selects slot one and fires an arrow from just outside Link, in his current facing direction.
- One arrow can be active at a time. It flies at 320 pixels per second and disappears after 0.8 seconds or when its 16-by-16 footprint leaves the player area. Firing out of the area from the edge may produce no visible arrow; demonstrate from the middle of the area.
- Holding `1` does not repeatedly fire. Release and press it again after the arrow disappears.
- A fired arrow keeps its original direction even if Link turns. It does not hit enemies, blocks, or items; no collision or damage is added.
- New arrows cannot be fired during sword attacks or damage. Already-fired arrows continue while the player moves, attacks, or flashes during damage.
- `R` clears the arrow and restores the player as before. The menu blocks firing; a fire key held across Enter must be released before it works in gameplay.
- `2` now throws a boomerang and `3` places a bomb; see [secondary items](PLAYER_SECONDARY_ITEMS.md). The existing player animation is preserved; there is no dedicated item-use pose or item-use state yet.

## Code path

`KeyboardController` → `FireArrowCommand` → `Player.TryFireArrow()` → `ArrowProjectile.TryLaunch()`.

`Player.Update()` advances the projectile with elapsed game time. `Player.Draw()` draws it through `ISprite`, including frames where the player is invisible due to damage flashing. `ArrowSprite` draws an original procedural arrow using the existing white pixel texture; there is no new asset or package. Existing constructors and interfaces remain available.

## Automated verification

```sh
dotnet run --project tests/PlayerItems/PlayerItems.Tests.csproj --configuration Release
bash scripts/verify.sh
```

The new headless suite covers four-direction spawning/travel, independence from later player turns, elapsed-time movement, lifetime, all area boundaries, reset, drawing without state changes, attack/damage guards, and the actual number-key/menu/reset bindings. The standard script includes this sixth suite.

## Manual verification — record actual results after running

1. Start from the menu. Hold `1`, press Enter, and keep holding `1`: no arrow should fire. Release and press `1`: an arrow should appear.
2. From the middle of the green area, tap each direction and then `1`. Observe the arrow start in front of Link and fly up/down/left/right. Wait for it to disappear between shots.
3. Hold `1` for two seconds. Only one shot should occur. Release and press again to get the next shot.
4. Fire and immediately turn/move: Link changes direction but the arrow keeps its launch direction. Fire, then press `E`: Link may flash while the arrow continues.
5. Fire and immediately press `R`: the arrow disappears and the initial player state returns. Check `Z`/`N`, `E`, `2`/`3`, and the existing gallery controls still work.
6. Face outwards at each edge and press `1`: the arrow must not remain outside the green player area or cross the HUD. From the middle, confirm an arrow overlapping a previewed enemy does not damage it.
7. Check that `1 FIRE` and `ITEM 1 ARROW` are legible and the bottom controls fit. Quit with `Q` and Escape in separate runs.

Headless tests cannot certify visual appearance. Attach a screenshot or short recording of the actual arrow when recording manual acceptance.
