# Sprint 3 task — collision core

Owner: Hank Zhang (`@hankkyy`)  
Reviewer: Xuanzhe Li (`@JojoLi132`)  
Estimate: 12 points / 15 hours

Implement only the shared collision infrastructure and player-solid/boundary response assigned in `docs/SPRINT3_TASKS.md`. Start with the approved additive contract from the integration branch. Separate broad-phase/pair detection from response dispatch. Hitboxes and resolution must be testable without a graphics device.

Required:

- collision category and active hitbox contract;
- unordered pair detection exactly once per frame;
- dispatcher/handler seam used by other owners;
- stable player response against walls, solid blocks, room bounds, and closed doors;
- four-direction, diagonal, corner, elapsed-time, and reset tests;
- no gameplay-specific collision switch in `Game1`.

Do not implement item effects, enemy damage, room parsing, mouse navigation, or another owner's response rules. If a shared API is missing, propose it on `integration/sprint3` before depending on it.

Check-in is complete only when required rows owned by Hank in `docs/COLLISION_MATRIX.md` pass automated tests and manual wall/corner checks. Run both verification scripts and request review of the collision detector and dispatcher.
