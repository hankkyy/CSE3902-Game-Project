using GameProject.Core;

namespace GameProject.Collision;

/// <summary>Runs one detection and response pass over the active room objects.</summary>
public sealed class CollisionSystem(CollisionDispatcher dispatcher)
{
    public int Process(IReadOnlyList<ICollidable> objects)
    {
        ArgumentNullException.ThrowIfNull(objects);
        IReadOnlyList<CollisionPair> collisions = CollisionDetector.FindCollisions(objects);

        int handledCount = 0;
        foreach (CollisionPair collision in collisions)
        {
            if (!collision.First.IsActive || !collision.Second.IsActive)
            {
                continue;
            }

            if (dispatcher.Dispatch(collision))
            {
                handledCount++;
            }
        }

        return handledCount;
    }
}
