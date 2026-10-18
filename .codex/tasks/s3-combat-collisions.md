# Sprint 3 task — combat collisions

Owner: Michael Xu (`@mo46-123`)  
Reviewer: Leo Zhuang (`@Lzzz-7`)  
Estimate: 12 points / 15 hours

Implement enemy health/state and combat/environment responses assigned to Michael in `docs/COLLISION_MATRIX.md`, using the merged collision interfaces.

Check-in required:

- player-enemy contact damage with an invulnerability window;
- enemy projectile-player damage and projectile deactivation;
- sword, arrow, and bomb-explosion damage to enemies;
- arrow and enemy projectile response to solid geometry;
- enemy response to walls/blocks;
- dead/inactive enemies stop colliding, updating, and drawing;
- deterministic reset and headless tests proving damage does not repeat every frame.

Final work adds boomerang hit/return behavior and completes every Michael-owned matrix row. Do not implement room parsing, inventory effects, push blocks, or mouse input. Run both verification scripts and request review of the combat handler and enemy damage/state classes.
