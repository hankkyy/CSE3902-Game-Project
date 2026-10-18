# Sprint 3 checklist

## Before implementation

- [x] Sprint 2 final baseline is on `main`.
- [x] Annotated tag `sprint2-final` identifies the baseline.
- [x] `integration/sprint3` starts from that baseline.
- [x] Five equal-effort assignments and review ring are documented.
- [x] Five assigned GitHub issues and the Sprint 3 project board are created.
- [ ] Copy the current Carmen sprint start, check-in, final, and CATME dates to this file and the GitHub project.
- [ ] Each member confirms their issue, branch, reviewer, estimate, and acceptance criteria.
- [ ] Merge the small shared collision/room contract PR before dependent implementation.

## Functionality check-in gate — all required

- [ ] Room contents come from external data files, not hard-coded `Game1` lists.
- [ ] At least five rooms load with different object subsets.
- [ ] An artificial test room contains the player and every first-dungeon object type.
- [ ] The player can enter another room through a valid exit.
- [ ] Mouse input can quickly move to previous/next rooms or select a room.
- [ ] Player cannot pass through room bounds, walls, or solid blocks.
- [ ] Player can collect an item and the item is removed.
- [ ] Player/enemy contact and enemy projectile collisions damage the player safely.
- [ ] Sword, arrow, and bomb collisions damage enemies.
- [ ] Arrow/enemy projectiles stop at the appropriate wall or target.
- [ ] Current room, health, item/inventory state, and navigation help are visible.
- [ ] Existing Sprint 2 verification plus new room/collision tests pass.
- [ ] The README contains exact check-in controls and known limitations.
- [ ] A clean-clone manual walkthrough has been recorded.

## Final functionality

- [ ] All required rows in `COLLISION_MATRIX.md` are implemented.
- [ ] Push block movement and obstruction behave deterministically.
- [ ] Key/locked-door behavior works.
- [ ] Leaving/re-entering preserves collected, defeated, moved, and unlocked state.
- [ ] Reset restores player, inventory, rooms, objects, projectiles, and current room.
- [ ] Every major deviation from the original game is documented.

## Quality and process

- [ ] Each task records original estimate, Remaining Hours, actual time, start/end dates, owner, and reviewer.
- [ ] Remaining Hours are updated after each work session.
- [ ] `./scripts/analyze-strict.sh` is run and recorded at least weekly.
- [ ] Every strict warning is fixed or individually documented with a reason.
- [ ] Every member reviews two named classes for readability/maintainability.
- [ ] Every member has at least two classes reviewed.
- [ ] Every review records author, reviewer, date, minutes, comments, hypothetical change, and PR.
- [ ] Every member contributes a signed reflection paragraph.
- [ ] Reflection discusses the burndown chart and process changes.
- [ ] README, reflection, reviews, analysis log, task records, and source are included in the submission zip.
- [ ] One member submits on Carmen and the team records who/when/which commit.
