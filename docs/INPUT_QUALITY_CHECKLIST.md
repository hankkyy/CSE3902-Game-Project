# Input, menu, HUD, and release checklist

Owner: `@xing-gif` · Branch: `feature/input-quality` · Issue: #12 · Reviewer: `@mo46-123`

This checklist tracks final PR/release verification. Check items after running them on the final branch; implementation and local testing do not imply reviewer approval or remote CI success.

## Automated checks

Run from the repository root:

```sh
bash scripts/verify.sh
dotnet run --project tests/InputTests/InputTests.csproj --configuration Release
dotnet format tests/InputTests/InputTests.csproj --no-restore --verify-no-changes
git diff --check
```

- [ ] Repository verification succeeds: formatting, analyzers, Release build, and hygiene.
- [ ] All 25 input/menu assertions pass.
- [ ] Test-project formatting succeeds.
- [ ] `git diff --check` reports no whitespace errors.

The console test project uses the existing game dependency through a project reference and does not open a graphics window. It covers the dispatcher and state-gated commands. It is not a substitute for testing the real keyboard controller and visuals below.

## Manual acceptance

Launch with `dotnet run --project GameProject/GameProject.csproj`.

- [ ] Start screen is visible with Enter and quit instructions.
- [ ] While still in the menu, press movement, attack, E, 1-3, gallery keys, and R. Press Enter and confirm the objects retain their initial state.
- [ ] Hold Y in the menu, press Enter, and confirm the gallery has not changed. Release Y and press it again; exactly one change occurs.
- [ ] In gameplay, test all arrows and W/A/S/D. Hold a movement key to confirm continuous movement and check FACING.
- [ ] Test Z and N separately; the HUD briefly shows ATTACKING. Hold one attack key to confirm it does not repeatedly retrigger; release and press again.
- [ ] Test E; health decreases and DAMAGED appears. Holding E causes one damage event. Wait for damage state to end before testing attack again.
- [ ] Test 1, 2, and 3; ITEM SLOT and its bar match the selection.
- [ ] Test T/Y, U/I, and O/P in both directions. Confirm names/indexes, first-to-last and last-to-first wraparound, and one change per press.
- [ ] Repeat representative letter controls with Shift held and with Caps Lock enabled; their actions remain available.
- [ ] Change position, health, selection, and each gallery, then press R. Confirm initial position, health 5, item slot 1, IDLE, DOWN, and index 1 for each gallery. Gameplay stays active.
- [ ] Press Enter during gameplay; it does not restart or reset the game.
- [ ] Test Q and Escape in separate runs, both in the menu and gameplay.
- [ ] HUD labels, gallery names, and bottom controls are visible without overlap or clipping. Preserve a menu screenshot and a gameplay screenshot.

The current scaffold uses geometric placeholder sprites and does not implement combat collisions. Use HUD state and the short movement pause to verify attacks; do not expect enemies in the separate gallery to take damage.

## CI coverage and integration

The existing `.github/workflows/build.yml` runs `./scripts/verify.sh` on PRs targeting `main` and pushes to `main`. The separate test executable and test formatting are currently local checks, so a green existing CI job alone does not demonstrate that those checks ran.

A future CI update can run these additional steps after repository verification:

```yaml
- name: Verify input and menu behavior
  run: dotnet run --project tests/InputTests/InputTests.csproj --configuration Release
- name: Verify input test formatting
  run: dotnet format tests/InputTests/InputTests.csproj --no-restore --verify-no-changes
```

Keep existing verification enabled. Coordinate changes to shared CI configuration with the integrator; this task's documentation does not claim that the additional steps have already been installed.

## Before review and release

- [ ] README reflects the menu, controls, HUD, limitations, and verification commands.
- [ ] Only owned feature paths and task-specific tests/docs are changed; frozen contracts, gameplay objects, and sprite implementations remain intact.
- [ ] Generated `bin/`, `obj/`, IDE files, and local artifacts are not tracked.
- [ ] PR targets `main` from `feature/input-quality`, links #12, and includes exact manual steps and the two screenshots.
- [ ] Request `@mo46-123` as reviewer. Record readability feedback and a maintainability scenario for an authored class, such as adding a pause mode to the input dispatcher/session flow.
- [ ] Wait for the latest PR CI result and reviewer approval; resolve feedback and any integration conflicts before merging.
- [ ] After acceptance and merge, update the issue/project status and remaining effort according to the team workflow.

`@xing-gif` is also assigned to review `@Lzzz-7`'s items/blocks PR. That is a separate code-review responsibility, not permission to implement that teammate's feature.

Team-wide reflection, burndown, packaging, and Carmen submission remain team release responsibilities; this checklist does not mark them complete.
