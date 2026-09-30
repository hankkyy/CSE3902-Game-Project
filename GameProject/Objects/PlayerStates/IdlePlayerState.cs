namespace GameProject.Objects.PlayerStates;

internal sealed class IdlePlayerState : IPlayerState
{
    public static IdlePlayerState Instance { get; } = new();

    private IdlePlayerState()
    {
    }

    public PlayerAction Action => PlayerAction.Idle;
    public bool AllowsMovement => true;

    public IPlayerState Update(double elapsedSeconds, bool wantsToMove) =>
        wantsToMove ? WalkingPlayerState.Instance : this;
}
