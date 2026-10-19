using GameProject.Core;
using GameProject.Objects;
using GameProject.Rooms.Definitions;
using GameProject.Sprites;
using Microsoft.Xna.Framework;

namespace GameProject.Rooms.Loading;

/// <summary>Maps validated data to existing objects; sprite injection keeps tests headless.</summary>
public sealed class RoomObjectFactory : IRoomObjectFactory
{
    private readonly Rectangle bounds;
    private readonly Func<string, ISprite> createSprite;

    public RoomObjectFactory(Rectangle bounds, SpriteFactory sprites)
        : this(bounds, SpriteProvider(sprites)) { }

    public RoomObjectFactory(Rectangle bounds, Func<string, ISprite> createSprite)
    {
        ArgumentNullException.ThrowIfNull(createSprite);
        this.bounds = bounds;
        this.createSprite = createSprite;
    }

    public IGameObject Create(RoomObjectDefinition definition)
    {
        ArgumentNullException.ThrowIfNull(definition);
        string type = definition.Type;
        if (!RoomObjectTypes.IsKnown(type)) throw new InvalidDataException($"Unknown room object type '{type}'.");
        Vector2 position = definition.Position.ToVector2();
        ISprite sprite = createSprite(type);
        if (type == "Player")
            return new Player(position, bounds, sprite, createSprite("Arrow"), createSprite("Boomerang"),
                createSprite("Bomb"), createSprite("BombExplosion"));
        if (type.StartsWith("Block.", StringComparison.Ordinal))
            return new BlockObject(definition.Name, position, Enum.Parse<BlockKind>(type[6..]), sprite);
        if (type.StartsWith("Item.", StringComparison.Ordinal))
            return new ItemObject(definition.Name, position, Enum.Parse<ItemKind>(type[5..]), sprite);

        EnemyKind kind = Enum.Parse<EnemyKind>(type[6..]);
        ISprite? projectile = null;
        if (kind == EnemyKind.Octorok) projectile = createSprite("Item.Bomb");
        else if (kind == EnemyKind.PrismSentinel) projectile = createSprite("LightBolt");
        return new EnemyObject(definition.Name, position, kind, sprite, projectile);
    }

    private static Func<string, ISprite> SpriteProvider(SpriteFactory sprites)
    {
        ArgumentNullException.ThrowIfNull(sprites);
        return type =>
        {
            if (type == "Player") return sprites.CreatePlayerSprite();
            if (type == "Arrow") return sprites.CreateArrowSprite();
            if (type == "Boomerang") return sprites.CreateBoomerangSprite();
            if (type == "Bomb") return sprites.CreateBombSprite();
            if (type == "BombExplosion") return sprites.CreateBombExplosionSprite();
            if (type == "LightBolt") return sprites.CreateLightBoltSprite();
            if (type.StartsWith("Block.", StringComparison.Ordinal)) return sprites.CreateBlockSprite(Enum.Parse<BlockKind>(type[6..]));
            if (type.StartsWith("Item.", StringComparison.Ordinal)) return sprites.CreateItemSprite(Enum.Parse<ItemKind>(type[5..]));
            return sprites.CreateEnemySprite(Enum.Parse<EnemyKind>(type[6..]));
        };
    }
}
