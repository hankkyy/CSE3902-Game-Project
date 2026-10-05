using GameProject.Sprites;
using Microsoft.Xna.Framework;

namespace GameProject.Objects.Items;

/// <summary>The original relic reward preview for the Starlight Ruins.</summary>
public sealed class StarShardItem(string name, Vector2 position, ISprite sprite)
    : ItemEntity(name, position, sprite);
