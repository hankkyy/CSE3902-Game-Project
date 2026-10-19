namespace GameProject.Rooms.Definitions;

/// <summary>An exit leading to an entry in another room.</summary>
public sealed record RoomLink
{
    public required string Id { get; init; }
    public required string Direction { get; init; }
    public required string TargetRoomId { get; init; }
    public required string TargetEntryId { get; init; }
    public required RoomArea Trigger { get; init; }
}
