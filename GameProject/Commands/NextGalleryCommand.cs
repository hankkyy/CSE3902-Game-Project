namespace GameProject.Commands;

/// <summary>Selects the next object in a gallery.</summary>
public sealed class NextGalleryCommand(Action next) : ICommand
{
    public void Execute() => next();
}
