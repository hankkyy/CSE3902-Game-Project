using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace GameProject;

/// <summary>Minimal shared MonoGame starting point for Sprint 1.</summary>
public sealed class Game1 : Game
{
    private readonly GraphicsDeviceManager graphics;

    public Game1()
    {
        graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Update(GameTime gameTime)
    {
        if (Keyboard.GetState().IsKeyDown(Keys.Escape) ||
            GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
        {
            Exit();
        }

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        // Sprint 1 Git practice — Xuanzhe Li: clear the screen before each frame.
        GraphicsDevice.Clear(Color.CornflowerBlue);
        base.Draw(gameTime);
    }
}
