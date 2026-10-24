using GameProject.Core;
using GameProject.Rooms.Definitions;
using GameProject.Rooms.Loading;

namespace GameProject.Rooms.Navigation;

/// <summary>Creates each visited room once and shares one player across rooms.</summary>
public sealed class RoomCache
{
    private readonly Dictionary<string, LoadedRoom> savedRooms = new(StringComparer.Ordinal);
    private readonly IGameObject player;
    private readonly Func<RoomDefinition, IRoomObjectFactory> createFactory;

    public RoomCache(IGameObject player, Func<RoomDefinition, IRoomObjectFactory> createFactory)
    {
        ArgumentNullException.ThrowIfNull(player);
        ArgumentNullException.ThrowIfNull(createFactory);
        this.player = player;
        this.createFactory = createFactory;
    }

    public LoadedRoom GetRoom(RoomDefinition definition)
    {
        ArgumentNullException.ThrowIfNull(definition);

        if (savedRooms.TryGetValue(definition.Id, out LoadedRoom? existingRoom))
        {
            return existingRoom;
        }

        IRoomObjectFactory objectFactory = createFactory(definition);
        var sharedFactory = new SharedPlayerFactory(player, objectFactory);
        var newRoom = new LoadedRoom(definition, sharedFactory);
        savedRooms.Add(definition.Id, newRoom);
        return newRoom;
    }

    public void Reset()
    {
        foreach (LoadedRoom room in savedRooms.Values)
        {
            foreach (IGameObject roomObject in room.Objects.Values)
            {
                // The player is shared, so reset it only once below.
                if (!ReferenceEquals(roomObject, player))
                {
                    roomObject.Reset();
                }
            }
        }

        player.Reset();
    }

    private sealed class SharedPlayerFactory : IRoomObjectFactory
    {
        private readonly IGameObject player;
        private readonly IRoomObjectFactory objectFactory;

        public SharedPlayerFactory(IGameObject player, IRoomObjectFactory objectFactory)
        {
            ArgumentNullException.ThrowIfNull(objectFactory);
            this.player = player;
            this.objectFactory = objectFactory;
        }

        public IGameObject Create(RoomObjectDefinition definition)
        {
            ArgumentNullException.ThrowIfNull(definition);

            if (definition.Type == "Player")
            {
                return player;
            }

            return objectFactory.Create(definition);
        }
    }
}
