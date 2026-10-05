using GameProject.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameProject.Sprites;

/// <summary>Draws a small round bomb with a flashing fuse, using the existing pixel texture.</summary>
public sealed class BombSprite(Texture2D pixel) : ISprite
{
    public void Draw(SpriteBatch batch, Vector2 position, Direction direction, bool alternateFrame)
    {
        int x = (int)position.X;
        int y = (int)position.Y;
        Color body = alternateFrame ? Color.DarkRed : new Color(25, 32, 50);
        batch.Draw(pixel, new Rectangle(x + 4, y + 4, 8, 12), Color.LightSteelBlue);
        batch.Draw(pixel, new Rectangle(x + 2, y + 6, 12, 8), Color.LightSteelBlue);
        batch.Draw(pixel, new Rectangle(x + 4, y + 6, 8, 8), body);
        batch.Draw(pixel, new Rectangle(x + 6, y + 7, 2, 2), Color.White);
        batch.Draw(pixel, new Rectangle(x + 7, y + 2, 2, 4), Color.Gold);
        batch.Draw(pixel, new Rectangle(x + 8, y, 3, 3), alternateFrame ? Color.OrangeRed : Color.Yellow);
    }
}
