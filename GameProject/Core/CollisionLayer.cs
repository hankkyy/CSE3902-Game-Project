namespace GameProject.Core;

/// <summary>Identifies an object's role when collision responses are selected.</summary>
public enum CollisionLayer
{
    Player,
    Solid,
    Item,
    Enemy,
    PlayerAttack,
    PlayerProjectile,
    EnemyProjectile,
    RoomExit
}
