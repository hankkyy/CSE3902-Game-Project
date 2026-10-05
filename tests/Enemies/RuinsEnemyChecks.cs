using GameProject.Core;
using GameProject.Objects;
using GameProject.Objects.Enemies;
using GameProject.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

internal static class RuinsEnemyChecks
{
    public static int Run()
    {
        int checks = 0;
        Vector2 start = new(700, 420);
        EnemyCharacter[] Make(Vector2 position, ISprite sprite) =>
        [
            new RuneWisp("Rune Wisp", position, sprite),
            new ClockworkBeetle("Clockwork Beetle", position, sprite),
            new PrismSentinel("Prism Sentinel", position, sprite, sprite)
        ];

        RecordingSprite visual = new();
        EnemyCharacter[] characters = Make(start, visual);
        foreach (EnemyCharacter character in characters)
        {
            bool staysInside = true;
            for (int index = 0; index < 12000; index++)
            {
                character.Update(Step(0.01));
                staysInside &= character.Position.X >= 672 && character.Position.X + 32 <= 768 &&
                    character.Position.Y >= 404 && character.Position.Y + 32 <= 452;
                if (character is PrismSentinel sentinel && sentinel.Projectile.IsActive)
                    staysInside &= sentinel.Projectile.Position.X >= 628 && sentinel.Projectile.Position.X + 8 <= 800 &&
                        sentinel.Projectile.Position.Y == 430;
            }
            Check(staysInside, $"{character.Name} and its shots stay in their gallery envelope for 120 seconds");
            character.Reset();
            Check(character.Position == start && character.Facing == Direction.Right && !character.AlternateFrame,
                "New enemy resets position, facing and animation");
            character.Update(Step(0.2));
            Vector2 beforeDraw = character.Position;
            visual.Calls.Clear();
            character.Draw(null!);
            character.Draw(null!);
            Check(character.Position == beforeDraw && visual.Calls.Count == 2, "Drawing does not advance new enemy simulation");
        }

        EnemyCharacter[] smallSteps = Make(start, visual);
        EnemyCharacter[] largeStep = Make(start, visual);
        for (int index = 0; index < smallSteps.Length; index++)
        {
            for (int frame = 0; frame < 425; frame++) smallSteps[index].Update(Step(0.01));
            largeStep[index].Update(Step(4.25));
            Check(Near(smallSteps[index].Position, largeStep[index].Position) && smallSteps[index].Facing == largeStep[index].Facing,
                "New enemy movement and facing are frame independent");
        }
        PrismSentinel smallSentinel = (PrismSentinel)smallSteps[2];
        PrismSentinel largeSentinel = (PrismSentinel)largeStep[2];
        Check(smallSentinel.Projectile.IsActive && largeSentinel.Projectile.IsActive &&
            Near(smallSentinel.Projectile.Position, largeSentinel.Projectile.Position), "Sentinel shot is frame independent");

        ClockworkBeetle beetle = new("Beetle", start, visual);
        (double Time, Direction Facing, Vector2 Offset)[] patrolSamples =
        [
            (0.5, Direction.Right, new Vector2(16, 0)),
            (1.25, Direction.Up, new Vector2(36, -4)),
            (2, Direction.Left, new Vector2(24, -16)),
            (3, Direction.Down, new Vector2(0, -8)),
            (3.25, Direction.Right, Vector2.Zero)
        ];
        foreach ((double time, Direction facing, Vector2 offset) in patrolSamples)
        {
            beetle.Reset();
            beetle.Update(Step(time));
            Check(beetle.Facing == facing && Near(beetle.Position, start + offset), "Beetle follows its four-sided patrol and completes a loop");
        }

        RecordingSprite body = new();
        RecordingSprite bolt = new();
        PrismSentinel guard = new("Guard", start, body, bolt);
        guard.Update(Step(1.2));
        guard.Draw(null!);
        Check(guard.IsCharging && !guard.Projectile.IsActive && body.Calls[^1].Alternate, "Sentinel visibly charges before firing");
        guard.Update(Step(0.8));
        Check(!guard.IsCharging && guard.Projectile.IsActive && guard.Facing == Direction.Right &&
            guard.Projectile.Position == start + new Vector2(32, 10), "First shot starts on the right at two seconds");
        guard.Update(Step(0.61));
        Check(!guard.Projectile.IsActive, "Shot expires before crossing the panel boundary");
        guard.Update(Step(1.39));
        Check(guard.Projectile.IsActive && guard.Facing == Direction.Left &&
            guard.Projectile.Position == start + new Vector2(-12, 10), "Next shot starts on the left at four seconds");
        guard.Reset();
        Check(!guard.IsCharging && !guard.Projectile.IsActive, "Reset removes charge and shot");
        guard.Update(Step(1.99));
        Check(!guard.Projectile.IsActive, "Reset restarts the full shot interval");

        // Exercise every EnemyObject switch case and the same paired-gallery structure as Game1.
        EnemyKind[] kinds = Enum.GetValues<EnemyKind>();
        Check(kinds.Length == 7, "Designed roster contains seven enemy/NPC kinds");
        List<RecordingSprite> previews = [];
        List<RecordingSprite> playArea = [];
        List<EnemyDisplayPair> entries = [];
        Vector2 mainStart = new(370, 450);
        foreach (EnemyKind kind in kinds)
        {
            RecordingSprite preview = new();
            RecordingSprite main = new();
            previews.Add(preview);
            playArea.Add(main);
            entries.Add(new EnemyDisplayPair(
                new EnemyObject(kind.ToString(), start, kind, preview, preview),
                new EnemyObject(kind.ToString(), mainStart, kind, main, main)));
        }
        ObjectGallery<EnemyDisplayPair> gallery = new(entries);
        for (int index = 0; index < gallery.Count; index++)
        {
            // Sample while both shooters' projectiles are still inside their display envelopes.
            gallery.Current.Update(Step(2.1));
            gallery.Current.Draw(null!);
            bool matching = previews[index].Calls.Count == playArea[index].Calls.Count;
            bool inside = true;
            for (int call = 0; call < previews[index].Calls.Count; call++)
            {
                var first = previews[index].Calls[call];
                var second = playArea[index].Calls[call];
                matching &= Near(first.Position - start, second.Position - mainStart) &&
                    first.Facing == second.Facing && first.Alternate == second.Alternate;
                inside &= second.Position.X >= 16 && second.Position.X + 42 <= 592 &&
                    second.Position.Y >= 58 && second.Position.Y + 43 <= 510;
            }
            int expectedCalls = kinds[index] is EnemyKind.Octorok or EnemyKind.PrismSentinel ? 2 : 1;
            Check(matching && previews[index].Calls.Count == expectedCalls,
                $"Paired {kinds[index]} rendering: matching={matching}, draws={previews[index].Calls.Count}, expected={expectedCalls}");
            Check(inside, "Paired main-area enemy and shot fit inside the play area");
            gallery.Next();
        }
        Check(gallery.Index == 0, "All seven entries wrap forwards");
        gallery.Previous();
        Check(gallery.Index == gallery.Count - 1, "All seven entries wrap backwards");
        gallery.Reset();
        Check(gallery.Index == 0, "Gallery resets selection");
        for (int index = 0; index < gallery.Count; index++)
        {
            previews[index].Calls.Clear();
            playArea[index].Calls.Clear();
            gallery.Current.Draw(null!);
            Check(previews[index].Calls.Count == 1 && playArea[index].Calls.Count == 1 &&
                previews[index].Calls[0].Position == start && playArea[index].Calls[0].Position == mainStart &&
                !previews[index].Calls[0].Alternate, "Reset also restores hidden entries and clears their shots");
            gallery.Next();
        }
        return checks;

        void Check(bool condition, string message)
        {
            if (!condition) throw new InvalidOperationException(message);
            checks++;
        }
    }

    // Construct the requested decimal duration at the nearest TimeSpan tick.
    private static GameTime Step(double seconds) => new(TimeSpan.Zero,
        TimeSpan.FromTicks((long)Math.Round(seconds * TimeSpan.TicksPerSecond)));
    private static bool Near(Vector2 first, Vector2 second) => Vector2.Distance(first, second) < 0.001f;

    private sealed class RecordingSprite : ISprite
    {
        public List<(Vector2 Position, Direction Facing, bool Alternate)> Calls { get; } = [];
        public void Draw(SpriteBatch spriteBatch, Vector2 position, Direction direction, bool alternateFrame) =>
            Calls.Add((position, direction, alternateFrame));
    }
}
