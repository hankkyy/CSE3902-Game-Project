using GameProject.Core;
using GameProject.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameProject.Objects.PlayerItems;

/// <summary>A stationary timed bomb with a visual explosion only; it does not damage other objects.</summary>
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

    public bool TryPlace(Vector2 position)
    {
        if (IsActive || bounds.Width < ExplosionSize || bounds.Height < ExplosionSize ||
            position.X < bounds.Left || position.Y < bounds.Top ||
            position.X + Size > bounds.Right || position.Y + Size > bounds.Bottom) return false;

        Position = position;
        age = 0;
        IsActive = true;
        return true;
    }

    public void Update(GameTime gameTime)
    {
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
        Vector2 explosionPosition = Position + new Vector2((Size - ExplosionSize) / 2f);
        explosionPosition = new Vector2(
            MathHelper.Clamp(explosionPosition.X, bounds.Left, bounds.Right - ExplosionSize),
            MathHelper.Clamp(explosionPosition.Y, bounds.Top, bounds.Bottom - ExplosionSize));
        explosionSprite.Draw(spriteBatch, explosionPosition, Direction.Down, alternate);
    }

    public void Reset()
    {
        Position = Vector2.Zero;
        age = 0;
        IsActive = false;
    }
}
