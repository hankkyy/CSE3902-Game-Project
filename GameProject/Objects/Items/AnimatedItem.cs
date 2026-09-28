using GameProject.Core;
using GameProject.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameProject.Objects.Items;

/// <summary>Frame-rate-independent base behavior for non-interacting gallery items.</summary>
public abstract class AnimatedItem : IGameObject
{
    private readonly Vector2 startPosition;
    private readonly ISprite sprite;
    private double elapsedSeconds;

    protected AnimatedItem(string name, Vector2 position, ISprite sprite)
    {
        Name = name;
        startPosition = position;
        this.sprite = sprite;
    }

    public string Name { get; }

    protected double ElapsedSeconds => elapsedSeconds;

    public void Update(GameTime gameTime) => elapsedSeconds += gameTime.ElapsedGameTime.TotalSeconds;

    public void Draw(SpriteBatch spriteBatch) => sprite.Draw(
        spriteBatch,
        startPosition + AnimationOffset(elapsedSeconds),
        Direction.Down,
        AlternateFrame(elapsedSeconds));

    public void Reset() => elapsedSeconds = 0;

    protected abstract Vector2 AnimationOffset(double seconds);

    protected abstract bool AlternateFrame(double seconds);
}
