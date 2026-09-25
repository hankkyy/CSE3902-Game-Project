using GameProject.Core;
using GameProject.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameProject.Objects;

public enum ItemKind { Heart, Rupee, Key, Bomb }

public sealed class ItemObject : IGameObject
{
    private readonly Vector2 startPosition;
    private readonly ISprite sprite;
    private double elapsed;

    public ItemObject(string name, Vector2 position, ItemKind kind, SpriteFactory sprites)
    {
        Name = name;
        startPosition = position;
        sprite = sprites.CreateItemSprite(kind);
    }

    public string Name { get; }
    public void Update(GameTime gameTime) => elapsed += gameTime.ElapsedGameTime.TotalSeconds;

    public void Draw(SpriteBatch spriteBatch)
    {
        Vector2 bobbed = startPosition + new Vector2(0, (float)Math.Sin(elapsed * 4) * 7);
        sprite.Draw(spriteBatch, bobbed, Direction.Down, ((int)(elapsed * 6) % 2) == 1);
    }

    public void Reset() => elapsed = 0;
}
