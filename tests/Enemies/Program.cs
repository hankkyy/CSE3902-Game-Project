using GameProject.Core;
using GameProject.Objects.Enemies;
using GameProject.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

var sprite = new RecordingSprite();
var start = new Vector2(700, 420);
EnemyCharacter[] Make() => [new Octorok("Octorok", start, sprite, sprite),
    new Keese("Keese", start, sprite), new Gel("Gel", start, sprite), new Npc("NPC", start, sprite)];
void Require(bool condition, string message)
{
    if (!condition) throw new Exception(message);
}
GameTime Step(double seconds) => new(TimeSpan.Zero, TimeSpan.FromSeconds(seconds));
var characters = Make();
foreach (var character in characters)
{
    for (int i = 0; i < 12000; i++)
    {
        character.Update(Step(0.01));
        Require(character.Position.X >= 645 && character.Position.X <= 755 &&
            character.Position.Y >= 396 && character.Position.Y <= 438, "Movement bounds");
        if (character is Npc) Require(character.Position == start, "NPC moved");
        if (character is Octorok o && o.Projectile.IsActive)
            Require(o.Projectile.Position.X >= 624 && o.Projectile.Position.X <= 884, "Shot bounds");
    }
    character.Reset();
    Require(character.Position == start && !character.AlternateFrame &&
        character.Facing == Direction.Right, "Reset");
    character.Update(Step(0.2));
    Require(character.AlternateFrame, "Animation");
    var before = character.Position;
    character.Draw(null!);
    Require(character.Position == before, "Draw mutated position");
}
var small = Make();
var large = Make();
for (int n = 0; n < small.Length; n++)
{
    for (int i = 0; i < 425; i++) small[n].Update(Step(0.01));
    large[n].Update(Step(4.25));
    Require(Vector2.Distance(small[n].Position, large[n].Position) < 0.001f, "Frame dependence");
}
var shooter = (Octorok)large[0];
Require(shooter.Projectile.IsActive, "Missing shot");
Require(Vector2.Distance(shooter.Projectile.Position, ((Octorok)small[0]).Projectile.Position) < 0.001f, "Shot frame dependence");
shooter.Reset();
Require(!shooter.Projectile.IsActive, "Reset kept shot");
shooter.Update(Step(1.99));
Require(!shooter.Projectile.IsActive, "Shot before cooldown");
shooter.Update(Step(0.01));
Require(shooter.Projectile.IsActive, "Shot missing at cooldown");
var gallery = new ObjectGallery<EnemyCharacter>(characters);
gallery.Previous();
Require(gallery.Index == 3, "Previous wrap");
gallery.Next();
Require(gallery.Index == 0, "Next wrap");
for (int i = 0; i < 4; i++) gallery.Next();
Require(gallery.Index == 0, "Full cycle");
gallery.Previous();
gallery.Reset();
Require(gallery.Index == 0 && characters.All(c => c.Position == start), "Gallery reset");
Console.WriteLine("PASS: bounds, projectile, frame independence, animation, reset, draw and gallery wraparound.");

sealed class RecordingSprite : ISprite
{
    public void Draw(SpriteBatch batch, Vector2 position, Direction direction, bool alternateFrame) { }
}

