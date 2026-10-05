using GameProject.Sprites;
using Microsoft.Xna.Framework;

namespace GameProject.Objects.Items;

/// <summary>A bow pickup preview; firing remains the player item-use feature.</summary>
public sealed class BowItem(string name, Vector2 position, ISprite sprite)
    : ItemEntity(name, position, sprite);
