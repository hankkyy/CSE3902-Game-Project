using GameProject.Core;
using GameProject.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameProject.Objects.Enemies;

/// <summary>Deterministic simulation clock and sprite delegation for gallery characters.</summary>
public abstract class EnemyCharacter : IGameObject
{
    private readonly ISprite sprite;
    private readonly EnemyHealth health = new();
    private Vector2 movementOffset;
    protected readonly Vector2 startPosition;
    protected double elapsed;

    protected EnemyCharacter(string name, Vector2 position, ISprite sprite)
    {
        Name = name;
        startPosition = position;
        Position = position;
        PreviousPosition = position;
        this.sprite = sprite;
    }

    public string Name { get; }
    public Vector2 Position { get; protected set; }
    public Vector2 PreviousPosition { get; private set; }
    public Direction Facing { get; protected set; } = Direction.Right;
    public bool AlternateFrame { get; private set; }
    public int Health => health.Health;
    public bool IsActive => health.IsAlive;
    public virtual bool IsHostile => true;
    protected virtual Point HitboxSize => new(32, 32);
    protected Vector2 MovementOffset => movementOffset;
    public Rectangle Hitbox => new((int)Position.X, (int)Position.Y, HitboxSize.X, HitboxSize.Y);
    public Rectangle PreviousHitbox => new((int)PreviousPosition.X, (int)PreviousPosition.Y, HitboxSize.X, HitboxSize.Y);

    public bool ReceiveDamage(int amount)
    {
        if (!IsHostile || !health.ReceiveDamage(amount))
        {
            return false;
        }

        if (!IsActive)
        {
            OnDefeated();
        }

        return true;
    }

    protected virtual void OnDefeated() { }

    /// <summary>Retains wall correction when the procedural patrol updates next frame.</summary>
    public void ApplyCollisionCorrection(Vector2 correction)
    {
        if (!IsActive)
        {
            return;
        }

        Position += correction;
        movementOffset += correction;
    }

    public virtual void Update(GameTime gameTime)
    {
        ArgumentNullException.ThrowIfNull(gameTime);
        if (!IsActive)
        {
            return;
        }

        PreviousPosition = Position;
        elapsed += Math.Max(0, gameTime.ElapsedGameTime.TotalSeconds);
        AlternateFrame = (int)(elapsed * 7 % 2) == 1;
        UpdateMovement();
        Position += movementOffset;
    }

    protected abstract void UpdateMovement();
    public virtual void Draw(SpriteBatch spriteBatch)
    {
        if (IsActive)
        {
            sprite.Draw(spriteBatch, Position, Facing, AlternateFrame);
        }
    }

    public virtual void Reset()
    {
        elapsed = 0;
        health.Reset();
        movementOffset = Vector2.Zero;
        Position = startPosition;
        PreviousPosition = startPosition;
        Facing = Direction.Right;
        AlternateFrame = false;
    }
}
