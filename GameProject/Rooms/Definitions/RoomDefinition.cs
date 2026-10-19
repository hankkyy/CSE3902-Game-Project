namespace GameProject.Rooms.Definitions;

/// <summary>External initial state; runtime objects and progress live separately.</summary>
public sealed record RoomDefinition
{
    public required string Id { get; init; }
    public required string Name { get; init; }
    public required RoomArea Bounds { get; init; }
    public required IReadOnlyDictionary<string, RoomPoint> Entries { get; init; }
    public required IReadOnlyList<RoomLink> Links { get; init; }
    public required IReadOnlyList<RoomObjectDefinition> Objects { get; init; }
}
