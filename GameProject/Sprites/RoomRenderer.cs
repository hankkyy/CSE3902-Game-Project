using GameProject.Rooms.Loading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameProject.Sprites;

/// <summary>Draws the room floor, boundary, exits, and objects.</summary>
public sealed class RoomRenderer
{
    private readonly SpriteFactory sprites;
    private const int TileSize = 32;
    private const int BorderWidth = 4;

    public RoomRenderer(SpriteFactory sprites)
    {
        ArgumentNullException.ThrowIfNull(sprites);
        this.sprites = sprites;
    }

    /// <summary>Call inside a SpriteBatch Begin/End pair. Rendering never advances simulation.</summary>
    public void Draw(SpriteBatch batch, LoadedRoom room)
    {
        ArgumentNullException.ThrowIfNull(batch);
        ArgumentNullException.ThrowIfNull(room);
        Rectangle bounds = room.Definition.Bounds.ToRectangle();
        sprites.DrawPanel(batch, new Rectangle(bounds.X - BorderWidth, bounds.Y - BorderWidth,
            bounds.Width + 2 * BorderWidth, bounds.Height + 2 * BorderWidth), new Color(87, 116, 104));
        sprites.DrawPanel(batch, bounds, new Color(25, 43, 44));
        for (int x = bounds.Left; x < bounds.Right; x += TileSize)
            sprites.DrawPanel(batch, new Rectangle(x, bounds.Top, 1, bounds.Height), new Color(31, 51, 51));
        for (int y = bounds.Top; y < bounds.Bottom; y += TileSize)
            sprites.DrawPanel(batch, new Rectangle(bounds.Left, y, bounds.Width, 1), new Color(31, 51, 51));
        foreach (var link in room.Definition.Links)
            sprites.DrawPanel(batch, link.Trigger.ToRectangle(), new Color(64, 100, 91));
        room.Draw(batch);
    }
}
