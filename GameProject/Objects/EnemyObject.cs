using GameProject.Core;
using GameProject.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameProject.Objects;

public enum EnemyKind { Octorok, Keese, Gel, OldMan }

public sealed class EnemyObject : IGameObject
{
    private readonly Vector2 startPosition;
    private readonly EnemyKind kind;
    private readonly ISprite sprite;
    private double elapsed;

    public EnemyObject(string name, Vector2 position, EnemyKind kind, SpriteFactory sprites)
    {
        Name = name;
        startPosition = position;
        this.kind = kind;
        sprite = sprites.CreateEnemySprite(kind);
    }

    public string Name { get; }
    public void Update(GameTime gameTime) => elapsed += gameTime.ElapsedGameTime.TotalSeconds;

    public void Draw(SpriteBatch spriteBatch)
    {
        Vector2 offset = kind switch
        {
            EnemyKind.Octorok => new Vector2((float)Math.Sin(elapsed * 1.8) * 48, 0),
            EnemyKind.Keese => new Vector2((float)Math.Sin(elapsed * 3) * 55, (float)Math.Cos(elapsed * 4) * 18),
            EnemyKind.Gel => new Vector2(0, Math.Abs((float)Math.Sin(elapsed * 2.5)) * -28),
            _ => Vector2.Zero
        };
        Direction facing = Math.Cos(elapsed * 1.8) < 0 ? Direction.Left : Direction.Right;
        sprite.Draw(spriteBatch, startPosition + offset, facing, ((int)(elapsed * 7) % 2) == 1);
    }

    public void Reset() => elapsed = 0;
}
