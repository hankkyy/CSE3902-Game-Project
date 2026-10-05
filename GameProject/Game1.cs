using GameProject.Controllers;
using GameProject.Core;
using GameProject.Objects;
using GameProject.Objects.Enemies;
using GameProject.Sprites;
using GameProject.States;
using GameProject.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameProject;

/// <summary>Sprint 2 functionality gallery with isolated object families.</summary>
public sealed class Game1 : Game
{
    private readonly GraphicsDeviceManager graphics;
    private readonly GameSession session = new();
    private StartMenu? startMenu;
    private GameHud? hud;
    private SpriteBatch? spriteBatch;
    private SpriteFactory? spriteFactory;
    private Player? player;
    private ObjectGallery<BlockObject>? blocks;
    private ObjectGallery<ItemObject>? items;
    private ObjectGallery<EnemyDisplayPair>? enemies;
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
        Window.Title = "CSE 3902 - Starlight Ruins - Sprint 2";
    }

    protected override void LoadContent()
    {
        spriteBatch = new SpriteBatch(GraphicsDevice);
        spriteFactory = new SpriteFactory(GraphicsDevice);
        startMenu = new StartMenu(spriteFactory);
        hud = new GameHud(spriteFactory);
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
            new("Statue", new Vector2(700, 125), BlockKind.Statue, spriteFactory),
            new("Crystal Pillar", new Vector2(700, 125), BlockKind.CrystalPillar, spriteFactory),
            new("Rune Tile", new Vector2(700, 125), BlockKind.RuneTile, spriteFactory)
        ]);
        items = new ObjectGallery<ItemObject>(
        [
            new("Heart", new Vector2(700, 270), ItemKind.Heart, spriteFactory),
            new("Rupee", new Vector2(700, 270), ItemKind.Rupee, spriteFactory),
            new("Key", new Vector2(700, 270), ItemKind.Key, spriteFactory),
            new("Bomb", new Vector2(700, 270), ItemKind.Bomb, spriteFactory),
            new("Bow", new Vector2(700, 270), ItemKind.Bow, spriteFactory),
            new("Boomerang", new Vector2(700, 270), ItemKind.Boomerang, spriteFactory),
            new("Ruins Map", new Vector2(700, 270), ItemKind.Map, spriteFactory),
            new("Star Compass", new Vector2(700, 270), ItemKind.Compass, spriteFactory),
            new("Star Shard", new Vector2(700, 270), ItemKind.StarShard, spriteFactory)
        ]);
        // Reserve room to the right for the Octorok's projectile as well as its patrol.
        Vector2 playAreaEnemyPosition = new(370, 450);
        EnemyDisplayPair CreateEnemyPair(string name, EnemyKind kind) => new(
            new EnemyObject(name, new Vector2(700, 420), kind, spriteFactory),
            new EnemyObject(name, playAreaEnemyPosition, kind, spriteFactory));
        enemies = new ObjectGallery<EnemyDisplayPair>(
        [
            CreateEnemyPair("Octorok", EnemyKind.Octorok),
            CreateEnemyPair("Keese", EnemyKind.Keese),
            CreateEnemyPair("Gel", EnemyKind.Gel),
            CreateEnemyPair("Lantern Keeper", EnemyKind.OldMan),
            CreateEnemyPair("Rune Wisp", EnemyKind.RuneWisp),
            CreateEnemyPair("Clockwork Beetle", EnemyKind.ClockworkBeetle),
            CreateEnemyPair("Prism Sentinel", EnemyKind.PrismSentinel)
        ]);
        keyboard = new KeyboardController(
            player,
            () => blocks.Previous(), () => blocks.Next(),
            () => items.Previous(), () => items.Next(),
            () => enemies.Previous(), () => enemies.Next(),
            ResetDemo, Exit, session);
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
        if (session.Mode == GameMode.Playing)
        {
            player?.Update(gameTime);
            blocks?.Current.Update(gameTime);
            items?.Current.Update(gameTime);
            enemies?.Current.Update(gameTime);
        }

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
        if (session.Mode == GameMode.Menu)
        {
            startMenu?.Draw(spriteBatch, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
        }
        else
        {
            DrawGameplay(spriteBatch);
        }

        spriteBatch.End();
        base.Draw(gameTime);
    }

    private void DrawGameplay(SpriteBatch spriteBatch)
    {
        if (hud is null || player is null || blocks is null || items is null || enemies is null)
        {
            return;
        }

        hud.DrawBackground(spriteBatch);
        player.Draw(spriteBatch);
        blocks.Current.Draw(spriteBatch);
        items.Current.Draw(spriteBatch);
        enemies.Current.Draw(spriteBatch);
        hud.Draw(spriteBatch,
            new PlayerHudInfo(player.Health, player.SelectedItem, player.Action.ToString(), player.Facing.ToString()),
            GetGalleryHudInfo(blocks), GetGalleryHudInfo(items), GetGalleryHudInfo(enemies));
    }

    private static GalleryHudInfo GetGalleryHudInfo<T>(ObjectGallery<T> gallery) where T : IGameObject =>
        new(gallery.Current.Name, gallery.Index, gallery.Count);
}
