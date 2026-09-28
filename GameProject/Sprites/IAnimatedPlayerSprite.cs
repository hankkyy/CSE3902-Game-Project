using GameProject.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameProject.Sprites;

/// <summary>Adds explicit visual state and elapsed time without changing the frozen ISprite contract.</summary>
public interface IAnimatedPlayerSprite : ISprite
{
    void Draw(SpriteBatch spriteBatch, Vector2 position, Direction direction,
        PlayerSpriteAnimation animation, double elapsedSeconds);
}
