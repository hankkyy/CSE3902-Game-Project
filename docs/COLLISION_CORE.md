# Sprint 3 collision core

Owner: Hank Zhang  
Required reviewer: Xuanzhe Li

## Implemented scope

- Additive `ICollidable` and `IMovableCollidable` contracts.
- Explicit collision layers for later item, enemy, projectile, and room-exit handlers.
- AABB detection that skips inactive objects and emits each unordered pair once.
- A dispatcher that lets each feature register a focused response handler.
- Player-solid response for walls, stationary solid blocks, room boundaries, and closed doors.
- Previous-position response with a smallest-overlap fallback for initial/spawned intersections.

The collision code does not draw, poll input, remove room objects, or contain item/enemy rules. Other owners add handlers instead of editing the detector.

## Using the core

```csharp
CollisionDispatcher dispatcher = new(
[
    new PlayerSolidCollisionHandler()
    // Other feature handlers are registered during integration.
]);
CollisionSystem collisions = new(dispatcher);
collisions.Process(activeRoomCollidables);
```

Represent room walls, boundaries, and closed doors with `SolidCollider` until a visible block class exposes the same `ICollidable` contract. Opening a door can call `SetActive(false)`.

## Automated verification

```sh
dotnet run --project tests/Collision/Collision.Tests.csproj --configuration Release
```

Coverage includes four movement directions, diagonal corners, a large elapsed-time step, reversed pair order, inactive objects, unique pairs, unsupported pairs, a player initially overlapping a solid, and reset.

## Strict analyzer decision

The collision types intentionally remain public even though CA1515 recommends
`internal`. Separate feature modules and the headless test project consume
these contracts, so changing them to internal would prevent the assigned
owners from registering and testing their handlers. All other strict warnings
introduced during this work were fixed; the remaining CA1515 results are this
documented API-boundary decision rather than blanket suppression.

## Manual verification

After room integration:

1. Hold each movement direction against every wall and closed doorway.
2. Move diagonally into all four wall corners; confirm the player does not enter or jitter inside the wall.
3. Open a door and confirm the disabled door collider no longer blocks movement.
4. Press R after a collision and confirm the player and hitbox return to the starting position.
