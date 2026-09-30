using GameProject.States;

namespace GameProject.Commands;

/// <summary>Allows a gameplay command to run only after the game starts.</summary>
public sealed class GameplayCommand(GameSession session, ICommand command) : ICommand
{
    public void Execute()
    {
        if (session.Mode == GameMode.Playing)
        {
            command.Execute();
        }
    }
}
