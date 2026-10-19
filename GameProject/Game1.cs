using System.Diagnostics.CodeAnalysis;
using GameProject.Rooms.Navigation;
using GameProject.Sprites;
using GameProject.States;
using GameProject.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameProject;

/// <summary>Creates game services and forwards update and draw calls.</summary>
internal sealed class Game1 : Game
{
    [SuppressMessage("Usage", "CA2213:Disposable fields should be disposed",
        Justification = "MonoGame registers this manager with Game, and base.Dispose(disposing) releases it.")]
    private readonly GraphicsDeviceManager graphics;
    private readonly GameSession session = new();
    private SpriteBatch? spriteBatch;
    private SpriteFactory? sprites;
    private StartMenu? startMenu;
    private RoomGameplay? gameplay;
    private RoomRenderer? roomRenderer;
    private RoomHud? hud;

    public Game1()
    {
        graphics = new GraphicsDeviceManager(this)
        {
            PreferredBackBufferWidth = 960,
            PreferredBackBufferHeight = 540
        };
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
        Window.Title = "CSE 3902 - Starlight Ruins - Sprint 3";
    }

    protected override void LoadContent()
    {
        spriteBatch = new SpriteBatch(GraphicsDevice);
        sprites = new SpriteFactory(GraphicsDevice);
        startMenu = new StartMenu(sprites);
        roomRenderer = new RoomRenderer(sprites);
        hud = new RoomHud(sprites);
        gameplay = RoomGameplay.Create(sprites, session, Exit);
        base.LoadContent();
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            spriteBatch?.Dispose();
            spriteBatch = null;
        }

        base.Dispose(disposing);
    }

    protected override void Update(GameTime gameTime)
    {
        gameplay?.Update(gameTime, IsActive);
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(new Color(19, 35, 35));
        if (spriteBatch is null)
        {
            return;
        }

        spriteBatch.Begin(samplerState: SamplerState.PointClamp);
        if (session.Mode == GameMode.Menu)
        {
            startMenu?.Draw(spriteBatch, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
        }
        else if (gameplay is not null)
        {
            roomRenderer?.Draw(spriteBatch, gameplay.CurrentRoom);
            hud?.Draw(spriteBatch, gameplay.Navigation, gameplay.Player);
        }

        spriteBatch.End();
        base.Draw(gameTime);
    }
}
