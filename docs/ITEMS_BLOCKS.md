# Items and blocks: functionality implementation

The existing `BlockObject` and `ItemObject` constructors remain compatible with
Game1. They delegate to dedicated StoneBlock, PushBlock, WaterBlock, StatueBlock,
HeartItem, RupeeItem, KeyItem, and BombItem classes. Shared behavior lives in
BlockEntity and ItemEntity. ISprite injection allows tests without a graphics device.

All blocks remain stationary, including PushBlock. Items now animate in place
instead of using the scaffold's generic floating motion. The existing two-frame
placeholder animation runs from GameTime; it is a temporary preview, not a claim
that final NES item animation is complete. Final art and item-specific clips still
need coordination with the sprite owner. No textures/assets were added.

There is no collection, collision, pushing, item use, fuse, or explosion.
Reset restores the item animation phase; gallery reset restores index zero and
resets hidden objects too. Existing Game1/keyboard wiring is unchanged. Only the
visible object advances, matching the existing gallery lifecycle.

## Automated checks

Run from the repository root:

```sh
./scripts/verify.sh
dotnet run --project tests/ItemsBlocks/ItemsBlocks.csproj --configuration Release
```

The headless executable checks every kind, stationary positions, elapsed-time
animation, draw purity, repeatable reset, both-direction gallery wraparound, and
reset of hidden items. It uses the existing project dependency, no new packages.

## Manual acceptance

1. Run `dotnet run --project GameProject/GameProject.csproj`.
2. Press Y four times: Stone → Push → Water → Statue → Stone. Press T at Stone
   to reach Statue. Wait on each: it must not move or interact with the player.
3. Press I four times: Heart → Rupee → Key → Bomb → Heart. Press U at Heart
   to reach Bomb. Wait on each: its placeholder detail animates in place.
4. Select later entries, wait, then press R: Stone and Heart must be selected;
   revisit every item to confirm its animation restarts from the initial phase.
5. Repeat R and confirm no drift. Verify the player/enemy controls still work.

Visual acceptance and final recognizable artwork remain pending. Before a PR,
attach a screenshot/GIF, record manual results, and request reviewer @xing-gif.
