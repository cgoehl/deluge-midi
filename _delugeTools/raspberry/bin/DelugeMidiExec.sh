#!/bin/bash
set -euo pipefail

if [[ -f "$CONFIG_SD_PATH/_delugeTools/DelugeMidi.cfg" ]]; then
    set -a
    source "$CONFIG_SD_PATH/_delugeTools/DelugeMidi.cfg"
    set +a
fi

mono "$CONFIG_SD_PATH/_delugeTools/DelugeMidi.exe"

