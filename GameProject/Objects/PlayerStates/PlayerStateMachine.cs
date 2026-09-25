namespace GameProject.Objects.PlayerStates;

/// <summary>Owns valid player state transitions independently of input and drawing.</summary>
internal sealed class PlayerStateMachine
{
    private IPlayerState currentState = IdlePlayerState.Instance;

    public PlayerAction Action => currentState.Action;
    public bool AllowsMovement => currentState.AllowsMovement;

    public bool TryAttack()
    {
        if (Action is PlayerAction.Attacking or PlayerAction.Damaged)
        {
            return false;
        }

        currentState = new AttackingPlayerState();
        return true;
    }

    public bool TryTakeDamage()
    {
        if (Action == PlayerAction.Damaged)
        {
            return false;
        }

        currentState = new DamagedPlayerState();
        return true;
    }

    public void Update(double elapsedSeconds, bool wantsToMove) =>
        currentState = currentState.Update(elapsedSeconds, wantsToMove);

    public void Reset() => currentState = IdlePlayerState.Instance;
}
