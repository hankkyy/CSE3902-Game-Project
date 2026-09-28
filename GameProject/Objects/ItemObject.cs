using GameProject.Core;
using GameProject.Objects.Items;
using GameProject.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameProject.Objects;

public enum ItemKind { Heart, Rupee, Key, Bomb }

public sealed class ItemObject : IGameObject
{
    private readonly IGameObject item;

    public ItemObject(string name, Vector2 position, ItemKind kind, SpriteFactory sprites)
    {
        Name = name;
        ISprite sprite = sprites.CreateItemSprite(kind);
        item = kind switch
        {
            ItemKind.Heart => new HeartItem(position, sprite),
            ItemKind.Rupee => new RupeeItem(position, sprite),
            ItemKind.Key => new KeyItem(position, sprite),
            _ => new BombItem(position, sprite)
        };
    }

    public string Name { get; }
    public void Update(GameTime gameTime) => item.Update(gameTime);
    public void Draw(SpriteBatch spriteBatch) => item.Draw(spriteBatch);
    public void Reset() => item.Reset();
}
