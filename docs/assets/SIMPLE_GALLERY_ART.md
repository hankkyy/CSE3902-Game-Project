# Simple gallery art — Starlight Ruins

The twelve baseline gallery motifs in `ClassicRuinsSprite.cs` were newly drawn as small code-defined patterns with coding-assistant help. No external images, downloads, fonts, or new packages are used. They reuse the existing one-pixel texture. This document records provenance; it does not change the license of the repository or the separate player atlas.

Stone has mortar lines; Push has a wooden brace; Water has stationary wave marks; Statue has an engraved face. Heart, Rupee, Key and Bomb have recognizable silhouettes with two-frame highlights or fuse details. Octorok changes its feet, Keese flaps, Gel squashes, and the Lantern Keeper has a flickering lantern. The existing `alternateFrame` supplied by each object controls the frames, so Draw remains state-free and Reset uses the existing timing.

All motifs fit the original sprite width/height. Existing movement, projectiles, key bindings, gallery counts and object behavior are unchanged. These are deliberately minimal illustrations, not elaborate final artwork or a new animation/state system for the player.

Manual acceptance: cycle T/Y through all six blocks, U/I through all nine items, and O/P through all seven characters. Check the first four entries of each category for the new art, that blocks stay still, that item highlights change, and that the bat/gel/feet animate. Check the preview and play-area enemy match, then press R and repeat. Confirm no clipping or unreadable silhouettes and recheck 1/2/3. This record is instructions, not a claim that manual acceptance was performed.
