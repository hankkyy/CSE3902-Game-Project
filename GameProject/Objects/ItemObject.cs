using GameProject.Core;
using GameProject.Objects.Items;
using GameProject.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameProject.Objects;

public enum ItemKind { Heart, Rupee, Key, Bomb, Bow, Boomerang, Map, Compass, StarShard }

/// <summary>Compatibility entry point that delegates to a dedicated item implementation.</summary>
public sealed class ItemObject : IGameObject
{
    private readonly ItemEntity implementation;

    public ItemObject(string name, Vector2 position, ItemKind kind, SpriteFactory sprites)
        : this(name, position, kind, sprites.CreateItemSprite(kind)) { }

    /// <summary>Allows sprite injection for headless tests and future art integration.</summary>
    public ItemObject(string name, Vector2 position, ItemKind kind, ISprite sprite)
    {
        ArgumentNullException.ThrowIfNull(sprite);
        implementation = kind switch
        {
            ItemKind.Heart => new HeartItem(name, position, sprite),
            ItemKind.Rupee => new RupeeItem(name, position, sprite),
            ItemKind.Key => new KeyItem(name, position, sprite),
            ItemKind.Bomb => new BombItem(name, position, sprite),
            ItemKind.Bow => new BowItem(name, position, sprite),
            ItemKind.Boomerang => new BoomerangItem(name, position, sprite),
            ItemKind.Map => new MapItem(name, position, sprite),
            ItemKind.Compass => new CompassItem(name, position, sprite),
            ItemKind.StarShard => new StarShardItem(name, position, sprite),
            _ => throw new ArgumentOutOfRangeException(nameof(kind))
        };
    }

    public string Name => implementation.Name;
    public Vector2 Position => implementation.Position;
    public void Update(GameTime gameTime) => implementation.Update(gameTime);
    public void Draw(SpriteBatch spriteBatch) => implementation.Draw(spriteBatch);
    public void Reset() => implementation.Reset();
}
