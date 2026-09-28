using GameProject.Objects;

namespace GameProject.Commands;

/// <summary>Applies damage to the player.</summary>
public sealed class TakeDamageCommand(Player player) : ICommand
{
    public void Execute() => player.TakeDamage();
}
