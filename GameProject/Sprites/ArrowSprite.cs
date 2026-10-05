using GameProject.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameProject.Sprites;

/// <summary>Draws a four-direction arrow within a 16-by-16 footprint using the existing pixel texture.</summary>
public sealed class ArrowSprite(Texture2D pixel) : ISprite
{
    public void Draw(SpriteBatch batch, Vector2 position, Direction direction, bool alternateFrame)
    {
        Vector2 forward = direction switch
        {
            Direction.Up => new Vector2(0, -1),
            Direction.Down => new Vector2(0, 1),
            Direction.Left => new Vector2(-1, 0),
            _ => new Vector2(1, 0)
        };
        Vector2 across = new(-forward.Y, forward.X);
        Vector2 center = position + new Vector2(7, 7);

        for (int offset = -6; offset <= 6; offset += 2)
        {
            DrawPixel(batch, center + forward * offset, Color.Gold);
        }

        for (int spread = 0; spread <= 4; spread += 2)
        {
            Vector2 head = center + forward * (6 - spread);
            DrawPixel(batch, head + across * spread, Color.White);
            DrawPixel(batch, head - across * spread, Color.White);
        }
    }

    private void DrawPixel(SpriteBatch batch, Vector2 position, Color color) =>
        batch.Draw(pixel, new Rectangle((int)position.X, (int)position.Y, 2, 2), color);
}
