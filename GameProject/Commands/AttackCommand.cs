using GameProject.Objects;

namespace GameProject.Commands;

/// <summary>Executes the player's attack action.</summary>
public sealed class AttackCommand(Player player) : ICommand
{
    public void Execute() => player.Attack();
}
