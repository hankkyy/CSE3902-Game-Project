# Player sprites integration proposal

Status: **proposed; owner approval and integration pending**. No shared or player-owned file was changed by `feature/player-sprites`.

**October 5 update:** Player-state wiring is implemented in `integration/sprint2-complete` at `f3b30ca` and passes its automated integration tests. It remains absent from this isolated feature branch. The proposal below records the original integration plan; it is not a claim that the team integration is still missing. Formal review and physical keyboard acceptance were not verified by this audit. See [the final check](../assets/PLAYER_SPRITES_CHECK.md).

## 1 Problem

The frozen `ISprite.Draw` receives only position, direction, and `alternateFrame`. Walking, attacking, and damaged states can produce the same arguments. An implementation cannot reliably recover the missing state or a clip timer. Both the starter Player and the current player-state branch still call this boolean signature.

Separately, the frozen `GameProject.csproj` has no MonoGame Content Builder task. A `.mgcb` entry alone cannot make the production project automatically build/copy an XNB file.

## 2 Smallest additive surface

Implemented entirely inside the sprite-owned paths:

```csharp
public interface IAnimatedPlayerSprite : ISprite
{
    void Draw(SpriteBatch spriteBatch, Vector2 position, Direction direction,
              PlayerSpriteAnimation animation, double elapsedSeconds);
}

// Added to SpriteFactory; all old methods and signatures remain.
public IAnimatedPlayerSprite CreateAnimatedPlayerSprite();
```

`PlayerSpriteAnimation` contains Idle, Walking, Attacking, and Damaged. It describes visuals, not gameplay rules. Existing `ISprite` remains byte-for-byte unchanged. Source rectangles, atlas layout, colors, and frame selection remain in Sprites/Content.

## 3 Affected owners and callers

- **@hankkyy / feature/player-states:** opt into `IAnimatedPlayerSprite` in `Player`, create it with `CreateAnimatedPlayerSprite()`, and map `PlayerAction` to `PlayerSpriteAnimation` explicitly in the player-owned code. Pass time since the current visual state began. Reset that timer on transitions, accepted repeated actions, and `Reset()`. Accumulate time only in `Update(GameTime)`. Preserve the player's existing damage visibility policy and position offset if desired.
- **Integrator:** decide whether to replace the embedded PNG path with `Content.Load<Texture2D>("Player/moss-scout")`. A future approved factory overload can accept the loaded texture while preserving `SpriteFactory(GraphicsDevice)`. If selecting the pipeline route, add the approved MonoGame content build tooling and project entry.
- **@xing-gif / feature/input-quality:** only needs involvement if the integrator changes content-loading construction in `Game1`. No input change is required for the sprite API.
- Enemy/item/block owners have no migration work. Their factory methods and `ISprite.Draw` calls remain unchanged.

## 4 Migration and compatibility plan

1. Review and merge the sprite-owned implementation after CI and visual review. The existing game keeps working via the old boolean call.
2. The player-state owner opts into the additive interface. No source rectangle or timing table should be copied into Player; only the action-to-visual mapping and elapsed state time belong there.
3. Verify four directions with WASD/arrows, sword with Z/N, damage with E, and reset during each state with R. The full walking/attack/damage clips must now appear in the production scene.
4. Approve pipeline wiring separately if the team wants content managed through XNB. The checked-in PNG, buildable MGCB entry, and embedded bytes describe the same asset.

Suggested call after owner-approved wiring:

```csharp
sprite.Draw(spriteBatch, Position + GetActionOffset(), Facing,
    visualAnimation, visualElapsedSeconds);
```

The player-state owner chooses where to record transition time so that returning to the same action also resets a newly accepted action. Do not use wall-clock time, a mutable global callback, or draw-count animation as a workaround.

## 5 Approval

- [ ] @hankkyy approves and applies player-state wiring.
- [ ] Integrator approves automatic content pipeline wiring, or explicitly accepts embedded PNG loading for this sprint.
- [ ] Production keyboard acceptance is checked after integration.

The remaining integration is intentionally visible rather than hidden in edits to someone else's branch.
