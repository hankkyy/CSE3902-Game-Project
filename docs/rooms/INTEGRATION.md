# Room API handoff and shared-file proposal

Owner: Xuanzhe Li (@JojoLi132). Consumers: Ashley (navigation), Hank (contracts),
Leo (inventory/persistence), Michael (combat). This feature changes only owned
room definitions/loading/data/visuals and room-specific tests/docs.

## Additive boundary to approve

`GameProject.Rooms.Loading.IRoomObjectFactory` follows the proposed architecture
signature: `IGameObject Create(RoomObjectDefinition definition)`.
`RoomDefinition` lives in `GameProject.Rooms.Definitions`; all associated data
types live alongside it. No existing `Core` interface or object API changes.
The architecture calls for an integrator-owned contract PR before consumers
depend on these APIs. No merged room-contract PR was present when this feature
was prepared; approve/extract this boundary before integrating dependent work.

Example composition after contract approval:

```csharp
var definitions = RoomLoader.LoadDirectory(roomDataDirectory);
RoomDefinition definition = definitions[0];
var factory = new RoomObjectFactory(definition.Bounds.ToRectangle(), sprites);
var room = new LoadedRoom(definition, factory);
var renderer = new RoomRenderer(sprites);

// In the active-room game loop:
room.Update(gameTime);
// Process collisions, deferred removals, and transitions here.
// Within SpriteBatch.Begin/End:
renderer.Draw(spriteBatch, room);
```

`Objects` is a read-only dictionary keyed by the room-local stable object ID.
The default factory returns existing compatibility objects; it does not add
collision interfaces. Consumers may provide their own `IRoomObjectFactory`
to return persistent/collision-aware objects. Keep references to definitions
for object properties. Cache a `LoadedRoom` to preserve its runtime objects;
do not rebuild on every frame or every revisit.

`LoadedRoom.Reset()` resets all its contained objects. It does not reset the
navigation manager, inventory, active-room choice, or transition queue. Those
belong to Ashley/Leo. A navigation factory should return the same persistent
player for a room's `Player` definition and let navigation set its arrival
position through the approved player API. Full reset should happen centrally.
Removal/deactivation must remain deferred until collision enumeration ends;
the current default room collection itself does not implement pickup removal.

## Small integrator-owned changes still needed

1. Approve the above room contract, then consume it in RoomManager/Game1.
2. In `GameProject.csproj`, copy room data to build/publish output:

   ```xml
   <None Update="Content/Rooms/*.json"
         CopyToOutputDirectory="PreserveNewest"
         CopyToPublishDirectory="PreserveNewest" />
   ```

   Resolve production data from `AppContext.BaseDirectory/Content/Rooms`.
   The preview/test project already copies data independently, without editing
   the shared game project. JSON does not need MGCB compilation.
3. Add `tests/Rooms/Rooms.Tests.csproj` to `scripts/verify.sh` and run it in CI.
   Until then run it separately using the documented command.
4. Wire the active room, player input, renderer, bounds/exit triggers, and the
   existing HUD in Ashley's owned paths. The data uses a 960x540 window with
   walkable bounds `(24,72,880,430)`; replace the old right-side gallery panel
   when showing full rooms. Draw exit markers as geometry cues, not proof of
   implemented door state or transitions.
5. Link `docs/rooms/README.md` from the main README and update overall feature
   status only after the relevant feature PRs merge.

## Tracking limitations

On 2026-10-18, the README's project 5 URL returned 404 even while Chrome was
signed in as JojoLi132. The configured Git credential has repository access,
but no Projects scope. Issue #23 only exposed the old Sprint1 project in that
account's sidebar. The intended Sprint 3 board therefore could not be read or
updated; the unrelated Sprint1 board was left unchanged. An owner needs to
grant JojoLi132 access or correct the board link before tracking can be finalized.
