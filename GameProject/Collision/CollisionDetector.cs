using GameProject.Core;

namespace GameProject.Collision;

/// <summary>Finds active AABB intersections without applying gameplay responses.</summary>
public static class CollisionDetector
{
    public static IReadOnlyList<CollisionPair> FindCollisions(IReadOnlyList<ICollidable> objects)
    {
        ArgumentNullException.ThrowIfNull(objects);
        List<CollisionPair> collisions = [];

        for (int firstIndex = 0; firstIndex < objects.Count; firstIndex++)
        {
            ICollidable first = objects[firstIndex];
            if (!first.IsActive)
            {
                continue;
            }

            for (int secondIndex = firstIndex + 1; secondIndex < objects.Count; secondIndex++)
            {
                ICollidable second = objects[secondIndex];
                if (!second.IsActive)
                {
                    continue;
                }

                if (first.Hitbox.Intersects(second.Hitbox))
                {
                    collisions.Add(new CollisionPair(first, second));
                }
            }
        }

        return collisions;
    }
}
