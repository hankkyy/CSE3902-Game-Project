using GameProject.Core;
using GameProject.Sprites;
using Microsoft.Xna.Framework;

namespace GameProject.Objects.Enemies;

/// <summary>Flies in a bounded figure-eight relative to its starting point.</summary>
public sealed class Keese(string name, Vector2 position, ISprite sprite) : EnemyCharacter(name, position, sprite)
{
    protected override void UpdateMovement()
    {
        const float horizontalRange = 55;
        const float verticalRange = 18;
        double phase = elapsed * 3;
        Position = startPosition + new Vector2((float)Math.Sin(phase) * horizontalRange,
            (float)Math.Sin(phase * 2) * verticalRange);
        Facing = Math.Cos(phase) < 0 ? Direction.Left : Direction.Right;
    }
}
