using GameProject.Core;
using GameProject.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameProject.Objects;

public enum BlockKind { Stone, Push, Water, Statue }

public sealed class BlockObject : IGameObject
{
    private readonly Vector2 position;
    private readonly ISprite sprite;

    public BlockObject(string name, Vector2 position, BlockKind kind, SpriteFactory sprites)
    {
        Name = name;
        this.position = position;
        sprite = sprites.CreateBlockSprite(kind);
    }

    public string Name { get; }
    public void Update(GameTime gameTime) { }
    public void Draw(SpriteBatch spriteBatch) => sprite.Draw(spriteBatch, position, Direction.Down, false);
    public void Reset() { }
}
