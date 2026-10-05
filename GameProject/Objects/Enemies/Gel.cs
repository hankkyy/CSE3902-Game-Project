using GameProject.Sprites;
using Microsoft.Xna.Framework;

namespace GameProject.Objects.Enemies;

/// <summary>Pauses on the ground before each short hop.</summary>
public sealed class Gel(string name, Vector2 position, ISprite sprite) : EnemyCharacter(name, position, sprite)
{
    protected override void UpdateMovement()
    {
        const double cycleSeconds = 1.6;
        const double restSeconds = 0.8;
        const float hopHeight = 24;
        double phase = elapsed % cycleSeconds;
        float height = phase <= restSeconds ? 0 :
            (float)Math.Sin((phase - restSeconds) / (cycleSeconds - restSeconds) * Math.PI) * hopHeight;
        Position = startPosition - new Vector2(0, height);
    }
}
