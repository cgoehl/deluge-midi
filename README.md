# DelugeMidi

This project creates MIDI-Mappings for [Synthstrom Deluge](https://synthstrom.com/product/deluge/) to be controlled by [LaunchControl XL](https://novationmusic.com/en/launch/launch-control-xl). Support for other controllers exists, but is undocumented and untested.

Features:
- Update Deluge Songs on SD-Card to include MIDI mappings for:
  - Synthesizers: 
    - Map a complete preset / page / channel of knobs depending on synthesizer type (subtractive, ring and FM)
    - Map buttons as notes
  - Kits: 
    - Map each sample to a column of the controller. Can span multiple pages.
    - Map buttons as audition
  - Layouts are customizable per type
- Headless mode on Raspberry PI
  - Add MIDI-Mappings in few seconds. Just plug the card, no further interaction required. (Needs card-reader indicating access)
  - Setup script, also setting up a USB MIDI-hub


## Basic setup
- Download source-code and executeable.
- Copy `_delugeTools` folder from source code to the root of your Deluge SD-card.
- Copy `DelugeMidi.exe` to the `_delugeTools` folder of the SD-card.
- Run `DelugeMidi.exe` to update mappings.
  - Existing mappings will be overwritten.

## Headless setup
- Flash a fresh raspbian buster minimal to a SD-card.
- Copy all files in `_delugeTools/raspberry/boot` to the boot volume.
- Copy all files in `_delugeTools/raspberry/setup.sh` to the boot volume.
- Update `wpa_supplicant.conf` according to your WiFi.
- Boot the card.
- Login via ssh or physically. User/Password: pi/raspberry
- Then execute the commands below:
```
$ sudo -i
# cp /boot/setup.sh .
# chmod +x setup.sh
# ./setup.sh
```

## Configuration

### Layout
This [sheet](https://docs.google.com/spreadsheets/d/1HbQi0aSfgHbYNjTW637rvSPKAifUnQkZiXBxY-HuZeE/edit?usp=sharing) might be handy to create your customizations.
You will also find a list of all controllable parameters there, or at least the ones I found ;)

Export each one as CSV to the corresponding file in `_delugeTools/DelugeMidiLayouts`. Please keep in mind that I might change my default layouts without notice.