using Microsoft.Xna.Framework.Input;

namespace GameProject.Controllers;

/// <summary>
/// Requests room changes when a mouse button is newly pressed.
/// </summary>
internal sealed class MouseController
{
    private readonly Action previousRoom;
    private readonly Action nextRoom;

    private MouseState previousState;
    private bool hasPreviousState;

    public MouseController(Action previousRoom, Action nextRoom)
    {
        ArgumentNullException.ThrowIfNull(previousRoom);
        ArgumentNullException.ThrowIfNull(nextRoom);

        this.previousRoom = previousRoom;
        this.nextRoom = nextRoom;
    }

    /// <summary>Reads the mouse once for this frame.</summary>
    public void Update(bool navigationEnabled)
    {
        MouseState currentState = Mouse.GetState();
        Update(currentState, navigationEnabled);
    }

    /// <summary>
    /// Accepts a mouse snapshot so input can also be tested without a window.
    /// </summary>
    public void Update(MouseState currentState, bool navigationEnabled)
    {
        if (!hasPreviousState)
        {
            Reset(currentState);
            return;
        }

        if (!navigationEnabled)
        {
            previousState = currentState;
            return;
        }

        bool leftPressed =
            currentState.LeftButton == ButtonState.Pressed &&
            previousState.LeftButton == ButtonState.Released;

        bool rightPressed =
            currentState.RightButton == ButtonState.Pressed &&
            previousState.RightButton == ButtonState.Released;

        previousState = currentState;

        // Ignore simultaneous new presses to avoid two room changes.
        if (leftPressed && rightPressed)
        {
            return;
        }

        if (leftPressed)
        {
            previousRoom();
        }
        else if (rightPressed)
        {
            nextRoom();
        }
    }

    /// <summary>
    /// Synchronizes input history without treating a held button as a new click.
    /// </summary>
    public void Reset(MouseState currentState)
    {
        previousState = currentState;
        hasPreviousState = true;
    }
}
