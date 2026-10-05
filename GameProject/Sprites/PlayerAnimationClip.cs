namespace GameProject.Sprites;

/// <summary>Stores frame timing and selects a frame from the supplied animation time.</summary>
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
        if (!Loops && elapsedSeconds >= duration)
        {
            return FrameCount - 1;
        }

        double timeInClip = elapsedSeconds;
        if (Loops)
        {
            timeInClip %= duration;
        }

        int frameIndex = (int)(timeInClip / FrameSeconds);
        return Math.Min(frameIndex, FrameCount - 1);
    }
}
