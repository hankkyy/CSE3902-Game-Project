using GameProject.Rooms.Definitions;

namespace GameProject.Rooms.Loading;

/// <summary>Rejects unusable room data before any sprites or game objects are created.</summary>
public static class RoomValidator
{
    private const int PlayerWidth = 28;
    private const int PlayerHeight = 34;
    private const int MaximumCoordinate = 16384;

    public static void Validate(RoomDefinition room)
    {
        ArgumentNullException.ThrowIfNull(room);
        RequireText(room.Id, "room id");
        RequireText(room.Name, $"{room.Id}: name");
        ValidateArea(room.Bounds, $"{room.Id}: bounds");
        if (room.Bounds.Width < PlayerWidth || room.Bounds.Height < PlayerHeight)
            throw new InvalidDataException($"{room.Id}: bounds cannot fit an object.");
        if (room.Entries is null || room.Entries.Count == 0 || !room.Entries.ContainsKey("start"))
            throw new InvalidDataException($"{room.Id}: entries must include 'start'.");
        foreach (var entry in room.Entries)
        {
            RequireText(entry.Key, $"{room.Id}: entry id");
            ValidatePosition(entry.Value, room.Bounds, PlayerWidth, PlayerHeight, $"{room.Id}: entry '{entry.Key}'");
        }

        if (room.Objects is null || room.Links is null)
            throw new InvalidDataException($"{room.Id}: objects and links must be arrays.");
        HashSet<string> ids = new(StringComparer.Ordinal);
        int players = 0;
        foreach (RoomObjectDefinition obj in room.Objects)
        {
            if (obj is null) throw new InvalidDataException($"{room.Id}: null object.");
            RequireText(obj.Id, $"{room.Id}: object id");
            if (!ids.Add(obj.Id)) throw new InvalidDataException($"{room.Id}: duplicate object id '{obj.Id}'.");
            RequireText(obj.Name, $"{room.Id}/{obj.Id}: name");
            if (!RoomObjectTypes.IsKnown(obj.Type))
                throw new InvalidDataException($"{room.Id}/{obj.Id}: unknown type '{obj.Type}'.");
            // Reserve the largest existing sprite footprint in each family.
            int width = 32;
            int height = 34;
            if (obj.Type == "Player") width = PlayerWidth;
            else if (obj.Type.StartsWith("Block.", StringComparison.Ordinal)) { width = 52; height = 60; }
            else if (obj.Type.StartsWith("Enemy.", StringComparison.Ordinal)) { width = 38; height = 42; }
            ValidatePosition(obj.Position, room.Bounds, width, height, $"{room.Id}/{obj.Id}");
            if (obj.Properties is null) throw new InvalidDataException($"{room.Id}/{obj.Id}: properties cannot be null.");
            foreach (var property in obj.Properties)
            {
                RequireText(property.Key, $"{room.Id}/{obj.Id}: property key");
                if (property.Value is null) throw new InvalidDataException($"{room.Id}/{obj.Id}: property '{property.Key}' cannot be null.");
            }
            if (obj.Type == "Player") players++;
        }
        if (players != 1) throw new InvalidDataException($"{room.Id}: expected exactly one Player, found {players}.");

        ids.Clear();
        foreach (RoomLink link in room.Links)
        {
            if (link is null) throw new InvalidDataException($"{room.Id}: null link.");
            RequireText(link.Id, $"{room.Id}: link id");
            if (!ids.Add(link.Id)) throw new InvalidDataException($"{room.Id}: duplicate link id '{link.Id}'.");
            RequireText(link.TargetRoomId, $"{room.Id}/{link.Id}: target room");
            RequireText(link.TargetEntryId, $"{room.Id}/{link.Id}: target entry");
            if (link.Direction != "Up" && link.Direction != "Down" && link.Direction != "Left" && link.Direction != "Right")
                throw new InvalidDataException($"{room.Id}/{link.Id}: invalid direction '{link.Direction}'.");
            ValidateArea(link.Trigger, $"{room.Id}/{link.Id}: trigger");
            if (!room.Bounds.ToRectangle().Contains(link.Trigger.ToRectangle()))
                throw new InvalidDataException($"{room.Id}/{link.Id}: trigger is outside room bounds.");
        }
    }

    private static void RequireText(string value, string context)
    {
        if (string.IsNullOrWhiteSpace(value)) throw new InvalidDataException($"{context} must not be empty.");
    }

    private static void ValidateArea(RoomArea area, string context)
    {
        if (area is null || area.X < 0 || area.Y < 0 || area.Width <= 0 || area.Height <= 0 ||
            (long)area.X + area.Width > MaximumCoordinate || (long)area.Y + area.Height > MaximumCoordinate)
            throw new InvalidDataException($"{context}: invalid rectangle (positive size, coordinates 0..{MaximumCoordinate} required).");
    }

    private static void ValidatePosition(RoomPoint point, RoomArea bounds, int width, int height, string context)
    {
        if (point is null || point.X < bounds.X || point.Y < bounds.Y ||
            (long)point.X + width > bounds.X + bounds.Width ||
            (long)point.Y + height > bounds.Y + bounds.Height)
            throw new InvalidDataException($"{context}: position and {width}x{height} footprint must fit inside bounds.");
    }
}
