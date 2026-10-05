using GameProject.Core;
using GameProject.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameProject.Objects.Items;

/// <summary>Shared item preview timing without graphics-device dependencies.</summary>
public abstract class ItemEntity(string name, Vector2 position, ISprite sprite) : IGameObject
{
    public string Name { get; } = name;
    public Vector2 Position { get; } = position;
    private const double AnimationCycleSeconds = 1.0 / 3.0;
    private double elapsed;

    public void Update(GameTime gameTime)
    {
        elapsed = (elapsed + gameTime.ElapsedGameTime.TotalSeconds) % AnimationCycleSeconds;
    }

    public void Draw(SpriteBatch spriteBatch) =>
        sprite.Draw(spriteBatch, Position, Direction.Down, elapsed >= AnimationCycleSeconds / 2);

    public void Reset() => elapsed = 0;
}
