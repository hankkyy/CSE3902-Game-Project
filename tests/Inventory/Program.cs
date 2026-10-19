using GameProject.Inventory;
using GameProject.Objects;

int checks = 0;
void Check(bool condition, string message)
{
    checks++;
    if (!condition) throw new InvalidOperationException(message);
}
void Reject(Action action, string message)
{
    bool rejected = false;
    try { action(); }
    catch (ArgumentException) { rejected = true; }
    Check(rejected, message);
}

int health = 3;
InventoryState inventory = new();
ItemPickupHandler pickups = new(inventory, amount => health = Math.Min(5, health + amount));
List<PickupId> roomObjects = [];
foreach (ItemKind kind in Enum.GetValues<ItemKind>())
{
    roomObjects.Add(new PickupId("hall", kind.ToString()));
}

// Iterate the actual collection: removing here would invalidate enumeration.
foreach (PickupId id in roomObjects)
{
    ItemKind kind = Enum.Parse<ItemKind>(id.ObjectId);
    Check(pickups.TryCollect(id, kind), $"First collection: {kind}");
    Check(!pickups.TryCollect(id, kind), $"Repeated contact: {kind}");
}
Check(roomObjects.Count == 9 && pickups.PendingRemovalCount == 9, "Removal must be deferred");
Check(health == 4, "Heart heals exactly once");
Check(inventory.Rupees == 1 && inventory.Keys == 1 && inventory.Bombs == 1, "Resource counts");
Check(inventory.HasBow && inventory.HasBoomerang, "Weapon unlocks");
Check(inventory.HasMap && inventory.HasCompass, "Navigation pickups");
Check(inventory.StarShards == 1 && inventory.HasObjective, "Objective state");
pickups.ApplyPendingRemovals(id => roomObjects.Remove(id));
Check(roomObjects.Count == 0 && pickups.PendingRemovalCount == 0, "Post-pass removal");
pickups.ApplyPendingRemovals(_ => throw new InvalidOperationException("Empty queue ran"));

// Recreate identities as a room loader would; object references are irrelevant.
PickupId revisited = new("hall", "Rupee");
Check(pickups.IsCollected(revisited), "Re-entry remembers stable identity");
Check(!pickups.TryCollect(revisited, ItemKind.Rupee) && inventory.Rupees == 1, "No reward after re-entry");
Check(pickups.TryCollect(new PickupId("cellar", "Rupee"), ItemKind.Rupee), "Room-local IDs do not collide");
Check(inventory.Rupees == 2, "Other room grants its reward");
Check(pickups.TryCollect(new PickupId("hall", "Rupee2"), ItemKind.Rupee), "Different item in same room");
Check(inventory.Rupees == 3, "Second distinct resource counts");
List<PickupId> removed = [];
pickups.ApplyPendingRemovals(removed.Add);
Check(removed.SequenceEqual(new[] { new PickupId("cellar", "Rupee"), new PickupId("hall", "Rupee2") }), "Removal carries correct room identity");

health = 5;
Check(pickups.TryCollect(new PickupId("hall", "full-heart"), ItemKind.Heart), "Full-health policy consumes heart");
Check(health == 5, "Healing callback caps health");
int count = pickups.PendingRemovalCount;
Reject(() => pickups.TryCollect(new PickupId("hall", "bad"), (ItemKind)999), "Unknown kind rejected");
Reject(() => pickups.TryCollect(default, ItemKind.Key), "Missing identity rejected");
Reject(() => pickups.TryCollect(new PickupId(" ", "item"), ItemKind.Key), "Blank room rejected");
Check(!pickups.IsCollected(new PickupId("hall", "bad")) && pickups.PendingRemovalCount == count, "Invalid request leaves state unchanged");

bool removalFailed = false;
try { pickups.ApplyPendingRemovals(_ => throw new InvalidOperationException("Simulated room removal failure")); }
catch (InvalidOperationException) { removalFailed = true; }
Check(removalFailed && pickups.PendingRemovalCount == count, "Failed removal remains queued");
Check(!pickups.TryCollect(new PickupId("hall", "full-heart"), ItemKind.Heart), "Failure cannot award twice");
pickups.ApplyPendingRemovals(removed.Add);
Check(pickups.PendingRemovalCount == 0, "Removal retry succeeds");

pickups.TryCollect(new PickupId("hall", "pending-key"), ItemKind.Key);
pickups.Reset();
pickups.Reset();
Check(pickups.PendingRemovalCount == 0 && !pickups.IsCollected(revisited), "Reset clears pending and persistent state");
Check(inventory.Rupees == 0 && inventory.Keys == 0 && inventory.Bombs == 0 && inventory.StarShards == 0, "Reset clears counters");
Check(!inventory.HasBow && !inventory.HasBoomerang && !inventory.HasMap && !inventory.HasCompass && !inventory.HasObjective, "Reset clears flags");
pickups.ApplyPendingRemovals(_ => throw new InvalidOperationException("Old removal survived reset"));
Check(pickups.TryCollect(revisited, ItemKind.Rupee) && inventory.Rupees == 1, "New run can collect again");
Check(pickups.TryCollect(new PickupId("a/b", "c"), ItemKind.Key), "Structured identity first pickup");
Check(pickups.TryCollect(new PickupId("a", "b/c"), ItemKind.Key), "Structured identity avoids concatenation collisions");
Check(pickups.TryCollect(new PickupId("hall", "bow-again"), ItemKind.Bow), "Another bow pickup is consumed");
Check(inventory.HasBow, "Weapon unlock stays true");

InventoryState separateInventory = new();
ItemPickupHandler failingHeal = new(separateInventory, _ => throw new InvalidOperationException("Healing unavailable"));
bool healFailed = false;
PickupId failedHeart = new("hall", "heart-failure");
try { failingHeal.TryCollect(failedHeart, ItemKind.Heart); }
catch (InvalidOperationException) { healFailed = true; }
Check(healFailed && !failingHeal.IsCollected(failedHeart) && failingHeal.PendingRemovalCount == 0,
    "Failed healing must not mark a pickup collected or queue its removal");
Console.WriteLine($"PASS: {checks} inventory checks (effects, duplicates, deferred removal, re-entry, invalid input, retry, reset).");
