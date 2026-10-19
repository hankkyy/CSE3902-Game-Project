using GameProject.Combat;
using GameProject.Core;
using GameProject.Objects;
using GameProject.Objects.Enemies;
using GameProject.Objects.PlayerItems;
using GameProject.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

internal static class CombatChecks
{
    private static readonly Rectangle Bounds = new(0, 0, 1000, 1000);
    private static int checks;

    public static int Run()
    {
        checks = 0;
        ContactDamage();
        SwordDamage();
        ArrowDamage();
        BombDamage();
        BoomerangDamage();
        EnemyWallsAndDeath();
        ProjectileLifetime();
        NpcAndInactivePairs();
        return checks;
    }

    private static void ContactDamage()
    {
        Player player = MakePlayer();
        Gel enemy = MakeEnemy(new Vector2(120, 100));
        CombatCollisionHandler.PlayerTouchesEnemy(player, enemy);
        Check(player.Health == 4 && player.Position.X == 88, "Contact damages and knocks away from the enemy");
        Vector2 afterHit = player.Position;
        for (int frame = 0; frame < 20; frame++)
        {
            Check(!player.ReceiveContactDamage(Vector2.Zero), "Invulnerability rejects another hit");
            player.Update(Time(0.01));
        }
        Check(player.Health == 4 && player.Position == afterHit, "Repeated damage does not move or hurt player");
        player.Update(Time(0.26));
        Check(player.ReceiveContactDamage(Vector2.Zero) && player.Health == 3, "Damage works after time window ends");

        EnemyProjectile shot = new(new RecordingSprite());
        shot.Launch(player.Position, Direction.Right);
        CombatCollisionHandler.EnemyProjectileHitsPlayer(shot, player);
        Check(!shot.IsActive && player.Health == 3, "Invulnerable player still consumes incoming shot");
        player.Reset();
        shot.Launch(player.Position, Direction.Right);
        CombatCollisionHandler.EnemyProjectileHitsPlayer(shot, player);
        CombatCollisionHandler.EnemyProjectileHitsPlayer(shot, player);
        Check(player.Health == 4 && !shot.IsActive, "Enemy shot damages once and deactivates");

        Player corner = MakePlayer(Vector2.Zero);
        corner.ReceiveContactDamage(new Vector2(100, 0));
        Check(corner.Position.X == 0, "Knockback stays in room bounds");
        for (int hit = 0; hit < 6; hit++)
        {
            corner.Update(Time(0.5));
            corner.ReceiveContactDamage(Vector2.Zero);
        }
        Check(corner.Health == 0 && !corner.IsActive, "Player health cannot fall below zero");
        corner.Attack();
        Check(corner.SwordHitbox == Rectangle.Empty && !corner.TryFireArrow(), "Defeated player cannot start attacks");
        corner.Reset();
        Check(corner.Health == 5 && corner.IsActive && corner.Position == Vector2.Zero, "Reset restores player");
    }

    private static void SwordDamage()
    {
        (Direction Facing, Vector2 Movement, Vector2 Target)[] cases =
        [
            (Direction.Up, new Vector2(0, -1), new Vector2(100, 75)),
            (Direction.Down, new Vector2(0, 1), new Vector2(100, 134)),
            (Direction.Left, new Vector2(-1, 0), new Vector2(74, 100)),
            (Direction.Right, new Vector2(1, 0), new Vector2(128, 100))
        ];
        foreach (var example in cases)
        {
            Player player = MakePlayer();
            Gel first = MakeEnemy(example.Target);
            Gel second = MakeEnemy(example.Target);
            CombatCollisionHandler.SwordHitsEnemy(player, first);
            Check(first.Health == 3, "Idle player has no damaging sword");
            player.SetMovement(example.Movement);
            player.Update(Time(0));
            player.Attack();
            Check(player.Facing == example.Facing, "Sword follows facing direction");
            for (int frame = 0; frame < 20; frame++)
            {
                CombatCollisionHandler.SwordHitsEnemy(player, first);
                player.Attack(); // A rejected repeated input must not clear the hit history.
                player.Update(Time(0.01));
            }
            CombatCollisionHandler.SwordHitsEnemy(player, second);
            Check(first.Health == 2 && second.Health == 2, "One swing hits each enemy only once");
            player.Update(Time(0.1));
            player.Attack();
            CombatCollisionHandler.SwordHitsEnemy(player, first);
            Check(first.Health == 1, "A new swing can hit the same enemy again");
            player.Reset();
            first.Reset();
            player.SetMovement(example.Movement);
            player.Update(Time(0));
            player.Attack();
            CombatCollisionHandler.SwordHitsEnemy(player, first);
            Check(first.Health == 2, "Reset clears sword hit history");
        }
    }

    private static void ArrowDamage()
    {
        Player player = MakePlayer();
        Check(player.TryFireArrow(), "Actual player arrow launches");
        ArrowProjectile arrow = player.Arrow!;
        Gel enemy = MakeEnemy(arrow.Position);
        Gel behind = MakeEnemy(arrow.Position);
        CombatCollisionHandler.ArrowHitsEnemy(arrow, enemy);
        CombatCollisionHandler.ArrowHitsEnemy(arrow, behind);
        Check(enemy.Health == 2 && behind.Health == 3 && !arrow.IsActive, "Arrow is consumed by its first target");
        arrow.Draw(null!);
        arrow.TryLaunch(new Vector2(200, 200), Direction.Right);
        CombatCollisionHandler.ArrowHitsSolid(arrow, new Rectangle(205, 190, 20, 50));
        Check(!arrow.IsActive, "Arrow stops at solid geometry");
        arrow.TryLaunch(new Vector2(200, 200), Direction.Right);
        CombatCollisionHandler.ArrowHitsSolid(arrow, new Rectangle(400, 400, 20, 20));
        Check(arrow.IsActive, "Distant walls do not consume arrows");
        player.Reset();
        Check(!arrow.IsActive, "Full player reset clears arrow");
    }

    private static void BombDamage()
    {
        RecordingSprite explosion = new();
        PlacedBomb bomb = new(Bounds, new RecordingSprite(), explosion);
        Gel first = MakeEnemy(new Vector2(100, 100));
        Gel second = MakeEnemy(new Vector2(120, 100));
        bomb.TryPlace(new Vector2(110, 110));
        CombatCollisionHandler.BombHitsEnemy(bomb, first);
        Check(first.Health == 3 && bomb.ExplosionHitbox == Rectangle.Empty, "Bomb fuse cannot hurt enemies");
        bomb.Update(Time(1.2));
        for (int frame = 0; frame < 20; frame++)
        {
            CombatCollisionHandler.BombHitsEnemy(bomb, first);
            CombatCollisionHandler.BombHitsEnemy(bomb, second);
            bomb.Update(Time(0.01));
        }
        Check(first.Health == 2 && second.Health == 2, "Explosion damages each enemy once across frames");
        bomb.Draw(null!);
        Check(bomb.ExplosionHitbox.Location == explosion.Position.ToPoint(), "Damage area matches explosion drawing");
        bomb.Update(Time(0.2));
        CombatCollisionHandler.BombHitsEnemy(bomb, first);
        Check(first.Health == 2 && !bomb.IsExploding, "Expired explosion does no damage");
        bomb.TryPlace(new Vector2(110, 110));
        bomb.Update(Time(1.2));
        CombatCollisionHandler.BombHitsEnemy(bomb, first);
        Check(first.Health == 1, "New bomb can hit the same enemy");
        bomb.Reset();
        first.Reset();
        bomb.TryPlace(new Vector2(110, 110));
        bomb.Update(Time(1.2));
        CombatCollisionHandler.BombHitsEnemy(bomb, first);
        Check(first.Health == 2, "Reset clears bomb hit history");
        foreach (Vector2 corner in new[] { Vector2.Zero, new Vector2(984, 984) })
        {
            bomb.Reset();
            bomb.TryPlace(corner);
            bomb.Update(Time(1.2));
            bomb.Draw(null!);
            Check(Bounds.Contains(bomb.ExplosionHitbox) && bomb.ExplosionHitbox.Location == explosion.Position.ToPoint(),
                "Corner bomb collision matches the clamped visual");
        }
    }

    private static void BoomerangDamage()
    {
        Vector2 start = new(100, 100);
        BoomerangProjectile boomerang = new(Bounds, new RecordingSprite(), () => start);
        boomerang.TryLaunch(start, Direction.Right);
        boomerang.Update(Time(0.1));
        Gel enemy = MakeEnemy(boomerang.Position);
        for (int frame = 0; frame < 20; frame++)
        {
            CombatCollisionHandler.BoomerangHitsEnemy(boomerang, enemy);
        }
        Check(enemy.Health == 2 && boomerang.IsReturning, "Boomerang hits once and turns back");
        boomerang.Update(Time(0.2));
        Check(!boomerang.IsActive && boomerang.Position == start, "Collision return reaches owner");
        boomerang.TryLaunch(start, Direction.Right);
        boomerang.Update(Time(0.1));
        CombatCollisionHandler.BoomerangHitsEnemy(boomerang, enemy);
        Check(enemy.Health == 1, "New throw clears boomerang hit history");

        (Direction Facing, Rectangle Wall)[] walls =
        [
            (Direction.Right, new Rectangle(130, 0, 10, 1000)),
            (Direction.Left, new Rectangle(76, 0, 10, 1000)),
            (Direction.Down, new Rectangle(0, 130, 1000, 10)),
            (Direction.Up, new Rectangle(0, 76, 1000, 10))
        ];
        foreach (var example in walls)
        {
            boomerang.Reset();
            boomerang.TryLaunch(start, example.Facing);
            boomerang.Update(Time(0.1));
            CombatCollisionHandler.BoomerangHitsSolid(boomerang, example.Wall);
            Check(boomerang.IsReturning && !boomerang.Hitbox.Intersects(example.Wall),
                "Boomerang starts returning outside each wall");
            CombatCollisionHandler.BoomerangHitsSolid(boomerang, example.Wall);
            boomerang.Update(Time(0.5));
            Check(!boomerang.IsActive && boomerang.Position == start, "Wall return reaches owner");
        }
        boomerang.Reset();
        boomerang.TryLaunch(start, Direction.Right);
        boomerang.Update(Time(0.15));
        Rectangle thinWall = new(130, 0, 10, 1000);
        CombatCollisionHandler.BoomerangHitsSolid(boomerang, thinWall);
        Check(boomerang.Hitbox.Right <= thinWall.Left, "Deep overlap returns to entry side, not through wall");
    }

    private static void EnemyWallsAndDeath()
    {
        Rectangle[] walls =
        [
            new(120, 90, 30, 60), new(80, 90, 30, 60),
            new(90, 118, 60, 30), new(90, 80, 60, 30)
        ];
        foreach (Rectangle wall in walls)
        {
            Gel enemy = MakeEnemy(new Vector2(100, 100));
            CombatCollisionHandler.EnemyHitsSolid(enemy, wall);
            Check(!enemy.Hitbox.Intersects(wall), "Enemy resolves wall penetration");
            Vector2 resolved = enemy.Position;
            enemy.Update(Time(0));
            Check(enemy.Position == resolved, "Next patrol update retains wall correction");
            enemy.Reset();
            Check(enemy.Position == new Vector2(100, 100), "Reset clears wall correction");
        }

        RecordingSprite sprite = new();
        EnemyCharacter[] enemies =
        [
            new Octorok("Octorok", new Vector2(100, 100), sprite, sprite),
            new PrismSentinel("Sentinel", new Vector2(100, 100), sprite, sprite),
            new Keese("Keese", new Vector2(100, 100), sprite),
            new Gel("Gel", new Vector2(100, 100), sprite),
            new RuneWisp("Wisp", new Vector2(100, 100), sprite),
            new ClockworkBeetle("Beetle", new Vector2(100, 100), sprite)
        ];
        foreach (EnemyCharacter enemy in enemies)
        {
            Check(!enemy.ReceiveDamage(0) && !enemy.ReceiveDamage(-1), "Nonpositive damage is ignored");
            enemy.Update(Time(2.1));
            enemy.ReceiveDamage(99);
            Vector2 deadPosition = enemy.Position;
            sprite.DrawCount = 0;
            enemy.Update(Time(4));
            enemy.Draw(null!);
            CombatCollisionHandler.EnemyHitsSolid(enemy, enemy.Hitbox);
            Check(enemy.Health == 0 && !enemy.IsActive && enemy.Position == deadPosition && sprite.DrawCount == 0,
                "Dead enemies stop moving, drawing, and responding to solids");
            if (enemy is Octorok octorok)
            {
                Check(!octorok.Projectile.IsActive, "Octorok death clears its projectile");
            }
            else if (enemy is PrismSentinel sentinel)
            {
                Check(!sentinel.Projectile.IsActive && !sentinel.IsCharging, "Sentinel death clears its projectile and charge");
            }
            enemy.Reset();
            Check(enemy.IsActive && enemy.Health == 3 && enemy.Position == new Vector2(100, 100),
                "Reset restores enemy health, activity, and start position");
        }
    }

    private static void ProjectileLifetime()
    {
        RecordingSprite sprite = new();
        Octorok octorok = new("Octorok", new Vector2(100, 100), sprite, sprite);
        PrismSentinel sentinel = new("Sentinel", new Vector2(100, 100), sprite, sprite);
        foreach (EnemyCharacter shooter in new EnemyCharacter[] { octorok, sentinel })
        {
            EnemyProjectile shot;
            if (shooter is Octorok octopus)
            {
                shot = octopus.Projectile;
            }
            else
            {
                shot = ((PrismSentinel)shooter).Projectile;
            }
            shooter.Update(Time(2.01));
            Check(shot.IsActive, "Shooter creates shot on schedule");
            CombatCollisionHandler.EnemyProjectileHitsSolid(shot, shot.Hitbox);
            Vector2 stopped = shot.Position;
            for (int frame = 0; frame < 10; frame++)
            {
                shooter.Update(Time(0.01));
                Check(!shot.IsActive && shot.Position == stopped, "Consumed shot does not respawn next frame");
            }
            shooter.Update(Time(1.9));
            Check(shot.IsActive, "Next scheduled shot can spawn after previous collision");
            shooter.Reset();
            Check(!shot.IsActive, "Reset clears shot and its schedule");
            shooter.Update(Time(1.99));
            Check(!shot.IsActive, "Reset shot does not fire early");
            shooter.Update(Time(0.01));
            Check(shot.IsActive, "Reset schedule fires at two seconds");
        }
    }

    private static void NpcAndInactivePairs()
    {
        Player player = MakePlayer();
        Npc npc = new("Keeper", player.Position, new RecordingSprite());
        CombatCollisionHandler.PlayerTouchesEnemy(player, npc);
        Check(player.Health == 5 && !npc.ReceiveDamage(1), "NPC does not hurt player or take combat damage");
        ArrowProjectile arrow = new(Bounds, new RecordingSprite());
        arrow.TryLaunch(npc.Position, Direction.Right);
        CombatCollisionHandler.ArrowHitsEnemy(arrow, npc);
        Check(arrow.IsActive && npc.Health == 3, "Friendly NPC does not consume arrow");
        Gel dead = MakeEnemy(player.Position);
        dead.ReceiveDamage(99);
        CombatCollisionHandler.PlayerTouchesEnemy(player, dead);
        CombatCollisionHandler.ArrowHitsEnemy(arrow, dead);
        Check(player.Health == 5 && arrow.IsActive, "Dead enemies do not hurt player or consume weapons");
        EnemyProjectile inactive = new(new RecordingSprite());
        CombatCollisionHandler.EnemyProjectileHitsPlayer(inactive, player);
        Check(player.Health == 5, "Inactive shot cannot damage player");
        Player actualPlayer = MakePlayer();
        actualPlayer.TryFireArrow();
        actualPlayer.TryThrowBoomerang();
        actualPlayer.TryPlaceBomb();
        actualPlayer.Reset();
        Check(!actualPlayer.HasActiveArrow && !actualPlayer.HasActiveBoomerang && !actualPlayer.HasActiveBomb,
            "Full reset clears all actual player weapons together");
    }

    private static Player MakePlayer(Vector2? position = null)
    {
        RecordingSprite sprite = new();
        return new Player(position ?? new Vector2(100, 100), Bounds, sprite, sprite, sprite, sprite, sprite);
    }

    private static Gel MakeEnemy(Vector2 position) => new("Gel", position, new RecordingSprite());
    private static GameTime Time(double seconds) => new(TimeSpan.Zero, TimeSpan.FromSeconds(seconds));

    private static void Check(bool condition, string description)
    {
        if (!condition)
        {
            throw new InvalidOperationException(description);
        }
        checks++;
    }

    private sealed class RecordingSprite : ISprite
    {
        public int DrawCount { get; set; }
        public Vector2 Position { get; private set; }

        public void Draw(SpriteBatch spriteBatch, Vector2 position, Direction direction, bool alternateFrame)
        {
            DrawCount++;
            Position = position;
        }
    }
}
