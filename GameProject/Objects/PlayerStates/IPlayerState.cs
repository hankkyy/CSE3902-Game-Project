namespace GameProject.Objects.PlayerStates;

internal interface IPlayerState
{
    PlayerAction Action { get; }
    bool AllowsMovement { get; }
    IPlayerState Update(double elapsedSeconds, bool wantsToMove);
}
