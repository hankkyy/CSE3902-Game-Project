# Starlight Ruins sprite provenance

The new Crystal Pillar, Rune Tile, Bow, Boomerang pickup, Map, Compass, Star Shard, Rune Wisp, Clockwork Beetle, Prism Sentinel, and light-bolt motifs were created as original small pixel patterns for this project with coding-assistant help. They are stored directly in `GameProject/Sprites/RuinsSprite.cs` and rendered through the existing one-pixel texture.

No artwork was extracted or downloaded from another game for these additions. There is no new binary image asset or third-party asset dependency. The existing player atlas and its attribution remain unchanged; see `PLAYER_SPRITES.md` for that separate asset.

The patterns use eight rows of eight cells. Blocks render at scale 6, item previews at scale 3, enemies at scale 4, and the light bolt at scale 1. `#` draws the body, `+` draws an accent, `*` appears in the alternate frame, and `.` is transparent. Blocks always use the base frame; other objects select the alternate frame through their simulation clock or charging state.

These are initial original motifs, not a claim of final animation quality or instructor acceptance. Record actual in-game visual checks before declaring visual acceptance complete.
