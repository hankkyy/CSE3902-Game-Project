using GameProject.Sprites;
using Microsoft.Xna.Framework;

namespace GameProject.Objects.Items;

/// <summary>A map pickup preview for the planned Starlight Ruins rooms.</summary>
public sealed class MapItem(string name, Vector2 position, ISprite sprite)
    : ItemEntity(name, position, sprite);
