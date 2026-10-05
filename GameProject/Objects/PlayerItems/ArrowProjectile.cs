using GameProject.Core;
using GameProject.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameProject.Objects.PlayerItems;

/// <summary>A single player-owned arrow that flies without colliding with other objects.</summary>
public sealed class ArrowProjectile(Rectangle bounds, ISprite sprite) : IGameObject
{
    public const int Size = 16;
    private const float Speed = 320f;
    private const double LifetimeSeconds = 0.8;
    private Vector2 origin;
    private Vector2 velocity;
    private double age;

    public string Name => "Player arrow";
    public Vector2 Position { get; private set; }
    public Direction Facing { get; private set; } = Direction.Down;
    public bool IsActive { get; private set; }

    /// <summary>Starts at a top-left position; an active arrow cannot be replaced.</summary>
    public bool TryLaunch(Vector2 position, Direction direction)
    {
        if (IsActive || !IsInsideBounds(position)) return false;

        origin = position;
        Position = position;
        Facing = direction;
        velocity = direction switch
        {
            Direction.Up => new Vector2(0, -Speed),
            Direction.Down => new Vector2(0, Speed),
            Direction.Left => new Vector2(-Speed, 0),
            _ => new Vector2(Speed, 0)
        };
        age = 0;
        IsActive = true;
        return true;
    }

    public void Update(GameTime gameTime)
    {
        if (!IsActive) return;

        age += Math.Max(0, gameTime.ElapsedGameTime.TotalSeconds);
        Position = origin + velocity * (float)age;
        if (age >= LifetimeSeconds || !IsInsideBounds(Position)) IsActive = false;
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        if (IsActive) sprite.Draw(spriteBatch, Position, Facing, false);
    }

    public void Reset()
    {
        IsActive = false;
        origin = Vector2.Zero;
        velocity = Vector2.Zero;
        Position = Vector2.Zero;
        Facing = Direction.Down;
        age = 0;
    }

    private bool IsInsideBounds(Vector2 position) =>
        position.X >= bounds.Left && position.Y >= bounds.Top &&
        position.X + Size <= bounds.Right && position.Y + Size <= bounds.Bottom;
}
