using GameProject.Rooms.Definitions;

namespace GameProject.Rooms.Navigation;

/// <summary>Keeps track of the current room and queues room changes.</summary>
public sealed class RoomManager
{
    private readonly List<RoomDefinition> rooms;
    private readonly int startingIndex;
    private int currentIndex;
    private int pendingIndex = -1;
    private string currentEntryId = "start";
    private string pendingEntryId = "start";

    public RoomManager(IReadOnlyList<RoomDefinition> definitions, string startingRoomId)
    {
        ArgumentNullException.ThrowIfNull(definitions);
        ArgumentException.ThrowIfNullOrWhiteSpace(startingRoomId);

        rooms = new List<RoomDefinition>(definitions);
        if (rooms.Count == 0)
        {
            throw new ArgumentException("At least one room is required.", nameof(definitions));
        }

        var ids = new HashSet<string>(StringComparer.Ordinal);
        foreach (RoomDefinition room in rooms)
        {
            if (!ids.Add(room.Id) || !room.Entries.ContainsKey("start"))
            {
                throw new ArgumentException("Rooms need unique IDs and a start entry.", nameof(definitions));
            }
        }

        startingIndex = FindRoomIndex(startingRoomId);
        if (startingIndex == -1)
        {
            throw new ArgumentException("The starting room does not exist.", nameof(startingRoomId));
        }

        currentIndex = startingIndex;
    }

    public RoomDefinition CurrentRoom => rooms[currentIndex];
    public RoomPoint EntryPosition => CurrentRoom.Entries[currentEntryId];
    public int RoomNumber => currentIndex + 1;
    public int RoomCount => rooms.Count;

    public void RequestPreviousRoom()
    {
        if (rooms.Count < 2)
        {
            return;
        }

        int targetIndex = currentIndex - 1;
        if (targetIndex < 0)
        {
            targetIndex = rooms.Count - 1;
        }

        QueueRoom(targetIndex, "start");
    }

    public void RequestNextRoom()
    {
        if (rooms.Count < 2)
        {
            return;
        }

        int targetIndex = currentIndex + 1;
        if (targetIndex >= rooms.Count)
        {
            targetIndex = 0;
        }

        QueueRoom(targetIndex, "start");
    }

    // Call this only after checking that the exit is open and usable.
    public bool RequestExit(string exitId)
    {
        foreach (RoomLink link in CurrentRoom.Links)
        {
            if (link.Id == exitId)
            {
                int targetIndex = FindRoomIndex(link.TargetRoomId);
                return QueueRoom(targetIndex, link.TargetEntryId);
            }
        }

        return false;
    }

    // Apply after collision processing, then place the player at EntryPosition.
    public bool ApplyPendingTransition()
    {
        if (pendingIndex == -1)
        {
            return false;
        }

        currentIndex = pendingIndex;
        currentEntryId = pendingEntryId;
        pendingIndex = -1;
        return true;
    }

    // Resets navigation only. Gameplay systems need their own reset calls.
    public void ResetNavigation()
    {
        currentIndex = startingIndex;
        currentEntryId = "start";
        pendingIndex = -1;
        pendingEntryId = "start";
    }

    private bool QueueRoom(int targetIndex, string entryId)
    {
        // Keep the first valid request until it has been applied.
        if (pendingIndex != -1 || targetIndex < 0 || targetIndex >= rooms.Count)
        {
            return false;
        }

        if (!rooms[targetIndex].Entries.ContainsKey(entryId))
        {
            return false;
        }

        pendingIndex = targetIndex;
        pendingEntryId = entryId;
        return true;
    }

    private int FindRoomIndex(string roomId)
    {
        for (int index = 0; index < rooms.Count; index++)
        {
            if (rooms[index].Id == roomId)
            {
                return index;
            }
        }

        return -1;
    }
}
