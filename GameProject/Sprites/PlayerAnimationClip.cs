namespace GameProject.Sprites;

/// <summary>A deterministic clip sampled using seconds supplied by the game update.</summary>
public readonly record struct PlayerAnimationClip(int FrameCount, double FrameSeconds, bool Loops)
{
    public double DurationSeconds => FrameCount * FrameSeconds;

    public int FrameAt(double elapsedSeconds)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(FrameCount);
        if (!double.IsFinite(FrameSeconds) || FrameSeconds <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(FrameSeconds));
        }

        if (!double.IsFinite(elapsedSeconds) || elapsedSeconds < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(elapsedSeconds));
        }

        double duration = DurationSeconds;
        if (!Loops && elapsedSeconds >= duration) return FrameCount - 1;
        double sample = Loops ? elapsedSeconds % duration : elapsedSeconds;
        return Math.Min((int)(sample / FrameSeconds), FrameCount - 1);
    }
}
