# Enemy/NPC implementation (issue #10)

## Main play-area integration

Each selection now includes the side preview at (700,420) and a matching enemy
in the lower-right play area at (370,450). Both share update timing, selection,
animation and reset. O/P cycles both displays; R resets both, including shots.
The main-area projectile stays within x=294..554, leaving space for its sprite
inside the play area's right edge. Game1 composes the paired gallery without
changing input mappings, menus, HUD or shared interfaces.

Automated enemy tests cover matching movement, animation, projectile timing,
both draw calls, wraparound, reset and main-area bounds. For a visual check,
start the game, cycle O/P through all four characters, observe matching motion
and shots in both panels, and press R during a shot and after changing selection.
In-game visual verification of this addition remains pending.

Branch: feature/enemies-npcs. Reviewer: @Lzzz-7.

EnemyObject preserves the gallery constructor and delegates to Octorok, Keese,
Gel or Npc. All simulation updates use GameTime; drawing only delegates to ISprite.
Octorok patrols horizontally and shoots every two seconds; Keese flies a figure-eight;
Gel rests then hops; the NPC stays stationary and animates.
Reset restores the constructor position, right-facing direction, animation clock and shots.
The existing ObjectGallery and keyboard commands retain O/P wraparound and R reset.

The enemy-owned projectile uses the existing Bomb sprite as a visible placeholder.
No sprite factory, assets, interfaces or teammate-owned files were changed.
Movement envelopes are relative to the constructor position and fit the current
(700,420) gallery layout; projectile top-left x stays within 624..884.
A future gallery relocation or larger sprite needs corresponding bounds review.

## Automated checks
Run ./scripts/verify.sh and:
dotnet run --project tests/Enemies/Enemies.Tests.csproj --configuration Release

The headless executable checks movement and shot bounds over 120 seconds,
NPC immobility, animation, split-frame equivalence, projectile cooldown,
reset, draw purity and gallery wrapping without a graphics device.

## Visual evidence

The Release game was launched successfully. The screenshot below shows the
Octorok and its enemy-owned projectile inside the gallery panel. Automated
desktop key input did not reliably reach MonoGame, so O/P and R gameplay checks
remain pending; headless tests cover the underlying gallery and reset behavior.

![Running gallery with Octorok and projectile](enemies-octorok.png)

## Manual checks (keyboard interaction still requires verification)
1. Run dotnet run --project GameProject/GameProject.csproj.
2. Wait two seconds on Octorok: verify horizontal patrol and a visible shot.
   Watch shots disappear before leaving the lower gallery panel.
3. Press P through Keese, Gel, Old Man, Octorok. Press O from Octorok:
   it must wrap to Old Man. Each press should advance once.
4. Observe Keese flight, Gel pause/hop, and stationary animated Old Man.
5. Press R during a shot, and again after selecting another character:
   return to Octorok at its start with no shot; the next shot starts after two seconds.
6. Confirm arrows/WASD, other galleries and Q still work.
7. Capture a screenshot/GIF for review; these visual checks are not claimed
   as completed by the automated test.
