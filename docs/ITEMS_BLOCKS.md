# Items and blocks: Starlight Ruins roster

The existing BlockObject and ItemObject constructors remain compatible. They delegate to dedicated classes; shared stationary behavior remains in BlockEntity and elapsed-time preview animation in ItemEntity. The complete planned list and future room roles are in [STARLIGHT_RUINS_ROSTER.md](STARLIGHT_RUINS_ROSTER.md).

## Current gallery

- Six blocks: Stone, Push, Water, Statue, Crystal Pillar, Rune Tile.
- Nine items: Heart, Rupee, Key, Bomb, Bow, Boomerang, Ruins Map, Star Compass, Star Shard.
- All blocks stay stationary, including Push Block. Item previews animate in place and do not collect, equip, collide, or trigger puzzles.
- The new block/item kinds have original small motifs in RuinsSprite. Older sprites are still placeholders; their final visual fidelity is not claimed complete.
- Only the visible gallery entry updates. R resets index zero and all entries, including hidden animation clocks.
- Gallery previews are separate from player item use. The player can now use arrow/boomerang/bomb on 1/2/3; see [PLAYER_SECONDARY_ITEMS.md](PLAYER_SECONDARY_ITEMS.md).

## Automated checks

```sh
bash scripts/verify.sh
dotnet run --project tests/ItemsBlocks/ItemsBlocks.csproj --configuration Release
```

The test executable enumerates every kind and checks stationary placement, elapsed-time animation, pure drawing, repeated reset, bidirectional wraparound, and hidden-item reset. Its final line reports the actual counts rather than a hard-coded eight objects.

## Manual acceptance — still to perform on the expanded roster

1. Run the game and enter gameplay.
2. Press Y six times to return to Stone; T from Stone must reach Rune Tile. Check all blocks remain stationary.
3. Press I nine times to return to Heart; U from Heart must reach Star Shard. Inspect the five new item motifs and their preview animation.
4. Select later entries, wait, then press R. Revisit hidden items and confirm their animation restarts.
5. Check player 1/2/3 use remains independent of gallery selection, and existing movement, enemy controls, and quit still work.
6. Attach screenshots or a recording after actual visual checks. This document does not claim they have been completed.

The original feature/items-blocks branch requested reviewer @xing-gif; record review of the new changes separately rather than treating earlier review as approval of this expansion.
