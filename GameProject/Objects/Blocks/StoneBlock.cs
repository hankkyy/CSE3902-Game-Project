using GameProject.Sprites;
using Microsoft.Xna.Framework;

namespace GameProject.Objects.Blocks;

/// <summary>Immovable stone dungeon block.</summary>
public sealed class StoneBlock(Vector2 position, ISprite sprite)
    : StationaryBlock("Stone Block", position, sprite);
