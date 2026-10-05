using GameProject.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameProject.Objects;

/// <summary>Keeps the preview and play-area copies synchronized.</summary>
public sealed class ObjectDisplayPair(
    IGameObject preview,
    IGameObject playArea) : IGameObject
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
