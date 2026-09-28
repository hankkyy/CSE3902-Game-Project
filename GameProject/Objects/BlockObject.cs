using GameProject.Core;
using GameProject.Objects.Blocks;
using GameProject.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameProject.Objects;

public enum BlockKind { Stone, Push, Water, Statue }

public sealed class BlockObject : IGameObject
{
    private readonly IGameObject block;

    public BlockObject(string name, Vector2 position, BlockKind kind, SpriteFactory sprites)
    {
        Name = name;
        ISprite sprite = sprites.CreateBlockSprite(kind);
        block = kind switch
        {
            BlockKind.Stone => new StoneBlock(position, sprite),
            BlockKind.Push => new PushBlock(position, sprite),
            BlockKind.Water => new WaterTile(position, sprite),
            _ => new StatueBlock(position, sprite)
        };
    }

    public string Name { get; }
    public void Update(GameTime gameTime) => block.Update(gameTime);
    public void Draw(SpriteBatch spriteBatch) => block.Draw(spriteBatch);
    public void Reset() => block.Reset();
}
