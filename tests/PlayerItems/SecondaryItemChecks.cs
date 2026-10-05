using GameProject.Controllers;
using GameProject.Core;
using GameProject.Objects;
using GameProject.Objects.PlayerItems;
using GameProject.Sprites;
using GameProject.States;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

internal static class SecondaryItemChecks
{
    public static int Run()
    {
        int checks = 0;
        Rectangle bounds = new(0, 0, 1000, 1000);
        Vector2 start = new(400, 400);
        (Direction Facing, Vector2 Movement)[] directions =
        [
            (Direction.Up, new Vector2(0, -1)),
            (Direction.Down, new Vector2(0, 1)),
            (Direction.Left, new Vector2(-1, 0)),
            (Direction.Right, new Vector2(1, 0))
        ];

        foreach ((Direction facing, Vector2 movement) in directions)
        {
            RecordingSprite visual = new();
            BoomerangProjectile boomerang = new(bounds, visual, () => start);
            Check(boomerang.TryLaunch(start, facing), $"Boomerang launches {facing}");
            Check(!boomerang.TryLaunch(start, facing), "Cannot replace an active boomerang");
            boomerang.Update(Time(0.25));
            Check(Near(boomerang.Position, start + movement * 60), $"Outbound travel {facing}");
            Vector2 beforeDraw = boomerang.Position;
            boomerang.Draw(null!);
            boomerang.Draw(null!);
            Check(visual.DrawCount == 2 && boomerang.Position == beforeDraw && !boomerang.IsReturning,
                "Boomerang drawing is pure");
            boomerang.Update(Time(0.5));
            Check(boomerang.IsReturning && boomerang.IsActive &&
                Near(boomerang.Position, start + movement * (140 - 340 / 6f)),
                "Frame crossing turn point uses leftover time for the return trip");
            boomerang.Update(Time(0.5));
            Check(!boomerang.IsActive && Near(boomerang.Position, start), "Boomerang reaches its owner");
            Check(boomerang.TryLaunch(start, facing), "Caught boomerang can be thrown again");
            boomerang.Reset();
            Check(!boomerang.IsActive && !boomerang.IsReturning && boomerang.Position == Vector2.Zero,
                "Boomerang reset clears both phases and position");
        }

        Vector2 movingTarget = start;
        BoomerangProjectile homing = new(bounds, new RecordingSprite(), () => movingTarget);
        homing.TryLaunch(start, Direction.Right);
        homing.Update(Time(0.6));
        movingTarget = new Vector2(300, 550);
        float oldDistance = Vector2.Distance(homing.Position, movingTarget);
        homing.Update(Time(0.2));
        Check(homing.Position.Y > start.Y && Vector2.Distance(homing.Position, movingTarget) < oldDistance,
            "Returning boomerang follows a moved owner rather than the launch position");
        homing.Update(Time(1));
        Check(!homing.IsActive && Near(homing.Position, movingTarget), "Boomerang catches a moved owner");

        BoomerangProjectile wholeFrame = new(bounds, new RecordingSprite(), () => start);
        BoomerangProjectile splitFrames = new(bounds, new RecordingSprite(), () => start);
        wholeFrame.TryLaunch(start, Direction.Right);
        splitFrames.TryLaunch(start, Direction.Right);
        wholeFrame.Update(Time(0.75));
        for (int index = 0; index < 15; index++) splitFrames.Update(Time(0.05));
        Check(Near(wholeFrame.Position, splitFrames.Position) && wholeFrame.IsReturning && splitFrames.IsReturning,
            "Static-target round trip is independent of frame partition");
        wholeFrame.Update(Time(10));
        Check(!wholeFrame.IsActive, "A large elapsed time cannot leave a permanent boomerang");
        Check(!wholeFrame.TryLaunch(new Vector2(-1, 0), Direction.Up), "Reject boomerang launch outside bounds");

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
            BoomerangProjectile boomerang = new(smallBounds, new RecordingSprite(), () => new Vector2(100, 100));
            boomerang.TryLaunch(position, facing);
            boomerang.Update(Time(0.05));
            Check(boomerang.IsReturning && Inside(boomerang.Position, 16, smallBounds), $"Turn at {facing} area edge");
        }

        RecordingSprite bombVisual = new();
        RecordingSprite explosionVisual = new();
        PlacedBomb bomb = new(bounds, bombVisual, explosionVisual);
        Check(bomb.TryPlace(start), "Bomb can be placed");
        Check(!bomb.TryPlace(start + Vector2.One), "Cannot replace a bomb with a running fuse");
        bomb.Draw(null!);
        bomb.Update(Time(0.15));
        bomb.Draw(null!);
        Check(bombVisual.Alternate && bomb.Position == start, "Fuse animates while bomb remains stationary");
        bomb.Update(Time(1.04));
        Check(bomb.IsActive && !bomb.IsExploding, "Fuse still runs just before 1.2 seconds");
        bomb.Update(Time(0.02));
        Check(bomb.IsExploding, "Bomb enters explosion after fuse ends");
        bomb.Draw(null!);
        bomb.Draw(null!);
        Check(explosionVisual.DrawCount == 2 && bombVisual.DrawCount == 2 && bomb.Position == start,
            "Explosion uses its own sprite and repeated drawing does not advance time");
        Check(!bomb.TryPlace(start), "Cannot replace an exploding bomb");
        bomb.Update(Time(0.3));
        Check(bomb.IsExploding, "Explosion remains visible for its duration");
        bomb.Update(Time(0.05));
        bomb.Draw(null!);
        Check(!bomb.IsActive && !bomb.IsExploding && explosionVisual.DrawCount == 2, "Explosion ends and stops drawing");
        Check(bomb.TryPlace(start), "A new bomb can be placed after the explosion");
        bomb.Reset();
        Check(!bomb.IsActive && bomb.Position == Vector2.Zero, "Reset clears bomb during fuse");
        bomb.TryPlace(start);
        bomb.Update(Time(1.25));
        bomb.Reset();
        Check(!bomb.IsActive && !bomb.IsExploding, "Reset also clears explosion phase");
        bomb.TryPlace(start);
        bomb.Update(Time(0.1));
        Check(!bomb.IsExploding && bomb.IsActive, "Reset restores a fresh fuse for the next bomb");

        foreach (Vector2 position in new[] { new Vector2(10, 20), new Vector2(194, 20), new Vector2(10, 184), new Vector2(194, 184) })
        {
            RecordingSprite explosion = new();
            PlacedBomb cornerBomb = new(smallBounds, new RecordingSprite(), explosion);
            Check(cornerBomb.TryPlace(position), "Place bomb in play-area corner");
            cornerBomb.Update(Time(1.25));
            cornerBomb.Draw(null!);
            Check(explosion.DrawCount == 1 && Inside(explosion.Position, 48, smallBounds),
                "Entire explosion stays inside the play area");
        }

        PlacedBomb largeFrame = new(bounds, new RecordingSprite(), new RecordingSprite());
        PlacedBomb smallFrames = new(bounds, new RecordingSprite(), new RecordingSprite());
        largeFrame.TryPlace(start);
        smallFrames.TryPlace(start);
        largeFrame.Update(Time(1.3));
        for (int index = 0; index < 13; index++) smallFrames.Update(Time(0.1));
        Check(largeFrame.IsExploding && smallFrames.IsExploding, "Bomb phase depends on elapsed time, not frame count");
        largeFrame.Update(Time(10));
        Check(!largeFrame.IsActive, "Large elapsed time finishes both fuse and explosion");
        Check(!largeFrame.TryPlace(new Vector2(999, 999)), "Reject out-of-bounds bomb placement");

        foreach ((Direction facing, Vector2 movement) in directions)
        {
            RecordingSprite boomerangSprite = new();
            RecordingSprite placedSprite = new();
            Player player = new(start, bounds, new RecordingSprite(), new RecordingSprite(),
                boomerangSprite, placedSprite, new RecordingSprite());
            player.SetMovement(movement);
            player.Update(Time(0));
            Check(player.TryThrowBoomerang() && player.SelectedItem == 2, $"Player throws boomerang {facing}");
            Check(player.TryPlaceBomb() && player.SelectedItem == 3, $"Player places bomb {facing}");
            player.Draw(null!);
            Vector2 placedAt = placedSprite.Position;
            Vector2 expected = facing switch
            {
                Direction.Up => start + new Vector2(6, -16),
                Direction.Down => start + new Vector2(6, 34),
                Direction.Left => start + new Vector2(-16, 9),
                _ => start + new Vector2(28, 9)
            };
            Check(Near(placedAt, expected) && Near(boomerangSprite.Position, expected), "Both items spawn in front of the player");
            player.SetMovement(-movement);
            player.Update(Time(0.1));
            player.Draw(null!);
            Check(placedSprite.Position == placedAt && player.Position != start, "Placed bomb does not follow walking player");
            player.Reset();
            Check(!player.HasActiveBomb && !player.HasActiveBoomerang && player.SelectedItem == 1, "Player reset clears both items");
            player.Attack();
            Check(!player.TryThrowBoomerang() && !player.TryPlaceBomb(), "Sword attack blocks both new items");
            player.Reset();
            player.TakeDamage();
            Check(!player.TryThrowBoomerang() && !player.TryPlaceBomb(), "Damage blocks both new items");
            player.Update(Time(0.5));
            Check(player.TryThrowBoomerang() && player.TryPlaceBomb(), "Both items work after damage ends");
        }

        foreach (Keys key in new[] { Keys.D2, Keys.D3 })
        {
            Player player = MakePlayer(start, bounds);
            GameSession session = new();
            KeyboardController controller = new(player, NoAction, NoAction, NoAction, NoAction,
                NoAction, NoAction, player.Reset, NoAction, session);
            bool Active() => key == Keys.D2 ? player.HasActiveBoomerang : player.HasActiveBomb;
            controller.Update(new KeyboardState(key));
            Check(!Active(), $"{key} is blocked in menu");
            controller.Update(new KeyboardState(key, Keys.Enter));
            controller.Update(new KeyboardState(key));
            Check(session.Mode == GameMode.Playing && !Active(), $"Held {key} does not leak into gameplay");
            controller.Update(new KeyboardState());
            controller.Update(new KeyboardState(key));
            Check(Active(), $"Fresh {key} press uses the actual item binding");
            player.Update(Time(3));
            controller.Update(new KeyboardState(key));
            Check(!Active(), $"Holding {key} does not auto-repeat after effect ends");
            controller.Update(new KeyboardState());
            controller.Update(new KeyboardState(key));
            Check(Active(), $"Releasing and pressing {key} again works");
            controller.Update(new KeyboardState(key, Keys.R));
            Check(!Active(), $"R clears active {key} item");
            controller.Update(new KeyboardState(key));
            Check(!Active(), $"Held {key} stays inactive across reset");
        }

        Player allItems = MakePlayer(start, bounds);
        KeyboardController allBindings = new(allItems, NoAction, NoAction, NoAction, NoAction,
            NoAction, NoAction, allItems.Reset, NoAction);
        allBindings.Update(new KeyboardState(Keys.D1, Keys.D2, Keys.D3));
        Check(allItems.HasActiveArrow && allItems.HasActiveBoomerang && allItems.HasActiveBomb,
            "All three actual bindings create separate item effects");
        allBindings.Update(new KeyboardState(Keys.R));
        Check(!allItems.HasActiveArrow && !allItems.HasActiveBoomerang && !allItems.HasActiveBomb,
            "R clears all three effects together");
        return checks;

        void Check(bool condition, string message)
        {
            if (!condition) throw new InvalidOperationException(message);
            checks++;
        }
    }

    private static Player MakePlayer(Vector2 position, Rectangle bounds) => new(position, bounds,
        new RecordingSprite(), new RecordingSprite(), new RecordingSprite(), new RecordingSprite(), new RecordingSprite());

    private static GameTime Time(double seconds) => new(TimeSpan.Zero, TimeSpan.FromSeconds(seconds));
    private static bool Near(Vector2 first, Vector2 second) => Vector2.Distance(first, second) < 0.001f;
    private static bool Inside(Vector2 position, int size, Rectangle bounds) => position.X >= bounds.Left &&
        position.Y >= bounds.Top && position.X + size <= bounds.Right && position.Y + size <= bounds.Bottom;
    private static void NoAction() { }

    private sealed class RecordingSprite : ISprite
    {
        public Vector2 Position { get; private set; }
        public bool Alternate { get; private set; }
        public int DrawCount { get; private set; }

        public void Draw(SpriteBatch spriteBatch, Vector2 position, Direction direction, bool alternateFrame)
        {
            Position = position;
            Alternate = alternateFrame;
            DrawCount++;
        }
    }
}
