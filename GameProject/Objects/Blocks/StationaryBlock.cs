using GameProject.Core;
using GameProject.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameProject.Objects.Blocks;

/// <summary>Shared stationary, non-interacting behavior for gallery blocks.</summary>
public abstract class StationaryBlock : IGameObject
{
    private readonly Vector2 position;
    private readonly ISprite sprite;

    protected StationaryBlock(string name, Vector2 position, ISprite sprite)
    {
        Name = name;
        this.position = position;
        this.sprite = sprite;
    }

    public string Name { get; }

    public void Update(GameTime gameTime)
    {
    }

    public void Draw(SpriteBatch spriteBatch) => sprite.Draw(spriteBatch, position, Direction.Down, false);

    public void Reset()
    {
    }
}
