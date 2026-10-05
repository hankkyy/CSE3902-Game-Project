using GameProject.Objects;

namespace GameProject.Commands;

/// <summary>Selects slot three and tries to place the player's bomb once.</summary>
public sealed class PlaceBombCommand(Player player) : ICommand
{
    public void Execute() => player.TryPlaceBomb();
}
