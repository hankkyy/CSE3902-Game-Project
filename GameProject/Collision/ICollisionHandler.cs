using GameProject.Core;

namespace GameProject.Collision;

/// <summary>Handles one supported combination of collision layers.</summary>
public interface ICollisionHandler
{
    bool CanHandle(CollisionLayer first, CollisionLayer second);
    void Handle(ICollidable first, ICollidable second);
}
