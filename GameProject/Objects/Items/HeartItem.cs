using GameProject.Sprites;
using Microsoft.Xna.Framework;

namespace GameProject.Objects.Items;

/// <summary>Heart that gently floats and pulses.</summary>
public sealed class HeartItem(Vector2 position, ISprite sprite) : AnimatedItem("Heart", position, sprite)
{
    protected override Vector2 AnimationOffset(double seconds) =>
        new(0, (float)Math.Sin(seconds * 4) * 7);

    protected override bool AlternateFrame(double seconds) => ((int)(seconds * 6) % 2) == 1;
}
