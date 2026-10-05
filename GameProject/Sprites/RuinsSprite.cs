using GameProject.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameProject.Sprites;

internal enum RuinsVisual
{
    CrystalPillar, RuneTile, Bow, Boomerang, Map, Compass, StarShard,
    RuneWisp, ClockworkBeetle, PrismSentinel, LightBolt
}

/// <summary>Original small pixel motifs for the Starlight Ruins, drawn from the existing pixel texture.</summary>
internal sealed class RuinsSprite : ISprite
{
    private readonly Texture2D pixel;
    private readonly string[] pattern;
    private readonly Color body;
    private readonly Color accent;
    private readonly int scale;

    public RuinsSprite(Texture2D pixel, RuinsVisual visual)
    {
        this.pixel = pixel;
        (string[] Pattern, Color Body, Color Accent, int Scale) definition = visual switch
        {
            RuinsVisual.CrystalPillar =>
                (["...+....", "..+#+...", ".+###+..", ".+###+..", ".+###+..", "..+#+...", ".#####..", "########"], Color.MediumPurple, Color.LightCyan, 6),
            RuinsVisual.RuneTile =>
                (["########", "#......#", "#..++..#", "#.+..+.#", "#.+..+.#", "#..++..#", "#......#", "########"], Color.SlateGray, Color.Turquoise, 6),
            RuinsVisual.Bow =>
                (["..+#....", "..+.#...", "..+..#..", "..+..#..", "..+..#..", "..+.#...", "..+#....", "........"], Color.SaddleBrown, Color.Gold, 3),
            RuinsVisual.Boomerang =>
                (["........", ".####...", ".#+++#..", ".#+.....", ".#+.....", "..#.....", "........", "........"], Color.SaddleBrown, Color.Gold, 3),
            RuinsVisual.Map =>
                ([".######.", "#+....+#", "#..+...#", "#..++..#", "#...+..#", "#....+.#", "#.....+#", ".######."], Color.Wheat, Color.Sienna, 3),
            RuinsVisual.Compass =>
                (["..####..", ".#....#.", "#..+...#", "#.+++..#", "#..+++.#", "#...+..#", ".#....#.", "..####.."], Color.Goldenrod, Color.LightCyan, 3),
            RuinsVisual.StarShard =>
                (["...*....", "...+....", "..+++...", "*+++++*.", "..+++...", "...+....", "...*....", "........"], Color.Gold, Color.LightCyan, 3),
            RuinsVisual.RuneWisp =>
                (["..*..*..", "..++++..", ".+####+.", ".+#++#+.", ".+####+.", "..++++..", "...++...", "..*..*.."], Color.Teal, Color.LightCyan, 4),
            RuinsVisual.ClockworkBeetle =>
                ([".+....+.", "..####..", "+#++++#+", ".#+##+#.", "+#+##+#+", ".#++++#.", "..####..", ".+....+."], Color.SaddleBrown, Color.Gold, 4),
            RuinsVisual.PrismSentinel =>
                (["..++++..", ".+####+.", "+##**##+", "+######+", ".+####+.", "..++++..", ".######.", "########"], Color.DarkSlateBlue, Color.MediumPurple, 4),
            _ =>
                (["........", "...+....", "..+++...", ".+++++..", "..+++...", "...+....", "........", "........"], Color.White, Color.LightCyan, 1)
        };
        (pattern, body, accent, scale) = definition;
    }

    public void Draw(SpriteBatch batch, Vector2 position, Direction direction, bool alternateFrame)
    {
        for (int row = 0; row < pattern.Length; row++)
        {
            for (int column = 0; column < pattern[row].Length; column++)
            {
                char mark = pattern[row][column];
                if (mark == '.' || (mark == '*' && !alternateFrame)) continue;
                Color color = mark == '#' ? body : alternateFrame ? Color.White : accent;
                int visualColumn = direction == Direction.Left ? pattern[row].Length - 1 - column : column;
                batch.Draw(pixel, new Rectangle((int)position.X + visualColumn * scale,
                    (int)position.Y + row * scale, scale, scale), color);
            }
        }
    }
}
