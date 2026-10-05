using GameProject.Core;
using GameProject.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameProject.Objects.Blocks;

/// <summary>Shared stationary block behavior without graphics-device dependencies.</summary>
public abstract class BlockEntity(string name, Vector2 position, ISprite sprite) : IGameObject
{
    public string Name { get; } = name;
    public Vector2 Position { get; } = position;

    public void Update(GameTime gameTime) { }
    public void Draw(SpriteBatch spriteBatch) => sprite.Draw(spriteBatch, Position, Direction.Down, false);
    public void Reset() { }
}
