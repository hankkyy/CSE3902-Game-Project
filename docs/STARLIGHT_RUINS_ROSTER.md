# Starlight Ruins — designed Sprint 2 object roster

星灯遗迹 is an original extension theme for this team's current course scaffold. The player explores an abandoned observatory and finds a Star Shard to restore its lamp. This preserves existing player controls and baseline characters while adding ten themed objects with dedicated classes and original pixel motifs.

This document defines a small first-dungeon concept so the roster has a purpose. The current executable remains a Sprint 2 functionality gallery: rooms, collection, puzzle solving, collisions, and win conditions described as future roles below are not implemented here. Counts are design choices, not a claim that the instructor requires exactly 6/9/7 types or that the submission has passed grading.

## Planned room roles — design only

| Future room | Relevant roster entries | Intended later role |
|---|---|---|
| Entrance hall / 入门廊厅 | Stone, Rune Tile, Lantern Keeper, Map, Heart | Introduce the observatory and its routes |
| Quiet-water corridor / 静水回廊 | Water, Key, Rupee, Octorok, Gel | Water-themed traversal and basic encounters |
| Echo workshop / 回声工坊 | Push Block, Statue, Bow, Bomb, Clockwork Beetle | Mechanical obstacles and item introduction |
| Starlamp court / 星灯中庭 | Crystal Pillar, Compass, Boomerang, Rune Wisp, Keese | Light motifs and mobile enemies |
| Prism sanctuary / 棱镜圣所 | Statue, Crystal Pillar, Prism Sentinel, Star Shard | A guarded relic and the dungeon's eventual objective |

## Six stationary blocks — T/Y

| Kind / class | Display name | Sprint 2 behavior |
|---|---|---|
| Stone / StoneBlock | Stone Block | Existing stationary stone |
| Push / PushBlock | Push Block | Stationary preview; no actual pushing |
| Water / WaterBlock | Water Tile | Stationary preview; no collision |
| Statue / StatueBlock | Statue | Existing stationary statue |
| CrystalPillar / CrystalPillar | Crystal Pillar | New purple/cyan pillar; stationary |
| RuneTile / RuneTile | Rune Tile | New engraved tile; stationary |

## Nine item previews — U/I

| Kind / class | Display name | Sprint 2 behavior |
|---|---|---|
| Heart / HeartItem | Heart | Existing in-place preview |
| Rupee / RupeeItem | Rupee | Existing in-place preview |
| Key / KeyItem | Key | Existing in-place preview |
| Bomb / BombItem | Bomb | Pickup preview; separate from a placed bomb |
| Bow / BowItem | Bow | New bow silhouette and preview shimmer |
| Boomerang / BoomerangItem | Boomerang | New pickup silhouette; separate from thrown motion |
| Map / MapItem | Ruins Map | New route-map motif and preview shimmer |
| Compass / CompassItem | Star Compass | New compass motif and preview shimmer |
| StarShard / StarShardItem | Star Shard | New sparkling relic motif |

All previews stay in place and use the existing elapsed-time preview clock. They do not collect, equip, or activate anything. Player item use remains `1` arrow, `2` boomerang, and `3` bomb; the two systems serve different demonstrations.

## Seven enemies/NPCs — O/P

| Kind / class | Display name | Sprint 2 behavior |
|---|---|---|
| Octorok / Octorok | Octorok | Existing horizontal patrol and shots |
| Keese / Keese | Keese | Existing figure-eight flight |
| Gel / Gel | Gel | Existing rest/hop cycle |
| OldMan / Npc | Lantern Keeper | Existing stationary NPC, renamed only for the theme |
| RuneWisp / RuneWisp | Rune Wisp | New small elliptical float with particle flicker |
| ClockworkBeetle / ClockworkBeetle | Clockwork Beetle | New four-sided patrol with matching facing direction |
| PrismSentinel / PrismSentinel | Prism Sentinel | New stationary charge cycle, alternating right/left light shots every two seconds |

The keeper is not counted as a second NPC in addition to Old Man. Existing enum values retain their ordering; new kinds are appended. Each selection has synchronized preview/play-area copies. Shots and movement stay within the current display envelopes, and no object-to-object damage is introduced.

## Manual acceptance — perform and record

1. Start the game; the window title and menu subtitle should identify Starlight Ruins.
2. Enter gameplay. Confirm counts `BLOCKS 1/6`, `ITEMS 1/9`, and `ENEMIES / NPCS 1/7`.
3. Press Y six times: return to Stone. Press T from Stone: Rune Tile appears. Wait on Crystal Pillar/Rune Tile: they must stay stationary.
4. Press I nine times: return to Heart. Press U from Heart: Star Shard appears. Check the new bow, boomerang, map, compass, and shard have different visible motifs.
5. Press P seven times: return to Octorok. Press O from Octorok: Prism Sentinel appears. The side preview and play-area copy must match.
6. Observe Rune Wisp floating, Clockwork Beetle turning around all four sides, and Prism Sentinel charging then shooting. Wait through at least two shots to see both directions.
7. Change every gallery selection, wait for an active shot, then press R. Confirm indexes reset to zero. Revisit hidden entries: their motion/animation/charge should start fresh.
8. Retest 1/2/3 item use, player movement, Z/N, E, menu-held keys, Q/Escape, and HUD readability. No test here requires collision or collecting the new map/relic.

## Verification and limits

`tests/ItemsBlocks` enumerates all block/item kinds and checks stationary placement, animation timing, draw purity, wraparound, and hidden resets. `tests/Enemies/RuinsEnemyChecks.cs` verifies the three new behaviors and all seven paired `EnemyObject` kinds, including light-shot timing and reset. The existing full verification command remains `bash scripts/verify.sh`.

The new sprites are deliberately small original procedural motifs. Older gallery art remains placeholder-based; object-specific animation polish, player item-use poses, manual visual acceptance, final documentation, and code-review records still require attention. This roster expansion is a concrete implementation of the proposed mini-dungeon object list, not certification of the entire Project Submission.
