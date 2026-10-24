namespace GameProject.Rooms.Definitions;

/// <summary>Stable, room-local identity and external data for one object.</summary>
public sealed record RoomObjectDefinition
{
    public required string Id { get; init; }
    public required string Type { get; init; }
    public required string Name { get; init; }
    public required RoomPoint Position { get; init; }
    public IReadOnlyDictionary<string, string> Properties { get; init; } = new Dictionary<string, string>();
}
