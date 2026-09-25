using GameProject.Core;
using GameProject.Objects;
using GameProject.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

static void Check(bool condition, string description)
{
    if (!condition) throw new InvalidOperationException(description);
}

static GameTime Step(double seconds) => new(TimeSpan.Zero, TimeSpan.FromSeconds(seconds));

Vector2 start = new(700, 270);
foreach (BlockKind kind in Enum.GetValues<BlockKind>())
{
    RecordingSprite sprite = new();
    BlockObject block = new(kind.ToString(), start, kind, sprite);
    block.Update(Step(500));
    block.Draw(null!);
    Check(sprite.Position == start && !sprite.Alternate, $"{kind} must remain stationary");
    block.Reset();
    block.Draw(null!);
    Check(sprite.Position == start, $"{kind} reset position");
}

List<ItemObject> items = [];
List<RecordingSprite> sprites = [];
foreach (ItemKind kind in Enum.GetValues<ItemKind>())
{
    RecordingSprite sprite = new();
    ItemObject item = new(kind.ToString(), start, kind, sprite);
    items.Add(item);
    sprites.Add(sprite);
    item.Draw(null!);
    Check(!sprite.Alternate, $"{kind} initial frame");
    item.Update(Step(0.2));
    item.Draw(null!);
    Check(sprite.Alternate && sprite.Position == start, $"{kind} animated in place");
    item.Draw(null!);
    Check(sprite.Alternate, "Drawing must not advance animation");
    item.Reset();
    item.Reset();
    item.Draw(null!);
    Check(!sprite.Alternate, $"{kind} repeatable reset");
    for (int i = 0; i < 20; i++) item.Update(Step(0.01));
    item.Draw(null!);
    Check(sprite.Alternate, "Animation must depend on elapsed time, not update count");
}

ObjectGallery<ItemObject> gallery = new(items);
gallery.Previous();
Check(gallery.Index == 3, "Previous wraps to last");
gallery.Next();
Check(gallery.Index == 0, "Next wraps to first");
for (int i = 0; i < 4; i++) gallery.Next();
Check(gallery.Index == 0, "Complete cycle");
gallery.Next();
gallery.Reset();
Check(gallery.Index == 0, "Reset selection");
for (int i = 0; i < items.Count; i++)
{
    items[i].Draw(null!);
    Check(!sprites[i].Alternate, "Reset includes hidden items");
}
ObjectGallery<BlockObject> blocks = new(Enum.GetValues<BlockKind>()
    .Select(kind => new BlockObject(kind.ToString(), start, kind, new RecordingSprite())).ToArray());
blocks.Previous();
Check(blocks.Index == 3, "Block previous wraps");
blocks.Next();
Check(blocks.Index == 0, "Block next wraps");
blocks.Next();
blocks.Reset();
Check(blocks.Index == 0, "Block selection resets");
Console.WriteLine("Passed: all eight objects, animation timing, pure drawing, gallery wraparound, hidden-item reset.");

sealed class RecordingSprite : ISprite
{
    public Vector2 Position { get; private set; }
    public bool Alternate { get; private set; }
    public void Draw(SpriteBatch spriteBatch, Vector2 position, Direction direction, bool alternateFrame)
    {
        Position = position;
        Alternate = alternateFrame;
    }
}
