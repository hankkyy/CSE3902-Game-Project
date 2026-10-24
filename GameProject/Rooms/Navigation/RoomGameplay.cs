using GameProject.Collision;
using GameProject.Controllers;
using GameProject.Objects;
using GameProject.Rooms.Definitions;
using GameProject.Rooms.Loading;
using GameProject.Sprites;
using GameProject.States;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace GameProject.Rooms.Navigation;

/// <summary>Connects room navigation, cached objects, and player input.</summary>
public sealed class RoomGameplay
{
    private readonly RoomCache rooms;
    private readonly GameSession session;
    private readonly KeyboardController keyboard;
    private readonly MouseController mouse;
    private readonly RoomExitNavigator exits = new();
    private readonly RoomSolidCollisionSystem solidCollisions = new();
    private readonly Func<RoomLink, bool> canUseExit;
    private bool resetRequested;
    private bool waitingForKeyboardRelease;

    public RoomGameplay(RoomManager navigation, Player player, RoomCache rooms,
        GameSession session, Action quit, Func<RoomLink, bool>? canUseExit = null)
    {
        ArgumentNullException.ThrowIfNull(navigation);
        ArgumentNullException.ThrowIfNull(player);
        ArgumentNullException.ThrowIfNull(rooms);
        ArgumentNullException.ThrowIfNull(session);
        ArgumentNullException.ThrowIfNull(quit);
        Navigation = navigation;
        Player = player;
        this.rooms = rooms;
        this.session = session;
        this.canUseExit = canUseExit ?? AllowNormalExit;
        mouse = new MouseController(navigation.RequestPreviousRoom, navigation.RequestNextRoom);
        keyboard = new KeyboardController(player,
            IgnoreGalleryKey, IgnoreGalleryKey,
            IgnoreGalleryKey, IgnoreGalleryKey,
            IgnoreGalleryKey, IgnoreGalleryKey,
            RequestReset, quit, session);
        rooms.GetRoom(navigation.CurrentRoom);
        player.EnterRoom(navigation.EntryPosition.ToVector2());
        exits.OnRoomEntered(navigation.CurrentRoom, player.Hitbox);
    }

    public RoomManager Navigation { get; }
    public Player Player { get; }
    public LoadedRoom CurrentRoom => rooms.GetRoom(Navigation.CurrentRoom);

    public static RoomGameplay Create(SpriteFactory sprites, GameSession session, Action quit)
    {
        ArgumentNullException.ThrowIfNull(sprites);
        string path = Path.Combine(AppContext.BaseDirectory, "Content", "Rooms");
        IReadOnlyList<RoomDefinition> definitions = RoomLoader.LoadDirectory(path);
        var navigation = new RoomManager(definitions, "entrance-hall");
        Rectangle bounds = navigation.CurrentRoom.Bounds.ToRectangle();

        // The current player and its projectiles keep one fixed movement boundary.
        foreach (RoomDefinition definition in definitions)
        {
            if (definition.Bounds.ToRectangle() != bounds)
            {
                throw new InvalidDataException("Room bounds must match until variable room bounds are supported.");
            }
        }

        var player = new Player(navigation.EntryPosition.ToVector2(), bounds, sprites);
        var rooms = new RoomCache(player, definition =>
            new RoomObjectFactory(definition.Bounds.ToRectangle(), sprites));
        return new RoomGameplay(navigation, player, rooms, session, quit);
    }

    public void Update(GameTime gameTime, bool windowActive)
    {
        Update(gameTime, Keyboard.GetState(), Mouse.GetState(), windowActive);
    }

    public void Update(GameTime gameTime, KeyboardState keys, MouseState mouseState, bool windowActive)
    {
        ArgumentNullException.ThrowIfNull(gameTime);
        if (!windowActive)
        {
            mouse.Update(mouseState, false);
            Player.SetMovement(Vector2.Zero);
            waitingForKeyboardRelease = true;
            return;
        }

        // After returning to the window, release old keys before accepting new input.
        if (waitingForKeyboardRelease)
        {
            if (keys.GetPressedKeys().Length > 0)
            {
                mouse.Update(mouseState, false);
                return;
            }

            waitingForKeyboardRelease = false;
        }

        bool wasPlaying = session.Mode == GameMode.Playing;
        keyboard.Update(keys);
        bool playing = session.Mode == GameMode.Playing;
        mouse.Update(mouseState, wasPlaying && playing && !resetRequested);

        if (resetRequested)
        {
            Reset(mouseState);
            return;
        }

        if (!playing)
        {
            return;
        }

        CurrentRoom.Update(gameTime);
        solidCollisions.Process(Player, Navigation.CurrentRoom);
        exits.CheckExits(Navigation, Player.Hitbox, canUseExit);
        if (Navigation.ApplyPendingTransition())
        {
            rooms.GetRoom(Navigation.CurrentRoom);
            Player.EnterRoom(Navigation.EntryPosition.ToVector2());
            exits.OnRoomEntered(Navigation.CurrentRoom, Player.Hitbox);
        }
    }

    public void Reset(MouseState mouseState)
    {
        rooms.Reset();
        Navigation.ResetNavigation();
        rooms.GetRoom(Navigation.CurrentRoom);
        Player.EnterRoom(Navigation.EntryPosition.ToVector2());
        exits.OnRoomEntered(Navigation.CurrentRoom, Player.Hitbox);
        mouse.Reset(mouseState);
        resetRequested = false;
    }

    private static bool AllowNormalExit(RoomLink link)
    {
        // These six room files currently describe ordinary open exits.
        // Supply canUseExit when integrating locked doors or combat restrictions.
        return true;
    }

    private void RequestReset()
    {
        resetRequested = true;
    }

    private static void IgnoreGalleryKey()
    {
        // Full rooms replace the Sprint 2 gallery selection controls.
    }
}
