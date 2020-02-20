#!/bin/bash
set -euo pipefail

# I have some user presets for Novation circuit, you might want to change this to zero.
# IMPORTANT: Channel numbers start at 0, as in Deluge's file format

# todo: source config from SD-Card

if [[ -f "$CONFIG_SD_PATH/_delugeTools/DelugeMidi.cfg" ]]; then
    set -a
    source "$CONFIG_SD_PATH/_delugeTools/DelugeMidi.cfg"
    set +a
fi

mono "$CONFIG_SD_PATH/_delugeTools/DelugeMidi.exe"

