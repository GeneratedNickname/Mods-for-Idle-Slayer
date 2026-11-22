### Version 2.2.0

- Master on/off switch (`Enabled`, and a toggle in the in-game mod settings)
- Armory multiply hotkey default changed from `C` to `V` (`C` is also Quality Of Life Mods' chest purchaser key); an existing config keeps its saved key

### Version 2.1.0

- New **Overlay** mode: no auto-opening, only labels each chest with its contents next to the chest icon
- Optional overlay while auto-opening, and an optional hotkey to cycle Perfect → Random → Overlay
- Refactored: Perfect and Random are separate strategies; waiting for the round and finishing it is shared
- Changing mode mid-round stops the running auto-opener instead of leaving it running
- Fixed: after opening a mimic for the "That was fun" achievement, the opener no longer stays stuck waiting for perfect chests (it used to stay alive into later rounds)
- Armory multiply hotkey now saves the setting
- Builds against `/Libraries` when no mod manager is installed

### Version 1.1.0

- Added config option to reveal duplicator item
- Internal renames to "Chest Revealer"

### Version 1.0.3

- Updated for new config format in core library

### Version 1.0.2

- Updated to reflect core mod dependency

### Version 1.0.1

- Added config option to reveal multipliers
- Better logic to prevent performance expensive checks

### Version 1.0.0

Initial release