using GameProject.Core;
using GameProject.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameProject.Objects.Enemies;

/// <summary>Enemy-owned horizontal shot with a sprite provided by the visual layer.</summary>
public sealed class EnemyProjectile(ISprite sprite) : IGameObject
{
    private const float Speed = 100;
    private Direction facing;
    private Vector2 origin;
    private double age;
    public string Name => "Octorok projectile";
    public Vector2 Position { get; private set; }
    public bool IsActive { get; private set; }

    public void Launch(Vector2 position, Direction direction)
    {
        origin = position;
        Position = position;
        facing = direction;
        age = 0;
        IsActive = true;
    }

    public void Update(GameTime gameTime)
    {
        if (!IsActive) return;
        age += Math.Max(0, gameTime.ElapsedGameTime.TotalSeconds);
        Position = origin + new Vector2((facing == Direction.Left ? -1 : 1) * Speed * (float)age, 0);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        if (IsActive) sprite.Draw(spriteBatch, Position, facing, false);
    }

    public void Reset()
    {
        IsActive = false;
        age = 0;
        origin = Vector2.Zero;
        Position = Vector2.Zero;
        facing = Direction.Right;
    }
}
