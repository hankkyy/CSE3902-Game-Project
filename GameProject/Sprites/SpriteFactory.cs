using GameProject.Objects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameProject.Sprites;

/// <summary>Centralizes visual creation so gameplay classes do not know texture details.</summary>
public sealed class SpriteFactory
{
    private readonly Texture2D pixel;
    private readonly Texture2D playerAtlas;

    public SpriteFactory(GraphicsDevice graphicsDevice)
    {
        pixel = new Texture2D(graphicsDevice, 1, 1);
        pixel.SetData([Color.White]);
        using MemoryStream stream = new(Convert.FromBase64String(PlayerSpriteAtlasData.PngBase64));
        playerAtlas = Texture2D.FromStream(graphicsDevice, stream);
    }

    public ISprite CreatePlayerSprite() => CreateAnimatedPlayerSprite();

    public ISprite CreateArrowSprite() => new ArrowSprite(pixel);

    public ISprite CreateBoomerangSprite() => new BoomerangSprite(pixel);

    public ISprite CreateBombSprite() => new BombSprite(pixel);

    public ISprite CreateBombExplosionSprite() => new BombExplosionSprite(pixel);

    public ISprite CreateLightBoltSprite() => new RuinsSprite(pixel, RuinsVisual.LightBolt);

    /// <summary>Additive animation entry point for the player-state owner; existing calls remain valid.</summary>
    public IAnimatedPlayerSprite CreateAnimatedPlayerSprite() => new PlayerSprite(playerAtlas);

    public ISprite CreateBlockSprite(BlockKind kind) => kind switch
    {
        BlockKind.Stone => new ClassicRuinsSprite(pixel, ClassicVisual.Stone),
        BlockKind.Push => new ClassicRuinsSprite(pixel, ClassicVisual.Push),
        BlockKind.Water => new ClassicRuinsSprite(pixel, ClassicVisual.Water),
        BlockKind.Statue => new ClassicRuinsSprite(pixel, ClassicVisual.Statue),
        BlockKind.CrystalPillar => new RuinsSprite(pixel, RuinsVisual.CrystalPillar),
        BlockKind.RuneTile => new RuinsSprite(pixel, RuinsVisual.RuneTile),
        _ => throw new ArgumentOutOfRangeException(nameof(kind))
    };

    public ISprite CreateItemSprite(ItemKind kind) => kind switch
    {
        ItemKind.Heart => new ClassicRuinsSprite(pixel, ClassicVisual.Heart),
        ItemKind.Rupee => new ClassicRuinsSprite(pixel, ClassicVisual.Rupee),
        ItemKind.Key => new ClassicRuinsSprite(pixel, ClassicVisual.Key),
        ItemKind.Bomb => new ClassicRuinsSprite(pixel, ClassicVisual.Bomb),
        ItemKind.Bow => new RuinsSprite(pixel, RuinsVisual.Bow),
        ItemKind.Boomerang => new RuinsSprite(pixel, RuinsVisual.Boomerang),
        ItemKind.Map => new RuinsSprite(pixel, RuinsVisual.Map),
        ItemKind.Compass => new RuinsSprite(pixel, RuinsVisual.Compass),
        ItemKind.StarShard => new RuinsSprite(pixel, RuinsVisual.StarShard),
        _ => throw new ArgumentOutOfRangeException(nameof(kind))
    };

    public ISprite CreateEnemySprite(EnemyKind kind) => kind switch
    {
        EnemyKind.Octorok => new ClassicRuinsSprite(pixel, ClassicVisual.Octorok),
        EnemyKind.Keese => new ClassicRuinsSprite(pixel, ClassicVisual.Keese),
        EnemyKind.Gel => new ClassicRuinsSprite(pixel, ClassicVisual.Gel),
        EnemyKind.OldMan => new ClassicRuinsSprite(pixel, ClassicVisual.Keeper),
        EnemyKind.RuneWisp => new RuinsSprite(pixel, RuinsVisual.RuneWisp),
        EnemyKind.ClockworkBeetle => new RuinsSprite(pixel, RuinsVisual.ClockworkBeetle),
        EnemyKind.PrismSentinel => new RuinsSprite(pixel, RuinsVisual.PrismSentinel),
        _ => throw new ArgumentOutOfRangeException(nameof(kind))
    };

    public void DrawPanel(SpriteBatch spriteBatch, Rectangle rectangle, Color color) => spriteBatch.Draw(pixel, rectangle, color);
}
