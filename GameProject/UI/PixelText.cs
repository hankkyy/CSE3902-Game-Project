using GameProject.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameProject.UI;

/// <summary>Draws original five-by-seven UI lettering through the existing sprite factory.</summary>
public sealed class PixelText(SpriteFactory sprites)
{
    private const int GlyphWidth = 5;
    private const int GlyphHeight = 7;
    private const int CharacterSpacing = 1;

    private static readonly IReadOnlyDictionary<char, string> glyphs = new Dictionary<char, string>
    {
        ['A'] = "01110/10001/10001/11111/10001/10001/10001",
        ['B'] = "11110/10001/10001/11110/10001/10001/11110",
        ['C'] = "01111/10000/10000/10000/10000/10000/01111",
        ['D'] = "11110/10001/10001/10001/10001/10001/11110",
        ['E'] = "11111/10000/10000/11110/10000/10000/11111",
        ['F'] = "11111/10000/10000/11110/10000/10000/10000",
        ['G'] = "01111/10000/10000/10111/10001/10001/01110",
        ['H'] = "10001/10001/10001/11111/10001/10001/10001",
        ['I'] = "11111/00100/00100/00100/00100/00100/11111",
        ['J'] = "00111/00010/00010/00010/10010/10010/01100",
        ['K'] = "10001/10010/10100/11000/10100/10010/10001",
        ['L'] = "10000/10000/10000/10000/10000/10000/11111",
        ['M'] = "10001/11011/10101/10101/10001/10001/10001",
        ['N'] = "10001/11001/11001/10101/10011/10011/10001",
        ['O'] = "01110/10001/10001/10001/10001/10001/01110",
        ['P'] = "11110/10001/10001/11110/10000/10000/10000",
        ['Q'] = "01110/10001/10001/10001/10101/10010/01101",
        ['R'] = "11110/10001/10001/11110/10100/10010/10001",
        ['S'] = "01111/10000/10000/01110/00001/00001/11110",
        ['T'] = "11111/00100/00100/00100/00100/00100/00100",
        ['U'] = "10001/10001/10001/10001/10001/10001/01110",
        ['V'] = "10001/10001/10001/10001/10001/01010/00100",
        ['W'] = "10001/10001/10001/10101/10101/10101/01010",
        ['X'] = "10001/10001/01010/00100/01010/10001/10001",
        ['Y'] = "10001/10001/01010/00100/00100/00100/00100",
        ['Z'] = "11111/00001/00010/00100/01000/10000/11111",
        ['0'] = "01110/10001/10011/10101/11001/10001/01110",
        ['1'] = "00100/01100/00100/00100/00100/00100/01110",
        ['2'] = "01110/10001/00001/00010/00100/01000/11111",
        ['3'] = "11110/00001/00001/01110/00001/00001/11110",
        ['4'] = "00010/00110/01010/10010/11111/00010/00010",
        ['5'] = "11111/10000/10000/11110/00001/00001/11110",
        ['6'] = "01110/10000/10000/11110/10001/10001/01110",
        ['7'] = "11111/00001/00010/00100/01000/01000/01000",
        ['8'] = "01110/10001/10001/01110/10001/10001/01110",
        ['9'] = "01110/10001/10001/01111/00001/00001/01110",
        ['-'] = "00000/00000/00000/11111/00000/00000/00000",
        ['/'] = "00001/00001/00010/00100/01000/10000/10000",
        [':'] = "00000/00100/00100/00000/00100/00100/00000",
        ['.'] = "00000/00000/00000/00000/00000/00110/00110",
        ['?'] = "01110/10001/00001/00010/00100/00000/00100"
    };

    public static int MeasureWidth(string text, int scale) =>
        text.Length == 0 ? 0 : ((text.Length * (GlyphWidth + CharacterSpacing)) - CharacterSpacing) * scale;

    public void Draw(SpriteBatch batch, string text, Point position, int scale, Color color)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(scale, 1);
        int x = position.X;
        foreach (char character in text)
        {
            if (character != ' ')
            {
                string glyph = glyphs.TryGetValue(char.ToUpperInvariant(character), out string? found)
                    ? found : glyphs['?'];
                DrawGlyph(batch, glyph, new Point(x, position.Y), scale, color);
            }

            x += (GlyphWidth + CharacterSpacing) * scale;
        }
    }

    private void DrawGlyph(SpriteBatch batch, string glyph, Point position, int scale, Color color)
    {
        for (int row = 0; row < GlyphHeight; row++)
        {
            for (int column = 0; column < GlyphWidth; column++)
            {
                if (glyph[(row * (GlyphWidth + 1)) + column] == '1')
                {
                    sprites.DrawPanel(batch,
                        new Rectangle(position.X + (column * scale), position.Y + (row * scale), scale, scale),
                        color);
                }
            }
        }
    }
}
