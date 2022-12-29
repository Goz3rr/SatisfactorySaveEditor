

# ![SSE Logo](https://i.imgur.com/YgnPVSo.png) Satisfactory Save Editor 

## Update 6/7 is not yet supported! See [this issue](https://github.com/Goz3rr/SatisfactorySaveEditor/issues/267) for details. In the mean time, you can use [SCIM](https://satisfactory-calculator.com/en/interactive-map) or another editor of your choice.

**View installation instructions in the [guide](https://ficsit.app/guide/Z8h6z2CczH43c)**!

**See the [releases page](https://github.com/Goz3rr/SatisfactorySaveEditor/releases) to download a compiled release**, or build your own by cloning this repo.
Stuck? Bugs? Questions? Feel free to contact us here on Github or on the [Satisfactory Modding](https://bit.ly/SatisfactoryModding) discord server.

A save editor for Satisfactory, a game by Coffee Stain Studios. Consists of both a save parser and an application for viewing and editing the parsed data.

Features include:
* View all save data in a more human readable format
* Dismantle portions of your factory instantly, putting all materials in one crate for easy pickup
* Automatically create backups before saving in case you break your save
* Delete broken content or fix duplicated milestones caused by mod bugs.
* Spawn lizard doggos!
* Remove ghost players
* Edit the contents of containers and your inventory
* (Should) support any modded content serialized in the game save
* Manually edit data tags
* Enable the minimap early
* Unlock all milestones (to make a testing map, for example)
* Search your save for certain information
* Determine the location of resource nodes
* Edit your player position to teleport yourself
* More coming soon!

Written in C# using WPF for the user interface.

Made in Visual Studio 2017. Requires .NET Framework 4.7.2 Dev Pack and .NET Core 2.2 SDK. 

Screenshot of the interface:
![Interface Screenshot](https://i.imgur.com/iPO9Gp1.png)

# Help

Check out the [guide](https://ficsit.app/guide/Z8h6z2CczH43c) on ficsit.app for some installation instructions and basic usage information.

Mousing over items in the left pane tree view will provide a short description of their purpose in the save. Please help us keep this list up to date by updating [`Types.xml`](https://github.com/Goz3rr/SatisfactorySaveEditor/blob/master/SatisfactorySaveEditor/Data/Types.xml) with the tags you discover in your save editing adventures.

If you have any questions, please feel free to contact us here on Github or on Discord. 

On Discord, you can request help in the `#savegame-edits` channel of the [Satisfactory Modding](https://bit.ly/SatisfactoryModding) discord server.

# FAQ

* None of the changes I'm making are doing anything in game.

Are you sure you're using `File > Save` to save after making your changes?

* I'm trying to use the `Unlock all research` cheat, but it isn't working!

Some people have encountered issues if they don't complete HUB tier 0 before running this cheat. Try that, and contact us if the issue continues.

* The editor crashed!

Please create a [github issue](https://github.com/Goz3rr/SatisfactorySaveEditor/issues/new) with the details of what you were doing before the editor crashed, and include the save file you were editing if you'd like. Please contact us on Discord (above) as well.
