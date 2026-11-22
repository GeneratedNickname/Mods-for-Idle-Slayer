Handles the Chest Hunt for you, or just shows you what is inside every chest.

## Modes

| Mode | What it does |
| --- | --- |
| **Perfect** (default) | Opens every non-mimic chest in the order that gives the highest multiplier (multiplier increasers, duplicators, optionally armory chests, best multiplier, rest), then picks the best perfect chest (permanent upgrade first, otherwise highest multiplier). While the "That was fun" achievement is locked, it opens a mimic instead to unlock it. |
| **Random** | Opens chests in random order, mimics included. If the round still ends perfect, it picks the best perfect chest. |
| **Overlay** | Opens nothing. Writes the contents next to each chest icon (`x25`, `MIMIC`, `Dupe`, `Saver`, ...) and marks the best multiplier / best perfect chest with **(best)**. You play the round yourself. |

Switching mode during a round takes effect immediately: the running auto-opener stops, and the new mode carries on with the chests that are left.

## Settings

In-game (mod settings):

- **Perfect Chest Hunter**: master switch, turns the whole mod on or off
- **Auto-open Chests (off: Overlay only)**: off = Overlay mode
- **Open Chests Randomly/Perfectly**: Perfect or Random when auto-opening
- **Chest Overlay while Auto-opening**: also show the labels in Perfect / Random mode

In the config file (`[Perfect Chest Hunter]`):

| Key | Default | |
| --- | --- | --- |
| `Enabled` | `true` | Master switch for the whole mod |
| `AutoOpenChests` | `true` | `false` = Overlay mode |
| `PerfectChestsHuntOn` | `true` | Perfect (`true`) or Random (`false`) when auto-opening |
| `ModeCycleKey` | `None` | Key that cycles Perfect → Random → Overlay |
| `MultiplyArmoryChests` | `false` | Perfect mode: open armory chests right after the duplicators |
| `ArmoryChestMultiplyToggleKey` | `C` | Toggles the setting above |
| `ShowOverlayWhileAutoOpening` | `false` | Overlay labels in Perfect / Random mode too |
| `OverlayFontSize` | `0` | Label font size, `0` = same as the game's chest text |
| `OverlayOffsetX` / `OverlayOffsetY` | `4` / `0` | Label position relative to the right edge of the chest icon |

## Install

Plugin goes into **Path/To/Idle Slayer/Mods**

Requires Melon Loader and IdleSlayer.Common to function

## Code layout

- `ChestHuntController`: follows the chest hunt lifecycle and starts the current mode's strategy and the overlay
- `ChestHunt`: wrapper around the game's `ChestHuntManager` (state queries, open a chest, finish the round)
- `ChestPicks`: which chest is the best pick (shared by Perfect mode and the overlay)
- `Strategies/`: `PerfectStrategy`, `RandomStrategy`, one per auto-opening mode
- `Overlay/`: `ChestOverlay` (creates the labels), `ChestLabel` (text and colour per chest type)

## Building

The project references the assemblies of a local Idle Slayer Mod Manager install (`%LOCALAPPDATA%\IdleSlayerModManager`). If none is installed it falls back to the snapshot in `/Libraries`, which can be older than the current game.
