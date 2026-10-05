using GameProject.Core;
using GameProject.Objects;
using GameProject.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

List<string> failures = [];
Vector2 startingPosition = new(100, 100);
RecordingSprite sprite = new();
Player player = new(startingPosition, new Rectangle(0, 0, 300, 300), sprite);

Check(player.Action == PlayerAction.Idle, "Player should start idle.");
Check(player.Facing == Direction.Down, "Player should start facing down.");
Check(player.Health == 5, "Player should start with five health.");
Check(player.SelectedItem == 1, "Player should start with item one selected.");

CheckFacing(player, new Vector2(-1, 0), Direction.Left);
CheckFacing(player, new Vector2(1, 0), Direction.Right);
CheckFacing(player, new Vector2(0, -1), Direction.Up);
CheckFacing(player, new Vector2(0, 1), Direction.Down);

player.SetMovement(Vector2.UnitX);
player.Update(Frame(0.1));
Check(player.Action == PlayerAction.Walking, "Movement should enter the walking state.");
Check(player.Position.X > startingPosition.X, "Movement should change the player's position.");

player.Attack();
Vector2 attackPosition = player.Position;
player.Draw(null!);
Check(player.Action == PlayerAction.Attacking, "Attack should enter the attacking state.");
Check(sprite.Position.X > attackPosition.X, "A right-facing attack should move the drawing position to the right.");
Check(!sprite.AlternateFrame, "The attack animation should start from its first frame.");

player.SetMovement(Vector2.UnitY);
player.Update(Frame(0.1));
Check(player.Position == attackPosition, "The player should not move during an attack.");

player.Attack();
Check(player.Action == PlayerAction.Attacking, "A second attack should not restart an active attack.");

player.TakeDamage();
player.Draw(null!);
Check(player.Action == PlayerAction.Damaged, "Damage should enter the damaged state.");
Check(player.Health == 4, "Damage should remove one health point.");
Check(!sprite.AlternateFrame, "The damage animation should start from its first frame.");

player.Attack();
player.TakeDamage();
Check(player.Action == PlayerAction.Damaged, "Attack should not interrupt the damaged state.");
Check(player.Health == 4, "Repeated damage should be ignored while already damaged.");

player.Update(Frame(0.5));
Check(player.Action == PlayerAction.Idle, "The damaged state should return to idle.");

player.SelectItem(0);
Check(player.SelectedItem == 1, "Item selection should not go below one.");
player.SelectItem(4);
Check(player.SelectedItem == 3, "Item selection should not go above three.");

player.SetMovement(new Vector2(10, 10));
Check(player.Facing == Direction.Down, "Equal horizontal and vertical input should use the vertical direction.");

player.Reset();
player.Draw(null!);
Check(player.Position == startingPosition, "Reset should restore the starting position.");
Check(player.Facing == Direction.Down, "Reset should restore the starting direction.");
Check(player.Action == PlayerAction.Idle, "Reset should restore the idle state.");
Check(player.Health == 5, "Reset should restore health.");
Check(player.SelectedItem == 1, "Reset should restore item selection.");
Check(!sprite.AlternateFrame, "Reset should restart the animation.");

if (failures.Count > 0)
{
    foreach (string failure in failures)
    {
        Console.Error.WriteLine($"FAIL: {failure}");
    }

    return 1;
}

Console.WriteLine("PASS: player movement, facing, states, damage guards, selection, and reset.");
return 0;

GameTime Frame(double seconds)
{
    return new GameTime(TimeSpan.Zero, TimeSpan.FromSeconds(seconds));
}

void CheckFacing(Player testPlayer, Vector2 movement, Direction expected)
{
    testPlayer.SetMovement(movement);
    Check(testPlayer.Facing == expected, $"Expected {expected} facing for movement {movement}.");
    testPlayer.Update(Frame(0));
}

void Check(bool condition, string message)
{
    if (!condition)
    {
        failures.Add(message);
    }
}

sealed class RecordingSprite : ISprite
{
    public Vector2 Position { get; private set; }
    public bool AlternateFrame { get; private set; }

    public void Draw(SpriteBatch spriteBatch, Vector2 position, Direction direction, bool alternateFrame)
    {
        Position = position;
        AlternateFrame = alternateFrame;
    }
}
