# Sprint 3 task — data-driven rooms

Owner: Xuanzhe Li (`@JojoLi132`)  
Reviewer: Michael Xu (`@mo46-123`)  
Estimate: 12 points / 15 hours

Implement the room schema, parser/validator, room object factory boundary, external room data, and room rendering assigned in `docs/SPRINT3_TASKS.md`.

Required:

- stable room/object IDs, display name, links/entry points, positions, and optional properties;
- clear validation for malformed files, duplicate IDs, unknown types, and invalid positions;
- five distinct Starlight Ruins rooms;
- one artificial test room containing the player and every first-dungeon object type;
- active-room collection updates/draws every object in that room;
- parsing and roster-coverage tests that do not require a graphics device.

Do not hard-code room lists in `Game1`, implement mouse input, own room transitions, or implement collision responses. Preserve existing assets and document any new asset source. Run both verification scripts and request review of the room definition and loader.
