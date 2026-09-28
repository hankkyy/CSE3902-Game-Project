using GameProject.Sprites;
using Microsoft.Xna.Framework;

namespace GameProject.Objects.Blocks;

/// <summary>Stationary water obstacle tile.</summary>
public sealed class WaterTile(Vector2 position, ISprite sprite)
    : StationaryBlock("Water Tile", position, sprite);
