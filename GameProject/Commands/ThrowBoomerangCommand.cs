using GameProject.Objects;

namespace GameProject.Commands;

/// <summary>Selects slot two and tries to throw the player's boomerang once.</summary>
public sealed class ThrowBoomerangCommand(Player player) : ICommand
{
    public void Execute() => player.TryThrowBoomerang();
}
