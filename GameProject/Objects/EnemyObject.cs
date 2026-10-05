using GameProject.Core;
using GameProject.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameProject.Objects;

public enum EnemyKind { Octorok, Keese, Gel, OldMan, RuneWisp, ClockworkBeetle, PrismSentinel }

public sealed class EnemyObject : IGameObject
{
    private readonly IGameObject character;

    public EnemyObject(string name, Vector2 position, EnemyKind kind, SpriteFactory sprites)
        : this(name, position, kind, sprites.CreateEnemySprite(kind), kind switch
        {
            EnemyKind.Octorok => sprites.CreateItemSprite(ItemKind.Bomb),
            EnemyKind.PrismSentinel => sprites.CreateLightBoltSprite(),
            _ => null
        })
    {
    }

    /// <summary>Allows all character kinds and their shots to be verified without a graphics device.</summary>
    public EnemyObject(string name, Vector2 position, EnemyKind kind, ISprite sprite, ISprite? projectileSprite = null)
    {
        Name = name;
        character = kind switch
        {
            EnemyKind.Octorok => new Enemies.Octorok(name, position, sprite,
                projectileSprite ?? throw new ArgumentNullException(nameof(projectileSprite))),
            EnemyKind.Keese => new Enemies.Keese(name, position, sprite),
            EnemyKind.Gel => new Enemies.Gel(name, position, sprite),
            EnemyKind.OldMan => new Enemies.Npc(name, position, sprite),
            EnemyKind.RuneWisp => new Enemies.RuneWisp(name, position, sprite),
            EnemyKind.ClockworkBeetle => new Enemies.ClockworkBeetle(name, position, sprite),
            EnemyKind.PrismSentinel => new Enemies.PrismSentinel(name, position, sprite,
                projectileSprite ?? throw new ArgumentNullException(nameof(projectileSprite))),
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
