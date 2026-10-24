using GameProject.Core;
using GameProject.Objects;
using GameProject.Rooms.Definitions;
using GameProject.Rooms.Loading;
using GameProject.Rooms.Navigation;
using GameProject.Sprites;
using GameProject.States;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

var definitions = RoomLoader.LoadDirectory(Path.Combine(AppContext.BaseDirectory, "Content", "Rooms"));
var navigation = new RoomManager(definitions, "entrance-hall");
var sprite = new TestSprite();
var player = new Player(navigation.EntryPosition.ToVector2(), navigation.CurrentRoom.Bounds.ToRectangle(),
    sprite, sprite, sprite, sprite, sprite);
var cache = new RoomCache(player, definition => new RoomObjectFactory(definition.Bounds.ToRectangle(), _ => sprite));
var session = new GameSession();
int quitCount = 0;
var game = new RoomGameplay(navigation, player, cache, session, () => quitCount++);
var time = new GameTime(TimeSpan.FromSeconds(0.1), TimeSpan.FromSeconds(0.1));
MouseState Mouse(bool left = false, bool right = false) => new(0, 0, 0,
    left ? ButtonState.Pressed : ButtonState.Released, ButtonState.Released,
    right ? ButtonState.Pressed : ButtonState.Released, ButtonState.Released, ButtonState.Released);
void Tick(MouseState mouse, params Keys[] keys) => game.Update(time, new KeyboardState(keys), mouse, true);
int passed = 0;
void Check(bool ok, string message) { if (!ok) throw new Exception(message); passed++; }

Check(game.CurrentRoom.Definition.Id == "entrance-hall", "Initial room");
Tick(Mouse());
Tick(Mouse(right: true));
Check(navigation.RoomNumber == 1, "Mouse disabled in menu");
Tick(Mouse(right: true), Keys.Enter);
Check(session.Mode == GameMode.Playing && navigation.RoomNumber == 1, "Menu click does not leak into gameplay");
Tick(Mouse(right: true));
Check(navigation.RoomNumber == 1, "Holding right does not navigate");
Tick(Mouse());
Tick(Mouse(right: true));
Check(navigation.RoomNumber == 2, "Fresh right navigates");
Check(player.Position == navigation.EntryPosition.ToVector2(), "Arrival position");
Tick(Mouse(right: true));
Check(navigation.RoomNumber == 2, "One change per click");
Tick(Mouse());
Tick(Mouse(left: true, right: true));
Check(navigation.RoomNumber == 2, "Simultaneous buttons ignored");
Tick(Mouse());
Tick(Mouse(left: true));
Check(navigation.RoomNumber == 1, "Fresh left navigates");
Tick(Mouse());
Vector2 before = player.Position;
Tick(Mouse(), Keys.D);
Check(Math.Abs(player.Position.X - before.X - 19) < 0.01f, "Movement updated exactly once");
Tick(Mouse());
player.EnterRoom(new Vector2(160, 190));
Tick(Mouse(), Keys.D);
Check(player.Hitbox.Right == 192, "Entrance Hall stone block stops the player");
Check(!player.Hitbox.Intersects(new Rectangle(192, 180, 52, 52)), "Player remains outside the stone block");
player.EnterRoom(new Vector2(410, 247));
Tick(Mouse(), Keys.D);
Check(Math.Abs(player.Position.X - 429) < 0.01f, "Rune floor tile remains walkable");
game.Reset(Mouse());
Tick(Mouse(), Keys.E);
Tick(Mouse(), Keys.D2);
int health = player.Health;
Check(health == 4 && player.SelectedItem == 2, "Existing input bindings");
Tick(Mouse(right: true));
Check(player.Health == health && player.SelectedItem == 2, "Health and selected item survive transition");
Check(!player.HasActiveArrow && !player.HasActiveBoomerang && !player.HasActiveBomb, "Transient items cleared on transition");
Tick(Mouse());
Tick(Mouse(right: true), Keys.R);
Check(navigation.RoomNumber == 1 && player.Health == 5 && player.SelectedItem == 1, "Reset wins over simultaneous room click");
Check(player.Position == navigation.EntryPosition.ToVector2(), "Reset entry");
Tick(Mouse(right: true), Keys.R);
Check(navigation.RoomNumber == 1, "Held reset and click do not repeat");
Tick(Mouse());
game.Update(time, new KeyboardState(Keys.D), Mouse(right: true), false);
Tick(Mouse(right: true), Keys.D);
Check(navigation.RoomNumber == 1, "Focus loss input suppressed");
Tick(Mouse(right: true));
Check(navigation.RoomNumber == 1, "Held mouse after focus restore suppressed");
Tick(Mouse());
Tick(Mouse(right: true));
Check(navigation.RoomNumber == 2, "Fresh click after focus restore");
Tick(Mouse());
Tick(Mouse(), Keys.Q);
Check(quitCount == 1, "Quit binding");
foreach (RoomDefinition definition in definitions)
{
    LoadedRoom room = cache.GetRoom(definition);
    Check(ReferenceEquals(room, cache.GetRoom(definition)), "Cached room reused");
    foreach (RoomPoint entry in definition.Entries.Values)
    {
        player.EnterRoom(entry.ToVector2());
        Check(player.Position == entry.ToVector2(), "Entry accepted");
    }
}
Vector2 saved = player.Position;
try { player.EnterRoom(new Vector2(float.NaN, 0)); throw new Exception("Invalid entry accepted"); }
catch (ArgumentOutOfRangeException) { Check(player.Position == saved, "Invalid entry leaves player unchanged"); }
game.Reset(Mouse());
Check(navigation.RoomNumber == 1 && player.Position == navigation.EntryPosition.ToVector2(), "Reset after visiting all rooms");
// Walk through every real exit using the actual player movement code.
foreach (RoomDefinition definition in definitions)
{
    foreach (RoomLink link in definition.Links)
    {
        var doorNavigation = new RoomManager(definitions, definition.Id);
        var doorPlayer = new Player(doorNavigation.EntryPosition.ToVector2(), definition.Bounds.ToRectangle(), sprite);
        var doorCache = new RoomCache(doorPlayer, room => new RoomObjectFactory(room.Bounds.ToRectangle(), _ => sprite));
        var doorSession = new GameSession();
        doorSession.Start();
        var doorGame = new RoomGameplay(doorNavigation, doorPlayer, doorCache, doorSession, () => { });
        Rectangle trigger = link.Trigger.ToRectangle();
        Vector2 approach;
        Keys move;
        if (link.Direction == "Right")
        {
            approach = new Vector2(trigger.Left - 29, trigger.Top + 10);
            move = Keys.D;
        }
        else if (link.Direction == "Left")
        {
            approach = new Vector2(trigger.Right + 1, trigger.Top + 10);
            move = Keys.A;
        }
        else if (link.Direction == "Up")
        {
            approach = new Vector2(trigger.Left + 10, trigger.Bottom + 1);
            move = Keys.W;
        }
        else
        {
            approach = new Vector2(trigger.Left + 10, trigger.Top - 35);
            move = Keys.S;
        }
        doorPlayer.EnterRoom(approach);
        doorGame.Update(time, new KeyboardState(move), Mouse(), true);
        Check(doorNavigation.CurrentRoom.Id == link.TargetRoomId, "Walk through " + definition.Id + "/" + link.Id);
        Check(doorPlayer.Position == doorNavigation.EntryPosition.ToVector2(), "Walk arrival entry");
        doorGame.Update(time, new KeyboardState(), Mouse(), true);
        Check(doorNavigation.CurrentRoom.Id == link.TargetRoomId, "No immediate bounce");
        Check(ReferenceEquals(doorGame.CurrentRoom, doorCache.GetRoom(doorNavigation.CurrentRoom)), "Door uses cache");
    }
}
var exitNavigation = new RoomManager(definitions, "entrance-hall");
var exitChecker = new RoomExitNavigator();
exitChecker.OnRoomEntered(exitNavigation.CurrentRoom, new Rectangle(64, 440, 28, 34));
exitChecker.CheckExits(exitNavigation, new Rectangle(852, 260, 28, 34), _ => true);
Check(!exitNavigation.ApplyPendingTransition(), "Touching edge without overlap does not enter");
exitChecker.CheckExits(exitNavigation, new Rectangle(880, 260, 28, 34), _ => false);
Check(!exitNavigation.ApplyPendingTransition(), "Closed exit callback blocks transition");
exitChecker.CheckExits(exitNavigation, new Rectangle(880, 260, 28, 34), _ => true);
Check(exitNavigation.CurrentRoom.Id == "entrance-hall", "Exit is deferred");
Check(exitNavigation.ApplyPendingTransition() && exitNavigation.CurrentRoom.Id == "quiet-water", "Valid exit queued");
exitChecker.OnRoomEntered(exitNavigation.CurrentRoom, new Rectangle(24, 260, 28, 34));
for (int frame = 0; frame < 3; frame++)
{
    exitChecker.CheckExits(exitNavigation, new Rectangle(24, 260, 28, 34), _ => true);
    Check(!exitNavigation.ApplyPendingTransition(), "Arrival inside exit cannot bounce");
}
exitChecker.CheckExits(exitNavigation, new Rectangle(64, 260, 28, 34), _ => true);
Check(!exitNavigation.ApplyPendingTransition(), "Leaving trigger rearms without transition");
exitChecker.CheckExits(exitNavigation, new Rectangle(24, 260, 28, 34), _ => true);
Check(exitNavigation.ApplyPendingTransition() && exitNavigation.CurrentRoom.Id == "entrance-hall", "Reenter after leaving works");
exitNavigation.RequestNextRoom();
exitChecker.OnRoomEntered(exitNavigation.CurrentRoom, new Rectangle(64, 440, 28, 34));
exitChecker.CheckExits(exitNavigation, new Rectangle(440, 72, 28, 34), _ => true);
Check(exitNavigation.ApplyPendingTransition() && exitNavigation.CurrentRoom.Id == "quiet-water", "First pending mouse request wins");
Console.WriteLine($"Passed {passed} room gameplay checks.");

sealed class TestSprite : ISprite
{
    public void Draw(SpriteBatch batch, Vector2 position, Direction direction, bool alternateFrame) { }
}
