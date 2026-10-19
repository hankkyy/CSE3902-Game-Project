# Sprint 3 check-in: pickup module

Owner: Leo Zhuang (@Lzzz-7). Reviewer: Ashley Zhang (@xing-gif).
Issue #24; branch `feature/s3-items-blocks-collisions`; target PR branch
`integration/sprint3`. Check-in functionality only; pushing and key doors are deferred.

## Status and scope

The pickup response, inventory state, stable-ID persistence, deferred removal
queue, and reset are implemented and tested independently. This is **not yet a
playable room integration**. At implementation time the remote integration branch
was c2ae73d and did not contain the collision or room APIs. Those APIs exist only
on teammates' feature branches. No teammate-owned code or frozen contracts were
changed, and none of those branches were merged into this feature branch.

## Small public surface

- `InventoryState`: read-only resource counters and unlock flags. No duplicate
  health field; health remains owned by Player.
- `PickupId`: room ID plus stable object ID from room data. Object identity survives
  reconstruction. The same object ID in a different room is a different pickup.
- `ItemPickupHandler`: owns one run's collected IDs and pending removals. Calls
  `TryCollect(id, kind)` after a confirmed player/item contact. The first call
  awards the effect; repeated calls for that ID return false. Run one handler
  across all rooms, not one handler per room or one handler per collision.

No ItemObject replacement, extra object wrapper, new packages, collision detector,
input polling, or drawing behavior was added. The existing nine-value ItemKind
is reused. Effects are explicit if/else statements.

| Kind | Current effect |
|---|---|
| Heart | Ask the player-owned healing callback to restore one health, capped by the player |
| Rupee | Add one rupee |
| Key | Add one key |
| Bomb | Add one bomb to inventory |
| Bow / Boomerang | Set the corresponding weapon unlock flag |
| Map / Compass | Set the corresponding dungeon navigation flag |
| StarShard | Add one shard; HasObjective becomes true |

These are initial effect policies, not a claim of final game balance. A heart is
consumed even at full health; another copy of a permanent unlock is also consumed.
Inventory weapon flags are represented but are not yet enforced by the player's
existing item-use controls. Map/compass flags currently apply to the one dungeon.

## Frame and room lifecycle

1. Detect contacts using the team's collision system.
2. For a confirmed player/item pair, obtain the room ID, object ID, and item kind
   from room metadata and call `TryCollect`.
3. `IsCollected(id)` immediately becomes true, so later contacts cannot award a
   second effect, even though the room collection is unchanged during enumeration.
4. After collision enumeration ends, call `ApplyPendingRemovals(removeFromRoom)`.
   The callback must address the room carried in PickupId, not blindly use the
   currently selected room. Drain before room transitions and drawing.
5. On re-entry, preserve this handler. When reconstructing objects, filter already
   collected IDs with `IsCollected` so objects do not visually respawn.
6. Full reset calls `handler.Reset()` and separately resets the player and restores
   room objects from their initial data. Reset must not run during queue draining.

Room removal callbacks must be idempotent and must not re-enter the handler.
If a removal callback throws, that entry stays queued; retry does not re-award
inventory effects. The healing callback must clamp health and complete without
re-entering pickup processing; it must not mutate health and then throw.

## Integration gaps requiring owner coordination

- Collision owner: expose a stable item ID/kind with the pickup collider and
  register a player/item response that calls this handler. The handler does not
  implement the currently unmerged ICollisionHandler contract yet.
- Player/combat owner: provide an additive `RestoreHealth(int amount)` (or an
  equivalent approved operation) clamped to max health. Current Player only has
  private-set Health and TakeDamage; do not use reflection or create another
  health counter. Pass the approved method as the healing callback.
- Room owner: provide removal/deactivation keyed by stable ID. Current LoadedRoom
  only exposes a read-only dictionary and updates/draws every member. Its current
  Reset only resets surviving instances, so physically removed items would not
  return without reconstruction. Removal and initial-state reconstruction must
  be integrated together.
- Navigation owner: retain one handler and inventory across rooms, drain removals
  after collisions, filter restored rooms, and include this module in global reset.
- HUD owner: read inventory properties. Player-item use gating, door consumption,
  push behavior, and victory transitions are not added by this check-in module.
- Integrator: add the new test project to the shared verification script once
  reviewed. For now run the explicit command below; shared scripts were untouched.

## Verification

```sh
./scripts/verify.sh
dotnet run --project tests/Inventory/Inventory.Tests.csproj --configuration Release
./scripts/analyze-strict.sh
```

Tests cover all nine effects, repeated contacts, deferral while enumerating a real
collection, stable-ID re-entry, different rooms, ambiguous concatenation IDs,
full-health policy, duplicate unlocks, invalid requests, failed removal/retry,
failed healing, reset before queue draining, and collection again in a new run.

Manual end-to-end acceptance remains pending integration: walk onto each pickup,
verify effects and disappearance, leave/re-enter, verify no duplicate award, then
reset and confirm initial inventory/health and restored room items. A successful
headless test is not evidence that the current Game1 is wired to this module.

## Strict analysis

The strict build reports CA1515 for each of the three new public types:

- InventoryState: public so the separate Inventory.Tests assembly can inspect state.
- ItemPickupHandler: public for headless tests and the planned collision/navigation wiring.
- PickupId: public because it is part of the handler's public API and external tests.

These three diagnostics are deliberately retained and individually documented;
no suppressions are introduced. Changing the types to internal would require a
shared-project InternalsVisibleTo change. Other diagnostics are outside this
module. Environment-only NU1900 warnings can occur when NuGet's HTTP cache is
not writable; these are not C# analyzer diagnostics.

## Verification record — 2026-10-19

- Baseline and final `./scripts/verify.sh`: passed, including the six existing
  headless suites. Baseline produced an environment-only NuGet cache warning.
- New Inventory.Tests: 50 checks passed; its formatting check passed.
- `./scripts/analyze-strict.sh`: completed, zero errors; 112 warnings overall.
  The only warnings in new production files are the three CA1515 entries above.
- `git diff --check`: passed. Only feature-owned additions and its proposal/docs/tests.
- NuGet HTTP cache was redirected to a writable temporary directory for the
  final verification; no repository package configuration was changed.
- In-game pickup acceptance, interface approval, review, and shared test-script
  integration remain pending. No completion of those steps is asserted here.
