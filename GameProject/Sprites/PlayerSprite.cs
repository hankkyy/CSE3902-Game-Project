using GameProject.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameProject.Sprites;

/// <summary>Draws the original player atlas without advancing time or owning gameplay state.</summary>
public sealed class PlayerSprite : IAnimatedPlayerSprite
{
    private readonly Texture2D texture;

    /// <summary>The caller retains ownership of the texture and must outlive the sprite.</summary>
    public PlayerSprite(Texture2D texture)
    {
        ArgumentNullException.ThrowIfNull(texture);
        if (texture.Width != PlayerSpriteFrames.Columns * PlayerSpriteFrames.CellSize ||
            texture.Height != PlayerSpriteFrames.Rows * PlayerSpriteFrames.CellSize)
        {
            throw new ArgumentException("The player atlas must be 160 by 640 pixels.", nameof(texture));
        }

        this.texture = texture;
    }

    /// <summary>Preserves the starter's two-frame visual signal; it cannot encode attacks or damage.</summary>
    public void Draw(SpriteBatch spriteBatch, Vector2 position, Direction direction, bool alternateFrame)
    {
        PlayerSpriteAnimation animation;
        double seconds;
        if (alternateFrame)
        {
            animation = PlayerSpriteAnimation.Walking;
            seconds = PlayerSpriteFrames.Clip(animation).FrameSeconds;
        }
        else
        {
            animation = PlayerSpriteAnimation.Idle;
            seconds = 0;
        }

        Draw(spriteBatch, position, direction, animation, seconds);
    }

    /// <summary>Samples a full clip using the time since its state began, accumulated in Update.</summary>
    public void Draw(SpriteBatch spriteBatch, Vector2 position, Direction direction,
        PlayerSpriteAnimation animation, double elapsedSeconds)
    {
        ArgumentNullException.ThrowIfNull(spriteBatch);
        Rectangle source = PlayerSpriteFrames.Source(animation, direction, elapsedSeconds);
        Rectangle destination = PlayerSpriteFrames.Destination(position);
        spriteBatch.Draw(texture, destination, source, Color.White);
    }
}
