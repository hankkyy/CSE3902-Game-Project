namespace GameProject.States;

/// <summary>Tracks the transition from the menu into gameplay.</summary>
public sealed class GameSession
{
    public GameMode Mode { get; private set; } = GameMode.Menu;

    public void Start()
    {
        Mode = GameMode.Playing;
    }
}
