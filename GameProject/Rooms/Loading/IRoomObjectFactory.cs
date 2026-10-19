using GameProject.Core;
using GameProject.Rooms.Definitions;

namespace GameProject.Rooms.Loading;

/// <summary>Creates an object from its room definition.</summary>
public interface IRoomObjectFactory
{
    IGameObject Create(RoomObjectDefinition definition);
}
