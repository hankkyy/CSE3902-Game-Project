using System.Diagnostics.CodeAnalysis;

// These four types are used directly by the separate RoomNavigation test project.
// Suppress only CA1515 for the named types; other analyzer warnings stay enabled.
[assembly: SuppressMessage("Maintainability", "CA1515:Consider making public types internal",
    Justification = "RoomNavigation.Tests constructs RoomManager and verifies queued transitions and entry positions.",
    Scope = "type", Target = "~T:GameProject.Rooms.Navigation.RoomManager")]

[assembly: SuppressMessage("Maintainability", "CA1515:Consider making public types internal",
    Justification = "RoomNavigation.Tests constructs RoomCache and verifies reuse of room instances with an injected player and factory.",
    Scope = "type", Target = "~T:GameProject.Rooms.Navigation.RoomCache")]

[assembly: SuppressMessage("Maintainability", "CA1515:Consider making public types internal",
    Justification = "RoomNavigation.Tests constructs RoomGameplay and supplies input snapshots to test integrated input, navigation, and reset.",
    Scope = "type", Target = "~T:GameProject.Rooms.Navigation.RoomGameplay")]

[assembly: SuppressMessage("Maintainability", "CA1515:Consider making public types internal",
    Justification = "RoomNavigation.Tests constructs RoomExitNavigator to verify blocked exits, deferred transitions, and arrival protection.",
    Scope = "type", Target = "~T:GameProject.Rooms.Navigation.RoomExitNavigator")]
