using GameProject.Objects;

namespace GameProject.Commands;

/// <summary>Selects the player's secondary item slot.</summary>
public sealed class SelectItemCommand(Player player, int slot) : ICommand
{
    public void Execute() => player.SelectItem(slot);
}
