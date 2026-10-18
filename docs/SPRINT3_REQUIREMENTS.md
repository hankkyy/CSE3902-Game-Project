# Sprint 3 requirements — Team 6 interpretation

Source: instructor-provided `sprint3.html`. The dates printed in that copy
belong to a different term. Team 6's current schedule is:

- Sprint start: October 7, 2026
- Functionality check-in: October 19, 2026
- Sprint 3 final submission: October 26, 2026 by 8:00 PM

All times use America/New_York unless Carmen states otherwise.

## Required functionality

- Continue developing the 2D game framework.
- Handle every collision type that can occur, including necessary position corrections and state transitions.
- Divide the dungeon into individual rooms/screens, each containing only its own objects.
- Store room contents in one or more external files and initialize room objects from those files.
- Provide an artificial test level containing one player and every object type used in the first dungeon.
- Provide a fast way to switch to any room without walking through intermediate rooms.
- Support arrows and WASD for player movement.
- Continue item use through numeric keys.
- Use a mouse controller for room navigation. A clickable map or previous/next mouse buttons are both acceptable.

Gamepad support is recommended but optional. Sound and smooth camera transitions are explicitly not required until a later sprint. Major intentional deviations from the original game must be documented.

## Functionality check-in

The target is at least two thirds of the Sprint 3 functionality. Team 6 will submit only after the check-in section of `SPRINT3_CHECKLIST.md` is complete and demonstrated from a clean clone.

## Required process

- Record Sprint start/end dates and associate every work item with the current sprint.
- Put work in progress on the board and reduce Remaining Hours after every work session.
- Finish functionality early enough for two review/refactor passes.
- Maintain the README with controls, known bugs, deviations, and special tools/processes.
- Run metrics or Roslyn analysis at least weekly; record each warning's fix or documented suppression.
- Every member reviews and is reviewed for readability and maintainability.
- Write a burndown-based reflection containing every member's signed opinion.
- Include README, reflection, reviews, analysis records, and other project-management documents in the final submission.

## Team decisions

- JSON is the preferred room format because room links and optional object properties are nested data. This still fulfills the external-file requirement.
- The all-object test room will start with all three player items available.
- Mouse left/right navigation is the minimum check-in implementation. A clickable map is an enhancement.
- Camera smoothing and audio are out of scope.
- Sprint 2 gallery keys may remain temporarily in a debug mode, but the submitted Sprint 3 path must demonstrate real rooms and interactions.
