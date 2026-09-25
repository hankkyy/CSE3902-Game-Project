namespace GameProject.Objects.PlayerStates;

internal sealed class AttackingPlayerState : IPlayerState
{
    private const double DurationSeconds = 0.28;
    private double remainingSeconds = DurationSeconds;

    public PlayerAction Action => PlayerAction.Attacking;
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
