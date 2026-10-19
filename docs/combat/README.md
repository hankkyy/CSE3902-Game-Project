# Michael's Sprint 3 combat code

Branch: `feature/s3-combat-collisions`

Assignment: [#22](https://github.com/hankkyy/CSE3902-Game-Project/issues/22)

Required reviewer: Leo (`@Lzzz-7`)

## Current status

The combat responses and their headless tests are implemented. The shared
collision interfaces, dispatcher, and room loop do not exist on the starting
branch (`c2ae73d`). Connecting these responses to the playable room build is
still pending team integration. Running the normal game currently opens the
Sprint 2 gallery; it does not demonstrate the new combat automatically.

No changes were made to `Game1`, shared `Core` interfaces, room loading,
inventory, blocks, controllers, project files, or build scripts. Small additions
to `Player`, `EnemyObject`, and player projectiles expose the real objects and
their combat state; no second player, weapon, or health simulation is created.

See [the integration proposal](../architecture-proposals/s3-combat-collisions.md)
before wiring the module into the shared dispatcher.

## Read the code in this order

1. `GameProject/Objects/Enemies/EnemyHealth.cs`: stores health. If health is zero,
   ignore damage. Otherwise subtract damage, stopping at zero. Reset restores 3 HP.
2. `GameProject/Combat/CombatCollisionHandler.cs`: one small method per collision
   pair. Each method first checks whether the objects are active and touching.
   It then applies the appropriate response.
3. `GameProject/Objects/Player.cs`: `ReceiveContactDamage` uses the existing
   damaged state for 0.45 seconds of invulnerability, then moves the player
   12 pixels away from the damage source. Room bounds clamp that movement.
4. `GameProject/Objects/Enemies/EnemyCharacter.cs`: dead enemies skip update and
   draw. Wall corrections are retained when the procedural patrol updates again.
5. `tests/Enemies/CombatChecks.cs`: examples using the real objects without a
   graphics device.

The arrow response follows this logic:

```csharp
if (arrow.IsActive && enemy.IsActive && enemy.IsHostile &&
    arrow.Hitbox.Intersects(enemy.Hitbox))
{
    enemy.ReceiveDamage(1);
    arrow.Deactivate();
}
```

Sword swings, explosions, and boomerang throws remember which enemies they
already hit in a `HashSet`. Think of it as a list that cannot contain duplicates:
`Add(enemy)` returns true only the first time. A successful new attack clears
the list. Reset clears it too. A rejected attack input does not clear it.

## Implemented responses

| Pair | Behavior |
|---|---|
| Player / hostile enemy | One health point, knockback, existing invulnerability window |
| Enemy projectile / player | One hit; consume shot even if invulnerability prevents damage |
| Sword / enemy | One point per enemy per swing, in all four facing directions |
| Arrow / enemy | One point; consume arrow after the first target |
| Arrow / solid | Consume arrow |
| Bomb explosion / enemy | One point per enemy per explosion; fuse does no damage |
| Boomerang / enemy | One point per enemy per throw; start returning |
| Boomerang / solid | Correct overlap toward entry side, then return |
| Enemy / solid | Correct overlap and retain correction on later movement updates |
| Enemy projectile / solid | Consume shot |

The friendly Lantern Keeper is neither a damage target nor a contact hazard.
Defeated enemies stop moving, drawing, colliding, and firing. Their existing
shots are also deactivated. Octorok and Sentinel shots launch only when a new
scheduled shot begins, so a consumed shot cannot reappear on the next frame.

These are team implementation choices, not claimed original-game values:
all hostile enemies start with 3 HP; each weapon deals 1 damage; the boomerang
deals damage rather than stun; the sword extends 20 pixels; dead shooters lose
their outstanding shot. Bomb/breakable-wall behavior is excluded stretch work.

## Verification

From this checkout in Git Bash:

```sh
dotnet run --project tests/Enemies/Enemies.Tests.csproj --configuration Release
bash scripts/verify.sh
bash scripts/analyze-strict.sh
```

The existing enemy test entry point runs the new combat checks, so the shared
verification script already includes them without changing the script or any
project file. Tests cover repeated-frame damage, fresh attacks, distinct targets,
all sword directions, boomerang return, explosion/visual alignment, NPC safety,
dead objects, wall correction, consumed-shot lifetime, and reset.

## Manual demonstration after team integration

These steps are required before marking the issue complete; they have not yet
been run against an integrated room build.

1. Enter the all-object room and walk into an enemy. Health should drop once,
   with knockback and flashing. Continued contact must not drain health per frame.
2. Let an enemy shot hit the player. The shot disappears. Repeat during flashing:
   the shot disappears without another health loss.
3. Use Z/N in each direction. One swing can damage several enemies, but each
   enemy loses only one point for that swing.
4. Press 1 to fire arrows at enemies and walls. Each arrow disappears at its
   first valid target; a second overlapping enemy must not also take damage.
5. Press 3 near two enemies. The fuse is harmless. Each enemy in the explosion
   takes one hit. Place a bomb at the room edge and check its damage matches its
   visible explosion.
6. Press 2 toward an enemy, then toward each wall. The boomerang turns back and
   can be thrown again after it returns. It must not hit an enemy twice per throw.
7. Defeat an Octorok and Sentinel. Their sprites, movement, and shots stop.
   Leave and re-enter; the room manager must preserve their defeated state.
8. Push enemies into a corner and take contact damage beside a wall. Verify the
   shared solid resolver prevents penetration after combat knockback.
9. Press R with attacks active and enemies defeated. Player health, enemy health,
   positions, clocks, weapons, and per-attack hit memories return to initial state.

Leo should review `CombatCollisionHandler` for readability and `EnemyHealth` /
`EnemyCharacter` for maintainability. Review date, time, findings, final room
demonstration, and a screenshot/GIF remain to be recorded by the team.
