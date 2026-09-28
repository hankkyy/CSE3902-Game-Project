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
            [Keys.Z] = new AttackCommand(player),
            [Keys.N] = new AttackCommand(player),
            [Keys.D1] = new SelectItemCommand(player, 1),
            [Keys.D2] = new SelectItemCommand(player, 2),
            [Keys.D3] = new SelectItemCommand(player, 3),
            [Keys.E] = new TakeDamageCommand(player),
            [Keys.T] = new PreviousGalleryCommand(previousBlock),
            [Keys.Y] = new NextGalleryCommand(nextBlock),
            [Keys.U] = new PreviousGalleryCommand(previousItem),
            [Keys.I] = new NextGalleryCommand(nextItem),
            [Keys.O] = new PreviousGalleryCommand(previousEnemy),
            [Keys.P] = new NextGalleryCommand(nextEnemy),
            [Keys.R] = new ResetGameCommand(reset),
            [Keys.Q] = new QuitGameCommand(quit),
            [Keys.Escape] = new QuitGameCommand(quit)
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
