using GameProject.Commands;
using GameProject.Controllers;
using GameProject.States;
using Microsoft.Xna.Framework.Input;

int nextCount = 0;
int resetCount = 0;

KeyPressDispatcher dispatcher = new(new Dictionary<Keys, ICommand>
{
    [Keys.Y] = new NextGalleryCommand(() => nextCount++),
    [Keys.R] = new ResetGameCommand(() => resetCount++)
});

dispatcher.Update(new KeyboardState());
Check("No key pressed", 0, nextCount);

dispatcher.Update(new KeyboardState(Keys.Y));
Check("First press", 1, nextCount);
Check("Unrelated command stays inactive", 0, resetCount);

for (int frame = 0; frame < 60; frame++)
{
    dispatcher.Update(new KeyboardState(Keys.Y));
}
Check("Holding does not repeat", 1, nextCount);

dispatcher.Update(new KeyboardState());
Check("Release does not trigger", 1, nextCount);
dispatcher.Update(new KeyboardState(Keys.Y));
Check("Press again after release", 2, nextCount);

dispatcher.Update(new KeyboardState(Keys.Y, Keys.LeftShift));
Check("Adding Shift does not repeat", 2, nextCount);
dispatcher.Update(new KeyboardState(Keys.LeftShift));
dispatcher.Update(new KeyboardState(Keys.LeftShift, Keys.Y));
Check("Shift plus Y works", 3, nextCount);

dispatcher.Update(new KeyboardState(Keys.Space));
Check("Unbound key is ignored", 3, nextCount);
Check("Unbound key does not reset", 0, resetCount);

dispatcher.Update(new KeyboardState(Keys.Y, Keys.R));
Check("Simultaneous press: Y", 4, nextCount);
Check("Simultaneous press: R", 1, resetCount);
dispatcher.Update(new KeyboardState(Keys.Y, Keys.R));
Check("Holding two keys: Y", 4, nextCount);
Check("Holding two keys: R", 1, resetCount);

GameSession session = new();
int menuNextCount = 0;
int menuResetCount = 0;
int quitCount = 0;
KeyPressDispatcher menuDispatcher = new(new Dictionary<Keys, ICommand>
{
    [Keys.Y] = new GameplayCommand(session, new NextGalleryCommand(() => menuNextCount++)),
    [Keys.R] = new GameplayCommand(session, new ResetGameCommand(() => menuResetCount++)),
    [Keys.Enter] = new StartGameCommand(session),
    [Keys.Q] = new QuitGameCommand(() => quitCount++)
});

Check("Starts in menu", GameMode.Menu, session.Mode);
menuDispatcher.Update(new KeyboardState(Keys.Q));
Check("Quit is available in menu", 1, quitCount);
menuDispatcher.Update(new KeyboardState(Keys.Y, Keys.R));
Check("Menu blocks gallery changes", 0, menuNextCount);
Check("Menu blocks reset", 0, menuResetCount);
menuDispatcher.Update(new KeyboardState(Keys.Y, Keys.R, Keys.Enter));
Check("Enter starts gameplay", GameMode.Playing, session.Mode);
menuDispatcher.Update(new KeyboardState(Keys.Y, Keys.R));
Check("Held gallery key does not leak into gameplay", 0, menuNextCount);
Check("Held reset does not leak into gameplay", 0, menuResetCount);
menuDispatcher.Update(new KeyboardState());
menuDispatcher.Update(new KeyboardState(Keys.Y, Keys.R));
Check("Gallery works after a fresh press", 1, menuNextCount);
Check("Reset works after a fresh press", 1, menuResetCount);
menuDispatcher.Update(new KeyboardState(Keys.Enter));
Check("Enter during gameplay keeps playing", GameMode.Playing, session.Mode);
menuDispatcher.Update(new KeyboardState(Keys.Q));
Check("Quit is available during gameplay", 2, quitCount);

Console.WriteLine("All input and menu tests passed.");

static void Check<T>(string name, T expected, T actual)
{
    if (!EqualityComparer<T>.Default.Equals(actual, expected))
    {
        throw new InvalidOperationException(
            $"FAIL: {name}. Expected {expected}, got {actual}.");
    }

    Console.WriteLine($"PASS: {name}");
}
