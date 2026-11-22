# Mods-for-Idle-Slayer
Mods for Idle Slayer

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
| Hawk Tuah | renames the upgrade back to "Hawk Eye" |
| Armory Manager | restores the slot colours; stops the 100% armory chest chances |
| Auto Rage Mode | keeps the selected mode for when it is switched back on |
| No Sync Game Load | takes effect from the next start (the mod only acts on the title screen) |

Not undone: Armory Manager's level-requirement change on weapons that already exist, and Enhanced Quests'
quest reroll / portal cooldown changes made while it was on.
