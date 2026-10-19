using Microsoft.Xna.Framework;

namespace GameProject.Rooms.Definitions;

/// <summary>A top-left position in room screen coordinates.</summary>
public sealed record RoomPoint
{
    public required int X { get; init; }
    public required int Y { get; init; }
    public Vector2 ToVector2() => new(X, Y);
}
