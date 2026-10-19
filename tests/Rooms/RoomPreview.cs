using GameProject.Objects;
using GameProject.Rooms.Definitions;
using GameProject.Rooms.Loading;
using GameProject.Sprites;
using GameProject.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Rooms.Tests;

/// <summary>A test window for viewing the room data.</summary>
internal sealed class RoomPreview : Game
{
    private readonly string? captureDirectory;
    private readonly GraphicsDeviceManager graphics;
    private SpriteBatch batch = null!;
    private RoomRenderer renderer = null!;
    private PixelText text = null!;
    private RenderTarget2D target = null!;
    private LoadedRoom[] rooms = [];
    private KeyboardState previous;
    private int index;

    public RoomPreview(string? captureDirectory)
    {
        this.captureDirectory = captureDirectory;
        graphics = new GraphicsDeviceManager(this)
        {
            PreferredBackBufferWidth = 960,
            PreferredBackBufferHeight = 540
        };
        Window.Title = "Sprint 3 - Room loading acceptance";
    }

    protected override void LoadContent()
    {
        SpriteFactory sprites = new(GraphicsDevice);
        batch = new SpriteBatch(GraphicsDevice);
        target = new RenderTarget2D(GraphicsDevice, 960, 540);
        renderer = new RoomRenderer(sprites);
        text = new PixelText(sprites);
        IReadOnlyList<RoomDefinition> definitions = RoomLoader.LoadDirectory(Path.Combine(AppContext.BaseDirectory, "Content", "Rooms"));
        rooms = definitions.Select(definition => new LoadedRoom(definition, new RoomObjectFactory(definition.Bounds.ToRectangle(), sprites))).ToArray();
    }

    protected override void Update(GameTime gameTime)
    {
        if (captureDirectory is not null) return;
        KeyboardState keys = Keyboard.GetState();
        bool Pressed(Keys key) => keys.IsKeyDown(key) && previous.IsKeyUp(key);
        if (Pressed(Keys.Escape) || Pressed(Keys.Q)) Exit();
        if (Pressed(Keys.PageDown)) index = (index + 1) % rooms.Length;
        if (Pressed(Keys.PageUp)) index = (index + rooms.Length - 1) % rooms.Length;
        Player player = (Player)rooms[index].Objects["player"];
        Vector2 direction = Vector2.Zero;
        if (keys.IsKeyDown(Keys.A) || keys.IsKeyDown(Keys.Left)) direction.X--;
        if (keys.IsKeyDown(Keys.D) || keys.IsKeyDown(Keys.Right)) direction.X++;
        if (keys.IsKeyDown(Keys.W) || keys.IsKeyDown(Keys.Up)) direction.Y--;
        if (keys.IsKeyDown(Keys.S) || keys.IsKeyDown(Keys.Down)) direction.Y++;
        player.SetMovement(direction);
        if (Pressed(Keys.Z) || Pressed(Keys.N)) player.Attack();
        if (Pressed(Keys.D1)) player.TryFireArrow();
        if (Pressed(Keys.D2)) player.TryThrowBoomerang();
        if (Pressed(Keys.D3)) player.TryPlaceBomb();
        if (Pressed(Keys.R)) rooms[index].Reset();
        rooms[index].Update(gameTime);
        previous = keys;
    }

    protected override void Draw(GameTime gameTime)
    {
        LoadedRoom room = rooms[index];
        GraphicsDevice.SetRenderTarget(target);
        GraphicsDevice.Clear(new Color(15, 25, 30));
        batch.Begin(samplerState: SamplerState.PointClamp);
        text.Draw(batch, room.Name.ToUpperInvariant(), new Point(24, 20), 3, Color.Wheat);
        text.Draw(batch, $"ROOM {index + 1}/6 - {room.Objects.Count} OBJECTS - EXTERNAL JSON", new Point(24, 50), 1, Color.LightGray);
        renderer.Draw(batch, room);
        text.Draw(batch, "PGUP/PGDN ROOM  WASD MOVE  Z ATTACK  1/2/3 ITEMS  R RESET  Q QUIT", new Point(24, 518), 1, Color.LightGray);
        batch.End();
        GraphicsDevice.SetRenderTarget(null);
        batch.Begin(samplerState: SamplerState.PointClamp);
        batch.Draw(target, Vector2.Zero, Color.White);
        batch.End();
        if (captureDirectory is not null)
        {
            Directory.CreateDirectory(captureDirectory);
            using FileStream file = File.Create(Path.Combine(captureDirectory, room.Definition.Id + ".png"));
            target.SaveAsPng(file, 960, 540);
            index++;
            if (index == rooms.Length) Exit();
        }
    }

    protected override void UnloadContent()
    {
        target?.Dispose();
        batch?.Dispose();
        base.UnloadContent();
    }
}
