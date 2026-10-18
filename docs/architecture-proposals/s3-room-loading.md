# Sprint 3 room-loading integration proposal

The feature implements the proposed `IRoomObjectFactory.Create` concept in
the owner's `Rooms/Loading` directory and puts data models in
`Rooms/Definitions`. Existing shared contracts are unchanged.

Before dependent feature work consumes these types, the integrator should
approve/extract their additive boundary in the contract PR required by
`SPRINT3_ARCHITECTURE.md`.

The smallest shared-file follow-ups are to copy external JSON files from
`Content/Rooms` to build/publish output in the game project and add the room
test project to the verification script. Ashley then supplies production
navigation and `Game1` wiring. Exact snippets and ownership boundaries are in
[the room integration handoff](../rooms/INTEGRATION.md).

The separate room test/preview host already loads and renders all six rooms,
so reviewers can validate this feature before shared-file integration.
