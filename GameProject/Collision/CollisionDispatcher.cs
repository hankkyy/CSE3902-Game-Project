using GameProject.Core;

namespace GameProject.Collision;

/// <summary>Sends a collision pair to the first registered compatible handler.</summary>
public sealed class CollisionDispatcher
{
    private readonly IReadOnlyList<ICollisionHandler> handlers;

    public CollisionDispatcher(IReadOnlyList<ICollisionHandler> handlers)
    {
        ArgumentNullException.ThrowIfNull(handlers);
        this.handlers = handlers;
    }

    public bool Dispatch(CollisionPair pair)
    {
        foreach (ICollisionHandler handler in handlers)
        {
            if (handler.CanHandle(pair.First.CollisionLayer, pair.Second.CollisionLayer))
            {
                handler.Handle(pair.First, pair.Second);
                return true;
            }
        }

        return false;
    }
}
