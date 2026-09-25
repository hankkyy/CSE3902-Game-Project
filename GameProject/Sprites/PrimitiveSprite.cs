using GameProject.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameProject.Sprites;

/// <summary>Asset-free placeholder that can later be replaced by a sprite-sheet implementation.</summary>
public sealed class PrimitiveSprite(Texture2D pixel, Color primary, Color accent, Point size) : ISprite
{
    public void Draw(SpriteBatch spriteBatch, Vector2 position, Direction direction, bool alternateFrame)
    {
        Rectangle body = new((int)position.X, (int)position.Y, size.X, size.Y);
        spriteBatch.Draw(pixel, body, primary);
        int frameOffset = alternateFrame ? 3 : 0;
        Rectangle marker = direction switch
        {
            Direction.Up => new(body.Center.X - 4, body.Top - 4, 8, 7),
            Direction.Down => new(body.Center.X - 4, body.Bottom - 3, 8, 7),
            Direction.Left => new(body.Left - 4, body.Center.Y - 4 + frameOffset, 7, 8),
            _ => new(body.Right - 3, body.Center.Y - 4 + frameOffset, 7, 8)
        };
        spriteBatch.Draw(pixel, marker, accent);
        if (alternateFrame) spriteBatch.Draw(pixel, new Rectangle(body.Left + 4, body.Bottom - 4, 6, 5), accent);
    }
}
