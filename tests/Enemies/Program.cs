using GameProject.Core;
using GameProject.Objects.Enemies;
using GameProject.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

var sprite = new RecordingSprite();
var start = new Vector2(700, 420);
EnemyCharacter[] MakeAt(Vector2 position) => [new Octorok("Octorok", position, sprite, sprite),
    new Keese("Keese", position, sprite), new Gel("Gel", position, sprite), new Npc("NPC", position, sprite)];
EnemyCharacter[] Make() => MakeAt(start);
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

var playAreaStart = new Vector2(370, 450);
var previews = Make();
var playAreaEnemies = MakeAt(playAreaStart);
var pairs = new ObjectGallery<EnemyDisplayPair>(previews.Select((preview, index) =>
    new EnemyDisplayPair(preview, playAreaEnemies[index])).ToArray());
void CheckPair(int index)
{
    Require(pairs.Index == index && pairs.Current.Name == previews[index].Name, "Paired selection");
    for (int i = 0; i < 1200; i++)
    {
        pairs.Current.Update(Step(0.1));
        var preview = previews[index];
        var main = playAreaEnemies[index];
        Require(Vector2.Distance(preview.Position - start, main.Position - playAreaStart) < 0.001f,
            "Paired movement diverged");
        Require(preview.Facing == main.Facing && preview.AlternateFrame == main.AlternateFrame,
            "Paired animation diverged");
        int spriteHeight = main switch { Keese => 20, Gel => 24, Npc => 42, _ => 32 };
        Require(main.Position.X >= 294 && main.Position.X + 42 <= 592 &&
            main.Position.Y >= 396 && main.Position.Y + spriteHeight + 1 <= 510,
            "Main enemy outside lower-right play area");
        if (preview is Octorok previewShooter && main is Octorok mainShooter)
        {
            Require(previewShooter.Projectile.IsActive == mainShooter.Projectile.IsActive, "Paired shot timing");
            if (mainShooter.Projectile.IsActive)
            {
                Require(Vector2.Distance(previewShooter.Projectile.Position - start,
                    mainShooter.Projectile.Position - playAreaStart) < 0.001f, "Paired shot movement");
                Require(mainShooter.Projectile.Position.X >= 16 &&
                    mainShooter.Projectile.Position.X + 32 <= 592, "Main shot outside play area");
            }
        }
        sprite.DrawCount = 0;
        pairs.Current.Draw(null!);
        int expectedDraws = preview is Octorok o && o.Projectile.IsActive ? 4 : 2;
        Require(sprite.DrawCount == expectedDraws, "Both enemies and active shots must draw");
    }
}
for (int i = 0; i < 4; i++)
{
    CheckPair(i);
    pairs.Next();
}
Require(pairs.Index == 0, "Paired next wrap");
pairs.Previous();
CheckPair(3);
pairs.Previous();
CheckPair(2);
pairs.Reset();
Require(pairs.Index == 0, "Paired reset selection");
Require(previews.All(c => c.Position == start && !c.AlternateFrame) &&
    playAreaEnemies.All(c => c.Position == playAreaStart && !c.AlternateFrame), "Paired reset positions and animation");
Require(!((Octorok)previews[0]).Projectile.IsActive &&
    !((Octorok)playAreaEnemies[0]).Projectile.IsActive, "Paired reset shots");
Console.WriteLine("PASS: paired selection, movement, animation, projectiles, drawing, play-area bounds and reset.");

sealed class RecordingSprite : ISprite
{
    public int DrawCount { get; set; }
    public void Draw(SpriteBatch batch, Vector2 position, Direction direction, bool alternateFrame) => DrawCount++;
}
