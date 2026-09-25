using GameProject.Core;
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
    private Vector2 pendingDirection;
    private double actionTimeRemaining;
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
    public PlayerAction Action { get; private set; } = PlayerAction.Idle;
    public int Health { get; private set; } = 5;
    public int SelectedItem { get; private set; } = 1;

    public void SetMovement(Vector2 direction)
    {
        pendingDirection = direction;
        if (direction == Vector2.Zero || Action is PlayerAction.Attacking or PlayerAction.Damaged) return;
        Facing = Math.Abs(direction.X) > Math.Abs(direction.Y)
            ? (direction.X < 0 ? Direction.Left : Direction.Right)
            : (direction.Y < 0 ? Direction.Up : Direction.Down);
    }

    public void Attack()
    {
        if (Action == PlayerAction.Damaged) return;
        Action = PlayerAction.Attacking;
        actionTimeRemaining = 0.28;
    }

    public void TakeDamage()
    {
        Health = Math.Max(0, Health - 1);
        Action = PlayerAction.Damaged;
        actionTimeRemaining = 0.45;
    }

    public void SelectItem(int slot) => SelectedItem = Math.Clamp(slot, 1, 3);

    public void Update(GameTime gameTime)
    {
        double seconds = gameTime.ElapsedGameTime.TotalSeconds;
        animationTime += seconds;
        if (actionTimeRemaining > 0)
        {
            actionTimeRemaining -= seconds;
            if (actionTimeRemaining <= 0) Action = PlayerAction.Idle;
        }

        if (Action is not PlayerAction.Attacking and not PlayerAction.Damaged && pendingDirection != Vector2.Zero)
        {
            pendingDirection.Normalize();
            Position += pendingDirection * Speed * (float)seconds;
            Position = new Vector2(
                MathHelper.Clamp(Position.X, bounds.Left, bounds.Right - 28),
                MathHelper.Clamp(Position.Y, bounds.Top, bounds.Bottom - 34));
            Action = PlayerAction.Walking;
        }
        else if (actionTimeRemaining <= 0)
        {
            Action = PlayerAction.Idle;
        }

        pendingDirection = Vector2.Zero;
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        bool alternate = ((int)(animationTime * 8) % 2) == 1;
        if (Action == PlayerAction.Damaged && alternate) return; // Damage flash.
        sprite.Draw(spriteBatch, Position, Facing, alternate && Action != PlayerAction.Idle);
    }

    public void Reset()
    {
        Position = startPosition;
        Facing = Direction.Down;
        Action = PlayerAction.Idle;
        Health = 5;
        SelectedItem = 1;
        actionTimeRemaining = 0;
        animationTime = 0;
    }
}
