using GameProject.Core;

namespace GameProject.Collision;

/// <summary>One intersecting unordered pair found during a collision pass.</summary>
public readonly record struct CollisionPair(ICollidable First, ICollidable Second);
