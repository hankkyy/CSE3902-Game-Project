using GameProject.Objects;
using GameProject.Objects.Enemies;
using GameProject.Objects.PlayerItems;
using Microsoft.Xna.Framework;

namespace GameProject.Combat;

/// <summary>
/// Responses for Michael's collision pairs. The shared dispatcher chooses the pair;
/// these methods check activity and overlap before changing the objects.
/// </summary>
public static class CombatCollisionHandler
{
    private const int WeaponDamage = 1;

    public static void PlayerTouchesEnemy(Player player, EnemyCharacter enemy)
    {
        ArgumentNullException.ThrowIfNull(player);
        ArgumentNullException.ThrowIfNull(enemy);
        if (player.IsActive && enemy.IsActive && enemy.IsHostile && player.Hitbox.Intersects(enemy.Hitbox))
        {
            player.ReceiveContactDamage(enemy.Hitbox.Center.ToVector2());
        }
    }

    public static void EnemyProjectileHitsPlayer(EnemyProjectile projectile, Player player)
    {
        ArgumentNullException.ThrowIfNull(projectile);
        ArgumentNullException.ThrowIfNull(player);
        if (!projectile.IsActive || !player.IsActive || !projectile.Hitbox.Intersects(player.Hitbox))
        {
            return;
        }

        player.ReceiveContactDamage(projectile.Hitbox.Center.ToVector2());
        // A shot is consumed even if the player's invulnerability blocked its damage.
        projectile.Deactivate();
    }

    public static void SwordHitsEnemy(Player player, EnemyCharacter enemy)
    {
        ArgumentNullException.ThrowIfNull(player);
        ArgumentNullException.ThrowIfNull(enemy);
        if (!player.IsActive || player.Action != PlayerAction.Attacking || !CanDamage(enemy) ||
            !player.SwordHitbox.Intersects(enemy.Hitbox))
        {
            return;
        }

        // Add returns false if this swing already hit this enemy.
        if (player.SwordTargets.Add(enemy))
        {
            enemy.ReceiveDamage(WeaponDamage);
        }
    }

    public static void ArrowHitsEnemy(ArrowProjectile arrow, EnemyCharacter enemy)
    {
        ArgumentNullException.ThrowIfNull(arrow);
        ArgumentNullException.ThrowIfNull(enemy);
        if (arrow.IsActive && CanDamage(enemy) && arrow.Hitbox.Intersects(enemy.Hitbox))
        {
            enemy.ReceiveDamage(WeaponDamage);
            arrow.Deactivate();
        }
    }

    public static void ArrowHitsSolid(ArrowProjectile arrow, Rectangle solid)
    {
        ArgumentNullException.ThrowIfNull(arrow);
        if (arrow.IsActive && OverlapsSolid(arrow.Hitbox, solid))
        {
            arrow.Deactivate();
        }
    }

    public static void BombHitsEnemy(PlacedBomb bomb, EnemyCharacter enemy)
    {
        ArgumentNullException.ThrowIfNull(bomb);
        ArgumentNullException.ThrowIfNull(enemy);
        if (!bomb.IsExploding || !CanDamage(enemy) || !bomb.ExplosionHitbox.Intersects(enemy.Hitbox))
        {
            return;
        }

        if (bomb.HitTargets.Add(enemy))
        {
            enemy.ReceiveDamage(WeaponDamage);
        }
    }

    public static void BoomerangHitsEnemy(BoomerangProjectile boomerang, EnemyCharacter enemy)
    {
        ArgumentNullException.ThrowIfNull(boomerang);
        ArgumentNullException.ThrowIfNull(enemy);
        if (!boomerang.IsActive || !CanDamage(enemy) || !boomerang.Hitbox.Intersects(enemy.Hitbox))
        {
            return;
        }

        if (boomerang.HitTargets.Add(enemy))
        {
            enemy.ReceiveDamage(WeaponDamage);
        }

        boomerang.BeginReturn(Vector2.Zero);
    }

    public static void BoomerangHitsSolid(BoomerangProjectile boomerang, Rectangle solid)
    {
        ArgumentNullException.ThrowIfNull(boomerang);
        if (!boomerang.IsActive || !OverlapsSolid(boomerang.Hitbox, solid))
        {
            return;
        }

        Rectangle previous = new((int)boomerang.PreviousPosition.X, (int)boomerang.PreviousPosition.Y,
            BoomerangProjectile.Size, BoomerangProjectile.Size);
        Vector2 correction = CombatGeometry.Separation(boomerang.Hitbox, solid, previous);
        boomerang.BeginReturn(correction);
    }

    public static void EnemyHitsSolid(EnemyCharacter enemy, Rectangle solid)
    {
        ArgumentNullException.ThrowIfNull(enemy);
        if (enemy.IsActive && OverlapsSolid(enemy.Hitbox, solid))
        {
            Vector2 correction = CombatGeometry.Separation(enemy.Hitbox, solid, enemy.PreviousHitbox);
            enemy.ApplyCollisionCorrection(correction);
        }
    }

    public static void EnemyProjectileHitsSolid(EnemyProjectile projectile, Rectangle solid)
    {
        ArgumentNullException.ThrowIfNull(projectile);
        if (projectile.IsActive && OverlapsSolid(projectile.Hitbox, solid))
        {
            projectile.Deactivate();
        }
    }

    private static bool CanDamage(EnemyCharacter enemy)
    {
        return enemy.IsActive && enemy.IsHostile;
    }

    private static bool OverlapsSolid(Rectangle hitbox, Rectangle solid)
    {
        return solid.Width > 0 && solid.Height > 0 && hitbox.Intersects(solid);
    }
}
