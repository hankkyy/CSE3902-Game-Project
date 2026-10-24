using GameProject.Objects;

namespace GameProject.Rooms.Loading;

/// <summary>Case-sensitive external names derived from the existing first-dungeon roster.</summary>
public static class RoomObjectTypes
{
    public static IReadOnlyList<string> All { get; } = Array.AsReadOnly(
        new[] { "Player" }
            .Concat(Enum.GetNames<BlockKind>().Select(name => "Block." + name))
            .Concat(Enum.GetNames<ItemKind>().Select(name => "Item." + name))
            .Concat(Enum.GetNames<EnemyKind>().Select(name => "Enemy." + name)).ToArray());

    public static bool IsKnown(string type) => All.Contains(type, StringComparer.Ordinal);
}
