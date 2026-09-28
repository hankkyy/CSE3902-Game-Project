using GameProject.Commands;
using Microsoft.Xna.Framework.Input;

namespace GameProject.Controllers;

/// <summary>Executes commands when their keys change from released to pressed.</summary>
public sealed class KeyPressDispatcher(
    IReadOnlyDictionary<Keys, ICommand> commands)
{
    private KeyboardState previousState;

    public void Update(KeyboardState currentState)
    {
        foreach ((Keys key, ICommand command) in commands)
        {
            if (currentState.IsKeyDown(key) && previousState.IsKeyUp(key))
            {
                command.Execute();
            }
        }

        previousState = currentState;
    }
}
