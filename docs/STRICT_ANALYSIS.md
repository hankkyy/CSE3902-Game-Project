# Strict analyzer record

## Baseline — Sprint 3 planning

- Date: 2026-10-18
- Command: `./scripts/analyze-strict.sh`
- Commit: Sprint 2 final baseline `6e6dfeb`
- Result: build succeeded with 109 warnings and 0 errors under `AnalysisMode=AllEnabledByDefault`.

| Rule | Count | Planned action |
|---|---:|---|
| CA1515 | 78 | Review API visibility. Keep public only where cross-project tests or contracts require it; document intentional exceptions. |
| CA1062 | 20 | Add appropriate public-boundary null validation. |
| CA1725 | 4 | Align implementation parameter names with interface declarations. |
| CA2213 | 2 | Review ownership/disposal of MonoGame resources. |
| CA1051 | 2 | Encapsulate visible fields where compatible. |
| CA2208 | 1 | Correct the invalid exception parameter name. |
| CA1859 | 1 | Use the concrete collection type when it does not harm design. |
| CA1001 | 1 | Correct or document disposable ownership in `SpriteFactory`. |

The normal analyzer configuration produced zero warnings, which explains the Sprint 2 report but is not sufficient for Sprint 3. Do not claim that the strict baseline is clean. Run this script at least weekly and append dated results below.

## Weekly runs

### Run 2

- Date:
- Commit:
- Counts:
- Warnings fixed:
- Suppressions and individual reasons:

### Run 3

- Date:
- Commit:
- Counts:
- Warnings fixed:
- Suppressions and individual reasons:


### 2026-10-19 — Room navigation

- Working copy: `feature/s3-room-navigation-integration` download; changes are not committed yet.
- Command: `bash scripts/analyze-strict.sh`.
- Before visibility changes: 127 warnings, 0 errors.
- After changing `MouseController` and `RoomHud` to `internal`: 125 warnings, 0 errors, reproduced in a separate local copy.
- Navigation tests after these changes: 116 checks passed.
- No analyzer rules were disabled and no suppression attributes were added.
- Keep the local before/after logs in `docs/room-navigation/` and confirm these results in the working copy before submitting.

| Rule | Planning baseline | Before changes | After changes |
|---|---:|---:|---:|
| CA1515 | 78 | 96 | 94 |
| CA1062 | 20 | 20 | 20 |
| CA1725 | 4 | 4 | 4 |
| CA2213 | 2 | 2 | 2 |
| CA1051 | 2 | 2 | 2 |
| CA2208 | 1 | 1 | 1 |
| CA1859 | 1 | 1 | 1 |
| CA1001 | 1 | 1 | 1 |
| Total | 109 | 127 | 125 |

The increase over the recorded baseline is in CA1515. Matching counts for the other rules do not prove that every diagnostic is unchanged. The strict build is not warning-free.

The 18 CA1515 diagnostics in the added navigation and room dependency files were reviewed as follows. Paths below are relative to `GameProject`.

| File | Decision and reason |
|---|---|
| `Controllers/MouseController.cs` | Fixed: made internal. It is used inside the game project. Input behavior is tested through `RoomGameplay`. |
| `UI/RoomHud.cs` | Fixed: made internal. Only the game uses this HUD. |
| `Rooms/Navigation/RoomManager.cs` | Keep public: the separate navigation test project constructs it and checks transitions. |
| `Rooms/Navigation/RoomCache.cs` | Keep public: the separate test project constructs it and checks room reuse. |
| `Rooms/Navigation/RoomGameplay.cs` | Keep public: the separate test project supplies input snapshots and checks gameplay integration. |
| `Rooms/Navigation/RoomExitNavigator.cs` | Keep public: the separate test project checks exit overlap and arrival protection directly. |
| `Rooms/Definitions/RoomArea.cs` | Keep public for now: used by the public room data model and tests. Confirm the shared contract with the room owner. |
| `Rooms/Definitions/RoomDefinition.cs` | Keep public for now: returned by the loader and consumed by navigation and separate tests. Confirm the shared contract with the room owner. |
| `Rooms/Definitions/RoomLink.cs` | Keep public for now: used by public room links and the exit-permission callback. Confirm the shared contract with the room owner. |
| `Rooms/Definitions/RoomObjectDefinition.cs` | Keep public for now: used by the object-factory interface and room data. Confirm the shared contract with the room owner. |
| `Rooms/Definitions/RoomPoint.cs` | Keep public for now: used by public room entries and navigation arrival positions. Confirm the shared contract with the room owner. |
| `Rooms/Loading/IRoomObjectFactory.cs` | Keep public for now: separate room tests implement this interface. Confirm the shared contract with the room owner. |
| `Rooms/Loading/LoadedRoom.cs` | Keep public for now: used by separate room tests and returned by the room cache. Confirm the shared contract with the room owner. |
| `Rooms/Loading/RoomLoader.cs` | Keep public for now: separate tests load the real room JSON files. Confirm the shared contract with the room owner. |
| `Rooms/Loading/RoomObjectFactory.cs` | Keep public for now: separate tests inject sprites without opening a game window. Confirm the shared contract with the room owner. |
| `Rooms/Loading/RoomObjectTypes.cs` | Pending room-owner review: current consumers appear to be inside the game project. Consider making it internal after checking the owner's branch. |
| `Rooms/Loading/RoomValidator.cs` | Pending room-owner review: current consumers appear to be inside the game project. Consider making it internal after checking the owner's branch. |
| `Sprites/RoomRenderer.cs` | Pending room-owner review: current consumers appear to be inside the game project. Consider making it internal after checking the owner's branch. |

The retained warnings remain visible. These decisions still need reviewer confirmation. This record does not mark room-contract approval, collision/inventory integration, or all Sprint 3 acceptance criteria complete.

### 2026-10-19 — Targeted navigation suppressions

The instructor's handout requires each reported warning to be fixed or suppressed with an explanation. A note saying "keep public" or "pending review" alone does not finish that requirement.

`MouseController` and `RoomHud` were already changed to internal. The four navigation types below stay public because the separate `RoomNavigation.Tests` project uses them directly. Their CA1515 warnings are now suppressed individually in `GameProject/Rooms/Navigation/NavigationAnalyzerSuppressions.cs`.

| Type | Rule | Reason for this suppression |
|---|---|---|
| `RoomManager` | CA1515 | The test project constructs it and verifies queued room changes and entry positions. |
| `RoomCache` | CA1515 | The test project constructs it with an injected player and object factory and checks room reuse. |
| `RoomGameplay` | CA1515 | The test project supplies keyboard/mouse snapshots and checks gameplay integration and reset. |
| `RoomExitNavigator` | CA1515 | The test project directly checks blocked exits, deferred transitions, and arrival protection. |

Each attribute names one type and one rule. No project-wide rule or warning category was disabled.

Validation in a separate local copy:

- Strict analysis before these suppressions: 125 warnings, 0 errors.
- Strict analysis after these suppressions: 121 warnings, 0 errors.
- CA1515 decreased from 94 to 90. Other rule counts did not change.
- All 116 room-navigation checks passed.

Confirm the same results in the working copy and save the updated analysis log before submitting. The remaining warnings, including inherited baseline diagnostics and room dependency diagnostics, still need individual fixes or justified suppressions coordinated with their owners. The project is not yet fully compliant with the instructor's warning-handling requirement.
