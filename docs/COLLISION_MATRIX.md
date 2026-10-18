# Sprint 3 collision matrix

This matrix defines the required collision pairs and prevents duplicate or contradictory implementations. The listed owner implements the response; Hank's collision core provides detection and dispatch infrastructure.

| Pair | Required response | Owner | Check-in |
|---|---|---|---|
| Player - wall/solid block | Resolve penetration and stop movement | Hank | Required |
| Player - room boundary/closed door | Block movement or queue valid room transition | Hank/Ashley | Required |
| Player - pickup item | Apply item effect, update inventory, queue removal | Leo | Required |
| Player - enemy | Damage/knockback with invulnerability window | Michael | Required |
| Player - enemy projectile | Damage player once and deactivate projectile | Michael | Required |
| Sword hitbox - enemy | Damage enemy once per attack | Michael | Required |
| Arrow - enemy | Damage enemy and deactivate arrow | Michael | Required |
| Arrow - wall/solid block | Deactivate arrow | Michael | Required |
| Boomerang - enemy | Damage or stun once, then return | Michael | Final |
| Boomerang - wall/solid block | Begin return without entering solid geometry | Michael | Final |
| Bomb explosion - enemy | Damage once during explosion | Michael | Required |
| Bomb explosion - breakable wall | Change wall/door state if implemented | Leo/Michael | Stretch |
| Enemy - wall/solid block | Resolve or change movement direction | Michael | Required |
| Enemy projectile - wall/solid block | Deactivate projectile | Michael | Required |
| Player - push block | Move one grid step only when destination is free | Leo | Final |
| Push block - wall/block | Prevent push | Leo | Final |
| Player - open door/room exit | Queue transition and place at destination entry | Ashley | Required |

General rules:

- Process an unordered pair only once per frame.
- Inactive/dead/collected objects do not collide.
- Damage events are idempotent for a single attack or explosion.
- Hitboxes must be testable without a graphics device.
- Collision response never calls `Draw` and never polls keyboard or mouse state.
