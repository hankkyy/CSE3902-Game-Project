using GameProject.Core;
using GameProject.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameProject.Objects.Enemies;

/// <summary>A stationary prism guard that charges, then alternates left/right light shots.</summary>
public sealed class PrismSentinel : EnemyCharacter
{
    private const double ShotInterval = 2;
    private const double ShotLifetime = 0.6;
    private const double ChargeStartsAt = 1.2;
    private readonly ISprite sprite;

    public PrismSentinel(string name, Vector2 position, ISprite sprite, ISprite projectileSprite)
        : base(name, position, sprite)
    {
        this.sprite = sprite;
        Projectile = new EnemyProjectile(projectileSprite);
    }

    public EnemyProjectile Projectile { get; }
    // Match GameTime's 100-nanosecond precision at exact charge/shot boundaries.
    private double SimulationTime => Math.Round(elapsed, 7);
    public bool IsCharging => SimulationTime % ShotInterval >= ChargeStartsAt;

    protected override void UpdateMovement()
    {
        Position = startPosition;
        long cycle = (long)Math.Floor(SimulationTime / ShotInterval);
        bool showingLastShot = cycle > 0 && SimulationTime % ShotInterval < ShotLifetime;
        long facingCycle = showingLastShot ? cycle - 1 : cycle;
        Facing = facingCycle % 2 == 0 ? Direction.Right : Direction.Left;
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
        double shotTime = Math.Floor(SimulationTime / ShotInterval) * ShotInterval;
        double shotAge = SimulationTime - shotTime;
        Projectile.Reset();
        if (shotTime < ShotInterval || shotAge >= ShotLifetime) return;

        long shotNumber = (long)Math.Floor(shotTime / ShotInterval);
        Direction shotDirection = shotNumber % 2 == 1 ? Direction.Right : Direction.Left;
        Vector2 offset = shotDirection == Direction.Right ? new Vector2(32, 10) : new Vector2(-12, 10);
        Projectile.Launch(startPosition + offset, shotDirection);
        Projectile.Update(new GameTime(TimeSpan.Zero, TimeSpan.FromSeconds(shotAge)));
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        sprite.Draw(spriteBatch, Position, Facing, IsCharging);
        Projectile.Draw(spriteBatch);
    }

    public override void Reset()
    {
        base.Reset();
        Projectile.Reset();
    }
}
