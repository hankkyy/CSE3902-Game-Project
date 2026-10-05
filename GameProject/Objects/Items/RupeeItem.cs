using GameProject.Sprites;
using Microsoft.Xna.Framework;

namespace GameProject.Objects.Items;

/// <summary>Rupee pickup preview; collection and use belong to a later sprint.</summary>
public sealed class RupeeItem(string name, Vector2 position, ISprite sprite)
    : ItemEntity(name, position, sprite);
