using GameProject.Rooms.Definitions;
using Microsoft.Xna.Framework;

namespace GameProject.Rooms.Navigation;

/// <summary>Queues an open exit when the player overlaps its trigger.</summary>
public sealed class RoomExitNavigator
{
    private bool waitingUntilClear;

    public void CheckExits(RoomManager navigation, Rectangle playerHitbox,
        Func<RoomLink, bool> canUseExit)
    {
        ArgumentNullException.ThrowIfNull(navigation);
        ArgumentNullException.ThrowIfNull(canUseExit);
        RoomDefinition room = navigation.CurrentRoom;

        if (!TouchesAnyExit(room, playerHitbox))
        {
            waitingUntilClear = false;
            return;
        }

        // After arriving inside an exit, leave its trigger before using it again.
        if (waitingUntilClear)
        {
            return;
        }

        foreach (RoomLink link in room.Links)
        {
            if (playerHitbox.Intersects(link.Trigger.ToRectangle()) && canUseExit(link))
            {
                if (navigation.RequestExit(link.Id))
                {
                    waitingUntilClear = true;
                    return;
                }
            }
        }
    }

    public void OnRoomEntered(RoomDefinition room, Rectangle playerHitbox)
    {
        ArgumentNullException.ThrowIfNull(room);
        waitingUntilClear = TouchesAnyExit(room, playerHitbox);
    }

    private static bool TouchesAnyExit(RoomDefinition room, Rectangle playerHitbox)
    {
        foreach (RoomLink link in room.Links)
        {
            if (playerHitbox.Intersects(link.Trigger.ToRectangle()))
            {
                return true;
            }
        }

        return false;
    }
}
