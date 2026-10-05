using GameProject.Sprites;
using Microsoft.Xna.Framework;

namespace GameProject.Objects.Enemies;

/// <summary>Stationary NPC using the shared idle animation clock.</summary>
public sealed class Npc(string name, Vector2 position, ISprite sprite) : EnemyCharacter(name, position, sprite)
{
    protected override void UpdateMovement() => Position = startPosition;
}
