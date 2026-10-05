using GameProject.Core;
using GameProject.Objects.PlayerItems;
using GameProject.Objects.PlayerStates;
using GameProject.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameProject.Objects;

public enum PlayerAction { Idle, Walking, Attacking, Damaged }

/// <summary>Owns player gameplay state; drawing remains behind ISprite.</summary>
public sealed class Player : IGameObject
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
    }

    public string Name => "Link";
    public Vector2 Position { get; private set; }
    public Direction Facing { get; private set; } = Direction.Down;
    public PlayerAction Action => stateMachine.Action;
    public int Health { get; private set; } = 5;
    public int SelectedItem { get; private set; } = 1;
    public bool HasActiveArrow => arrow?.IsActive ?? false;
    public bool HasActiveBoomerang => boomerang?.IsActive ?? false;
    public bool HasActiveBomb => bomb?.IsActive ?? false;

    public void SetMovement(Vector2 direction)
    {
        pendingDirection = direction;
        if (direction == Vector2.Zero || !stateMachine.AllowsMovement)
        {
            return;
        }

        Facing = Math.Abs(direction.X) > Math.Abs(direction.Y)
            ? (direction.X < 0 ? Direction.Left : Direction.Right)
            : (direction.Y < 0 ? Direction.Up : Direction.Down);
    }

    public void Attack()
    {
        if (stateMachine.TryAttack())
        {
            ResetVisualActionClock();
        }
    }

    public void TakeDamage()
    {
        if (!stateMachine.TryTakeDamage())
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
        if (arrow is null || Action is PlayerAction.Attacking or PlayerAction.Damaged) return false;

        return arrow.TryLaunch(ItemStartPosition(ArrowProjectile.Size), Facing);
    }

    /// <summary>Uses slot two; an active boomerang returns to the player's current position.</summary>
    public bool TryThrowBoomerang()
    {
        SelectItem(2);
        if (boomerang is null || Action is PlayerAction.Attacking or PlayerAction.Damaged) return false;
        return boomerang.TryLaunch(ItemStartPosition(BoomerangProjectile.Size), Facing);
    }

    /// <summary>Uses slot three; the bomb stays where it is placed until its visual explosion ends.</summary>
    public bool TryPlaceBomb()
    {
        SelectItem(3);
        if (bomb is null || Action is PlayerAction.Attacking or PlayerAction.Damaged) return false;
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
        double seconds = gameTime.ElapsedGameTime.TotalSeconds;
        visualActionTime += seconds;
        bool wantsToMove = pendingDirection != Vector2.Zero;
        stateMachine.Update(seconds, wantsToMove);
        if (Action != visualAction)
        {
            ResetVisualActionClock();
        }

        if (stateMachine.AllowsMovement && wantsToMove)
        {
            pendingDirection.Normalize();
            Position += pendingDirection * Speed * (float)seconds;
            Position = new Vector2(
                MathHelper.Clamp(Position.X, bounds.Left, bounds.Right - Width),
                MathHelper.Clamp(Position.Y, bounds.Top, bounds.Bottom - Height));
        }

        pendingDirection = Vector2.Zero;
        arrow?.Update(gameTime);
        boomerang?.Update(gameTime);
        bomb?.Update(gameTime);
    }

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

    public void Reset()
    {
        Position = startPosition;
        Facing = Direction.Down;
        Health = 5;
        SelectedItem = 1;
        pendingDirection = Vector2.Zero;
        stateMachine.Reset();
        arrow?.Reset();
        boomerang?.Reset();
        bomb?.Reset();
        ResetVisualActionClock();
    }
}
