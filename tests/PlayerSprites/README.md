# Player sprite verification

These checks belong only to `feature/player-sprites`. They do not modify the production Game1, controls, Player, or project file. The test executable references the existing game project and adds no testing packages.

## Automated checks

From the repository root:

```sh
./scripts/verify.sh
dotnet run --project tests/PlayerSprites/PlayerSprites.Tests.csproj --configuration Release
dotnet format tests/PlayerSprites/PlayerSprites.Tests.csproj --no-restore --verify-no-changes
python tests/PlayerSprites/check_assets.py
```

The first command is the team's required hygiene, format, analyzer, and Release-build check. On Windows it needs Git Bash with its Unix utilities and the .NET SDK on PATH. A Git checkout with CRLF conversion conflicts with the repository's LF formatting rule; use a clean LF checkout without committing mass line-ending changes.

The headless executable checks 79 cases: clip starts and boundaries, loop and nonloop behavior, frame-rate-independent sampling, long elapsed times, all atlas regions, the legacy body anchor, reset-at-zero semantics, and invalid arguments. The Python authoring check uses Pillow to verify all 44 active frames are nonempty and distinct within their clips, their transparent gutters are intact, and the runtime PNG exactly matches the attributed source PNG and SHA-256 metadata.

For actual MonoGame GPU rendering on a DesktopGL-capable machine:

```sh
dotnet run --project tests/PlayerSprites/PlayerSprites.Tests.csproj --configuration Release -- --capture outputs/player-sprites
```

The test host renders twelve deterministic frames, then exits. It checks:

- Repeated draws with identical inputs produce identical pixels.
- Every one of the sixteen direction/action pairs has visible, differing first and second frames.
- Existing player and other-object factory signatures still draw inside an outer SpriteBatch.
- The unmodified production Player still responds to movement, attack, damage, selection, and reset through its public methods.

This is automated rendering and API-level input-command smoke coverage, not an assertion that physical keyboard events were manually tested. Main CI runs the existing verification script; headless and GPU test-host commands are additional branch checks, not silently added to another owner's CI.

## Repeatable visual acceptance

Run:

```sh
dotnet run --project tests/PlayerSprites/PlayerSprites.Tests.csproj --configuration Release -- --visual
```

1. Read the 4×4 grid: columns Down/Left/Right/Up, rows Idle/Walk/Attack/Damage. All directions must be distinct and upright; a sword should extend toward each attack direction.
2. Compare walk frames: feet and body must animate without the body anchor drifting. Idle has a small scarf detail change. Damage alternates red tint and impact marks.
3. In the **test host only**, select idle with `1`, walking with `2`, attack with `Z/N`, damage with `E`. Use WASD/arrows to change the bottom sample's facing. These are test-host controls; production item keys remain untouched.
4. Repeat `Z/N`; each press restarts the test sample's attack at frame zero. It should hold its recovery frame after 0.27 seconds until another state is selected. The grid intentionally replays attacks every 0.6 seconds for inspection.
5. Press Space to pause; the rendered animation must stop advancing. Press R during any sample to reset to Down/Idle and resume time. Q/Escape exits.
6. Run the production game using the root README commands. Confirm the new player body and old controls still work. Do **not** expect the full attack/damage clips here until the player-state owner applies the [proposal](../../docs/architecture-proposals/player-sprites.md).
7. After integration, repeat production Z/N, E, four-direction movement, and R. Check that no source rectangles or frame tables were added to gameplay objects.

## Results recorded on 2026-09-27

- Clean baseline: repository verify script passed, zero warnings/errors.
- Feature: 79 headless checks passed; GPU deterministic rendering and sixteen clip checks passed; legacy Player/factory smoke checks passed.
- Atlas: 44 active frames passed integrity checks.
- Content entry: MGCB 3.8.5.1 built `Content.mgcb`, one succeeded, zero failed.
- Reviewer manual keyboard test and production richer-state wiring remain pending; no human review is claimed.

Visual evidence and the asset license are in [docs/assets/PLAYER_SPRITES.md](../../docs/assets/PLAYER_SPRITES.md).
