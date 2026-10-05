using GameProject.Core;
using GameProject.Sprites;
using Microsoft.Xna.Framework;

namespace PlayerSprites.Tests;

internal static class Program
{
    private static int checks;

    private static int Main(string[] args)
    {
        try
        {
            RunTimingChecks();
            if (args.Length > 0 && args[0] is "--capture" or "--visual")
            {
                string? capturePath = args[0] == "--capture" && args.Length == 2 ? args[1] : null;
                using SpriteGallery gallery = new(capturePath);
                gallery.Run();
            }

            Console.WriteLine($"PASS: {checks} headless animation checks.");
            return 0;
        }
        catch (Exception exception)
        {
            Console.Error.WriteLine(exception);
            return 1;
        }
    }

    private static void RunTimingChecks()
    {
        foreach (PlayerSpriteAnimation animation in Enum.GetValues<PlayerSpriteAnimation>())
        {
            PlayerAnimationClip clip = PlayerSpriteFrames.Clip(animation);
            Check(clip.FrameAt(0) == 0, $"{animation}: reset selects the first frame");
            for (int frame = 0; frame < clip.FrameCount; frame++)
            {
                Check(clip.FrameAt((frame + 0.5) * clip.FrameSeconds) == frame, $"{animation}: frame {frame}");
            }

            Check(clip.FrameAt(clip.FrameSeconds - 0.000001) == 0, "before frame boundary");
            Check(clip.FrameAt(clip.FrameSeconds + 0.000001) == 1, "after frame boundary");
            Check(clip.FrameAt(clip.DurationSeconds) == (clip.Loops ? 0 : clip.FrameCount - 1), "clip boundary");
            Check(clip.FrameAt(1_000_000) >= 0 && clip.FrameAt(1_000_000) < clip.FrameCount, "long session stays in range");

            // A stuttering Update and sixty smaller updates represent the same elapsed time.
            double accumulated = Enumerable.Repeat(1.0 / 60, 60).Sum();
            Check(clip.FrameAt(accumulated + 0.023) == clip.FrameAt(1.023), "frame rate independence");

            foreach (Direction direction in Enum.GetValues<Direction>())
            {
                Rectangle source = PlayerSpriteFrames.Source(animation, direction, 0.15);
                Check(source.X >= 0 && source.Y >= 0 && source.Right <= 160 && source.Bottom <= 640, "atlas bounds");
                Check(source.Width == 40 && source.Height == 40, "fixed cell size");
            }
        }

        Check(PlayerSpriteFrames.Clip(PlayerSpriteAnimation.Attacking).FrameAt(100) == 2, "attack holds its recovery frame");
        Check(PlayerSpriteFrames.Clip(PlayerSpriteAnimation.Damaged).FrameAt(0.08) == 1, "damage switches visibly");
        Check(PlayerSpriteFrames.Destination(new Vector2(100, 100)) == new Rectangle(74, 78, 80, 80), "legacy body anchor");
        Check(PlayerSpriteFrames.Destination(new Vector2(0, 0)) == new Rectangle(-26, -22, 80, 80), "anchor at origin");
        Throws(() => PlayerSpriteFrames.Clip((PlayerSpriteAnimation)99));
        Throws(() => PlayerSpriteFrames.Source(PlayerSpriteAnimation.Idle, (Direction)99, 0));
        Throws(() => PlayerSpriteFrames.Clip(PlayerSpriteAnimation.Idle).FrameAt(-1));
        Throws(() => PlayerSpriteFrames.Clip(PlayerSpriteAnimation.Idle).FrameAt(double.NaN));
        Throws(() => PlayerSpriteFrames.Clip(PlayerSpriteAnimation.Idle).FrameAt(double.PositiveInfinity));
        Throws(() => new PlayerAnimationClip(0, 0.1, true).FrameAt(0));
        Throws(() => new PlayerAnimationClip(2, 0, true).FrameAt(0));
        Throws(() => new PlayerAnimationClip(2, double.NaN, true).FrameAt(0));
    }

    private static void Throws(Action action)
    {
        try { action(); }
        catch (ArgumentOutOfRangeException) { checks++; return; }
        throw new InvalidOperationException("Expected ArgumentOutOfRangeException.");
    }

    private static void Check(bool condition, string message)
    {
        checks++;
        if (!condition) throw new InvalidOperationException(message);
    }
}
