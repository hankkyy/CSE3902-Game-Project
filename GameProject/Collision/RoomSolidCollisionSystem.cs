using GameProject.Core;
using GameProject.Objects;
using GameProject.Rooms.Definitions;
using Microsoft.Xna.Framework;

namespace GameProject.Collision;

/// <summary>Runs player collision against the solid blocks described by the active room.</summary>
public sealed class RoomSolidCollisionSystem
{
    private readonly CollisionSystem collisionSystem = new(
        new CollisionDispatcher([new PlayerSolidCollisionHandler()]));
    private readonly Dictionary<string, IReadOnlyList<SolidCollider>> roomSolids =
        new(StringComparer.Ordinal);

    public int Process(Player player, RoomDefinition room)
    {
        ArgumentNullException.ThrowIfNull(player);
        ArgumentNullException.ThrowIfNull(room);

        if (!roomSolids.TryGetValue(room.Id, out IReadOnlyList<SolidCollider>? solids))
        {
            solids = CreateSolids(room);
            roomSolids.Add(room.Id, solids);
        }

        List<ICollidable> collidables = new(solids.Count + 1) { player };
        collidables.AddRange(solids);
        return collisionSystem.Process(collidables);
    }

    private static List<SolidCollider> CreateSolids(RoomDefinition room)
    {
        List<SolidCollider> solids = [];
        foreach (RoomObjectDefinition roomObject in room.Objects)
        {
            Rectangle? hitbox = CreateSolidHitbox(roomObject);
            if (hitbox.HasValue)
            {
                solids.Add(new SolidCollider(hitbox.Value));
            }
        }

        return solids;
    }

    private static Rectangle? CreateSolidHitbox(RoomObjectDefinition roomObject)
    {
        int width;
        int height;

        if (roomObject.Type == "Block.Stone" ||
            roomObject.Type == "Block.Push" ||
            roomObject.Type == "Block.Water")
        {
            width = 52;
            height = 52;
        }
        else if (roomObject.Type == "Block.Statue")
        {
            width = 52;
            height = 60;
        }
        else if (roomObject.Type == "Block.CrystalPillar")
        {
            width = 48;
            height = 48;
        }
        else
        {
            // Rune tiles and non-block objects are floor decorations or use other handlers.
            return null;
        }

        return new Rectangle(roomObject.Position.X, roomObject.Position.Y, width, height);
    }
}
