# Sprint 2 Reflection

## Outcome

The team combined player movement and states, directional sprites, enemies,
items, blocks, keyboard controls, a start menu, and a HUD into one working
demonstration. The final verification script builds the Release configuration
with zero warnings and runs all six automated test projects.

The largest unfinished work is visual polish. Some gallery objects use simple
two-frame motifs rather than final object-specific animation. Collision,
collection, and room navigation remain outside the Sprint 2 demonstration.

## Burndown evidence

The five original issues started at eight effort points each, for 40 points.
The board currently reports remaining values of 3, 1, 8, 8, and 8. One item is
marked Done while still showing eight remaining points, so the displayed total
of 28 does not represent the amount of unfinished code.

The repository history also shows that work was concentrated late in the
sprint:

| Date | Commits in the integrated Sprint 2 history |
| --- | ---: |
| September 25 | 18 |
| September 27 | 5 |
| September 28 | 10 |
| September 30 | 1 |
| October 5 | 2 |

The board and commit history show the same process problem: implementation
progressed faster than task estimates and status fields were updated. This made
the burndown appear worse than the actual build and left integration and
documentation work near the deadline.

## Team process

Dividing ownership by feature reduced source conflicts. Shared interfaces also
made it possible to integrate the five areas without changing the basic game
loop. The most difficult part was combining changes that all touched the player
and then checking that animation, commands, and reset behavior still agreed.

For the next sprint, each owner should update remaining effort at the end of a
work session. The team should merge smaller changes earlier and run the full
verification script after each integration. Review records and manual results
should be written when the work is reviewed instead of at the end of the sprint.

## Changes for Sprint 3

1. Each feature owner updates task status and remaining effort after working.
2. The integrator merges completed work at least twice each week.
3. The assigned reviewer records readability and maintainability comments before
   approving a pull request.
