using System.Text.Json.Nodes;
using GameProject.Core;
using GameProject.Objects;
using GameProject.Rooms.Definitions;
using GameProject.Rooms.Loading;
using GameProject.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Rooms.Tests;

if (args.Length > 0 && (args[0] == "--preview" || args[0] == "--capture"))
{
    using RoomPreview preview = new(args[0] == "--capture" ? args[1] : null);
    preview.Run();
    return;
}

int checks = 0;
void Check(bool condition, string name)
{
    if (!condition) throw new InvalidOperationException(name);
    checks++;
    Console.WriteLine("PASS: " + name);
}
void Reject(Action action, string message)
{
    try { action(); }
    catch (InvalidDataException error)
    {
        Check(error.Message.Contains(message, StringComparison.OrdinalIgnoreCase), $"Rejected data with diagnostic '{message}'");
        return;
    }
    throw new InvalidOperationException("Accepted invalid data: " + message);
}

string directory = Path.Combine(AppContext.BaseDirectory, "Content", "Rooms");
IReadOnlyList<RoomDefinition> definitions = RoomLoader.LoadDirectory(directory);
Check(definitions.Count == 6, "Five dungeon rooms and one laboratory load from external files");
Check(definitions[0].Id == "entrance-hall" && definitions[^1].Id == "all-object-lab", "Stable file order");
Check(definitions.Select(room => room.Name).Distinct().Count() == 6, "Distinct room names");
RoomDefinition lab = definitions.Single(room => room.Id == "all-object-lab");
HashSet<string> expected = ["Player"];
expected.UnionWith(Enum.GetNames<BlockKind>().Select(kind => "Block." + kind));
expected.UnionWith(Enum.GetNames<ItemKind>().Select(kind => "Item." + kind));
expected.UnionWith(Enum.GetNames<EnemyKind>().Select(kind => "Enemy." + kind));
Check(expected.Count == 23 && expected.SetEquals(lab.Objects.Select(obj => obj.Type)), "Lab covers player and all 22 roster kinds");
Check(expected.SetEquals(definitions.Take(5).SelectMany(room => room.Objects).Select(obj => obj.Type)), "Five dungeon rooms use the entire roster");
Check(lab.Objects.Single(obj => obj.Id == "block-Stone").Properties["purpose"] == "solid-collision-test", "Optional properties survive parsing");

HashSet<string> visited = [];
Queue<string> pending = new();
pending.Enqueue(definitions[0].Id);
while (pending.Count > 0)
{
    string id = pending.Dequeue();
    if (!visited.Add(id)) continue;
    foreach (RoomLink link in definitions.Single(room => room.Id == id).Links) pending.Enqueue(link.TargetRoomId);
}
Check(visited.Count == 6, "All six rooms are reachable through external links");
foreach (RoomDefinition definition in definitions)
{
    Check(definition.Links.All(link => definitions.Single(room => room.Id == link.TargetRoomId).Links.Any(back => back.TargetRoomId == definition.Id)), definition.Id + " links have return routes");
    List<RecordingSprite> sprites = [];
    RoomObjectFactory factory = new(definition.Bounds.ToRectangle(), _ =>
    {
        RecordingSprite sprite = new();
        sprites.Add(sprite);
        return sprite;
    });
    LoadedRoom room = new(definition, factory);
    Check(room.Objects.Count == definition.Objects.Count, definition.Id + " creates every real object");
    Player player = (Player)room.Objects["player"];
    Check(player.Position == definition.Entries["start"].ToVector2(), definition.Id + " player starts at entry");
    Check(player.TryFireArrow() && player.TryThrowBoomerang() && player.TryPlaceBomb(), definition.Id + " all three player items available");
    room.Update(new GameTime(TimeSpan.Zero, TimeSpan.FromSeconds(0.2)));
    room.Draw(null!);
    Check(sprites.Count(sprite => sprite.Draws > 0) >= definition.Objects.Count, definition.Id + " all room objects draw");
    var firstDraw = sprites.Select(sprite => (sprite.Position, sprite.Alternate)).ToArray();
    room.Draw(null!);
    Check(firstDraw.SequenceEqual(sprites.Select(sprite => (sprite.Position, sprite.Alternate))), definition.Id + " drawing does not advance simulation");
    room.Reset();
    room.Reset();
    Check(!player.HasActiveArrow && !player.HasActiveBoomerang && !player.HasActiveBomb && player.Position == definition.Entries["start"].ToVector2(), definition.Id + " deterministic reset clears item effects");
}

RecordingFactory recording = new();
LoadedRoom active = new(lab, recording);
RecordingFactory inactiveFactory = new();
LoadedRoom inactive = new(lab, inactiveFactory);
active.Update(new GameTime(TimeSpan.Zero, TimeSpan.FromSeconds(0.25)));
active.Draw(null!);
active.Reset();
Check(recording.Objects.All(obj => obj.Updates == 1 && obj.Draws == 1 && obj.Resets == 1 && obj.Seconds == 0.25), "Active room visits every object exactly once and forwards elapsed time");
Check(inactiveFactory.Objects.All(obj => obj.Updates == 0 && obj.Draws == 0 && obj.Resets == 0), "Inactive room does not update or draw");

string validJson = File.ReadAllText(Path.Combine(directory, "01-entrance-hall.json"));
void Invalid(Action<JsonNode> edit, string expectedMessage)
{
    JsonNode root = JsonNode.Parse(validJson)!;
    edit(root);
    Reject(() => RoomLoader.Parse(root.ToJsonString(), "bad-room.json"), expectedMessage);
}
Reject(() => RoomLoader.Parse("{"), "malformed room JSON");
Reject(() => RoomLoader.Parse("null"), "not null");
Reject(() => RoomLoader.Parse("[]"), "malformed room JSON");
Reject(() => RoomLoader.Parse(validJson.Replace("\"id\": \"entrance-hall\"", "\"id\": \"other\", \"id\": \"entrance-hall\"", StringComparison.Ordinal)), "Duplicate JSON property");
Invalid(root => root.AsObject().Remove("id"), "malformed room JSON");
Invalid(root => root["id"] = " ", "id must not be empty");
Invalid(root => root["nmae"] = "Typo", "malformed room JSON");
Invalid(root => root["objects"]![0]!["type"] = "Enemy.999", "unknown type");
Invalid(root => root["objects"]![0]!["type"] = "player", "unknown type");
Invalid(root => root["objects"]![1]!["id"] = "player", "duplicate object id");
Invalid(root => root["objects"]![0]!["position"]!["x"] = -1, "position");
Invalid(root => root["objects"]![0]!["position"]!["x"] = 903, "footprint");
Invalid(root => root["objects"]![1]!["position"]!["x"] = 870, "52x60 footprint");
Invalid(root => root["objects"]![4]!["position"]!["y"] = 470, "38x42 footprint");
Invalid(root => root["objects"]![0]!["position"]!["x"] = int.MaxValue, "position");
Invalid(root => root["objects"]![0]!["position"]!["x"] = 0.5, "malformed room JSON");
Invalid(root => root["objects"]![0]!["position"] = null, "position");
Invalid(root => root["objects"]![0]!["properties"] = null, "properties");
Invalid(root => root["objects"]![0]!["properties"] = new JsonObject { ["bad"] = null }, "property 'bad'");
Invalid(root => root["objects"]![0] = null, "null object");
Invalid(root => root["objects"] = null, "objects and links");
Invalid(root => root["objects"]!.AsArray().RemoveAt(0), "exactly one Player");
Invalid(root => root["objects"]![1]!["type"] = "Player", "exactly one Player");
Invalid(root => root["bounds"]!["width"] = 0, "invalid rectangle");
Invalid(root => root["bounds"]!["width"] = int.MaxValue, "invalid rectangle");
Invalid(root => root["bounds"] = null, "invalid rectangle");
Invalid(root => root["entries"]!.AsObject().Remove("start"), "entries must include");
Invalid(root => root["entries"]!["start"]!["y"] = 501, "entry 'start'");
Invalid(root => root["links"]![0]!["direction"] = "Diagonal", "invalid direction");
Invalid(root => root["links"]![0]!["trigger"]!["x"] = 0, "outside room bounds");
Invalid(root => root["links"]![1]!["id"] = "to-water", "duplicate link id");
Invalid(root => root["links"]![0] = null, "null link");
Invalid(root => root["links"]![0]!["targetRoomId"] = " ", "target room");

string temp = Path.Combine(Path.GetTempPath(), "rooms-tests-" + Guid.NewGuid().ToString("N"));
Directory.CreateDirectory(temp);
try
{
    Reject(() => RoomLoader.LoadDirectory(temp), "no room JSON");
    File.WriteAllText(Path.Combine(temp, "a.json"), validJson);
    File.WriteAllText(Path.Combine(temp, "b.json"), validJson);
    Reject(() => RoomLoader.LoadDirectory(temp), "duplicate room id");
    File.Delete(Path.Combine(temp, "b.json"));
    Reject(() => RoomLoader.LoadDirectory(temp), "unknown target room");
    foreach (string path in Directory.GetFiles(directory, "*.json")) File.Copy(path, Path.Combine(temp, Path.GetFileName(path)));
    File.Delete(Path.Combine(temp, "a.json"));
    JsonNode badLink = JsonNode.Parse(validJson)!;
    badLink["links"]![0]!["targetEntryId"] = "missing-entry";
    File.WriteAllText(Path.Combine(temp, "01-entrance-hall.json"), badLink.ToJsonString());
    Reject(() => RoomLoader.LoadDirectory(temp), "unknown entry");
}
finally { Directory.Delete(temp, true); }
Check(definitions.Select(room => room.Id).SequenceEqual(RoomLoader.LoadDirectory(directory).Select(room => room.Id)), "Repeated loading is deterministic");
Console.WriteLine($"Passed {checks} room checks without a graphics device.");

internal sealed class RecordingSprite : ISprite
{
    public int Draws { get; private set; }
    public Vector2 Position { get; private set; }
    public bool Alternate { get; private set; }
    public void Draw(SpriteBatch spriteBatch, Vector2 position, Direction direction, bool alternateFrame)
    {
        Draws++;
        Position = position;
        Alternate = alternateFrame;
    }
}

internal sealed class RecordingFactory : IRoomObjectFactory
{
    public List<RecordingObject> Objects { get; } = [];
    public IGameObject Create(RoomObjectDefinition definition)
    {
        RecordingObject obj = new();
        Objects.Add(obj);
        return obj;
    }
}

internal sealed class RecordingObject : IGameObject
{
    public string Name => "Recording object";
    public int Updates { get; private set; }
    public int Draws { get; private set; }
    public int Resets { get; private set; }
    public double Seconds { get; private set; }
    public void Update(GameTime gameTime) { Updates++; Seconds = gameTime.ElapsedGameTime.TotalSeconds; }
    public void Draw(SpriteBatch spriteBatch) => Draws++;
    public void Reset() => Resets++;
}
