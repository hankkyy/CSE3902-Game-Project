using GameProject.Core;
using GameProject.Objects.Blocks;
using GameProject.Objects.Items;
using GameProject.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

List<string> failures = [];
Vector2 start = new(100, 80);

VerifyStationaryBlocks();
VerifyAnimatedItems();
VerifyGalleryWraparound();

if (failures.Count > 0)
{
    foreach (string failure in failures)
    {
        Console.Error.WriteLine($"FAIL: {failure}");
    }

    return 1;
}

Console.WriteLine("PASS: dedicated blocks, item animation, reset, and gallery wraparound.");
return 0;

void VerifyStationaryBlocks()
{
    (IGameObject Block, RecordingSprite Sprite)[] blocks =
    [
        CreateObject(sprite => new StoneBlock(start, sprite)),
        CreateObject(sprite => new PushBlock(start, sprite)),
        CreateObject(sprite => new WaterTile(start, sprite)),
        CreateObject(sprite => new StatueBlock(start, sprite))
    ];

    foreach ((IGameObject block, RecordingSprite sprite) in blocks)
    {
        block.Update(Frame(3));
        block.Draw(null!);
        Check(sprite.Position == start, $"{block.Name} moved even though blocks must remain stationary");
        Check(!sprite.AlternateFrame, $"{block.Name} unexpectedly animated");
    }
}

void VerifyAnimatedItems()
{
    (IGameObject Item, RecordingSprite Sprite)[] items =
    [
        CreateObject(sprite => new HeartItem(start, sprite)),
        CreateObject(sprite => new RupeeItem(start, sprite)),
        CreateObject(sprite => new KeyItem(start, sprite)),
        CreateObject(sprite => new BombItem(start, sprite))
    ];

    foreach ((IGameObject item, RecordingSprite sprite) in items)
    {
        item.Update(Frame(0.25));
        item.Draw(null!);
        Check(sprite.Position != start, $"{item.Name} did not move from its initial position");

        item.Reset();
        item.Draw(null!);
        Check(sprite.Position == start, $"{item.Name} reset did not restore its animation origin");
        Check(!sprite.AlternateFrame, $"{item.Name} reset did not restore its first frame");
    }
}

void VerifyGalleryWraparound()
{
    ObjectGallery<IGameObject> gallery = new(
    [
        new StoneBlock(start, new RecordingSprite()),
        new PushBlock(start, new RecordingSprite()),
        new WaterTile(start, new RecordingSprite()),
        new StatueBlock(start, new RecordingSprite())
    ]);

    gallery.Previous();
    Check(gallery.Index == 3, "Previous did not wrap to the last block");
    gallery.Next();
    Check(gallery.Index == 0, "Next did not wrap to the first block");
    gallery.Next();
    gallery.Reset();
    Check(gallery.Index == 0, "Reset did not restore the first gallery selection");
}

(IGameObject, RecordingSprite) CreateObject(Func<RecordingSprite, IGameObject> create)
{
    RecordingSprite sprite = new();
    return (create(sprite), sprite);
}

GameTime Frame(double seconds) => new(TimeSpan.Zero, TimeSpan.FromSeconds(seconds));

void Check(bool condition, string message)
{
    if (!condition)
    {
        failures.Add(message);
    }
}

sealed class RecordingSprite : ISprite
{
    public Vector2 Position { get; private set; }
    public bool AlternateFrame { get; private set; }

    public void Draw(SpriteBatch spriteBatch, Vector2 position, Direction direction, bool alternateFrame)
    {
        Position = position;
        AlternateFrame = alternateFrame;
    }
}
