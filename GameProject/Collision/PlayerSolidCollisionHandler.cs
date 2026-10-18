using GameProject.Core;
using Microsoft.Xna.Framework;

namespace GameProject.Collision;

/// <summary>Moves a player out of an intersecting solid using its prior position.</summary>
public sealed class PlayerSolidCollisionHandler : ICollisionHandler
{
    public bool CanHandle(CollisionLayer first, CollisionLayer second)
    {
        bool playerFirst = first == CollisionLayer.Player && second == CollisionLayer.Solid;
        bool playerSecond = first == CollisionLayer.Solid && second == CollisionLayer.Player;
        return playerFirst || playerSecond;
    }

    public void Handle(ICollidable first, ICollidable second)
    {
        ArgumentNullException.ThrowIfNull(first);
        ArgumentNullException.ThrowIfNull(second);
        IMovableCollidable player;
        ICollidable solid;

        if (first.CollisionLayer == CollisionLayer.Player)
        {
            player = RequireMovablePlayer(first);
            solid = second;
        }
        else
        {
            player = RequireMovablePlayer(second);
            solid = first;
        }

        Resolve(player, solid.Hitbox);
    }

    private static IMovableCollidable RequireMovablePlayer(ICollidable collidable)
    {
        if (collidable is IMovableCollidable movable)
        {
            return movable;
        }

        throw new InvalidOperationException("The player collision layer must implement IMovableCollidable.");
    }

    private static void Resolve(IMovableCollidable player, Rectangle solid)
    {
        Rectangle current = player.Hitbox;
        if (!current.Intersects(solid))
        {
            return;
        }

        Rectangle previous = player.PreviousHitbox;
        Vector2 corrected = player.Position;

        if (previous.Bottom <= solid.Top)
        {
            corrected.Y -= current.Bottom - solid.Top;
        }
        else if (previous.Top >= solid.Bottom)
        {
            corrected.Y += solid.Bottom - current.Top;
        }
        else if (previous.Right <= solid.Left)
        {
            corrected.X -= current.Right - solid.Left;
        }
        else if (previous.Left >= solid.Right)
        {
            corrected.X += solid.Right - current.Left;
        }
        else
        {
            corrected = ResolveSmallestOverlap(player.Position, current, solid);
        }

        player.CorrectPosition(corrected);
    }

    private static Vector2 ResolveSmallestOverlap(Vector2 position, Rectangle current, Rectangle solid)
    {
        int leftOverlap = current.Right - solid.Left;
        int rightOverlap = solid.Right - current.Left;
        int topOverlap = current.Bottom - solid.Top;
        int bottomOverlap = solid.Bottom - current.Top;

        int horizontalOverlap;
        float horizontalCorrection;
        if (current.Center.X < solid.Center.X)
        {
            horizontalOverlap = leftOverlap;
            horizontalCorrection = -leftOverlap;
        }
        else
        {
            horizontalOverlap = rightOverlap;
            horizontalCorrection = rightOverlap;
        }

        int verticalOverlap;
        float verticalCorrection;
        if (current.Center.Y < solid.Center.Y)
        {
            verticalOverlap = topOverlap;
            verticalCorrection = -topOverlap;
        }
        else
        {
            verticalOverlap = bottomOverlap;
            verticalCorrection = bottomOverlap;
        }

        if (horizontalOverlap < verticalOverlap)
        {
            return new Vector2(position.X + horizontalCorrection, position.Y);
        }

        return new Vector2(position.X, position.Y + verticalCorrection);
    }
}
