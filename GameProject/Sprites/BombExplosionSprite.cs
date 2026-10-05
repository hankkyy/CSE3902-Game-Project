using GameProject.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameProject.Sprites;

/// <summary>Draws a two-frame, 48-by-48 explosion without changing gameplay state.</summary>
public sealed class BombExplosionSprite(Texture2D pixel) : ISprite
{
    public void Draw(SpriteBatch batch, Vector2 position, Direction direction, bool alternateFrame)
    {
        int x = (int)position.X;
        int y = (int)position.Y;
        int inset = alternateFrame ? 4 : 0;
        batch.Draw(pixel, new Rectangle(x + inset, y + 16, 48 - inset * 2, 16), Color.OrangeRed);
        batch.Draw(pixel, new Rectangle(x + 16, y + inset, 16, 48 - inset * 2), Color.OrangeRed);
        batch.Draw(pixel, new Rectangle(x + 8, y + 8, 32, 32), Color.Orange);
        batch.Draw(pixel, new Rectangle(x + 14, y + 14, 20, 20), Color.Yellow);
        batch.Draw(pixel, new Rectangle(x + 20, y + 20, 8, 8), Color.White);
    }
}
