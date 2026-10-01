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

## Manual acceptance results — 2026-10-01

The items/blocks owner (`@Lzzz-7`) reported completing manual acceptance with
no issues. These are user-reported results, not an automated UI test.

| Check | Result |
|---|---|
| T/Y select the previous/next block | Passed |
| U/I select the previous/next item | Passed |
| Both galleries wrap from first to last and last to first | Passed |
| Blocks remain stationary and non-interacting | Passed |
| All four items display the in-place placeholder animation | Passed |
| R restores the initial gallery selections and restarts the preview | Passed |

The owner observed approximately two to three flashes per second, consistent
with the placeholder animation. Exact animation-phase reset and resetting hidden
items are covered by the headless tests; manual observation does not measure
those timings precisely.

When this record was added, the checked-out branch was `feature/items-blocks`
at `6276325016f67c6ada8be5a02f79e9baa4203267`. This identifies the documentation
baseline; the exact commit used for the owner's manual run was not separately
captured.

This acceptance covers the requested functionality-only version. Final artwork
and reference-game visual fidelity remain outside this pass. No screenshot/GIF
is attached to this record yet. Request reviewer `@xing-gif` for the PR; this
manual acceptance record does not constitute code-review approval.
