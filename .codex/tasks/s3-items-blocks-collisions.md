# Sprint 3 task — items, inventory, blocks, and doors

Owner: Leo Zhuang (`@Lzzz-7`)  
Reviewer: Ashley Zhang (`@xing-gif`)  
Estimate: 12 points / 15 hours

Implement item collection, inventory effects, push-block behavior, key/door behavior, and their persistent room state.

Check-in required:

- pickup applies an effect and queues removal after collision processing;
- heart, rupee, key, map/compass, weapon pickup, and objective state are represented;
- an item cannot be collected twice after leaving/re-entering a room;
- collection and reset have headless tests.

Final work adds one-axis grid-aligned push behavior, blocked-destination checks, key consumption/unlocking, persistence, and full reset. Bomb/breakable-wall behavior is stretch work and may start only after required criteria pass.

Do not implement enemy damage, room parsing, collision detection infrastructure, or mouse input. Run both verification scripts and request review of the item handler and push-block/door handler.
