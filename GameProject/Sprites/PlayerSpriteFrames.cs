using GameProject.Core;
using Microsoft.Xna.Framework;

namespace GameProject.Sprites;

/// <summary>Owns atlas layout, timing, and the anchor that preserves the player's 28 by 34 footprint.</summary>
public static class PlayerSpriteFrames
{
    public const int CellSize = 40;
    public const int Columns = 4;
    public const int Rows = 16;
    public const int Scale = 2;
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
        int row = ((int)animation * 4) + directionRow;
        return new Rectangle(frame * CellSize, row * CellSize, CellSize, CellSize);
    }

    public static Rectangle Destination(Vector2 bodyPosition) => new(
        (int)bodyPosition.X - (BodyOrigin.X * Scale),
        (int)bodyPosition.Y - (BodyOrigin.Y * Scale),
        CellSize * Scale,
        CellSize * Scale);
}
