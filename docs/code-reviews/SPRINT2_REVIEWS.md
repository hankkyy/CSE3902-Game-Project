# Sprint 2 Code Reviews

These records cover the final integrated source. Hank completed supplemental
integration reviews where the assigned pull-request review was not submitted.

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

- Reviewer: Hank Zhang (supplemental integration review)
- Date: October 5, 2026
- Review type: Readability and maintainability
- Files reviewed: `ItemObject.cs` and `BlockObject.cs`
- Code author: Leo Zhuang
- Review time: 12 minutes
- Pull requests: #17 and #19

Readability: both wrapper classes keep the existing constructor surface and delegate to a
dedicated implementation selected by the enum. Sprite injection makes gallery
behavior testable without opening a graphics window. The switch expressions
also make the supported roster visible in one place.

Maintainability: the current item implementations share the same preview timing. If a later item
needs unique movement, `ItemEntity.Update` will need to become virtual or that
item will need a separate implementation behind `ItemObject`. This does not
block the independent Sprint 2 gallery demonstration.

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
