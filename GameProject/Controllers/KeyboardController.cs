using GameProject.Commands;
using GameProject.Objects;
using GameProject.States;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace GameProject.Controllers;

/// <summary>Maps keyboard edges to commands and held keys to continuous movement.</summary>
public sealed class KeyboardController
{
    private readonly Player player;
    private readonly GameSession? session;
    private readonly KeyPressDispatcher keyPressDispatcher;

    public KeyboardController(
        Player player,
        Action previousBlock, Action nextBlock,
        Action previousItem, Action nextItem,
        Action previousEnemy, Action nextEnemy,
        Action reset, Action quit,
        GameSession? session = null)
    {
        this.player = player;
        this.session = session;
        Dictionary<Keys, ICommand> pressedCommands = new()
        {
            [Keys.Z] = new AttackCommand(player),
            [Keys.N] = new AttackCommand(player),
            [Keys.D1] = new FireArrowCommand(player),
            [Keys.D2] = new ThrowBoomerangCommand(player),
            [Keys.D3] = new PlaceBombCommand(player),
            [Keys.E] = new TakeDamageCommand(player),
            [Keys.T] = new PreviousGalleryCommand(previousBlock),
            [Keys.Y] = new NextGalleryCommand(nextBlock),
            [Keys.U] = new PreviousGalleryCommand(previousItem),
            [Keys.I] = new NextGalleryCommand(nextItem),
            [Keys.O] = new PreviousGalleryCommand(previousEnemy),
            [Keys.P] = new NextGalleryCommand(nextEnemy),
            [Keys.R] = new ResetGameCommand(reset)
        };

        if (session is not null)
        {
            foreach (Keys key in pressedCommands.Keys.ToArray())
            {
                pressedCommands[key] = new GameplayCommand(session, pressedCommands[key]);
            }

            pressedCommands[Keys.Enter] = new StartGameCommand(session);
        }

        pressedCommands[Keys.Q] = new QuitGameCommand(quit);
        pressedCommands[Keys.Escape] = new QuitGameCommand(quit);
        keyPressDispatcher = new KeyPressDispatcher(pressedCommands);
    }

    public void Update() => Update(Keyboard.GetState());

    /// <summary>Accepts one keyboard snapshot so actual bindings can be tested without a window.</summary>
    public void Update(KeyboardState current)
    {
        Vector2 movement = Vector2.Zero;
        if (session is null || session.Mode == GameMode.Playing)
        {
            if (current.IsKeyDown(Keys.Left) || current.IsKeyDown(Keys.A)) movement.X -= 1;
            if (current.IsKeyDown(Keys.Right) || current.IsKeyDown(Keys.D)) movement.X += 1;
            if (current.IsKeyDown(Keys.Up) || current.IsKeyDown(Keys.W)) movement.Y -= 1;
            if (current.IsKeyDown(Keys.Down) || current.IsKeyDown(Keys.S)) movement.Y += 1;
        }

        player.SetMovement(movement);

        keyPressDispatcher.Update(current);
    }
}
