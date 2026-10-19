namespace GameProject.Inventory;

/// <summary>Stable identity from room data; equal object IDs in different rooms are distinct pickups.</summary>
public readonly record struct PickupId(string RoomId, string ObjectId);
