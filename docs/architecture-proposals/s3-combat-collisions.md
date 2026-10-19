# Combat integration request for Sprint 3

Owner: Michael (`@mo46-123`). Related issue: #22.

At the starting commit `c2ae73d`, both the assigned branch and
`integration/sprint3` contain planning documents but no shared `ICollidable`,
`IDamageable`, collision layers, detector, or dispatcher. This feature does not
invent competing shared contracts. Combat responses are independently testable
methods; the integrator's contract PR must settle the adapters before live wiring.

## Requested contract and dispatcher wiring

Hank/integrator should map the agreed collision categories to the following
existing properties and response methods. Names here are the feature APIs, not
a proposed replacement for the team's shared contract.

| Runtime object | Hitbox / activity | Response |
|---|---|---|
| Player body | `player.Hitbox`, `player.IsActive` | `PlayerTouchesEnemy`, `EnemyProjectileHitsPlayer` |
| Player sword | `player.SwordHitbox`; active only while attacking and alive | `SwordHitsEnemy` |
| Player arrow | `player.Arrow.Hitbox`, `IsActive` | `ArrowHitsEnemy`, `ArrowHitsSolid` |
| Player boomerang | `player.Boomerang.Hitbox`, `IsActive` | `BoomerangHitsEnemy`, `BoomerangHitsSolid` |
| Bomb explosion | `player.Bomb.ExplosionHitbox`, `IsExploding` | `BombHitsEnemy` |
| Enemy character | `enemy.Hitbox`, `IsActive`, `IsHostile` | `EnemyHitsSolid` and weapon/player responses |
| Enemy shot | `shooter.Projectile.Hitbox`, `IsActive` | `EnemyProjectileHitsPlayer`, `EnemyProjectileHitsSolid` |

The methods live on `GameProject.Combat.CombatCollisionHandler`. Pass the
actual objects (and the solid's world-space rectangle), not copies. Optional
player weapons can be null when a test constructs a player without item sprites.
`EnemyObject.Character` exposes the actual underlying enemy for room factories
that still use the Sprint 2 wrapper.

Specific integration requirements:

1. Merge the additive shared contract first. Adapt these objects without expanding
   `IGameObject` or implementing a second detector on Michael's branch.
2. Detect each unordered pair once. Normalize pair order before calling the
   named typed handler. The handlers recheck activity so a projectile deactivated
   by an earlier pair cannot hit again in the same frame.
3. Only register real solid geometry. Water, decorative tiles, open doors, etc.
   must follow the room/block owner's agreed collision category.
4. Use the same weapon instances owned by `Player`; it already updates/draws them.
   Enemy shooters already update/draw their shots. Do not update these child
   objects a second time merely because they also have collision registrations.
5. Finish combat before the final player-solid resolution pass. Contact knockback
   is clamped to room bounds, but only Hank's solid resolver knows interior walls.
   Resolve those walls after knockback, including corners.
6. Register the boomerang with its prior/current positions. The handler corrects
   detected overlap toward the entry side. Fast objects that entirely skip a thin
   wall require the shared detector's swept candidates or a fixed simulation step;
   these response methods do not provide broad-phase or swept collision detection.
7. Keep defeated enemies in the room's persistent state or record their defeat
   before queued removal. Re-entering a room must not call `Reset` on survivors
   or defeated enemies. Full reset calls `Player.Reset` and each enemy's `Reset`.
8. Queue removals and transitions until dispatch ends. This feature only changes
   activity/health; it never removes objects from collections during iteration.

## Files deliberately left to other owners

- Hank: `Core`, `Collision`, shared adapter contracts and player-solid resolution.
- Ashley: room update/dispatch ordering, `Game1`, HUD, transitions and persistence.
- Xuanzhe: room parsing/data/factory composition.
- Leo: pickups, inventory, push blocks and doors.

Michael's additions to `Player.cs` are restricted to combat hitboxes, weapon
access, sword hit memory, damage/knockback, and defeated-player attack/movement
guards. Player-item changes are the projectile collision integration explicitly
assigned to this feature. Existing constructors, input commands, and item motion
remain compatible. No changes to shared contracts or another owner's production
directories are included.

## Merge gate

Do not describe the room game or check-in as finished from the headless tests
alone. After contracts and dispatcher are merged, verify these adapters, run the
manual steps in `docs/combat/README.md`, obtain Leo's review, then merge through
`integration/sprint3`. Do not push this feature directly to `main`.
