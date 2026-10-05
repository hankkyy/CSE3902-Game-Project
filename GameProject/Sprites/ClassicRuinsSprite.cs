using GameProject.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameProject.Sprites;

internal enum ClassicVisual { Stone, Push, Water, Statue, Heart, Rupee, Key, Bomb, Octorok, Keese, Gel, Keeper }

/// <summary>Small original motifs for the existing gallery objects, using their original drawing bounds.</summary>
internal sealed class ClassicRuinsSprite : ISprite
{
    private sealed record Motif(string[] Rest, string[]? Alternate, Color Body, Color Highlight, Point Size);
    private static readonly Color Outline = new(25, 36, 45);
    private readonly Texture2D pixel;
    private readonly Motif motif;

    public ClassicRuinsSprite(Texture2D pixel, ClassicVisual visual)
    {
        this.pixel = pixel;
        motif = visual switch
        {
            ClassicVisual.Stone => new(["########", "#+##+###", "########", "oooo#ooo", "####o###", "#+##o#+#", "####o###", "oooooooo"],
                null, Color.SlateGray, Color.LightSlateGray, new Point(52, 52)),
            ClassicVisual.Push => new(["oooooooo", "o######o", "o+####+o", "o#+##+#o", "o##++##o", "o#+##+#o", "o+####+o", "oooooooo"],
                null, Color.Sienna, Color.Tan, new Point(52, 52)),
            ClassicVisual.Water => new(["########", "#+####+#", "##++++##", "########", "++++####", "####++++", "########", "##++++##"],
                null, Color.SteelBlue, Color.LightCyan, new Point(52, 52)),
            ClassicVisual.Statue => new(["..oooo..", ".o####o.", ".o+oo+o.", ".o####o.", "..o##o..", ".o####o.", "o######o", "oooooooo"],
                null, Color.SlateGray, Color.LightCyan, new Point(52, 60)),
            ClassicVisual.Heart => new(["........", ".oo..oo.", "o++oo##o", "o+#####o", ".o####o.", "..o##o..", "...oo...", "........"],
                ["........", ".oo..oo.", "o##oo++o", "o#####+o", ".o####o.", "..o##o..", "...oo...", "........"], Color.Crimson, Color.Pink, new Point(24, 24)),
            ClassicVisual.Rupee => new(["...oo...", "..o++o..", ".o+###o.", ".o+###o.", ".o+###o.", ".o####o.", "..o##o..", "...oo..."],
                ["...oo...", "..o##o..", ".o###+o.", ".o###+o.", ".o###+o.", ".o####o.", "..o++o..", "...oo..."], Color.SeaGreen, Color.PaleGreen, new Point(18, 30)),
            ClassicVisual.Key => new(["..oooo..", ".o++++o.", ".o+oo+o.", "..o++o..", "...##...", "...##oo.", "...##oo.", "...oo..."],
                ["..oooo..", ".o####o.", ".o#oo#o.", "..o##o..", "...++...", "...++oo.", "...++oo.", "...oo..."], Color.Goldenrod, Color.LightYellow, new Point(16, 34)),
            ClassicVisual.Bomb => new([".....+..", "....+...", "...oo...", "..o##o..", ".o+###o.", ".o####o.", ".o####o.", "..oooo.."],
                ["....+...", ".....+..", "...oo...", "..o##o..", ".o+###o.", ".o####o.", ".o####o.", "..oooo.."], Color.DarkSlateGray, Color.Orange, new Point(28, 28)),
            ClassicVisual.Octorok => new(["..oooo..", ".o####o.", "o#+o+##o", "o######o", "o####+++", ".o####o.", "o#o..o#o", "oo....oo"],
                ["..oooo..", ".o####o.", "o#+o+##o", "o######o", "o####+++", ".o####o.", ".o#oo#o.", "..oooo.."], Color.IndianRed, Color.MistyRose, new Point(32, 32)),
            ClassicVisual.Keese => new(["o......o", "oo....oo", "o#o..o#o", "o##oo##o", ".o+##+o.", "..o##o..", "...oo...", "........"],
                ["........", "........", "...oo...", "..o##o..", ".o+##+o.", "o##oo##o", "o#o..o#o", "oo....oo"], Color.MediumPurple, Color.LightCyan, new Point(38, 20)),
            ClassicVisual.Gel => new(["........", "..oooo..", ".o+###o.", "o++####o", "o##o#o#o", "o######o", ".o####o.", "..oooo.."],
                ["........", "........", "........", "..oooo..", ".o+###o.", "o##o#o#o", "o######o", "oooooooo"], Color.CornflowerBlue, Color.LightCyan, new Point(26, 24)),
            ClassicVisual.Keeper => new(["..oooo..", ".o####o.", ".o+oo+o.", "..o++o..", ".o####o.", "o##++#oo", ".o####o+", "..oooooo"],
                ["..oooo..", ".o####o.", ".o+oo+o.", "..o++o..", ".o####o.", "o##++#oo", ".o####ow", "..oooooo"], Color.DarkSlateBlue, Color.Wheat, new Point(30, 42)),
            _ => throw new ArgumentOutOfRangeException(nameof(visual))
        };
    }

    public void Draw(SpriteBatch batch, Vector2 position, Direction direction, bool alternateFrame)
    {
        string[] frame = alternateFrame && motif.Alternate is not null ? motif.Alternate : motif.Rest;
        for (int row = 0; row < frame.Length; row++)
        {
            for (int column = 0; column < frame[row].Length; column++)
            {
                char mark = frame[row][column];
                if (mark == '.') continue;
                Color color = mark switch
                {
                    'o' => Outline,
                    '+' => motif.Highlight,
                    'w' => Color.LightYellow,
                    _ => motif.Body
                };
                int visualColumn = direction == Direction.Left ? frame[row].Length - 1 - column : column;
                // Integer edges fill the original footprint without seams or overflow.
                int left = visualColumn * motif.Size.X / frame[row].Length;
                int right = (visualColumn + 1) * motif.Size.X / frame[row].Length;
                int top = row * motif.Size.Y / frame.Length;
                int bottom = (row + 1) * motif.Size.Y / frame.Length;
                batch.Draw(pixel, new Rectangle((int)position.X + left, (int)position.Y + top,
                    right - left, bottom - top), color);
            }
        }
    }
}
