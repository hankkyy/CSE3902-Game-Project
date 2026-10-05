using GameProject.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameProject.UI;

/// <summary>Displays player status, gallery selections, and keyboard help without changing game state.</summary>
public sealed class GameHud
{
    private const int GalleryLeft = 632;
    private const int GalleryNameLimit = 24;
    private const int HealthSegmentWidth = 30;
    private const int ItemSegmentWidth = 24;
    private const string Controls = "WASD/ARROWS MOVE  Z/N SWORD  1 ARROW 2 BOOM 3 BOMB  E HURT R RESET Q/ESC QUIT";
    private readonly SpriteFactory sprites;
    private readonly PixelText text;

    public GameHud(SpriteFactory sprites)
    {
        this.sprites = sprites;
        text = new PixelText(sprites);
    }

    public void DrawBackground(SpriteBatch batch)
    {
        sprites.DrawPanel(batch, new Rectangle(16, 58, 576, 452), new Color(43, 78, 59));
        sprites.DrawPanel(batch, new Rectangle(616, 58, 328, 452), new Color(34, 47, 64));
        sprites.DrawPanel(batch, new Rectangle(GalleryLeft, 203, 296, 1), new Color(65, 80, 95));
        sprites.DrawPanel(batch, new Rectangle(GalleryLeft, 345, 296, 1), new Color(65, 80, 95));
    }

    public void Draw(SpriteBatch batch, PlayerHudInfo player,
        GalleryHudInfo blocks, GalleryHudInfo items, GalleryHudInfo enemies)
    {
        DrawPlayerStatus(batch, player);
        DrawGallery(batch, blocks, "BLOCKS", "T PREV", "Y NEXT", 68, new Color(175, 200, 220));
        DrawGallery(batch, items, "ITEMS", "U PREV", "I NEXT", 210, Color.Gold);
        DrawGallery(batch, enemies, "ENEMIES / NPCS", "O PREV", "P NEXT", 352, Color.IndianRed);
        text.Draw(batch, Controls, new Point(24, 518), 2, new Color(215, 230, 225));
    }

    private void DrawPlayerStatus(SpriteBatch batch, PlayerHudInfo player)
    {
        text.Draw(batch, $"HEALTH {player.Health}", new Point(28, 10), 1, Color.White);
        sprites.DrawPanel(batch, new Rectangle(28, 27, 160, 16), new Color(55, 28, 28));
        sprites.DrawPanel(batch, new Rectangle(30, 29, player.Health * HealthSegmentWidth, 12), Color.IndianRed);

        string itemLabel = player.SelectedItem switch
        {
            1 => "ITEM 1 ARROW",
            2 => "ITEM 2 BOOMERANG",
            _ => "ITEM 3 BOMB"
        };
        text.Draw(batch, itemLabel, new Point(216, 10), 1, Color.White);
        sprites.DrawPanel(batch, new Rectangle(216, 27, player.SelectedItem * ItemSegmentWidth, 16), Color.Gold);

        text.Draw(batch, "STATE", new Point(340, 10), 1, Color.White);
        text.Draw(batch, player.Action, new Point(340, 28), 2, Color.Gold);
        text.Draw(batch, "FACING", new Point(536, 10), 1, Color.White);
        text.Draw(batch, player.Facing, new Point(536, 28), 2, new Color(175, 200, 220));
        text.Draw(batch, "SPRINT 2", new Point(740, 20), 2, new Color(190, 225, 230));
    }

    private void DrawGallery(SpriteBatch batch, GalleryHudInfo gallery, string title,
        string previousKey, string nextKey, int top, Color color)
    {
        text.Draw(batch, $"{title} {gallery.Index + 1}/{gallery.Count}", new Point(GalleryLeft, top), 1, Color.White);
        text.Draw(batch, previousKey, new Point(840, top), 1, new Color(190, 205, 215));
        text.Draw(batch, nextKey, new Point(840, top + 12), 1, new Color(190, 205, 215));

        string name = gallery.Name.Length <= GalleryNameLimit
            ? gallery.Name : gallery.Name[..(GalleryNameLimit - 3)] + "...";
        text.Draw(batch, name, new Point(GalleryLeft, top + 24), 2, color);

        for (int index = 0; index < gallery.Count; index++)
        {
            sprites.DrawPanel(batch, new Rectangle(GalleryLeft + (index * 18), top + 126, 12, 8),
                index == gallery.Index ? color : new Color(75, 85, 95));
        }
    }
}
