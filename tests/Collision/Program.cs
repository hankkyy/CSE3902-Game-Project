using GameProject.Collision;
using GameProject.Core;
using GameProject.Objects;
using GameProject.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

List<string> failures = [];
CollisionDispatcher dispatcher = new([new PlayerSolidCollisionHandler()]);
CollisionSystem collisionSystem = new(dispatcher);
Rectangle playArea = new(0, 0, 400, 300);

CheckRightCollision();
CheckLeftCollision();
CheckDownCollision();
CheckUpCollision();
CheckDiagonalCorner();
CheckLargeElapsedStep();
CheckReversedPairOrder();
CheckInactiveAndUniquePairs();
CheckUnsupportedPair();
CheckSpawnedOverlap();
CheckReset();

if (failures.Count > 0)
{
    foreach (string failure in failures)
    {
        Console.Error.WriteLine($"FAIL: {failure}");
    }

    return 1;
}

Console.WriteLine("PASS: collision detection, dispatch, four directions, corners, elapsed steps, inactive objects and reset.");
return 0;

void CheckRightCollision()
{
    Player player = CreatePlayer(new Vector2(70, 100));
    SolidCollider wall = new(new Rectangle(100, 80, 30, 80));
    MoveAndProcess(player, wall, Vector2.UnitX, 0.2);
    Check(player.Hitbox.Right == wall.Hitbox.Left, "Right movement did not stop at the wall");
    Check(!player.Hitbox.Intersects(wall.Hitbox), "Right collision left the player intersecting the wall");
}

void CheckLeftCollision()
{
    Player player = CreatePlayer(new Vector2(132, 100));
    SolidCollider wall = new(new Rectangle(100, 80, 30, 80));
    MoveAndProcess(player, wall, -Vector2.UnitX, 0.2);
    Check(player.Hitbox.Left == wall.Hitbox.Right, "Left movement did not stop at the wall");
}

void CheckDownCollision()
{
    Player player = CreatePlayer(new Vector2(100, 44));
    SolidCollider wall = new(new Rectangle(80, 80, 80, 30));
    MoveAndProcess(player, wall, Vector2.UnitY, 0.2);
    Check(player.Hitbox.Bottom == wall.Hitbox.Top, "Down movement did not stop above the wall");
}

void CheckUpCollision()
{
    Player player = CreatePlayer(new Vector2(100, 112));
    SolidCollider wall = new(new Rectangle(80, 80, 80, 30));
    MoveAndProcess(player, wall, -Vector2.UnitY, 0.2);
    Check(player.Hitbox.Top == wall.Hitbox.Bottom, "Up movement did not stop below the wall");
}

void CheckDiagonalCorner()
{
    Player player = CreatePlayer(new Vector2(70, 44));
    SolidCollider wall = new(new Rectangle(100, 80, 40, 40));
    MoveAndProcess(player, wall, Vector2.One, 0.2);
    Check(!player.Hitbox.Intersects(wall.Hitbox), "Diagonal corner collision was not resolved");
}

void CheckLargeElapsedStep()
{
    Player player = CreatePlayer(new Vector2(20, 100));
    SolidCollider thickWall = new(new Rectangle(100, 70, 160, 100));
    MoveAndProcess(player, thickWall, Vector2.UnitX, 0.6);
    Check(player.Hitbox.Right == thickWall.Hitbox.Left, "A large elapsed step crossed the solid face");
}

void CheckReversedPairOrder()
{
    Player player = CreatePlayer(new Vector2(70, 100));
    SolidCollider wall = new(new Rectangle(100, 80, 30, 80));
    player.SetMovement(Vector2.UnitX);
    player.Update(Frame(0.2));
    int handled = collisionSystem.Process([wall, player]);
    Check(handled == 1, "Reversed player-solid order was not handled");
    Check(!player.Hitbox.Intersects(wall.Hitbox), "Reversed order did not resolve overlap");
}

void CheckInactiveAndUniquePairs()
{
    Player player = CreatePlayer(new Vector2(100, 100));
    SolidCollider active = new(new Rectangle(100, 100, 30, 30));
    SolidCollider inactive = new(new Rectangle(100, 100, 30, 30));
    inactive.SetActive(false);

    IReadOnlyList<CollisionPair> pairs = CollisionDetector.FindCollisions([player, active, inactive]);
    Check(pairs.Count == 1, "Detector did not skip inactive objects or emitted a duplicate pair");
}

void CheckUnsupportedPair()
{
    SolidCollider first = new(new Rectangle(10, 10, 20, 20));
    SolidCollider second = new(new Rectangle(15, 15, 20, 20));
    int handled = collisionSystem.Process([first, second]);
    Check(handled == 0, "Unsupported solid-solid collision should remain unhandled");
}

void CheckSpawnedOverlap()
{
    Player player = CreatePlayer(new Vector2(105, 105));
    SolidCollider wall = new(new Rectangle(100, 100, 40, 50));
    int handled = collisionSystem.Process([player, wall]);
    Check(handled == 1, "Spawned overlap was not dispatched");
    Check(!player.Hitbox.Intersects(wall.Hitbox), "Smallest-overlap fallback did not separate the player");
}

void CheckReset()
{
    Vector2 start = new(70, 100);
    Player player = CreatePlayer(start);
    SolidCollider wall = new(new Rectangle(100, 80, 30, 80));
    MoveAndProcess(player, wall, Vector2.UnitX, 0.2);
    player.Reset();
    Check(player.Position == start, "Reset did not restore the player start position");
    Check(player.PreviousPosition == start, "Reset did not restore the previous collision position");
    Check(player.Hitbox == player.PreviousHitbox, "Reset left hitbox history inconsistent");
}

void MoveAndProcess(Player player, SolidCollider solid, Vector2 movement, double seconds)
{
    player.SetMovement(movement);
    player.Update(Frame(seconds));
    int handled = collisionSystem.Process([player, solid]);
    Check(handled == 1, $"Expected one handled collision while moving {movement}");
}

Player CreatePlayer(Vector2 position) => new(position, playArea, new EmptySprite());

GameTime Frame(double seconds) => new(TimeSpan.Zero, TimeSpan.FromSeconds(seconds));

void Check(bool condition, string message)
{
    if (!condition)
    {
        failures.Add(message);
    }
}

sealed class EmptySprite : ISprite
{
    public void Draw(SpriteBatch spriteBatch, Vector2 position, Direction direction, bool alternateFrame)
    {
    }
}
