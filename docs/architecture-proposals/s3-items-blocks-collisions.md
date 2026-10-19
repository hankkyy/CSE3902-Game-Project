# Proposal: connect Sprint 3 pickup responses

Status: proposed, not approved or implemented in other owners' modules.
Owner: @Lzzz-7. Affected owners: @hankkyy (collision/player), @mo46-123
(player health coordination), @JojoLi132 (room loading), @xing-gif (navigation/reset).

## Problem

ItemPickupHandler implements effects, inventory, identity persistence and deferred
removal, but the integration/sprint3 baseline lacks the proposed collision/room
contracts. The inspected room branch exposes a read-only object dictionary;
Player has no healing operation. A running-game pickup cannot be honestly claimed
until these owners agree on the wiring.

## Minimal additive changes

1. Player owner provides `public void RestoreHealth(int amount)` (or equivalent),
   rejecting negative amounts and clamping Health to its existing maximum. No
   damage logic changes. Pass this method to the ItemPickupHandler constructor.
2. Room owner provides an ID-based removal/deactivation operation (proposed
   `bool RemoveObject(string objectId)`). It is called only after enumeration.
   Full reset must reconstruct removed objects from the original definition,
   preserving the shared Player instance. Existing Create/Update/Draw signatures
   remain compatible. Owner may choose deactivation instead if reset restores
   activation and Update/Draw/collision filtering consistently exclude it.
3. After the collision contracts merge, register a player/item adapter that maps
   collider metadata to PickupId(roomId, objectId) and ItemKind, then invokes
   TryCollect. Keep detection in Collision; do not poll input here.
4. Navigation retains one ItemPickupHandler for the run, drains its removal queue
   before transitions, filters collected IDs on reconstruction, and coordinates
   handler.Reset with room restoration and Player.Reset. Add Inventory.Tests to
   integration verification after review.

No change to IGameObject, ISprite, Sprint 2 constructors, or gameplay controls is
needed. Do not merge teammate feature branches solely to obtain unapproved APIs.

## Validation after integration

Walk onto each item; verify inventory/health and deferred disappearance. Trigger
repeated overlaps, revisit and reconstruct rooms, and reset before/after removal.
Confirm one award per stable ID, no collection-enumeration exceptions, health
capping, and restoration of all initial items. Re-run all existing suites plus
tests/Inventory/Inventory.Tests.csproj. Review the two owner-controlled lifecycle
methods as well as ItemPickupHandler before declaring check-in complete.
