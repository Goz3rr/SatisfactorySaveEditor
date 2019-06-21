# Satisfactory Save Editor

**View installation instructions in the [guide](https://ficsit.app/guide/Z8h6z2CczH43c)**!

**See the [releases page](https://github.com/Goz3rr/SatisfactorySaveEditor/releases) to download a compiled release**, or build your own by cloning this repo.
Stuck? Bugs? Questions? Feel free to contact us here on Github or on the [Satisfactory Modding](https://discord.gg/rNxYXht) discord server.

A work in progress save editor for Satisfactory. Consists of both a save parser and an application for viewing and editing the parsed data.

Features include:
* View all save data in a more human readable format
* Manually edit data tags
* Enable the minimap early
* Change your inventory size
* Unlock all milestones (to make a testing map, for example)
* Delete things from your save
* Search your save for certain information
* Determine the location of resource nodes
* Edit the contents of containers and your inventory
* Edit your player position to teleport yourself
* More coming soon

Written in C# using WPF for the user interface.

Made in Visual Studio 2017. Requires .NET Framework 4.7.2 Dev Pack and .NET Core 2.2 SDK. 

# Help

Check out the [guide](https://ficsit.app/guide/Z8h6z2CczH43c) on ficsit.app for some installation instructions and basic usage information.

Mousing over items in the left pane tree view will provide a short description of their purpose in the save. Please help us keep this list up to date by updating [`Types.xml`](https://github.com/Goz3rr/SatisfactorySaveEditor/blob/master/SatisfactorySaveEditor/Types.xml) with the tags you discover in your save editing adventures.

If you have any questions, please feel free to contact us here on Github or on Discord. 

On Discord, you can request help in the `#savegame-edits` channel of the [Satisfactory Modding](https://discord.gg/rNxYXht) discord server.

# FAQ

* None of the changes I'm making are doing anything in game.

Are you sure you're using `File > Save` to save after making your changes?

* I'm trying to use the `Unlock all research` cheat, but it isn't working!

Some people have encountered issues if they don't complete HUB tier 0 before running this cheat. Try that, and contact us if the issue continues.

* Placing some things I unlocked using the `Unlock all research` cheat crashes my game!

A few updates ago, something in the game was changed to cause crashes when some, but not all, unreleased buildings are placed. We aren't sure if this was intentionally done by Coffee Stain or if it happened on accident, but we can't do anything about it.

