using GameProject.Core;
using Microsoft.Xna.Framework;

namespace GameProject.Collision;

internal readonly record struct SweptHit(float Time, bool Horizontal);

internal static class SweptAabb
{
    public static bool TryFind(IMovableCollidable moving, Rectangle solid, out SweptHit hit)
    {
        Vector2 start = moving.PreviousPosition;
        Vector2 movement = moving.Position - start;
        Rectangle previous = moving.PreviousHitbox;

        if (!AxisTimes(start.X, previous.Width, movement.X, solid.Left, solid.Right,
                out float xEntry, out float xExit) ||
            !AxisTimes(start.Y, previous.Height, movement.Y, solid.Top, solid.Bottom,
                out float yEntry, out float yExit))
        {
            hit = default;
            return false;
        }

        float entry = Math.Max(xEntry, yEntry);
        float exit = Math.Min(xExit, yExit);
        if (entry > exit || exit < 0 || entry < 0 || entry > 1)
        {
            hit = default;
            return false;
        }

        hit = new SweptHit(entry, xEntry > yEntry);
        return true;
    }

    private static bool AxisTimes(float start, int size, float movement, int solidStart,
        int solidEnd, out float entry, out float exit)
    {
        if (movement > 0)
        {
            entry = (solidStart - (start + size)) / movement;
            exit = (solidEnd - start) / movement;
            return true;
        }

        if (movement < 0)
        {
            entry = (solidEnd - start) / movement;
            exit = (solidStart - (start + size)) / movement;
            return true;
        }

        entry = float.NegativeInfinity;
        exit = float.PositiveInfinity;
        return start < solidEnd && start + size > solidStart;
    }
}
