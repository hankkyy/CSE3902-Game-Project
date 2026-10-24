using GameProject.Core;
using GameProject.Objects.PlayerItems;
using GameProject.Objects.PlayerStates;
using GameProject.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameProject.Objects;

public enum PlayerAction { Idle, Walking, Attacking, Damaged }

/// <summary>Stores the player's position, health, and current action.</summary>
public sealed class Player : IGameObject, IMovableCollidable
{
    private const float Speed = 190f;
    private const int Width = 28;
    private const int Height = 34;
    private readonly Vector2 startPosition;
    private readonly Rectangle bounds;
    private readonly ISprite sprite;
    private readonly ArrowProjectile? arrow;
    private readonly BoomerangProjectile? boomerang;
    private readonly PlacedBomb? bomb;
    private readonly PlayerStateMachine stateMachine = new();
    private Vector2 pendingDirection;
    private PlayerAction visualAction = PlayerAction.Idle;
    private double visualActionTime;

    public Player(Vector2 position, Rectangle bounds, SpriteFactory sprites)
        : this(position, bounds, sprites.CreatePlayerSprite(), sprites.CreateArrowSprite(),
            sprites.CreateBoomerangSprite(), sprites.CreateBombSprite(), sprites.CreateBombExplosionSprite())
    {
    }

    /// <summary>Creates a player with an injected sprite for headless behavior verification.</summary>
    public Player(Vector2 position, Rectangle bounds, ISprite sprite)
        : this(position, bounds, sprite, null)
    {
    }

    /// <summary>Adds an optional arrow sprite without changing existing constructor calls.</summary>
    public Player(Vector2 position, Rectangle bounds, ISprite sprite, ISprite? arrowSprite)
        : this(position, bounds, sprite, arrowSprite, null, null, null)
    {
    }

    /// <summary>Injects item visuals for verification while preserving all earlier constructors.</summary>
    public Player(Vector2 position, Rectangle bounds, ISprite sprite, ISprite? arrowSprite,
        ISprite? boomerangSprite, ISprite? bombSprite, ISprite? explosionSprite)
    {
        startPosition = position;
        this.bounds = bounds;
        this.sprite = sprite;
        arrow = arrowSprite is null ? null : new ArrowProjectile(bounds, arrowSprite);
        boomerang = boomerangSprite is null ? null : new BoomerangProjectile(bounds, boomerangSprite,
            () => Position + new Vector2((Width - BoomerangProjectile.Size) / 2f, (Height - BoomerangProjectile.Size) / 2f));
        bomb = bombSprite is null || explosionSprite is null ? null : new PlacedBomb(bounds, bombSprite, explosionSprite);
        Position = position;
        PreviousPosition = position;
    }

    public string Name => "Link";
    public Vector2 Position { get; private set; }
    public Vector2 PreviousPosition { get; private set; }
    public Rectangle Hitbox => CreateHitbox(Position);
    public Rectangle PreviousHitbox => CreateHitbox(PreviousPosition);
    public CollisionLayer CollisionLayer => CollisionLayer.Player;
    public bool IsActive => Health > 0;
    public Direction Facing { get; private set; } = Direction.Down;
    public PlayerAction Action => stateMachine.Action;
    public int Health { get; private set; } = 5;
    public int SelectedItem { get; private set; } = 1;
    public bool HasActiveArrow => arrow?.IsActive ?? false;
    public bool HasActiveBoomerang => boomerang?.IsActive ?? false;
    public bool HasActiveBomb => bomb?.IsActive ?? false;
    public ArrowProjectile? Arrow => arrow;
    public BoomerangProjectile? Boomerang => boomerang;
    public PlacedBomb? Bomb => bomb;
    internal HashSet<object> SwordTargets { get; } = [];

    /// <summary>The active blade reaches 20 pixels in front of the player.</summary>
    public Rectangle SwordHitbox
    {
        get
        {
            if (!IsActive || Action != PlayerAction.Attacking)
            {
                return Rectangle.Empty;
            }

            const int reach = 20;
            Rectangle body = Hitbox;
            if (Facing == Direction.Up)
            {
                return new Rectangle(body.Left, body.Top - reach, Width, reach);
            }
            else if (Facing == Direction.Down)
            {
                return new Rectangle(body.Left, body.Bottom, Width, reach);
            }
            else if (Facing == Direction.Left)
            {
                return new Rectangle(body.Left - reach, body.Top, reach, Height);
            }
            else
            {
                return new Rectangle(body.Right, body.Top, reach, Height);
            }
        }
    }

    /// <summary>Uses the existing 0.45-second damage state as the invulnerability window.</summary>
    public bool ReceiveContactDamage(Vector2 sourcePosition)
    {
        if (!IsActive || Action == PlayerAction.Damaged)
        {
            return false;
        }

        TakeDamage();
        const float knockbackDistance = 12;
        Vector2 away = Hitbox.Center.ToVector2() - sourcePosition;
        Vector2 knockback;
        if (Math.Abs(away.X) > Math.Abs(away.Y))
        {
            if (away.X < 0)
            {
                knockback = new Vector2(-knockbackDistance, 0);
            }
            else
            {
                knockback = new Vector2(knockbackDistance, 0);
            }
        }
        else if (away.Y < 0)
        {
            knockback = new Vector2(0, -knockbackDistance);
        }
        else
        {
            knockback = new Vector2(0, knockbackDistance);
        }

        Position += knockback;
        Position = new Vector2(
            MathHelper.Clamp(Position.X, bounds.Left, bounds.Right - Width),
            MathHelper.Clamp(Position.Y, bounds.Top, bounds.Bottom - Height));
        return true;
    }

    public void SetMovement(Vector2 direction)
    {
        pendingDirection = direction;
        if (direction == Vector2.Zero || !stateMachine.AllowsMovement)
        {
            return;
        }

        if (Math.Abs(direction.X) > Math.Abs(direction.Y))
        {
            if (direction.X < 0)
            {
                Facing = Direction.Left;
            }
            else
            {
                Facing = Direction.Right;
            }
        }
        else
        {
            if (direction.Y < 0)
            {
                Facing = Direction.Up;
            }
            else
            {
                Facing = Direction.Down;
            }
        }
    }

    public void Attack()
    {
        if (IsActive && stateMachine.TryAttack())
        {
            SwordTargets.Clear();
            ResetVisualActionClock();
        }
    }

    public void TakeDamage()
    {
        if (!IsActive || !stateMachine.TryTakeDamage())
        {
            return;
        }

        ResetVisualActionClock();
        Health = Math.Max(0, Health - 1);
    }

    public void SelectItem(int slot) => SelectedItem = Math.Clamp(slot, 1, 3);

    /// <summary>Uses slot one; shooting is blocked during sword attacks and damage.</summary>
    public bool TryFireArrow()
    {
        SelectItem(1);
        if (!IsActive || arrow is null || Action is PlayerAction.Attacking or PlayerAction.Damaged) return false;

        return arrow.TryLaunch(ItemStartPosition(ArrowProjectile.Size), Facing);
    }

    /// <summary>Uses slot two; an active boomerang returns to the player's current position.</summary>
    public bool TryThrowBoomerang()
    {
        SelectItem(2);
        if (!IsActive || boomerang is null || Action is PlayerAction.Attacking or PlayerAction.Damaged) return false;
        return boomerang.TryLaunch(ItemStartPosition(BoomerangProjectile.Size), Facing);
    }

    /// <summary>Uses slot three; the bomb stays where it is placed until its visual explosion ends.</summary>
    public bool TryPlaceBomb()
    {
        SelectItem(3);
        if (!IsActive || bomb is null || Action is PlayerAction.Attacking or PlayerAction.Damaged) return false;
        return bomb.TryPlace(ItemStartPosition(PlacedBomb.Size));
    }

    private Vector2 ItemStartPosition(int size)
    {
        Vector2 offset = Facing switch
        {
            Direction.Up => new Vector2((Width - size) / 2f, -size),
            Direction.Down => new Vector2((Width - size) / 2f, Height),
            Direction.Left => new Vector2(-size, (Height - size) / 2f),
            _ => new Vector2(Width, (Height - size) / 2f)
        };
        return Position + offset;
    }

    public void Update(GameTime gameTime)
    {
        ArgumentNullException.ThrowIfNull(gameTime);
        PreviousPosition = Position;
        double seconds = Math.Max(0, gameTime.ElapsedGameTime.TotalSeconds);
        visualActionTime += seconds;
        bool wantsToMove = pendingDirection != Vector2.Zero;
        stateMachine.Update(seconds, wantsToMove);
        if (Action != visualAction)
        {
            ResetVisualActionClock();
        }

        if (IsActive && stateMachine.AllowsMovement && wantsToMove)
        {
            pendingDirection.Normalize();
            Position += pendingDirection * Speed * (float)seconds;
            CorrectPosition(Position);
        }

        pendingDirection = Vector2.Zero;
        arrow?.Update(gameTime);
        boomerang?.Update(gameTime);
        bomb?.Update(gameTime);
    }

    /// <summary>Applies collision correction while preserving the play-area boundary.</summary>
    public void CorrectPosition(Vector2 position)
    {
        Position = new Vector2(
            MathHelper.Clamp(position.X, bounds.Left, bounds.Right - Width),
            MathHelper.Clamp(position.Y, bounds.Top, bounds.Bottom - Height));
    }

    private static Rectangle CreateHitbox(Vector2 position) =>
        new((int)MathF.Round(position.X), (int)MathF.Round(position.Y), Width, Height);

    public void Draw(SpriteBatch spriteBatch)
    {
        // Active items keep drawing even when the player flashes during damage.
        arrow?.Draw(spriteBatch);
        boomerang?.Draw(spriteBatch);
        bomb?.Draw(spriteBatch);
        bool alternate = ((int)(visualActionTime * 8) % 2) == 1;
        if (Action == PlayerAction.Damaged && alternate) return; // Damage flash.

        Vector2 drawPosition = Position + GetActionOffset();
        if (sprite is IAnimatedPlayerSprite animatedSprite)
        {
            animatedSprite.Draw(spriteBatch, drawPosition, Facing, AnimationFor(Action), visualActionTime);
            return;
        }

        sprite.Draw(spriteBatch, drawPosition, Facing, alternate && Action != PlayerAction.Idle);
    }

    private static PlayerSpriteAnimation AnimationFor(PlayerAction action) => action switch
    {
        PlayerAction.Walking => PlayerSpriteAnimation.Walking,
        PlayerAction.Attacking => PlayerSpriteAnimation.Attacking,
        PlayerAction.Damaged => PlayerSpriteAnimation.Damaged,
        _ => PlayerSpriteAnimation.Idle
    };

    private void ResetVisualActionClock()
    {
        visualAction = Action;
        visualActionTime = 0;
    }

    private Vector2 GetActionOffset()
    {
        if (Action != PlayerAction.Attacking)
        {
            return Vector2.Zero;
        }

        const float attackLungeDistance = 6f;
        return Facing switch
        {
            Direction.Up => new Vector2(0, -attackLungeDistance),
            Direction.Down => new Vector2(0, attackLungeDistance),
            Direction.Left => new Vector2(-attackLungeDistance, 0),
            _ => new Vector2(attackLungeDistance, 0)
        };
    }

    /// <summary>Moves between rooms while keeping health and the selected item.</summary>
    public void EnterRoom(Vector2 position)
    {
        if (!float.IsFinite(position.X) || !float.IsFinite(position.Y) ||
            position.X < bounds.Left || position.X + Width > bounds.Right ||
            position.Y < bounds.Top || position.Y + Height > bounds.Bottom)
        {
            throw new ArgumentOutOfRangeException(
                nameof(position), "The entry must fit inside the room.");
        }

        Position = position;
        PreviousPosition = position;
        pendingDirection = Vector2.Zero;
        stateMachine.Reset();
        SwordTargets.Clear();
        arrow?.Reset();
        boomerang?.Reset();
        bomb?.Reset();
        ResetVisualActionClock();
    }
    public void Reset()
    {
        Position = startPosition;
        PreviousPosition = startPosition;
        Facing = Direction.Down;
        Health = 5;
        SelectedItem = 1;
        pendingDirection = Vector2.Zero;
        stateMachine.Reset();
        SwordTargets.Clear();
        arrow?.Reset();
        boomerang?.Reset();
        bomb?.Reset();
        ResetVisualActionClock();
    }
}
