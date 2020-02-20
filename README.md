# DelugeMidi

DISCLAIMER: This documentation is WIP!

This project injects MIDI-Mappings into Synthstrom Deluge song files. While a certain amount of flexiblity should be given (untested), it is intended to work with LaunchControl XL controllers. Each synthesizer instrument is mapped to a full page / preset of the LC XL. Individual presets per synthesizer type are supported. Each kit row is mapped to one column of the controller.

## Configuration

This [sheet](https://docs.google.com/spreadsheets/d/1HbQi0aSfgHbYNjTW637rvSPKAifUnQkZiXBxY-HuZeE/edit?usp=sharing) might be handy to create your customizations.
You will also find a list of all controllable parameters there, or at least the ones I found ;)

Export each one as CSV to the corresponding file in `_delugeTools/DelugeMidiLayouts`. Please keep in mind, that I might change my default layouts without notice.

## Setup

- Copy `_delugeTools` to the root of your Deluge SD-card.
- Build the project and put the resulting binaries somewhere below the `_delugeTools` directory.

## Execution

- Consider creating a backup of your SD-card, especially your songs. All existing mappings will be removed; no merge is implemented.
- Run `DelugeMidi.exe` or whatever is appropriate for your platform.

## Headless (WIP)

I am using a Raspberry PI as MIDI-hub anyways, so I plan to use it as a fast way to patch song files. 

Instructions to set-up MIDI-hub:
[https://neuma.studio/rpi-as-midi-host.html](https://neuma.studio/rpi-as-midi-host.html)


Once you have the hub running, follow steps below (find all files in `_delugeTools/raspberry`):
- Run `packages.sh` (required to run packaged net. core code. YMMV if you use Mono or already have net. core installed).
- Copy `40-deluge-midi.rules` into `/etc/udev/rules.d`.
- Copy `DelugeMidiJob.sh` and `DelugeMidiUdev.sh` into `/usr/local/bin`.
  - Verify path of linux binary in `DelugeMidiJob.sh`.
- Make sure all have `chmod +x` permissions for root.
- Reboot or reload udev.


# DelugeMidi

This project maps 

```
$ sudo -i
# cp /boot/setup.sh .
# chmod +x setup.sh
# ./setup.sh
```