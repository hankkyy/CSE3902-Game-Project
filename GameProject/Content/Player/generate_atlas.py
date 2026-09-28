"""Original Moss Scout pixel art. Regenerate PNG and its embedded C# copy with Pillow.

No imported artwork or copyrighted game pixels are used. This asset is dedicated
to CC0-1.0; see docs/assets/PLAYER_SPRITES.md. The generator is an authoring tool,
not a game/runtime dependency.
"""

from pathlib import Path
import base64
import hashlib
import json
from PIL import Image, ImageDraw

CELL = 40
PALETTE = {
    '.': (0, 0, 0, 0),
    'o': '#182c35', 'g': '#377a5c', 'l': '#74b66b', 'd': '#255340',
    's': '#efc08b', 'h': '#a96b41', 'b': '#684c39', 'r': '#c15b55',
    'y': '#eccd77', 'w': '#e4eece', 't': '#829eaa', 'e': '#20323d'
}

# Fourteen by seventeen pixels; the red scarf and round buckler identify our scout.
FRONT = [
    '....oooooo....', '...ogllllgo...', '..ogllllllgo..', '..oggggggggo..',
    '...ohssssho...', '...oseeseso...', '....osssso....', '..ooorrrrooo..',
    '.osogllglosso.', '.osogllglotts.', '..oogllgottwo.', '...obyybottto.',
    '...ogddgoooo..', '...obbbbbbbo..', '...obb..bbbo..', '...obb..bbbo..',
    '..oooo..oooo..'
]
BACK = [
    '....oooooo....', '...ogllllgo...', '..ogllllllgo..', '..oggggggggo..',
    '...oggggggo...', '...ohhhhho....', '....ohhhho....', '..ooorrrrooo..',
    '.osogddglosso.', '.osogddglosso.', '..oogddglooo..', '...obyybbgo...',
    '...ogddgggo...', '...obbbbbbbo..', '...obb..bbbo..', '...obb..bbbo..',
    '..oooo..oooo..'
]
RIGHT = [
    '...ooooooo....', '..oglllllgo...', '..oglllllggo..', '...ogggggggo..',
    '...ohsssso....', '...ohsesso....', '....osssso....', '...oorrrroo...',
    '..oogglgosso..', '..otttogosso..', '..otwtogooo...', '..otttobyybo..',
    '...ooogddgo...', '....obbbbbbo..', '....obb.bbbo..', '....obb.bbbo..',
    '...oooo.oooo..'
]


def body(direction, animation, frame):
    rows = FRONT if direction == 0 else BACK if direction == 3 else RIGHT
    image = Image.new('RGBA', (14, 17))
    for y, row in enumerate(rows):
        assert len(row) == 14, (y, row, len(row))
        for x, key in enumerate(row):
            image.putpixel((x, y), Image.new('RGBA', (1, 1), PALETTE[key]).getpixel((0, 0)))
    if direction == 1:
        image = image.transpose(Image.Transpose.FLIP_LEFT_RIGHT)
    pen = ImageDraw.Draw(image)
    if animation == 0 and frame == 1:
        pen.rectangle((4, 7, 8, 7), fill=PALETTE['y'])
        pen.point((3, 8), fill=PALETTE['r'])
    if animation == 1:
        pen.rectangle((0, 13, 13, 16), fill=(0, 0, 0, 0))
        stride = [0, 1, 0, -1][frame]
        for x, offset in [(4, stride), (9, -stride)]:
            pen.rectangle((x, 13, x + 2, 15 + min(offset, 0)), fill=PALETTE['b'])
            pen.rectangle((x - 1, 16 + min(offset, 0), x + 2, 16 + min(offset, 0)), fill=PALETTE['o'])
        if frame == 2:
            pen.line((5, 8, 8, 8), fill=PALETTE['l'])
    if animation == 3:
        for y in range(image.height):
            for x in range(image.width):
                r, g, b, a = image.getpixel((x, y))
                if a and frame == 1:
                    image.putpixel((x, y), (min(255, r + 65), int(g * .70), int(b * .75), a))
        # A visible recoil expression instead of making the whole frame transparent.
        if direction != 3:
            pen.line((7, 5, 9, 6), fill=PALETTE['o'])
    return image


def make_frame(direction, animation, frame):
    image = Image.new('RGBA', (CELL, CELL))
    dx, dy = [(0, 1), (-1, 0), (1, 0), (0, -1)][direction]
    offset_x = -dx if animation == 3 else dx if animation == 2 and frame == 1 else 0
    offset_y = -dy if animation == 3 else dy if animation == 2 and frame == 1 else 0
    if animation == 1 and frame in (1, 3):
        offset_y -= 1
    image.alpha_composite(body(direction, animation, frame), (13 + offset_x, 11 + offset_y))
    pen = ImageDraw.Draw(image)
    if animation == 2:
        # Right-facing sword is built in its own transparent layer, then rotated.
        weapon = Image.new('RGBA', (CELL, CELL))
        blade = ImageDraw.Draw(weapon)
        reach = [4, 10, 6][frame]
        blade.rectangle((25, 20, 28 + reach, 22), fill=PALETTE['o'])
        blade.line((29, 20, 28 + reach, 20), fill=PALETTE['w'])
        blade.line((29, 21, 28 + reach, 21), fill=PALETTE['t'])
        blade.line((27, 18, 27, 24), fill=PALETTE['y'])
        blade.point((26, 21), fill=PALETTE['b'])
        rotations = {0: Image.Transpose.ROTATE_270, 1: Image.Transpose.ROTATE_180,
                     2: None, 3: Image.Transpose.ROTATE_90}
        if rotations[direction] is not None:
            weapon = weapon.transpose(rotations[direction])
        image.alpha_composite(weapon)
    if animation == 3:
        x, y = (10, 9) if frame == 0 else (29, 10)
        pen.line((x - 2, y, x + 2, y), fill=PALETTE['y'])
        pen.line((x, y - 2, x, y + 2), fill=PALETTE['y'])
    return image


def main():
    here = Path(__file__).resolve().parent
    atlas = Image.new('RGBA', (CELL * 4, CELL * 16))
    frame_counts = [2, 4, 3, 2]
    for animation, count in enumerate(frame_counts):
        for direction in range(4):
            for column in range(4):
                frame = make_frame(direction, animation, min(column, count - 1))
                atlas.alpha_composite(frame, (column * CELL, (animation * 4 + direction) * CELL))
    target = here / 'moss-scout.png'
    atlas.save(target, optimize=False)
    data = target.read_bytes()
    encoded = base64.b64encode(data).decode('ascii')
    chunks = [encoded[i:i + 120] for i in range(0, len(encoded), 120)]
    expression = ' +\n'.join('        "' + part + '"' for part in chunks) + ';'
    generated = ('// <auto-generated />\n// Original CC0 asset. Regenerate with Content/Player/generate_atlas.py.\n'
                 'namespace GameProject.Sprites;\n\ninternal static class PlayerSpriteAtlasData\n{\n'
                 '    internal const string PngBase64 =\n' + expression + '\n}\n')
    (here.parent.parent / 'Sprites' / 'PlayerSpriteAtlasData.g.cs').write_text(generated, encoding='utf-8', newline='\n')
    metadata = {'asset':target.name,'sha256':hashlib.sha256(data).hexdigest(),'size':[160,640],
                'cellSize':40,'bodyOrigin':[13,11],'scale':2,'directions':['Down','Left','Right','Up'],
                'animations':{'Idle':{'frames':2,'seconds':0.45,'loop':True},
                              'Walking':{'frames':4,'seconds':0.1,'loop':True},
                              'Attacking':{'frames':3,'seconds':0.09,'loop':False},
                              'Damaged':{'frames':2,'seconds':0.075,'loop':True}}}
    (here/'moss-scout.json').write_text(json.dumps(metadata,indent=2)+'\n',encoding='utf-8',newline='\n')
    print(f'Wrote {target.name}: {len(data)} bytes, SHA-256 {metadata["sha256"]}')


if __name__ == '__main__':
    main()
