using GameProject.Sprites;
using Microsoft.Xna.Framework;

namespace GameProject.Objects.Blocks;

/// <summary>A stationary engraved floor tile; its later puzzle role is not active in Sprint 2.</summary>
public sealed class RuneTile(string name, Vector2 position, ISprite sprite)
    : BlockEntity(name, position, sprite);
