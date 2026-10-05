"""Asset integrity checks. Optional authoring dependency: Pillow."""
from pathlib import Path
from PIL import Image
import base64
import hashlib
import json
import re

root = Path(__file__).resolve().parents[2]
content = root / 'GameProject' / 'Content' / 'Player'
data = (content / 'moss-scout.png').read_bytes()
metadata = json.loads((content / 'moss-scout.json').read_text())
source = (root / 'GameProject' / 'Sprites' / 'PlayerSpriteAtlasData.g.cs').read_text()
encoded = ''.join(re.findall(r'"([A-Za-z0-9+/=]+)"', source))
assert base64.b64decode(encoded) == data, 'Embedded runtime image differs from content image'
assert hashlib.sha256(data).hexdigest() == metadata['sha256'], 'Stale asset metadata'
atlas = Image.open(content / 'moss-scout.png').convert('RGBA')
assert atlas.size == (160, 640)
checked = 0
for animation, config in enumerate(metadata['animations'].values()):
    for direction in range(4):
        hashes = set()
        for frame in range(config['frames']):
            x, y = frame * 40, (animation * 4 + direction) * 40
            cell = atlas.crop((x, y, x + 40, y + 40))
            bounds = cell.getbbox()
            assert bounds is not None, 'Empty frame'
            assert bounds[0] > 0 and bounds[1] > 0 and bounds[2] < 40 and bounds[3] < 40, 'Art reaches atlas cell edge'
            assert set(cell.getchannel('A').tobytes()) == {0, 255}, 'Unexpected partial alpha'
            hashes.add(hashlib.sha256(cell.tobytes()).hexdigest())
            checked += 1
        assert len(hashes) == config['frames'], 'Clip contains duplicate active frames'
print(f'PASS: {checked} nonempty unique clip frames, transparent gutters, embedded PNG identity and SHA-256.')
