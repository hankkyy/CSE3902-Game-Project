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
- Swept collision checks that stop the player from crossing a thin solid during a long frame.
- Active-room wiring that runs before exit checks in `RoomGameplay`.

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

`RoomSolidCollisionSystem` creates colliders for stone, push, water, statue, and crystal-pillar room objects. Rune tiles remain walkable floor decoration. Opening a door can call `SetActive(false)` on its collider when locked-door state is integrated.

## Automated verification

```sh
dotnet run --project tests/Collision/Collision.Tests.csproj --configuration Release
```

Coverage includes four movement directions, diagonal corners, thin-wall tunneling, fractional positions, reversed pair order, deactivation during a collision pass, all current solid block types, a player initially overlapping a solid, active-room integration, and reset.

## Review follow-up

Xuanzhe's review identified thin-wall tunneling, fractional correction, and repeated handling after deactivation. The detector now checks the player's swept path, correction snaps to the solid edge, and the system rechecks active state before every dispatch. Each case has a regression test.

## Strict analyzer decision

The collision types intentionally remain public even though CA1515 recommends
`internal`. Separate feature modules and the headless test project consume
these contracts, so changing them to internal would prevent the assigned
owners from registering and testing their handlers. All other strict warnings
introduced during this work were fixed; the remaining CA1515 results are this
documented API-boundary decision rather than blanket suppression.

## Manual verification

1. In Entrance Hall, walk into either stone block from each open side; the player stops at its edge.
2. In Quiet Water, walk into the water row and the stone block; neither can be crossed.
3. In Starlamp Court, walk across the rune tile, then into a crystal pillar; only the pillar blocks movement.
4. Press R after a collision and confirm the player returns to the starting position.
