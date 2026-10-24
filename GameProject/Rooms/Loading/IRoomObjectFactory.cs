using GameProject.Core;
using GameProject.Rooms.Definitions;

namespace GameProject.Rooms.Loading;

/// <summary>Allows navigation to supply persistent objects without coupling the loader to it.</summary>
public interface IRoomObjectFactory
{
    IGameObject Create(RoomObjectDefinition definition);
}
