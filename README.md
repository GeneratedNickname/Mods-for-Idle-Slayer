# Mods-for-Idle-Slayer
Mods for Idle Slayer

## Install

1. **MelonLoader 0.7** must be installed into Idle Slayer. The Idle Slayer Mod Manager does this for you;
   otherwise use the MelonLoader installer and point it at `Idle Slayer.exe`.
2. **IdleSlayerMods.Common** must be in the game's `Mods` folder; every mod here depends on it.
   The mod manager ships it.
3. Download the `.dll` of each mod you want from the [Releases](../../releases) page and put it into
   `<Idle Slayer folder>\Mods` (Steam: right-click Idle Slayer → Manage → Browse local files).
4. Start the game. The MelonLoader console lists every mod it loaded.

Don't install both mods of an overlapping pair (see [Overlapping mods](#overlapping-mods)).

## Use

- Every mod is on as soon as it is installed. Switch mods on or off in the game's **mod settings**:
  each mod has a toggle named after it there. Some mods have more toggles in the same place.
- Hotkeys are listed under [Hotkeys](#hotkeys); most mods show a short popup when you press one.
- Every setting, keys included, is in `<Idle Slayer folder>\UserData\<ModName>.cfg`. The file is created on
  first start; edit it while the game is closed.
- What each mod does: see the README in the mod's folder where there is one (e.g.
  [Perfect Chest Hunter](PerfectChestHunter/README.md)).

To remove a mod, delete its `.dll` from `Mods`.

## Switching mods on and off

Every mod has a master switch. It shows up in the in-game mod settings as a toggle named after the mod,
and is stored as `Enabled` in the mod's config file. It is on by default, so installing a mod behaves as before.
When switched off the mod stops acting immediately (no hotkeys, no automation, its game hooks do nothing).

Where a mod changed game state, switching it off puts that back where it can:

| Mod | Switching off also |
| --- | --- |
| Perfect Chest Hunter | stops the running auto-opener and removes the overlay |
| Auto Jump | uses its existing `UseAutoJump` setting as the switch; gives back the climbing boots skill if it was locked |
| Better Upgrades | re-enables the upgrades it disabled (they have to be bought again) |
| No Special Boxes Mode | restores the game's special box chance; stops skipping the bonus slider |
| Armory Manager | restores the slot colours and the weapons' level requirement per level; stops the 100% armory chest chances |
| Auto Rage Mode | keeps the selected mode for when it is switched back on |
| No Sync Game Load | takes effect from the next start (the mod only acts on the title screen) |

Not undone: Enhanced Quests' quest reroll / portal cooldown changes made while it was on.

## Hotkeys

Default keys, chosen so that no two mods share one. A config file that already exists keeps the keys saved in it.

| Key | Mod |
| --- | --- |
| `Z` | Auto Jump on/off |
| `Q` | Auto Ascending Heights on/off |
| `T` | Auto Rage Mode: cycle mode |
| `J` | Bonus Stage Completer: skip at spirit boost |
| `C` / `M` | Quality Of Life Mods: chest purchaser / minion sender |
| `V` | Perfect Chest Hunter: multiply armory chests (was `C`) |
| `-` / `=` | Armory Manager: weapons to keep |
| `F1` | Bonus Stage Quest Completer: complete the goblin quest |
| `0` / `9` | Enhanced Quests Local: reroll quests / daily reroll reward |
| `1` | Auto Ascend: re-check the quest list |

Experimental Mods' keys (formerly `Z`, `X`, `Q`, `W`) are unbound by default.

## Overlapping mods

Some mods are variants of each other. Install only one of each pair:

- **Enhanced Quests** / **Enhanced Quests Local**
- **Minion Sender** / **Quality Of Life Mods** (which contains its own minion sender)
