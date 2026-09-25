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
    private double animationTime;

    public Player(Vector2 position, Rectangle bounds, SpriteFactory sprites)
    {
        startPosition = position;
        this.bounds = bounds;
        sprite = sprites.CreatePlayerSprite();
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

    public void Attack() => stateMachine.TryAttack();

    public void TakeDamage()
    {
        if (!stateMachine.TryTakeDamage())
        {
            return;
        }

        Health = Math.Max(0, Health - 1);
    }

    public void SelectItem(int slot) => SelectedItem = Math.Clamp(slot, 1, 3);

    public void Update(GameTime gameTime)
    {
        double seconds = gameTime.ElapsedGameTime.TotalSeconds;
        animationTime += seconds;
        bool wantsToMove = pendingDirection != Vector2.Zero;
        stateMachine.Update(seconds, wantsToMove);

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
        bool alternate = ((int)(animationTime * 8) % 2) == 1;
        if (Action == PlayerAction.Damaged && alternate) return; // Damage flash.
        sprite.Draw(spriteBatch, Position + GetActionOffset(), Facing, alternate && Action != PlayerAction.Idle);
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
        animationTime = 0;
        pendingDirection = Vector2.Zero;
        stateMachine.Reset();
    }
}
