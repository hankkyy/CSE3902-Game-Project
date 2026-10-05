using GameProject.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameProject.Objects.Enemies;

/// <summary>Keeps a gallery preview and its play-area enemy on the same simulation clock.</summary>
public sealed class EnemyDisplayPair(IGameObject preview, IGameObject playArea) : IGameObject
{
    public string Name => preview.Name;

    public void Update(GameTime gameTime)
    {
        preview.Update(gameTime);
        playArea.Update(gameTime);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        preview.Draw(spriteBatch);
        playArea.Draw(spriteBatch);
    }

    public void Reset()
    {
        preview.Reset();
        playArea.Reset();
    }
}
