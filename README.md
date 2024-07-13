# Stationeers-Creative-Freedom
Break the limits of creativity!
My first attempt for hardmodding the Unity game.

- skip collision checks for structures, so you can overlap things and place cables under devices and circuits.
- can cross cables/pipes without merging, so you can overlap networks.
- unlocked rotation limits (disabled by default, as for now it have problem with smart rotation by *C*)
- raised jetpack effective height (configurable)
- changeable mine drilling speed (in config file)
- can label door sides separately (using particular string in name ("@@" by default), configurable)

Creative Only:
- spawn menu set to constant scale mode (to fix tiny size on wide screen).
- spawn completed structures (last build stage)
- endless jetpack without fumes (no need propellant)
- speedup jetpack by Shift (like in FuelJetpack mod)
- endless terrain manipulator, which can place dirt anywhere
- clean built-in nightvision by *N* for any race
- bigger range of zoom by FieldOfView keys, can look close at celestal bodies and far structures.
- can rename any thing by Labeller (walls, wires and characters too)


Config for modified limits and switchers will be here (after game load once with the mod):

**Stationeers\BepInEx\config\net.kastuk.stationeers.CreativeFreedom.cfg**

Installation
=============
1. Download last stable (I hope) release of compiled mod:
https://github.com/Kastuk/Stationeers-Creative-Freedom/releases
2. Download last stable BepInEx release:
https://github.com/BepInEx/BepInEx/releases
Supposedly x64 for your Windows or unix for Linux.
3. Unpack it into SteamLibrary\steamapps\common\Stationeers folder
4. Run Stationeers once, so BepInEx will be installed.
5. Place dll file of the mod into Stationeers\BepInEx\plugins

You can also use Steam Workshop and StationeersMods plugin:
https://steamcommunity.com/sharedfiles/filedetails/?id=3193624149

What can bring errors and problems: 
---
Try not to place big frame structure into another big frame. 
Using of smart rotation at unlocked angles will bring red text too.

TODO
---
*Interface window menu or console commands for ingame config.
*Changeable color of spawning structures and items.

*Switcher key to change placement type of mountable things to small grid (to place buttons on tables and stuff).

*Lighted up the game by disabling some features (Main Menu scene and Stationpedia) to reduce memory load at slow pc.

More things for breaking free...
