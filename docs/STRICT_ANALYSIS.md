# Strict analyzer record — navigation scope

- Author: Ashley Zhang
- Date: 2026-10-19
- Branch: `feature/s3-room-navigation-integration`
- Base commit: `0542485`; this record describes the follow-up changes.
- Command: `bash scripts/analyze-strict.sh`

This entry covers `Game1`, `StartMenu`, `MouseController`, `RoomHud`, and the four navigation classes listed below. It does not claim that the whole project is warning-free. Room-loading dependencies and existing shared `Player` diagnostics are outside this entry's scope.

## Fixes and individual suppressions

| File or type | Rule | Action and reason |
| --- | --- | --- |
| `MouseController` | CA1515 | Fixed earlier: made internal because only the game assembly uses it directly. |
| `RoomHud` | CA1515 | Fixed earlier: made internal because only the game assembly uses it. |
| `RoomManager` | CA1515 | Suppressed for this type only: the separate navigation test project constructs it and checks queued transitions and entries. |
| `RoomCache` | CA1515 | Suppressed for this type only: the test project uses an injected player and factory to check room reuse. |
| `RoomGameplay` | CA1515 | Suppressed for this type only: the test project supplies input snapshots to check navigation and reset. |
| `RoomExitNavigator` | CA1515 | Suppressed for this type only: the test project directly checks exit triggers and arrival protection. |
| `Game1` | CA1515 | Fixed in this follow-up: made internal; the entry point is in the same assembly. |
| `StartMenu` | CA1515 | Fixed in this follow-up: made internal; its callers are in the same assembly. |
| `Game1.spriteBatch` | CA2213 | Fixed in this follow-up: dispose the batch and clear the reference before calling the base disposal method. |
| `Game1.graphics` | CA2213 | Suppressed for this field only: MonoGame registers the manager with `Game`, and `base.Dispose(disposing)` disposes it. |

The four type suppressions are in `NavigationAnalyzerSuppressions.cs`. The graphics-field suppression is beside the field in `Game1.cs`. No project-wide rule was disabled.

This follow-up addresses four diagnostics: three fixes and one justified suppression. Together with the earlier six entries, this scope has five fixes and five justified suppressions.

MonoGame ownership was checked against the project's pinned version:
- [GraphicsDeviceManager registration](https://github.com/MonoGame/MonoGame/blob/v3.8.5.1/MonoGame.Framework/GraphicsDeviceManager.cs#L83-L88)
- [Game.Dispose](https://github.com/MonoGame/MonoGame/blob/v3.8.5.1/MonoGame.Framework/Game.cs#L107-L159)

## Validation and evidence

A separate local copy based on `0542485` was checked after these changes. The strict build completed with no errors and no reported warnings in the scope listed above. NuGet audit was disabled only for the local verification command because network access was restricted; no analyzer rules or committed package settings were disabled.

The 116 room gameplay checks also passed in that copy. The full `verify.sh` run stopped at `dotnet format` because the sandbox denied its build-host IPC connection; rerun the complete script in the normal terminal before committing. Graphical launch and exit should also be checked after the disposal change.

- [Full follow-up analyzer output](room-navigation/strict-analysis-navigation-followup.txt)
- [Earlier records, including the team baseline](room-navigation/strict-analysis-history.md)
- [Earlier full analyzer output](room-navigation/strict-analysis.txt)

The full outputs are retained for team follow-up. This scoped result does not mark other modules, collision/inventory integration, or code review complete.
