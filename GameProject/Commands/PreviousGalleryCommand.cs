namespace GameProject.Commands;

/// <summary>Selects the previous object in a gallery.</summary>
public sealed class PreviousGalleryCommand(Action previous) : ICommand
{
    public void Execute() => previous();
}
