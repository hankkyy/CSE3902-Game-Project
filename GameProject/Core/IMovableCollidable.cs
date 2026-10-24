using Microsoft.Xna.Framework;

namespace GameProject.Core;

/// <summary>A collidable object whose position can be corrected after movement.</summary>
public interface IMovableCollidable : ICollidable
{
    Vector2 Position { get; }
    Vector2 PreviousPosition { get; }
    Rectangle PreviousHitbox { get; }
    void CorrectPosition(Vector2 position);
}
