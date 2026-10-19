using Microsoft.Xna.Framework;

namespace GameProject.Combat;

/// <summary>Position corrections used only by combat responses, not pair detection.</summary>
internal static class CombatGeometry
{
    public static Vector2 Separation(Rectangle moving, Rectangle solid, Rectangle previous)
    {
        if (!moving.Intersects(solid))
        {
            return Vector2.Zero;
        }

        int left = solid.Left - moving.Right;
        int right = solid.Right - moving.Left;
        int up = solid.Top - moving.Bottom;
        int down = solid.Bottom - moving.Top;
        // Prefer the side we entered, even when a long frame crosses the wall's center.
        if (previous.Right <= solid.Left)
        {
            return new Vector2(left, 0);
        }
        else if (previous.Left >= solid.Right)
        {
            return new Vector2(right, 0);
        }
        else if (previous.Bottom <= solid.Top)
        {
            return new Vector2(0, up);
        }
        else if (previous.Top >= solid.Bottom)
        {
            return new Vector2(0, down);
        }

        int horizontal = right;
        int vertical = down;
        if (Math.Abs(left) <= Math.Abs(right))
        {
            horizontal = left;
        }

        if (Math.Abs(up) <= Math.Abs(down))
        {
            vertical = up;
        }

        if (Math.Abs(horizontal) <= Math.Abs(vertical))
        {
            return new Vector2(horizontal, 0);
        }
        else
        {
            return new Vector2(0, vertical);
        }
    }
}
