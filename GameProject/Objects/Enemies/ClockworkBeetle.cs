using GameProject.Core;
using GameProject.Sprites;
using Microsoft.Xna.Framework;

namespace GameProject.Objects.Enemies;

/// <summary>A mechanical ruin guard that patrols four sides of a small rectangle.</summary>
public sealed class ClockworkBeetle(string name, Vector2 position, ISprite sprite) : EnemyCharacter(name, position, sprite)
{
    protected override void UpdateMovement()
    {
        const float width = 36;
        const float height = 16;
        const float speed = 32;
        float distance = (float)(elapsed * speed % (2 * (width + height)));
        if (distance < width)
        {
            Position = startPosition + new Vector2(distance, 0);
            Facing = Direction.Right;
        }
        else if (distance < width + height)
        {
            Position = startPosition + new Vector2(width, -(distance - width));
            Facing = Direction.Up;
        }
        else if (distance < 2 * width + height)
        {
            Position = startPosition + new Vector2(width - (distance - width - height), -height);
            Facing = Direction.Left;
        }
        else
        {
            Position = startPosition + new Vector2(0, -height + (distance - 2 * width - height));
            Facing = Direction.Down;
        }
    }
}
