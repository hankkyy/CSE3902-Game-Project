using GameProject.Sprites;
using Microsoft.Xna.Framework;

namespace GameProject.Objects.Blocks;

/// <summary>A stationary light-focusing pillar in the Starlight Ruins.</summary>
public sealed class CrystalPillar(string name, Vector2 position, ISprite sprite)
    : BlockEntity(name, position, sprite);
