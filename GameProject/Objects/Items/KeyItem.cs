using GameProject.Sprites;
using Microsoft.Xna.Framework;

namespace GameProject.Objects.Items;

/// <summary>Key that sways horizontally on a slow hover cycle.</summary>
public sealed class KeyItem(Vector2 position, ISprite sprite) : AnimatedItem("Key", position, sprite)
{
    protected override Vector2 AnimationOffset(double seconds) =>
        new((float)Math.Sin(seconds * 3) * 4, (float)Math.Sin(seconds * 6) * 3);

    protected override bool AlternateFrame(double seconds) => ((int)(seconds * 4) % 2) == 1;
}
