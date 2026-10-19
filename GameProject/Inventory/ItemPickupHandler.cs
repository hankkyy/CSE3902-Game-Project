using GameProject.Objects;

namespace GameProject.Inventory;

/// <summary>Applies confirmed player/item contacts once and defers room removal until after dispatch.</summary>
public sealed class ItemPickupHandler
{
    private readonly Action<int> restoreHealth;
    private readonly HashSet<PickupId> collected = [];
    private readonly Queue<PickupId> pendingRemovals = new();

    /// <param name="inventory">One shared inventory for the whole run, not a new one per room.</param>
    /// <param name="restoreHealth">Player-owned healing operation; clamps at maximum health.</param>
    public ItemPickupHandler(InventoryState inventory, Action<int> restoreHealth)
    {
        ArgumentNullException.ThrowIfNull(inventory);
        ArgumentNullException.ThrowIfNull(restoreHealth);
        Inventory = inventory;
        this.restoreHealth = restoreHealth;
    }

    public InventoryState Inventory { get; }
    public int PendingRemovalCount => pendingRemovals.Count;

    /// <summary>Use during room construction and collision filtering to hide previously collected objects.</summary>
    public bool IsCollected(PickupId id)
    {
        ValidateId(id);
        return collected.Contains(id);
    }

    /// <summary>Called only after collision detection confirms a player/pickup contact.</summary>
    /// <remarks>Does not detect collisions or mutate the room collection. Hearts are consumed even at full health.</remarks>
    public bool TryCollect(PickupId id, ItemKind kind)
    {
        ValidateId(id);
        if (!Enum.IsDefined(kind))
        {
            throw new ArgumentOutOfRangeException(nameof(kind));
        }

        if (collected.Contains(id))
        {
            return false;
        }

        // Keep the list of effects explicit: adding a kind requires deciding its gameplay effect here.
        if (kind == ItemKind.Heart) restoreHealth(1);
        else if (kind == ItemKind.Rupee) Inventory.AddRupee();
        else if (kind == ItemKind.Key) Inventory.AddKey();
        else if (kind == ItemKind.Bomb) Inventory.AddBomb();
        else if (kind == ItemKind.Bow) Inventory.UnlockBow();
        else if (kind == ItemKind.Boomerang) Inventory.UnlockBoomerang();
        else if (kind == ItemKind.Map) Inventory.RevealMap();
        else if (kind == ItemKind.Compass) Inventory.RevealCompass();
        else if (kind == ItemKind.StarShard) Inventory.AddStarShard();
        else throw new ArgumentOutOfRangeException(nameof(kind));

        collected.Add(id);
        pendingRemovals.Enqueue(id);
        return true;
    }

    /// <summary>Call after collision enumeration and before transitions/drawing. Removal should tolerate an absent object.</summary>
    /// <remarks>If removal fails, the entry remains queued for retry without awarding its effect again.</remarks>
    public void ApplyPendingRemovals(Action<PickupId> removeFromRoom)
    {
        ArgumentNullException.ThrowIfNull(removeFromRoom);
        while (pendingRemovals.Count > 0)
        {
            PickupId id = pendingRemovals.Peek();
            removeFromRoom(id);
            pendingRemovals.Dequeue();
        }
    }

    /// <summary>Clears this module's run state. The global reset must also restore player health and room objects.</summary>
    public void Reset()
    {
        pendingRemovals.Clear();
        collected.Clear();
        Inventory.Reset();
    }

    private static void ValidateId(PickupId id)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(id.RoomId);
        ArgumentException.ThrowIfNullOrWhiteSpace(id.ObjectId);
    }
}
