using GameProject.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameProject.UI;

/// <summary>Renders the start screen and its keyboard instructions.</summary>
public sealed class StartMenu
{
    private const int PanelWidth = 672;
    private const int PanelHeight = 348;
    private const int StartButtonWidth = 432;
    private const int StartButtonHeight = 66;
    private readonly SpriteFactory sprites;
    private readonly PixelText text;

    public StartMenu(SpriteFactory sprites)
    {
        this.sprites = sprites;
        text = new PixelText(sprites);
    }

    public void Draw(SpriteBatch batch, int screenWidth, int screenHeight)
    {
        int top = (screenHeight - PanelHeight) / 2;
        sprites.DrawPanel(batch,
            new Rectangle((screenWidth - PanelWidth) / 2, top, PanelWidth, PanelHeight),
            new Color(34, 47, 64));

        DrawCentered(batch, "CSE 3902", screenWidth, top + 36, 5, new Color(190, 225, 230));
        DrawCentered(batch, "SPRINT 2", screenWidth, top + 100, 3, Color.Gold);
        DrawCentered(batch, "STARLIGHT RUINS", screenWidth, top + 144, 2, Color.White);

        sprites.DrawPanel(batch,
            new Rectangle((screenWidth - StartButtonWidth) / 2, top + 190, StartButtonWidth, StartButtonHeight),
            new Color(43, 78, 59));
        DrawCentered(batch, "ENTER TO START", screenWidth, top + 212, 3, Color.White);
        DrawCentered(batch, "Q / ESC TO QUIT", screenWidth, top + 290, 2, new Color(190, 205, 215));
    }

    private void DrawCentered(SpriteBatch batch, string label, int screenWidth, int y, int scale, Color color)
    {
        int x = (screenWidth - PixelText.MeasureWidth(label, scale)) / 2;
        text.Draw(batch, label, new Point(x, y), scale, color);
    }
}
