# Sprint 2 Code Reviews

These records cover the final integrated source. Hank completed supplemental
integration reviews where the assigned pull-request review was not submitted.

## Review timeline

- **September 16:** Sprint work began. The team planned a readability pass after
  each feature became reviewable and a maintainability pass before final
  packaging. No completed feature review is recorded for this date.
- **September 25:** The player-state implementation was submitted in pull
  request #13 and became available for review.
- **September 27–28:** Player sprites, enemies/NPCs, and input/menu/HUD work
  became available in pull requests #14–#16. The first integration build was
  assembled so the features could be checked together.
- **October 1:** Leo approved the enemy/NPC implementation in pull request #14.
  Ashley also reviewed the items/blocks acceptance notes in pull request #19.
- **October 5:** Hank completed the missing integration reviews, including the
  maintainability scenarios below. Ashley completed the items/blocks source
  review and recorded her comments and review time.

## Player sprites

- Reviewer: Hank Zhang
- Date: October 5, 2026
- Review type: Readability and maintainability
- File reviewed: `GameProject/Sprites/PlayerSprite.cs`
- Code author: Xuanzhe Li
- Review time: 12 minutes
- Pull request: #15

Readability: `PlayerSprite` has one responsibility: selecting a source rectangle and drawing
the player atlas. The compatibility `Draw` overload uses ordinary branches and
then delegates to the full animation overload. Atlas validation in the
constructor makes an incorrect texture fail early.

Maintainability: the exception text contains the fixed size `160 by 640` even though the check is
calculated from frame constants. If the atlas layout changes later, the message
must be updated as well. A fifth animation can otherwise be added through
`PlayerSpriteFrames` without placing frame rectangles in the player object.

Outcome: no blocking issue for Sprint 2.

## Player states

- Reviewer: Hank Zhang (supplemental integration self-review)
- Date: October 5, 2026
- Review type: Readability and maintainability
- Files reviewed: `PlayerStateMachine.cs` and the four player state classes
- Code author: Hank Zhang
- Review time: 10 minutes
- Pull request: #13

Readability: the transition guards are easy to follow: attacks cannot overlap attacks or
damage, and repeated damage is ignored until the damaged state ends. Timed
states use elapsed game time and return to walking only when movement input is
still active. Reset returns directly to the shared idle state.

Maintainability: the attack and damage classes contain similar code for returning to idle or
walking. The duplication is small and clearer than adding another abstraction
for two classes. A future knockback state can implement `IPlayerState` and be
started through one new state-machine method without changing input polling or
sprite frame data.

Outcome: no blocking issue for Sprint 2.

## Enemies and NPCs

- Reviewers: Leo Zhuang (pull-request approval) and Hank Zhang (supplemental integration review)
- Date: October 1, 2026
- Review type: Readability and maintainability
- Files reviewed: `EnemyCharacter.cs` and `Octorok.cs`
- Code author: Michael Xu
- Review time: 10 minutes for the supplemental integration review; Leo's time was not recorded
- Pull request: #14, approved

Readability: the common enemy class keeps elapsed time, drawing, and reset behavior in one
place, while each enemy class contains its own movement. Named constants make
the Octorok patrol and firing interval readable. Tests cover movement bounds,
frame independence, projectile timing, gallery wraparound, and reset.

Maintainability: a future enemy can extend `EnemyCharacter` and implement its movement without
changing input handling. A larger projectile sprite or a different gallery
layout would require replacing the Octorok's current fixed shot bounds.

Outcome: approved in pull request #14.

## Items and blocks

- Reviewer: Ashley Zhang
- Date: 2026.10.05
- Sprint: 2
- Review type: Readability and maintainability
- Files reviewed:
  - `GameProject/Objects/BlockObject.cs`
  - `GameProject/Objects/ItemObject.cs`
- Code author: Leo Zhuang
- Review time: 30mins
- Pull request: #17 and #19

Readability: 
Both classes are short and easy to follow. The switch expressions clearly show which class is used for each item or block. The similar structure also makes the two files easy to compare. One small improvement would be to explain what “compatibility entry point” means in the comments.

Maintainability: 
The separate object classes and `ISprite` parameter make the code easier to extend and test. For example, adding a Potion item would require a new enum value, a new class, and updates to the switches in `ItemObject` and `SpriteFactory`. This is manageable, but both mappings need to be updated together. A test for the new item would help catch missing changes.

Outcome: no blocking issue for Sprint 2.

## Input, menu, and HUD

- Reviewer: Hank Zhang (supplemental integration review)
- Date: October 5, 2026
- Review type: Readability and maintainability
- Files reviewed: `KeyboardController.cs` and `KeyPressDispatcher.cs`
- Code author: Ashley Zhang
- Review time: 10 minutes
- Pull request: #16

Readability: the command dictionary keeps discrete bindings in one location, and
`KeyPressDispatcher` has a short edge-detection loop. Continuous movement is
kept separate from single-press actions. Menu gating wraps gameplay commands
without duplicating the individual command classes.

Maintainability: if a pause state is added later, `GameSession` can expose the additional mode
and the command wrapper can decide which actions remain available. The current
separation means edge detection should not need to change.

Outcome: no blocking issue for Sprint 2.
