using GameProject.Controllers;
using GameProject.Core;
using GameProject.Objects;
using GameProject.Objects.PlayerItems;
using GameProject.Sprites;
using GameProject.States;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

int checks = 0;
Rectangle bounds = new(0, 0, 1000, 1000);
Vector2 start = new(400, 400);
(Direction Facing, Vector2 Movement, Vector2 SpawnOffset)[] directions =
[
    (Direction.Up, new Vector2(0, -1), new Vector2(6, -16)),
    (Direction.Down, new Vector2(0, 1), new Vector2(6, 34)),
    (Direction.Left, new Vector2(-1, 0), new Vector2(-16, 9)),
    (Direction.Right, new Vector2(1, 0), new Vector2(28, 9))
];

foreach ((Direction facing, Vector2 movement, Vector2 spawnOffset) in directions)
{
    RecordingSprite arrowSprite = new();
    Player player = new(start, bounds, new RecordingSprite(), arrowSprite);
    player.SetMovement(movement);
    player.Update(Time(0));
    Check(player.TryFireArrow(), $"Fire while facing {facing}");
    player.Draw(null!);
    Check(Near(arrowSprite.Position, start + spawnOffset) && arrowSprite.Facing == facing,
        $"Arrow starts just outside the player, facing {facing}");
    Check(!player.TryFireArrow(), "An active arrow cannot be replaced");

    // Turning after firing must not steer an arrow already in flight.
    player.SetMovement(-movement);
    player.Update(Time(0.25));
    player.Draw(null!);
    Check(Near(arrowSprite.Position, start + spawnOffset + movement * 80),
        $"Arrow flies independently of player movement, facing {facing}");
    Check(arrowSprite.Facing == facing, "Arrow keeps its launch direction");
    player.Reset();
    Check(!player.HasActiveArrow && player.Position == start && player.SelectedItem == 1,
        "Player reset clears its arrow and restores initial player state");
}

ArrowProjectile singleStep = new(bounds, new RecordingSprite());
ArrowProjectile splitSteps = new(bounds, new RecordingSprite());
singleStep.TryLaunch(start, Direction.Right);
splitSteps.TryLaunch(start, Direction.Right);
singleStep.Update(Time(0.5));
for (int index = 0; index < 10; index++) splitSteps.Update(Time(0.05));
Check(Near(singleStep.Position, splitSteps.Position) && Near(singleStep.Position, start + new Vector2(160, 0)),
    "Equal elapsed time gives equal travel at different frame rates");
singleStep.Update(Time(0.31));
Check(!singleStep.IsActive, "Arrow expires after its lifetime");
Check(singleStep.TryLaunch(start, Direction.Up), "Expired arrow can be launched again");
singleStep.Reset();
Check(!singleStep.IsActive && singleStep.Position == Vector2.Zero && singleStep.Facing == Direction.Down,
    "Projectile reset restores all exposed state");
singleStep.TryLaunch(start, Direction.Down);
singleStep.Update(Time(0.1));
Check(singleStep.IsActive && Near(singleStep.Position, start + new Vector2(0, 32)),
    "Reset clears the previous lifetime and velocity");

Rectangle smallBounds = new(10, 20, 200, 180);
(Direction Facing, Vector2 Position)[] edges =
[
    (Direction.Up, new Vector2(50, 20)),
    (Direction.Down, new Vector2(50, 184)),
    (Direction.Left, new Vector2(10, 50)),
    (Direction.Right, new Vector2(194, 50))
];
foreach ((Direction facing, Vector2 position) in edges)
{
    RecordingSprite sprite = new();
    ArrowProjectile arrow = new(smallBounds, sprite);
    Check(arrow.TryLaunch(position, facing), $"Launch at {facing} boundary");
    arrow.Update(Time(0.01));
    arrow.Draw(null!);
    Check(!arrow.IsActive && sprite.DrawCount == 0, $"Arrow disappears before drawing outside {facing} boundary");
}

RecordingSprite recording = new();
ArrowProjectile drawnArrow = new(bounds, recording);
Check(!drawnArrow.TryLaunch(new Vector2(-1, 20), Direction.Left), "Reject launch outside the play area");
Check(!drawnArrow.TryLaunch(new Vector2(990, 20), Direction.Right), "Entire arrow must fit in the play area");
drawnArrow.TryLaunch(start, Direction.Right);
drawnArrow.Update(Time(0.1));
Vector2 beforeDrawing = drawnArrow.Position;
for (int index = 0; index < 3; index++) drawnArrow.Draw(null!);
Check(drawnArrow.IsActive && drawnArrow.Position == beforeDrawing && recording.DrawCount == 3,
    "Drawing repeatedly does not move or expire the arrow");
drawnArrow.Reset();
drawnArrow.Draw(null!);
Check(recording.DrawCount == 3, "Reset projectile is not drawn");

RecordingSprite playerVisual = new();
RecordingSprite arrowVisual = new();
Player guardedPlayer = new(start, bounds, playerVisual, arrowVisual);
guardedPlayer.Attack();
Check(!guardedPlayer.TryFireArrow(), "Sword attack blocks new arrows");
guardedPlayer.Update(Time(0.3));
Check(guardedPlayer.TryFireArrow(), "Player can shoot after sword attack ends");
guardedPlayer.TakeDamage();
guardedPlayer.Update(Time(0.15));
guardedPlayer.Draw(null!);
Check(playerVisual.DrawCount == 0 && arrowVisual.DrawCount == 1,
    "Existing arrow remains visible while damaged player flashes");
guardedPlayer.Reset();
guardedPlayer.TakeDamage();
Check(!guardedPlayer.TryFireArrow(), "Damage blocks new arrows");
guardedPlayer.Update(Time(0.5));
Check(guardedPlayer.TryFireArrow(), "Player can shoot after damage ends");
Player legacyPlayer = new(start, bounds, new RecordingSprite());
Check(!legacyPlayer.TryFireArrow(), "Original injected-sprite constructor remains usable without an arrow sprite");

// Exercise the real keyboard bindings, menu guard, dispatcher, and reset command together.
Player inputPlayer = new(start, bounds, new RecordingSprite(), new RecordingSprite());
GameSession session = new();
int quitCount = 0;
KeyboardController controller = new(inputPlayer, NoAction, NoAction, NoAction, NoAction,
    NoAction, NoAction, inputPlayer.Reset, () => quitCount++, session);
controller.Update(new KeyboardState(Keys.D1));
Check(!inputPlayer.HasActiveArrow, "Number 1 does not fire in the menu");
controller.Update(new KeyboardState(Keys.D1, Keys.Enter));
Check(session.Mode == GameMode.Playing && !inputPlayer.HasActiveArrow,
    "Held fire key does not fire when Enter starts gameplay");
controller.Update(new KeyboardState(Keys.D1));
Check(!inputPlayer.HasActiveArrow, "Held fire key stays inactive after the menu transition");
controller.Update(new KeyboardState());
controller.Update(new KeyboardState(Keys.D1));
Check(inputPlayer.HasActiveArrow, "A fresh press of actual number 1 binding fires an arrow");
inputPlayer.Update(Time(1));
controller.Update(new KeyboardState(Keys.D1));
Check(!inputPlayer.HasActiveArrow, "Holding number 1 does not auto-fire after the arrow expires");
controller.Update(new KeyboardState());
controller.Update(new KeyboardState(Keys.D1));
Check(inputPlayer.HasActiveArrow, "Release and press again fires a new arrow");
controller.Update(new KeyboardState(Keys.D1, Keys.R));
Check(!inputPlayer.HasActiveArrow, "Actual R binding clears an arrow in flight");
controller.Update(new KeyboardState(Keys.D1));
Check(!inputPlayer.HasActiveArrow, "Holding fire across reset does not create a new arrow");
controller.Update(new KeyboardState(Keys.D2));
Check(inputPlayer.SelectedItem == 2 && !inputPlayer.HasActiveArrow, "Number 2 retains slot selection");
controller.Update(new KeyboardState(Keys.D3));
Check(inputPlayer.SelectedItem == 3 && !inputPlayer.HasActiveArrow, "Number 3 retains slot selection");
controller.Update(new KeyboardState(Keys.D1));
Check(inputPlayer.SelectedItem == 1 && inputPlayer.HasActiveArrow, "Number 1 selects arrow slot and fires");
controller.Update(new KeyboardState(Keys.Q));
controller.Update(new KeyboardState(Keys.Escape));
Check(quitCount == 2, "Q and Escape still invoke quit");

Console.WriteLine($"PASS: {checks} player-arrow checks (directions, timing, bounds, reset, drawing, input and menu).");
int secondaryChecks = SecondaryItemChecks.Run();
Console.WriteLine($"PASS: {secondaryChecks} boomerang and bomb checks (return, fuse, explosion, bounds, input and reset).");

void Check(bool condition, string message)
{
    if (!condition) throw new InvalidOperationException(message);
    checks++;
}

static GameTime Time(double seconds) => new(TimeSpan.Zero, TimeSpan.FromSeconds(seconds));
static bool Near(Vector2 first, Vector2 second) => Vector2.Distance(first, second) < 0.001f;
static void NoAction() { }

sealed class RecordingSprite : ISprite
{
    public Vector2 Position { get; private set; }
    public Direction Facing { get; private set; }
    public int DrawCount { get; private set; }

    public void Draw(SpriteBatch spriteBatch, Vector2 position, Direction direction, bool alternateFrame)
    {
        Position = position;
        Facing = direction;
        DrawCount++;
    }
}
