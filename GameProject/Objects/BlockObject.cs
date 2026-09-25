using GameProject.Core;
using GameProject.Objects.Blocks;
using GameProject.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameProject.Objects;

public enum BlockKind { Stone, Push, Water, Statue }

/// <summary>Compatibility entry point that delegates to a dedicated block implementation.</summary>
public sealed class BlockObject : IGameObject
{
    private readonly BlockEntity implementation;

    public BlockObject(string name, Vector2 position, BlockKind kind, SpriteFactory sprites)
        : this(name, position, kind, sprites.CreateBlockSprite(kind)) { }

    /// <summary>Allows sprite injection for headless tests and future art integration.</summary>
    public BlockObject(string name, Vector2 position, BlockKind kind, ISprite sprite)
    {
        ArgumentNullException.ThrowIfNull(sprite);
        implementation = kind switch
        {
            BlockKind.Stone => new StoneBlock(name, position, sprite),
            BlockKind.Push => new PushBlock(name, position, sprite),
            BlockKind.Water => new WaterBlock(name, position, sprite),
            BlockKind.Statue => new StatueBlock(name, position, sprite),
            _ => throw new ArgumentOutOfRangeException(nameof(kind))
        };
    }

    public string Name => implementation.Name;
    public Vector2 Position => implementation.Position;
    public void Update(GameTime gameTime) => implementation.Update(gameTime);
    public void Draw(SpriteBatch spriteBatch) => implementation.Draw(spriteBatch);
    public void Reset() => implementation.Reset();
}
