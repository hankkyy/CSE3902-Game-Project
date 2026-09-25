namespace GameProject.Commands;

/// <summary>Small command adapter keeps key bindings independent of their targets.</summary>
public sealed class ActionCommand(Action action) : ICommand
{
    public void Execute() => action();
}
