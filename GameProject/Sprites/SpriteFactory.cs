using GameProject.Objects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameProject.Sprites;

/// <summary>Centralizes visual creation so gameplay classes do not know texture details.</summary>
public sealed class SpriteFactory
{
    private readonly Texture2D pixel;

    public SpriteFactory(GraphicsDevice graphicsDevice)
    {
        pixel = new Texture2D(graphicsDevice, 1, 1);
        pixel.SetData([Color.White]);
    }

    public ISprite CreatePlayerSprite() =>
        new PrimitiveSprite(pixel, new Color(50, 165, 83), new Color(229, 211, 118), new Point(28, 34));

    public ISprite CreateBlockSprite(BlockKind kind) => kind switch
    {
        BlockKind.Stone => new PrimitiveSprite(pixel, Color.SlateGray, Color.LightGray, new Point(52, 52)),
        BlockKind.Push => new PrimitiveSprite(pixel, new Color(139, 91, 55), Color.Bisque, new Point(52, 52)),
        BlockKind.Water => new PrimitiveSprite(pixel, Color.RoyalBlue, Color.LightBlue, new Point(52, 52)),
        _ => new PrimitiveSprite(pixel, Color.DarkGoldenrod, Color.Goldenrod, new Point(52, 60))
    };

    public ISprite CreateItemSprite(ItemKind kind) => kind switch
    {
        ItemKind.Heart => new PrimitiveSprite(pixel, Color.Crimson, Color.Pink, new Point(24, 24)),
        ItemKind.Rupee => new PrimitiveSprite(pixel, Color.LimeGreen, Color.LightGreen, new Point(18, 30)),
        ItemKind.Key => new PrimitiveSprite(pixel, Color.Gold, Color.LightYellow, new Point(16, 34)),
        _ => new PrimitiveSprite(pixel, Color.Black, Color.OrangeRed, new Point(28, 28))
    };

    public ISprite CreateEnemySprite(EnemyKind kind) => kind switch
    {
        EnemyKind.Octorok => new PrimitiveSprite(pixel, Color.IndianRed, Color.MistyRose, new Point(32, 32)),
        EnemyKind.Keese => new PrimitiveSprite(pixel, Color.MediumPurple, Color.Lavender, new Point(38, 20)),
        EnemyKind.Gel => new PrimitiveSprite(pixel, Color.CornflowerBlue, Color.LightCyan, new Point(26, 24)),
        _ => new PrimitiveSprite(pixel, Color.BurlyWood, Color.White, new Point(30, 42))
    };

    public void DrawPanel(SpriteBatch spriteBatch, Rectangle rectangle, Color color) => spriteBatch.Draw(pixel, rectangle, color);
}
