# Architecture and integration contract

This contract keeps five independently generated Codex implementations compatible. Changes require team agreement and an integrator-owned PR before feature branches depend on them.

## Dependency direction

```text
Game1 (composition only)
  ├─ Controllers → Commands → Objects
  ├─ Objects → Core + Sprites contracts
  └─ Sprites → Core + MonoGame graphics

Core → MonoGame value types only
```

Feature modules must not reference sibling feature modules. For example, enemy code must not import player-state implementations, and item code must not call the keyboard controller.

## Frozen public contracts

Until the integrator approves a proposal, preserve these members and semantics:

```csharp
public interface IGameObject
{
    string Name { get; }
    void Update(GameTime gameTime);
    void Draw(SpriteBatch spriteBatch);
    void Reset();
}

public interface ISprite
{
    void Draw(SpriteBatch spriteBatch, Vector2 position,
              Direction direction, bool alternateFrame);
}

public interface ICommand
{
    void Execute();
}
```

Also preserve:

- `Direction`: `Down`, `Left`, `Right`, `Up`.
- `ObjectGallery<T>.Current`, `Count`, `Index`, `Next()`, `Previous()`, and `Reset()`.
- `Player(Vector2, Rectangle, SpriteFactory)` and its public input methods.
- Existing `BlockObject`, `ItemObject`, and `EnemyObject` constructor signatures until integration.
- `SpriteFactory(GraphicsDevice)` and all existing creation methods.

Additive overloads and new types are allowed inside an owned path. Breaking changes, renamed enum values, or moved types are not.

## Runtime invariants

- `Update` changes state; `Draw` must not change gameplay state.
- Drawing assumes `SpriteBatch.Begin()` has already been called and must not call `Begin()`/`End()` itself.
- Positions use screen pixels and refer to the top-left of the sprite.
- Timers use seconds from `GameTime`, never wall-clock time.
- `Reset()` is safe at any moment and returns the object to its original constructor-defined state.
- A single key press triggers a discrete command once; held movement is evaluated every frame.
- Objects in the right-side galleries do not collide or interact during Sprint 2.

## Integration seams

- Player-state work communicates visual state through existing player properties or additive player-owned types.
- Sprite work keeps the existing `ISprite.Draw` entry point and may hide richer animation internals behind it.
- Enemy, item, and block work keeps `IGameObject` as the common surface.
- Menu/HUD/input work is the only feature branch allowed to edit `Game1`; it must preserve all existing object construction and controls.
- The integrator resolves approved cross-module wiring after feature PRs are individually green.

## Proposal template

Create `docs/architecture-proposals/<branch-name>.md` with:

1. Problem that cannot be solved through the current contract.
2. Minimal proposed signature change.
3. Existing callers affected.
4. Migration and backward-compatibility plan.
5. Approval from affected owners.
