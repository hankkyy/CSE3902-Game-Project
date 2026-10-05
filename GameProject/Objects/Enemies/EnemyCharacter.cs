using GameProject.Core;
using GameProject.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameProject.Objects.Enemies;

/// <summary>Deterministic simulation clock and sprite delegation for gallery characters.</summary>
public abstract class EnemyCharacter : IGameObject
{
    private readonly ISprite sprite;
    protected readonly Vector2 startPosition;
    protected double elapsed;

    protected EnemyCharacter(string name, Vector2 position, ISprite sprite)
    {
        Name = name;
        startPosition = position;
        Position = position;
        this.sprite = sprite;
    }

    public string Name { get; }
    public Vector2 Position { get; protected set; }
    public Direction Facing { get; protected set; } = Direction.Right;
    public bool AlternateFrame { get; private set; }

    public virtual void Update(GameTime gameTime)
    {
        elapsed += Math.Max(0, gameTime.ElapsedGameTime.TotalSeconds);
        AlternateFrame = (int)(elapsed * 7 % 2) == 1;
        UpdateMovement();
    }

    protected abstract void UpdateMovement();
    public virtual void Draw(SpriteBatch spriteBatch) =>
        sprite.Draw(spriteBatch, Position, Facing, AlternateFrame);

    public virtual void Reset()
    {
        elapsed = 0;
        Position = startPosition;
        Facing = Direction.Right;
        AlternateFrame = false;
    }
}
