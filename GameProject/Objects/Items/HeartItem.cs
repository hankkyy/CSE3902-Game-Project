using GameProject.Sprites;
using Microsoft.Xna.Framework;

namespace GameProject.Objects.Items;

/// <summary>Heart pickup preview; collection and use belong to a later sprint.</summary>
public sealed class HeartItem(string name, Vector2 position, ISprite sprite)
    : ItemEntity(name, position, sprite);
