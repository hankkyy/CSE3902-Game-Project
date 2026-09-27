using GameProject.Core;
using GameProject.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameProject.Objects.Enemies;

/// <summary>Patrols horizontally and fires a bounded shot every two seconds.</summary>
public sealed class Octorok : EnemyCharacter
{
    private const double ShotInterval = 2;
    private const float PatrolRange = 40;
    private const float ShotOffset = 34;
    public EnemyProjectile Projectile { get; }

    public Octorok(string name, Vector2 position, ISprite sprite, ISprite projectileSprite)
        : base(name, position, sprite) => Projectile = new EnemyProjectile(projectileSprite);

    protected override void UpdateMovement()
    {
        Position = PositionAt(elapsed);
        Facing = FacingAt(elapsed);
    }

    private Vector2 PositionAt(double time) =>
        startPosition + new Vector2((float)Math.Sin(time * 1.8) * PatrolRange, 0);
    private static Direction FacingAt(double time) =>
        Math.Cos(time * 1.8) < 0 ? Direction.Left : Direction.Right;

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
        // Absolute simulation time makes long frames equivalent to small updates.
        double shotTime = Math.Floor(elapsed / ShotInterval) * ShotInterval;
        Projectile.Reset();
        if (shotTime < ShotInterval) return;
        Direction direction = FacingAt(shotTime);
        Vector2 origin = PositionAt(shotTime) +
            new Vector2(direction == Direction.Left ? -ShotOffset : ShotOffset, 0);
        Projectile.Launch(origin, direction);
        Projectile.Update(new GameTime(TimeSpan.Zero, TimeSpan.FromSeconds(elapsed - shotTime)));
        // Leave space for the existing 28-pixel placeholder sprite inside the panel.
        if (Projectile.Position.X < startPosition.X - 76 || Projectile.Position.X > startPosition.X + 184)
        {
            Projectile.Reset();
        }
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        base.Draw(spriteBatch);
        Projectile.Draw(spriteBatch);
    }

    public override void Reset()
    {
        base.Reset();
        Projectile.Reset();
    }
}
