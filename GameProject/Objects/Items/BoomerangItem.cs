using GameProject.Sprites;
using Microsoft.Xna.Framework;

namespace GameProject.Objects.Items;

/// <summary>A boomerang pickup preview, separate from the thrown projectile.</summary>
public sealed class BoomerangItem(string name, Vector2 position, ISprite sprite)
    : ItemEntity(name, position, sprite);
