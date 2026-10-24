using Microsoft.Xna.Framework;

namespace GameProject.Rooms.Definitions;

/// <summary>Walkable bounds or an exit trigger in room screen coordinates.</summary>
public sealed record RoomArea
{
    public required int X { get; init; }
    public required int Y { get; init; }
    public required int Width { get; init; }
    public required int Height { get; init; }
    public Rectangle ToRectangle() => new(X, Y, Width, Height);
}
