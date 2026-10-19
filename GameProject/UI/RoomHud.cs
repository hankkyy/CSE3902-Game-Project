using GameProject.Objects;
using GameProject.Rooms.Navigation;
using GameProject.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameProject.UI;

/// <summary>Shows room information and the current player status.</summary>
internal sealed class RoomHud
{
    private readonly PixelText text;

    public RoomHud(SpriteFactory sprites)
    {
        ArgumentNullException.ThrowIfNull(sprites);
        text = new PixelText(sprites);
    }

    public void Draw(SpriteBatch batch, RoomManager navigation, Player player)
    {
        ArgumentNullException.ThrowIfNull(navigation);
        ArgumentNullException.ThrowIfNull(player);
        string itemName = "ARROW";
        if (player.SelectedItem == 2)
        {
            itemName = "BOOMERANG";
        }
        else if (player.SelectedItem == 3)
        {
            itemName = "BOMB";
        }

        text.Draw(batch, $"ROOM {navigation.RoomNumber}/{navigation.RoomCount}: {navigation.CurrentRoom.Name}",
            new Point(24, 12), 2, Color.White);
        text.Draw(batch, $"HEALTH {player.Health}   ITEM {player.SelectedItem}: {itemName}   STATE {player.Action}",
            new Point(24, 40), 1, Color.Gold);
        text.Draw(batch, "MOUSE: LEFT PREVIOUS ROOM / RIGHT NEXT ROOM",
            new Point(24, 514), 1, Color.White);
        text.Draw(batch, "WASD/ARROWS MOVE  Z/N SWORD  1/2/3 ITEMS  E HURT  R RESET  Q/ESC QUIT",
            new Point(24, 528), 1, Color.White);
    }
}
