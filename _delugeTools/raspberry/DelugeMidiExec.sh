#!/bin/bash
set -euo pipefail

# I have some user presets for Novation circuit, you might want to change this to zero.
# IMPORTANT: Channel numbers start at 0, as in Deluge's file format

# todo: source config from SD-Card

export CONFIG_FIRST_CHANNEL=5
mono "$CONIG_SD_PATH/_delugeTools/DelugeMidi.exe"

