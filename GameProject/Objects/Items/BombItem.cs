using GameProject.Sprites;
using Microsoft.Xna.Framework;

namespace GameProject.Objects.Items;

/// <summary>Bomb that hops and flashes its fuse without interacting with other objects.</summary>
public sealed class BombItem(Vector2 position, ISprite sprite) : AnimatedItem("Bomb", position, sprite)
{
    protected override Vector2 AnimationOffset(double seconds) =>
        new(0, -Math.Abs((float)Math.Sin(seconds * 4)) * 6);

    protected override bool AlternateFrame(double seconds) => ((int)(seconds * 8) % 2) == 1;
}
