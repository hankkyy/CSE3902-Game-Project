# Sprint 3 five-person task plan

All five assignments have 12 effort points and an initial estimate of 15 hours.
Sprint 3 runs from October 7, 2026 through October 26, 2026 at 8:00 PM
(America/New_York). The functionality check-in is October 19, 2026.

| Owner | Branch | Scope | Check-in deliverable | Final acceptance | Reviewer |
|---|---|---|---|---|---|
| Hank Zhang `@hankkyy` | `feature/s3-collision-core` | Shared collision contract/detection/dispatch (4 pts/5h); player-solid and boundary resolution (4/5h); tests/docs and contract coordination (4/5h) | Hitboxes, pair dispatch, and stable player collision against walls/blocks/bounds | Four-direction and diagonal resolution, deterministic reset, no collision rules in `Game1`, headless tests | Xuanzhe |
| Xuanzhe Li `@JojoLi132` | `feature/s3-room-loading` | Room schema/validation/loader (4/5h); five room data files plus all-object lab (4/5h); room rendering/tests/docs (4/5h) | External data loads at least five distinct rooms and an all-object test room | Unknown/invalid data is rejected, no room lists in `Game1`, every first-dungeon object type is covered | Michael |
| Michael Xu `@mo46-123` | `feature/s3-combat-collisions` | Enemy health/contact/environment responses (4/5h); sword/arrow/bomb/projectile responses (5/6h); boomerang, reset, tests/docs (3/4h) | Player/enemy damage, sword/arrow/bomb hits, enemy projectile and wall responses | Every required combat row in `COLLISION_MATRIX.md` passes; damage is not repeated per frame | Leo |
| Leo Zhuang `@Lzzz-7` | `feature/s3-items-blocks-collisions` | Inventory and pickup effects (4/5h); push block/door/key behavior (4/5h); persistence/reset/tests/docs (4/5h) | Real item pickup/removal and inventory updates | Push blocking, key door, room persistence and reset work without duplicate collection | Ashley |
| Ashley Zhang `@xing-gif` | `feature/s3-room-navigation-integration` | RoomManager/transitions/persistence (4/5h); mouse controller and HUD (3/4h); `Game1` integration, CI/analyzer/docs (5/6h) | Door and mouse navigation, current room HUD, integrated check-in build | Full reset and room persistence work; strict-analysis changes are recorded; clean-clone demo passes | Hank |

## Review ring

Each reviewer covers at least two named classes and records time:

- Xuanzhe reviews Hank's collision detector and dispatcher.
- Michael reviews Xuanzhe's room definition and loader.
- Leo reviews Michael's combat handler and enemy damage/state implementation.
- Ashley reviews Leo's item handler and push-block/door handler.
- Hank reviews Ashley's room manager and mouse controller.

## Integration order

1. Merge the additive collision and room contracts.
2. Merge room schema, loader, data files, and the all-object test room.
3. Merge collision core and player-solid response.
4. Merge inventory/items and combat responses independently.
5. Merge RoomManager, mouse input, HUD, and composition wiring.
6. Run normal verification, strict analysis, the manual check-in demo, and the first review pass.
7. Refactor, complete the second review focus, and finish documentation.

No owner may wait until the final days to expose a required API. Cross-owner API needs are raised in the issue and resolved through a small integration PR.

## GitHub tracking

- [#26 — Collision core and player-solid response](https://github.com/hankkyy/CSE3902-Game-Project/issues/26) — Hank
- [#23 — Data-driven room loading and all-object test level](https://github.com/hankkyy/CSE3902-Game-Project/issues/23) — Xuanzhe
- [#22 — Combat, enemy, and projectile collision responses](https://github.com/hankkyy/CSE3902-Game-Project/issues/22) — Michael
- [#24 — Item pickup, inventory, push blocks, and doors](https://github.com/hankkyy/CSE3902-Game-Project/issues/24) — Leo
- [#25 — Room navigation, mouse input, HUD, and integration](https://github.com/hankkyy/CSE3902-Game-Project/issues/25) — Ashley
- [Sprint 3 project board](https://github.com/users/hankkyy/projects/5)

All five issues begin in Todo with labels `sprint-3`, `effort-12`, and
`functionality-check-in`. Their project fields start at 12 Effort Points,
15 Original Estimate Hours, 15 Remaining Hours, and 0 Actual Hours. Their
Start Date is October 7, Functionality Check-in is October 19, and Target Date
is October 26, 2026.
