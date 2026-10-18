# Sprint 3 room loading — Xuanzhe Li (@JojoLi132)

Implements issue #23 on `feature/s3-room-loading`. Required reviewer: Michael Xu
(@mo46-123), with `RoomDefinition.cs` for readability and `RoomLoader.cs` for
maintainability. PR base: `integration/sprint3`.

## What is available

Six external JSON files describe Entrance Hall, Quiet-Water Corridor, Echo
Workshop, Starlamp Court, Prism Sanctuary, and the All-Object Laboratory.
The five dungeon rooms use all 22 existing block/item/enemy kinds. The lab
contains those same kinds and one player (23 objects). The starting hall has
all three usable item pickups, and the default factory enables all three
player item effects in every room, including the lab.

`RoomLoader` parses data without a graphics device. `RoomObjectFactory` creates
existing player/block/item/enemy objects. `LoadedRoom` updates, draws, and
resets every object, and exposes a read-only ID-to-object mapping. Call these
methods only on the active room. `RoomRenderer` draws the boundary, grid, exit
markers, and objects without advancing simulation.

The feature-local preview runs now. Production `Game1` remains the Sprint 2
gallery until Ashley integrates room navigation. This PR does not implement
collisions, pickups, doors, persistence, or production mouse navigation.

## JSON schema

All field names and type names are case-sensitive. Unknown fields are errors
so misspellings cannot silently disappear. Every file contains:

| Field | Meaning |
| --- | --- |
| `id`, `name` | Stable room identity and display name |
| `bounds` | Walkable screen rectangle: integer `x`, `y`, `width`, `height` |
| `entries` | Named `x`/`y` entry positions, including `start` |
| `links` | Array of `id`, `direction`, `targetRoomId`, `targetEntryId`, `trigger` |
| `objects` | Array of `id`, `type`, `name`, `position`, optional `properties` |

`trigger` is a rectangle inside the walkable boundary. Its direction is `Up`,
`Down`, `Left`, or `Right`. Entry positions are inset from exit triggers so
arriving does not immediately trigger another transition. Each room has one
player definition for initial state; navigation must reuse a persistent player
when moving between rooms rather than create a fresh player each time.

Object types are `Player`, `Block.<BlockKind>`, `Item.<ItemKind>`, and
`Enemy.<EnemyKind>`. `Enemy.OldMan` is the existing Lantern Keeper NPC.
`properties` is a string-to-string data dictionary. It is preserved, never
executed; the default factory does not interpret future gameplay properties.
Projectiles and explosions are transient effects created by the player/enemies,
not additional room spawn types.

Object IDs are unique **within a room**. Persistence should use `(roomId,
objectId)` as its key. Room IDs must be unique across the loaded directory.
Link IDs and JSON member names cannot repeat. Directory loading validates both
target rooms and target entries and uses ordinal filename order for repeatable
room selection. The default data order puts Entrance Hall first and the lab last.

Positions are top-left screen pixels. Validation reserves conservative family
footprints: player 28x34, blocks 52x60, items 32x34, enemies/NPCs 38x42. These
cover current sprites, not collision hitboxes. Coordinates are nonnegative,
rectangles have positive dimensions, and rectangle edges cannot exceed 16384.
Movement envelopes and projectile collision are the combat owner's responsibility.

Malformed JSON, null/missing data, duplicate IDs or properties, unknown object
types, invalid positions, and dangling links throw `InvalidDataException` with
file/room/object context. Missing/unreadable files retain normal I/O exceptions.
No invalid data is silently skipped. Parsing creates fresh definitions and the
default factory creates fresh objects on each load.

## Run and verify

From the repository root:

```sh
bash scripts/verify.sh
bash scripts/analyze-strict.sh
dotnet run --project tests/Rooms/Rooms.Tests.csproj --configuration Release
dotnet format tests/Rooms/Rooms.Tests.csproj --verify-no-changes
dotnet run --project tests/Rooms/Rooms.Tests.csproj --configuration Release -- --preview
```

The shared verification script is integrator-owned, so room tests are currently
an explicit additional command. The test project copies the JSON files to its
output folder. It works when launched from another working directory as well.

Manual acceptance in the separate preview:

1. Confirm Entrance Hall has a player, keeper, blocks, and all five pickups.
2. Press Page Down five times; inspect the five themed rooms and the laboratory.
   Page Up and Page Down wrap. These are preview controls, not production input.
3. In the lab, check six blocks on the top row, nine items in the middle,
   seven enemies/NPCs below, and the player at the bottom left.
4. Wait several seconds. Observe all animated items, moving enemies, Octorok
   shots and Sentinel light bolts simultaneously, not only a gallery selection.
5. Move with WASD/arrows, attack with Z/N, use 1/2/3. Press R; the current room
   resets positions, animation clocks, and active player effects.
6. Q/Escape quits. The preview deliberately does not claim collision or pickup
   completion; walking through a block here is expected until integration.

Capture all six initial frames with the actual MonoGame renderer:

```sh
dotnet run --project tests/Rooms/Rooms.Tests.csproj --configuration Release -- --capture /absolute/output/path
```

Evidence: [lab](all-object-lab.png), [entrance](entrance-hall.png).
All object art is reused from Sprint 2 with its existing attribution. The grid,
border, and exit rectangles are original procedural drawing; no new assets or
packages were downloaded.

## Verification record — 2026-10-18

- .NET SDK 8.0.425, Windows, MonoGame DesktopGL 3.8.5.1.
- Required baseline verification passed before edits.
- 89 headless checks cover data rejection, roster coverage, reachability,
  real factory construction, all-object update/draw, draw purity, item effects,
  inactive rooms, and repeated reset/loading.
- Real MonoGame capture rendered all six rooms; screenshots were inspected.
- Final `scripts/verify.sh` and the separate room format/test commands passed.
- `scripts/analyze-strict.sh` passed with 0 errors and 31 existing warnings:
  CA1062 (20), CA1725 (4), CA2213 (2), CA1051 (2), CA2208 (1), CA1859 (1),
  CA1001 (1). No diagnostic points to the new room files; no warning was
  suppressed. The documented 109-warning team baseline includes CA1515 from
  a newer analyzer, which this .NET 8 SDK does not emit. These totals are not
  evidence that this branch fixed those 78 visibility findings.
- See [integration handoff](INTEGRATION.md) for shared-file follow-ups and the
  distinction between feature completion and whole-game integration.

Review and final integration acceptance remain external gates. This record does
not impersonate Michael's required review or mark pending work as complete.
