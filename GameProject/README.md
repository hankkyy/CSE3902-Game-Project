# Sprint 1 MonoGame starting point

This minimal DesktopGL project opens a cornflower-blue window. Press Escape (or a controller's Back button) to exit. It contains no Sprint 0 implementation or game assets.

Requires the .NET 8 SDK and a graphics environment supported by MonoGame DesktopGL.

From the repository root:

```sh
dotnet restore GameProject/GameProject.csproj
dotnet build GameProject/GameProject.csproj
dotnet run --project GameProject/GameProject.csproj
```

Visual Studio users can open `GameProject.csproj` directly.

Sprint 1 contribution: Xuanzhe Li (@JojoLi132), associated with issue #4. The small comment in `Game1.Draw` documents the frame-clear step for the Git practice exercise.
