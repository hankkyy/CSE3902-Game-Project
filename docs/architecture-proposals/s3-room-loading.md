# Room integration notes

The room API uses `RoomDefinition` in `Rooms/Definitions` and
`IRoomObjectFactory.Create(RoomObjectDefinition)` in `Rooms/Loading`.
The integrator should confirm this interface before other features depend on it,
as required by `SPRINT3_ARCHITECTURE.md`.

```csharp
var definitions = RoomLoader.LoadDirectory(roomDataDirectory);
var definition = definitions[0];
var factory = new RoomObjectFactory(definition.Bounds.ToRectangle(), sprites);
var room = new LoadedRoom(definition, factory);

room.Update(gameTime);
// Call inside SpriteBatch.Begin/End.
renderer.Draw(spriteBatch, room);
```

Keep loaded rooms when revisiting them. `Objects` maps room-local IDs to game
objects. A custom factory can provide persistent objects and a shared player.
`LoadedRoom.Reset()` resets its objects; navigation and inventory need their own
reset handling. Apply collection changes after collision processing.

Shared-file changes for integration:

- Copy room data to the game output in `GameProject.csproj`:
  ```xml
  <None Update="Content/Rooms/*.json"
        CopyToOutputDirectory="PreserveNewest"
        CopyToPublishDirectory="PreserveNewest" />
  ```
- Load from `AppContext.BaseDirectory/Content/Rooms`.
- Add `tests/Rooms/Rooms.Tests.csproj` to `scripts/verify.sh`.
- Connect the active room, player, renderer, and exit triggers in RoomManager
  and Game1. The data uses a 960x540 window and bounds `(24,72,880,430)`.
  The room view replaces the old right-side gallery panel.

The room tests and preview already copy their own data. The default factory
creates existing objects; collision responses, persistence, and transitions
are handled by the corresponding gameplay modules.
