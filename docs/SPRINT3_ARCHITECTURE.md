# Sprint 3 architecture contract

## Dependency direction

```text
Game1 (composition only)
  ├─ Controllers -> Commands / room navigation
  ├─ RoomManager -> RoomLoader -> external room data
  ├─ CollisionSystem -> collision contracts + response handlers
  ├─ Rooms -> Objects + Sprites
  └─ Objects -> Core + Sprites
```

Room loading must not depend on input. Collision code must not poll input or draw. Feature modules communicate through small interfaces rather than referencing sibling implementations.

## Contract PR required before feature work

The integrator and affected owners must agree on additive equivalents of these concepts before implementation branches diverge:

```csharp
public interface ICollidable
{
    Rectangle Hitbox { get; }
    CollisionLayer CollisionLayer { get; }
    bool IsActive { get; }
}

public interface IDamageable
{
    void ReceiveDamage(int amount, Vector2 sourcePosition);
}

public interface IRoomObjectFactory
{
    IGameObject Create(RoomObjectDefinition definition);
}
```

Exact names may change in the contract PR, but the concepts must remain. Prefer additive interfaces over expanding `IGameObject` and breaking every Sprint 2 class.

## Room data boundary

Room files are data, not executable instructions. Each room needs:

- stable room ID and display name;
- neighbor/door links and entry points;
- object type, stable object ID, position, and optional properties;
- walls or a walkable boundary;
- enough validation to reject unknown types, duplicate IDs, missing fields, and invalid coordinates.

Use JSON, XML, or CSV. JSON is preferred for nested properties; XML/CSV are acceptable because the course handout explicitly recommends external files. No room-specific object list belongs in `Game1`.

## Runtime order

For each gameplay frame:

1. Read input and set intentions.
2. Update active-room objects.
3. Build collision candidates from active objects.
4. Detect intersections.
5. Dispatch each supported pair once.
6. Apply queued removals, deaths, pickups, and room transitions.
7. Draw the resulting active room.

Do not transition rooms or remove objects inside a collection enumeration.

## Persistence and reset

- Leaving and re-entering a room preserves collected items, defeated enemies, moved blocks, and unlocked doors for the current run.
- A full reset restores the player, inventory, current room, room object states, projectiles, and transition state.
- Loading the same room data twice produces the same initial state.

## Compatibility

Until integration explicitly removes the Sprint 2 gallery:

- preserve existing public constructors;
- preserve arrows/WASD, Z/N, 1–3, Enter, R, and Q/Escape;
- introduce new services through constructors or additive overloads;
- keep player, enemy, block, item, and sprite tests passing.
