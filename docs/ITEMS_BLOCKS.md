# Sprint 2 items and blocks

The block gallery now delegates to dedicated `StoneBlock`, `PushBlock`, `WaterTile`, and `StatueBlock` classes. All four remain stationary and non-interacting. The item gallery delegates to dedicated `HeartItem`, `RupeeItem`, `KeyItem`, and `BombItem` classes. Each item uses `GameTime` for a recognizable movement and frame rhythm, and `Reset()` returns the animation to its constructor-defined origin.

## Verification

```sh
./scripts/verify.sh
dotnet run --project tests/ItemsBlocks/ItemsBlocks.Tests.csproj --configuration Release
```

The headless checks cover stationary blocks, all four moving items, deterministic reset, and gallery wraparound.

## Manual acceptance

1. Start the game and press `T` and `Y` through all four blocks in both directions. Confirm every block remains stationary and the gallery wraps.
2. Press `U` and `I` through Heart, Rupee, Key, and Bomb. Leave each visible long enough to observe its distinct hover, sway, hop, or frame rhythm.
3. Change both galleries, then press `R`. Confirm the first block and item are selected and the item's animation restarts from its initial position.
4. Confirm none of the displayed blocks or items interacts with the player or other gallery objects.
