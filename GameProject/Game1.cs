using GameProject.Controllers;
using GameProject.Core;
using GameProject.Objects;
using GameProject.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameProject;

/// <summary>Sprint 2 functionality gallery with isolated object families.</summary>
public sealed class Game1 : Game
{
    private readonly GraphicsDeviceManager graphics;
    private SpriteBatch? spriteBatch;
    private SpriteFactory? spriteFactory;
    private Player? player;
    private ObjectGallery<BlockObject>? blocks;
    private ObjectGallery<ItemObject>? items;
    private ObjectGallery<EnemyObject>? enemies;
    private KeyboardController? keyboard;

    public Game1()
    {
        graphics = new GraphicsDeviceManager(this)
        {
            PreferredBackBufferWidth = 960,
            PreferredBackBufferHeight = 540
        };
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
        Window.Title = "CSE 3902 - Sprint 2 Functionality Gallery";
    }

    protected override void LoadContent()
    {
        spriteBatch = new SpriteBatch(GraphicsDevice);
        spriteFactory = new SpriteFactory(GraphicsDevice);
        CreateDemoObjects();
        base.LoadContent();
    }

    private void CreateDemoObjects()
    {
        if (spriteFactory is null)
        {
            throw new InvalidOperationException("Sprite factory must be initialized first.");
        }

        player = new Player(new Vector2(270, 270), new Rectangle(24, 72, 560, 430), spriteFactory);
        blocks = new ObjectGallery<BlockObject>(
        [
            new("Stone Block", new Vector2(700, 125), BlockKind.Stone, spriteFactory),
            new("Push Block", new Vector2(700, 125), BlockKind.Push, spriteFactory),
            new("Water Tile", new Vector2(700, 125), BlockKind.Water, spriteFactory),
            new("Statue", new Vector2(700, 125), BlockKind.Statue, spriteFactory)
        ]);
        items = new ObjectGallery<ItemObject>(
        [
            new("Heart", new Vector2(700, 270), ItemKind.Heart, spriteFactory),
            new("Rupee", new Vector2(700, 270), ItemKind.Rupee, spriteFactory),
            new("Key", new Vector2(700, 270), ItemKind.Key, spriteFactory),
            new("Bomb", new Vector2(700, 270), ItemKind.Bomb, spriteFactory)
        ]);
        enemies = new ObjectGallery<EnemyObject>(
        [
            new("Octorok", new Vector2(700, 420), EnemyKind.Octorok, spriteFactory),
            new("Keese", new Vector2(700, 420), EnemyKind.Keese, spriteFactory),
            new("Gel", new Vector2(700, 420), EnemyKind.Gel, spriteFactory),
            new("Old Man", new Vector2(700, 420), EnemyKind.OldMan, spriteFactory)
        ]);
        keyboard = new KeyboardController(
            player,
            () => blocks.Previous(), () => blocks.Next(),
            () => items.Previous(), () => items.Next(),
            () => enemies.Previous(), () => enemies.Next(),
            ResetDemo, Exit);
    }

    private void ResetDemo()
    {
        player?.Reset();
        blocks?.Reset();
        items?.Reset();
        enemies?.Reset();
    }

    protected override void Update(GameTime gameTime)
    {
        keyboard?.Update();
        player?.Update(gameTime);
        blocks?.Current.Update(gameTime);
        items?.Current.Update(gameTime);
        enemies?.Current.Update(gameTime);
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(new Color(19, 35, 35));
        if (spriteBatch is null || spriteFactory is null)
        {
            return;
        }

        spriteBatch.Begin(samplerState: SamplerState.PointClamp);
        spriteFactory.DrawPanel(spriteBatch, new Rectangle(16, 58, 576, 452), new Color(43, 78, 59));
        spriteFactory.DrawPanel(spriteBatch, new Rectangle(616, 58, 328, 452), new Color(34, 47, 64));
        player?.Draw(spriteBatch);
        blocks?.Current.Draw(spriteBatch);
        items?.Current.Draw(spriteBatch);
        enemies?.Current.Draw(spriteBatch);
        DrawGalleryIndicators(spriteBatch);
        spriteBatch.End();
        base.Draw(gameTime);
    }

    private void DrawGalleryIndicators(SpriteBatch batch)
    {
        if (spriteFactory is null || blocks is null || items is null || enemies is null || player is null)
        {
            return;
        }

        spriteFactory.DrawPanel(batch, new Rectangle(28, 22, 160, 18), new Color(55, 28, 28));
        spriteFactory.DrawPanel(batch, new Rectangle(30, 24, player.Health * 30, 14), Color.IndianRed);
        spriteFactory.DrawPanel(batch, new Rectangle(216, 22, player.SelectedItem * 24, 18), Color.Gold);
        DrawDots(batch, blocks, 650, 82, new Color(150, 170, 190));
        DrawDots(batch, items, 650, 227, Color.Gold);
        DrawDots(batch, enemies, 650, 377, Color.IndianRed);
    }

    private void DrawDots<T>(SpriteBatch batch, ObjectGallery<T> gallery, int x, int y, Color color)
        where T : IGameObject
    {
        if (spriteFactory is null) return;
        for (int i = 0; i < gallery.Count; i++)
        {
            spriteFactory.DrawPanel(batch, new Rectangle(x + (i * 18), y, 12, 8),
                i == gallery.Index ? color : new Color(75, 85, 95));
        }
    }
}
