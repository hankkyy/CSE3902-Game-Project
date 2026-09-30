using GameProject.Core;
using GameProject.Objects;
using GameProject.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace PlayerSprites.Tests;

/// <summary>Branch-local acceptance host. It does not change the production game's input or state code.</summary>
internal sealed class SpriteGallery : Game
{
    private const int Width = 880;
    private const int Height = 620;
    private readonly string? capturePath;
    private readonly GraphicsDeviceManager graphics;
    private SpriteBatch batch = null!;
    private SpriteFactory factory = null!;
    private IAnimatedPlayerSprite sprite = null!;
    private Texture2D pixel = null!;
    private RenderTarget2D target = null!;
    private KeyboardState previous;
    private PlayerSpriteAnimation selected;
    private Direction facing = Direction.Down;
    private double elapsed;
    private int captureIndex;
    private bool paused;

    public SpriteGallery(string? capturePath)
    {
        this.capturePath = capturePath;
        graphics = new GraphicsDeviceManager(this)
        {
            PreferredBackBufferWidth = Width,
            PreferredBackBufferHeight = Height
        };
        Window.Title = "Player sprites - isolated branch acceptance";
        IsMouseVisible = true;
    }

    protected override void LoadContent()
    {
        batch = new SpriteBatch(GraphicsDevice);
        factory = new SpriteFactory(GraphicsDevice);
        sprite = factory.CreateAnimatedPlayerSprite();
        pixel = new Texture2D(GraphicsDevice, 1, 1);
        pixel.SetData([Color.White]);
        target = new RenderTarget2D(GraphicsDevice, Width, Height);
        VerifyRendering();
    }

    protected override void Update(GameTime gameTime)
    {
        KeyboardState keys = Keyboard.GetState();
        bool Pressed(Keys key) => keys.IsKeyDown(key) && previous.IsKeyUp(key);
        if (Pressed(Keys.Q) || Pressed(Keys.Escape)) Exit();
        if (Pressed(Keys.Space)) paused = !paused;
        if (!paused) elapsed += gameTime.ElapsedGameTime.TotalSeconds;
        if (keys.IsKeyDown(Keys.Up) || keys.IsKeyDown(Keys.W)) facing = Direction.Up;
        if (keys.IsKeyDown(Keys.Down) || keys.IsKeyDown(Keys.S)) facing = Direction.Down;
        if (keys.IsKeyDown(Keys.Left) || keys.IsKeyDown(Keys.A)) facing = Direction.Left;
        if (keys.IsKeyDown(Keys.Right) || keys.IsKeyDown(Keys.D)) facing = Direction.Right;
        if (Pressed(Keys.D1)) Select(PlayerSpriteAnimation.Idle);
        if (Pressed(Keys.D2)) Select(PlayerSpriteAnimation.Walking);
        if (Pressed(Keys.Z) || Pressed(Keys.N)) Select(PlayerSpriteAnimation.Attacking);
        if (Pressed(Keys.E)) Select(PlayerSpriteAnimation.Damaged);
        if (Pressed(Keys.R)) { Select(PlayerSpriteAnimation.Idle); facing = Direction.Down; paused = false; }
        previous = keys;
        base.Update(gameTime);
    }

    private void Select(PlayerSpriteAnimation animation)
    {
        selected = animation;
        elapsed = 0;
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.SetRenderTarget(target);
        GraphicsDevice.Clear(new Color(17, 26, 33));
        batch.Begin(samplerState: SamplerState.PointClamp);
        Label("MOSS SCOUT / PLAYER SPRITES", 24, 20, new Color(227, 238, 218), 3);
        Label("SPRINT 2   JOJOLI132   ORIGINAL PIXEL ART", 24, 51, new Color(122, 172, 151));
        string[] headings = ["DOWN", "LEFT", "RIGHT", "UP"];
        string[] names = ["IDLE", "WALK", "ATTACK", "DAMAGE"];
        for (int column = 0; column < 4; column++) Label(headings[column], 185 + column * 170, 86, Color.Wheat);
        double sampleTime = capturePath is null ? elapsed : captureIndex * 0.05;
        for (int row = 0; row < 4; row++)
        {
            Label(names[row], 24, 143 + row * 94, Color.Wheat);
            PlayerSpriteAnimation animation = (PlayerSpriteAnimation)row;
            double time = sampleTime;
            if (animation == PlayerSpriteAnimation.Attacking) time %= 0.6;
            for (int column = 0; column < 4; column++)
            {
                batch.Draw(pixel, new Rectangle(148 + column * 170, 113 + row * 94, 146, 86), new Color(35, 53, 58));
                sprite.Draw(batch, new Vector2(206 + column * 170, 138 + row * 94), (Direction)column, animation, time);
            }
        }

        Label("LIVE SAMPLE", 24, 513, new Color(116, 182, 107));
        sprite.Draw(batch, new Vector2(199, 532), facing, selected, elapsed);
        Label("WASD FACING   1 IDLE   2 WALK", 290, 514, Color.LightGray);
        Label("Z/N ATTACK   E DAMAGE   R RESET", 290, 540, Color.LightGray);
        Label("SPACE PAUSE   Q QUIT", 290, 566, Color.LightGray);
        batch.End();
        GraphicsDevice.SetRenderTarget(null);
        batch.Begin(samplerState: SamplerState.PointClamp);
        batch.Draw(target, Vector2.Zero, Color.White);
        batch.End();

        if (capturePath is not null)
        {
            Directory.CreateDirectory(capturePath);
            using FileStream output = File.Create(Path.Combine(capturePath, $"frame-{captureIndex:D2}.png"));
            target.SaveAsPng(output, Width, Height);
            captureIndex++;
            if (captureIndex == 12) Exit();
        }

        base.Draw(gameTime);
    }

    private void VerifyRendering()
    {
        // Same inputs must render the same pixels: drawing never advances an internal clock.
        Color[] first = Sample(PlayerSpriteAnimation.Attacking, Direction.Right, 0.12);
        Color[] repeated = Sample(PlayerSpriteAnimation.Attacking, Direction.Right, 0.12);
        Require(first.SequenceEqual(repeated), "Draw changed the sampled frame.");
        foreach (PlayerSpriteAnimation animation in Enum.GetValues<PlayerSpriteAnimation>())
        {
            foreach (Direction direction in Enum.GetValues<Direction>())
            {
                Color[] a = Sample(animation, direction, 0);
                Color[] b = Sample(animation, direction, PlayerSpriteFrames.Clip(animation).FrameSeconds * 1.1);
                Require(a.Any(color => color.A != 0), "Empty animation frame.");
                Require(!a.SequenceEqual(b), $"Frames do not differ: {animation}/{direction}");
            }
        }

        // Exercise the real, unchanged starter Player's public command surface.
        Player player = new(new Vector2(100, 100), new Rectangle(0, 0, 400, 400), factory);
        player.SetMovement(Vector2.UnitX);
        player.Update(new GameTime(TimeSpan.FromSeconds(0.1), TimeSpan.FromSeconds(0.1)));
        Require(player.Facing == Direction.Right && player.Position.X > 100, "Legacy movement regression.");
        player.Attack();
        Require(player.Action == PlayerAction.Attacking, "Legacy attack regression.");
        player.TakeDamage();
        Require(player.Action == PlayerAction.Damaged && player.Health == 4, "Legacy damage regression.");
        player.SelectItem(3);
        Require(player.SelectedItem == 3, "Legacy selection regression.");
        player.Reset();
        Require(player.Position == new Vector2(100, 100) && player.Health == 5 && player.Facing == Direction.Down, "Legacy reset regression.");
        GraphicsDevice.SetRenderTarget(target);
        batch.Begin();
        player.Draw(batch);
        factory.CreatePlayerSprite().Draw(batch, Vector2.Zero, Direction.Down, false);
        factory.CreatePlayerSprite().Draw(batch, Vector2.Zero, Direction.Left, true);
        factory.CreateBlockSprite(BlockKind.Stone).Draw(batch, Vector2.Zero, Direction.Down, false);
        factory.CreateItemSprite(ItemKind.Heart).Draw(batch, Vector2.Zero, Direction.Down, false);
        factory.CreateEnemySprite(EnemyKind.Gel).Draw(batch, Vector2.Zero, Direction.Down, false);
        batch.End();
        GraphicsDevice.SetRenderTarget(null);
        Console.WriteLine("PASS: GPU draw determinism, 16 directional clips, legacy factory calls and Player command/reset smoke checks.");
    }

    private Color[] Sample(PlayerSpriteAnimation animation, Direction direction, double seconds)
    {
        GraphicsDevice.SetRenderTarget(target);
        GraphicsDevice.Clear(Color.Transparent);
        batch.Begin(samplerState: SamplerState.PointClamp);
        sprite.Draw(batch, new Vector2(50, 50), direction, animation, seconds);
        batch.End();
        GraphicsDevice.SetRenderTarget(null);
        Color[] pixels = new Color[Width * Height];
        target.GetData(pixels);
        return pixels;
    }

    private static void Require(bool condition, string message)
    {
        if (!condition) throw new InvalidOperationException(message);
    }

    private void Label(string text, int x, int y, Color color, int scale = 2)
    {
        foreach (char letter in text)
        {
            if (PixelFont.Glyphs.TryGetValue(letter, out string? pattern))
            {
                for (int i = 0; i < pattern.Length; i++)
                {
                    if (pattern[i] == '1') batch.Draw(pixel, new Rectangle(x + i % 5 * scale, y + i / 5 * scale, scale, scale), color);
                }
            }

            x += 6 * scale;
        }
    }

    protected override void UnloadContent()
    {
        target.Dispose();
        pixel.Dispose();
        batch.Dispose();
        base.UnloadContent();
    }
}
