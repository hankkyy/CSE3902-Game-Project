namespace GameProject.Objects.PlayerStates;

internal sealed class WalkingPlayerState : IPlayerState
{
    public static WalkingPlayerState Instance { get; } = new();

    private WalkingPlayerState()
    {
    }

    public PlayerAction Action => PlayerAction.Walking;
    public bool AllowsMovement => true;

    public IPlayerState Update(double elapsedSeconds, bool wantsToMove)
    {
        if (wantsToMove)
        {
            return this;
        }

        return IdlePlayerState.Instance;
    }
}
