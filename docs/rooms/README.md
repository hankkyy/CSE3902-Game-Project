# Room loading

Xuanzhe Li (@JojoLi132), Issue #23. Reviewer: @mo46-123.

Room data is stored in `GameProject/Content/Rooms`. There are five dungeon
rooms and a test room containing the player, six block types, nine item types,
and seven enemy/NPC types.

`RoomLoader` reads and validates JSON. `RoomObjectFactory` creates the objects.
`LoadedRoom` updates, draws, and resets every object in a room. `RoomRenderer`
draws the floor, boundary, exits, and objects.

## Room format

| Field | Content |
| --- | --- |
| `id`, `name` | Room ID and display name |
| `bounds` | Walkable rectangle: `x`, `y`, `width`, `height` |
| `entries` | Named positions, including `start` |
| `links` | Exit ID, direction, target room/entry, and trigger rectangle |
| `objects` | Object ID, type, name, position, and optional properties |

Names are case-sensitive. Object types use `Player`, `Block.<BlockKind>`,
`Item.<ItemKind>`, or `Enemy.<EnemyKind>`. `Enemy.OldMan` is the Lantern Keeper.
Optional properties are string pairs; the default factory does not apply them.
Each room contains one player definition. Object IDs are unique within a room;
use `(roomId, objectId)` when saving progress.

The loader rejects missing fields, duplicate IDs or JSON fields, unknown types,
invalid positions, and missing link targets. Coordinates are integer screen
pixels. Validation reserves space for each sprite family: player 28x34,
blocks 52x60, items 32x34, and enemies/NPCs 38x42. Files load in filename order.

## Tests and preview

```sh
dotnet run --project tests/Rooms/Rooms.Tests.csproj -c Release
dotnet run --project tests/Rooms/Rooms.Tests.csproj -c Release -- --preview
```

The 89 checks cover parsing errors, room links, object coverage, creation,
update/draw, and reset. The test project copies room JSON to its output folder.

In the preview, use Page Up/Down to visit all six rooms. Check the lab's three
object rows, wait for animations and enemy shots, then test WASD/arrows, Z/N,
and items 1/2/3. R resets the current room; Q/Escape exits. The preview does
not include collision responses, pickups, or production room navigation.

To save screenshots, replace `--preview` with `--capture <output-directory>`.
Examples: [laboratory](all-object-lab.png), [entrance](entrance-hall.png).
Sprites reuse the existing Sprint 2 artwork; the room grid and border are
drawn in code. See [integration notes](../architecture-proposals/s3-room-loading.md).

## Verification — October 18, 2026

Normal verification, room tests, formatting, and six room captures passed on
.NET SDK 8.0.425. Strict analysis reported 0 errors and 31 existing warnings:
CA1062 (20), CA1725 (4), CA2213 (2), CA1051 (2), CA2208 (1), CA1859 (1), and
CA1001 (1). None were in the new room code; no warnings were suppressed.
The team's newer analyzer also reports CA1515, which this SDK does not emit,
so its warning total cannot be compared directly.
