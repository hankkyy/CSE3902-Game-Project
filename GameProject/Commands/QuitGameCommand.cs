namespace GameProject.Commands;

/// <summary>Exits the game through its quit callback.</summary>
public sealed class QuitGameCommand(Action quit) : ICommand
{
    public void Execute() => quit();
}
