using GameProject.Core;
using Microsoft.Xna.Framework;

namespace GameProject.Sprites;

/// <summary>Defines animation settings and the rectangles used to draw the player.</summary>
public static class PlayerSpriteFrames
{
    public const int CellSize = 40;
    public const int Columns = 4;
    public const int Rows = 16;
    public const int Scale = 2;
    private const int DirectionsPerAnimation = 4;
    public static Point BodyOrigin => new(13, 11);

    public static PlayerAnimationClip Clip(PlayerSpriteAnimation animation) => animation switch
    {
        PlayerSpriteAnimation.Idle => new(2, 0.45, true),
        PlayerSpriteAnimation.Walking => new(4, 0.10, true),
        PlayerSpriteAnimation.Attacking => new(3, 0.09, false),
        PlayerSpriteAnimation.Damaged => new(2, 0.075, true),
        _ => throw new ArgumentOutOfRangeException(nameof(animation))
    };

    public static Rectangle Source(PlayerSpriteAnimation animation, Direction direction, double elapsedSeconds)
    {
        int frame = Clip(animation).FrameAt(elapsedSeconds);
        int directionRow = direction switch
        {
            Direction.Down => 0,
            Direction.Left => 1,
            Direction.Right => 2,
            Direction.Up => 3,
            _ => throw new ArgumentOutOfRangeException(nameof(direction))
        };
        // Each action occupies four direction rows; frames run across the columns.
        int row = ((int)animation * DirectionsPerAnimation) + directionRow;
        int sourceX = frame * CellSize;
        int sourceY = row * CellSize;
        return new Rectangle(sourceX, sourceY, CellSize, CellSize);
    }

    public static Rectangle Destination(Vector2 bodyPosition)
    {
        // The cell includes space for the sword; align the body, not the cell corner.
        int screenX = (int)bodyPosition.X - BodyOrigin.X * Scale;
        int screenY = (int)bodyPosition.Y - BodyOrigin.Y * Scale;
        int size = CellSize * Scale;
        return new Rectangle(screenX, screenY, size, size);
    }
}
