using System.Text.Json;
using System.Text.Json.Serialization;
using GameProject.Rooms.Definitions;

namespace GameProject.Rooms.Loading;

/// <summary>Loads and validates external JSON without needing a graphics device.</summary>
public static class RoomLoader
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        UnmappedMemberHandling = JsonUnmappedMemberHandling.Disallow
    };

    public static RoomDefinition Parse(string json, string source = "room JSON")
    {
        ArgumentNullException.ThrowIfNull(json);
        try
        {
            using JsonDocument document = JsonDocument.Parse(json);
            RejectDuplicateProperties(document.RootElement);
            RoomDefinition room = document.Deserialize<RoomDefinition>(JsonOptions)
                ?? throw new InvalidDataException("Expected a room object, not null.");
            RoomValidator.Validate(room);
            return room;
        }
        catch (JsonException error)
        {
            throw new InvalidDataException($"{source}: malformed room JSON: {error.Message}", error);
        }
        catch (InvalidDataException error)
        {
            throw new InvalidDataException($"{source}: {error.Message}", error);
        }
    }

    public static RoomDefinition LoadFile(string path) => Parse(File.ReadAllText(path), path);

    /// <summary>Loads files in ordinal filename order, then checks room and entry references.</summary>
    public static IReadOnlyList<RoomDefinition> LoadDirectory(string directory)
    {
        string[] paths = Directory.GetFiles(directory, "*.json");
        Array.Sort(paths, StringComparer.Ordinal);
        if (paths.Length == 0) throw new InvalidDataException($"{directory}: no room JSON files found.");
        List<RoomDefinition> rooms = [];
        Dictionary<string, RoomDefinition> byId = new(StringComparer.Ordinal);
        foreach (string path in paths)
        {
            RoomDefinition room = LoadFile(path);
            if (!byId.TryAdd(room.Id, room)) throw new InvalidDataException($"{path}: duplicate room id '{room.Id}'.");
            rooms.Add(room);
        }
        foreach (RoomDefinition room in rooms)
        {
            foreach (RoomLink link in room.Links)
            {
                if (!byId.TryGetValue(link.TargetRoomId, out RoomDefinition? target))
                    throw new InvalidDataException($"{room.Id}/{link.Id}: unknown target room '{link.TargetRoomId}'.");
                if (!target.Entries.ContainsKey(link.TargetEntryId))
                    throw new InvalidDataException($"{room.Id}/{link.Id}: unknown entry '{link.TargetEntryId}' in '{target.Id}'.");
            }
        }
        return rooms.AsReadOnly();
    }

    private static void RejectDuplicateProperties(JsonElement element)
    {
        if (element.ValueKind == JsonValueKind.Object)
        {
            HashSet<string> names = new(StringComparer.Ordinal);
            foreach (JsonProperty property in element.EnumerateObject())
            {
                if (!names.Add(property.Name)) throw new InvalidDataException($"Duplicate JSON property '{property.Name}'.");
                RejectDuplicateProperties(property.Value);
            }
        }
        else if (element.ValueKind == JsonValueKind.Array)
        {
            foreach (JsonElement child in element.EnumerateArray()) RejectDuplicateProperties(child);
        }
    }
}
