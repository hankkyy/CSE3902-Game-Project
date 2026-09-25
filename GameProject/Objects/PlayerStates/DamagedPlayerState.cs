namespace GameProject.Objects.PlayerStates;

internal sealed class DamagedPlayerState : IPlayerState
{
    private const double DurationSeconds = 0.45;
    private double remainingSeconds = DurationSeconds;

    public PlayerAction Action => PlayerAction.Damaged;
    public bool AllowsMovement => false;

    public IPlayerState Update(double elapsedSeconds, bool wantsToMove)
    {
        remainingSeconds -= elapsedSeconds;
        if (remainingSeconds > 0)
        {
            return this;
        }

        return wantsToMove ? WalkingPlayerState.Instance : IdlePlayerState.Instance;
    }
}
