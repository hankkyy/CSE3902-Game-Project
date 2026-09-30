using GameProject.Core;
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
    private readonly Vector2 startPosition;
    private readonly Rectangle bounds;
    private readonly ISprite sprite;
    private readonly PlayerStateMachine stateMachine = new();
    private Vector2 pendingDirection;
    private PlayerAction visualAction = PlayerAction.Idle;
    private double visualActionTime;

    public Player(Vector2 position, Rectangle bounds, SpriteFactory sprites)
        : this(position, bounds, sprites.CreatePlayerSprite())
    {
    }

    /// <summary>Creates a player with an injected sprite for headless behavior verification.</summary>
    public Player(Vector2 position, Rectangle bounds, ISprite sprite)
    {
        startPosition = position;
        this.bounds = bounds;
        this.sprite = sprite;
        Position = position;
    }

    public string Name => "Link";
    public Vector2 Position { get; private set; }
    public Direction Facing { get; private set; } = Direction.Down;
    public PlayerAction Action => stateMachine.Action;
    public int Health { get; private set; } = 5;
    public int SelectedItem { get; private set; } = 1;

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
                MathHelper.Clamp(Position.X, bounds.Left, bounds.Right - 28),
                MathHelper.Clamp(Position.Y, bounds.Top, bounds.Bottom - 34));
        }

        pendingDirection = Vector2.Zero;
    }

    public void Draw(SpriteBatch spriteBatch)
    {
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
        ResetVisualActionClock();
    }
}
