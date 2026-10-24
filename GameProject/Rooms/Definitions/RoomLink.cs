namespace GameProject.Rooms.Definitions;

/// <summary>Navigation data only; the navigation owner decides when to transition.</summary>
public sealed record RoomLink
{
    public required string Id { get; init; }
    public required string Direction { get; init; }
    public required string TargetRoomId { get; init; }
    public required string TargetEntryId { get; init; }
    public required RoomArea Trigger { get; init; }
}
