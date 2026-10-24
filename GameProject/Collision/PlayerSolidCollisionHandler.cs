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
            ResolveSweptCollision(player, solid);
            return;
        }

        Rectangle previous = player.PreviousHitbox;
        Vector2 corrected = player.Position;

        if (previous.Bottom <= solid.Top)
        {
            corrected.Y = solid.Top - current.Height;
        }
        else if (previous.Top >= solid.Bottom)
        {
            corrected.Y = solid.Bottom;
        }
        else if (previous.Right <= solid.Left)
        {
            corrected.X = solid.Left - current.Width;
        }
        else if (previous.Left >= solid.Right)
        {
            corrected.X = solid.Right;
        }
        else
        {
            corrected = ResolveSmallestOverlap(player.Position, current, solid);
        }

        player.CorrectPosition(corrected);
    }

    private static void ResolveSweptCollision(IMovableCollidable player, Rectangle solid)
    {
        if (!SweptAabb.TryFind(player, solid, out SweptHit hit))
        {
            return;
        }

        Vector2 movement = player.Position - player.PreviousPosition;
        Vector2 corrected = player.PreviousPosition + movement * hit.Time;
        if (hit.Horizontal)
        {
            corrected.X = movement.X > 0
                ? solid.Left - player.PreviousHitbox.Width
                : solid.Right;
        }
        else
        {
            corrected.Y = movement.Y > 0
                ? solid.Top - player.PreviousHitbox.Height
                : solid.Bottom;
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
            horizontalCorrection = solid.Left - current.Width;
        }
        else
        {
            horizontalOverlap = rightOverlap;
            horizontalCorrection = solid.Right;
        }

        int verticalOverlap;
        float verticalCorrection;
        if (current.Center.Y < solid.Center.Y)
        {
            verticalOverlap = topOverlap;
            verticalCorrection = solid.Top - current.Height;
        }
        else
        {
            verticalOverlap = bottomOverlap;
            verticalCorrection = solid.Bottom;
        }

        if (horizontalOverlap < verticalOverlap)
        {
            return new Vector2(horizontalCorrection, position.Y);
        }

        return new Vector2(position.X, verticalCorrection);
    }
}
