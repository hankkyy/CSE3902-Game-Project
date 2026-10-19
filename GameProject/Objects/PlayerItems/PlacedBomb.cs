using GameProject.Core;
using GameProject.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameProject.Objects.PlayerItems;

/// <summary>A timed bomb with a separate explosion hitbox and one hit per target per placement.</summary>
public sealed class PlacedBomb(Rectangle bounds, ISprite bombSprite, ISprite explosionSprite) : IGameObject
{
    public const int Size = 16;
    private const int ExplosionSize = 48;
    private const double FuseSeconds = 1.2;
    private const double ExplosionSeconds = 0.35;
    private double age;

    public string Name => "Player bomb";
    public Vector2 Position { get; private set; }
    public bool IsActive { get; private set; }
    public bool IsExploding => IsActive && age >= FuseSeconds;
    internal HashSet<object> HitTargets { get; } = [];
    public Rectangle ExplosionHitbox
    {
        get
        {
            if (!IsExploding)
            {
                return Rectangle.Empty;
            }

            Vector2 position = ExplosionPosition;
            return new Rectangle((int)position.X, (int)position.Y, ExplosionSize, ExplosionSize);
        }
    }

    private Vector2 ExplosionPosition
    {
        get
        {
            Vector2 position = Position + new Vector2((Size - ExplosionSize) / 2f);
            return new Vector2(
                MathHelper.Clamp(position.X, bounds.Left, bounds.Right - ExplosionSize),
                MathHelper.Clamp(position.Y, bounds.Top, bounds.Bottom - ExplosionSize));
        }
    }

    public bool TryPlace(Vector2 position)
    {
        if (IsActive || bounds.Width < ExplosionSize || bounds.Height < ExplosionSize ||
            position.X < bounds.Left || position.Y < bounds.Top ||
            position.X + Size > bounds.Right || position.Y + Size > bounds.Bottom) return false;

        Position = position;
        HitTargets.Clear();
        age = 0;
        IsActive = true;
        return true;
    }

    public void Update(GameTime gameTime)
    {
        ArgumentNullException.ThrowIfNull(gameTime);
        if (!IsActive) return;
        age += Math.Max(0, gameTime.ElapsedGameTime.TotalSeconds);
        if (age >= FuseSeconds + ExplosionSeconds) IsActive = false;
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        if (!IsActive) return;
        bool alternate = ((int)(age * 10) % 2) == 1;
        if (!IsExploding)
        {
            bombSprite.Draw(spriteBatch, Position, Direction.Down, alternate);
            return;
        }

        // Keep the complete explosion visible inside the play area, including edge placements.
        explosionSprite.Draw(spriteBatch, ExplosionPosition, Direction.Down, alternate);
    }

    public void Reset()
    {
        Position = Vector2.Zero;
        age = 0;
        IsActive = false;
        HitTargets.Clear();
    }
}
