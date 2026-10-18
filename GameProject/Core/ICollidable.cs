using Microsoft.Xna.Framework;

namespace GameProject.Core;

/// <summary>Exposes collision state without coupling detection to a concrete object type.</summary>
public interface ICollidable
{
    Rectangle Hitbox { get; }
    CollisionLayer CollisionLayer { get; }
    bool IsActive { get; }
}
