using GameProject.States;

namespace GameProject.Commands;

/// <summary>Starts gameplay from the menu.</summary>
public sealed class StartGameCommand(GameSession session) : ICommand
{
    public void Execute() => session.Start();
}
