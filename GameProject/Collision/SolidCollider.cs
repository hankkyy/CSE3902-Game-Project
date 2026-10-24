using GameProject.Core;
using Microsoft.Xna.Framework;

namespace GameProject.Collision;

/// <summary>An invisible wall, room boundary, or closed-door collision area.</summary>
public sealed class SolidCollider(Rectangle hitbox) : ICollidable
{
    public Rectangle Hitbox { get; } = hitbox;
    public CollisionLayer CollisionLayer => CollisionLayer.Solid;
    public bool IsActive { get; private set; } = true;

    public void SetActive(bool isActive) => IsActive = isActive;
}
