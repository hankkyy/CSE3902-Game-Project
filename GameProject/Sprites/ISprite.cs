using GameProject.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameProject.Sprites;

public interface ISprite
{
    void Draw(SpriteBatch spriteBatch, Vector2 position, Direction direction, bool alternateFrame);
}
