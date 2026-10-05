using GameProject.Objects;

namespace GameProject.Commands;

/// <summary>Selects slot one and tries to fire the player's arrow once.</summary>
public sealed class FireArrowCommand(Player player) : ICommand
{
    public void Execute() => player.TryFireArrow();
}
