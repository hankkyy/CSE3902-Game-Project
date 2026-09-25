using GameProject.Sprites;
using Microsoft.Xna.Framework;

namespace GameProject.Objects.Blocks;

/// <summary>Statue obstacle preview; remains stationary and non-interacting.</summary>
public sealed class StatueBlock(string name, Vector2 position, ISprite sprite)
    : BlockEntity(name, position, sprite);
