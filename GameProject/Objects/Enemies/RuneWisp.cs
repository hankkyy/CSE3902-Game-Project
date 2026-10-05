using GameProject.Core;
using GameProject.Sprites;
using Microsoft.Xna.Framework;

namespace GameProject.Objects.Enemies;

/// <summary>A ruin light-spirit that floats around a small elliptical path.</summary>
public sealed class RuneWisp(string name, Vector2 position, ISprite sprite) : EnemyCharacter(name, position, sprite)
{
    protected override void UpdateMovement()
    {
        double phase = elapsed * 1.4;
        Position = startPosition + new Vector2((float)Math.Sin(phase) * 28,
            ((float)Math.Cos(phase) - 1) * 8);
        Facing = Math.Cos(phase) < 0 ? Direction.Left : Direction.Right;
    }
}
