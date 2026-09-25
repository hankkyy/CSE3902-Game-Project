using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameProject.Core;

public interface IGameObject
{
    string Name { get; }
    void Update(GameTime gameTime);
    void Draw(SpriteBatch spriteBatch);
    void Reset();
}
