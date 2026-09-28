namespace GameProject.UI;

/// <summary>Read-only player information presented by the HUD.</summary>
public readonly record struct PlayerHudInfo(int Health, int SelectedItem, string Action, string Facing);
