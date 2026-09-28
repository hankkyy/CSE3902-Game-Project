using GameProject.Commands;
using GameProject.Controllers;
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

Console.WriteLine("All input tests passed.");

static void Check(string name, int expected, int actual)
{
    if (actual != expected)
    {
        throw new InvalidOperationException(
            $"FAIL: {name}. Expected {expected}, got {actual}.");
    }

    Console.WriteLine($"PASS: {name}");
}
