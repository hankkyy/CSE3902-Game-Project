namespace GameProject.Commands;

/// <summary>Restores the game through its reset callback.</summary>
public sealed class ResetGameCommand(Action reset) : ICommand
{
    public void Execute() => reset();
}
