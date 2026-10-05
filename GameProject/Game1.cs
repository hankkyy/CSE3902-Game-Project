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
    private ObjectGallery<ObjectDisplayPair>? blocks;
    private ObjectGallery<ObjectDisplayPair>? items;
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
        ObjectDisplayPair CreateBlockPair(string name, BlockKind kind) => new(
            new BlockObject(name, new Vector2(700, 125), kind, spriteFactory),
            new BlockObject(name, new Vector2(430, 140), kind, spriteFactory));

        blocks = new ObjectGallery<ObjectDisplayPair>(
        [
            CreateBlockPair("Stone Block", BlockKind.Stone),
            CreateBlockPair("Push Block", BlockKind.Push),
            CreateBlockPair("Water Tile", BlockKind.Water),
            CreateBlockPair("Statue", BlockKind.Statue),
            CreateBlockPair("Crystal Pillar", BlockKind.CrystalPillar),
            CreateBlockPair("Rune Tile", BlockKind.RuneTile)
        ]);

        ObjectDisplayPair CreateItemPair(string name, ItemKind kind) => new(
            new ItemObject(name, new Vector2(700, 270), kind, spriteFactory),
            new ItemObject(name, new Vector2(480, 280), kind, spriteFactory));

        items = new ObjectGallery<ObjectDisplayPair>(
        [
            CreateItemPair("Heart", ItemKind.Heart),
            CreateItemPair("Rupee", ItemKind.Rupee),
            CreateItemPair("Key", ItemKind.Key),
            CreateItemPair("Bomb", ItemKind.Bomb),
            CreateItemPair("Bow", ItemKind.Bow),
            CreateItemPair("Boomerang", ItemKind.Boomerang),
            CreateItemPair("Ruins Map", ItemKind.Map),
            CreateItemPair("Star Compass", ItemKind.Compass),
            CreateItemPair("Star Shard", ItemKind.StarShard)
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
