using GameProject.Sprites;
using Microsoft.Xna.Framework;

namespace GameProject.Objects.Blocks;

/// <summary>Water obstacle preview; remains stationary and non-interacting.</summary>
public sealed class WaterBlock(string name, Vector2 position, ISprite sprite)
    : BlockEntity(name, position, sprite);
