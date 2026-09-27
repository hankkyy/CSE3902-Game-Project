using GameProject.Core;
using GameProject.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameProject.Objects;

public enum EnemyKind { Octorok, Keese, Gel, OldMan }

public sealed class EnemyObject : IGameObject
{
    private readonly IGameObject character;

    public EnemyObject(string name, Vector2 position, EnemyKind kind, SpriteFactory sprites)
    {
        Name = name;
        ISprite sprite = sprites.CreateEnemySprite(kind);
        character = kind switch
        {
            EnemyKind.Octorok => new Enemies.Octorok(name, position, sprite, sprites.CreateItemSprite(ItemKind.Bomb)),
            EnemyKind.Keese => new Enemies.Keese(name, position, sprite),
            EnemyKind.Gel => new Enemies.Gel(name, position, sprite),
            EnemyKind.OldMan => new Enemies.Npc(name, position, sprite),
            _ => throw new ArgumentOutOfRangeException(nameof(kind))
        };
    }

    public string Name { get; }
    public void Update(GameTime gameTime) => character.Update(gameTime);

    public void Draw(SpriteBatch spriteBatch)
    {
        character.Draw(spriteBatch);
    }

    public void Reset() => character.Reset();
}
