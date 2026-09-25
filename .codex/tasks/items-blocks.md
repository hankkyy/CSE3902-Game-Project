# Codex task: items and blocks (`feature/items-blocks`)

Work only in the owned paths from `AGENTS.md`. Add dedicated item and block classes behind `IGameObject`. Blocks remain stationary and non-interacting; items animate/move as intended. Preserve current `BlockObject` and `ItemObject` constructors for compatibility, deterministic reset, and gallery wraparound. Do not poll input or edit enemies, player, sprites, `Game1`, Core, or project files. Run `./scripts/verify.sh`; document T/Y and U/I cycling in both directions plus reset behavior.
