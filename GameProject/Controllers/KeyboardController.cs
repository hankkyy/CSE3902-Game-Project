using GameProject.Commands;
using GameProject.Objects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace GameProject.Controllers;

/// <summary>Maps keyboard edges to commands and held keys to continuous movement.</summary>
public sealed class KeyboardController
{
    private readonly Player player;
    private readonly Dictionary<Keys, ICommand> pressedCommands;
    private KeyboardState previousState;

    public KeyboardController(
        Player player,
        Action previousBlock, Action nextBlock,
        Action previousItem, Action nextItem,
        Action previousEnemy, Action nextEnemy,
        Action reset, Action quit)
    {
        this.player = player;
        pressedCommands = new Dictionary<Keys, ICommand>
        {
            [Keys.Z] = new ActionCommand(player.Attack),
            [Keys.N] = new ActionCommand(player.Attack),
            [Keys.D1] = new ActionCommand(() => player.SelectItem(1)),
            [Keys.D2] = new ActionCommand(() => player.SelectItem(2)),
            [Keys.D3] = new ActionCommand(() => player.SelectItem(3)),
            [Keys.E] = new ActionCommand(player.TakeDamage),
            [Keys.T] = new ActionCommand(previousBlock),
            [Keys.Y] = new ActionCommand(nextBlock),
            [Keys.U] = new ActionCommand(previousItem),
            [Keys.I] = new ActionCommand(nextItem),
            [Keys.O] = new ActionCommand(previousEnemy),
            [Keys.P] = new ActionCommand(nextEnemy),
            [Keys.R] = new ActionCommand(reset),
            [Keys.Q] = new ActionCommand(quit),
            [Keys.Escape] = new ActionCommand(quit)
        };
    }

    public void Update()
    {
        KeyboardState current = Keyboard.GetState();
        Vector2 movement = Vector2.Zero;
        if (current.IsKeyDown(Keys.Left) || current.IsKeyDown(Keys.A)) movement.X -= 1;
        if (current.IsKeyDown(Keys.Right) || current.IsKeyDown(Keys.D)) movement.X += 1;
        if (current.IsKeyDown(Keys.Up) || current.IsKeyDown(Keys.W)) movement.Y -= 1;
        if (current.IsKeyDown(Keys.Down) || current.IsKeyDown(Keys.S)) movement.Y += 1;
        player.SetMovement(movement);

        foreach ((Keys key, ICommand command) in pressedCommands)
        {
            if (current.IsKeyDown(key) && previousState.IsKeyUp(key)) command.Execute();
        }
        previousState = current;
    }
}
