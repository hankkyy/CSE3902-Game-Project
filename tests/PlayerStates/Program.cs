using GameProject.Core;
using GameProject.Objects;
using GameProject.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

List<string> failures = [];
Vector2 start = new(100, 100);
RecordingPlayerSprite sprite = new();
Player player = new(start, new Rectangle(0, 0, 500, 500), sprite);

player.Draw(null!);
Check(sprite.Animation == PlayerSpriteAnimation.Idle, "Player did not begin with the idle clip");

player.SetMovement(Vector2.UnitX);
player.Update(Frame(0.1));
player.Draw(null!);
Check(player.Action == PlayerAction.Walking, "Movement did not enter Walking");
Check(player.Facing == Direction.Right, "Movement did not update facing");
Check(player.Position.X > start.X, "Movement did not change position");
Check(sprite.Animation == PlayerSpriteAnimation.Walking, "Walking did not select the walking clip");

player.Attack();
Vector2 attackStart = player.Position;
player.Draw(null!);
Check(player.Action == PlayerAction.Attacking, "Attack did not enter Attacking");
Check(sprite.Animation == PlayerSpriteAnimation.Attacking, "Attack did not select the attack clip");
Check(sprite.Position.X > attackStart.X, "Right-facing attack did not show its directional lunge");

player.SetMovement(Vector2.UnitY);
player.Update(Frame(0.1));
Check(player.Position == attackStart, "Player moved while attacking");
player.Attack();
Check(player.Action == PlayerAction.Attacking, "Overlapping attack changed the active state");

player.TakeDamage();
player.Draw(null!);
Check(player.Action == PlayerAction.Damaged, "Damage did not enter Damaged");
Check(player.Health == 4, "Damage did not remove exactly one health point");
Check(sprite.Animation == PlayerSpriteAnimation.Damaged, "Damage did not select the damaged clip");

player.Attack();
player.TakeDamage();
Check(player.Action == PlayerAction.Damaged, "Attack interrupted the damage state");
Check(player.Health == 4, "Repeated damage bypassed the damage-state guard");

player.Update(Frame(0.5));
Check(player.Action == PlayerAction.Idle, "Timed damage state did not return to Idle");

player.SelectItem(3);
player.Reset();
player.Draw(null!);
Check(player.Position == start, "Reset did not restore position");
Check(player.Facing == Direction.Down, "Reset did not restore facing");
Check(player.Health == 5, "Reset did not restore health");
Check(player.SelectedItem == 1, "Reset did not restore item selection");
Check(player.Action == PlayerAction.Idle, "Reset did not restore Idle");
Check(sprite.Animation == PlayerSpriteAnimation.Idle, "Reset did not restore the idle clip");
Check(sprite.ElapsedSeconds == 0, "Reset did not restart the visual-state clock");

if (failures.Count > 0)
{
    foreach (string failure in failures)
    {
        Console.Error.WriteLine($"FAIL: {failure}");
    }

    return 1;
}

Console.WriteLine("PASS: player states, guards, animation integration, and reset.");
return 0;

GameTime Frame(double seconds) => new(TimeSpan.Zero, TimeSpan.FromSeconds(seconds));

void Check(bool condition, string message)
{
    if (!condition)
    {
        failures.Add(message);
    }
}

sealed class RecordingPlayerSprite : IAnimatedPlayerSprite
{
    public Vector2 Position { get; private set; }
    public PlayerSpriteAnimation Animation { get; private set; }
    public double ElapsedSeconds { get; private set; }

    public void Draw(SpriteBatch spriteBatch, Vector2 position, Direction direction, bool alternateFrame)
    {
        Position = position;
        Animation = alternateFrame ? PlayerSpriteAnimation.Walking : PlayerSpriteAnimation.Idle;
        ElapsedSeconds = 0;
    }

    public void Draw(SpriteBatch spriteBatch, Vector2 position, Direction direction,
        PlayerSpriteAnimation animation, double elapsedSeconds)
    {
        Position = position;
        Animation = animation;
        ElapsedSeconds = elapsedSeconds;
    }
}
