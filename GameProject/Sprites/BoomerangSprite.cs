using GameProject.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameProject.Sprites;

/// <summary>Draws an original two-frame turning boomerang inside a 16-by-16 footprint.</summary>
public sealed class BoomerangSprite(Texture2D pixel) : ISprite
{
    public void Draw(SpriteBatch batch, Vector2 position, Direction direction, bool alternateFrame)
    {
        int quarterTurns = direction switch
        {
            Direction.Up => 0,
            Direction.Right => 1,
            Direction.Down => 2,
            _ => 3
        };
        if (alternateFrame) quarterTurns++;

        for (int offset = -4; offset <= 4; offset += 2)
        {
            DrawSegment(batch, position, new Point(offset, -4), quarterTurns);
            DrawSegment(batch, position, new Point(-4, offset), quarterTurns);
        }
    }

    private void DrawSegment(SpriteBatch batch, Vector2 position, Point offset, int quarterTurns)
    {
        for (int index = 0; index < quarterTurns; index++) offset = new Point(-offset.Y, offset.X);
        int x = (int)position.X + 6 + offset.X;
        int y = (int)position.Y + 6 + offset.Y;
        batch.Draw(pixel, new Rectangle(x, y, 4, 4), Color.SaddleBrown);
        batch.Draw(pixel, new Rectangle(x + 1, y + 1, 2, 2), Color.Gold);
    }
}
