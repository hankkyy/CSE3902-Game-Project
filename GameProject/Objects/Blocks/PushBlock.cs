using GameProject.Sprites;
using Microsoft.Xna.Framework;

namespace GameProject.Objects.Blocks;

/// <summary>Push-block presentation; interaction is intentionally deferred beyond Sprint 2.</summary>
public sealed class PushBlock(Vector2 position, ISprite sprite)
    : StationaryBlock("Push Block", position, sprite);
