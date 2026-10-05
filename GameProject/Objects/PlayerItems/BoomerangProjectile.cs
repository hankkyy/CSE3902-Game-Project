using GameProject.Core;
using GameProject.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameProject.Objects.PlayerItems;

/// <summary>Flies outward, then returns to the owner's current position without object collisions.</summary>
public sealed class BoomerangProjectile(Rectangle bounds, ISprite sprite, Func<Vector2> returnPosition) : IGameObject
{
    public const int Size = 16;
    private const float OutboundSpeed = 240f;
    private const float ReturnSpeed = 340f;
    private const float Range = 140f;
    private const double MaximumLifetime = 4;
    private Vector2 origin;
    private Vector2 velocity;
    private double outboundDuration;
    private double outboundAge;
    private double age;

    public string Name => "Player boomerang";
    public Vector2 Position { get; private set; }
    public Direction Facing { get; private set; } = Direction.Down;
    public bool IsActive { get; private set; }
    public bool IsReturning { get; private set; }

    public bool TryLaunch(Vector2 position, Direction direction)
    {
        if (IsActive || position.X < bounds.Left || position.Y < bounds.Top ||
            position.X + Size > bounds.Right || position.Y + Size > bounds.Bottom) return false;

        origin = position;
        Position = position;
        Facing = direction;
        velocity = direction switch
        {
            Direction.Up => new Vector2(0, -OutboundSpeed),
            Direction.Down => new Vector2(0, OutboundSpeed),
            Direction.Left => new Vector2(-OutboundSpeed, 0),
            _ => new Vector2(OutboundSpeed, 0)
        };
        float distanceToEdge = direction switch
        {
            Direction.Up => position.Y - bounds.Top,
            Direction.Down => bounds.Bottom - Size - position.Y,
            Direction.Left => position.X - bounds.Left,
            _ => bounds.Right - Size - position.X
        };
        outboundDuration = Math.Min(Range, distanceToEdge) / (double)OutboundSpeed;
        outboundAge = 0;
        age = 0;
        IsReturning = false;
        IsActive = true;
        return true;
    }

    public void Update(GameTime gameTime)
    {
        if (!IsActive) return;

        double seconds = Math.Max(0, gameTime.ElapsedGameTime.TotalSeconds);
        age += seconds;
        if (age >= MaximumLifetime)
        {
            IsActive = false;
            return;
        }

        if (!IsReturning)
        {
            double outwardSeconds = Math.Min(seconds, outboundDuration - outboundAge);
            outboundAge += outwardSeconds;
            Position = origin + velocity * (float)outboundAge;
            seconds -= outwardSeconds;
            if (outboundAge < outboundDuration) return;
            IsReturning = true;
        }

        // Consume the remainder of this frame, including frames that cross the turn point.
        Vector2 target = returnPosition();
        target = new Vector2(MathHelper.Clamp(target.X, bounds.Left, bounds.Right - Size),
            MathHelper.Clamp(target.Y, bounds.Top, bounds.Bottom - Size));
        Vector2 displacement = target - Position;
        float distance = displacement.Length();
        float travel = ReturnSpeed * (float)seconds;
        if (distance <= travel || distance < 0.001f)
        {
            Position = target;
            IsActive = false;
            return;
        }

        Position += displacement / distance * travel;
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        if (IsActive) sprite.Draw(spriteBatch, Position, Facing, ((int)(age * 12) % 2) == 1);
    }

    public void Reset()
    {
        IsActive = false;
        IsReturning = false;
        Position = Vector2.Zero;
        Facing = Direction.Down;
        origin = Vector2.Zero;
        velocity = Vector2.Zero;
        outboundDuration = 0;
        outboundAge = 0;
        age = 0;
    }
}
