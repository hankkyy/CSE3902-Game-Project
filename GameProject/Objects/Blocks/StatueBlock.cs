using GameProject.Sprites;
using Microsoft.Xna.Framework;

namespace GameProject.Objects.Blocks;

/// <summary>Stationary dungeon statue obstacle.</summary>
public sealed class StatueBlock(Vector2 position, ISprite sprite)
    : StationaryBlock("Statue", position, sprite);
