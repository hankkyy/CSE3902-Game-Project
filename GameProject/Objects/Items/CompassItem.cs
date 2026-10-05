using GameProject.Sprites;
using Microsoft.Xna.Framework;

namespace GameProject.Objects.Items;

/// <summary>A compass pickup preview; no collection or navigation interaction yet.</summary>
public sealed class CompassItem(string name, Vector2 position, ISprite sprite)
    : ItemEntity(name, position, sprite);
