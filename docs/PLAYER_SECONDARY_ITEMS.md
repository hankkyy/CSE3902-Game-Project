# Player item use: 2 = boomerang, 3 = bomb

This change adds two usable items alongside the arrow on `1`. It addresses the Sprint 2 number-key item-use requirement; it does not claim that every final-submission requirement or every target-dungeon object is complete.

## Controls and behavior

- `2` selects and throws a boomerang in the player's current facing direction. It travels up to 140 pixels at 240 pixels/second, turns sooner at the play-area edge, and returns to the player's current position at 340 pixels/second. It can follow a moving player. A four-second lifetime prevents a stranded effect.
- `3` selects and places a bomb just in front of the player. The bomb stays at its placement position, flashes during a 1.2-second fuse, shows a two-frame explosion for 0.35 seconds, then disappears. The explosion footprint is kept inside the play area, including near its edges.
- One effect of each type can exist at a time. Pressing again while that effect is active does not replace it. Release and press again after it ends; holding a key does not repeat.
- Menu input gating and held-key handling apply to all three items. Sword attack and damage states block new item uses; existing effects continue, even while the player flashes.
- `R` clears all arrows, boomerangs, bombs, and explosions, as well as the existing player/gallery state.
- These objects do not hit, damage, or collect other objects. A boomerang disappearing at its owner's return position is its own lifecycle, not an enemy/item collision system.
- Placement requires room for the 16-by-16 item in front of the player. Facing directly out of the area while standing at its edge may reject use; demonstrate from the middle first.

## Architecture

`KeyboardController` maps `D2` to `ThrowBoomerangCommand` and `D3` to `PlaceBombCommand`. The commands invoke player-owned methods. `Player` owns the effects and calls their `Update`, `Draw`, and `Reset` methods. `BoomerangProjectile` and `PlacedBomb` implement the existing `IGameObject` interface and draw through `ISprite`.

The return-position callback lets the boomerang ask for the player's current location without referencing the player class. Its update splits a frame across the outward/return transition so elapsed time is not lost at the turn point. The bomb's fuse and explosion are based on total elapsed game time; drawing never advances either timer.

`SpriteFactory` creates original procedural boomerang, bomb, and explosion sprites from the existing white pixel texture. No external image, dependency, or frozen interface is added. Original three- and four-argument injected-sprite player constructors remain available; the game uses the existing factory constructor, which supplies all item sprites.

## Verification

```sh
dotnet run --project tests/PlayerItems/PlayerItems.Tests.csproj --configuration Release
bash scripts/verify.sh
```

The player-item suite contains 59 arrow checks and 113 boomerang/bomb checks. The latter cover four-direction travel and placement, return to a moving target, turn-point timing, boundaries, fuse/explosion phases, reset, pure drawing, state guards, and actual keyboard/menu bindings. The existing verification script already includes this suite.

Manual acceptance (record results after actually performing these steps):

1. From the middle of the green area, face each of the four directions and tap `2`. Verify visible movement outward, a turning animation, return, and disappearance. Walk while it returns and confirm it follows you.
2. In each direction, press `3`, then walk away. Verify a bomb stays in front of the original position, flashes, explodes after about 1.2 seconds, and disappears. It must not damage the player or other objects.
3. Hold `2` or `3` longer than the effect duration. Only the first effect should occur. Release and press again to use it again.
4. Fire/throw/place all three items, then press `R`. Verify all effects clear and the initial player state returns. Also reset during the bomb's visible explosion.
5. In a fresh menu run, hold `2` or `3`, press Enter, and continue holding. No effect should appear until the item key is released and pressed again.
6. Check sword attack, damage, arrow use, gallery keys, and Q/Escape still work. During damage, a newly pressed item key should not spawn an effect, while an already active effect continues.
7. Near area edges, ensure the boomerang returns without entering the HUD and bomb explosions stay within the area. Confirm HUD labels match the selected item and the bottom help is readable.

## Remaining scope

The existing player poses are preserved; a dedicated item-use animation/state is not added here. Inventory/ammunition rules, collisions, enemy damage, final reference-game art, and expansion of the target-dungeon object roster are separate work. The right-side item gallery is also separate: pressing `2`/`3` uses player items but does not add new gallery entries.
