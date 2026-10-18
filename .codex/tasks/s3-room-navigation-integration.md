# Sprint 3 task — room navigation and integration

Owner: Ashley Zhang (`@xing-gif`)  
Reviewer: Hank Zhang (`@hankkyy`)  
Estimate: 12 points / 15 hours

Implement RoomManager, valid transitions and entry placement, persistent room-state coordination, mouse room navigation, current-room/inventory HUD, and composition wiring.

Required:

- normal door/exit transition and destination entry placement;
- edge-triggered MouseController with at least left=previous and right=next room;
- current room, health, selected item/inventory, and mouse help on the HUD;
- `Game1` creates services and calls update/draw but contains no parsing or collision rules;
- full reset restores the starting room and every registered subsystem;
- clean-clone check-in walkthrough;
- strict analyzer baseline and per-warning fix/suppression records.

Coordinate integration only after owner PRs expose approved APIs. Do not silently rewrite another owner's implementation. Run both verification scripts and request review of RoomManager and MouseController.
