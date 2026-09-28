using GameProject.Sprites;
using Microsoft.Xna.Framework;

namespace GameProject.Objects.Items;

/// <summary>Rupee that spins quickly while hovering.</summary>
public sealed class RupeeItem(Vector2 position, ISprite sprite) : AnimatedItem("Rupee", position, sprite)
{
    protected override Vector2 AnimationOffset(double seconds) =>
        new(0, (float)Math.Sin(seconds * 5) * 5);

    protected override bool AlternateFrame(double seconds) => ((int)(seconds * 10) % 2) == 1;
}
