using GameProject.Sprites;
using Microsoft.Xna.Framework;

namespace GameProject.Objects.Items;

/// <summary>Key pickup preview; collection and use belong to a later sprint.</summary>
public sealed class KeyItem(string name, Vector2 position, ISprite sprite)
    : ItemEntity(name, position, sprite);
